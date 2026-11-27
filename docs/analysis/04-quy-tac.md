# 04. Quy tắc nghiệp vụ và phân quyền

Module M4 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.3).

## 1. Vai trò và ma trận quyền [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. Loại ngoại lệ [Chung – TV1]
_[Chung – TV1] chưa điền._

## 3. Quy tắc phân hệ CSKH Q1–Q6 [TV1]
_[TV1] chưa điền._

## 4. Quy tắc phân hệ quan hệ khách hàng Q7–Q13 [TV2]
Thông báo lỗi của `Reward`, `Campaign`, `MembershipTier` lấy đúng từ code PR #2; của `Customer`, `Booking`, `Room` là bản tiếng Việt đề xuất, vì code v1 còn thông báo tiếng Anh và sẽ dịch ở đợt 2. Thông báo chép từ code còn chữ "chiến dịch" được giữ nguyên tới khi code đổi sang "chương trình chăm sóc".

Cách cài: service tra cứu dữ liệu và kiểm quyền; lớp Domain quyết định và ném ngoại lệ. Quy tắc cần dữ liệu của nhiều đối tượng được viết thành hàm Domain nhận kết quả tra cứu làm tham số (ví dụ `Reward.EnsureCanRedeem(tier, balance)`). Mã ca test TC2-xx khớp ma trận ở `10-kiem-thu.md` mục 3. Cột "Code" cho biết lớp/hàm đã có trong PR #2 hay sẽ viết ở đợt 2 (P2) hoặc P3.

| Mã | Quy tắc | Liên phân hệ | Lớp quyết định | Code | Ca test |
|---|---|---|---|---|---|
| Q7 | Hồ sơ khách hợp lệ và không trùng | | `Person`, `Customer` | đợt 2 | TC2-01, TC2-02 |
| Q8 | Đặt phòng, nhận, trả, hủy đúng ngày và trạng thái | | `Booking`, `Room` | có (v1), sửa ở đợt 2 | TC2-03, TC2-04 |
| Q9 | Tích điểm theo hạng, nâng hạng không bao giờ hạ | (L2 qua Q12) | `MembershipTier`, `Customer` | `PointsFor` có; `Customer.EarnPoints` đợt 2 | TC2-05, TC2-06, TC2-11 |
| Q10 | Đổi quà đúng điều kiện, điểm chốt lúc đổi | | `Reward`, `RewardRedemption`, `PointTransaction` | có | TC2-06, TC2-07, TC2-14 |
| Q11 | Chọn khách cho chương trình chăm sóc, ghi phản hồi đúng hạn | ✓ L4 (`ITicketQuery`) | `Campaign`, `ISegmentStrategy` | có | TC2-08, TC2-09, TC2-14 |
| Q12 | Đóng phiếu yêu cầu → điểm thiện chí, cùng giao dịch | ✓ L2 (`ITicketObserver`) | `GoodwillPointsObserver`, `PointTransaction` | `ForGoodwill` có; observer P3 | integration L2 của TV1 |
| Q13 | Không xóa khách đã có lưu trú, phiếu yêu cầu hoặc đã nằm trong chương trình chăm sóc | ✓ L5 (`ITicketQuery`) | `Customer` | đợt 2 | TC2-10 |

### Q7 – Hồ sơ khách hợp lệ và không trùng
- **Phát biểu:**
  - Họ tên bắt buộc.
  - SĐT đúng 10 chữ số, bắt đầu bằng 0.
  - Email có đúng một `@`, có dấu chấm sau `@`, không có khoảng trắng.
  - Quốc tịch bắt buộc. Quốc tịch "Việt Nam" thì số giấy tờ là CCCD đúng 12 chữ số; quốc tịch khác thì là hộ chiếu 6–9 ký tự chữ (A–Z) và số (quyết định ở `docs/PROGRESS.md` mục 8).
  - Ngày sinh không bắt buộc; nếu nhập thì không sau ngày hiện tại.
  - Không hai khách nào trùng SĐT, email hoặc số giấy tờ.
