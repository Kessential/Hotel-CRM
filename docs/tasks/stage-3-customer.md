# GĐ 3 — Suất B: phân hệ Khách hàng/Lưu trú

Hạn: 23/11/2026. Model: Sonnet 5.5. Chạy song song được với GĐ 2.

## Nhánh và cách bàn giao
Đọc mục "Cách commit của suất B" trong `docs/PROGRESS.md` trước:
- **Nhận patch** (mặc định): làm trên nhánh cục bộ `local/b-work` (đã có từ GĐ 1, **không bao giờ push**). Mỗi commit xuất thành một patch trong `handoff/` theo mục "Xuất patch" bên dưới.
- **Tự code**: phiên này không viết code suất B; thành viên B tự làm theo file này trên `feature/b-customer`.

Trên `local/b-work` lúc này chưa có code ticket của suất A: 11 test trong `Tickets/` vẫn đỏ suốt giai đoạn này, đó là bình thường. Không sửa gì của suất A.

## Mục tiêu
1. Điền thân hàm của khách hàng, hạng thành viên, phòng, booking: quy tắc 6, 7, 8, 10; ba strategy phân công.
2. 9 test xanh: `CustomerTests` (4), `CustomerServiceTests` (2), `BookingTests` (1), `BookingServiceTests` (1), `AssignmentStrategyTests` (1). Hai test của `SurveyServiceTests` để GĐ 4. Không sửa test.
3. `CustomerListForm`, `CustomerEditForm`, `BookingForm` có nội dung, mở được bằng Designer.

Khung đã có sẵn từ GĐ 1. Việc của giai đoạn này là thay `throw new NotImplementedException();` bằng thân hàm đúng với chú thích XML ngay phía trên nó.

## Đọc trước
- `.claude/rules/domain.md`, `business.md`, `winforms.md`, `tests.md`.
- Mã nguồn: `Domain/Customers/**`, `Domain/Hotel/**`, `Domain/Common/Person.cs`, `Domain/Interfaces/**`, `Business/Services/CustomerService.cs`, `BookingService.cs`, `Business/Queries/CustomerDirectory.cs`, `StayLookup.cs`, `Business/Assignment/**`, `Forms/LoginForm*.cs` (form mẫu), `tests/HotelCRM.Tests/Customers/**`, `Assignment/**`.
- `docs/PLAN.md`: chỉ mục "Git" (quy trình patch). Con số đã chép bên dưới.

## File được sửa
- `src/HotelCRM.Domain/Customers/**` trừ `SatisfactionSurvey.cs` (GĐ 4)
- `src/HotelCRM.Domain/Hotel/**`
- `src/HotelCRM.Business/Services/CustomerService.cs`, `BookingService.cs`
- `src/HotelCRM.Business/Queries/CustomerDirectory.cs`, `StayLookup.cs`
- `src/HotelCRM.Business/Assignment/*Strategy.cs` (3 lớp)
- `src/HotelCRM.WinForms/Forms/Customers/CustomerListForm*`, `CustomerEditForm*`; `Forms/Bookings/BookingForm*`
- `src/HotelCRM.WinForms/Data/roomtypes.json`, `rooms.json`: thêm vài dòng để bấm thử (xem Module 7)
- `docs/PROGRESS.md`, `handoff/**`

## File cấm đụng
`Program.cs`, `MainForm*`, `LoginForm*`, `ErrorHandler.cs`, toàn bộ `src/HotelCRM.Data/**`, `Domain/Common/**`, `Domain/Staff/**`, `Domain/Interfaces/**`, `Domain/Tickets/**`, `TicketService.cs`, `TicketQuery.cs`, `Business/Factories/**`, `Forms/Tickets/**`, `IAssignmentStrategy.cs`, `AgentWorkload.cs`, mọi test và lớp giả. Không đổi tên, không đổi chữ ký public, không đổi constructor của Form. Thấy cần đổi thì dừng, ghi lý do vào `docs/PROGRESS.md` và hỏi.

Để GĐ 4: `SatisfactionSurvey`, `SurveyService`, `ReportService`, `Business/Observers/**`, `UiNotificationObserver`, `CustomerHistoryForm`, `SurveyForm`, `ReportForm`.

## Quy tắc và con số

