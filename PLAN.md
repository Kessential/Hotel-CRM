# Kế hoạch BTL OOP — Hotel CRM (bản hoàn chỉnh, thay thế plan `clever-fiddle`)

> **Dành cho phiên làm việc sau:** đây là đặc tả đủ để bắt đầu code. Repo `D:\Project\Hotel-CRM` (nhánh `main`, chỉ có `README.md`, có remote `origin`). C# / .NET 10, WinForms, lưu JSON, test xUnit. Tên lớp/hàm trong tài liệu này phải xuất hiện đúng trong code vì báo cáo và UML đối chiếu 1-1. Phiên này **chỉ lưu plan, chưa code**; bản chính dùng cho mọi phiên sau là `docs/PLAN.md` trong repo.

## Context
BTL môn LTHĐT, nhóm 2 người, đề "Quản lý dịch vụ chăm sóc khách hàng (CRM)" áp dụng cho khách sạn; gợi ý lớp: `Customer`, `SupportTicket`, `TicketState (Open, InProgress, Resolved)`, `Agent`, `InteractionHistory`. Chấm theo sheet **"Nhóm 02 người"** của `C:\Users\Khanh\Downloads\Checklist_BTL_LTHDT_v2.xlsx`: 20 tiêu chí × 1 điểm (A=1 / B=0.75 / C=0.5 / D=0.25 / F=0), quy về thang 10, kèm "Bảng mô tả chi tiết thiết kế & code" 5 dòng (Module 1.0, Module 1.1, Design Pattern 1, Design Pattern 2, Kiến trúc đóng gói).

## Kết quả rà soát plan cũ
Plan cũ đã chép đúng cả 20 tiêu chí và vượt mọi ngưỡng tối thiểu. Các điểm còn thiếu/chưa ổn, đã bổ sung trong bản này:

| # | Vấn đề ở plan cũ | Tiêu chí bị ảnh hưởng | Cách xử lý |
|---|---|---|---|
| 1 | Phân vai "A code chính, B phần nhỏ hơn" trái với yêu cầu "chia đều 2 thành viên" | 1, 7, 9, 12, 18, 19 | Chia đôi thật: 5 UC, 5 quy tắc, 2 sơ đồ, 11 test, ≥ 12 commit mỗi người; cách commit chờ 2 thành viên chốt (mục Git) |
| 2 | Không có bảng UC → hàm điều khiển | 1 | Thêm bảng ánh xạ UC → Form → Service method |
| 3 | Không nêu interface ghép 2 phân hệ | 6, 16 | Thêm 4 interface ranh giới + bảng quan hệ UML |
| 4 | Quy tắc nghiệp vụ chưa gắn với phương thức cụ thể | 3 | Thêm cột "cài đặt tại" |
| 5 | "Module 1.0 / 1.1" chỉ ghi chú rời rạc | Bảng mô tả thiết kế | Định nghĩa rõ 1.0 và 1.1 |
| 6 | Chưa tính chuyện lưu JSON cho lớp trừu tượng, private setter, State, Observer | 13, 16, 17 | Thêm tầng Record + Mapper trong Data |
| 7 | Chưa có đăng nhập/băm mật khẩu, xử lý ngoại lệ toàn cục, coding convention | 4, 15, 19 | Thêm `AuthService`, `PasswordHasher`, `ErrorHandler`, `.editorconfig` |
| 8 | Tài liệu: báo cáo chưa rõ định dạng | 20 | Báo cáo LaTeX theo mẫu bạn đưa sau; thêm tài liệu ôn vấn đáp |

## Quyết định đã chốt (từ phần hỏi đáp)
- Bạn (cùng Claude) viết **cả hai phân hệ**. **Phân công giữa 2 thành viên chưa chốt** — xem mục "Hướng phân công (tạm thời)"; bạn và thành viên còn lại sẽ bàn rồi cập nhật sau.
- Thời hạn **2 tuần** (16/11 → 29/11/2026). Không có ràng buộc thêm từ giảng viên.
- IDE **Visual Studio** → mỗi Form có `.Designer.cs` chuẩn, mở được bằng Designer.
- Lưu **JSON**; giữ **10 Use Case**; tác nhân **Manager + Agent**.
- **Toàn bộ tiếng Anh**: tên lớp/hàm, comment, giao diện, dữ liệu mẫu, thông báo lỗi. (Báo cáo và tài liệu ôn vấn đáp viết tiếng Việt.)
- Mật khẩu **băm có salt** (PBKDF2-SHA256 qua `Rfc2898DeriveBytes.Pbkdf2`, có sẵn trong .NET).
- Tài liệu Claude làm: UML PlantUML, báo cáo **LaTeX** (chờ mẫu), README + kịch bản demo, tài liệu ôn vấn đáp.
- Cách làm: **xong mỗi module thì dừng**, tóm tắt thiết kế + điểm cần nhớ khi vấn đáp, bạn duyệt rồi đi tiếp.

## Hướng phân công (tạm thời — chờ 2 thành viên chốt)
Ký hiệu **A** và **B** trong toàn bộ tài liệu này là hai *suất công việc*, chưa gắn với người cụ thể. Mọi cột "Người" ở các bảng bên dưới là đề xuất, đổi được mà không ảnh hưởng thiết kế.

