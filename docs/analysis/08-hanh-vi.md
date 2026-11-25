# 08. Thiết kế hành vi: sơ đồ tuần tự và máy trạng thái

Module M7 của P1. [TV1] viết tuần tự L2 (đóng ticket) và máy trạng thái ticket; [TV2] viết tuần tự L3 (trả phòng) và máy trạng thái booking. Định mức N2: ≥ 4 sơ đồ, ≥ 2 mỗi người, ≥ 1 xuyên phân hệ; ở đây có 4 sơ đồ, 2 sơ đồ xuyên phân hệ (L2, L3).

## 1. Tuần tự L2 – Đóng ticket và máy trạng thái ticket [TV1]
> TV1 viết. Phần của TV2 trong L2: `GoodwillPointsObserver` và `InteractionLogger` nhận sự kiện Đã đóng bên trong giao dịch của TV1, ghi dòng sổ điểm Thiện chí (`PointTransaction.ForGoodwill`), cộng điểm đã tích (`Customer.EarnPoints`), ghi tương tác. Observer lỗi → TV1 rollback.

## 2. Tuần tự L3 – Trả phòng [TV2]
Sơ đồ: `diagrams/seq-l3-checkout.puml`. Đặc tả: UC08 luồng C (`03-use-case.md`). Quy tắc: Q6, Q8, Q9.

### 2.1 Các bước

| # | Đối tượng | Việc | Quy tắc / ghi chú |
|---|---|---|---|
| 1 | `frm203` → `ErrorHandler.Run` | handler của nút Trả phòng bọc trong `ErrorHandler.Run` | Form không có quy tắc, không `try/catch` |
| 2 | `StayService.CheckOut(actor, bookingId)` | kiểm quyền: `actor.EnsureCan…()` (tên hàm do TV1 chốt) | Q6; ẩn nút chỉ là lớp chặn thứ hai |
| 3 | `IUnitOfWork.Execute` | mở một connection + một giao dịch | mọi bước 4–10 nằm trong giao dịch này |
| 4 | `Booking.CheckOut(now)` | Đang ở → Đã trả phòng, ghi `CheckedOutAt` | Q8; sai trạng thái → `BusinessRuleException` |
| 5 | `MembershipTier.PointsFor(TotalAmount)` | điểm theo hạng **trước** khi trả phòng | Q9; `TotalAmount` = số đêm × `RatePerNight` đã chốt |
| 6 | `PointTransaction.ForStay` | ghi dòng sổ điểm Lưu trú gắn booking | bỏ qua nếu 0 điểm; unique index chặn cộng hai lần |
| 7 | `Customer.EarnPoints(points)` | cộng điểm đã tích, nâng hạng (có thể nhiều bậc) | Q9; hạng không bao giờ hạ |
| 8 | `Interaction` | ghi tương tác "Trả phòng, +N điểm" | |
| 9 | `ISurveyScheduler.ScheduleFor` (**TV1**) | tạo phiếu khảo sát "Chờ" cho booking | xuyên phân hệ; tham gia cùng giao dịch |
| 10 | `IUnitOfWork` | **COMMIT** nếu mọi bước thành công; **ROLLBACK** và ném lại nếu một bước lỗi | khung `alt` trên sơ đồ |
| 11 | `StayService` → Form | trả `CheckOutResult(points, tierName, upgraded)`; Form báo điểm và hạng mới | |

### 2.2 Vì sao một giao dịch
Nếu bước 9 (phân hệ TV1) lỗi mà không có rollback:
- booking đã Đã trả phòng và điểm đã cộng;
- nhưng không có phiếu khảo sát; lễ tân bấm lại thì bị từ chối vì booking không còn Đang ở;
- lần ở đó không bao giờ được khảo sát, và số liệu R5 lệch.

Rollback đưa mọi thứ về như trước khi bấm, nên lễ tân chỉ việc thử lại.