| # | Quy tắc | Ngoại lệ khi vi phạm |
|---|---|---|
| 6 | SĐT đúng 10 chữ số, bắt đầu bằng `0`; email có đúng một `@`, có dấu chấm sau `@`, không có khoảng trắng (đã cài trong `Person`). SĐT và email không trùng với khách khác; email so không phân biệt hoa thường | Sai định dạng, bỏ trống trường bắt buộc: `ValidationException`. Trùng: `BusinessRuleException` |
| 7 | Ngày trả phòng phải sau ngày nhận phòng. Một phòng không bị đặt chồng: chỉ booking Reserved và CheckedIn chặn phòng; hai lần ở chồng nhau khi `checkIn < b.CheckOutDate && b.CheckInDate < checkOut` (nhận phòng đúng ngày khách trước trả phòng thì được). Chỉ hủy được booking Reserved. Reserved→CheckedIn→CheckedOut | Ngày sai: `ValidationException`. Đặt chồng, hủy/nhận/trả sai trạng thái: `BusinessRuleException` |
| 8 | Cộng điểm khi checkout: 1 điểm cho mỗi USD chẵn của `TotalAmount` (= số đêm × `PricePerNight`). Sau khi cộng, lên hạng cao nhất đã đủ `MinPoints`; có thể lên nhiều bậc một lần; không hạ hạng. `AddPoints` nhận số > 0 | Số điểm ≤ 0: `ValidationException` |
| 10 | Không xóa khách còn ticket chưa Closed | `BusinessRuleException` |

| Hạng | `Name` | `SlaFactor` | `PriorityBoost` | `MinPoints` | `Next()` |
|---|---|---|---|---|---|
| `StandardTier` | Standard | 1.0 | 0 | 0 | `SilverTier` |
| `SilverTier` | Silver | 0.9 | 0 | 1000 | `GoldTier` |
| `GoldTier` | Gold | 0.75 | 1 | 5000 | `VipTier` |
| `VipTier` | VIP | 0.5 | 2 | 15000 | `null` |

Trường của khách hàng (UC6): `FullName`, `Phone`, `Email`, `IdentityNumber`, `Nationality` bắt buộc; `Notes` tùy chọn. Thông báo lỗi viết tiếng Anh, đủ rõ để hiện thẳng lên giao diện.

## Các module
Mỗi mục là một module: làm xong thì build, chạy test, dừng, tóm tắt thiết kế và điểm cần nhớ khi vấn đáp, đề xuất commit, chờ duyệt, rồi xuất patch.

### Module 1 — Hạng thành viên và khách hàng (quy tắc 6, 8, 10)
- Bốn lớp hạng: điền `SlaFactor`, `PriorityBoost`, `MinPoints`, `Next()` theo bảng. Không dùng `switch` theo tên hạng ở bất cứ đâu.
- `Customer.ValidateProfile`: `IdentityNumber` hoặc `Nationality` rỗng thì `ValidationException`.
- `Customer.UpdateProfile`: kiểm tra hết rồi mới gán; tên, SĐT, email đổi qua `ChangeContact` của `Person` (đã kiểm tra định dạng).
- `EnsureUnique`, `EnsureCanBeDeleted`: nhận kết quả tra cứu, ném `BusinessRuleException` với thông báo nêu rõ cái gì trùng.
- `AddPoints`: cộng rồi lặp `Tier.Next()` chừng nào còn hạng kế và `Points >= MinPoints` của nó.
- `InteractionHistory.Add`: tóm tắt rỗng thì `ValidationException`; tạo `Interaction`, thêm vào cuối, trả về. `Customer.LogInteraction` gọi `History.Add`.

Test phải xanh (4): cả `CustomerTests`.

### Module 2 — Phòng và booking (quy tắc 7)
- `RoomType.Validate`, `Room.Validate`: trường bắt buộc và số dương, theo chú thích XML.
- `Booking.ValidateNewBooking`: thiếu khách hoặc phòng, hoặc `CheckOutDate <= CheckInDate` thì `ValidationException`.
- `Nights`, `TotalAmount`, `PointsEarned` (làm tròn xuống), `BlocksRoom`.
- `CheckIn`, `CheckOut`, `Cancel`: kiểm tra trạng thái hiện tại rồi đổi.
- `Room.IsAvailable`: chỉ xét booking của chính phòng này và có `BlocksRoom`. `Room.EnsureAvailable`: ném khi `IsAvailable` sai.

Test phải xanh (1): `BookingTests.Constructor_CheckOutNotAfterCheckIn_ThrowsValidation`.