| | Suất A — phân hệ Ticket | Suất B — phân hệ Khách hàng/Lưu trú |
|---|---|---|
| Use Case | UC1–UC5 | UC6–UC10 |
| Quy tắc nghiệp vụ | 1–5 | 6–10 |
| Design Pattern | State, Strategy, Factory Method | Observer, Repository + Singleton |
| Sơ đồ tuần tự | UC2, UC4 | UC7, UC8 |
| Test | 11 (`Tests/Tickets/`) | 11 (`Tests/Customers/`) |
| Thư mục chính | `Domain/Tickets`, `Business/Assignment`, `Business/Factories`, `Forms/Tickets` | `Domain/Customers`, `Domain/Hotel`, `Business/Observers`, `Data/`, `Forms/Customers`, `Forms/Bookings`, `Forms/Reports` |
| Giai đoạn code | GĐ 2 (18–20/11) | GĐ 3 (21–23/11) + phần khảo sát, lịch sử, báo cáo, observer của GĐ 4 (24–25/11) |
| Tài liệu riêng | sơ đồ use case; sơ đồ State/Strategy/Factory; chương báo cáo về phân hệ Ticket; README; `demo-script.md` | class diagram tổng thể; sơ đồ Observer/Repository; chương báo cáo về phân hệ Khách hàng/Lưu trú; `viva-notes.md` |

Phần chung, không tính cho suất nào: khung GĐ 1, quy tắc 11 (phân quyền), `ErrorHandler`, đóng gói, các chương mở đầu/kiến trúc/kết luận của báo cáo, cột "Minh chứng" của checklist.

Nguyên tắc giữ cố định dù ai nhận suất nào:
- Hai suất cân bằng về số lượng (checklist yêu cầu "chia đều" ở tiêu chí 1, 7, 9, 18, 19).
- Ranh giới theo thư mục/namespace, hai suất chỉ chạm nhau qua 4 interface ở mục "Ghép 2 phân hệ" → đổi người hoặc đổi cách chia không phải sửa code.
- Phần khung chung (giai đoạn 1: `Common/`, `Interfaces/`, `AppConfig`, đăng nhập, `MainForm`) không thuộc suất nào; khi chốt sẽ quyết định ai commit.

Cần chốt trước 17/11 (trước khi bắt đầu giai đoạn 2): ai nhận suất nào, có muốn hoán đổi UC/pattern nào giữa hai suất không, và cách commit (mục Git).

## Đối chiếu ngưỡng tối thiểu
| Tiêu chí | Tối thiểu | Kế hoạch |
|---|---|---|
| Use Case | 8, chia đều | 10 (A 5, B 5) |
| Lớp thực thể | 15 | 26 |
| Quy tắc nghiệp vụ | 8, cả 2 phân hệ | 11 (A 5, B 5, chung 1) |
| Class Diagram | 16 lớp | toàn bộ Domain + Business |
| Sơ đồ tuần tự | 4 (2/người) | 4 |
| Design Pattern | 3 (1–2/người) | 5 (A: State, Strategy, Factory Method; B: Observer, Repository + Singleton) |
| Test | 20 (10/người) | 22 (11/người) |
| Git | nhánh, 20 commit chia đều | `main`/`develop`/`feature/a-*`/`feature/b-*`, ≥ 12 commit/người |

## Kiến trúc (3 tầng + Domain, 5 project)
```
HotelCRM.sln   .editorconfig   .gitignore   README.md
├─ src/HotelCRM.Domain/        # thực thể, State, quy tắc, interface — không tham chiếu project nào
│   ├─ Common/      Person, Employee, DomainException (+ ValidationException, BusinessRuleException), BusinessSettings
│   ├─ Customers/   Customer, MembershipTier + 4 tier, Interaction, InteractionHistory, SatisfactionSurvey
│   ├─ Hotel/       RoomType, Room, Booking, BookingStatus
│   ├─ Staff/       Agent, Manager, Department
│   ├─ Tickets/     SupportTicket + 3 lớp con, TicketPriority, TicketCategory, States/ (TicketState + 4 state)
│   └─ Interfaces/  IEntity, IRepository<T>, ITicketObserver, ICustomerDirectory, IStayLookup, ITicketQuery
├─ src/HotelCRM.Business/      # tầng nghiệp vụ
│   ├─ Services/    AuthService, TicketService, CustomerService, BookingService, SurveyService, ReportService
│   ├─ Queries/     CustomerDirectory, StayLookup, TicketQuery   # cài đặt 3 interface ranh giới, chỉ dựa trên repository
│   ├─ Assignment/  IAssignmentStrategy + 3 strategy
│   ├─ Factories/   TicketFactory + 3 factory
│   ├─ Observers/   InteractionLogger, LoyaltyObserver
│   └─ Security/    PasswordHasher, UserSession
├─ src/HotelCRM.Data/          # tầng dữ liệu
│   ├─ AppConfig.cs (Singleton)   JsonRepository.cs   InMemoryRepository.cs
│   └─ Records/ + Mappers/     # DTO phẳng để ghi JSON + chuyển đổi sang thực thể
├─ src/HotelCRM.WinForms/      # tầng giao diện (net10.0-windows)
│   ├─ Program.cs (composition root)   ErrorHandler.cs   UiNotificationObserver.cs
│   ├─ Forms/ LoginForm, MainForm, Tickets/*, Customers/*, Bookings/*, Reports/*
│   └─ appsettings.json   Data/*.json (copy ra thư mục output)
└─ tests/HotelCRM.Tests/       # xUnit: Tickets/ (A), Customers/ (B)
docs/  uml/*.puml + ảnh, report/ (LaTeX), demo-script.md, viva-notes.md
```
Chiều gọi: `WinForms → Business → Domain ← Data`. Form chỉ gọi Service; Service chỉ biết `IRepository<T>`; `Program.cs` tạo `AppConfig`, repository, observer, strategy, service rồi truyền qua constructor (inject thủ công, không dùng thư viện DI).

