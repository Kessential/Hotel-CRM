# Hotel CRM (v2)

Bài tập lớn môn HTTTQL (MI4090), nhóm 2 người: CRM (quản lý dịch vụ chăm sóc khách hàng) áp dụng cho khách sạn. C# / .NET 10, WinForms, SQLite, test xUnit. Chấm theo sheet "Nhóm 02 người" của `docs/slide_checklist/Checklist_BTL_HTTTQL_v2026.xlsx`; quy ước đặt tên theo `docs/slide_checklist/HuongdanXDHTTTQLv1_1.pdf`. Hai file này là tài liệu của giảng viên, **không đưa lên repo** (repo public): mỗi người tự giữ một bản ở `docs/slide_checklist/` trên máy mình; thư mục này nằm trong `.gitignore`.

## Đọc trước khi làm
1. `docs/PROGRESS.md` (v2): module nào đã xong, việc đang dở, quyết định lệch khỏi plan, việc chờ người.
2. File task của giai đoạn đang làm trong `docs/tasks/` (P1: `docs/tasks/v2-p1-analysis.md`).
3. `docs/PLAN.md` (v2): chỉ mở những mục mà file task nêu tên. Không đọc cả file.

Tài liệu v1 (`docs/PLAN.md`, `docs/PROGRESS.md`, `docs/tasks/stage-*.md` bản cũ) đã xóa khỏi cây thư mục, chỉ còn trong lịch sử git ở commit `fd7b7e4` (ví dụ `git show fd7b7e4:docs/PLAN.md`). Chỉ để tra cứu, không làm theo.

Giai đoạn chuyển tiếp:
- `.claude/rules/*.md` còn viết cho v1 tới khi TV1 sửa ở P2. Các tham chiếu `docs/PLAN.md` trong rules tạm hiểu là PLAN v1 (`git show fd7b7e4:docs/PLAN.md`). Chỗ nào rules mâu thuẫn với file này thì theo file này. Rules chỉ tự nạp khi đã có file khớp đường dẫn; khi tạo file đầu tiên trong một thư mục thì tự đọc file rules tương ứng trước.
- Code hiện tại vẫn là bản v1 (lưu JSON, giao diện tiếng Anh, project `WinForms`/`Business`/`Data`). Chuyển sang SQLite, đổi tên project và giao diện tiếng Việt làm ở P2–P3 theo `docs/PLAN.md`.

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
- Tên project giữ như hiện tại tới P2. Ở P2 đổi `HotelCRM.WinForms` → `HotelCRM.GUI`, `HotelCRM.Business` → `HotelCRM.BLL`, `HotelCRM.Data` → `HotelCRM.DAL`; lệnh `run`/`publish` đổi theo.
- `HashPassword` sinh `PasswordHash` và `Salt` (PBKDF2): hiện cho `employees.json`, từ P2 cho `Database/seed.sql`. Tài khoản mẫu ghi trong `README.md`.
- `tools/plantuml.jar` chưa có trong repo; tải khi người dùng đồng ý (xem "Việc chờ người" trong `docs/PROGRESS.md`).

