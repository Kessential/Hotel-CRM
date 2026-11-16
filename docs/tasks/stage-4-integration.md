# GĐ 4 — Ghép nối và phần còn lại của suất B

Hạn: 25/11/2026. Model: Sonnet 5.5 (Module 1–7 và 9); Haiku 5.5 trong một phiên riêng cho Module 8 (bảy file dữ liệu mẫu). Nhánh: `local/b-work`, sau khi đã merge `develop` có suất A.

Điều kiện bắt đầu: GĐ 2 đã merge vào `develop` (11 test `Tickets/` xanh) và GĐ 3 đã xong trên `local/b-work` (9 test suất B xanh). Chưa đủ thì dừng và báo người dùng.

## Mục tiêu
1. Hai phân hệ chạy chung: UC1–UC7 bấm được trên giao diện.
2. Phần còn lại của suất B: khảo sát (UC8), ba observer, lịch sử tương tác (UC9), báo cáo (UC10).
3. 22/22 test xanh. Không sửa test.
4. Dữ liệu mẫu đầy đủ, liên thông; bản publish chạy được.
5. Có `docs/tasks/stage-5-docs.md`; thành viên B đã push toàn bộ code.

## Đọc trước
- `docs/PROGRESS.md` (cách commit của suất B, số patch tiếp theo), `docs/tasks/stage-3-customer.md` mục "Xuất patch".
- `.claude/rules/*.md`.
- Mã nguồn: `Domain/Customers/SatisfactionSurvey.cs`, `Business/Services/SurveyService.cs`, `ReportService.cs`, `AgentPerformanceRow.cs`, `DepartmentReportRow.cs`, `Business/Observers/**`, `UiNotificationObserver.cs`, `Domain/Interfaces/ITicketQuery.cs`, `ITicketObserver.cs`, `src/HotelCRM.Data/Records/**` (cho dữ liệu mẫu).
- `docs/PLAN.md`: mục "Dữ liệu mẫu liên thông", "Git", "Đóng gói", "Kiểm tra (Verification)".

## File được sửa
- `src/HotelCRM.Domain/Customers/SatisfactionSurvey.cs`
- `src/HotelCRM.Business/Services/SurveyService.cs`, `ReportService.cs`
- `src/HotelCRM.Business/Observers/InteractionLogger.cs`, `LoyaltyObserver.cs`
- `src/HotelCRM.WinForms/UiNotificationObserver.cs`
- `src/HotelCRM.WinForms/Forms/Customers/SurveyForm*`, `CustomerHistoryForm*`, `CustomerListForm*` (chỉ để nút History nạp sẵn khách); `Forms/Reports/ReportForm*`
- `src/HotelCRM.WinForms/Data/*.json`
- `README.md`: chỉ bảng "Sample accounts"
- `docs/PROGRESS.md`, `docs/tasks/stage-5-docs.md`, `handoff/**`

## File cấm đụng
`Program.cs`, `MainForm*`, `LoginForm*`, `ErrorHandler.cs`, toàn bộ `src/HotelCRM.Data/**`, mọi file của suất A (`Domain/Tickets/**`, `TicketService.cs`, `TicketQuery.cs`, `Business/Factories/**`, `Forms/Tickets/**`), mọi chữ ký public, mọi constructor của Form, mọi test và lớp giả. Thấy lỗi trong code suất A thì dừng, ghi vào `docs/PROGRESS.md` và hỏi: lỗi đó sửa trên `develop`, không sửa trong patch của suất B.

## Quy tắc và con số

| # | Quy tắc | Ngoại lệ khi vi phạm |
|---|---|---|
| 8 | Khi ticket chuyển sang Closed, khách được cộng `ticket.GoodwillPoints` (10, complaint 50) và có thể lên hạng | Là phép tính, không ném lỗi |
| 9 | Khảo sát chỉ cho ticket Resolved hoặc Closed; điểm từ 1 đến 5; mỗi ticket một lần | Điểm ngoài 1–5: `ValidationException`. Sai trạng thái hoặc đã khảo sát: `BusinessRuleException` |

| Định nghĩa báo cáo | Cách tính |
|---|---|
| Ticket trễ hạn | `ticket.IsOverdue(now)` |
| Số giờ giải quyết | `ticket.ResolutionHours`; chỉ tính ticket có giá trị (đã từng Resolved) |
| Phòng ban của ticket | phòng ban của `ticket.AssignedAgent`; ticket chưa giao không tính |
| Khảo sát của một Agent / phòng ban | khảo sát có `TicketId` thuộc ticket đang giao cho Agent đó / Agent của phòng ban đó |
| Trung bình khi không có dữ liệu | `null` (lưới để trống), không phải 0 |

