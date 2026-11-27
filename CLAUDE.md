# Hotel CRM (v2)

Bài tập lớn môn HTTTQL (MI4090), nhóm 2 người: CRM (quản lý dịch vụ chăm sóc khách hàng) áp dụng cho khách sạn. C# / .NET 10, ASP.NET Core Razor Pages (web chạy tại máy, mở bằng trình duyệt), SQLite, test xUnit. Ngày 26/11 thầy yêu cầu đổi giao diện WinForms sang web; các quyết định hai người chốt ngày 27/11 nằm ở `docs/PLAN.md` mục 3.1. Chấm theo sheet "Nhóm 02 người" của `docs/slide_checklist/Checklist_BTL_HTTTQL_v2026.xlsx`; quy ước đặt tên theo `docs/slide_checklist/HuongdanXDHTTTQLv1_1.pdf`. Hai file này là tài liệu của giảng viên, **không đưa lên repo** (repo public): mỗi người tự giữ một bản ở `docs/slide_checklist/` trên máy mình; thư mục này nằm trong `.gitignore`.

## Đọc trước khi làm
1. `docs/PROGRESS.md` (v2): module nào đã xong, việc đang dở, quyết định lệch khỏi plan, việc chờ người.
2. File task của giai đoạn đang làm trong `docs/tasks/` (P1: `docs/tasks/v2-p1-analysis.md`).
3. `docs/PLAN.md` (v2): chỉ mở những mục mà file task nêu tên. Không đọc cả file.

Tài liệu v1 (`docs/PLAN.md`, `docs/PROGRESS.md`, `docs/tasks/stage-*.md` bản cũ) đã xóa khỏi cây thư mục, chỉ còn trong lịch sử git ở commit `fd7b7e4` (ví dụ `git show fd7b7e4:docs/PLAN.md`). Chỉ để tra cứu, không làm theo.

Giai đoạn chuyển tiếp:
- `.claude/rules/*.md` còn viết cho v1 tới khi TV1 sửa ở P2 (`winforms.md` thay bằng `web.md`). Các tham chiếu `docs/PLAN.md` trong rules tạm hiểu là PLAN v1 (`git show fd7b7e4:docs/PLAN.md`). Chỗ nào rules mâu thuẫn với file này thì theo file này. Rules chỉ tự nạp khi đã có file khớp đường dẫn; khi tạo file đầu tiên trong một thư mục thì tự đọc file rules tương ứng trước.
- Code hiện tại vẫn là bản v1 (WinForms, lưu JSON, giao diện tiếng Anh, project `WinForms`/`Business`/`Data`). Chuyển sang web và SQLite, đổi tên project và giao diện tiếng Việt làm ở P2–P3 theo `docs/PLAN.md`.

## Người đang dùng phiên
- Xem `git config user.name`: `Kessential` là TV1, `munn8726` là TV2. Trống hoặc tên khác thì hỏi.
- Mỗi phiên chỉ viết code và tài liệu thuộc phần của người đang dùng phiên (mục "Phân công"). Phần của người kia chỉ đọc, review, góp ý; không sửa, không viết hộ. Làm hộ có thể đưa hệ số cá nhân về 0 (Khóa 3 của checklist).
- Code ai commit thì người đó phải giải thích được và tự viết lại được khi tắt AI. Khi viết code, nói rõ lý do thiết kế; giữ thiết kế tối giản, chỉ thêm thứ phục vụ một tiêu chí của checklist (không code chết, không pattern giả, không lớp rỗng).

