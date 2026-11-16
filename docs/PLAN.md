# Kế hoạch BTL OOP — Hotel CRM

> **Đặc tả chính của dự án.** File này (`docs/PLAN.md`) tự đủ, không phụ thuộc tài liệu nào khác. C# / .NET 10, WinForms, lưu JSON, test xUnit. Tên lớp/hàm trong tài liệu này phải xuất hiện đúng trong code vì báo cáo và UML đối chiếu 1-1. Mỗi phiên làm việc đọc `CLAUDE.md` → `docs/PROGRESS.md` → file task của giai đoạn, và chỉ mở những mục của file này mà file task nêu tên.

## Context
BTL môn LTHĐT, nhóm 2 người, đề "Quản lý dịch vụ chăm sóc khách hàng (CRM)" áp dụng cho khách sạn; gợi ý lớp: `Customer`, `SupportTicket`, `TicketState (Open, InProgress, Resolved)`, `Agent`, `InteractionHistory`. Chấm theo sheet **"Nhóm 02 người"** trong file checklist của giảng viên; toàn bộ nội dung sheet đã chép vào mục "Checklist chấm điểm" bên dưới.

## Checklist chấm điểm (sheet "Nhóm 02 người")
20 tiêu chí, mỗi tiêu chí 1 điểm, chấm theo mức A = 1 / B = 0.75 / C = 0.5 / D = 0.25 / F = 0; tổng 20 điểm quy về thang 10. Báo cáo phải điền cột "Minh chứng" (trang X–Y) cho từng tiêu chí.

| # | Nhóm | Yêu cầu | Minh chứng trong dự án |
|---|---|---|---|
| 1 | Phân tích & Code | Xác định bài toán, tác nhân, đặc tả chức năng chính (≥ 8 Use Case chia đều 2 thành viên) khớp với các hàm điều khiển trong code | bảng "Use Case → hàm điều khiển", sơ đồ use case |
| 2 | | Hệ thống lớp thực thể dùng chung (≥ 15 lớp), giải thích được ý nghĩa từng thuộc tính từ code | 26 lớp trong `HotelCRM.Domain`; bảng thuộc tính trong báo cáo |
| 3 | | Đặc tả quy tắc nghiệp vụ (≥ 8) và cài đặt trực tiếp vào phương thức của các lớp ở cả hai phân hệ | bảng "Quy tắc nghiệp vụ → nơi cài đặt" + test tương ứng |
| 4 | | Tách biệt lớp giao diện với lớp xử lý nghiệp vụ theo đúng phân quyền tác nhân và ranh giới hệ thống | `WinForms` chỉ gọi `Business/Services`; menu theo `UserSession` |
| 5 | Thiết kế & Code | Sơ đồ lớp tổng thể (≥ 16 lớp) đồng bộ với khai báo lớp, thuộc tính, phương thức trong code của cả nhóm | `docs/uml/class-overview.puml` |
| 6 | | Thiết kế và cài đặt đúng các quan hệ (kế thừa, thực thi, kết tập, hợp thành, phụ thuộc), đặc biệt giữa 2 phân hệ | bảng "Quan hệ UML" |
| 7 | | Sơ đồ tuần tự hoặc hoạt động (≥ 4, mỗi thành viên 2) khớp thứ tự gọi hàm thực tế khi chạy | `docs/uml/seq-uc2`, `seq-uc4`, `seq-uc7`, `seq-uc8` |
| 8 | | Kiến trúc phân tầng (≥ 3 tầng), phân chia module rõ ràng, khớp cấu trúc package và chiều gọi lệnh | 4 project trong `src/` + chiều tham chiếu project |
| 9 | Design Pattern & Code | Lựa chọn và giải thích lý do áp dụng mẫu thiết kế (≥ 3, mỗi thành viên 1–2 mẫu) | bảng Design Pattern; chương pattern trong báo cáo |
| 10 | | Xác định vai trò từng lớp trong các Design Pattern; cả 2 thành viên đối chiếu đúng với lớp, giao diện trong code | sơ đồ từng pattern |
| 11 | | Cài đặt đúng cơ chế phối hợp giữa các lớp trong Design Pattern (đa hình, ủy quyền, khởi tạo) xuyên suốt chương trình | code State/Strategy/Factory/Observer/Repository + test |
| 12 | | Đánh giá hiệu quả Design Pattern; cả 2 thành viên thực hành mở rộng thêm chức năng/lớp mới ngay trên code | 5 bài mở rộng trong `viva-notes.md` |
| 13 | Nguyên lý LTHĐT & Code | Đóng gói và trừu tượng hóa toàn dự án: che giấu thuộc tính (private), kiểm tra dữ liệu hợp lệ khi giao tiếp giữa các lớp | `private set`, validate trong constructor/method, lớp abstract |
| 14 | | Kế thừa và đa hình: lớp cơ sở/giao diện dùng chung, ghi đè phương thức, lớp con thay thế được lớp cha | 5 cây kế thừa, override, test dùng lớp con qua kiểu cha |
| 15 | | Mỗi lớp một nhiệm vụ rõ ràng, interface tinh gọn, thống nhất Coding Convention giữa 2 thành viên | mỗi service một nhóm UC; interface 1–5 hàm; `.editorconfig` |
| 16 | | Ghép nối công việc 2 thành viên qua interface, dễ mở rộng hoặc thay đổi cài đặt mà không làm lỗi code của nhau | `ICustomerDirectory`, `IStayLookup`, `ITicketQuery`, `ITicketObserver`, `IAssignmentStrategy` |
| 17 | Đóng gói | Tích hợp mã nguồn 2 thành viên, đóng gói thành 1 tệp thực thi hoàn chỉnh, đường dẫn dữ liệu tương đối, chạy được trên máy khác | lệnh publish single-file, đường dẫn theo `AppContext.BaseDirectory` |
| 18 | | Dữ liệu mẫu liên thông và ca kiểm thử (≥ 20 test case chia đều 2 thành viên) cho các phương thức xử lý chính | `Data/*.json`, 22 test xUnit |
| 19 | | Xử lý ngoại lệ (try-catch) thống nhất, tách file cấu hình, làm việc nhóm trên Git có phân nhánh (≥ 20 commit chia đều 2 thành viên) | `ErrorHandler`, `appsettings.json`, nhánh + `git shortlog -sn` |
| 20 | | Hoàn thiện báo cáo, README, kịch bản demo và thuyết trình (cả 2 thành viên hiểu rõ thiết kế và code của nhau) | báo cáo LaTeX, README, `demo-script.md`, `viva-notes.md` |

**Bảng mô tả chi tiết thiết kế & code** (5 dòng phải điền trong báo cáo, mỗi dòng gồm mô tả thiết kế và cài đặt thực tế trong code):

| Dòng trong checklist | Nội dung của dự án |
|---|---|
| Phân hệ / Module 1.0 (cấu trúc cơ sở) | xem "Module 1.0" ở mục Lớp thực thể |
| Phân hệ / Module 1.1 (mở rộng từ 1.0 bằng kế thừa và đa hình) | xem "Module 1.1" ở mục Lớp thực thể |
| Design Pattern 1 (tên mẫu, vai trò các lớp) | State |
| Design Pattern 2 (tên mẫu, cơ chế phối hợp) | Observer |
| Kiến trúc đóng gói (phân tầng, file cấu hình, Unit Test) | 3 tầng + Domain, `appsettings.json`, 22 test xUnit |