## Các module
Mỗi mục là một module: làm xong thì build, chạy test, dừng, tóm tắt, đề xuất commit, chờ duyệt, rồi xuất patch (số `NN` chạy tiếp từ GĐ 3).

### Module 1 — Merge và điểm duyệt chung (UC1–UC7)
- `git checkout local/b-work`, `git merge develop`. Xung đột chỉ có thể nằm ở `docs/PROGRESS.md` hoặc `handoff/`; xung đột ở file code nghĩa là có suất đã sửa file cấm: dừng và báo.
- `dotnet build` sạch; `dotnet test`: 22 test, 20 xanh, 2 test `SurveyServiceTests` đỏ vì `NotImplementedException`.
- Merge commit không xuất patch. Ghi vào `handoff/README.txt`: từ patch này trở đi, thành viên B phải `git merge origin/develop` vào `feature/b-customer` trước khi áp.
- **Dừng để người dùng bấm thử** (dùng dữ liệu phòng tạm của GĐ 3): đăng ký khách → đặt phòng → check-in → Manager tạo ticket bảo trì cho phòng đó → Auto Assign → Agent đăng nhập, Start, Resolve → Manager Close → tìm ticket theo trạng thái. Ba observer lúc này còn thân rỗng nên lịch sử và điểm chưa đổi: đó là đúng. Lỗi ghép nối tìm ra ở đây phải xử lý xong trước khi làm Module 2.

### Module 2 — Khảo sát (UC8, quy tắc 9)
- `SatisfactionSurvey.EnsureAllowed`: điểm ngoài `MinScore`–`MaxScore` thì `ValidationException`; `ticketResolvedOrClosed` sai hoặc `alreadySurveyed` đúng thì `BusinessRuleException`.
- `SurveyService.Submit`: tra `_tickets.IsResolvedOrClosed(ticketId)` và xem repository đã có khảo sát của ticket chưa; `NextId()`; `SatisfactionSurvey.Create(...)` quyết định; `Save`. Service không tự `throw`.
- `GetSurveyableTickets(customerId)`: ticket của khách (`GetByCustomer`) mà `IsResolvedOrClosed` đúng và chưa có khảo sát. `FindByTicket`.
- `SurveyForm`: `txtCustomerKeyword` + `btnFindCustomer` → `CustomerService.Search` đổ vào `cboCustomer`; `btnLoadTickets` → `GetSurveyableTickets` đổ vào `cboTicket` (số và tiêu đề); `numScore` (1–5), `txtComment`; `btnSubmit` → `Submit(ticketId, score, comment, DateTime.Now)`; `btnCancel`.

Test phải xanh (2): cả `SurveyServiceTests`. Tổng cộng 22/22.

### Module 3 — Ba observer (Observer pattern, quy tắc 8)
Thay thân rỗng của `OnStateChanged`. Observer chạy sau khi trạng thái đã đổi, tự lưu thứ mình sửa, không bắt ngoại lệ.
- `InteractionLogger`: tìm khách bằng `_customers.GetRequired(ticket.CustomerId)`; `LogInteraction(ContactChannel.System, <tóm tắt nêu số ticket, tiêu đề, from.Name và to.Name>, ticket.UpdatedAt, ticket.Id)`; `_customers.Save(customer)`.
- `LoyaltyObserver`: chỉ khi `to.IsClosed`: `customer.AddPoints(ticket.GoodwillPoints)`; `Save`. Không so tên trạng thái bằng chuỗi, không `is ClosedState`.
- `UiNotificationObserver`: ghép câu thông báo rồi gọi `Publish(...)`; `MainForm` đã nghe sẵn sự kiện và hiện lên thanh trạng thái.

Không có test tự động cho module này; kiểm tra bằng tay ở cuối giai đoạn.

### Module 4 — Lịch sử tương tác (UC9)
- `CustomerHistoryForm`: mở từ menu thì chọn khách (`txtCustomerKeyword`, `btnFindCustomer`, `cboCustomer`, `btnLoad`); thêm hàm public, ví dụ `ShowCustomer(int customerId)`, để `CustomerListForm` mở sẵn khách đang chọn.
- Phần trên: tên, hạng, điểm (`GetById`). `dgvHistory` chỉ đọc: Time, Channel, Summary, Ticket → `GetHistory(customerId)`.
- Sửa `CustomerListForm`: nút History gọi `ShowCustomer` trước khi `ShowDialog`.