**Lưu JSON:** thực thể giữ `private set` và validate nên không serialize trực tiếp. `JsonRepository<TEntity, TRecord> : IRepository<TEntity>` ghi/đọc `TRecord` (DTO phẳng) qua `IMapper<TEntity, TRecord>`; trạng thái ticket và hạng thành viên lưu bằng tên rồi dựng lại (`TicketState.FromName`, `MembershipTier.FromName`). Observer không lưu xuống file: `TicketService` gắn lại observer mỗi khi nạp/tạo ticket.

## Lớp thực thể (26 lớp + 5 enum)
- **Người (5):** `Person` (abstract: Id, FullName, Phone, Email) → `Customer`; `Employee` (abstract: Username, PasswordHash, Salt, `Role`, abstract `CanReassign()`/`CanClose()`) → `Agent`, `Manager`
- **Tổ chức (1):** `Department`
- **Hạng thành viên (5):** `MembershipTier` (abstract: `Name`, `SlaFactor`, `PriorityBoost`, `MinPoints`, `Next()`) → `StandardTier`, `SilverTier`, `GoldTier`, `VipTier`
- **Khách sạn (3):** `RoomType`, `Room`, `Booking` (+ enum `BookingStatus`: Reserved, CheckedIn, CheckedOut, Cancelled)
- **Ticket (4):** `SupportTicket` (abstract) → `ComplaintTicket`, `ServiceRequestTicket`, `MaintenanceTicket` (+ enum `TicketPriority`, `TicketCategory`)
- **State (5):** `TicketState` (abstract) → `OpenState`, `InProgressState`, `ResolvedState`, `ClosedState`
- **Tương tác (3):** `InteractionHistory`, `Interaction` (+ enum `ContactChannel`), `SatisfactionSurvey`

**Module 1.0 (baseline):** `Person`/`Customer`/`Employee`/`Agent`, `SupportTicket` + `TicketState` 3 trạng thái của đề (Open, InProgress, Resolved), `InteractionHistory`, `IRepository<T>`.
**Module 1.1 (mở rộng bằng kế thừa/đa hình):** 3 lớp con ticket (override `BaseSlaHours`, `Validate()`), `ClosedState` + `Reopen()`, cây `MembershipTier`, `Manager` (override quyền). Báo cáo trình bày 1.1 là phần thêm vào mà không sửa lớp của 1.0.

## Quan hệ UML phải có trong code (tiêu chí 6)
| Quan hệ | Ví dụ |
|---|---|
| Kế thừa | `Customer`→`Person`; `Agent`,`Manager`→`Employee`; 3 ticket→`SupportTicket`; 4 state→`TicketState`; 4 tier→`MembershipTier` |
| Thực thi | `JsonRepository`→`IRepository<T>`; strategy→`IAssignmentStrategy`; observer→`ITicketObserver`; `CustomerDirectory`→`ICustomerDirectory`; `StayLookup`→`IStayLookup`; `TicketQuery`→`ITicketQuery` |
| Hợp thành | `Customer` ◆— `InteractionHistory` ◆— `Interaction`; `SupportTicket` ◆— `TicketState` |
| Kết tập | `Department` ◇— `Agent`; `RoomType` ◇— `Room`; `SupportTicket` ◇— `Agent` (người được giao) |
| Phụ thuộc | `TicketService` ⇢ `IAssignmentStrategy`, `TicketFactory`; Form ⇢ Service |

**Ghép 2 phân hệ qua interface (tiêu chí 16):**
- Phân hệ Ticket (A) cần dữ liệu của B qua `ICustomerDirectory` (tìm khách, lấy hạng) và `IStayLookup` (phòng khách đang ở — quy tắc 5).
- Phân hệ Khách hàng (B) cần dữ liệu của A qua `ITicketQuery` (khách còn ticket mở không — quy tắc 10; trạng thái ticket — quy tắc 9).
- B phản ứng với sự kiện của A qua `ITicketObserver` (ghi lịch sử, cộng điểm).
Không service nào tham chiếu trực tiếp lớp service của phân hệ kia. Ba interface truy vấn do lớp riêng trong `Business/Queries` cài đặt (không phải service) để tránh phụ thuộc vòng khi khởi tạo.

## Design Pattern
| Pattern | Người | Vai trò lớp | Mở rộng làm tại chỗ khi vấn đáp |
|---|---|---|---|
| **State** | A | Context `SupportTicket`; State `TicketState`; Concrete 4 state. `Start()`, `Resolve(note)`, `Close()`, `Reopen()` ủy quyền cho `_state` | thêm `OnHoldState` |
| **Strategy** | A | `IAssignmentStrategy.Pick(ticket, agents)` → `LeastLoadedStrategy`, `DepartmentMatchStrategy`, `RoundRobinStrategy`; Context `TicketService.AutoAssign()`; chọn theo `appsettings.json` | thêm `SkillBasedStrategy` |
| **Factory Method** | A | Creator `TicketFactory.Create(...)` → `ComplaintTicketFactory`, `ServiceRequestTicketFactory`, `MaintenanceTicketFactory` | thêm `LostItemTicket` + factory |
| **Observer** | B | Subject `SupportTicket` (`Attach`/`Notify`); `ITicketObserver.OnStateChanged(ticket, from, to)` → `InteractionLogger`, `LoyaltyObserver`, `UiNotificationObserver` | thêm `EmailNotifier` |
| **Repository + Singleton** | B | `IRepository<T>` → `JsonRepository`, `InMemoryRepository` (dùng cho test); `AppConfig.Instance` | thêm repository lưu CSV |

