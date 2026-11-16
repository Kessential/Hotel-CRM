# Tiến độ dự án Hotel CRM

Mỗi phiên đọc file này đầu tiên và cập nhật trước khi kết thúc. Ghi ngắn, đúng sự thật: việc chưa kiểm tra thì ghi là chưa kiểm tra. File này chỉ ghi trạng thái; thiết kế và lộ trình nằm ở `docs/PLAN.md`, cách làm từng giai đoạn nằm ở `docs/tasks/`.

## Trạng thái các giai đoạn

| GĐ | Hạn | Trạng thái | File task |
|---|---|---|---|
| 0 Chuẩn bị | 16/11 | Xong, đã commit và push trên `develop` | — |
| 1 Khung | 17/11 | Đang làm: xong Module 5/6, còn Module 6 | `docs/tasks/stage-1-skeleton.md` |
| 2 Suất A | 20/11 | Chưa bắt đầu | viết ở cuối GĐ 1 |
| 3 Suất B | 23/11 | Chưa bắt đầu | viết ở cuối GĐ 1 |
| 4 Ghép nối | 25/11 | Chưa bắt đầu | viết ở cuối GĐ 1 |
| 5 Tài liệu kỹ thuật | 27/11 | Chưa bắt đầu | viết ở cuối GĐ 4 |
| 6 Báo cáo & nộp | 29/11 | Chưa bắt đầu | viết ở cuối GĐ 5 |

## Việc tiếp theo
GĐ 1, Module 6 (viết `stage-2-ticket.md`, `stage-3-customer.md`, `stage-4-integration.md`; rà 5 file `.claude/rules/`; sửa `docs/PLAN.md` theo bảng "Quyết định lệch khỏi plan") trong `docs/tasks/stage-1-skeleton.md`. Phiên 16/11 dừng trước Module 6 vì hết hạn mức sử dụng. Sau Module 6: merge `develop` vào `local/b-work`. Chủ repo đã cho phép (16/11) làm lần lượt Module 2–6, commit sau mỗi module rồi báo cáo.

## Cách commit của suất B
**Nhận patch** (mặc định). Thành viên B chưa xác nhận; hạn 17/11. Nếu họ chọn tự code, sửa dòng này thành "Tự code".

## Tiện ích băm mật khẩu
`tools/HashPassword.cs` là file-based app của .NET 10, không nằm trong solution, gọi thẳng `PasswordHasher` của project `Business`. Chạy từ thư mục gốc repo:

```
dotnet run tools/HashPassword.cs -- <mật khẩu 1> [<mật khẩu 2> ...]
```

Mỗi mật khẩu in ra một cặp `"PasswordHash"` và `"Salt"` để dán vào `src/HotelCRM.WinForms/Data/employees.json`. Mỗi lần chạy sinh salt mới nên chuỗi băm khác nhau dù cùng mật khẩu. GĐ 4 dùng lại lệnh này cho `employees.json` đầy đủ. Mật khẩu của tài khoản mẫu ghi trong mục "Sample accounts" của `README.md` (viết ở Module 4).

## Trạng thái Git
- `main`: 1 commit ("first commit"), chỉ có `README.md`.
- `develop`: đã tạo từ `main` ở GĐ 0, có thêm 1 commit chứa `CLAUDE.md`, `.gitignore`, `.claude/`, `docs/`. Đã push lên `origin/develop` ngày 16/11.
- `develop` (cục bộ) đi trước `origin/develop` các commit của GĐ 1 Module 1–5; chưa push.
- `local/b-work`: tạo ngày 16/11 từ `develop`, chỉ có trên máy chủ repo, không bao giờ push.
- `docs/TEAM-BRIEF.md` đã gửi cho thành viên B và nằm trong `.gitignore`; không commit, không cập nhật.