## Quyết định đã chốt
- **Phân công (chốt 15/11):** bạn (chủ repo) nhận **suất A**, thành viên còn lại nhận **suất B**. Code của cả hai suất do bạn viết cùng Claude; cách commit của suất B (nhận patch hay tự code) chờ thành viên còn lại chọn trước 17/11, không trả lời thì mặc định nhận patch.
- Commit không kèm dòng `Co-Authored-By`; việc dùng Claude được nêu trong README và trong báo cáo.
- Thời hạn **2 tuần** (16/11 → 29/11/2026). Không có ràng buộc thêm từ giảng viên.
- IDE **Visual Studio** → mỗi Form có `.Designer.cs` chuẩn, mở được bằng Designer.
- Lưu **JSON**; giữ **10 Use Case**; tác nhân **Manager + Agent**.
- **Toàn bộ tiếng Anh**: tên lớp/hàm, comment, giao diện, dữ liệu mẫu, thông báo lỗi. (Báo cáo và tài liệu ôn vấn đáp viết tiếng Việt.)
- Mật khẩu **băm có salt** (PBKDF2-SHA256 qua `Rfc2898DeriveBytes.Pbkdf2`, có sẵn trong .NET).
- Tài liệu Claude làm: UML PlantUML, báo cáo **LaTeX** (chờ mẫu), README + kịch bản demo, tài liệu ôn vấn đáp.
- Cách làm: **xong mỗi module thì dừng**, tóm tắt thiết kế + điểm cần nhớ khi vấn đáp, bạn duyệt rồi đi tiếp.

## Phân công (chốt 15/11)
Trong toàn bộ tài liệu, **A** = bạn (chủ repo), **B** = thành viên còn lại. Quyền sở hữu tính theo file, không theo thư mục.

| | Suất A (bạn) — phân hệ Ticket | Suất B (thành viên còn lại) — phân hệ Khách hàng/Lưu trú |
|---|---|---|
| Use Case | UC1–UC5 | UC6–UC10 |
| Quy tắc nghiệp vụ | 1–5 | 6–10 |
| Design Pattern | State, Factory Method | Observer, Strategy |
| Sơ đồ tuần tự | UC2, UC4 | UC7, UC8 |
| Test | 11 (`tests/HotelCRM.Tests/Tickets/`) | 11 (`tests/HotelCRM.Tests/Customers/`, `Assignment/`) |
| Domain | `Domain/Tickets/**`; hàm `Agent.EnsureCanTake` | `Domain/Customers/**`, `Domain/Hotel/**` |
| Business | `TicketService`, `Queries/TicketQuery`, `Factories/**` | `CustomerService`, `BookingService`, `SurveyService`, `ReportService`, `Queries/CustomerDirectory`, `Queries/StayLookup`, `Assignment/**`, `Observers/**` |
| Giao diện | `Forms/Tickets/**` | `Forms/Customers/**`, `Forms/Bookings/**`, `Forms/Reports/**`, `UiNotificationObserver` |
| Lớp giả cho test | `FakeCustomerDirectory`, `FakeStayLookup` | `FakeTicketQuery` |
| Giai đoạn code | GĐ 2 | GĐ 3 và GĐ 4 |
| Tài liệu riêng | sơ đồ use case; sơ đồ State/Factory Method; chương báo cáo về phân hệ Ticket; README; `demo-script.md` | class diagram tổng thể; sơ đồ Observer/Strategy; chương báo cáo về phân hệ Khách hàng/Lưu trú; `viva-notes.md` |

**Phần chung, do bạn commit ở GĐ 1:** solution và cấu hình build; `Domain/Common`, `Domain/Staff`, `Domain/Interfaces`; toàn bộ `Data/` (Repository + Singleton, mọi record và mapper); `Business/Security`, `AuthService`; `Program.cs`, `ErrorHandler`, `LoginForm`, `MainForm`, vỏ rỗng của 9 Form; chữ ký của mọi lớp. Ngoài GĐ 1: quy tắc 11 (phân quyền), đóng gói, các chương mở đầu/kiến trúc/kết luận của báo cáo, cột "Minh chứng" của checklist.

Nguyên tắc:
- Hai suất cân bằng về số lượng (checklist yêu cầu "chia đều" ở tiêu chí 1, 7, 9, 18, 19).
- Hai suất chỉ chạm nhau qua 5 interface ở mục "Ghép 2 phân hệ". Sau GĐ 1, không suất nào sửa `Program.cs`, `MainForm`, `Data/` hay chữ ký của khung.

**Việc của thành viên B theo ngày:**

| Hạn | Việc |
|---|---|
| 17/11 | Cài .NET 10 SDK, Visual Studio (workload ".NET desktop development"), Git; gửi tên tài khoản GitHub; clone, build thử; chọn cách commit (nhận patch hay tự code). Không trả lời thì mặc định nhận patch |
| 25/11 | Áp từng patch code (hoặc tự code), đọc hiểu, commit ít nhất 12 lần, push; chạy thử UC6–UC10 |
| 27/11 | Đọc `viva-notes.md`; tự làm lại bài mở rộng Observer và Strategy, và ít nhất 1 bài của suất A, trên nhánh `practice/*` (không merge) |
| 29/11 | Áp patch tài liệu riêng của suất B, push; đọc báo cáo; chạy bản publish trên máy mình; tập vấn đáp chung |

Bạn cũng tự làm lại bài mở rộng State và Factory Method, và ít nhất 1 bài của suất B, trước 27/11.

## Đối chiếu ngưỡng tối thiểu
| Tiêu chí | Tối thiểu | Kế hoạch |
|---|---|---|
| Use Case | 8, chia đều | 10 (A 5, B 5) |
| Lớp thực thể | 15 | 26 |
| Quy tắc nghiệp vụ | 8, cả 2 phân hệ | 11 (A 5, B 5, chung 1) |
| Class Diagram | 16 lớp | toàn bộ Domain + Business |
| Sơ đồ tuần tự | 4 (2/người) | 4 |
| Design Pattern | 3 (1–2/người) | 5 (A: State, Factory Method; B: Observer, Strategy; chung: Repository + Singleton) |
| Test | 20 (10/người) | 22 (11/người) |
| Git | nhánh, 20 commit chia đều | `main`/`develop`/`feature/a-ticket`/`feature/b-customer`, ≥ 12 commit/người, tỷ lệ không lệch quá 60/40 |

## Kiến trúc (3 tầng + Domain, 5 project)
```
HotelCRM.sln   Directory.Build.props   .editorconfig   .gitignore   README.md   CLAUDE.md
├─ .claude/  settings.json, rules/*.md      # instruction cho các phiên Claude (mục "Tổ chức phiên làm việc")
├─ src/HotelCRM.Domain/        # thực thể, State, quy tắc, interface — không tham chiếu project nào
│   ├─ Common/      Person, Employee, EmployeeRole, DomainException (+ ValidationException, BusinessRuleException), BusinessSettings
│   ├─ Customers/   Customer, MembershipTier + 4 tier, Interaction, InteractionHistory, ContactChannel, SatisfactionSurvey
│   ├─ Hotel/       RoomType, Room, Booking, BookingStatus
│   ├─ Staff/       Agent, Manager, Department
│   ├─ Tickets/     SupportTicket + 3 lớp con, TicketPriority, States/ (TicketState + 4 state)
│   └─ Interfaces/  IEntity, IRepository<T>, RepositoryExtensions, ITicketObserver, ICustomerDirectory, CustomerSlaProfile, CustomerSummary, IStayLookup, ITicketQuery
├─ src/HotelCRM.Business/      # tầng nghiệp vụ
│   ├─ Services/    AuthService, TicketService, CustomerService, BookingService, SurveyService, ReportService (+ AgentPerformanceRow, DepartmentReportRow)
│   ├─ Queries/     CustomerDirectory, StayLookup, TicketQuery   # cài đặt 3 interface ranh giới, chỉ dựa trên repository
│   ├─ Assignment/  IAssignmentStrategy, AgentWorkload + 3 strategy
│   ├─ Factories/   TicketFactory + 3 factory
│   ├─ Observers/   InteractionLogger, LoyaltyObserver
│   └─ Security/    PasswordHasher, UserSession
├─ src/HotelCRM.Data/          # tầng dữ liệu
│   ├─ AppConfig.cs (Singleton)   JsonRepository.cs   InMemoryRepository.cs
│   └─ Records/ + Mappers/     # DTO phẳng để ghi JSON + chuyển đổi sang thực thể
├─ src/HotelCRM.WinForms/      # tầng giao diện (net10.0-windows)
│   ├─ Program.cs (composition root)   ErrorHandler.cs   UiNotificationObserver.cs
│   ├─ Forms/ LoginForm, MainForm
│   │         Tickets/   TicketListForm, TicketEditForm, TicketDetailForm
│   │         Customers/ CustomerListForm, CustomerEditForm, CustomerHistoryForm, SurveyForm
│   │         Bookings/  BookingForm
│   │         Reports/   ReportForm
│   └─ appsettings.json   Data/*.json (copy ra thư mục output)
└─ tests/HotelCRM.Tests/       # xUnit: Tickets/ (A), Customers/ và Assignment/ (B), Fakes/ (lớp giả viết tay)
docs/  PLAN.md, PROGRESS.md, tasks/stage-N-*.md, uml/*.puml + ảnh, report/ (LaTeX), demo-script.md, viva-notes.md
handoff/  (git-ignore) patch của suất B
tools/  HashPassword.cs (file-based app sinh chuỗi băm mật khẩu, không nằm trong solution)
```
Chiều gọi: `WinForms → Business → Domain ← Data`. Form chỉ gọi Service; Service chỉ biết `IRepository<T>`; `Program.cs` tạo `AppConfig`, repository, observer, strategy, service rồi truyền qua constructor (inject thủ công, không dùng thư viện DI).

