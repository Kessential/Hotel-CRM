# GĐ 2 — Suất A: phân hệ Ticket

Hạn: 20/11/2026. Model: Sonnet 5.5. Nhánh: `feature/a-ticket`, tạo từ `develop` (hỏi người dùng trước khi tạo nhánh). Chạy song song được với GĐ 3.

## Mục tiêu
1. Điền thân hàm của phân hệ Ticket: quy tắc 1–5 và 11, State, Factory Method, phần Subject của Observer.
2. 11 test trong `tests/HotelCRM.Tests/Tickets/` xanh. Không sửa test.
3. Ba Form ticket có nội dung, mở được bằng Designer của Visual Studio.

Khung đã có sẵn từ GĐ 1: mọi lớp, chữ ký và chú thích XML. Việc của giai đoạn này là thay `throw new NotImplementedException();` bằng thân hàm đúng với chú thích XML ngay phía trên nó. Chú thích XML là đặc tả; file này chỉ nói thêm thứ tự làm và những chỗ dễ sai.

## Đọc trước
- `.claude/rules/domain.md`, `business.md`, `winforms.md`, `tests.md`.
- Mã nguồn: `Domain/Tickets/**`, `Domain/Staff/Agent.cs`, `Domain/Common/Employee.cs`, `Domain/Interfaces/**`, `Business/Services/TicketService.cs`, `Business/Factories/**`, `Forms/LoginForm*.cs` (form mẫu), `tests/HotelCRM.Tests/Tickets/**`.
- `docs/PLAN.md`: không cần mở. Mọi con số đã chép ở mục "Quy tắc và con số" bên dưới.

## File được sửa
- `src/HotelCRM.Domain/Tickets/**` (trừ `TicketSnapshot.cs`, `TicketPriority.cs`)
- `src/HotelCRM.Domain/Staff/Agent.cs`: chỉ hai hàm `CanTake` và `EnsureCanTake`
- `src/HotelCRM.Business/Services/TicketService.cs`
- `src/HotelCRM.Business/Queries/TicketQuery.cs`
- `src/HotelCRM.Business/Factories/*TicketFactory.cs` (3 lớp con)
- `src/HotelCRM.WinForms/Forms/Tickets/**`
- `docs/PROGRESS.md`

## File cấm đụng
`Program.cs`, `MainForm*`, `LoginForm*`, `ErrorHandler.cs`, `UiNotificationObserver.cs`, toàn bộ `src/HotelCRM.Data/**`, `Domain/Common/**`, `Domain/Interfaces/**`, `Domain/Customers/**`, `Domain/Hotel/**`, mọi file của suất B trong `Business/` và `Forms/`, mọi test và lớp giả. Không đổi tên, không đổi chữ ký public, không đổi constructor của Form. Thấy cần đổi thì dừng, ghi lý do vào `docs/PROGRESS.md` và hỏi.

## Quy tắc và con số

| # | Quy tắc | Ngoại lệ khi vi phạm |
|---|---|---|
| 1 | Open→InProgress (phải có Agent); InProgress→Resolved (phải có ghi chú); Resolved→Closed; Resolved→InProgress (Reopen); Closed không đổi nữa. Phân công và phân công lại chỉ khi ticket ở Open hoặc InProgress | Chuyển sai trạng thái, chưa có Agent, phân công sai trạng thái: `BusinessRuleException`. Ghi chú rỗng: `ValidationException` |
| 2 | Reopen chỉ khi `now <= ResolvedAt + reopenDays` ngày; ticket về InProgress, giữ nguyên Agent, giữ nguyên `ResolvedAt` | `BusinessRuleException` |
| 3 | Agent nhận thêm được khi `activeCount < maxActive`. Ticket "đang mở" của Agent: đã giao cho Agent đó và ở Open/InProgress | `BusinessRuleException` |
| 4 | Lúc tạo: mức ưu tiên tăng `PriorityBoost` bậc theo thứ tự Low→Medium→High→Urgent, không vượt Urgent. `DueAt = CreatedAt + BaseSlaHours × hệ số ưu tiên × SlaFactor` giờ, tính trên mức ưu tiên sau khi nâng | Là phép tính, không ném lỗi |
| 5 | Ticket bảo trì phải gắn phòng mà khách đang ở (booking CheckedIn) | Không có phòng: `ValidationException`. Phòng không thuộc lần lưu trú đang diễn ra: `BusinessRuleException` |
| 11 | Tạo ticket: cả hai vai trò. Phân công và phân công lại: chỉ Manager. Start và Resolve: chỉ Agent đang được giao ticket đó. Close và Reopen: chỉ Manager | `BusinessRuleException` |