## Việc đang dở
- GĐ 1: Module 1–5 xong và đã commit (chưa push). Module 6 chưa làm: chưa có file task GĐ 2–4, chưa rà rules, chưa sửa `docs/PLAN.md`.
- `local/b-work` đã tạo từ `develop` (commit `1386d6f`), có 1 commit chứa 11 test của suất B và `FakeTicketQuery`. Patch số 01 nằm ở `handoff/01-customer-tests.patch` kèm `.msg.txt` và `handoff/README.txt`; `git apply --check` trên `develop` đạt. Thành viên B chỉ áp được sau khi `develop` được push.
- Chủ repo chưa làm 6 bước kiểm tra bằng tay của GĐ 1 (mục "Người dùng kiểm tra bằng tay" trong file task). Riêng bước 5 (mở Form bằng Designer của Visual Studio) không có cách kiểm tra tự động nào thay được.

## Quyết định lệch khỏi plan
Chốt ở GĐ 1 Module 2 khi viết chữ ký. `docs/PLAN.md` sẽ được sửa cho khớp ở Module 6.

| # | Plan ghi | Code thật | Lý do |
|---|---|---|---|
| 1 | `IAssignmentStrategy.Pick(ticket, agents)` | `Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)` trả về `Agent?` | Test strategy của suất B phải xanh ở GĐ 3 khi chưa có code ticket của suất A; strategy cũng cần số ticket đang mở của từng Agent. `candidates` chỉ gồm Agent còn nhận được (`Agent.CanTake`) |
| 2 | Interface 1–4 hàm | `IRepository<T>` có 5 hàm: `GetAll`, `GetById`, `NextId`, `Save`, `Delete` | Cần đủ cả cấp Id và xóa khách (UC6) |
| 3 | `Domain/Interfaces` gồm 7 kiểu | Thêm `CustomerSummary` (record) và `RepositoryExtensions.GetRequired` | Form ticket phải chọn khách và hiện tên khách mà không dùng `Customer`; "không tìm thấy" do Domain ném, service không tự ném |
| 4 | Observer ném `NotImplementedException` tới GĐ 4 | `InteractionLogger.OnStateChanged` và `LoyaltyObserver.OnStateChanged` để thân rỗng | Điểm duyệt chung UC1–UC7 ở đầu GĐ 4 chạy trước khi observer được cài; nếu ném lỗi thì UC4 không chạy được |
| 5 | `Employee.CanAssign()/CanClose()/CanReopen()` | Thêm `EnsureCanAssign()/EnsureCanClose()/EnsureCanReopen()` (ném `BusinessRuleException`) và `Employee.EnsureLoginValid(account, passwordMatches)` | Giữ nguyên tắc "service tra cứu, Domain quyết định": service không tự ném lỗi quyền hay lỗi đăng nhập |
| 6 | `Start()`, `Close()` không tham số thời gian | `Start(now)`, `Resolve(note, now)`, `Close(now)`, `Reopen(now, reopenDays)`; ticket có `UpdatedAt` | Observer cần thời điểm đổi trạng thái mà `OnStateChanged(ticket, from, to)` không mang theo |
| 7 | `Booking.CheckIn`/`CheckOut` vừa là ngày vừa là hàm | Thuộc tính `CheckInDate`, `CheckOutDate`; hàm `CheckIn()`, `CheckOut()`, `Cancel()` | C# không cho thuộc tính và hàm trùng tên |
| 8 | `Department (Id, Code, Name)`, `RoomType` | Thêm `Department.Agents` và `RoomType.Rooms` (Agent/Room tự đăng ký khi được tạo) | Để quan hệ kết tập `Department ◇— Agent`, `RoomType ◇— Room` có thật trong code |
| 9 | (chưa nêu) | Kiểu phụ: `TicketSnapshot` (internal, dùng cho `Restore`), `AgentWorkload`, `AgentPerformanceRow`, `DepartmentReportRow` | Gói tham số của `Restore`; kiểu trả về của `ReportService` |