## Kiến trúc (đích của v2)
```
Database/            schema.sql, seed.sql
Resource/            icon, logo
src/HotelCRM.GUI     WinForms; Program.cs = Composition Root
src/HotelCRM.BLL     Services, Queries, Observers, Segments, Factories, Reporting
src/HotelCRM.Domain  Common, Staff | Tickets, Surveys (TV1) | Customers, Hotel, Loyalty, Campaigns (TV2) | Interfaces
src/HotelCRM.DAL     Sqlite (kết nối, UnitOfWork, tạo CSDL), Repositories, ReadModels
tests/HotelCRM.Tests Unit/, Integration/, Fakes/
```
- Chiều phụ thuộc: `GUI → BLL → Domain ← DAL`. `Domain` không tham chiếu project nào. `GUI` tham chiếu `DAL` chỉ để `Program.cs` khởi tạo. `Tests` tham chiếu `Domain`, `BLL`, `DAL`. Form không chứa SQL hay quy tắc nghiệp vụ.
- Khởi tạo đối tượng bằng tay trong `Program.cs`, không dùng thư viện DI. Thêm loại ticket, phân khúc hay observer mới chỉ cần thêm lớp và một dòng đăng ký ở `Program.cs`; không `switch` cứng theo tên.
- Cấu hình: một `appsettings.json` (+ `appsettings.example.json`), đọc một lần ở `Program.cs` rồi truyền qua constructor. Không Singleton cấu hình. Lấy thời gian qua `IClock`.
- Quy tắc nghiệp vụ: **service tra cứu, lớp Domain quyết định và ném ngoại lệ.** Cây ngoại lệ: `DomainException` → `ValidationException` (sai dữ liệu một đối tượng), `BusinessRuleException` (sai quy tắc giữa các đối tượng hoặc sai trạng thái), `AuthorizationException` (sai quyền). DAL bọc lỗi SQLite thành `DataAccessException`. Không `catch {}` rỗng.
- Phân quyền ở tầng Service: service nhận `actor` và gọi `actor.EnsureCan…()` trước khi làm. Quyền là hàm đa hình của 4 lớp con `Employee` (`Administrator`, `Manager`, `Receptionist`, `Agent`), không `is`/`as`. Ẩn nút trên Form chỉ là lớp chặn thứ hai.
- CSDL: SQLite qua `Microsoft.Data.Sqlite` (ADO.NET), mọi câu SQL có tham số. Bật `PRAGMA foreign_keys=ON` mỗi kết nối. `IUnitOfWork.Execute(...)` dùng chung một connection và một transaction; lời gọi lồng nhau tham gia transaction ngoài. Luồng L2 (đóng ticket) và L3 (trả phòng) chạy trọn trong một transaction. Repository đọc CSDL khi cần, không nạp hết vào bộ nhớ. Giữa các aggregate tham chiếu bằng Id. Lưới và báo cáo dùng truy vấn JOIN, không N+1.
- Gói NuGet: ngoài bộ xUnit mặc định chỉ được dùng `Microsoft.Data.Sqlite`, `ClosedXML` (xuất .xlsx), `ScottPlot.WinForms` (biểu đồ). Cần gói khác thì dừng và hỏi; phải được cả hai đồng ý.

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
- Tên Form theo bảng mục 5.1 của `docs/PLAN.md` (`frm000_dang_nhap`, `frm201_dm_khach_hang`, …). Tên đang là tên tạm; rà theo HuongdanXDHTTTQL và chốt trước P3.
- Mọi handler của Form bọc trong `ErrorHandler.Run`. Kiểm tra dữ liệu ở Form (`ErrorProvider`) và ở Domain/Service.
- Trả lời người dùng bằng tiếng Việt. Tài liệu trong `docs/` viết tiếng Việt; mỗi mục đánh dấu [TV1]/[TV2]; mã F, UC, BM, Q, R, TC dùng thống nhất.

## Phân công (theo file, mục 6.2 của `docs/PLAN.md`)
- **TV1** (`Kessential`, chủ repo), Chăm sóc & hỗ trợ khách hàng + nền chung:
  - Domain: `Common/**`, `Staff/**` (trừ `Receptionist`), `Tickets/**`, `Surveys/**`.
  - Interface khai báo: `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`, `IUnitOfWork`, `IClock`.
  - BLL: `AuthService`, `EmployeeService`, `TicketService`, `SurveyService`, `CustomerCareReportService`, `Factories/**`, `TicketQuery`, `SurveyScheduler`, `CustomerCareStats`.
  - DAL: `Sqlite/**` (kết nối, UnitOfWork, tạo CSDL, nạp seed); repository và truy vấn báo cáo cho 7 bảng `departments`, `employees`, `tickets`, `ticket_logs`, `survey_criteria`, `surveys`, `survey_answers`.
  - Database: khung `schema.sql`/`seed.sql` + 7 bảng trên.
  - GUI: `Program.cs` (khung), `ErrorHandler`, frm000, frm001, frm101–frm106.
  - Tests: 10 unit + 4 integration của TV1; `FakeCustomerDirectory`, `FakeStayLookup`.
  - Tài liệu và slide: phần chung (4W+1H, cây BFD, sơ đồ UC tổng, DFD ngữ cảnh + mức 0, ma trận quyền, kiến trúc/package) + phân hệ TV1.