Bảng mô tả thiết kế trong báo cáo: Design Pattern 1 = State, Design Pattern 2 = Observer (mỗi người một mẫu chính); ba mẫu còn lại trình bày ở mục riêng.

## Quy tắc nghiệp vụ → nơi cài đặt
| # | Người | Quy tắc | Cài đặt tại |
|---|---|---|---|
| 1 | A | Chuyển trạng thái hợp lệ: Open→InProgress (phải có Agent), InProgress→Resolved (phải có ghi chú), Resolved→Closed/Reopen; Closed bất biến | các lớp `*State` |
| 2 | A | Chỉ Reopen trong `ReopenDays` ngày sau Resolved | `SupportTicket.Reopen(now, reopenDays)` → `ResolvedState.Reopen()` |
| 3 | A | Agent không giữ quá `MaxActiveTickets` ticket đang mở | `Agent.EnsureCanTake(activeCount, maxActive)`, gọi từ `SupportTicket.AssignTo(agent, activeCount, maxActive)`; `TicketService` đếm `activeCount` từ repository |
| 4 | A | Hạn SLA = `BaseSlaHours` (theo loại ticket) × hệ số Priority × `SlaFactor` của hạng; hạng cao tự nâng Priority (số cụ thể ở mục "Quyết định thiết kế bổ sung") | `SupportTicket.CalculateDueAt()`, `MembershipTier.Boost()` |
| 5 | A | `MaintenanceTicket` phải gắn phòng khách đang ở (CheckedIn) | `MaintenanceTicket.Validate()` + `IStayLookup` |
| 6 | B | SĐT/Email đúng định dạng, không trùng | `Person` setter; `CustomerService.Register()` |
| 7 | B | CheckOut > CheckIn; phòng không bị đặt chồng | `Booking` constructor; `Room.IsAvailable()` |
| 8 | B | Cộng điểm khi checkout; tự nâng hạng (Silver 1000 / Gold 5000 / VIP 15000) | `Customer.AddPoints()`, `MembershipTier.Next()` |
| 9 | B | Khảo sát chỉ cho ticket Resolved/Closed, điểm 1–5, mỗi ticket một lần | `SatisfactionSurvey` constructor; `SurveyService.Submit()` |
| 10 | B | Không xóa khách còn ticket chưa đóng | `CustomerService.Delete()` + `ITicketQuery` |
| 11 | Chung | Agent chỉ thao tác ticket được giao cho mình; chỉ Manager phân công lại và đóng ticket | `Employee.CanReassign()/CanClose()` (khai báo ở GĐ 1, `Agent`/`Manager` override); `TicketService` kiểm tra trước mỗi thao tác |

Vi phạm ném `ValidationException` hoặc `BusinessRuleException` (con của `DomainException`); quy tắc nào ném loại nào xem mục "Quyết định thiết kế bổ sung".

## Use Case → hàm điều khiển (tiêu chí 1)
| UC | Người | Tác nhân | Form | Hàm điều khiển |
|---|---|---|---|---|
| UC1 Log in | A | Manager, Agent | `LoginForm` | `AuthService.Login()` |
| UC2 Create ticket | A | Manager, Agent | `TicketEditForm` | `TicketService.CreateTicket()` |
| UC3 Assign ticket | A | Manager | `TicketListForm` | `TicketService.AutoAssign()` / `AssignManually()` |
| UC4 Process ticket | A | Agent, Manager | `TicketDetailForm` | `TicketService.Start()` / `Resolve()` / `Close()` / `Reopen()` |
| UC5 Search tickets | A | Manager, Agent | `TicketListForm` | `TicketService.Search()` |
| UC6 Manage customers | B | Manager, Agent | `CustomerListForm`, `CustomerEditForm` | `CustomerService.Register()` / `Update()` / `Delete()` |
| UC7 Manage bookings | B | Manager, Agent | `BookingForm` | `BookingService.Book()` / `CheckIn()` / `CheckOut()` |
| UC8 Record survey | B | Agent | `SurveyForm` | `SurveyService.Submit()` |
| UC9 View interaction history | B | Manager, Agent | `CustomerHistoryForm` | `CustomerService.GetHistory()` |
| UC10 Reports | B | Manager | `ReportForm` | `ReportService.AgentPerformance()` / `ByDepartment()` |

`MainForm` ẩn/hiện menu theo `UserSession.Current.Role`. Sơ đồ tuần tự: A vẽ UC2 và UC4, B vẽ UC7 và UC8.

## Ngoại lệ, cấu hình, quy ước
- `ErrorHandler.Run(Action)` bọc mọi event handler của Form: `DomainException` → hộp thoại cảnh báo; lỗi khác → ghi `logs/error.log` + thông báo chung. `Program.cs` đăng ký thêm `Application.ThreadException`.
- `appsettings.json`: `DataFolder`, `DefaultAssignmentStrategy`, `MaxActiveTickets` (mặc định 5), `ReopenDays` (mặc định 7). Ngưỡng điểm, giờ SLA và hệ số hạng là hằng số trong các lớp Domain (thể hiện đa hình), không nằm trong cấu hình. Đường dẫn tính từ `AppContext.BaseDirectory`, không gán cứng.
- `.editorconfig`: PascalCase cho public, `_camelCase` cho field private, file-scoped namespace, mỗi file một lớp, bật nullable.

