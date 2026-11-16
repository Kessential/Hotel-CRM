# GĐ 1 — Khung dự án

Hạn: 17/11/2026. Model: Fable 5.2 hoặc Opus 5.5. Nhánh: `develop` (và `local/b-work` cho test của suất B). Đây là giai đoạn quyết định chữ ký của toàn bộ hệ thống; các phiên sau chỉ điền thân hàm và nội dung Form.

## Mục tiêu
Sau giai đoạn này:
1. Solution build sạch, app chạy được: đăng nhập bằng tài khoản Manager và Agent, `MainForm` hiện menu theo vai trò, mở được 9 Form rỗng từ menu.
2. Toàn bộ `Data/`, `Program.cs`, `MainForm`, `ErrorHandler` đã hoàn chỉnh. Các giai đoạn sau không sửa những file này.
3. Mọi lớp và interface của `Domain` và `Business` đã tồn tại với chữ ký cuối cùng và chú thích XML. Phần nghiệp vụ của GĐ 2–4 có thân hàm `throw new NotImplementedException();`.
4. 22 test đã viết: 2 test xanh, 20 test đỏ vì `NotImplementedException`.
5. Có file task cho GĐ 2, 3, 4 để phiên Sonnet làm tiếp mà không cần đọc cả plan.

## Đọc trước
- Cả 5 file trong `.claude/rules/`. Chúng chưa tự nạp vì `src/` và `tests/` còn trống.
- Toàn bộ `docs/PLAN.md`, một lần. Các mục dùng nhiều nhất: Phân công; Kiến trúc; Lớp thực thể; Quan hệ UML; Design Pattern; Quy tắc nghiệp vụ; Use Case; Ngoại lệ, cấu hình, quy ước; Quyết định thiết kế bổ sung; Kiểm thử; Git.

## Các module
Mỗi mục dưới đây là một module: làm xong thì dừng, tóm tắt, chờ duyệt. Hỏi người dùng trước khi tạo nhánh và trước mỗi lần commit. Commit message không kèm dòng `Co-Authored-By`.

### Module 1 — Solution và project
- Chuyển sang nhánh `develop` (đã tạo từ `main` ở GĐ 0, đã có commit tài liệu và instruction).
- `dotnet new sln -n HotelCRM --format sln` (định dạng `sln` để Visual Studio bản cũ hơn của thành viên B vẫn mở được).
- Project:

| Project | Mẫu | Target | Tham chiếu |
|---|---|---|---|
| `src/HotelCRM.Domain` | `classlib` | `net10.0` | không |
| `src/HotelCRM.Business` | `classlib` | `net10.0` | Domain |
| `src/HotelCRM.Data` | `classlib` | `net10.0` | Domain |
| `src/HotelCRM.WinForms` | `winforms` | `net10.0-windows` | Business, Data |
| `tests/HotelCRM.Tests` | `xunit` | `net10.0` | Domain, Business, Data |

- `Directory.Build.props` ở gốc: `Nullable=enable`, `TreatWarningsAsErrors=true`, `LangVersion=latest`, `ImplicitUsings=enable`.
- `.editorconfig` theo mục "Ngoại lệ, cấu hình, quy ước" của plan.
- `Domain` khai báo `InternalsVisibleTo("HotelCRM.Data")`.
- Xóa các file mẫu (`Class1.cs`, `UnitTest1.cs`, `Form1`).

### Module 2 — Khung Domain và Business (chữ ký của mọi lớp)
Tạo mọi lớp trong mục "Kiến trúc" và "Lớp thực thể" của plan.

Cài thật ngay (phần chung, và mọi thứ chạy lúc khởi động):
- `Domain/Common`: `Person` (gồm kiểm tra định dạng SĐT/Email), `Employee`, `EmployeeRole`, ba lớp ngoại lệ, `BusinessSettings`.
- `Domain/Staff`: `Agent`, `Manager` (override `CanAssign()`, `CanClose()`, `CanReopen()`), `Department`. Riêng `Agent.EnsureCanTake` để `NotImplementedException` (quy tắc 3, suất A).
- `Domain/Interfaces`: `IEntity`, `IRepository<T>`, `ITicketObserver`, `ICustomerDirectory`, `CustomerSlaProfile`, `IStayLookup`, `ITicketQuery`.
- Ở mọi thực thể: field, thuộc tính, hàm `internal static Restore(...)`, `TicketState.FromName`, `MembershipTier.FromName`, `SupportTicket.Attach`.
- Ở mọi service, lớp Queries, observer, strategy, factory: constructor lưu phụ thuộc.
- `Business/Security`: `PasswordHasher` (thông số ở mục "Con số và định nghĩa nghiệp vụ"), `UserSession`. `AuthService.Login()` và `Logout()`.