Chỗ plan để ngỏ đã chốt: khách hàng (UC6) nhập `FullName`, `Phone`, `Email`, `IdentityNumber`, `Nationality` (bắt buộc) và `Notes` (tùy chọn); `ContactChannel` gồm `FrontDesk`, `Phone`, `Email`, `System`; `TicketFactory.Create(id, customerId, title, description, priority, slaProfile, createdAt, roomId)`.

## Câu hỏi và việc đang chờ người
| Việc | Cần trước | Kết quả |
|---|---|---|
| Chủ repo báo cho thành viên B: Strategy chuyển sang suất B, hạn chọn cách commit là 17/11 (khác bản tóm tắt đã gửi) | 17/11 | |
| Tên tài khoản GitHub của thành viên B | 17/11 | |
| Thành viên B chọn cách commit: nhận patch hay tự code | 17/11 | |
| Mẫu báo cáo LaTeX | 26/11 | |
| Máy có Java (xuất ảnh PlantUML) và bản TeX không | 26/11 | |

## Nhật ký
- 15/11/2026 — GĐ 0: tạo `CLAUDE.md`, `.gitignore`, `.claude/settings.json`, 5 file `.claude/rules/`, file này và `docs/tasks/stage-1-skeleton.md`. Các file rules viết khi chưa có code, phải rà lại ở cuối GĐ 1.
- 15/11/2026 — Rà soát và sửa plan + instruction theo 34 điểm. Các thay đổi chính: Strategy sang suất B, Repository + Singleton và toàn bộ `Data/` thành phần chung; quy tắc đặt theo "service tra cứu, Domain quyết định"; ticket không dùng `MembershipTier` mà nhận `CustomerSlaProfile`; GĐ 1 viết luôn `Program.cs` và vỏ 9 Form; ma trận phân quyền của quy tắc 11; commit không kèm `Co-Authored-By`. Chưa có dòng code nào; quy tắc `PowerShell(...)` trong `settings.json` chưa được chạy thử.
- 16/11/2026 — Tạo nhánh `develop` từ `main` và commit toàn bộ file GĐ 0 trên `develop` (commit số 1 trong danh sách đề xuất của `stage-1-skeleton.md`). GĐ 1 không cần tạo lại nhánh, chỉ cần chuyển sang `develop`.
- 16/11/2026 — GĐ 1 Module 1 (SDK 10.0.403): tạo `HotelCRM.sln` (định dạng `sln`), 5 project với tham chiếu đúng bảng trong file task, `Directory.Build.props`, `.editorconfig`; xóa `Class1.cs`, `UnitTest1.cs`, `Form1`. `InternalsVisibleTo("HotelCRM.Data")` khai báo bằng item `<InternalsVisibleTo>` trong `HotelCRM.Domain.csproj`. Gói test là bộ mặc định của mẫu `xunit` (xunit 2.9.3, xunit.runner.visualstudio 3.1.4, Microsoft.NET.Test.Sdk 17.14.1, coverlet.collector 6.0.4). Đã kiểm tra: `dotnet build HotelCRM.sln` 0 lỗi 0 warning; `dotnet test` chạy được, chưa có test nào. Chưa kiểm tra: mở solution bằng Visual Studio. Quy tắc đặt tên trong `.editorconfig` chỉ hiện trong IDE, không chặn `dotnet build` (không bật `EnforceCodeStyleInBuild` vì file task chỉ nêu 4 thuộc tính cho `Directory.Build.props`).
- 16/11/2026 — GĐ 1 Module 2: khung `Domain` (Common, Staff, Interfaces, Tickets + States, Customers, Hotel) và `Business` (Services, Queries, Assignment, Factories, Observers, Security), mọi thành viên public có chú thích XML. Cài thật: `Person`, `Employee`, `Agent`/`Manager` (trừ `CanTake`/`EnsureCanTake`), `Department`, `BusinessSettings`, 3 ngoại lệ, mọi `Restore`, `TicketState.FromName`, `MembershipTier.FromName`, `SupportTicket.Attach`, `PasswordHasher`, `UserSession`, `AuthService`, constructor của mọi service. Phần còn lại ném `NotImplementedException` (riêng 2 observer để thân rỗng, xem bảng lệch plan). Đã kiểm tra: build 0 lỗi 0 warning. Chưa kiểm tra bằng test (22 test thuộc Module 5). Ba commit: lõi chung + ticket, khách hàng + khách sạn, Business.
- 16/11/2026 — GĐ 1 Module 3: `AppConfig` (Singleton), `IMapper`, `JsonRepository`, `InMemoryRepository`, 9 record (8 file + `InteractionRecord`), 8 mapper, `appsettings.json`, dữ liệu mẫu tối thiểu (4 phòng ban, 1 manager, 2 agent, 6 file `[]`), `tools/HashPassword.cs`. `appsettings.json` và `Data/*.json` được chép ra thư mục output và đặt ngoài file `.exe` khi publish single-file. Đã kiểm tra bằng một script tạm ngoài repo (31 phép thử, đều đạt): nạp cả 8 file, tham chiếu trỏ đúng cùng đối tượng, ghi rồi nạp lại cho JSON giống hệt, xóa có ghi file, đăng nhập đúng/sai bằng tài khoản mẫu, JSON hỏng / thiếu file / tên trạng thái lạ đều báo lỗi có tên file. Chưa kiểm tra: publish single-file.
- 16/11/2026 — GĐ 1 Module 4: `Program.cs` (composition root, bảng strategy và factory, `Application.ThreadException`, vòng đăng nhập lại sau Logout), `ErrorHandler` (`Run` trả về `bool`, `Handle`, `ReportStartupFailure`), `UiNotificationObserver` (sự kiện `NotificationRaised`, `MainForm` hiện lên thanh trạng thái), `LoginForm`, `MainForm`, vỏ 9 Form, mục "Sample accounts" trong `README.md`. Các Form con mở dạng hộp thoại (`ShowDialog`). Menu: Reports chỉ Manager thấy, Record Survey chỉ Agent thấy. `.editorconfig` bỏ quy tắc PascalCase cho hàm trong `Forms/` vì tên handler theo kiểu Visual Studio (`btnLogin_Click`). Đã kiểm tra bằng script tạm ngoài repo (29 phép thử, đều đạt): nối dây như `Program`, đăng nhập sai bị từ chối, đăng nhập 2 vai trò, menu ẩn/hiện đúng, cả 9 Form tạo được cửa sổ, Logout kết thúc phiên. Chạy file `.exe` thật: hiện cửa sổ "Hotel CRM - Log In"; đổi `MaxActiveTickets` vẫn khởi động; đổi `DefaultAssignmentStrategy` thành tên lạ thì hiện hộp thoại lỗi và ghi `logs/error.log` có nêu 3 tên hợp lệ. Chưa kiểm tra: bấm thật trên giao diện, mở Form bằng Designer của Visual Studio (chỉ người làm được).
- 16/11/2026 — GĐ 1 Module 5: 11 test suất A + `FakeCustomerDirectory`, `FakeStayLookup`, `TicketTestData` trên `develop`; 11 test suất B + `FakeTicketQuery`, `CustomerTestData` trên `local/b-work`; xuất `handoff/01-customer-tests.*` và `handoff/README.txt`. Đã kiểm tra: `dotnet test` trên `develop` 11 test, cả 11 đỏ; trên `local/b-work` 22 test, 2 xanh (`Constructor_InvalidEmail_ThrowsValidation`, `Constructor_InvalidPhone_ThrowsValidation`), 20 đỏ. Mọi test đỏ đều do `NotImplementedException` (2 test hiện "Assert.Throws: not an exact match" vì lời gọi bị bọc trong `Assert.Throws`, ngoại lệ thật vẫn là `NotImplementedException`). `TicketServiceTests` dùng một strategy giả lồng trong lớp test để không phụ thuộc strategy của suất B.