### Module 3 — `CustomerService` và `CustomerDirectory` (UC6, phần dữ liệu của UC9)
- Service không tự `throw` lỗi nghiệp vụ. Tìm theo Id dùng `_customers.GetRequired(id)` (trong `Domain/Interfaces/RepositoryExtensions.cs`).
- `Register`: lấy `NextId()`, tạo `Customer`, tra trùng, `EnsureUnique`, `Save`. Khách không hợp lệ thì không được lưu.
- `Update`: khi tra trùng phải bỏ qua chính khách đang sửa.
- `Delete`: `EnsureCanBeDeleted(_tickets.HasOpenTickets(id))` rồi `_customers.Delete(id)`.
- `Search`: từ khóa rỗng trả hết; so tên, SĐT, email không phân biệt hoa thường; sắp theo tên.
- `GetHistory`: `History.Items` đảo ngược (mới nhất trước).
- `CustomerDirectory`: `Search` và `GetSummary` trả `CustomerSummary(Id, FullName, Phone, Tier.Name)`; `GetSlaProfile` trả `new CustomerSlaProfile(Tier.SlaFactor, Tier.PriorityBoost)`; khách không tồn tại thì để `GetRequired` ném.

Test phải xanh (2): cả `CustomerServiceTests`.

### Module 4 — `BookingService` và `StayLookup` (UC7)
- `Book`: `room.EnsureAvailable(checkIn, checkOut, _bookings.GetAll())` trước, rồi tạo `Booking`, `Save`.
- `CheckIn`: `booking.CheckIn()`; `customer.LogInteraction(ContactChannel.FrontDesk, "Checked in to room 305.", now, null)`; lưu cả booking và khách.
- `CheckOut`: `booking.CheckOut()`; `customer.AddPoints(booking.PointsEarned)`; ghi một tương tác FrontDesk nêu số điểm được cộng; lưu cả hai.
- `GetAvailableRooms`: ngày không hợp lệ thì trả danh sách rỗng, không ném.
- `StayLookup`: lần lưu trú đang diễn ra = booking có `Status == CheckedIn`. `GetRoomNumber` trả chuỗi rỗng khi không có phòng.

Test phải xanh (1): `BookingServiceTests.Book_OverlappingDates_ThrowsBusinessRule`.

### Module 5 — Ba strategy phân công (Strategy pattern)
Chữ ký: `Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)`. `candidates` đã chỉ gồm Agent còn nhận được ticket; danh sách rỗng thì trả `null`.
- `LeastLoadedStrategy`: Agent có `ActiveCount` nhỏ nhất; bằng nhau thì lấy người đứng trước.
- `DepartmentMatchStrategy`: lọc ứng viên có `Agent.Department.Code` bằng `departmentCode` (không phân biệt hoa thường); không còn ai thì dùng toàn bộ; rồi chọn người ít ticket nhất.
- `RoundRobinStrategy`: giữ Id của Agent được chọn lần trước trong một field `private`; chọn ứng viên có Id nhỏ nhất lớn hơn Id đó, không có thì quay về Id nhỏ nhất.

Test phải xanh (1): `AssignmentStrategyTests.LeastLoaded_Pick_ReturnsAgentWithFewestActive`. Tổng cộng 9 test suất B xanh.

### Module 6 — `CustomerListForm` và `CustomerEditForm` (UC6)
Quy ước chung cho các Form:
- Chép cấu trúc `LoginForm`: file `.Designer.cs` chỉ tạo control, gán thuộc tính, đăng ký sự kiện; mọi handler bọc trong `ErrorHandler.Run(() => { ... })`.
- Thời gian truyền xuống service là `DateTime.Now` lấy trong Form. Sau mỗi thao tác, nạp lại dữ liệu từ service.

`CustomerListForm`:
- `txtKeyword`, `btnSearch` → `Search(keyword)`; nạp toàn bộ khi mở.
- `dgvCustomers` chỉ đọc, chọn cả dòng: Id, Full name, Phone, Email, Nationality, Tier, Points.
- `btnNew` mở `CustomerEditForm` trống; `btnEdit` mở `CustomerEditForm` cho dòng đang chọn; `btnDelete` hỏi xác nhận rồi `Delete(id)`; `btnHistory` mở `CustomerHistoryForm` (nội dung Form đó thuộc GĐ 4).

`CustomerEditForm`:
- `txtFullName`, `txtPhone`, `txtEmail`, `txtIdentityNumber`, `txtNationality`, `txtNotes` (nhiều dòng), `btnSave`, `btnCancel`.
- Mở từ menu thì là đăng ký mới → `Register(...)`. Mở để sửa: thêm một hàm public vào Form, ví dụ `LoadCustomer(int customerId)`, nạp bằng `GetById` và khi lưu gọi `Update(...)`. Thêm hàm public vào Form của mình được phép; đổi constructor thì không.