### Module 5 — Báo cáo (UC10)
- `ReportService.AgentPerformance(now)`: mỗi Agent một `AgentPerformanceRow`, sắp theo tên; Agent chưa có ticket vẫn có dòng với số 0 và trung bình `null`.
- `ReportService.ByDepartment()`: mỗi phòng ban một `DepartmentReportRow`, sắp theo tên; đếm theo `State.Name`.
- Chỉ đọc ticket qua `_tickets.GetAll()` (`ITicketQuery`); không dùng `TicketService`.
- `ReportForm`: `tabReports` hai trang, mỗi trang một `DataGridView` chỉ đọc; `btnRefresh`. Số giờ và điểm trung bình hiện một chữ số thập phân.

### Module 6 — Rà ghép nối
- Chạy lại kịch bản của Module 1: lịch sử có dòng cho từng lần đổi trạng thái, có dòng check-in/check-out; Close cộng điểm; thanh trạng thái của `MainForm` hiện thông báo.
- Báo người dùng mở phiên Opus chạy `/code-review`; sửa lỗi tìm ra.

### Module 7 — `employees.json` đầy đủ
- 2 manager, 6 agent, chia đủ 4 phòng ban (`GUEST`, `HOUSE`, `ENG`, `FRONT`); giữ nguyên ba tài khoản đang có (`manager1`, `agent1`, `agent2`) và Id của họ.
- Sinh `PasswordHash` và `Salt` bằng `dotnet run tools/HashPassword.cs -- <mật khẩu>`; không gõ tay, không chép lại chuỗi của tài khoản khác.
- Cập nhật bảng "Sample accounts" trong `README.md`. Không nêu mật khẩu trong tin nhắn trả lời người dùng: chỉ vào README.

### Module 8 — Bảy file dữ liệu còn lại (phiên Haiku riêng)
Giao cho phiên Haiku câu: "Đọc `CLAUDE.md`, `docs/PROGRESS.md` và mục Module 8 của `docs/tasks/stage-4-integration.md` rồi làm theo." Mỗi file một commit và một patch.

Số lượng: 4 phòng ban (giữ nguyên `departments.json`), 5 loại phòng, 20 phòng, 15 khách (đủ 4 hạng), khoảng 20 booking (đủ 4 trạng thái), khoảng 25 ticket (đủ 3 loại × 4 trạng thái), khoảng 10 khảo sát. Toàn bộ chữ bằng tiếng Anh. Thời gian viết dạng `2026-11-20T09:30:00`, nằm trong tháng 10–11/2026.

Định dạng từng file (mảng các đối tượng, tên trường đúng như sau):

| File | Một phần tử |
|---|---|
| `roomtypes.json` | `{ "Id": 1, "Name": "Deluxe Double", "PricePerNight": 120, "Capacity": 2 }` |
| `rooms.json` | `{ "Id": 1, "Number": "305", "Floor": 3, "RoomTypeId": 1 }` |
| `customers.json` | `{ "Id": 1, "FullName": "...", "Phone": "0912345678", "Email": "...", "IdentityNumber": "...", "Nationality": "...", "Notes": "", "Tier": "Gold", "Points": 5200, "Interactions": [ { "OccurredAt": "...", "Channel": "FrontDesk", "Summary": "...", "TicketId": null } ] }` |
| `bookings.json` | `{ "Id": 1, "CustomerId": 1, "RoomId": 1, "CheckInDate": "2026-11-01T00:00:00", "CheckOutDate": "2026-11-05T00:00:00", "Status": "CheckedIn" }` |
| `tickets.json` | `{ "Id": 1, "Type": "Maintenance", "CustomerId": 1, "Title": "...", "Description": "...", "Priority": "High", "CreatedAt": "...", "DueAt": "...", "UpdatedAt": "...", "State": "InProgress", "AssignedAgentId": 3, "ResolvedAt": null, "ResolutionNote": null, "ClosedAt": null, "RoomId": 1 }` |
| `surveys.json` | `{ "Id": 1, "TicketId": 2, "Score": 4, "Comment": "...", "SubmittedAt": "..." }` |

Giá trị hợp lệ: `Tier` ∈ Standard, Silver, Gold, VIP; `Channel` ∈ FrontDesk, Phone, Email, System; `Status` ∈ Reserved, CheckedIn, CheckedOut, Cancelled; `Type` ∈ Complaint, ServiceRequest, Maintenance; `Priority` ∈ Low, Medium, High, Urgent; `State` ∈ Open, InProgress, Resolved, Closed.