## Quyết định thiết kế bổ sung (để giao được cho model con)

**1. Khởi tạo không phụ thuộc vòng.** Thứ tự trong `Program.cs`: `AppConfig` → repository → `CustomerDirectory`, `StayLookup`, `TicketQuery` → observer → strategy → service → form. Mỗi kiểu thực thể có đúng một repository, nạp file một lần vào bộ nhớ và luôn trả về cùng một đối tượng. `TicketService` gắn observer cho mọi ticket trong repository lúc khởi tạo và cho ticket mới trong `CreateTicket()`.

**2. Cấu hình đi xuống Domain.** `BusinessSettings` (`Domain/Common`: `MaxActiveTickets`, `ReopenDays`, tự kiểm tra > 0). `AppConfig.Instance.Rules` trả về `BusinessSettings`; `Program.cs` truyền nó vào constructor của service. Lớp Domain không đọc cấu hình và không gọi `DateTime.Now`: giá trị giới hạn và thời điểm `now` đi vào qua tham số hàm. `AppConfig` chỉ được gọi trong `Program.cs`.

**3. Con số nghiệp vụ.**
| Mục | Giá trị |
|---|---|
| `BaseSlaHours` (override theo loại ticket) | Complaint 24, ServiceRequest 8, Maintenance 12 |
| Hệ số Priority | Low 2.0, Medium 1.0, High 0.5, Urgent 0.25 |
| `SlaFactor` theo hạng | Standard 1.0, Silver 0.9, Gold 0.75, VIP 0.5 |
| `PriorityBoost` (số bậc nâng, trần là Urgent) | Standard 0, Silver 0, Gold 1, VIP 2 |
| Hạn xử lý | `DueAt = CreatedAt + BaseSlaHours × hệ số Priority (sau khi nâng) × SlaFactor` giờ. Ví dụ: Complaint, Medium, khách Gold → High → 24 × 0.5 × 0.75 = 9 giờ |
| Điểm khi checkout | 1 điểm cho 1 USD; `Booking.TotalAmount` = số đêm × `RoomType.PricePerNight` |
| Điểm khi ticket Closed | `SupportTicket.GoodwillPoints`: mặc định 10, `ComplaintTicket` override thành 50 |
| `MinPoints` của hạng | Standard 0, Silver 1000, Gold 5000, VIP 15000; chỉ nâng, không hạ |
| Ticket "đang mở" | Quy tắc 3: đã giao cho Agent và ở Open/InProgress. Quy tắc 10: mọi ticket chưa Closed |
| `ReportService.AgentPerformance()` | mỗi Agent: số ticket được giao, số đã giải quyết, số giờ giải quyết trung bình, số ticket trễ hạn, điểm khảo sát trung bình |
| `ReportService.ByDepartment()` | mỗi phòng ban: số ticket theo trạng thái, số giờ giải quyết trung bình, điểm khảo sát trung bình |

**4. Loại ngoại lệ theo quy tắc.**
- `ValidationException` (dữ liệu của một đối tượng không hợp lệ): sai định dạng SĐT/Email (6), CheckOut ≤ CheckIn (7), điểm khảo sát ngoài 1–5 (9), ghi chú giải quyết rỗng (1), trường bắt buộc bỏ trống.
- `BusinessRuleException` (vi phạm quy tắc giữa các đối tượng): chuyển trạng thái sai hoặc chưa có Agent (1), Reopen quá hạn (2), vượt `MaxActiveTickets` (3), phòng không thuộc booking đang ở (5), trùng SĐT/Email (6), đặt chồng phòng (7), khảo sát sai trạng thái hoặc lần hai (9), xóa khách còn ticket (10), thao tác sai quyền (11).
- Quy tắc 4 và 8 là phép tính, không ném lỗi.

**5. Lưu trữ.**
- Mọi thực thể cài `IEntity { int Id }`; repository cấp Id kế tiếp, service lấy Id rồi mới tạo thực thể.
- File trong `Data/`: `departments.json`, `employees.json`, `roomtypes.json`, `rooms.json`, `customers.json`, `bookings.json`, `tickets.json`, `surveys.json`; nạp theo đúng thứ tự này.
- Tên trường trong record trùng tên thuộc tính của thực thể (PascalCase); enum ghi dạng chuỗi; thời gian ISO 8601.
- Trường phân biệt lớp con: `tickets.json` có `Type` (Complaint/ServiceRequest/Maintenance) và `State`; `employees.json` có `Role` (Agent/Manager), dùng chung `IRepository<Employee>`; `customers.json` có `Tier`, `Points` và mảng `Interactions` lồng bên trong.
- Tham chiếu: trong cùng phân hệ giữ đối tượng (`SupportTicket.AssignedAgent`, `Room.RoomType`, `Booking.Room`, `Booking.Customer`, `Employee.Department`); khác phân hệ giữ Id (`SupportTicket.CustomerId`, `MaintenanceTicket.RoomId`, `SatisfactionSurvey.TicketId`) và tra qua interface ranh giới. Mapper cần tham chiếu thì nhận repository tương ứng qua constructor.
- Chuỗi băm mật khẩu trong `employees.json` phải sinh bằng `PasswordHasher` (chạy một lần qua tiện ích seed), không gõ tay.