## Lệnh
```
dotnet build HotelCRM.sln
dotnet test HotelCRM.sln
dotnet run --project src/HotelCRM.WinForms
dotnet publish src/HotelCRM.WinForms -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
dotnet run tools/HashPassword.cs -- <mật khẩu>
java -jar tools/plantuml.jar -tpng docs/analysis/diagrams/*.puml
```
- Tên project giữ như hiện tại tới P2. Ở P2 (1/12) đổi `HotelCRM.WinForms` → `HotelCRM.GUI` (ASP.NET Core Razor Pages, `net10.0`), `HotelCRM.Business` → `HotelCRM.BLL`, `HotelCRM.Data` → `HotelCRM.DAL`, bằng một commit chỉ có `git mv` và sửa `.sln`/`.csproj`. Từ đó lệnh chạy là `dotnet run --project src/HotelCRM.GUI` rồi mở `http://localhost:5080` (cổng trong `appsettings.json`, chỉ nghe localhost, HTTP); đóng gói `dotnet publish src/HotelCRM.GUI -c Release -r win-x64 --self-contained` (thêm `-r linux-x64` khi cần).
- `HashPassword` sinh `PasswordHash` và `Salt` (PBKDF2): hiện cho `employees.json`, từ P2 cho `Database/seed.sql`. Tài khoản mẫu ghi trong `README.md`.
- `tools/plantuml.jar` chưa có trong repo; tải khi người dùng đồng ý (xem "Việc chờ người" trong `docs/PROGRESS.md`).

## Kiến trúc (đích của v2)
```
Database/            schema.sql, seed.sql
Resource/            icon, logo
src/HotelCRM.GUI     Razor Pages (Pages/frm…), wwwroot/lib (Bootstrap 5, Bootstrap Icons, Chart.js, font); Program.cs = Composition Root
src/HotelCRM.BLL     Services, Queries, Observers, Segments, Factories, Reporting
src/HotelCRM.Domain  Common, Staff | Tickets, Surveys (TV1) | Customers, Hotel, Loyalty, Campaigns (TV2) | Interfaces
src/HotelCRM.DAL     Sqlite (kết nối, UnitOfWork, tạo CSDL), Repositories, ReadModels
tests/HotelCRM.Tests Unit/, Integration/, Fakes/
```
- Chiều phụ thuộc: `GUI → BLL → Domain ← DAL`. `Domain` không tham chiếu project nào. `GUI` tham chiếu `DAL` chỉ để `Program.cs` khởi tạo. `Tests` tham chiếu `Domain`, `BLL`, `DAL`. Trang (PageModel) không chứa SQL hay quy tắc nghiệp vụ; BLL không dùng `HttpContext`.
- Khởi tạo đối tượng bằng tay trong `Program.cs`, dựng theo từng request (mỗi request một `UnitOfWork`, vì `UnitOfWork` giữ transaction đang mở); container có sẵn của ASP.NET Core chỉ dùng để giao các đối tượng đã tạo cho trang. Không thêm thư viện DI. Thêm loại ticket, phân khúc hay observer mới chỉ cần thêm lớp và một dòng đăng ký ở `Program.cs`; không `switch` cứng theo tên.
- Cấu hình: một `appsettings.json` (+ `appsettings.example.json`), đọc một lần ở `Program.cs` rồi truyền qua constructor. Không Singleton cấu hình. Lấy thời gian qua `IClock`.
- Quy tắc nghiệp vụ: **service tra cứu, lớp Domain quyết định và ném ngoại lệ.** Cây ngoại lệ: `DomainException` → `ValidationException` (sai dữ liệu một đối tượng), `BusinessRuleException` (sai quy tắc giữa các đối tượng hoặc sai trạng thái), `AuthorizationException` (sai quyền). DAL bọc lỗi SQLite thành `DataAccessException`. Không `catch {}` rỗng.
- Phân quyền ở tầng Service: service nhận `actor` và gọi `actor.EnsureCan…()` trước khi làm. Quyền là hàm đa hình của 4 lớp con `Employee` (`Administrator`, `Manager`, `Receptionist`, `Agent`), không `is`/`as`. Ẩn hay khóa nút trên trang và `[Authorize]` chỉ là lớp chặn thứ hai.
- Đăng nhập bằng cookie: đọc mã nhân viên từ cookie, nạp `Employee`, truyền làm `actor`; không dùng biến tĩnh kiểu `UserSession`. Giữ antiforgery mặc định của Razor Pages; không dùng `Html.Raw` với dữ liệu người dùng nhập; cột sắp xếp lấy từ danh sách cho phép, không ghép chuỗi SQL.
- CSDL: SQLite qua `Microsoft.Data.Sqlite` (ADO.NET), mọi câu SQL có tham số. Bật `PRAGMA foreign_keys=ON` mỗi kết nối; bật `journal_mode=WAL` và `busy_timeout` vì nhiều request chạy cùng lúc. `IUnitOfWork.Execute(...)` dùng chung một connection và một transaction; lời gọi lồng nhau tham gia transaction ngoài. Luồng L2 (đóng ticket) và L3 (trả phòng) chạy trọn trong một transaction. Repository đọc CSDL khi cần, không nạp hết vào bộ nhớ. Giữa các aggregate tham chiếu bằng Id. Lưới và báo cáo dùng truy vấn JOIN, không N+1.
- Gói NuGet: ngoài bộ xUnit mặc định chỉ được dùng `Microsoft.Data.Sqlite`, `ClosedXML` (xuất .xlsx). Thư viện giao diện là file tĩnh trong `wwwroot/lib`, không dùng npm: Bootstrap 5, Bootstrap Icons, Chart.js (biểu đồ), font Cormorant Garamond và Be Vietnam Pro (giấy phép SIL OFL). Cần gói hay thư viện khác thì dừng và hỏi; phải được cả hai đồng ý.