### Module 7 — `BookingForm` (UC7)
- Phần đặt phòng: `txtCustomerKeyword` + `btnFindCustomer` → `CustomerService.Search` đổ vào `cboCustomer`; `dtpCheckIn`, `dtpCheckOut`; `btnFindRooms` → `GetAvailableRooms(...)` đổ vào `cboRoom` (số phòng, loại, giá); `btnBook` → `Book(customerId, roomId, checkIn, checkOut)`.
- `dgvBookings` chỉ đọc: Id, Customer, Room, Room type, Check-in, Check-out, Nights, Total, Status → `GetAll()`.
- `btnCheckIn` → `CheckIn(id, DateTime.Now)`; `btnCheckOut` → `CheckOut(id, DateTime.Now)`; `btnCancelBooking` → `Cancel(id)`.
- Để bấm thử được, thêm vào `src/HotelCRM.WinForms/Data/roomtypes.json` 2 loại phòng và vào `rooms.json` 4 phòng (dữ liệu đầy đủ sẽ thay ở GĐ 4). Định dạng:
  - `roomtypes.json`: `{ "Id": 1, "Name": "Deluxe Double", "PricePerNight": 120, "Capacity": 2 }`
  - `rooms.json`: `{ "Id": 1, "Number": "305", "Floor": 3, "RoomTypeId": 1 }`

### Module 8 — Chốt giai đoạn
- `dotnet build` 0 lỗi 0 warning. `dotnet test` trên `local/b-work`: 22 test; 11 test xanh (2 có sẵn + 9 mới), 11 test `Tickets/` và 2 test `SurveyServiceTests` còn đỏ vì `NotImplementedException`. Test đỏ vì lý do khác là lỗi phải sửa.
- Cập nhật `docs/PROGRESS.md` (file này sửa trên `develop`, không nằm trong patch).
- Báo người dùng mở phiên Opus chạy `/code-review`; sửa lỗi tìm ra.
- Vòng xuất patch đầu tiên chạy đúng thì chép các bước thành `.claude/skills/export-patch/SKILL.md`.

## Xuất patch (khi dùng cách "Nhận patch")
Số `NN` chạy tiếp từ 02 (01 là bộ test, đã xuất ở GĐ 1). Sau mỗi commit trên `local/b-work`:

```
git diff --output=handoff/NN-ten-ngan.patch <commit trước> <commit này>
```

- Dùng `--output`, không dùng `>` của PowerShell (đổi ký tự xuống dòng làm patch hỏng).
- Viết commit message vào `handoff/NN-ten-ngan.msg.txt`, không kèm `Co-Authored-By`.
- Thêm một mục vào `handoff/README.txt`: patch cần gì trước, sau khi áp thì bao nhiêu test xanh.
- Kiểm tra: trên một bản sao sạch ở commit trước, `git apply --check` không in gì.
- Merge commit không xuất patch. Không push `local/b-work`.

## Đề xuất commit (chỉ commit khi người dùng đồng ý)
Mỗi dòng một commit và một patch: (02) bốn hạng thành viên; (03) quy tắc của `Customer` và lịch sử tương tác; (04) `RoomType` và `Room`; (05) `Booking`; (06) `CustomerService`; (07) `CustomerDirectory`; (08) `BookingService`; (09) `StayLookup`; (10) `LeastLoadedStrategy`; (11) `DepartmentMatchStrategy`; (12) `RoundRobinStrategy`; (13) `CustomerListForm` và `CustomerEditForm`; (14) `BookingForm` và dữ liệu phòng tạm.

## Người dùng kiểm tra bằng tay
1. Mở `CustomerListForm`, `CustomerEditForm`, `BookingForm` bằng Designer của Visual Studio: hiển thị được, không báo lỗi.
2. Chạy app, đăng ký một khách; đăng ký khách thứ hai trùng SĐT: hiện cảnh báo, không lưu. Nhập email sai định dạng: hiện cảnh báo.
3. Sửa khách, tìm theo tên, xóa khách: lưới cập nhật đúng.
4. Đặt phòng 3 đêm; đặt chồng ngày cho cùng phòng: hiện cảnh báo. Ngày trả trước ngày nhận: hiện cảnh báo.
5. Check-in, check-out: trạng thái đổi; điểm của khách tăng đúng số đêm × giá. Hủy một booking đã check-in: hiện cảnh báo.
6. Thoát app, mở lại: dữ liệu còn nguyên.

## Điều kiện xong
- Build sạch; 9 test nêu trên xanh; không test nào bị sửa.
- Ba Form mở được trong Designer (người dùng xác nhận).
- Patch 02 trở đi nằm trong `handoff/` kèm `.msg.txt`, `handoff/README.txt` đã cập nhật.
- `docs/PROGRESS.md` đã cập nhật.