Để `throw new NotImplementedException();`:
- Mọi phương thức nghiệp vụ của GĐ 2–4.
- Constructor public có quy tắc (ví dụ `Booking`, `Customer`, `SatisfactionSurvey.Create`): gán field rồi gọi một hàm kiểm tra `private`; hàm đó ném `NotImplementedException`. Nhờ vậy test dựng đối tượng sẽ đỏ đúng lý do.

Yêu cầu cho từng thành viên public:
- Chữ ký cuối cùng: tên, tham số, kiểu trả về.
- Chú thích XML bằng tiếng Anh: làm gì, quy tắc số mấy, ném ngoại lệ loại nào và khi nào, con số cụ thể.
- `Domain/Tickets` không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`.

Chốt nốt những chỗ plan để ngỏ, ghi vào chú thích XML và file task tương ứng:
- Các trường nhập của khách hàng (UC6).
- Tham số đầy đủ của `TicketFactory.Create(...)`, `IAssignmentStrategy.Pick(...)`, `ICustomerDirectory`, `IStayLookup`, `ITicketQuery`.
- Kiểu trả về của hai hàm trong `ReportService`.

### Module 3 — Tầng Data
- `AppConfig` (Singleton), `IMapper<TEntity, TRecord>`, `JsonRepository<TEntity, TRecord>`, `InMemoryRepository<T>`.
- Record và mapper cho cả 8 kiểu có file JSON; mapper dùng `Restore`.
- `appsettings.json`: `DataFolder`, `DefaultAssignmentStrategy`, `MaxActiveTickets` (5), `ReopenDays` (7).
- Dữ liệu mẫu tối thiểu trong `src/HotelCRM.WinForms/Data/`: 4 phòng ban (`GUEST`, `HOUSE`, `ENG`, `FRONT`), 1 manager, 2 agent; sáu file còn lại là mảng rỗng `[]`.
- Tiện ích băm mật khẩu: một lệnh chạy lại được (GĐ 4 sẽ dùng lại), không thêm project vào solution. Ghi cách dùng vào `docs/PROGRESS.md`. Chuỗi băm trong `employees.json` sinh bằng tiện ích này.

### Module 4 — Giao diện khung
- `Program.cs`: nối dây đầy đủ theo thứ tự ở mục "Khởi tạo" của plan, gồm bảng strategy (`LeastLoaded`, `DepartmentMatch`, `RoundRobin`) và bảng factory. Đăng ký `Application.ThreadException`.
- `ErrorHandler`.
- `LoginForm`, `MainForm` (menu theo vai trò, có Logout). Đây là form mẫu cho mọi Form về sau: làm cẩn thận, đúng `.claude/rules/winforms.md`.
- Vỏ rỗng của 9 Form (`TicketListForm`, `TicketEditForm`, `TicketDetailForm`, `CustomerListForm`, `CustomerEditForm`, `CustomerHistoryForm`, `SurveyForm`, `BookingForm`, `ReportForm`): có `.Designer.cs`, tiêu đề, và constructor nhận đúng các service mà Form sẽ dùng. `MainForm` mở các Form này từ menu.
- Ghi tài khoản mẫu vào mục "Sample accounts" của `README.md`.
- App khởi động và mở được cả 9 Form mà không gặp `NotImplementedException`.

### Module 5 — 22 test
- Viết đúng danh sách ở mục "Kiểm thử" của plan; tên hàm chỉnh được cho khớp chữ ký, số lượng và nội dung giữ nguyên.
- Trên `develop`: 11 test của suất A trong `tests/HotelCRM.Tests/Tickets/`, cùng `FakeCustomerDirectory` và `FakeStayLookup`. Test của suất A không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`.
- Tạo nhánh `local/b-work` từ `develop`. Commit đầu tiên trên nhánh này: 11 test của suất B trong `Customers/` và `Assignment/`, cùng `FakeTicketQuery`. Xuất thành `handoff/01-customer-tests.patch` và `handoff/01-customer-tests.msg.txt`. Không bao giờ push nhánh này.
- Kết quả `dotnet test` trên `local/b-work`: 22 test; `Constructor_InvalidEmail_ThrowsValidation` và `Constructor_InvalidPhone_ThrowsValidation` xanh; 20 test còn lại đỏ và lý do duy nhất là `NotImplementedException`. Test đỏ vì lý do khác là lỗi của test hoặc của khung, phải sửa.