### Ranh giới hai phân hệ
Hai phân hệ chỉ nói chuyện qua các interface dưới đây (trong `Domain/Interfaces`, tiêm qua constructor). Service của phân hệ này không gọi service hay bảng của phân hệ kia. Lớp Domain của phân hệ này không dùng lớp Domain của phân hệ kia. Phần của bên kia chưa xong thì dùng Fake.

| Interface | Khai báo | Cài đặt | Dùng cho |
|---|---|---|---|
| `ICustomerDirectory` | TV1 | TV2 (`CustomerDirectory`) | tìm khách, hệ số SLA/ưu tiên theo hạng, hạng theo lô (L1, R2) |
| `IStayLookup` | TV1 | TV2 (`StayLookup`) | khách có đang ở phòng này không (L1, Q4) |
| `ITicketObserver` | TV1 | TV2 (`GoodwillPointsObserver`, `InteractionLogger`) | sự kiện đổi trạng thái ticket (L2, Q12) |
| `ITicketQuery` | TV2 | TV1 (`TicketQuery`) | khách có ticket không, có khiếu nại chưa đóng không (L4, L5) |
| `ISurveyScheduler` | TV2 | TV1 (`SurveyScheduler`) | tạo phiếu khảo sát chờ khi trả phòng (L3) |
| `ICustomerCareStats` | TV2 | TV1 (`CustomerCareStats`) | thống kê CSKH theo khách cho dashboard (L6) |

Dùng chung: `IUnitOfWork`, `IClock` (TV1 khai báo và cài); `IReportExporter` (TV2 khai báo, TV2 cài `ExcelReportExporter`).

Chữ ký chốt ở M6 (27/11). Bên dùng viết file khai báo, bên kia cài đặt. Sau M6 muốn đổi phải được cả hai duyệt trong PR.