Hai lớp chặn trùng ngoài giao dịch:
- unique index `ux_points_stay`: mỗi booking chỉ có một dòng điểm Lưu trú;
- `surveys.BookingId` UNIQUE (bảng của TV1): mỗi booking chỉ có một phiếu khảo sát.

### 2.3 Đặt breakpoint theo sơ đồ (chuẩn bị vấn đáp)
Đặt breakpoint ở `SurveyScheduler.ScheduleFor`, bấm Trả phòng trên `frm203`. Call Stack đọc từ dưới lên:

```
frm203_nv_luu_tru.btnCheckOut_Click
ErrorHandler.Run
StayService.CheckOut
UnitOfWork.Execute
StayService.<CheckOut>b__…   (thân lambda truyền vào Execute)
SurveyScheduler.ScheduleFor  ← dừng ở đây, đã sang phân hệ TV1
```

**Thử rollback:**
- Cách làm: dừng ở breakpoint trên, ném một ngoại lệ (Immediate Window hoặc sửa tạm `FakeSurveyScheduler` trong test).
- Kỳ vọng sau khi tiếp tục: booking vẫn Đang ở, sổ điểm và hạng không đổi. Đây cũng là integration test "L3 rollback" ở M8.
- Breakpoint nên đặt thêm: `Booking.CheckOut`, `MembershipTier.PointsFor`, `Customer.EarnPoints`.

## 3. Máy trạng thái booking [TV2]
Sơ đồ: `diagrams/state-booking.puml`. Quy tắc: Q8.

### 3.1 Bảng chuyển trạng thái

| Trạng thái \ Sự kiện | `CheckIn(now)` | `CheckOut(now)` | `Cancel()` |
|---|---|---|---|
| **Đã đặt** (Reserved) | → Đang ở, nếu ngày nhận ≤ hôm nay < ngày trả; ngoài khoảng: lỗi | lỗi | → Đã hủy |
| **Đang ở** (CheckedIn) | lỗi | → Đã trả phòng (+ luồng L3) | lỗi |
| **Đã trả phòng** (CheckedOut) | lỗi | lỗi | lỗi |
| **Đã hủy** (Cancelled) | lỗi | lỗi | lỗi |

"Lỗi" = `BusinessRuleException`, thông báo nêu mã booking và trạng thái hiện tại; trạng thái giữ nguyên. Đã trả phòng và Đã hủy là trạng thái cuối.

Trạng thái nào giữ phòng: Đã đặt và Đang ở (`BlocksRoom`). Chỉ hai trạng thái này được tính khi kiểm tra trùng lịch (Q8).

### 3.2 Vì sao booking không dùng mẫu State (câu hỏi đánh đổi)
Ticket của TV1 dùng State. Booking dùng enum `BookingStatus` và điều kiện trong từng hàm của `Booking`, vì:
- Chỉ có 3 chuyển trạng thái, không có vòng lặp (ticket có Mở lại).
- Mỗi chuyển có đúng một điều kiện.
- Dùng State sẽ thêm 4 lớp và một lớp cha mà không bớt được `if` nào đáng kể.
- Nếu sau này thêm trạng thái có hành vi riêng (ví dụ "Không đến", "Gia hạn") thì đó là lúc chuyển sang State. Lúc ấy chỉ `Booking` đổi, không đổi service hay Form, vì bên ngoài chỉ gọi `CheckIn`, `CheckOut`, `Cancel`.

### 3.3 Hạn chế đã biết
- **Khách đã quá ngày trả mà chưa trả phòng** (vẫn Đang ở): booking của khách sau vẫn nhận phòng được đúng ngày của mình. Lý do: quy tắc trùng lịch xét theo ngày đã đặt, không theo việc khách đã rời phòng chưa. Ghi ở báo cáo; không xử lý trong phạm vi bài.
- **Booking Đã đặt mà quá ngày trả** (khách không đến) không tự hủy. Lễ tân hủy tay; nhận phòng lúc đó bị chặn (Q8).