## Kiểm thử (22 test xUnit, dùng `InMemoryRepository`)
- **A – `Tests/Tickets/` (11):** chuyển trạng thái hợp lệ ×3, chuyển sai ném lỗi ×2, Reopen quá hạn, vượt `MaxActiveTickets`, SLA theo hạng, bảo trì sai phòng, `LeastLoadedStrategy` chọn đúng, factory tạo đúng lớp con.
- **B – `Tests/Customers/` (11):** email/SĐT sai ×2, trùng SĐT, đặt chồng phòng, CheckOut ≤ CheckIn, cộng điểm, nâng hạng ×2, khảo sát sai trạng thái, khảo sát lần hai, xóa khách còn ticket mở.

## Dữ liệu mẫu liên thông (`Data/*.json`)
4 phòng ban, 2 manager, 6 agent, 5 loại phòng, 20 phòng, 15 khách (đủ 4 hạng), ~20 booking (đủ 4 trạng thái), ~25 ticket (đủ 3 loại × 4 trạng thái), lịch sử tương tác và khảo sát khớp id với nhau. Tài khoản mẫu ghi trong README.

## Git (cách chia commit chờ chốt cùng phân công)
- Nhánh: `main` (bản ổn định) ← `develop` ← `feature/a-*` và `feature/b-*`.
- Mục tiêu ≥ 12 commit mỗi người, commit nhỏ theo từng lớp/quy tắc/test.
- Giai đoạn 1 (khung chung) commit bằng tài khoản của bạn trên `develop`, không phụ thuộc phân công. Các commit này làm số commit của bạn nhiều hơn; người còn lại bù bằng commit dữ liệu mẫu đầy đủ, tài liệu riêng của suất mình và các bài mở rộng pattern, sao cho `git shortlog -sn` cuối kỳ lệch không quá 3 commit.
- Hướng đề xuất cho suất của thành viên còn lại: Claude viết code trên nhánh `feature/*` của suất đó nhưng **không commit**; mỗi bước xuất một file patch đánh số kèm commit message gợi ý vào `handoff/` (git-ignore). Người đó kéo repo về, `git apply` từng patch, tự commit và push bằng tài khoản của mình; bạn merge vào `develop`. Phương án khác nếu hai bạn muốn: người đó tự code suất của mình, Claude chỉ hỗ trợ suất của bạn.
- Lưu ý: tiêu chí 20 yêu cầu cả hai hiểu code của nhau, nên cả hai cần đọc `docs/viva-notes.md` và tự làm lại các bài mở rộng pattern trước buổi vấn đáp.

## Lộ trình
Tổng 14 ngày, 6 giai đoạn; mỗi giai đoạn kết thúc bằng một điểm dừng để bạn duyệt.

| GĐ | Ngày | Nội dung | Điểm dừng để duyệt |
|---|---|---|---|
| **1 Khung** | 16–17/11 | Solution + 5 project, `.editorconfig`, `.gitignore`, nhánh `develop`; `Person`/`Employee`/`DomainException`; `IRepository<T>`, `JsonRepository`, `InMemoryRepository`, `AppConfig`; `AuthService` + `PasswordHasher`; dữ liệu mẫu tối thiểu; `LoginForm`, `MainForm`; **khung Domain + interface của GĐ 2–3** (chữ ký đầy đủ, thân hàm `throw new NotImplementedException()`) | Chạy app, đăng nhập 2 vai trò; solution build được với khung Domain |
| **2 Suất A** | 18–20/11 | Ticket + 4 State + 3 Factory + 3 Strategy + `TicketService`; `TicketListForm`, `TicketEditForm`, `TicketDetailForm`; 11 test | UC1–UC5 chạy được |
| **3 Suất B** | 21–23/11 | `Customer`, cây `MembershipTier`, `Room`/`Booking`, `CustomerService`, `BookingService`; form khách hàng + đặt phòng; test phần B | UC6–UC7 chạy được |
| **4 Ghép nối** | 24–25/11 | Thuộc suất B: Observer (lịch sử, điểm thưởng, thông báo UI), khảo sát (UC8), lịch sử (UC9), báo cáo (UC10), dữ liệu mẫu đầy đủ. Phần chung: nối 2 phân hệ qua 4 interface, `ErrorHandler`, đủ 22 test, publish thử | Chạy trọn kịch bản demo trên bản publish |
| **5 Tài liệu kỹ thuật** | 26–27/11 | UML PlantUML (use case, class, 4 sequence, sơ đồ từng pattern); README; `demo-script.md`; `viva-notes.md` + làm thử 5 bài mở rộng pattern | Bộ sơ đồ khớp code |
| **6 Báo cáo & nộp** | 28–29/11 | Báo cáo LaTeX theo mẫu; điền cột "Minh chứng" (trang X–Y) của checklist; kiểm tra bản publish trên máy khác; tập vấn đáp | Bản nộp |

**Mốc phụ thuộc vào bạn:** phân công chính thức trước **17/11** (để GĐ 2–3 commit đúng người); mẫu báo cáo LaTeX trước **26/11**.

**Thứ tự cắt giảm nếu trễ tiến độ** (vẫn giữ đủ mọi ngưỡng tối thiểu của checklist): (1) `RoundRobinStrategy` — còn 2 strategy; (2) `ReportService.ByDepartment()` — còn báo cáo hiệu suất Agent; (3) `UiNotificationObserver` — còn 2 observer; (4) giảm dữ liệu mẫu còn một nửa; (5) sơ đồ riêng từng pattern gộp vào class diagram tổng thể. Không cắt: 10 UC, 11 quy tắc, 22 test, 5 pattern, 4 sơ đồ tuần tự.