- **Tiền điều kiện:** người thao tác là Lễ tân, Quản lý hoặc Admin (Q6).
- **Hậu điều kiện:** hồ sơ được lưu với giá trị đã cắt khoảng trắng hai đầu. Khách mới có hạng Standard.
- **Vi phạm:**
  - Sai định dạng hoặc thiếu trường → `ValidationException`, ví dụ "Số CCCD phải gồm đúng 12 chữ số.", "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0."
  - Trùng → `BusinessRuleException`, ví dụ "Số điện thoại đã được dùng cho khách khác."
  - Khi sửa hồ sơ thì không so trùng với chính khách đó.
- **Lớp/hàm:**
  - SĐT, email: `Person`.
  - Giấy tờ theo quốc tịch, ngày sinh: constructor và `UpdateProfile` của `Customer`. Kiểm tra hết rồi mới gán, để lỗi giữa chừng không để lại hồ sơ sửa dở.
  - Không trùng: `Customer.EnsureUnique(phoneTaken, emailTaken, identityTaken)`; `CustomerService` tra ba giá trị rồi truyền vào.
- **Liên phân hệ:** không.
- **Ca test:** TC2-01, TC2-02.
- **Giá trị biên:** SĐT 9 / 10 / 11 chữ số; CCCD 11 / 12 / 13 chữ số; hộ chiếu 5 / 6 / 9 / 10 ký tự; ngày sinh = hôm nay (được) / hôm nay + 1 (lỗi).

### Q8 – Đặt phòng, nhận phòng, trả phòng, hủy
- **Phát biểu:**
  - Ngày trả > ngày nhận; chỉ xét phần ngày.
  - Một phòng không có hai booking chưa hủy (Đã đặt hoặc Đang ở) chồng khoảng ngày. Khoảng ngày là nửa mở [nhận, trả): booking trả ngày 10 và booking nhận ngày 10 không chồng nhau.
  - Giá/đêm chốt bằng giá của loại phòng lúc đặt, không đổi theo bảng giá về sau.
  - Chỉ nhận phòng khi booking Đã đặt và hôm nay nằm trong [ngày nhận, ngày trả).
  - Chỉ trả phòng khi booking Đang ở. Trả sớm được và vẫn tính đủ số đêm đã đặt.
  - Chỉ hủy booking Đã đặt.
- **Tiền điều kiện:** người thao tác là Lễ tân, Quản lý hoặc Admin.
- **Hậu điều kiện:** booking ở trạng thái mới, có thời điểm và người làm. Máy trạng thái booking ở `08-hanh-vi.md` mục 4.
- **Vi phạm:**
  - Ngày sai → `ValidationException`, ví dụ "Ngày trả phòng phải sau ngày nhận phòng."
  - Trùng lịch, sai ngày nhận phòng, sai trạng thái → `BusinessRuleException`. Thông báo nêu số phòng hoặc mã booking và trạng thái hiện tại, ví dụ "Phòng 305 đã có đặt phòng từ 10/12/2026 đến 12/12/2026.", "Đặt phòng 15 đang ở trạng thái Đã hủy, không thể nhận phòng."
- **Lớp/hàm:** constructor `Booking`, `Room.EnsureAvailable(from, to, bookings)`, `Booking.CheckIn(now)`, `CheckOut(now)`, `Cancel()`.
- **Liên phân hệ:** không (phiếu khảo sát khi trả phòng thuộc L3, xem UC08).
- **Ca test:** TC2-03, TC2-04.
- **Giá trị biên:** booking 10–12: đặt 11–13 (chồng, lỗi), 12–14 (nối tiếp, được); nhận phòng lúc 09/xx 23:59 (lỗi), 10/xx 00:00 (được), 12/xx 00:00 (lỗi).