**Lưu JSON:** thực thể giữ `private set` và validate nên không serialize trực tiếp. `JsonRepository<TEntity, TRecord> : IRepository<TEntity>` ghi/đọc `TRecord` (DTO phẳng) qua `IMapper<TEntity, TRecord>`; trạng thái ticket và hạng thành viên lưu bằng tên rồi dựng lại (`TicketState.FromName`, `MembershipTier.FromName`). Observer không lưu xuống file: `TicketService` gắn lại observer mỗi khi nạp/tạo ticket.

## Lớp thực thể (26 lớp + 4 enum)
Enum: `EmployeeRole` (Agent, Manager), `TicketPriority` (Low, Medium, High, Urgent), `BookingStatus`, `ContactChannel`. Loại ticket không phải enum mà là lớp con.

- **Người (5):** `Person` (abstract: Id, FullName, Phone, Email) → `Customer`; `Employee` (abstract: Username, PasswordHash, Salt, `Role`, `Department`, abstract `CanAssign()`/`CanClose()`/`CanReopen()`, kèm `EnsureCanAssign()`/`EnsureCanClose()`/`EnsureCanReopen()` ném lỗi khi không có quyền) → `Agent`, `Manager`
- **Tổ chức (1):** `Department` (Id, `Code`, Name, `Agents`)
- **Hạng thành viên (5):** `MembershipTier` (abstract: `Name`, `SlaFactor`, `PriorityBoost`, `MinPoints`, `Next()`) → `StandardTier`, `SilverTier`, `GoldTier`, `VipTier`
- **Khách sạn (3):** `RoomType`, `Room`, `Booking` (+ enum `BookingStatus`: Reserved, CheckedIn, CheckedOut, Cancelled)
- **Ticket (4):** `SupportTicket` (abstract; các thành viên được override: `BaseSlaHours`, `HandlingDepartmentCode`, `GoodwillPoints`, `Validate()`) → `ComplaintTicket`, `ServiceRequestTicket`, `MaintenanceTicket`
- **State (5):** `TicketState` (abstract) → `OpenState`, `InProgressState`, `ResolvedState`, `ClosedState`
- **Tương tác (3):** `InteractionHistory`, `Interaction` (+ enum `ContactChannel`), `SatisfactionSurvey`

**Module 1.0 (baseline):** `Person`/`Customer`/`Employee`/`Agent`, `SupportTicket` với một lớp cụ thể `ServiceRequestTicket`, `TicketState` 3 trạng thái của đề (Open, InProgress, Resolved), `InteractionHistory`, `IRepository<T>`.
**Module 1.1 (mở rộng bằng kế thừa/đa hình):** `ComplaintTicket` và `MaintenanceTicket` (override `BaseSlaHours`, `HandlingDepartmentCode`, `GoodwillPoints`, `Validate()`), `ClosedState`, cây `MembershipTier`, `Manager` (override quyền). Báo cáo ghi đúng thực tế: 1.1 thêm lớp mới, và thêm đúng hai hàm `Close()`/`Reopen()` vào `SupportTicket`; các lớp khác của 1.0 không sửa.

## Quan hệ UML phải có trong code (tiêu chí 6)
| Quan hệ | Ví dụ |
|---|---|
| Kế thừa | `Customer`→`Person`; `Agent`,`Manager`→`Employee`; 3 ticket→`SupportTicket`; 4 state→`TicketState`; 4 tier→`MembershipTier` |
| Thực thi | `JsonRepository`→`IRepository<T>`; strategy→`IAssignmentStrategy`; observer→`ITicketObserver`; `CustomerDirectory`→`ICustomerDirectory`; `StayLookup`→`IStayLookup`; `TicketQuery`→`ITicketQuery` |
| Hợp thành | `Customer` ◆— `InteractionHistory` ◆— `Interaction`; `SupportTicket` ◆— `TicketState` |
| Kết tập | `Department` ◇— `Agent`; `RoomType` ◇— `Room`; `SupportTicket` ◇— `Agent` (người được giao) |
| Phụ thuộc | `TicketService` ⇢ `IAssignmentStrategy`, `TicketFactory`; Form ⇢ Service |

**Ghép 2 phân hệ qua interface (tiêu chí 16):**
- Phân hệ Ticket (A) cần dữ liệu của B qua `ICustomerDirectory` (tìm khách, lấy `CustomerSlaProfile` — quy tắc 4) và `IStayLookup` (phòng khách đang ở — quy tắc 5).
- Phân hệ Khách hàng (B) cần dữ liệu của A qua `ITicketQuery` (khách còn ticket mở không — quy tắc 10; trạng thái ticket — quy tắc 9).
- B phản ứng với sự kiện của A qua `ITicketObserver` (ghi lịch sử, cộng điểm).
- `TicketService` (A) gọi thuật toán phân công của B qua `IAssignmentStrategy` (khai báo trong `Business/Assignment`).