## Ngôn ngữ và định dạng
- Tên lớp/hàm/biến, comment, commit message: tiếng Anh.
- Chữ trên giao diện, thông báo lỗi, dữ liệu mẫu: tiếng Việt.
- Tiền VND định dạng `#,##0`, căn phải; ngày `dd/MM/yyyy`.
- Tên trang theo bảng mục 5.1 của `docs/PLAN.md` (chốt 27/11, tiếng Anh theo khung `frm…_dm_/_nv_/_bc_` của HuongdanXDHTTTQL): `frm000_login`, `frm201_dm_customer`, … File `Pages/frm201_dm_customer.cshtml`, lớp `frm201_dm_customerModel`, đường dẫn `/frm201_dm_customer`.
- Chữ trên giao diện và tài liệu dùng "Phiếu yêu cầu" (code giữ `SupportTicket`) và "Chương trình chăm sóc" (code giữ `Campaign`).
- Mọi handler của trang (`OnGet…`, `OnPost…`) bọc trong `ErrorHandler.Run`. Kiểm tra dữ liệu ở trang (báo lỗi cạnh ô: `is-invalid` + `invalid-feedback`) và ở Domain/Service. Ô nhập gắn vào lớp InputModel của GUI, không gắn thẳng vào Entity.
- Giao diện theo quy ước ở `docs/PLAN.md` mục 5.1: chỉ giao diện sáng, chỉ máy tính (từ 1280px), tông Navy & đồng thau, thêm/sửa bằng modal có nút "Thực hiện"/"Hủy bỏ", bảng dùng chung một thành phần.
- Trả lời người dùng bằng tiếng Việt. Tài liệu trong `docs/` viết tiếng Việt; mỗi mục đánh dấu [TV1]/[TV2]; mã F, UC, BM, Q, R, TC dùng thống nhất.

## Phân công (theo file, mục 6.2 của `docs/PLAN.md`)
- **TV1** (`Kessential`, chủ repo), Chăm sóc & hỗ trợ khách hàng + nền chung:
  - Domain: `Common/**`, `Staff/**` (trừ `Receptionist`), `Tickets/**`, `Surveys/**`.
  - Interface khai báo: `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`, `IUnitOfWork`, `IClock`.
  - BLL: `AuthService`, `EmployeeService`, `TicketService`, `SurveyService`, `CustomerCareReportService`, `Factories/**`, `TicketQuery`, `SurveyScheduler`, `CustomerCareStats`.
  - DAL: `Sqlite/**` (kết nối, UnitOfWork, tạo CSDL, nạp seed); repository và truy vấn báo cáo cho 7 bảng `departments`, `employees`, `tickets`, `ticket_logs`, `survey_criteria`, `surveys`, `survey_answers`.
  - Database: khung `schema.sql`/`seed.sql` + 7 bảng trên.
  - GUI: `Program.cs` (khung), `_Layout` (thanh bên thu gọn, menu theo vai trò, thanh trên, toast), đăng nhập cookie, `ErrorHandler`, trang lỗi chung, frm000, frm001, frm101–frm106.
  - Tests: 10 unit + 4 integration của TV1; `FakeCustomerDirectory`, `FakeStayLookup`.
  - Tài liệu và slide: phần chung (4W+1H, cây BFD, sơ đồ UC tổng, DFD ngữ cảnh + mức 0, ma trận quyền, kiến trúc/package) + phân hệ TV1.
- **TV2** (`munn8726`), Quan hệ khách hàng & khách hàng thân thiết:
  - Domain: `Customers/**`, `Hotel/**`, `Loyalty/**`, `Campaigns/**`, `Staff/Receptionist`.
  - Interface khai báo: `ITicketQuery`, `ISurveyScheduler`, `ICustomerCareStats`, `IReportExporter`, `ISegmentStrategy`.
  - BLL: `CustomerService`, `StayService`, `LoyaltyService`, `CampaignService`, `CustomerReportService`, `DashboardService`, `CustomerDirectory`, `StayLookup`, `Observers/**`, `Segments/**`, `ExcelReportExporter`.
  - DAL: repository và truy vấn báo cáo cho 10 bảng `customers`, `interactions`, `room_types`, `rooms`, `bookings`, `point_transactions`, `rewards`, `reward_redemptions`, `campaigns`, `campaign_targets`.
  - Database: 10 bảng trên trong `schema.sql`/`seed.sql`.
  - GUI: frm201–frm207; thành phần giao diện dùng chung (bảng có phân trang, sắp xếp, chọn cột, tiêu đề cố định; thanh lọc có "Xóa lọc"; khung modal; báo lỗi cạnh ô; nhãn trạng thái; ô số liệu; `site.css`); hàm định dạng tiền/ngày; hàm vẽ Chart.js dùng chung. Hạn 7/12.
  - Tests: 10 unit + 4 integration của TV2; `FakeTicketQuery`, `FakeSurveyScheduler`, `FakeCustomerCareStats`; hạ tầng integration test dùng chung (CSDL SQLite tạm từ `schema.sql` + `seed.sql`, `FixedClock`).
  - Tài liệu và slide: phân hệ TV2; biên dịch slide (máy có TeX).