| Con số | Giá trị |
|---|---|
| `BaseSlaHours` | Complaint 24, ServiceRequest 8, Maintenance 12 |
| Hệ số ưu tiên | Low 2.0, Medium 1.0, High 0.5, Urgent 0.25 |
| `HandlingDepartmentCode` | Complaint `GUEST`, ServiceRequest `HOUSE`, Maintenance `ENG` |
| `GoodwillPoints` | mặc định 10 (trong `SupportTicket`), `ComplaintTicket` override thành 50 |
| Ví dụ kiểm tra | Complaint, Medium, boost 1, factor 0.75 → High → 24 × 0.5 × 0.75 = 9 giờ |
| Trễ hạn | `(ResolvedAt ?? now) > DueAt` |
| Số giờ giải quyết | `(ResolvedAt − CreatedAt).TotalHours`; `null` khi `ResolvedAt` là `null` |

Thông báo lỗi viết tiếng Anh, đủ rõ để hiện thẳng lên giao diện, ví dụ `"A ticket that is Closed cannot be resolved."`.

## Các module
Mỗi mục là một module: làm xong thì build, chạy test, dừng, tóm tắt thiết kế và điểm cần nhớ khi vấn đáp, đề xuất commit, chờ duyệt.

### Module 1 — Quy tắc trong Domain (State + quy tắc 1–5, 11)
`Agent`:
- `CanTake`: `activeCount < maxActive`. `EnsureCanTake`: ném khi `CanTake` sai.

Bốn lớp State (`Domain/Tickets/States`):
- `IsActive`: Open và InProgress trả `true`. `IsClosed`: chỉ Closed trả `true`.
- `TicketState` (lớp cha): bốn hàm `Start/Resolve/Close/Reopen` mặc định ném `BusinessRuleException` có tên trạng thái trong thông báo. Lớp con chỉ override hành động hợp lệ; `ClosedState` không override gì.
- `OpenState.Start`: ném nếu `ticket.AssignedAgent` là `null`; rồi `ticket.MoveTo(new InProgressState(), now)`.
- `InProgressState.Resolve`: ghi chú rỗng thì `ValidationException`; `ticket.RecordResolution(note, now)`; `ticket.MoveTo(new ResolvedState(), now)`.
- `ResolvedState.Close`: `ticket.RecordClosure(now)`; `MoveTo(new ClosedState(), now)`.
- `ResolvedState.Reopen`: kiểm tra quy tắc 2; `MoveTo(new InProgressState(), now)`.

`SupportTicket`:
- `ApplyCreationRules`: tiêu đề rỗng thì `ValidationException`; nâng `Priority`; `DueAt = CalculateDueAt(slaProfile.SlaFactor)`.
- `Start/Resolve/Close/Reopen`: chỉ một dòng ủy quyền cho `State`. Không có `if` theo trạng thái trong `SupportTicket`.
- `MoveTo`: giữ trạng thái cũ, gán trạng thái mới, gán `UpdatedAt`, rồi `Notify(cũ, mới)`. Observer chạy sau khi trạng thái đã đổi.
- `Notify`: gọi `OnStateChanged` của từng observer; duyệt trên bản sao danh sách; không bắt ngoại lệ.
- `AssignTo`: `agent` là `null` → `BusinessRuleException` ("No agent is available..."); `State.IsActive` sai → `BusinessRuleException`; `agent.EnsureCanTake(activeCount, maxActive)`; gán `AssignedAgent`. Không đổi trạng thái.
- `EnsureAssignedTo(actor)`: ném khi `AssignedAgent` là `null` hoặc `AssignedAgent.Id != actor.Id`.
- `Validate` (lớp cha): không làm gì. `ComplaintTicket.Validate`: mô tả rỗng thì `ValidationException`. `MaintenanceTicket.Validate`: `roomInActiveStay` sai thì `BusinessRuleException`.
- `MaintenanceTicket.EnsureRoomGiven`: `roomId` là `null` thì `ValidationException`.
- `CalculateDueAt`, `IsOverdue`, `ResolutionHours`, `GoodwillPoints`, `BaseSlaHours`, `HandlingDepartmentCode`: theo bảng con số. Hệ số ưu tiên đặt trong một hàm `private static` của `SupportTicket`.

Test phải xanh sau module này (8): cả 6 test của `TicketStateTests`, 2 test của `TicketRuleTests`.

