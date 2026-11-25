# 04. Quy tắc nghiệp vụ và phân quyền

Module M4 của P1. [TV1] viết Q1–Q6 và ma trận quyền; [TV2] viết Q7–Q13.

## 1. Quy ước
- **Nguyên tắc cài đặt:** service tra cứu dữ liệu và kiểm quyền; lớp Domain quyết định và ném ngoại lệ. Quy tắc cần dữ liệu của nhiều đối tượng được viết thành hàm Domain nhận kết quả tra cứu làm tham số (ví dụ `Reward.EnsureCanRedeem(tier, balance)`).
- **Loại ngoại lệ:**
  - `ValidationException`: dữ liệu của một đối tượng sai (rỗng, sai định dạng, ngoài khoảng).
  - `BusinessRuleException`: sai quy tắc giữa các đối tượng hoặc sai trạng thái.
  - `AuthorizationException`: sai quyền.
- **Mỗi quy tắc ghi:** phát biểu, tiền điều kiện, hậu điều kiện, ngoại lệ, nơi cài đặt, test.
- **Mã test:** TC2-xx là dự kiến, chốt trong ma trận test ở M8 (`10-kiem-thu.md`). Cột "Code" cho biết lớp/hàm đã có trong PR #2 hay sẽ viết sau.

## 2. Q1–Q6 và ma trận quyền [TV1]
> TV1 viết. Q6 (phân quyền) là quy tắc chung: TV1 cài nền `Employee` và quyền ticket; TV2 gọi kiểm quyền trong service của mình theo ma trận này.

## 3. Q7–Q13 [TV2]

### Tóm tắt

| Mã | Quy tắc | Liên phân hệ | Lớp quyết định | Code |
|---|---|---|---|---|
| Q7 | Hồ sơ khách hợp lệ và không trùng | | `Person`, `Customer` | đợt 2 |
| Q8 | Đặt phòng, nhận, trả, hủy đúng ngày và trạng thái | | `Booking`, `Room` | có (v1), sửa ở đợt 2 |
| Q9 | Tích điểm theo hạng, nâng hạng không bao giờ hạ | | `MembershipTier`, `Customer` | `PointsFor` có; `Customer.EarnPoints` đợt 2 |
| Q10 | Đổi quà đúng điều kiện, điểm chốt lúc đổi | | `Reward`, `RewardRedemption`, `PointTransaction` | có |
| Q11 | Chọn khách cho chiến dịch, ghi phản hồi đúng hạn | ✓ L4 (`ITicketQuery`) | `Campaign`, `ISegmentStrategy` | có; thiếu `EnsureHasTargets` |
| Q12 | Đóng ticket → điểm thiện chí, cùng giao dịch | ✓ L2 (`ITicketObserver`) | `GoodwillPointsObserver`, `PointTransaction` | `ForGoodwill` có; observer P3 |
| Q13 | Không xóa khách đã có lưu trú hoặc ticket | ✓ L5 (`ITicketQuery`) | `Customer` | đợt 2 |

### Q7 – Hồ sơ khách hợp lệ và không trùng
- **Phát biểu:**
  - Họ tên bắt buộc.
  - SĐT đúng 10 chữ số, bắt đầu bằng 0.
  - Email có đúng một `@`, có dấu chấm sau `@`, không có khoảng trắng.
  - Quốc tịch bắt buộc. Quốc tịch "Việt Nam" thì số giấy tờ là CCCD đúng 12 chữ số; quốc tịch khác thì là hộ chiếu 6–9 ký tự chữ (A–Z) và số.
  - Ngày sinh không bắt buộc; nếu nhập thì không sau ngày hiện tại.
  - Không hai khách nào trùng SĐT, email hoặc số giấy tờ.
- **Tiền điều kiện:** người thao tác là Lễ tân, Quản lý hoặc Admin (Q6).
- **Hậu điều kiện:** hồ sơ được lưu với giá trị đã cắt khoảng trắng hai đầu. Khách mới có hạng Standard.
- **Ngoại lệ:**
  - Sai định dạng hoặc thiếu trường → `ValidationException`, ví dụ "Số CCCD phải gồm đúng 12 chữ số."
  - Trùng → `BusinessRuleException`, ví dụ "Số điện thoại đã được dùng cho khách khác."
  - Khi sửa hồ sơ thì không so trùng với chính khách đó.