- **Dùng chung:** `Program.cs` mỗi người thêm dòng đăng ký của mình. Sơ đồ chung (ERD, lớp) tách `-tv1.iuml`, `-tv2.iuml` rồi `!include` vào một file `.puml`. `CLAUDE.md`, `.claude/rules/**`, `docs/PLAN.md` sửa qua PR có cả hai duyệt. `docs/PROGRESS.md` mỗi người cập nhật dòng của mình trong PR của mình.

## Git
- Nhánh tích hợp `v2`. Nhánh việc `tv1/<việc>`, `tv2/<việc>` tạo từ `v2`, mở PR vào `v2`. Người kia đọc, chạy thử, comment rồi mới merge, trong 24 giờ. Mỗi người ≥ 2 PR được merge. Không force-push `v2`. Cuối cùng merge `v2` → `main`.
- Ai viết code thì người đó commit bằng tài khoản mình, trên máy mình. Không làm hộ, không chuyển code của người này sang cho người kia commit.
- Mỗi module một commit, message tiếng Anh. Commit message không kèm dòng `Co-Authored-By` hay dòng quảng cáo công cụ nào; việc dùng Claude được nêu trong README và báo cáo.
- Đo `git blame` (bỏ `*.Designer.cs` và tài liệu) vào các mốc 28/11, 4/12, 6/12, 9/12, 13/12: mỗi người phải viết 40–60% code logic. Giữ blame: đổi tên project bằng commit chỉ có `git mv`; không chạy định dạng hay refactor hàng loạt lên file của người kia; ai mở PR thì tự rebase và giải xung đột trong file của mình.

## Luật làm việc
- **Không tự commit, không tự push, không tự merge PR.** Đến điểm dừng thì đề xuất danh sách commit (message tiếng Anh); chỉ làm khi người dùng đồng ý.
- **Dừng sau mỗi module.** Một module là một dòng trong danh sách đánh số của file task. Khi dừng: tóm tắt thiết kế và những điểm cần nhớ khi vấn đáp, chờ duyệt rồi làm tiếp.
- **Không đổi khung đã chốt** (tên lớp, chữ ký public, interface ranh giới, cấu trúc thư mục) nếu chưa được duyệt. **Không sửa test có sẵn mà không hỏi**; không bao giờ sửa test của người kia. Thấy cần đổi thì dừng, ghi lý do vào `docs/PROGRESS.md` và hỏi.
- Test: unit test chạy không cần CSDL, dùng Fake; integration test chạy trên CSDL SQLite tạm có seed; `IClock` cố định thời gian; giá trị kỳ vọng lấy từ bảng tính tay, không chép công thức của code.
- Trang web: tự chạy và bấm thử trên trình duyệt được, nhưng vẫn nêu rõ trang nào người dùng cần mở và bấm thử.

## Điều kiện xong một module
- `dotnet build HotelCRM.sln` không lỗi, không warning.
- `dotnet test HotelCRM.sln` xanh 100% (module có code).
- `docs/PROGRESS.md` đã cập nhật (trong PR của người làm).
- PR đã được người kia review và merge; trang đã bấm thử trên trình duyệt (nếu có).