### Module 2 — Factory Method
Ba lớp `*TicketFactory.Create`: mỗi lớp `new` đúng một lớp ticket với các tham số nhận vào. `MaintenanceTicketFactory` truyền `roomId`; hai lớp kia bỏ qua `roomId`.

Test phải xanh (1): `TicketFactoryTests.Create_EachFactory_ReturnsMatchingSubclass`.

### Module 3 — `TicketService` và `TicketQuery`
Làm đúng các bước ghi trong chú thích XML của từng hàm. Những chỗ dễ sai:
- Service không tự `throw` lỗi nghiệp vụ. Tìm theo Id dùng `_tickets.GetRequired(id)` (hàm mở rộng trong `Domain/Interfaces/RepositoryExtensions.cs`, tự ném `BusinessRuleException` khi không có).
- `CreateTicket`: lấy factory bằng `_factories[typeName]`; không `switch` theo loại, không `new ComplaintTicket(...)`. Gọi `ticket.Validate(...)` trước khi lưu; ticket không hợp lệ thì không được lưu. Gắn observer cho ticket mới (hàm `AttachObservers` có sẵn).
- Đếm ticket đang mở của một Agent: ticket có `AssignedAgent.Id` bằng Id đó và `State.IsActive`; **không tính chính ticket đang được phân công** (để phân công lại cho cùng người không bị tính hai lần).
- `AutoAssign`: danh sách ứng viên chỉ gồm Agent có `CanTake` đúng, giữ thứ tự của repository; gọi `_strategy.Pick(ticket.HandlingDepartmentCode, candidates)`; kết quả `null` vẫn truyền vào `ticket.AssignTo` để Domain ném lỗi. Trả về `ticket.AssignedAgent`.
- `AssignManually`: nhân viên tìm được không phải `Agent` thì truyền `null` vào `AssignTo`.
- Quyền: `actor.EnsureCanAssign()`, `actor.EnsureCanClose()`, `actor.EnsureCanReopen()`, `ticket.EnsureAssignedTo(actor)` gọi trước mọi thay đổi.
- Sau mỗi thay đổi gọi `_tickets.Save(ticket)`.
- `Search`: tiêu chí `null` hoặc rỗng thì không lọc; so `State.Name`, `Priority`, `TypeName`, `AssignedAgent?.Id`, tiêu đề chứa từ khóa không phân biệt hoa thường; sắp theo `CreatedAt` giảm dần.
- `GetTicketTypes`: các khóa của `_factories`. `GetAgents`: `_employees.GetAll().OfType<Agent>()`.
- `TicketQuery`: `HasOpenTickets` = có ticket của khách mà `State.IsClosed` sai; `IsResolvedOrClosed` = ticket tồn tại và `State.IsActive` sai; `GetByCustomer` sắp theo `CreatedAt` tăng dần.

Test phải xanh (2): `TicketServiceTests.CreateTicket_MaintenanceRoomNotInActiveStay_ThrowsBusinessRule`, `Close_ByAgent_ThrowsBusinessRule`. Tổng cộng 11/11.

### Module 4 — `TicketListForm` (UC3, UC5)
Quy ước chung cho cả ba Form:
- Chép cấu trúc `LoginForm`: file `.Designer.cs` chỉ tạo control, gán thuộc tính, đăng ký sự kiện; mọi handler bọc trong `ErrorHandler.Run(() => { ... })`.
- Không gọi service lúc mở Form trừ `GetTicketTypes`, `GetAgents`, `Search`. Các hàm `FindCustomers`, `GetCustomer`, `GetActiveRooms`, `GetRoomNumber` đi sang suất B và còn ném `NotImplementedException` cho tới khi ghép ở GĐ 4: chỉ gọi khi người dùng bấm nút.
- Thời gian truyền xuống service là `DateTime.Now` lấy trong Form. Người thực hiện là `UserSession.Current`.
- Sau mỗi thao tác, nạp lại dữ liệu từ service.

Bố cục `TicketListForm`:
- Hàng lọc: `cboState` (dòng trống + 4 tên trạng thái), `cboPriority` (dòng trống + 4 mức), `cboType` (dòng trống + `GetTicketTypes()`), `cboAgent` (dòng trống + `GetAgents()`), `txtKeyword`, `btnSearch`, `btnClear` → `Search(...)`.
- `dgvTickets` chỉ đọc, chọn cả dòng: Id, Type, Title, Priority, State, Agent, Created, Due, Overdue (`IsOverdue(DateTime.Now)`).
- `btnNew` mở `TicketEditForm`; `btnDetails` mở `TicketDetailForm` cho dòng đang chọn; đóng hộp thoại thì nạp lại lưới.
- Chỉ hiện khi `UserSession.Current.CanAssign()`: `btnAutoAssign` → `AutoAssign(actor, id)`; `cboAssignAgent` + `btnAssign` → `AssignManually(actor, id, agentId)`.