Ràng buộc phải đúng (app nạp dữ liệu mà không chạy lại quy tắc, nên dữ liệu sai sẽ hiện sai):
- Mọi Id tham chiếu phải tồn tại; `AssignedAgentId` phải là Agent, không phải Manager.
- SĐT 10 chữ số bắt đầu bằng 0; email hợp lệ; SĐT và email của khách không trùng nhau.
- `Tier` khớp `Points`: Standard < 1000 ≤ Silver < 5000 ≤ Gold < 15000 ≤ VIP.
- Booking: `CheckOutDate` sau `CheckInDate`; cùng một phòng không có hai booking Reserved/CheckedIn chồng ngày.
- Ticket: `DueAt = CreatedAt + BaseSlaHours × hệ số ưu tiên × SlaFactor của hạng khách` (24/8/12 giờ; Low 2.0, Medium 1.0, High 0.5, Urgent 0.25; Standard 1.0, Silver 0.9, Gold 0.75, VIP 0.5). `Priority` là mức đã nâng.
- Ticket theo trạng thái: Open có thể chưa có Agent; InProgress, Resolved, Closed phải có Agent; Resolved và Closed có `ResolvedAt` và `ResolutionNote`; chỉ Closed có `ClosedAt`; `UpdatedAt` là thời điểm đổi trạng thái gần nhất.
- Ticket Maintenance có `RoomId` thuộc một booking CheckedIn hoặc CheckedOut của đúng khách đó; hai loại kia để `RoomId` là `null`.
- Không Agent nào giữ quá 5 ticket Open/InProgress.
- Khảo sát chỉ cho ticket Resolved/Closed, mỗi ticket tối đa một, `Score` từ 1 đến 5.
- `Interactions` của khách khớp với booking và ticket của khách đó; `TicketId` trong tương tác phải tồn tại.

Kiểm tra: `dotnet build`, chạy app, đăng nhập, mở từng Form và báo cáo: không có hộp thoại lỗi.

### Module 9 — Publish thử và chốt giai đoạn
- `dotnet publish src/HotelCRM.WinForms -c Release -r win-x64 --self-contained -p:PublishSingleFile=true`; trong thư mục publish phải có file `.exe`, `appsettings.json` và thư mục `Data/` nằm ngoài file `.exe`.
- Người dùng chép thư mục publish sang ổ khác và chạy trọn kịch bản ở mục "Người dùng kiểm tra bằng tay".
- `dotnet test`: 22/22 xanh. Cập nhật `docs/PROGRESS.md`.
- Thành viên B áp hết patch và push `feature/b-customer`; chủ repo kiểm tra `git diff local/b-work origin/feature/b-customer -- src tests` rỗng rồi merge vào `develop`.
- Viết `docs/tasks/stage-5-docs.md` (UML PlantUML, README, `demo-script.md`, `viva-notes.md`) theo mục "Lộ trình" và "Design Pattern" của `docs/PLAN.md`.

## Đề xuất commit (chỉ commit khi người dùng đồng ý, không kèm `Co-Authored-By`)
Mỗi dòng một commit và một patch: quy tắc khảo sát trong `SatisfactionSurvey`; `SurveyService`; `SurveyForm`; `InteractionLogger`; `LoyaltyObserver`; `UiNotificationObserver`; `CustomerHistoryForm`; `ReportService.AgentPerformance`; `ReportService.ByDepartment`; `ReportForm`; `employees.json` và bảng tài khoản; rồi mỗi file dữ liệu một commit.

## Người dùng kiểm tra bằng tay
1. Mở `SurveyForm`, `CustomerHistoryForm`, `ReportForm` bằng Designer: hiển thị được.
2. Kịch bản đầu-cuối: Manager đăng nhập → tạo ticket bảo trì cho một khách VIP đang ở → Auto Assign → Agent đăng nhập → Start → Resolve → ghi khảo sát → Manager Close → xem lịch sử tương tác và điểm của khách → xem hai báo cáo.
3. Thử vi phạm từng quy tắc 1–11 trên giao diện: mỗi lần hiện đúng một cảnh báo, app không thoát. Quy tắc 4 và 8 là phép tính: kiểm tra hạn xử lý và điểm/hạng hiển thị đúng.
4. Đổi `DefaultAssignmentStrategy` trong `appsettings.json` cạnh file chạy sang `DepartmentMatch` rồi `RoundRobin`: cách chọn Agent đổi theo mà không build lại.
5. Chạy bản publish ở ổ hoặc máy khác với dữ liệu mẫu.

## Điều kiện xong
- Build sạch; 22/22 test xanh; không test nào bị sửa.
- Bản publish chạy được trọn kịch bản (người dùng xác nhận).
- Thành viên B đã push toàn bộ; `feature/b-customer` đã merge vào `develop`.
- `docs/tasks/stage-5-docs.md` đã có; `docs/PROGRESS.md` đã cập nhật.