- **Cài đặt:**
  - SĐT, email: `Person`.
  - Giấy tờ theo quốc tịch, ngày sinh: constructor và `UpdateProfile` của `Customer`. Kiểm tra hết rồi mới gán, để lỗi giữa chừng không để lại hồ sơ sửa dở.
  - Không trùng: `Customer.EnsureUnique(phoneTaken, emailTaken, identityTaken)`; `CustomerService` tra ba giá trị rồi truyền vào.
- **Test:** TC2-01 (BVA độ dài: SĐT 9/10/11 số; CCCD 11/12/13 số; hộ chiếu 5/6/9/10 ký tự), TC2-02 (trùng SĐT).

### Q8 – Đặt phòng, nhận phòng, trả phòng, hủy
- **Phát biểu:**
  - Ngày trả > ngày nhận; chỉ xét phần ngày.
  - Một phòng không có hai booking chưa hủy (Đã đặt hoặc Đang ở) chồng khoảng ngày. Khoảng ngày là nửa mở [nhận, trả): booking trả ngày 10 và booking nhận ngày 10 không chồng nhau.
  - Giá/đêm chốt bằng giá của loại phòng lúc đặt, không đổi theo bảng giá về sau.
  - Chỉ nhận phòng khi booking Đã đặt và hôm nay nằm trong [ngày nhận, ngày trả).
  - Chỉ trả phòng khi booking Đang ở. Trả sớm được và vẫn tính đủ số đêm đã đặt.
  - Chỉ hủy booking Đã đặt.
- **Tiền điều kiện:** người thao tác là Lễ tân, Quản lý hoặc Admin.
- **Hậu điều kiện:** booking ở trạng thái mới, có thời điểm và người làm. Máy trạng thái booking vẽ ở M7.
- **Ngoại lệ:**
  - Ngày sai → `ValidationException`.
  - Trùng lịch, sai ngày nhận phòng, sai trạng thái → `BusinessRuleException`. Thông báo nêu số phòng hoặc mã booking và trạng thái hiện tại.
- **Cài đặt:** constructor `Booking`, `Room.EnsureAvailable(from, to, bookings)`, `Booking.CheckIn(now)`, `CheckOut(now)`, `Cancel()`.
- **Test:** TC2-03 (đặt trùng lịch và đặt nối tiếp đúng ngày trả, BVA), TC2-04 (nhận phòng trước ngày nhận).

### Q9 – Tích điểm và hạng thành viên
- **Phát biểu:**
  - Khi trả phòng, điểm = thành tiền / 10.000đ × hệ số tích điểm của hạng khách đang có trước khi trả phòng, làm tròn xuống.
  - Điểm đã tích = tổng các dòng sổ điểm lý do Lưu trú và Thiện chí. Điểm đã đổi quà không bị trừ khỏi con số này.
  - Hạng = hạng cao nhất có ngưỡng ≤ điểm đã tích (Standard 0, Silver 1.000, Gold 5.000, VIP 15.000). Một lần cộng điểm có thể lên nhiều bậc. Hạng không bao giờ hạ.
- **Tiền điều kiện:** booking vừa chuyển sang Đã trả phòng (UC08 C2).
- **Hậu điều kiện:** có đúng một dòng sổ điểm lý do Lưu trú gắn với booking; hạng khách đã được xét lại.
- **Ngoại lệ:** số tiền âm → `ValidationException`. Điểm tính ra bằng 0 thì không ghi dòng sổ điểm (dòng sổ điểm phải khác 0); không coi là lỗi.
- **Cài đặt:**
  - `MembershipTier.PointsFor(amountVnd)` (đã có), `EarnRate` của 4 lớp hạng (đã có).
  - `Customer.EarnPoints(points)`: cộng điểm đã tích, xét hạng bằng `Next()` (đợt 2).
  - `PointTransaction.ForStay` (đã có).