### Module 5 — `TicketEditForm` (UC2)
- `txtCustomerKeyword` + `btnFindCustomer` → `FindCustomers(keyword)` đổ vào `cboCustomer` (hiện `FullName`, giá trị `Id`).
- `cboType` (`GetTicketTypes()`), `cboPriority`, `txtTitle`, `txtDescription` (nhiều dòng).
- `cboRoom`: chỉ bật khi loại là `MaintenanceTicket.TypeKey`; nút `btnLoadRooms` → `GetActiveRooms(customerId)` (Id phòng → số phòng).
- `btnSave` → `CreateTicket(type, customerId, title, description, priority, roomId, DateTime.Now)`; báo số ticket, mức ưu tiên sau khi nâng và hạn xử lý; đóng với `DialogResult.OK`. `btnCancel`.

### Module 6 — `TicketDetailForm` (UC4)
- Mở từ menu thì trống: `numTicketId` + `btnLoad` → `GetById(id)`. Mở từ `TicketListForm` thì nạp sẵn qua một hàm public mới của Form, ví dụ `ShowTicket(int ticketId)` (thêm hàm public vào Form của mình được phép; đổi constructor thì không).
- Phần chỉ đọc: Type, Title, Description, Priority, State, Agent, Created, Due, Resolved, Closed, Resolution note; phòng (chỉ với `MaintenanceTicket`).
- `txtNote` + `btnResolve`; `btnStart`; `btnClose`; `btnReopen`. `btnStart`/`btnResolve` hiện khi người dùng là Agent; `btnClose`/`btnReopen` hiện khi `CanClose()`/`CanReopen()`. Ẩn nút chỉ để tiện; quyền thật do tầng dưới kiểm tra.
- Gọi `Start(actor, id, now)`, `Resolve(actor, id, note, now)`, `Close(actor, id, now)`, `Reopen(actor, id, now)`; xong thì nạp lại.

### Module 7 — Chốt giai đoạn
- `dotnet build` 0 lỗi 0 warning; `dotnet test` trên `feature/a-ticket`: 11 test, 11 xanh.
- Cập nhật `docs/PROGRESS.md`.
- Báo người dùng mở phiên Opus chạy `/code-review` trên nhánh; sửa lỗi tìm ra rồi mới đề nghị merge `feature/a-ticket` vào `develop`.

## Đề xuất commit (chỉ commit khi người dùng đồng ý, không kèm `Co-Authored-By`)
Trên `feature/a-ticket`, mỗi dòng một commit: (1) giới hạn ticket của Agent; (2) bốn lớp State; (3) quy tắc tạo ticket, hạn xử lý; (4) phân công và kiểm tra quyền trên ticket; (5) ba factory; (6) `TicketService` tạo và tìm ticket; (7) `TicketService` phân công và xử lý; (8) `TicketQuery`; (9) `TicketListForm`; (10) `TicketEditForm`; (11) `TicketDetailForm`; (12) cập nhật `PROGRESS.md`.

## Người dùng kiểm tra bằng tay
Dữ liệu khách hàng còn trống và code suất B chưa có, nên ở giai đoạn này chỉ kiểm tra được:
1. Mở `TicketListForm`, `TicketEditForm`, `TicketDetailForm` bằng Designer của Visual Studio: hiển thị được, không báo lỗi.
2. Chạy app, đăng nhập, mở ba Form từ menu Tickets: không có thông báo lỗi lúc mở. Bấm Search trên danh sách rỗng: không lỗi.
3. Đăng nhập bằng Agent: không thấy nút Auto Assign và Assign.

Kịch bản UC2–UC5 đầy đủ được bấm thử ở điểm duyệt chung đầu GĐ 4, sau khi ghép với suất B.

## Điều kiện xong
- Build sạch; 11/11 test của `Tickets/` xanh; không test nào bị sửa.
- Ba Form mở được trong Designer (người dùng xác nhận).
- `docs/PROGRESS.md` đã cập nhật; `feature/a-ticket` đã merge vào `develop` sau khi người dùng đồng ý.