## Tổ chức phiên làm việc và model
Tài liệu dùng chung cho mọi phiên (không soạn skill ở giai đoạn đầu):
- `CLAUDE.md` ở gốc repo, tối đa khoảng 80 dòng: lệnh build/test/run/publish, luật kiến trúc, quy ước đặt tên và ngôn ngữ, luật làm việc (trả lời bằng tiếng Việt, dừng sau mỗi module, không tự commit, code suất B xuất patch), và trỏ tới hai file dưới. Không chép nội dung plan vào đây.
- `docs/PLAN.md`: đặc tả (file này).
- `docs/PROGRESS.md`: giai đoạn đã xong, việc đang dở, quyết định lệch khỏi plan. Mỗi phiên đọc đầu tiên và cập nhật trước khi kết thúc.

| Việc | Model | Phiên đó đọc | Kết quả |
|---|---|---|---|
| GĐ 0 Chuẩn bị (16/11) | Fable/Opus | toàn bộ plan | Tạo `CLAUDE.md`, `docs/PROGRESS.md`; commit cùng `docs/PLAN.md` |
| GĐ 1 Khung + khung Domain | Fable/Opus | toàn bộ plan | Solution chạy được; mọi lớp/interface của GĐ 2–3 có chữ ký |
| GĐ 2 Suất A | Sonnet | `CLAUDE.md`, `PROGRESS.md`, các mục Design Pattern, quy tắc 1–5, UC1–UC5, test A; code khung | Thân hàm, 3 form ticket, 11 test |
| GĐ 3 Suất B | Sonnet | như trên với quy tắc 6–10, UC6–UC10, test B | Thân hàm, form khách hàng/đặt phòng, test B |
| GĐ 4 Ghép nối | Sonnet; dữ liệu mẫu đầy đủ giao Haiku | mục Ghép 2 phân hệ, Lưu trữ, Dữ liệu mẫu | Kịch bản demo chạy trên bản publish |
| Review cuối GĐ 2, 3, 4 | Fable/Opus | diff của giai đoạn | `/code-review`, danh sách lỗi cần sửa |
| GĐ 5 Tài liệu kỹ thuật | Sonnet (UML, `viva-notes.md`); Haiku (README, `demo-script.md`) | code + bảng trong plan | Sơ đồ khớp code |
| GĐ 6 Báo cáo | Sonnet | mẫu LaTeX, plan, UML | Báo cáo + cột minh chứng |

Luật cho mọi phiên:
- Mỗi giai đoạn một phiên mới; không kéo dài phiên cũ.
- Phiên Sonnet/Haiku không đổi tên lớp, chữ ký public hay cấu trúc thư mục của khung. Cần đổi thì dừng, ghi lý do vào `PROGRESS.md` để phiên model mạnh quyết định.
- Điều kiện xong: `dotnet build` không warning, `dotnet test` pass, `PROGRESS.md` đã cập nhật.
- GĐ 2 và GĐ 3 có thể chạy song song, mỗi phiên một worktree; hai phiên không sửa chung file.

## Đóng gói
`dotnet publish src/HotelCRM.WinForms -c Release -r win-x64 --self-contained -p:PublishSingleFile=true` → một file `.exe` kèm `appsettings.json` và thư mục `Data/`.

## Kiểm tra (Verification)
- `dotnet build` không lỗi, không warning; `dotnet test` đủ 22 test pass.
- Kịch bản đầu-cuối: Manager đăng nhập → tạo ticket bảo trì cho khách VIP đang ở → tự phân công → Agent đăng nhập → Start → Resolve → khảo sát → Manager Close → xem lịch sử tương tác và điểm thưởng tự cập nhật → xem báo cáo.
- Thử vi phạm từng quy tắc 1–11 trên giao diện, mỗi lần phải hiện đúng thông báo, không crash.
- Copy thư mục publish sang ổ/máy khác, chạy được với dữ liệu mẫu.
- Đổi `DefaultAssignmentStrategy` trong `appsettings.json`, hành vi phân công đổi mà không build lại.
- `git shortlog -sn` cho thấy commit chia đều 2 tác giả.
- Đối chiếu tên lớp/hàm trong UML và báo cáo với code.

## Tiêu chí checklist → minh chứng
| Tiêu chí | Minh chứng |
|---|---|
| 1 UC | bảng "Use Case → hàm điều khiển", sơ đồ use case |
| 2 Thực thể | 26 lớp trong `HotelCRM.Domain`; bảng thuộc tính trong báo cáo |
| 3 Quy tắc | bảng "Quy tắc → nơi cài đặt" + test tương ứng |
| 4 Tách UI/nghiệp vụ | `WinForms` chỉ gọi `Business/Services`; menu theo `UserSession` |
| 5 Class diagram | `docs/uml/class-overview.puml` |
| 6 Quan hệ | bảng "Quan hệ UML" |
| 7 Sơ đồ tuần tự | `docs/uml/seq-uc2|uc4|uc7|uc8.puml` |
| 8 Phân tầng | 4 project `src/` + chiều tham chiếu project |
| 9–12 Pattern | bảng Design Pattern, sơ đồ từng pattern, 5 bài mở rộng trong `viva-notes.md` |
| 13 Đóng gói/trừu tượng | `private set`, validate trong constructor/method, lớp abstract |
| 14 Kế thừa/đa hình | 5 cây kế thừa, override, test dùng lớp con qua kiểu cha |
| 15 SRP/ISP/convention | mỗi service một nhóm UC; interface 1–4 hàm; `.editorconfig` |
| 16 Ghép qua interface | `ICustomerDirectory`, `IStayLookup`, `ITicketQuery`, `ITicketObserver` |
| 17 Đóng gói | lệnh publish single-file, đường dẫn tương đối |
| 18 Dữ liệu + test | `Data/*.json`, 22 test xUnit |
| 19 Ngoại lệ/cấu hình/Git | `ErrorHandler`, `appsettings.json`, nhánh + `git shortlog` |
| 20 Tài liệu | báo cáo LaTeX, README, `demo-script.md`, `viva-notes.md` |