### Q9 – Tích điểm và hạng thành viên
- **Phát biểu:**
  - Khi trả phòng, điểm = thành tiền / 10.000đ × hệ số tích điểm của hạng khách đang có trước khi trả phòng, làm tròn xuống (quyết định ở `docs/PROGRESS.md` mục 8).
  - Điểm đã tích = tổng các dòng sổ điểm lý do Lưu trú và Thiện chí. Điểm đã đổi quà không bị trừ khỏi con số này.
  - Hạng = hạng cao nhất có ngưỡng ≤ điểm đã tích (Standard 0, Silver 1.000, Gold 5.000, VIP 15.000). Một lần cộng điểm có thể lên nhiều bậc. Hạng không bao giờ hạ.
  - Hệ số tích điểm: Standard 1,0 · Silver 1,2 · Gold 1,5 · VIP 2,0.
- **Tiền điều kiện:** booking vừa chuyển sang Đã trả phòng (UC08 C2), hoặc phiếu yêu cầu vừa đóng (Q12).
- **Hậu điều kiện:** có đúng một dòng sổ điểm lý do Lưu trú gắn với booking; hạng khách đã được xét lại.
- **Vi phạm:** số tiền âm → `ValidationException` ("Số tiền tính điểm không được âm."). Điểm tính ra bằng 0 thì không ghi dòng sổ điểm (dòng sổ điểm phải khác 0); không coi là lỗi.
- **Lớp/hàm:**
  - `MembershipTier.PointsFor(amountVnd)` (đã có), `EarnRate` của 4 lớp hạng (đã có).
  - `Customer.EarnPoints(points)`: cộng điểm đã tích, xét hạng bằng `Next()` (đợt 2).
  - `PointTransaction.ForStay` (đã có).
- **Liên phân hệ:** không trực tiếp; điểm thiện chí từ L2 đi qua Q12 rồi dùng chung `Customer.EarnPoints`.
- **Ca test:** `MembershipTierTests` (đã có, 11 trường hợp); TC2-05, TC2-06, TC2-11.
- **Giá trị biên:** điểm đã tích 999 / 1.000 (Silver), 4.999 / 5.000 (Gold), 14.999 / 15.000 (VIP).
- **Ví dụ tính tay:** khách Silver ở 2 đêm × 1.250.000đ = 2.500.000đ → 250 × 1,2 = **300 điểm**. Khách Standard có 900 điểm đã tích, ở 1 đêm 5.000.000đ → +500 điểm → 1.400 điểm → lên Silver. Nếu lần ở đó là 42.000.000đ (+4.200 điểm) thì có 5.100 điểm → lên thẳng Gold, bỏ qua Silver.

### Q10 – Đổi quà
- **Phát biểu:**
  - Chỉ đổi được khi: quà còn áp dụng; còn hàng; hạng khách ≥ hạng tối thiểu của quà; số dư ≥ điểm quy đổi.
  - Điểm quy đổi chốt vào lượt đổi tại lúc đổi.
  - Số dư (tổng mọi dòng sổ điểm) không bao giờ âm.
  - Hủy lượt đổi: trả quà về kho và ghi một dòng hoàn điểm bằng đúng điểm đã đổi. Mỗi lượt chỉ hủy được một lần.