- **Ví dụ tính tay:** khách Silver ở 2 đêm × 1.250.000đ = 2.500.000đ → 250 × 1,2 = **300 điểm**. Khách Standard có 900 điểm đã tích, ở 1 đêm 5.000.000đ → +500 điểm → 1.400 điểm → lên Silver. Nếu lần ở đó là 42.000.000đ (+4.200 điểm) thì có 5.100 điểm → lên thẳng Gold, bỏ qua Silver.
- **Test:** `MembershipTierTests` (đã có, 11 trường hợp); TC2-05 (tích điểm theo hạng và lên 2 bậc).

### Q10 – Đổi quà
- **Phát biểu:**
  - Chỉ đổi được khi: quà còn áp dụng; còn hàng; hạng khách ≥ hạng tối thiểu của quà; số dư ≥ điểm quy đổi.
  - Điểm quy đổi chốt vào lượt đổi tại lúc đổi.
  - Số dư (tổng mọi dòng sổ điểm) không bao giờ âm.
  - Hủy lượt đổi: trả quà về kho và ghi một dòng hoàn điểm bằng đúng điểm đã đổi. Mỗi lượt chỉ hủy được một lần.
- **Tiền điều kiện:** người thao tác là Lễ tân, Quản lý hoặc Admin. Trong giao dịch, service đọc lại số dư, hạng và số lượng quà.
- **Hậu điều kiện:** lượt đổi Đã đổi; kho quà giảm 1; sổ điểm có thêm dòng −điểm quy đổi; hạng không đổi.
- **Ngoại lệ:** mọi điều kiện trên sai → `BusinessRuleException`. Thông báo nêu cụ thể lý do, ví dụ "Quà "Bữa tối cho hai người" cần 500 điểm; khách chỉ còn 499 điểm." Kiểm tra theo thứ tự: còn áp dụng → còn hàng → hạng → điểm. Kiểm tra xong mới trừ kho.
- **Cài đặt:** `Reward.EnsureCanRedeem`, `Reward.Redeem`, `Reward.CancelRedemption`, `RewardRedemption` (constructor `internal`, chỉ `Reward` tạo), `PointTransaction.ForRedemption`, `ForRefund`. Tất cả đã có. CSDL thêm `CHECK (Quantity >= 0)`.
- **Test:** `RewardTests` (6), `PointTransactionTests` (3) đã có; TC2-06 (đổi quà giảm số dư, không hạ hạng), TC2-07 (thiếu điểm, thiếu hạng, hết quà).

### Q11 – Chiến dịch chăm sóc
- **Phát biểu:**
  - Danh sách khách = khách thuộc phân khúc đã chọn (Strategy, tính theo ngày bắt đầu) ∩ khách đồng ý nhận ưu đãi − khách đang có khiếu nại chưa đóng.
  - Mỗi khách xuất hiện một lần trong một chiến dịch. Chiến dịch phải có ít nhất một khách.
  - Ngày kết thúc ≥ ngày bắt đầu.
  - Phản hồi ghi được tới hết ngày kết thúc; ghi lại thì thay phản hồi cũ.
  - Lúc lưu, khách nào vừa trở nên không đủ điều kiện thì bị loại, các khách còn lại vẫn được lưu và hệ thống báo khách bị loại.
- **Tiền điều kiện:** tạo chiến dịch cần Quản lý hoặc Admin; ghi phản hồi thì mọi vai trò làm được (Q6).
- **Hậu điều kiện:** chiến dịch và danh sách khách được lưu trong một giao dịch; mỗi dòng có kênh và ngày gửi.
- **Ngoại lệ:**
  - Thiếu tên, ưu đãi hoặc phân khúc; ngày sai → `ValidationException`.
  - Khách không đồng ý, đang khiếu nại, đã có trong danh sách; danh sách rỗng; ghi phản hồi sau ngày kết thúc hoặc cho khách ngoài danh sách → `BusinessRuleException`.