- **TV2** (`munn8726`), Quan hệ khách hàng & khách hàng thân thiết:
  - Domain: `Customers/**`, `Hotel/**`, `Loyalty/**`, `Campaigns/**`, `Staff/Receptionist`.
  - Interface khai báo: `ITicketQuery`, `ISurveyScheduler`, `ICustomerCareStats`, `IReportExporter`, `ISegmentStrategy`.
  - BLL: `CustomerService`, `StayService`, `LoyaltyService`, `CampaignService`, `CustomerReportService`, `DashboardService`, `CustomerDirectory`, `StayLookup`, `Observers/**`, `Segments/**`, `ExcelReportExporter`.
  - DAL: repository và truy vấn báo cáo cho 10 bảng `customers`, `interactions`, `room_types`, `rooms`, `bookings`, `point_transactions`, `rewards`, `reward_redemptions`, `campaigns`, `campaign_targets`.
  - Database: 10 bảng trên trong `schema.sql`/`seed.sql`.
  - GUI: frm201–frm207; hàm định dạng tiền/ngày, căn phải, `ErrorProvider` dùng chung; hàm vẽ biểu đồ dùng chung.
  - Tests: 10 unit + 4 integration của TV2; `FakeTicketQuery`, `FakeSurveyScheduler`, `FakeCustomerCareStats`.
  - Tài liệu và slide: phân hệ TV2; biên dịch slide (máy có TeX).
- **Dùng chung:** `Program.cs` mỗi người thêm dòng đăng ký của mình. Sơ đồ chung (ERD, lớp) tách `-tv1.iuml`, `-tv2.iuml` rồi `!include` vào một file `.puml`. `CLAUDE.md`, `.claude/rules/**`, `docs/PLAN.md` sửa qua PR có cả hai duyệt. `docs/PROGRESS.md` mỗi người cập nhật dòng của mình trong PR của mình.

## Git
- Nhánh tích hợp `v2`. Nhánh việc `tv1/<việc>`, `tv2/<việc>` tạo từ `v2`, mở PR vào `v2`. Người kia đọc, chạy thử, comment rồi mới merge, trong 24 giờ. Mỗi người ≥ 2 PR được merge. Không force-push `v2`. Cuối cùng merge `v2` → `main`.
- Ai viết code thì người đó commit bằng tài khoản mình, trên máy mình. Không làm hộ, không chuyển code của người này sang cho người kia commit.
- Mỗi module một commit, message tiếng Anh. Commit message không kèm dòng `Co-Authored-By` hay dòng quảng cáo công cụ nào; việc dùng Claude được nêu trong README và báo cáo.
- Mỗi Chủ nhật (29/11, 6/12, 13/12) đo `git blame` (bỏ `*.Designer.cs` và tài liệu): mỗi người phải viết 40–60% code logic.

## Luật làm việc
- **Không tự commit, không tự push, không tự merge PR.** Đến điểm dừng thì đề xuất danh sách commit (message tiếng Anh); chỉ làm khi người dùng đồng ý.
- **Dừng sau mỗi module.** Một module là một dòng trong danh sách đánh số của file task. Khi dừng: tóm tắt thiết kế và những điểm cần nhớ khi vấn đáp, chờ duyệt rồi làm tiếp.
- **Không đổi khung đã chốt** (tên lớp, chữ ký public, interface ranh giới, cấu trúc thư mục) nếu chưa được duyệt. **Không sửa test có sẵn mà không hỏi**; không bao giờ sửa test của người kia. Thấy cần đổi thì dừng, ghi lý do vào `docs/PROGRESS.md` và hỏi.
- Test: unit test chạy không cần CSDL, dùng Fake; integration test chạy trên CSDL SQLite tạm có seed; `IClock` cố định thời gian; giá trị kỳ vọng lấy từ bảng tính tay, không chép công thức của code.
- Giao diện WinForms không tự kiểm tra được: nêu rõ Form nào người dùng cần mở và bấm thử.

## Điều kiện xong một module
- `dotnet build HotelCRM.sln` không lỗi, không warning.
- `dotnet test HotelCRM.sln` xanh 100% (module có code).
- `docs/PROGRESS.md` đã cập nhật (trong PR của người làm).
- PR đã được người kia review và merge; Form đã bấm thử (nếu có).
