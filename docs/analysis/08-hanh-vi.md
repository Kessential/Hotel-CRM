# 08. Sơ đồ hành vi

Module M7 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.6). Định mức N2: ≥ 4 sơ đồ, ≥ 2 mỗi người, ≥ 1 xuyên phân hệ; nhóm có 4 sơ đồ, 2 sơ đồ xuyên phân hệ (L2, L3). Tên lớp/hàm khớp `07-thiet-ke-lop.md`.

## 1. Tuần tự L2 — Đóng ticket (xuyên phân hệ) [TV1]
_[TV1] chưa điền._

## 2. Máy trạng thái ticket [TV1]
_[TV1] chưa điền._

## 3. Tuần tự L3 — Trả phòng (xuyên phân hệ) [TV2]
Sơ đồ: `diagrams/seq-l3-checkout.puml`. Đặc tả: UC08 luồng C (`03-use-case.md` mục 5.3). Quy tắc: Q6, Q8, Q9.

### 3.1 Các bước

| # | Đối tượng | Việc | Quy tắc / ghi chú |
|---|---|---|---|
| 1 | `frm203_nv_stay.OnPostCheckOut` → `ErrorHandler.Run` | handler của nút Trả phòng bọc trong `ErrorHandler.Run` | trang không có quy tắc, không `try/catch` |
| 2 | `StayService.CheckOut(actor, bookingId)` | kiểm quyền: `actor.EnsureCan…()` (tên hàm do TV1 chốt) | Q6; ẩn nút chỉ là lớp chặn thứ hai |
| 3 | `IUnitOfWork.Execute` | mở một connection + một giao dịch | mọi bước 4–10 nằm trong giao dịch này |
| 4 | `Booking.CheckOut(now)` | Đang ở → Đã trả phòng, ghi `CheckedOutAt` | Q8; sai trạng thái → `BusinessRuleException` |
| 5 | `MembershipTier.PointsFor(TotalAmount)` | điểm theo hạng **trước** khi trả phòng | Q9; `TotalAmount` = số đêm × `RatePerNight` đã chốt |
| 6 | `PointTransaction.ForStay` | ghi dòng sổ điểm Lưu trú gắn booking | bỏ qua nếu 0 điểm; unique index chặn cộng hai lần |
| 7 | `Customer.EarnPoints(points)` | cộng điểm đã tích, nâng hạng (có thể nhiều bậc) | Q9; hạng không bao giờ hạ |
| 8 | `Interaction` | ghi tương tác "Trả phòng, +N điểm" | |
| 9 | `ISurveyScheduler.ScheduleFor` (**TV1**) | tạo phiếu khảo sát "Chờ" cho booking | xuyên phân hệ; tham gia cùng giao dịch |
| 10 | `IUnitOfWork` | **COMMIT** nếu mọi bước thành công; **ROLLBACK** và ném lại nếu một bước lỗi | khung `alt` trên sơ đồ |
| 11 | `StayService` → trang | trả `CheckOutResult(points, tierName, upgraded)`; trang báo điểm và hạng mới bằng toast | |

### 3.2 Vì sao một giao dịch
Nếu bước 9 (phân hệ TV1) lỗi mà không có rollback:
- booking đã Đã trả phòng và điểm đã cộng;
- nhưng không có phiếu khảo sát; lễ tân bấm lại thì bị từ chối vì booking không còn Đang ở;
- lần ở đó không bao giờ được khảo sát, và số liệu R5 lệch.

Rollback đưa mọi thứ về như trước khi bấm, nên lễ tân chỉ việc thử lại.

Hai lớp chặn trùng ngoài giao dịch:
- unique index `ux_points_stay`: mỗi booking chỉ có một dòng điểm Lưu trú;
- `surveys.BookingId` UNIQUE (bảng của TV1): mỗi booking chỉ có một phiếu khảo sát.

### 3.3 Đặt breakpoint theo sơ đồ (chuẩn bị vấn đáp)
Đặt breakpoint ở `SurveyScheduler.ScheduleFor`, chạy web ở chế độ debug rồi bấm Trả phòng trên trang `/frm203_nv_stay`. Cửa sổ Call Stack của Visual Studio hiện khung đang dừng ở trên cùng, người gọi ở dưới:

```
SurveyScheduler.ScheduleFor                        ← dừng ở đây, đã sang phân hệ TV1
StayService.CheckOut.AnonymousMethod               (thân lambda truyền vào Execute)
UnitOfWork.Execute
StayService.CheckOut
frm203_nv_stayModel.OnPostCheckOut.AnonymousMethod (thân lambda truyền vào ErrorHandler.Run)
ErrorHandler.Run
frm203_nv_stayModel.OnPostCheckOut
… (các khung của ASP.NET Core)
```