- **Liên phân hệ (L4):** "khách đang có khiếu nại chưa đóng" lấy từ phân hệ TV1 qua `ITicketQuery`, hỏi một lần cho cả danh sách. Chữ ký chốt ở M6.
- **Cài đặt:**
  - Đã có: `Campaign.IsEligible`, `Campaign.AddTarget`, `Campaign.RecordResponse`, `ISegmentStrategy` + `VipSegment`, `LapsedGuestSegment`, `BirthdayMonthSegment`.
  - Cần thêm: `Campaign.EnsureHasTargets()`, gọi trước khi lưu.
  - Context: `CampaignService`.
- **Test:** `CampaignTests` (8), `SegmentStrategyTests` (11) đã có; TC2-08 (3 phân khúc chọn đúng khách, BVA ngày), TC2-09 (loại khách có khiếu nại, dùng `FakeTicketQuery`).

### Q12 – Điểm thiện chí khi đóng ticket
- **Phát biểu:**
  - Khi một ticket chuyển sang Đã đóng, khách của ticket được cộng điểm thiện chí theo loại ticket (khiếu nại 50, loại khác 10) và được ghi một tương tác gắn với ticket.
  - Hai việc này nằm trong cùng giao dịch với việc đóng ticket.
  - Ticket Đã đóng không mở lại được (Q1), nên mỗi ticket cho điểm thiện chí tối đa một lần.
- **Tiền điều kiện:** phân hệ TV1 đã đổi trạng thái ticket và phát sự kiện `ITicketObserver.OnStateChanged` bên trong giao dịch.
- **Hậu điều kiện:** sổ điểm có một dòng lý do Thiện chí gắn `TicketId`; điểm đã tích tăng nên hạng có thể được nâng (Q9); có một tương tác.
- **Ngoại lệ:** bước của TV2 lỗi → ngoại lệ đi ngược lên `TicketService`, **rollback cả việc đóng ticket** (luồng L2). Ticket vẫn Đã giải quyết, quản lý đóng lại sau.
- **Cài đặt:**
  - TV2: `GoodwillPointsObserver`, `InteractionLogger` (P3), `PointTransaction.ForGoodwill` (đã có).
  - TV1: `SupportTicket.GoodwillPoints` (số điểm theo loại ticket).
  - Observer chỉ làm việc khi trạng thái mới là Đã đóng.
- **Test:** integration của TV1 cho luồng L2 (đầy đủ và rollback).

### Q13 – Không xóa khách đã có lưu trú hoặc ticket
- **Phát biểu:** không xóa khách đã có ít nhất một booking (mọi trạng thái, kể cả Đã hủy) hoặc ít nhất một ticket (mọi trạng thái). Lý do: các bảng `bookings`, `tickets`, `point_transactions`, `interactions` trỏ tới khách; xóa khách làm mất lịch sử CRM.
- **Tiền điều kiện:** người thao tác là Quản lý hoặc Admin (Q6).
- **Hậu điều kiện:** khách bị xóa, hoặc không có gì thay đổi.
- **Ngoại lệ:** còn booking hoặc ticket → `BusinessRuleException` nêu số booking/ticket. Thiếu quyền → `AuthorizationException`. Khóa ngoại `ON DELETE RESTRICT` là lớp chặn thứ hai ở CSDL.
- **Liên phân hệ (L5):** "khách có ticket không" hỏi phân hệ TV1 qua `ITicketQuery`.
- **Cài đặt:** `Customer.EnsureCanBeDeleted(hasStays, hasTickets)` (đợt 2; bản v1 chỉ xét ticket chưa đóng).
- **Test:** TC2-10 (xóa khách có ticket bị chặn, dùng `FakeTicketQuery`).

## 4. Việc phát sinh cho code [TV2]
- Thêm `Campaign.EnsureHasTargets()` và test (Q11). Thêm vào PR #2 hoặc làm ở P3.
- Đợt 2 (`Customer`, `Booking`): cài Q7 theo quốc tịch, Q9 `EarnPoints`, Q13 theo `hasStays`/`hasTickets`, `Booking.CheckOut(now)`.