## Mức độ đầy đủ của plan
Plan bao trọn vòng đời dự án: phân tích → thiết kế → code → test → đóng gói → tài liệu → nộp. Những phần **cố ý chưa chi tiết**, sẽ làm rõ ngay trong giai đoạn tương ứng:
- Danh sách thuộc tính/chữ ký hàm của từng lớp (model mạnh chốt bằng code khung ở GĐ 1, rồi chép vào UML và báo cáo).
- Bố cục từng Form.
- Dàn ý báo cáo (phụ thuộc mẫu LaTeX).
- Phân công chính thức và cách commit.
- Slide thuyết trình: chưa nằm trong phạm vi; nếu cần thì thêm vào giai đoạn 6.

## Việc còn chờ bạn
- Mẫu báo cáo LaTeX (cần trước 26/11).
- **Phân công chính thức** giữa 2 thành viên (ai nhận suất A/B, có hoán đổi gì không) và cách commit — cần trước 17/11; khi có, cập nhật mục "Hướng phân công" và các cột "Người".
- Tên + tài khoản GitHub của thành viên còn lại.
- Kiểm tra máy có Java (để xuất ảnh PlantUML cục bộ) và bản TeX (để biên dịch báo cáo); thiếu thì dùng Overleaf/extension của bạn.

## Bản tóm tắt gửi thành viên còn lại (`docs/TEAM-BRIEF.md`)
PLAN.md là đặc tả kỹ thuật cho các phiên Claude, không gửi nguyên văn. Người còn lại nhận `docs/TEAM-BRIEF.md`: dài khoảng một trang, tiếng Việt, viết ở ngôi "mình – bạn" từ phía người gửi, không có đường dẫn máy cá nhân, không nhắc plan cũ hay tên model. PLAN.md đính kèm làm phụ lục cho ai muốn đọc sâu.

Nội dung theo thứ tự:
1. **Dự án:** đề bài, bối cảnh khách sạn, cách chấm (20 tiêu chí, các mục "chia đều 2 thành viên").
2. **Đã chọn sẵn, cần bạn đồng ý:** C# .NET 10 + WinForms, lưu JSON, giao diện và code toàn tiếng Anh, 10 UC, 5 pattern, hạn 16–29/11.
3. **Hai suất công việc:** bảng rút gọn từ mục "Hướng phân công" (UC, quy tắc, pattern, sơ đồ, test, tài liệu riêng, giai đoạn code) và phần chung.
4. **Cách làm việc:** nêu rõ code cả hai suất do người gửi làm cùng Claude; suất của bạn được xuất thành patch đánh số để bạn đọc, áp, tự commit và push bằng tài khoản của mình. Phương án thay thế: bạn tự code suất của mình theo đặc tả. Nhấn mạnh tiêu chí 12 và 20: khi vấn đáp cả hai phải giải thích và mở rộng được code ngay tại chỗ.
5. **Việc của bạn theo ngày:**

| Ngày | Việc |
|---|---|
| 16–17/11 | Cài môi trường, nhận quyền repo, clone, build thử; trả lời các câu ở mục 7 |
| 18–20/11 (nếu nhận suất A) hoặc 21–25/11 (nếu nhận suất B) | Áp từng patch, đọc hiểu, commit (≥ 12 commit), chạy thử các UC của suất mình |
| 26–27/11 | Đọc `viva-notes.md`; tự làm lại các bài mở rộng pattern của suất mình và ít nhất một bài của suất kia |
| 28–29/11 | Đọc báo cáo, chạy bản publish trên máy mình, tập vấn đáp chung |

6. **Chuẩn bị máy:** .NET 10 SDK, Visual Studio có workload ".NET desktop development", Git, tài khoản GitHub.
7. **Cần bạn trả lời trước 17/11:** (1) nhận suất A hay B; (2) có muốn đổi UC/pattern nào giữa hai suất; (3) chọn cách commit: nhận patch hay tự code; (4) tên tài khoản GitHub; (5) có mẫu báo cáo LaTeX của giảng viên không; (6) những ngày không làm được trong 16–29/11; (7) có phản đối quyết định nào ở mục 2.

## Ngay sau khi plan được duyệt (phiên này)
1. Tạo `D:\Project\Hotel-CRM\docs\TEAM-BRIEF.md` theo dàn ý ở mục trên.
2. Ghi đè `D:\Project\Hotel-CRM\docs\PLAN.md` bằng nguyên văn plan này (đã sửa: tách quy tắc 11, chia lại tài liệu, ghi rõ suất của GĐ 4, cân bằng commit).
3. Không commit, không viết code, không tạo `CLAUDE.md`.

## Bước đầu tiên ở phiên sau
GĐ 0 rồi GĐ 1 theo bảng "Tổ chức phiên làm việc và model": tạo `CLAUDE.md` và `docs/PROGRESS.md`, commit tài liệu, dựng khung đúng mục Kiến trúc kèm khung Domain, build và chạy thử `LoginForm` → `MainForm`, commit trên `develop`, rồi dừng lại tóm tắt để bạn duyệt.