Chữ ký `ErrorHandler.Run` bản web do TV1 viết ở P2; đối chiếu lại Call Stack khi có code.

**Thử rollback:**
- Cách làm: dừng ở breakpoint trên, ném một ngoại lệ (Immediate Window hoặc sửa tạm `FakeSurveyScheduler` trong test).
- Kỳ vọng sau khi tiếp tục: booking vẫn Đang ở, sổ điểm và hạng không đổi. Đây cũng là integration test TC2-12 (`10-kiem-thu.md`).
- Breakpoint nên đặt thêm: `Booking.CheckOut`, `MembershipTier.PointsFor`, `Customer.EarnPoints`.

## 4. Máy trạng thái booking [TV2]
Sơ đồ: `diagrams/state-booking.puml`. Quy tắc: Q8.

### 4.1 Bảng chuyển trạng thái

| Trạng thái \ Sự kiện | `CheckIn(now)` | `CheckOut(now)` | `Cancel()` |
|---|---|---|---|
| **Đã đặt** (Reserved) | → Đang ở, nếu ngày nhận ≤ hôm nay < ngày trả; ngoài khoảng: lỗi | lỗi | → Đã hủy |
| **Đang ở** (CheckedIn) | lỗi | → Đã trả phòng (+ luồng L3) | lỗi |
| **Đã trả phòng** (CheckedOut) | lỗi | lỗi | lỗi |
| **Đã hủy** (Cancelled) | lỗi | lỗi | lỗi |

"Lỗi" = `BusinessRuleException`, thông báo nêu mã booking và trạng thái hiện tại; trạng thái giữ nguyên. Đã trả phòng và Đã hủy là trạng thái cuối.

Trạng thái nào giữ phòng: Đã đặt và Đang ở (`BlocksRoom`). Chỉ hai trạng thái này được tính khi kiểm tra trùng lịch (Q8).

### 4.2 Vì sao booking không dùng mẫu State (câu hỏi đánh đổi)
Phiếu yêu cầu của TV1 dùng State. Booking dùng enum `BookingStatus` và điều kiện trong từng hàm của `Booking`, vì:
- Chỉ có 3 chuyển trạng thái, không có vòng lặp (phiếu yêu cầu có Mở lại).
- Mỗi chuyển có đúng một điều kiện.
- Dùng State sẽ thêm 4 lớp và một lớp cha mà không bớt được `if` nào đáng kể.
- Nếu sau này thêm trạng thái có hành vi riêng (ví dụ "Không đến", "Gia hạn") thì đó là lúc chuyển sang State. Lúc ấy chỉ `Booking` đổi, không đổi service hay trang, vì bên ngoài chỉ gọi `CheckIn`, `CheckOut`, `Cancel`.

### 4.3 Hạn chế đã biết
- **Khách đã quá ngày trả mà chưa trả phòng** (vẫn Đang ở): booking của khách sau vẫn nhận phòng được đúng ngày của mình. Lý do: quy tắc trùng lịch xét theo ngày đã đặt, không theo việc khách đã rời phòng chưa. Ghi ở báo cáo; không xử lý trong phạm vi bài.
- **Booking Đã đặt mà quá ngày trả** (khách không đến) không tự hủy. Lễ tân hủy tay; nhận phòng lúc đó bị chặn (Q8).

### 4.4 Đặt breakpoint theo sơ đồ (chuẩn bị vấn đáp)
Mỗi mũi tên của sơ đồ là một hàm của `Booking`: `CheckIn(now)`, `CheckOut(now)`, `Cancel()`. Điều kiện chặn (guard) nằm ngay đầu hàm, nên breakpoint đặt ở dòng đầu của hàm cho thấy trạng thái trước khi chuyển.

Ví dụ: đặt breakpoint ở `Booking.CheckIn`, trên trang `/frm203_nv_stay` bấm Nhận phòng ở một booking Đã đặt có ngày nhận là ngày mai (nút vẫn bật vì khóa nút chỉ xét trạng thái; ngày do Domain kiểm). Call Stack:

```
Booking.CheckIn                                   ← dừng ở đây; Status = Reserved, now.Date < CheckInDate
StayService.CheckIn.AnonymousMethod               (thân lambda truyền vào Execute)
UnitOfWork.Execute
StayService.CheckIn
frm203_nv_stayModel.OnPostCheckIn.AnonymousMethod (thân lambda truyền vào ErrorHandler.Run)
ErrorHandler.Run
frm203_nv_stayModel.OnPostCheckIn
```

Bước qua (F10) thì `BusinessRuleException` được ném; trong cửa sổ Locals `Status` vẫn là `Reserved`, đúng với "lỗi thì trạng thái giữ nguyên" ở bảng 4.1. Kiểm thử tương ứng: TC2-04.