Bốn interface đầu nằm trong `Domain/Interfaces`. Không service nào tham chiếu trực tiếp lớp service của phân hệ kia, và `Domain/Tickets` không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`. Ba interface truy vấn do lớp riêng trong `Business/Queries` cài đặt (không phải service) để tránh phụ thuộc vòng khi khởi tạo.

## Design Pattern
| Pattern | Người | Vai trò lớp | Bài mở rộng khi vấn đáp |
|---|---|---|---|
| **State** | A | Context `SupportTicket`; State `TicketState`; Concrete 4 state. `Start(now)`, `Resolve(note, now)`, `Close(now)`, `Reopen(now, reopenDays)` ủy quyền cho `State`; lớp State đổi trạng thái qua `SupportTicket.MoveTo(next, now)` | thêm `OnHoldState` |
| **Factory Method** | A | Creator `TicketFactory.Create(...)` → `ComplaintTicketFactory`, `ServiceRequestTicketFactory`, `MaintenanceTicketFactory`. `TicketService.CreateTicket()` chọn factory qua bảng tra `tên loại → TicketFactory` dựng trong `Program.cs` | thêm `LostItemTicket` + factory |
| **Observer** | B | Subject `SupportTicket` (`Attach`/`Notify`, phần Subject do A cài ở GĐ 2); `ITicketObserver.OnStateChanged(ticket, from, to)` → `InteractionLogger`, `LoyaltyObserver`, `UiNotificationObserver` | thêm `EmailNotifier` |
| **Strategy** | B | `IAssignmentStrategy.Pick(departmentCode, candidates)` (`candidates` là danh sách `AgentWorkload` của các Agent còn nhận được ticket; trả về `Agent?`) → `LeastLoadedStrategy`, `DepartmentMatchStrategy`, `RoundRobinStrategy`; Context `TicketService.AutoAssign()` (của A, chỉ biết interface); chọn theo `DefaultAssignmentStrategy` trong `appsettings.json` qua bảng tra dựng trong `Program.cs` | thêm `SkillBasedStrategy` |
| **Repository + Singleton** | Chung | `IRepository<T>` → `JsonRepository`, `InMemoryRepository` (dùng cho test); `AppConfig.Instance` | thêm repository lưu CSV |

- Bảng mô tả thiết kế trong báo cáo: Design Pattern 1 = State (A), Design Pattern 2 = Observer (B). Ba mẫu còn lại trình bày ở mục riêng; cả hai thành viên cùng trình bày Repository + Singleton.
- Mở rộng nghĩa là "thêm lớp mới và đăng ký một dòng": lớp mới không sửa lớp cũ, nhưng phải thêm một dòng vào bảng tra (`TicketState.FromName`, bảng factory, bảng strategy, mapper). Báo cáo và vấn đáp nói đúng như vậy, không nói "không sửa dòng nào".
- Bài mở rộng không commit vào `develop` hay nhánh `feature/*`; tập trên nhánh `practice/*`. `viva-notes.md` chứa lời giải dạng văn bản.

## Quy tắc nghiệp vụ → nơi cài đặt
Cách đặt quy tắc: **service tra cứu, lớp Domain quyết định.** Quy tắc cần dữ liệu của nhiều đối tượng thì service lấy dữ liệu rồi truyền kết quả vào một phương thức của lớp Domain; phương thức đó ném ngoại lệ. Service không tự ném ngoại lệ nghiệp vụ.

| # | Người | Quy tắc | Cài đặt tại |
|---|---|---|---|
| 1 | A | Chuyển trạng thái hợp lệ: Open→InProgress (phải có Agent), InProgress→Resolved (phải có ghi chú), Resolved→Closed, Resolved→InProgress (Reopen); Closed bất biến. Phân công lần đầu chỉ ở Open; phân công lại ở Open hoặc InProgress | các lớp `*State`; `SupportTicket.AssignTo()` |
| 2 | A | Reopen chỉ khi `now ≤ ResolvedAt + ReopenDays`; ticket quay về InProgress và giữ nguyên Agent | `SupportTicket.Reopen(now, reopenDays)` → `ResolvedState.Reopen()` |
| 3 | A | Agent không giữ quá `MaxActiveTickets` ticket đang mở | `Agent.EnsureCanTake(activeCount, maxActive)` (và `Agent.CanTake` trả `bool`, dùng để lọc ứng viên), gọi từ `SupportTicket.AssignTo(agent, activeCount, maxActive)`; `TicketService` đếm `activeCount` |
| 4 | A | Mức ưu tiên được nâng theo `PriorityBoost` của hạng khách lúc tạo ticket và lưu vào ticket. Hạn SLA = `BaseSlaHours` × hệ số Priority × `SlaFactor` | constructor của `SupportTicket` (nâng mức ưu tiên), `SupportTicket.CalculateDueAt(slaFactor)`; `TicketService` lấy `CustomerSlaProfile` từ `ICustomerDirectory` |
| 5 | A | `MaintenanceTicket` phải gắn phòng khách đang ở (booking CheckedIn) | `MaintenanceTicket.Validate(roomInActiveStay)`; `TicketService` tra `IStayLookup` |
| 6 | B | SĐT/Email đúng định dạng, không trùng | định dạng: `Person` (cài ở GĐ 1 vì đăng nhập cần); không trùng: `Customer.EnsureUnique(phoneTaken, emailTaken)`, `CustomerService.Register()`/`Update()` tra cứu |
| 7 | B | CheckOut > CheckIn; phòng không bị đặt chồng (booking Reserved và CheckedIn chặn); chỉ hủy được booking Reserved | constructor của `Booking`; `Room.IsAvailable(checkInDate, checkOutDate, bookings)` và `Room.EnsureAvailable(...)` (ném lỗi); `Booking.CheckIn()`/`CheckOut()`/`Cancel()` |
| 8 | B | Cộng điểm khi checkout và khi ticket Closed; tự nâng hạng theo ngưỡng, có thể nâng nhiều bậc một lần, không hạ | `Customer.AddPoints()`, `MembershipTier.Next()` |
| 9 | B | Khảo sát chỉ cho ticket Resolved/Closed, điểm 1–5, mỗi ticket một lần | `SatisfactionSurvey.Create(score, ticketResolvedOrClosed, alreadySurveyed)`; `SurveyService.Submit()` tra `ITicketQuery` và repository |
| 10 | B | Không xóa khách còn ticket chưa đóng | `Customer.EnsureCanBeDeleted(hasOpenTickets)`; `CustomerService.Delete()` tra `ITicketQuery` |
| 11 | Chung | Tạo ticket: cả hai vai trò. Phân công và phân công lại: chỉ Manager. Start và Resolve: chỉ Agent đang được giao ticket đó. Close và Reopen: chỉ Manager. Agent không tự nhận ticket | `Employee.CanAssign()`/`CanClose()`/`CanReopen()` (`Agent`/`Manager` override) và `Employee.EnsureCanAssign()`/`EnsureCanClose()`/`EnsureCanReopen()` ném `BusinessRuleException` (cài ở GĐ 1); `SupportTicket.EnsureAssignedTo(actor)`; `TicketService` nhận `actor` và gọi các hàm `Ensure...` trước mỗi thao tác (A cài ở GĐ 2) |

Vi phạm ném `ValidationException` hoặc `BusinessRuleException` (con của `DomainException`); quy tắc nào ném loại nào xem mục "Quyết định thiết kế bổ sung".

## Use Case → hàm điều khiển (tiêu chí 1)
| UC | Người | Tác nhân | Form | Hàm điều khiển |
|---|---|---|---|---|
| UC1 Log in | A | Manager, Agent | `LoginForm` | `AuthService.Login()` / `Logout()` (cài ở GĐ 1) |
| UC2 Create ticket | A | Manager, Agent | `TicketEditForm` | `TicketService.CreateTicket()` |
| UC3 Assign ticket | A | Manager | `TicketListForm` | `TicketService.AutoAssign()` / `AssignManually()` |
| UC4 Process ticket | A | Agent (Start, Resolve); Manager (Close, Reopen) | `TicketDetailForm` | `TicketService.Start()` / `Resolve()` / `Close()` / `Reopen()` |
| UC5 Search tickets | A | Manager, Agent | `TicketListForm` | `TicketService.Search()` |
| UC6 Manage customers | B | Manager, Agent | `CustomerListForm`, `CustomerEditForm` | `CustomerService.Register()` / `Update()` / `Delete()` |
| UC7 Manage bookings | B | Manager, Agent | `BookingForm` | `BookingService.Book()` / `CheckIn()` / `CheckOut()` / `Cancel()` |
| UC8 Record survey | B | Agent | `SurveyForm` | `SurveyService.Submit()` |
| UC9 View interaction history | B | Manager, Agent | `CustomerHistoryForm` | `CustomerService.GetHistory()` |
| UC10 Reports | B | Manager | `ReportForm` | `ReportService.AgentPerformance()` / `ByDepartment()` |

- `MainForm` ẩn/hiện menu theo `UserSession.Current.Role`.
- Form truyền `UserSession.Current` vào tham số `actor` của service. Service không đọc `UserSession`.
- Sơ đồ tuần tự: A vẽ UC2 và UC4, B vẽ UC7 và UC8.

## Ngoại lệ, cấu hình, quy ước
- `ErrorHandler.Run(Action)` bọc mọi event handler của Form: `DomainException` → hộp thoại cảnh báo; lỗi khác → ghi `logs/error.log` + thông báo chung. `Program.cs` đăng ký thêm `Application.ThreadException`.
- `appsettings.json`: `DataFolder`, `DefaultAssignmentStrategy`, `MaxActiveTickets` (mặc định 5), `ReopenDays` (mặc định 7). Ngưỡng điểm, giờ SLA và hệ số hạng là hằng số trong các lớp Domain (thể hiện đa hình), không nằm trong cấu hình. Đường dẫn tính từ `AppContext.BaseDirectory`, không gán cứng.
- `.editorconfig`: PascalCase cho public, `_camelCase` cho field private, file-scoped namespace, mỗi file một lớp.
- `Directory.Build.props`: `Nullable=enable`, `TreatWarningsAsErrors=true`, `LangVersion=latest`, `ImplicitUsings=enable` cho mọi project.
- Giao diện: `LoginForm` và `MainForm` (GĐ 1) là form mẫu, các form sau chép đúng cấu trúc. GĐ 1 cũng tạo vỏ rỗng của 9 Form còn lại và nối chúng vào menu, để GĐ 2–4 chỉ điền nội dung Form của mình. Mỗi form có `.Designer.cs`; `InitializeComponent()` chỉ tạo control và gán thuộc tính, không chứa logic, để Designer của Visual Studio mở được. Form chỉ gọi service; mọi event handler bọc trong `ErrorHandler.Run`.

## Quyết định thiết kế bổ sung

**1. Khởi tạo.** Thứ tự trong `Program.cs`: `AppConfig` → repository → `CustomerDirectory`, `StayLookup`, `TicketQuery` → observer → bảng strategy và bảng factory → service → form. GĐ 1 viết toàn bộ phần nối dây này; các giai đoạn sau không sửa `Program.cs`. Mỗi kiểu thực thể có đúng một repository, nạp file một lần vào bộ nhớ và luôn trả về cùng một đối tượng. `TicketService` gắn observer cho mọi ticket trong repository lúc khởi tạo và cho ticket mới trong `CreateTicket()`. Tên strategy hợp lệ trong `appsettings.json`: `LeastLoaded`, `DepartmentMatch`, `RoundRobin`; tên lạ thì báo lỗi lúc khởi động.

**2. Cấu hình và thời gian đi xuống Domain.** `BusinessSettings` (`Domain/Common`: `MaxActiveTickets`, `ReopenDays`, tự kiểm tra > 0). `AppConfig.Instance.Rules` trả về `BusinessSettings`; `Program.cs` truyền nó vào constructor của service. Lớp Domain không đọc cấu hình và không gọi `DateTime.Now`: giới hạn và thời điểm `now` đi vào qua tham số. `AppConfig` chỉ được gọi trong `Program.cs`.

**3. Ranh giới giữa hai phân hệ ở tầng Domain.** `Domain/Tickets` không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`, và `Domain/Customers`, `Domain/Hotel` không dùng lớp nào của `Domain/Tickets`; chỉ hai interface `ITicketObserver` và `ITicketQuery` (trong `Domain/Interfaces`) đưa `SupportTicket` sang cho tầng Business của suất B. Ticket lấy thông tin hạng khách qua `ICustomerDirectory.GetSlaProfile(customerId)`, trả về `CustomerSlaProfile(SlaFactor, PriorityBoost)` — hai con số, không phải `MembershipTier`. Form ticket chọn và hiển thị khách qua `CustomerSummary(Id, FullName, Phone, TierName)`, không dùng `Customer`. "Không tìm thấy theo Id" do `RepositoryExtensions.GetRequired` ném, để service không tự ném lỗi nghiệp vụ.

**4. Con số và định nghĩa nghiệp vụ.**
| Mục | Giá trị |
|---|---|
| `BaseSlaHours` (override theo loại ticket) | Complaint 24, ServiceRequest 8, Maintenance 12 |
| Hệ số Priority | Low 2.0, Medium 1.0, High 0.5, Urgent 0.25 |
| `SlaFactor` theo hạng | Standard 1.0, Silver 0.9, Gold 0.75, VIP 0.5 |
| `PriorityBoost` (số bậc nâng, trần là Urgent) | Standard 0, Silver 0, Gold 1, VIP 2 |
| Hạn xử lý | `DueAt = CreatedAt + BaseSlaHours × hệ số Priority (sau khi nâng) × SlaFactor` giờ. Ví dụ: Complaint, Medium, `PriorityBoost` 1, `SlaFactor` 0.75 → High → 24 × 0.5 × 0.75 = 9 giờ |
| Phòng ban xử lý (`HandlingDepartmentCode`, override theo loại ticket) | Complaint → `GUEST` (Guest Relations), ServiceRequest → `HOUSE` (Housekeeping), Maintenance → `ENG` (Engineering). Phòng ban thứ tư: `FRONT` (Front Office) |
| `DepartmentMatchStrategy` | chọn Agent ít ticket đang mở nhất trong phòng ban xử lý; phòng ban đó không còn ai nhận được thì chọn trong toàn bộ Agent |
| Điểm khi checkout | 1 điểm cho 1 USD; `Booking.TotalAmount` = số đêm × `RoomType.PricePerNight` |
| Điểm khi ticket Closed | `SupportTicket.GoodwillPoints`: mặc định 10, `ComplaintTicket` override thành 50 |
| `MinPoints` của hạng | Standard 0, Silver 1000, Gold 5000, VIP 15000 |
| Ticket "đang mở" | Quy tắc 3: đã giao cho Agent và ở Open/InProgress. Quy tắc 10: mọi ticket chưa Closed |
| Định dạng SĐT | đúng 10 chữ số, bắt đầu bằng `0` |
| Định dạng Email | có đúng một `@`, có dấu chấm sau `@`, không có khoảng trắng |
| Ticket trễ hạn | `(ResolvedAt ?? now) > DueAt` |
| Số giờ giải quyết | `ResolvedAt − CreatedAt`, lấy lần Resolved gần nhất; chỉ tính ticket đã từng Resolved |
| Phòng ban của ticket trong báo cáo | phòng ban của Agent đang được giao; ticket chưa giao không tính |
| `ReportService.AgentPerformance()` | mỗi Agent: số ticket được giao, số đã giải quyết, số giờ giải quyết trung bình, số ticket trễ hạn, điểm khảo sát trung bình |
| `ReportService.ByDepartment()` | mỗi phòng ban: số ticket theo trạng thái, số giờ giải quyết trung bình, điểm khảo sát trung bình |
| Tiêu chí tìm ticket (UC5) | trạng thái, mức ưu tiên, loại, Agent được giao, từ khóa trong tiêu đề; để trống nghĩa là không lọc |
| Băm mật khẩu | PBKDF2-SHA256, 100.000 vòng, salt 16 byte ngẫu nhiên cho mỗi tài khoản, hash 32 byte, lưu Base64 |

**5. Loại ngoại lệ theo quy tắc.**
- `ValidationException` (dữ liệu của một đối tượng không hợp lệ): sai định dạng SĐT/Email (6), CheckOut ≤ CheckIn (7), điểm khảo sát ngoài 1–5 (9), ghi chú giải quyết rỗng (1), trường bắt buộc bỏ trống.
- `BusinessRuleException` (vi phạm quy tắc giữa các đối tượng hoặc sai trạng thái): chuyển trạng thái sai, chưa có Agent, phân công sai trạng thái (1); Reopen quá hạn (2); vượt `MaxActiveTickets` (3); phòng không thuộc booking đang ở (5); trùng SĐT/Email (6); đặt chồng phòng, hủy booking không ở Reserved (7); khảo sát sai trạng thái hoặc lần hai (9); xóa khách còn ticket (10); thao tác sai quyền (11).
- Quy tắc 4 và 8 là phép tính, không ném lỗi.

**6. Lưu trữ.**
- 8 kiểu có file JSON cài `IEntity { int Id }`: `Department`, `Employee`, `RoomType`, `Room`, `Customer`, `Booking`, `SupportTicket`, `SatisfactionSurvey`. Repository cấp Id kế tiếp; service lấy Id rồi mới tạo thực thể. `TicketState`, `MembershipTier`, `InteractionHistory`, `Interaction` không có Id riêng.
- File trong `Data/`: `departments.json`, `employees.json`, `roomtypes.json`, `rooms.json`, `customers.json`, `bookings.json`, `tickets.json`, `surveys.json`; nạp theo đúng thứ tự này.
- Tên trường trong record trùng tên thuộc tính của thực thể (PascalCase); enum ghi dạng chuỗi; thời gian ISO 8601.
- Trường phân biệt lớp con: `tickets.json` có `Type` (Complaint/ServiceRequest/Maintenance) và `State`; `employees.json` có `Role` (Agent/Manager), dùng chung `IRepository<Employee>`; `customers.json` có `Tier`, `Points` và mảng `Interactions` lồng bên trong; `departments.json` có `Code`.
- Khôi phục từ file: mỗi thực thể có hàm `internal static Restore(...)` dựng lại đối tượng với trạng thái đã lưu mà không chạy lại quy tắc nghiệp vụ. `Domain` mở `internal` cho project `Data` bằng `InternalsVisibleTo`. Phía ngoài vẫn chỉ thấy constructor có kiểm tra.
- Tham chiếu: trong cùng phân hệ giữ đối tượng (`SupportTicket.AssignedAgent`, `Room.RoomType`, `Booking.Room`, `Booking.Customer`, `Employee.Department`); khác phân hệ giữ Id (`SupportTicket.CustomerId`, `MaintenanceTicket.RoomId`, `SatisfactionSurvey.TicketId`). Mapper cần tham chiếu thì nhận repository tương ứng qua constructor.
- Chuỗi băm mật khẩu trong `employees.json` phải sinh bằng `PasswordHasher`, không gõ tay.

**7. Observer và lịch sử tương tác.**
- Observer chạy sau khi trạng thái ticket đã đổi. Lỗi trong observer không bị nuốt: nó lan lên và `ErrorHandler` hiển thị.
- `InteractionLogger` và `LoyaltyObserver` nhận `IRepository<Customer>` qua constructor, sửa `Customer` rồi tự gọi repository để lưu.
- `InteractionLogger` ghi một `Interaction` mỗi lần ticket đổi trạng thái. `LoyaltyObserver` cộng `GoodwillPoints` khi ticket chuyển sang Closed.
- `Interaction` lúc check-in và check-out do `BookingService` ghi trực tiếp vào `Customer`, không đi qua `ITicketObserver`.
- Thời điểm đổi trạng thái nằm ở `SupportTicket.UpdatedAt`; observer đọc từ đó vì `OnStateChanged(ticket, from, to)` không mang thời gian.
- Từ GĐ 1 đến hết GĐ 3, thân `OnStateChanged` của ba observer để rỗng (không ném `NotImplementedException`) để các thao tác trên ticket chạy được ở điểm duyệt chung đầu GĐ 4; GĐ 4 điền thân hàm.

## Kiểm thử (22 test xUnit, dùng `InMemoryRepository`)
Cả 22 test được viết ở GĐ 1 trên khung. Ở cuối GĐ 1: 2 test định dạng của `CustomerTests` xanh (vì `Person` đã cài), 20 test còn lại đỏ và lý do duy nhất là `NotImplementedException`. Các giai đoạn sau làm cho test xanh và **không sửa test**; thấy test sai thì dừng, ghi vào `PROGRESS.md`. Thời điểm `now` luôn là giá trị cố định.

**A — `tests/HotelCRM.Tests/Tickets/` (11), xanh ở cuối GĐ 2**

| Lớp test | Hàm test |
|---|---|
| `TicketStateTests` | `Start_OpenTicketWithAgent_MovesToInProgress`; `Resolve_InProgressWithNote_MovesToResolved`; `Close_ResolvedTicket_MovesToClosed`; `Start_WithoutAgent_ThrowsBusinessRule`; `Resolve_ClosedTicket_ThrowsBusinessRule`; `Reopen_AfterReopenDays_ThrowsBusinessRule` |
| `TicketRuleTests` | `AssignTo_AgentAtMaxActive_ThrowsBusinessRule`; `CalculateDueAt_ComplaintBoostedToHighFactor075_Returns9Hours` |
| `TicketServiceTests` | `CreateTicket_MaintenanceRoomNotInActiveStay_ThrowsBusinessRule`; `Close_ByAgent_ThrowsBusinessRule` |
| `TicketFactoryTests` | `Create_EachFactory_ReturnsMatchingSubclass` |

**B — `tests/HotelCRM.Tests/Customers/` và `Assignment/` (11): 9 test xanh ở cuối GĐ 3, 2 test của `SurveyServiceTests` xanh ở GĐ 4**

| Lớp test | Hàm test |
|---|---|
| `CustomerTests` | `Constructor_InvalidEmail_ThrowsValidation`; `Constructor_InvalidPhone_ThrowsValidation`; `AddPoints_PositiveAmount_IncreasesPoints`; `AddPoints_CrossingThresholds_UpgradesTier` |
| `CustomerServiceTests` | `Register_DuplicatePhone_ThrowsBusinessRule`; `Delete_CustomerWithOpenTicket_ThrowsBusinessRule` |
| `BookingTests` | `Constructor_CheckOutNotAfterCheckIn_ThrowsValidation` |
| `BookingServiceTests` | `Book_OverlappingDates_ThrowsBusinessRule` |
| `SurveyServiceTests` | `Submit_TicketNotResolved_ThrowsBusinessRule`; `Submit_SecondSurveyForTicket_ThrowsBusinessRule` |
| `AssignmentStrategyTests` (trong `Assignment/`) | `LeastLoaded_Pick_ReturnsAgentWithFewestActive` |

- Tên hàm có thể chỉnh ở GĐ 1 cho khớp chữ ký thật, nhưng số lượng và nội dung kiểm tra giữ nguyên.
- Lớp giả viết tay trong `tests/HotelCRM.Tests/Fakes/`: `FakeCustomerDirectory`, `FakeStayLookup` thuộc suất A; `FakeTicketQuery` thuộc suất B.
- Test của suất A không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`, nên xanh được mà không cần code của suất B.
- 11 test của suất B và `FakeTicketQuery` chỉ có trên `local/b-work` (mục Git); trên `develop` chỉ có 11 test của suất A cho tới khi thành viên B push.
- Test chỉ phủ Domain và Business; giao diện kiểm tra bằng tay (mục Kiểm tra).

## Dữ liệu mẫu liên thông (`Data/*.json`)
- GĐ 1 (tối thiểu, phần chung): 4 phòng ban (`GUEST`, `HOUSE`, `ENG`, `FRONT`), 1 manager, 2 agent; sáu file còn lại là mảng rỗng.
- GĐ 4 (đầy đủ, thành viên B commit, chia theo file): 2 manager, 6 agent, 5 loại phòng, 20 phòng, 15 khách (đủ 4 hạng), khoảng 20 booking (đủ 4 trạng thái), khoảng 25 ticket (đủ 3 loại × 4 trạng thái), lịch sử tương tác và khảo sát khớp Id với nhau.
- `employees.json` đầy đủ do phiên Sonnet của GĐ 4 sinh bằng tiện ích băm của GĐ 1. Bảy file còn lại giao cho phiên Haiku.
- Tài khoản mẫu ghi trong README.

## Git
- Nhánh: `main` (bản ổn định) ← `develop` ← `feature/a-ticket` (bạn) và `feature/b-customer` (thành viên B). `local/b-work` chỉ có trên máy bạn và **không bao giờ push**. `practice/*` để tập bài mở rộng, không merge.
- Commit message không kèm dòng `Co-Authored-By` (đã tắt trong `.claude/settings.json`). Việc dùng Claude được nêu trong README và trong báo cáo.
- Mục tiêu: mỗi người ít nhất 12 commit và tỷ lệ không lệch quá 60/40 (`git shortlog -sn`). Commit nhỏ theo từng lớp/quy tắc/test.
- **Khung chung và suất A:** Claude commit bằng tài khoản của bạn khi bạn đồng ý; khung trên `develop`, suất A trên `feature/a-ticket` rồi merge vào `develop`.
- **Suất B, cách 1 — nhận patch (mặc định):**
  1. `local/b-work` được tạo ở GĐ 1 từ `develop`; commit đầu tiên là 11 test của suất B và `FakeTicketQuery`. Nhánh này tồn tại tới khi thành viên B đã push toàn bộ, chỉ xóa ở cuối GĐ 6.
  2. Claude làm việc của suất B trên `local/b-work`, chia thành commit nhỏ (commit khi bạn đồng ý). Khi `develop` có thay đổi mới, merge `develop` vào `local/b-work`.
  3. Mỗi commit của suất B xuất thành `handoff/NN-ten-ngan.patch` (bằng `git diff <commit trước> <commit này>`) kèm `handoff/NN-ten-ngan.msg.txt` chứa commit message gợi ý. Số `NN` chạy liên tục từ 01 qua GĐ 3, 4, 5, 6. Merge commit không xuất patch; `handoff/README.txt` ghi patch nào cần merge `origin/develop` trước khi áp.
  4. Thành viên B, trên `feature/b-customer` tạo từ `develop`: `git apply` từng patch theo thứ tự, đọc hiểu, build, tự commit bằng message đó, rồi push.
  5. Bạn fetch và kiểm tra `git diff local/b-work origin/feature/b-customer -- src tests` rỗng (so trước khi thành viên B thêm commit không qua patch), rồi merge `feature/b-customer` vào `develop`.
  6. Vòng đầu tiên của GĐ 3 chạy đúng thì chép các bước thành skill `.claude/skills/export-patch/SKILL.md`.
- **Suất B, cách 2 — tự code:** thành viên B tự viết theo file task của GĐ 3 và GĐ 4 trên `feature/b-customer`; Claude chỉ làm khung và suất A. `docs/PROGRESS.md` ghi rõ cách nào đang được dùng.
- **Test:** 11 test của suất A do bạn commit trên `develop` ở GĐ 1. 11 test của suất B là patch số 01, áp trước mọi patch khác.
- **Bù commit:** khung chung làm bạn nhiều commit hơn. Thành viên B bù bằng Strategy, dữ liệu mẫu đầy đủ (mỗi file một commit) và tài liệu riêng của suất B; tài liệu ở GĐ 5–6 cũng đi qua quy trình patch.
- Thành viên B push xong code trước khi GĐ 5 bắt đầu (hạn 25/11).
- Tiêu chí 12 và 20 yêu cầu cả hai hiểu code của nhau: cả hai đọc `docs/viva-notes.md` và tự làm lại các bài mở rộng pattern trước buổi vấn đáp.

## Lộ trình
14 ngày (16–29/11/2026). Cột "Hạn" là hạn chót; xong sớm thì dồn thời gian dư cho GĐ 5–6 và ôn vấn đáp. Mỗi giai đoạn kết thúc bằng một điểm dừng để bạn duyệt. Trạng thái thực tế của từng giai đoạn ghi ở `docs/PROGRESS.md`.

| GĐ | Hạn | Nhánh | Nội dung | Sản phẩm bắt buộc | Điểm dừng để duyệt |
|---|---|---|---|---|---|
| **0 Chuẩn bị** | 16/11 | — | Viết instruction cho các phiên sau | `CLAUDE.md`, `.claude/settings.json`, 5 file `.claude/rules/`, `docs/PROGRESS.md`, `docs/tasks/stage-1-skeleton.md`, `.gitignore` | Bạn đọc `CLAUDE.md` và rules |
| **1 Khung** | 17/11 | `develop`; `local/b-work` cho test của B | Solution + 5 project, `Directory.Build.props`, `.editorconfig`; `Domain/Common`, `Staff`, `Interfaces`; toàn bộ `Data/`; `PasswordHasher`, `UserSession`, `AuthService`; `Program.cs` nối dây đầy đủ; `ErrorHandler`; `LoginForm`, `MainForm`, vỏ rỗng của 9 Form; dữ liệu mẫu tối thiểu; **khung toàn bộ Domain + Business của GĐ 2–4** (chữ ký đầy đủ, chú thích XML, thân hàm `throw new NotImplementedException()`); **22 test** | `docs/tasks/stage-2-ticket.md`, `stage-3-customer.md`, `stage-4-integration.md`; 5 file rules đã rà lại theo code thật; `handoff/01-…` | Chạy app, đăng nhập 2 vai trò, mở được 9 Form rỗng từ menu; `dotnet build` sạch; 22 test: 2 xanh, 20 đỏ vì `NotImplementedException` |
| **2 Suất A** | 20/11 | `feature/a-ticket` | Thân hàm của ticket, 4 State, 3 Factory, `TicketService` (gồm quy tắc 11), `TicketQuery`, `Agent.EnsureCanTake`; nội dung của `TicketListForm`, `TicketEditForm`, `TicketDetailForm` | 11 test A xanh | Test xanh; 3 Form ticket mở được trong Designer |
| **3 Suất B** | 23/11 | `local/b-work` | Thân hàm của `Customer`, cây `MembershipTier`, `Room`/`Booking`, `CustomerService`, `BookingService`, `CustomerDirectory`, `StayLookup`, 3 strategy; nội dung của Form khách hàng và đặt phòng | 9 test B xanh; patch trong `handoff/` | Test xanh; Form khách hàng và đặt phòng mở được trong Designer |
| **4 Ghép nối** | 25/11 | `local/b-work`, sau khi đã merge `develop` có suất A | Mở đầu: điểm duyệt chung, UC1–UC7 chạy trên giao diện. Sau đó suất B: `InteractionLogger`, `LoyaltyObserver`, `UiNotificationObserver`, `SurveyService` (UC8), lịch sử (UC9), `ReportService` (UC10), dữ liệu mẫu đầy đủ; publish thử | 22 test xanh; bản publish chạy được; `docs/tasks/stage-5-docs.md`; thành viên B đã push toàn bộ code | Chạy trọn kịch bản demo trên bản publish |
| **5 Tài liệu kỹ thuật** | 27/11 | `develop` (phần của A), `local/b-work` (phần của B) | UML PlantUML (use case, class tổng thể, 4 sequence, 5 sơ đồ pattern); README có mục nêu việc dùng Claude; `demo-script.md`; `viva-notes.md` kèm lời giải 5 bài mở rộng | file `.puml` + ảnh trong `docs/uml/`; `docs/tasks/stage-6-report.md` | Sơ đồ khớp tên lớp/hàm trong code |
| **6 Báo cáo & nộp** | 29/11 | như GĐ 5 | Báo cáo LaTeX theo mẫu, có mục nêu việc dùng Claude; điền cột "Minh chứng" của checklist; chạy bản publish trên máy khác; tập vấn đáp | `docs/report/` biên dịch được; gói nộp | Bản nộp |

- **Chạy song song:** GĐ 2 và GĐ 3 chỉ phụ thuộc khung của GĐ 1, nên có thể mở hai phiên cùng lúc từ 18/11, mỗi phiên một worktree. Không phiên nào sửa `Program.cs`, `MainForm`, `Data/` hay chữ ký của khung. Trong GĐ 5, ba việc UML, README + kịch bản demo, `viva-notes.md` cũng chạy song song được.
- **Review:** cuối GĐ 2, 3 và 4, một phiên Opus chạy `/code-review`; lỗi tìm ra do phiên Sonnet của giai đoạn đó sửa trước điểm duyệt.
- **Mốc phụ thuộc vào người:** thành viên B gửi tên GitHub và chọn cách commit trước **17/11**; push xong code trước **25/11**; mẫu báo cáo LaTeX có trước **26/11**.
- **Thứ tự cắt giảm nếu trễ tiến độ** (vẫn giữ đủ mọi ngưỡng tối thiểu của checklist): (1) `RoundRobinStrategy` — còn 2 strategy; (2) `ReportService.ByDepartment()` — còn báo cáo hiệu suất Agent; (3) `UiNotificationObserver` — còn 2 observer; (4) giảm dữ liệu mẫu còn một nửa; (5) sơ đồ riêng từng pattern gộp vào class diagram tổng thể. Không cắt: 10 UC, 11 quy tắc, 22 test, 5 pattern, 4 sơ đồ tuần tự.

## Tổ chức phiên làm việc, model và instruction

**Nguyên tắc.** Mỗi giai đoạn một phiên mới, chọn model cho cả phiên. Không dùng agent tùy biến: agent con khởi động lại từ đầu và phải đọc lại ngữ cảnh, nên với code lõi nó tốn hơn một phiên model nhỏ làm trực tiếp. Tiết kiệm token đến từ: file task ngắn thay cho việc đọc cả plan, rules chỉ nạp theo thư mục, test có sẵn làm điều kiện xong, và không kéo dài phiên cũ.

**Model theo phiên**

| Phiên | Model | Lý do |
|---|---|---|
| GĐ 1 Khung | Fable 5.2, hoặc Opus 5.5 | Quyết định chữ ký, viết test và file task; sai ở đây thì mọi phiên sau sai theo |
| GĐ 2 Suất A | Sonnet 5.5 | Điền thân hàm theo khung và test có sẵn |
| GĐ 3 Suất B (song song với GĐ 2) | Sonnet 5.5 | Như trên |
| GĐ 4 Ghép nối, gồm `employees.json` đầy đủ | Sonnet 5.5 | Observer, khảo sát, báo cáo; chạy tiện ích băm mật khẩu |
| GĐ 4 Bảy file dữ liệu mẫu còn lại (phiên riêng) | Haiku 5.5 | Sinh JSON theo cấu trúc Record có sẵn |
| Review cuối GĐ 2, 3, 4 | Opus 5.5, chạy `/code-review` | Bắt lỗi thiết kế mà model nhỏ bỏ sót; phiên Sonnet của giai đoạn đó sửa |
| GĐ 5: UML, `viva-notes.md` | Sonnet 5.5 | Phải đọc code và đối chiếu tên |
| GĐ 5: README, `demo-script.md` (phiên riêng) | Haiku 5.5 | Văn bản theo khuôn |
| GĐ 6 Báo cáo | Sonnet 5.5; rà đối chiếu cuối bằng Opus 5.5 | Viết dài theo mẫu; rà cuối cần chính xác |

Không giao cho Haiku: code State/Observer, file `.Designer.cs`, chuỗi băm mật khẩu, và bất cứ việc gì đổi chữ ký.

**Cấu trúc instruction**

```
CLAUDE.md                     # luôn được nạp, tối đa ~80 dòng
.claude/
  settings.json               # cho phép sẵn dotnet build/test/run/publish/restore, git status/diff/log/show; tắt dòng Co-Authored-By
  rules/                      # mỗi file có frontmatter `paths:`, chỉ nạp khi làm việc với thư mục đó
    domain.md                 # src/HotelCRM.Domain/**
    business.md               # src/HotelCRM.Business/**
    data.md                   # src/HotelCRM.Data/**
    winforms.md               # src/HotelCRM.WinForms/**
    tests.md                  # tests/**
  skills/export-patch/SKILL.md   # chỉ tạo trong GĐ 3 nếu suất B dùng cách 1
docs/
  PLAN.md                     # đặc tả (file này)
  PROGRESS.md                 # trạng thái
  tasks/stage-N-*.md          # bản giao việc từng giai đoạn
```

| File | Nội dung |
|---|---|
| `CLAUDE.md` | Thứ tự đọc; lệnh build/test/run/publish; chiều tham chiếu project và 5 interface ranh giới; ngôn ngữ; phân công theo file; luật làm việc; điều kiện xong. Không chép nội dung plan |
| `rules/domain.md` | `private set`; validate trong constructor/method; `Restore` mức `internal`; không gọi `DateTime.Now`, không đọc cấu hình, không gắn attribute JSON; hai phân hệ không dùng lớp của nhau; loại ngoại lệ |
| `rules/business.md` | Service chỉ nhận interface qua constructor; "service tra cứu, Domain quyết định"; nhận `actor` và kiểm tra quyền; không gọi service của phân hệ kia |
| `rules/data.md` | Quy ước Record/Mapper; tên file JSON và thứ tự nạp; cấp Id; `AppConfig` chỉ được gọi trong `Program.cs`; `Data/` là phần chung, viết xong ở GĐ 1 |
| `rules/winforms.md` | Form chỉ gọi service và truyền `actor`; handler bọc trong `ErrorHandler.Run`; `InitializeComponent()` chỉ gán thuộc tính; chép cấu trúc `LoginForm`; không sửa `Program.cs` và `MainForm` sau GĐ 1 |
| `rules/tests.md` | Tên `Method_Condition_Expected`; dùng `InMemoryRepository`; `now` cố định; không sửa test có sẵn; thư mục và lớp giả của từng suất |
| `docs/PROGRESS.md` | Giai đoạn đã xong; việc đang dở; cách commit của suất B; quyết định lệch khỏi plan; câu hỏi chờ bạn. Mỗi phiên đọc đầu tiên và cập nhật trước khi kết thúc |
| `docs/tasks/stage-N-*.md` | Tự đủ cho một phiên: mục tiêu; danh sách module đánh số; file được tạo/sửa; file cấm đụng; quy tắc nghiệp vụ kèm con số; test phải xanh; các mục PLAN cần đọc; cách kiểm tra bằng tay; điều kiện xong |

**Luật làm việc cho mọi phiên** (ghi trong `CLAUDE.md`):
- Không tự commit; commit khi bạn đồng ý. Commit message không kèm dòng `Co-Authored-By`.
- Code suất B làm trên `local/b-work` và xuất patch (mục Git), trừ khi `docs/PROGRESS.md` ghi thành viên B tự code.
- Không đổi tên lớp, chữ ký public, cấu trúc thư mục của khung, và không sửa test có sẵn. Cần đổi thì dừng, ghi lý do vào `PROGRESS.md` và hỏi bạn.
- Dừng sau mỗi module. Một module là một dòng trong danh sách đánh số của file task. Khi dừng: tóm tắt thiết kế và điểm cần nhớ khi vấn đáp.
- Điều kiện xong: `dotnet build` không warning, test của giai đoạn xanh, `PROGRESS.md` đã cập nhật.

Câu mở đầu mỗi phiên: "Đọc `CLAUDE.md`, `docs/PROGRESS.md` và `docs/tasks/stage-N-….md` rồi làm theo."

## Đóng gói
`dotnet publish src/HotelCRM.WinForms -c Release -r win-x64 --self-contained -p:PublishSingleFile=true` → một file `.exe` kèm `appsettings.json` và thư mục `Data/`.

## Kiểm tra (Verification)
- `dotnet build` không lỗi, không warning; `dotnet test` đủ 22 test pass.
- Sau mỗi giai đoạn có giao diện (1–4), bạn tự mở app, bấm thử các Form của giai đoạn đó và mở từng Form bằng Designer của Visual Studio; model không tự kiểm tra được hai việc này.
- Kịch bản đầu-cuối: Manager đăng nhập → tạo ticket bảo trì cho khách VIP đang ở → tự phân công → Agent đăng nhập → Start → Resolve → khảo sát → Manager Close → xem lịch sử tương tác và điểm thưởng tự cập nhật → xem báo cáo.
- Thử vi phạm từng quy tắc 1–11 trên giao diện, mỗi lần phải hiện đúng thông báo, không crash. Riêng quy tắc 4 và 8 là phép tính: kiểm tra hạn xử lý và điểm/hạng hiển thị đúng.
- Copy thư mục publish sang ổ/máy khác, chạy được với dữ liệu mẫu.
- Đổi `DefaultAssignmentStrategy` trong `appsettings.json`, hành vi phân công đổi mà không build lại.
- `git shortlog -sn`: mỗi tác giả ít nhất 12 commit, tỷ lệ không lệch quá 60/40.
- Đối chiếu tên lớp/hàm trong UML và báo cáo với code.

## Phần cố ý chưa chi tiết trong plan
Sẽ được chốt ở giai đoạn ghi bên cạnh, không cần hỏi lại trước khi bắt đầu:
- Thuộc tính và chữ ký hàm của từng lớp: chốt bằng code khung + chú thích XML ở GĐ 1, rồi chép vào UML và báo cáo.
- Bố cục từng Form: theo form mẫu của GĐ 1; chi tiết ghi trong file task của GĐ 2–4.
- Dàn ý báo cáo: theo mẫu LaTeX, ghi trong `docs/tasks/stage-6-report.md`.
- Slide thuyết trình: chưa nằm trong phạm vi; nếu cần thì thêm vào GĐ 6.

## Việc còn chờ người
- Bạn: báo cho thành viên B hai thay đổi so với bản tóm tắt đã gửi — Strategy chuyển sang suất B (Repository + Singleton thành phần chung), và hạn chọn cách commit là 17/11.
- Thành viên B: tên tài khoản GitHub và cách commit (nhận patch hay tự code) — trước 17/11.
- Mẫu báo cáo LaTeX — trước 26/11.
- Bạn: kiểm tra máy có Java (xuất ảnh PlantUML cục bộ) và bản TeX (biên dịch báo cáo); thiếu thì dùng Overleaf — trước 26/11.

## Bước tiếp theo
Xem `docs/PROGRESS.md`. Giai đoạn kế tiếp là GĐ 1: mở phiên mới bằng model mạnh, làm theo `docs/tasks/stage-1-skeleton.md`, dừng ở điểm duyệt của GĐ 1.