- **Tiền điều kiện:** người thao tác là Lễ tân, Quản lý hoặc Admin. Trong giao dịch, service đọc lại số dư, hạng và số lượng quà.
- **Hậu điều kiện:** lượt đổi Đã đổi; kho quà giảm 1; sổ điểm có thêm dòng −điểm quy đổi; hạng không đổi.
- **Vi phạm:** mọi điều kiện trên sai → `BusinessRuleException`, kiểm tra theo thứ tự còn áp dụng → còn hàng → hạng → điểm; kiểm tra xong mới trừ kho. Thông báo (từ code PR #2): "Quà "…" đã ngừng áp dụng.", "Quà "…" đã hết.", "Quà "…" dành cho hạng Gold trở lên; khách đang ở hạng Silver.", "Quà "…" cần 500 điểm; khách chỉ còn 499 điểm.", "Lượt đổi quà 8 đã bị hủy trước đó."
- **Lớp/hàm:** `Reward.EnsureCanRedeem`, `Reward.Redeem`, `Reward.CancelRedemption`, `RewardRedemption` (constructor `internal`, chỉ `Reward` tạo), `PointTransaction.ForRedemption`, `ForRefund`. Tất cả đã có. CSDL thêm `CHECK (Quantity >= 0)`.
- **Liên phân hệ:** không.
- **Ca test:** `RewardTests` (6), `PointTransactionTests` (3) đã có; TC2-06, TC2-07; RBAC ở TC2-14.
- **Giá trị biên:** số dư = điểm quy đổi (được) / điểm quy đổi − 1 (lỗi); số lượng còn 1 (được) / 0 (lỗi).

### Q11 – Chương trình chăm sóc
- **Phát biểu:**
  - Danh sách khách = khách thuộc phân khúc đã chọn (Strategy, tính theo ngày bắt đầu) ∩ khách đồng ý nhận ưu đãi − khách đang có khiếu nại chưa đóng.
  - Mỗi khách xuất hiện một lần trong một chương trình. Chương trình phải có ít nhất một khách.
  - Ngày kết thúc ≥ ngày bắt đầu.
  - Phản hồi ghi được tới hết ngày kết thúc; ghi lại thì thay phản hồi cũ.
  - Lúc lưu, khách nào vừa trở nên không đủ điều kiện thì bị loại, các khách còn lại vẫn được lưu và hệ thống báo khách bị loại.
  - Hai điểm "ít nhất một khách" và "loại khách lúc lưu" là quyết định ở `docs/PROGRESS.md` mục 8.
- **Tiền điều kiện:** tạo chương trình cần Quản lý hoặc Admin; ghi phản hồi thì mọi vai trò làm được (Q6).
- **Hậu điều kiện:** chương trình và danh sách khách được lưu trong một giao dịch; mỗi dòng có kênh và ngày gửi.
- **Vi phạm:**
  - Thiếu tên, ưu đãi hoặc phân khúc; ngày sai → `ValidationException` ("Ngày kết thúc phải bằng hoặc sau ngày bắt đầu.").
  - Khách không đồng ý, đang khiếu nại, đã có trong danh sách; danh sách rỗng; ghi phản hồi sau ngày kết thúc hoặc cho khách ngoài danh sách → `BusinessRuleException` ("Khách 12 đang có khiếu nại chưa đóng.", "Chiến dịch "…" đã kết thúc ngày 31/12/2026.").
- **Lớp/hàm:**
  - Đã có: `Campaign.IsEligible`, `Campaign.AddTarget`, `Campaign.EnsureHasTargets`, `Campaign.RecordResponse`, `ISegmentStrategy` + `VipSegment`, `LapsedGuestSegment`, `BirthdayMonthSegment`.
  - Context: `CampaignService` (P3).
- **Liên phân hệ (L4):** "khách đang có khiếu nại chưa đóng" lấy từ phân hệ TV1 qua `ITicketQuery.GetCustomersWithOpenComplaints`, hỏi một lần cho cả danh sách. Chữ ký chốt ở M6.
- **Ca test:** `CampaignTests` (10), `SegmentStrategyTests` (11) đã có; TC2-08, TC2-09; RBAC ở TC2-14.
- **Giá trị biên:** lâu chưa quay lại: 180 ngày (có) / 179 ngày (không); sinh nhật với chương trình bắt đầu 01/12: sinh 01/12 và 31/12 (có), 30/11 (không); ghi phản hồi đúng ngày kết thúc (được) / ngày sau (lỗi).

### Q12 – Điểm thiện chí khi đóng phiếu yêu cầu
- **Phát biểu:**
  - Khi một phiếu yêu cầu chuyển sang Đã đóng, khách của phiếu được cộng điểm thiện chí theo loại phiếu (khiếu nại 50, loại khác 10) và được ghi một tương tác gắn với phiếu.
  - Hai việc này nằm trong cùng giao dịch với việc đóng phiếu.
  - Phiếu Đã đóng không mở lại được (Q1), nên mỗi phiếu cho điểm thiện chí tối đa một lần.
- **Tiền điều kiện:** phân hệ TV1 đã đổi trạng thái phiếu và phát sự kiện `ITicketObserver` bên trong giao dịch.
- **Hậu điều kiện:** sổ điểm có một dòng lý do Thiện chí gắn `TicketId`; điểm đã tích tăng nên hạng có thể được nâng (Q9); có một tương tác.
- **Vi phạm:** bước của TV2 lỗi → ngoại lệ (`DomainException` hoặc `DataAccessException`) đi ngược lên `TicketService`, **rollback cả việc đóng phiếu** (luồng L2). Phiếu vẫn Đã giải quyết, quản lý đóng lại sau.
- **Lớp/hàm:**
  - TV2: `GoodwillPointsObserver`, `InteractionLogger` (P3), `PointTransaction.ForGoodwill` (đã có).
  - TV1: `SupportTicket.GoodwillPoints` (số điểm theo loại phiếu).
  - Observer chỉ làm việc khi trạng thái mới là Đã đóng.
- **Liên phân hệ (L2):** nhận sự kiện qua `ITicketObserver` (TV1 khai báo, TV2 cài).
- **Ca test:** integration của TV1 cho luồng L2 (đầy đủ và rollback).
- **Giá trị biên:** không có.

### Q13 – Không xóa khách đã có lưu trú, phiếu yêu cầu hoặc đã nằm trong chương trình chăm sóc
- **Phát biểu:**
  - Không xóa khách đã có ít nhất một booking (mọi trạng thái, kể cả Đã hủy), ít nhất một phiếu yêu cầu (mọi trạng thái), hoặc đã nằm trong ít nhất một chương trình chăm sóc (mọi phản hồi, kể cả chưa phản hồi).
  - Lý do: các bảng `bookings`, `tickets`, `point_transactions`, `interactions`, `campaign_targets` trỏ tới khách; xóa khách làm mất lịch sử CRM và số liệu báo cáo R4.
  - PLAN 5.3 chỉ ghi "lưu trú hoặc ticket". Phần "đã nằm trong chương trình" thêm vào để khách chưa ở lần nào nhưng đã được chọn vào chương trình (ví dụ "Sinh nhật trong tháng") bị Domain chặn với thông báo rõ ràng, thay vì lỗi khóa ngoại của CSDL (`06-erd.md` mục 3.5; TV1 + TV2 duyệt ở PR #3).
- **Tiền điều kiện:** người thao tác là Quản lý hoặc Admin (Q6).
- **Hậu điều kiện:** khách bị xóa, hoặc không có gì thay đổi.
- **Vi phạm:** còn booking, phiếu yêu cầu hoặc chương trình → `BusinessRuleException` nêu lý do, ví dụ "Không xóa được khách đã có lần đặt phòng, phiếu yêu cầu hoặc đã nằm trong chương trình chăm sóc."; thiếu quyền → `AuthorizationException`. Khóa ngoại `ON DELETE RESTRICT` là lớp chặn thứ hai ở CSDL.
- **Lớp/hàm:** `Customer.EnsureCanBeDeleted(hasStays, hasTickets, inCampaigns)` (đợt 2; bản v1 chỉ xét phiếu yêu cầu chưa đóng). `CustomerService` tra cứu ba giá trị rồi gọi hàm này; `hasStays` và `inCampaigns` đọc từ bảng của TV2 (`bookings`, `campaign_targets`).
- **Liên phân hệ (L5):** "khách có phiếu yêu cầu không" hỏi phân hệ TV1 qua `ITicketQuery.HasAnyTicket`.
- **Ca test:** TC2-10.
- **Giá trị biên:** không có booking, phiếu yêu cầu, chương trình (xóa được) / 1 booking Đã hủy (lỗi) / 1 phiếu Đã đóng (lỗi) / chỉ nằm trong 1 chương trình, chưa phản hồi (lỗi).

### Việc phát sinh cho code [TV2]
- Đợt 2 (`Customer`, `Booking`): cài Q7 theo quốc tịch, Q9 `EarnPoints`, Q13 theo `hasStays`/`hasTickets`/`inCampaigns`, `Booking.CheckOut(now)`.