### Module 6 — File task và rà lại
Viết `docs/tasks/stage-2-ticket.md`, `stage-3-customer.md`, `stage-4-integration.md`. Mỗi file tự đủ cho một phiên Sonnet và gồm:
- Mục tiêu và nhánh làm việc.
- Danh sách module đánh số (mỗi module là một điểm dừng).
- File được sửa (đường dẫn thật) và file cấm đụng: `Program.cs`, `MainForm`, `ErrorHandler`, `Data/**`, mọi chữ ký, mọi test.
- Quy tắc nghiệp vụ liên quan, chép kèm con số và định nghĩa.
- Tên các test phải xanh.
- Bố cục từng Form: control nào, gọi hàm service nào.
- Các mục của `docs/PLAN.md` cần đọc (ít nhất có thể).
- Các bước người dùng kiểm tra bằng tay.
- Điều kiện xong.

Riêng từng file:
- `stage-3-customer.md`: làm trên `local/b-work`, xuất patch theo mục "Git" của plan, trừ khi `docs/PROGRESS.md` ghi thành viên B tự code. Gồm cả 3 strategy.
- `stage-4-integration.md`: mở đầu bằng việc merge `develop` (đã có suất A) vào `local/b-work` và điểm duyệt chung UC1–UC7 trên giao diện; `employees.json` đầy đủ sinh bằng tiện ích băm; bảy file dữ liệu còn lại tách thành phần việc cho phiên Haiku.

Rà lại:
- Sửa 5 file trong `.claude/rules/` những chỗ không khớp với code thật.
- Nếu có gì lệch so với `docs/PLAN.md` (tên lớp, tham số), sửa plan cho khớp code và ghi vào mục "Quyết định lệch khỏi plan" của `docs/PROGRESS.md`.
- Cập nhật `docs/PROGRESS.md`.

## Không làm trong giai đoạn này
- Không cài thân hàm của quy tắc nghiệp vụ 1–10 (trừ phần định dạng của quy tắc 6 trong `Person`), State, Strategy, Factory, Observer.
- Không làm nội dung của 9 Form.
- Không tạo dữ liệu mẫu đầy đủ.
- Không vẽ UML, không viết báo cáo.
- Không push, không tạo PR.

## Đề xuất commit (chỉ thực hiện khi người dùng đồng ý)
Trên `develop`, bằng tài khoản của chủ repo, mỗi dòng một commit:
1. Tài liệu và instruction (`CLAUDE.md`, `.claude/`, `docs/`, `.gitignore`) — đã commit ở GĐ 0, bỏ qua.
2. Solution, 5 project, `Directory.Build.props`, `.editorconfig`.
3. `Domain/Common`, `Domain/Staff`, `Domain/Interfaces`.
4. Khung của `Domain/Tickets`, `Domain/Customers`, `Domain/Hotel`.
5. Khung của tầng Business, `PasswordHasher`, `UserSession`, `AuthService`.
6. Tầng Data và dữ liệu mẫu tối thiểu.
7. `Program.cs`, `ErrorHandler`, `LoginForm`, `MainForm`.
8. Vỏ của 9 Form.
9. 11 test của suất A và hai lớp giả.
10. File task GĐ 2–4 và cập nhật `PROGRESS.md`.

Trên `local/b-work`: một commit chứa 11 test của suất B và `FakeTicketQuery`.

## Người dùng kiểm tra bằng tay
1. Chạy `dotnet run --project src/HotelCRM.WinForms`.
2. Đăng nhập sai mật khẩu: hiện thông báo lỗi, app không thoát.
3. Đăng nhập bằng tài khoản Manager: thấy đủ menu, kể cả Reports; mở lần lượt 9 Form rỗng.
4. Logout, đăng nhập bằng tài khoản Agent: không thấy các mục chỉ dành cho Manager.
5. Mở `LoginForm`, `MainForm` và vài Form rỗng bằng Designer trong Visual Studio: hiển thị được, không báo lỗi.
6. Sửa `MaxActiveTickets` trong `appsettings.json` cạnh file chạy rồi chạy lại: app vẫn khởi động. Đổi `DefaultAssignmentStrategy` thành một tên lạ: app báo lỗi rõ ràng lúc khởi động.

## Điều kiện xong
- `dotnet build HotelCRM.sln`: 0 lỗi, 0 warning.
- `dotnet test` trên `develop`: 11 test, tất cả đỏ vì `NotImplementedException`. Trên `local/b-work`: 22 test, 2 xanh, 20 đỏ vì `NotImplementedException`.
- Sáu bước kiểm tra bằng tay đã được người dùng xác nhận.
- Ba file task GĐ 2–4 và `handoff/01-…` đã có.
- `docs/PROGRESS.md` đã cập nhật.
