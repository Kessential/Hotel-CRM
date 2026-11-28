# Hotel CRM v2 — Kế hoạch (PLAN v2)

> Kế hoạch làm lại theo checklist HTTTQL v2026 (nhóm 2 người), chốt 22/11/2026; đưa vào repo 25/11/2026 (M1). File này giữ nội dung bản kế hoạch gốc, trừ mục "Bước làm ngay" (đã làm ở M1). Kế hoạch v1 chỉ còn trong lịch sử git: `git show fd7b7e4:docs/PLAN.md`.
>
> Đề tài đã xác nhận: **CRM (quản lý dịch vụ chăm sóc khách hàng) áp dụng cho khách sạn**. So với bản 21/11, phần TV2 chuyển từ "lưu trú & thanh toán" sang "quan hệ khách hàng & khách hàng thân thiết".
>
> Cập nhật 27/11/2026: giao diện chuyển từ WinForms sang web theo yêu cầu của thầy (26/11). Quyết định hai người chốt ngày 27/11 ghi ở mục 3.1; các mục khác đã sửa theo.
>
> Trạng thái, việc đang dở, việc chờ người: `docs/PROGRESS.md`. Việc chi tiết của giai đoạn đang làm: `docs/tasks/`.

## 0. Đọc nhanh (cả hai người đọc mục này trước)
- **Đề:** CRM cho khách sạn. Chấm theo sheet "Nhóm 02 người" của `docs/slide_checklist/Checklist_BTL_HTTTQL_v2026.xlsx`; quy ước tên theo `docs/slide_checklist/HuongdanXDHTTTQLv1_1.pdf`. Hai file này là tài liệu của giảng viên, **không đưa lên repo** (repo public): mỗi người tự giữ một bản ở `docs/slide_checklist/` trên máy mình; thư mục này nằm trong `.gitignore`.
- **Hai phân hệ, chia dọc:**
  - **TV1 (chủ repo) — Chăm sóc & hỗ trợ khách hàng:** ticket yêu cầu/khiếu nại, phân công, xử lý theo trạng thái, nhật ký xử lý, khảo sát hài lòng, báo cáo CSKH.
  - **TV2 — Quan hệ khách hàng & khách hàng thân thiết:** hồ sơ khách 360, lưu trú (nguồn dữ liệu cho CRM), sổ điểm và đổi quà, chiến dịch chăm sóc theo phân khúc, báo cáo khách hàng và dashboard 360.
- **Mỗi người tự làm trọn phần mình**: phân tích → Domain → mẫu thiết kế → test → giao diện → slide. Ai viết code người đó commit bằng tài khoản mình; merge qua PR có người kia review. Không làm hộ (Khóa 3 của checklist: hệ số cá nhân có thể về 0).
- **Hai phân hệ chỉ nói chuyện qua interface** (mục 4.5). Không gọi service hay bảng của bên kia.
- **Giao diện là web** (ASP.NET Core Razor Pages + Bootstrap 5 + Chart.js), chạy tại máy, mở bằng trình duyệt ở `http://localhost:5080`. Domain, BLL, DAL, CSDL, interface ranh giới và test không đổi (mục 3.1).
- **Mốc:** slide 30/11 · code xong 12/12 · vấn đáp từ 13/12 · nộp cuối 30/12.
- **Việc của từng người:** mục 6.4 (TV1), 6.5 (TV2), 6.6 (cả hai), dạng danh sách có hạn; trạng thái đánh dấu ở `docs/PROGRESS.md`.
- **Khi vấn đáp:** mỗi người giải thích được cả phần của bạn và tự thêm một lớp mới khi tắt AI (Live OCP). Vì vậy thiết kế giữ tối giản: chỉ thêm thứ phục vụ một tiêu chí của checklist.

## 1. Context
Checklist mới (định mức N2) đòi những thứ project hiện tại chưa có: CSDL thật (JSON có nguy cơ FAIL_RUN, trần 3.5), ≥11 bảng 3NF có khóa ngoại, ≥3 vai trò chặn quyền ở tầng Service, ≥4 Form Master-Detail, ≥5 báo cáo xuất Excel có biểu đồ, seed ≥120 dòng, ≥8 integration test (rollback, RBAC, báo cáo khớp tính tay), quy ước tên theo HuongdanXDHTTTQL, giao diện tiếng Việt, Git rải ≥3 tuần có PR và mỗi người viết 40–60% code logic (hiện 84/16). Ngoài ra bản 21/11 đã để phần TV2 nghiêng về quản lý khách sạn (hóa đơn, công nợ, doanh thu), dễ bị coi là lệch đề CRM. Bản này làm lại từ phân tích thiết kế, dùng lại Domain/Business cũ, đưa cả hai phân hệ về đúng CRM.

Đã đọc khi lập kế hoạch (22/11): checklist (3 sheet), PROGRESS, PLAN và `docs/tasks/stage-3b-sqlite.md` bản v1 (lịch sử git, `fd7b7e4`), bản rà soát của TV2 (`RaSoat_Checklist_HotelCRM_2026-11-21.md`), slide L1 của TV2 (`docs/slides/*.tex`, đã merge vào `develop` qua PR #1). Bổ sung 25/11: `docs/slide_checklist/HuongdanXDHTTTQLv1_1.pdf`.

## 2. Checklist chấm thế nào (tóm tắt)
- 20 tiêu chí × 0.5 = 10 điểm. Trục I sản phẩm 4 điểm; Trục II hiểu sâu (phân tích, thiết kế, kiểm thử ↔ code) 6 điểm.
- Cổng sản phẩm: thiếu >20% khối lượng → trần 6.0; không chạy được trên máy chấm hoặc không có CSDL thật → trần 3.5.
- Cổng hiểu sâu: Trục II < 3.0 → trần 6.5; < 1.8 → trần 3.5.
- Điểm cá nhân = điểm nhóm × hệ số: 1.00 khi mỗi người viết 40–60% code logic (`git blame`, bỏ Designer và tài liệu) và qua vấn đáp chéo; 30–40% → 0.85; 20–30% → 0.70; < 20% → 0.50; làm hộ, rửa commit, không giải thích được code đứng tên mình → 0.00.
- 5 khóa chống đối phó: lớp rỗng (Anemic); nhân bản Form CRUD một bảng; làm hộ hoặc chia "1 người code, 1 người làm slide"; test hình thức (thầy tiêm lỗi mà test vẫn xanh); code chết, pattern giả, không giải thích được code.
- Buổi chấm 10–15 phút: chạy app theo README (≤3 phút) → đăng nhập các vai trò, làm 1 giao dịch Master-Detail, mở báo cáo, xuất Excel → chạy test, thầy tiêm lỗi, đặt breakpoint xem Call Stack → hỏi đánh đổi, mỗi người Live OCP, hỏi chéo phần của bạn.

## 3. Quyết định đã chốt
| Mục | Chốt |
|---|---|
| Đề tài | CRM áp dụng cho khách sạn (xác nhận 22/11) |
| Lịch | Slide nộp **30/11**; vấn đáp **từ 13/12**; hạn cuối **30/12** |
| Repo | Giữ `Kessential/Hotel-CRM`, nhánh tích hợp mới `v2` (giữ lịch sử từ 15/11) |
| Chia phân hệ | TV1 = Chăm sóc & hỗ trợ khách hàng; TV2 = Quan hệ khách hàng & khách hàng thân thiết |
| Cách làm của TV2 | TV2 tự code trên máy mình, tự commit/push/mở PR. Phiên Claude của TV1: đặc tả chung, file task cho cả hai, code phần TV1, review PR của TV2. |
| CSDL | SQLite qua `Microsoft.Data.Sqlite` (ADO.NET, SQL có tham số) |
| Giao diện | Web: ASP.NET Core Razor Pages, chạy tại máy (`http://localhost:5080`), chỉ giao diện sáng, chỉ máy tính (chốt 27/11, mục 3.1) |
| Gói NuGet | `Microsoft.Data.Sqlite`, `ClosedXML` (xuất .xlsx); không làm PDF. Thư viện giao diện là file tĩnh trong `wwwroot/lib`: Bootstrap 5, Bootstrap Icons, Chart.js, font Cormorant Garamond + Be Vietnam Pro. Bỏ `ScottPlot.WinForms` (27/11) |
| Ngôn ngữ | Giao diện, thông báo lỗi, dữ liệu mẫu tiếng Việt; tiền VND `#,##0` căn phải; ngày `dd/MM/yyyy`. Tên lớp/hàm/comment tiếng Anh. Tên trang/thư mục theo HuongdanXDHTTTQL, tên trang bằng tiếng Anh (mục 5.1). Chữ trên giao diện: "Phiếu yêu cầu" (code `SupportTicket`), "Chương trình chăm sóc" (code `Campaign`) |
| Vai trò | 4 vai trò: Admin (làm được mọi thứ), Quản lý, Lễ tân, CSKH |
| Slide 30/11 | Sửa bộ Beamer của TV2 theo thiết kế mới + tự đánh giá L2; mỗi người làm slide phân hệ mình |
| Tài liệu | Markdown tiếng Việt + PlantUML trong `docs/analysis/`; DFD có thể vẽ draw.io; báo cáo cuối (Word hay LaTeX) chốt khi có mẫu |
| Đã bỏ so với bản 21/11 | đặt nhiều phòng, dịch vụ phát sinh, hóa đơn, công nợ, báo cáo doanh thu/công suất phòng, Strategy giảm giá; 3 strategy phân công cũ |

### 3.1 Chuyển giao diện sang web (chốt 27/11/2026)
Ngày 26/11 thầy yêu cầu đổi giao diện từ WinForms sang web. TV1 soạn bản kế hoạch chuyển đổi (file ngoài repo), TV2 duyệt toàn bộ ngày 27/11; hai người chọn chung các mục còn mở.

**Thầy trả lời (26/11):** chạy trên trình duyệt tại máy (localhost) là đủ; không bắt buộc công nghệ; tên trang theo HuongdanXDHTTTQL; các mục checklist ghi theo WinForms (Form, ErrorProvider, DataGridView, App.config) chấm theo phần tương đương trên web; hạn không đổi.

**Công nghệ**
| Quyết định | Vì sao |
|---|---|
| ASP.NET Core **Razor Pages** (không Blazor, MVC, React/Vue) | Mỗi trang = `.cshtml` + PageModel, giống Form + code-behind; đăng nhập cookie và tải file có sẵn; Call Stack gọn khi vấn đáp (`OnPostCheckOut → StayService.CheckOut → …`) |
| Giữ **SQLite** | Một file nằm cạnh app; máy chấm không cần server CSDL (tiêu chí 1.4) |
| Bootstrap 5 + Bootstrap Icons + **Chart.js** (file tĩnh), bỏ `ScottPlot.WinForms` | Không cần npm; biểu đồ tương tác, PageModel trả dữ liệu, trình duyệt vẽ |
| Font Cormorant Garamond (tiêu đề trang, tên khách, đăng nhập) + Be Vietnam Pro (chữ thường) | Phong cách khách sạn; file font để trong `wwwroot/lib` để máy không có mạng vẫn hiện đúng |

**Kỹ thuật khi lên web**
1. `HotelCRM.GUI` là project Razor Pages mới (TV2 tạo 28/11, PR #10), không đổi tên từ `WinForms`. Đổi tên `Business` → `BLL`, `Data` → `DAL` bằng một commit chỉ có `git mv` + sửa `.sln`/`.csproj`; mỗi người tự sửa `namespace`/`using` trong file của mình (giữ `git blame`). TV1, 1/12.
2. Xoá code v1 không còn dùng: project WinForms, phần lưu JSON, 3 strategy phân công cũ, `UserSession`, `UiNotificationObserver`. File v1 do ai viết thì người đó tự xoá: TV1 xoá phần của mình (1/12); TV2 xoá test của 3 strategy phân công cũ và `SurveyService` v1 (PR #7), phần lưu JSON của TV2 (PR #5), và viết lại phần của mình trong `CustomerDirectory`, `StayLookup` v1 ở P3.
3. `Program.cs` vẫn là Composition Root và tự `new` từng đối tượng, nhưng dựng theo từng request: mỗi request một `UnitOfWork` (vì `UnitOfWork` giữ transaction đang mở). Container có sẵn của ASP.NET Core chỉ giao các đối tượng đã tạo cho trang. Live OCP vẫn là thêm 1 lớp + 1 dòng ở đây.
4. Đăng nhập bằng cookie thay `UserSession` tĩnh: đọc mã nhân viên từ cookie, nạp `Employee`, truyền làm `actor`.
5. Bỏ Singleton `AppConfig`; đọc `appsettings.json` một lần; thêm khoá `Urls` (cổng chạy).
6. `ErrorHandler`: báo lỗi tiếng Việt bằng toast và một trang lỗi chung; vẫn ghi `logs/error.log`, không lộ stack trace.
7. SQLite bật `journal_mode=WAL` và `busy_timeout`.
8. Chạy HTTP tại `http://localhost:5080`, chỉ nghe localhost; ContentRoot đặt là thư mục chứa file chạy (`AppContext.BaseDirectory`), để chạy bản publish từ thư mục khác vẫn tìm thấy `wwwroot`, `Database/`, `Resource/`; publish self-contained (`win-x64`, thêm `linux-x64` khi cần). Thử trên máy sạch ngày **4/12** và lại ở P4.
9. Ô nhập gắn vào lớp InputModel ở GUI, không gắn thẳng vào Entity (giữ bất biến, tiêu chí 4.4).
10. Phân trang và sắp xếp ở server (`?page=&sort=`, `LIMIT/OFFSET`); cột sắp xếp lấy từ danh sách cho phép, không ghép chuỗi SQL.
11. Bảo mật: giữ antiforgery mặc định; không dùng `Html.Raw` với dữ liệu nhập; `[Authorize]` trên trang chỉ là lớp chặn thứ hai, service vẫn gọi `actor.EnsureCan…()`.
12. `.editorconfig` thêm khối tắt quy tắc tên lớp cho `Pages/` (lớp `frm…Model`); thêm `.gitattributes` (`* text=auto eol=lf`).

**Quy ước theo HuongdanXDHTTTQL:** giữ cây BFD 8 nhánh theo phân hệ (ghi đối chiếu vào `02-bfd.md` mục 3); tên trang theo khung `frm` + 3 số + `_dm_`/`_nv_`/`_bc_` + tên tiếng Anh, số đầu cho biết của ai (0 chung, 1 TV1, 2 TV2); tên biến/control giữ chuẩn .NET, không dùng `m_dc_…`, `m_txt_…` (thêm bảng "Quy ước đặt tên" đối chiếu với slide trong `07-thiet-ke-lop.md`); giữ `Database/`, `Resource/`; **bỏ** tên solution kèm phiên bản, thư mục `Dlllib` và tên bảng viết hoa.

**Phân công và `git blame`**
- Giữ chia theo phân hệ. TV2 nhận thêm **phần dùng chung** (khoảng 350–450 dòng, hạn 7/12): thành phần bảng, thanh lọc có "Xóa lọc", khung modal, báo lỗi cạnh ô, nhãn trạng thái, ô số liệu, định dạng tiền/ngày, `site.css`, hàm vẽ Chart.js, `ExcelReportExporter`, hạ tầng integration test (CSDL SQLite tạm từ `schema.sql` + `seed.sql`, `FixedClock`; làm sau khi TV1 có `DatabaseInitializer`).
- Khung web (project `HotelCRM.GUI`, khung `Program.cs`, `_Layout` gồm thanh bên và thanh trên, menu `Navigation/AppMenu.cs`, toast, `site.js`) chuyển từ TV1 sang TV2 (đổi 28/11, PR #10), vì TV2 cần trang chạy thật cho slide 30/11.
- TV1 giữ đăng nhập cookie, lọc menu theo vai trò (đi liền với `actor`), `ErrorHandler` (đẩy thông báo lỗi ra toast của TV2, trang lỗi chung), nền SQLite, nội dung trang chủ; đưa đăng nhập, `IUnitOfWork` và một repository mẫu lên **trước 2/12**.
- frm001 chỉ ghép các khối; mỗi người viết khối số liệu phân hệ mình.
- Luật giữ blame: không chạy định dạng/refactor hàng loạt lên file của người kia; ai mở PR thì tự rebase và giải xung đột trong file của mình; không sửa vặt để lấy dòng. TV2 mở PR nhỏ mỗi 1–2 ngày.
- Phạm vi đo: nhóm tự đo trên các file `.cs`, `.cshtml`, `.js`, `.css`; bỏ `wwwroot/lib` (thư viện chép vào), `*.Designer.cs` và tài liệu. Phần dùng chung giao cho TV2 phần lớn là `.cshtml`/`.css`/`.js`, chỉ đo `.cs` thì việc giao thêm không cân được tỷ lệ.
- Mốc đo blame: 28/11, 4/12, 6/12, 9/12, 13/12. Ngày 4/12 lệch về TV1 (khoảng 60/40) là bình thường. Nếu 6/12 TV2 dưới 43%: TV1 dừng thêm tiện ích phụ; TV2 bổ sung việc có ích trong phân hệ mình (biểu đồ R4, lọc nhiều điều kiện ở frm203/frm206, test biên Q7–Q11).

**Hai người cùng chọn (27/11)**
| Mục | Chọn |
|---|---|
| Tông màu | Navy & đồng thau: thanh bên `#16213a`, màu chính `#1f3a68`, màu nhấn `#b08d57`, nền `#f2f4f8` |
| Kiểu chữ | Cormorant Garamond + Be Vietnam Pro |
| Chuông báo phiếu sắp trễ SLA | Không làm (trang chủ đã có "Việc cần làm") |
| Lịch P1 | Giữ hạn: TV1 làm phần mình của M2–M8 trong 27–29/11; slide 30/11 đủ cả hai phân hệ |
| Còn mở | tải `plantuml.jar`, cài Java, ảnh biểu mẫu thật (`docs/PROGRESS.md`, "Việc chờ người") |

## 4. Hệ thống tổng thể

### 4.1 Vai trò và ma trận quyền
| Vai trò | Trong khách sạn | Việc chính |
|---|---|---|
| Admin | quản trị hệ thống | toàn quyền: tài khoản, danh mục, mọi nghiệp vụ, báo cáo |
| Quản lý | trưởng bộ phận | phân công, đóng/mở lại ticket, xóa khách, tạo chiến dịch, xem báo cáo |
| Lễ tân | nhân viên quầy | hồ sơ khách, đặt/nhận/trả phòng, đổi quà, tạo ticket, ghi khảo sát |
| CSKH | nhân viên chăm sóc, kỹ thuật | xử lý ticket được giao, ghi khảo sát, gọi chăm sóc theo chiến dịch |

| Việc | Admin | Quản lý | Lễ tân | CSKH |
|---|---|---|---|---|
| Tài khoản, phòng ban, nhân viên | ✓ | xem | – | – |
| Danh mục phòng, quà tặng | ✓ | xem | xem | xem |
| Thêm/sửa hồ sơ khách | ✓ | ✓ | ✓ | xem |
| Xóa khách | ✓ | ✓ | – | – |
| Đặt phòng, nhận phòng, trả phòng | ✓ | ✓ | ✓ | – |
| Đổi quà | ✓ | ✓ | ✓ | – |
| Tạo chiến dịch | ✓ | ✓ | – | – |
| Ghi phản hồi chiến dịch | ✓ | ✓ | ✓ | ✓ |
| Tạo ticket | ✓ | ✓ | ✓ | ✓ |
| Phân công, đóng, mở lại ticket | ✓ | ✓ | – | – |
| Bắt đầu, giải quyết ticket | ✓ | – | – | ✓ (ticket của mình) |
| Ghi khảo sát | ✓ | ✓ | ✓ | ✓ |
| Báo cáo, dashboard, xuất Excel | ✓ | ✓ | – | – |

Cài đặt: `Employee` (abstract) có 4 lớp con `Administrator`, `Manager`, `Receptionist`, `Agent`, mỗi lớp override hàm quyền (đa hình, không `is/as`). Service nhận `actor` và gọi `actor.EnsureCan…()` trước khi làm; ẩn nút trên Form chỉ là lớp chặn thứ hai. Quy tắc nghiệp vụ (tải tối đa, trạng thái) vẫn áp cho Admin; mọi thao tác ghi người làm. Khi vấn đáp: cho Admin vượt "chỉ người được giao mới xử lý" là quyết định có chủ đích (giám sát can thiệp, có nhật ký).

### 4.2 BFD (gốc → 8 nhánh cấp 2 → nút lá cấp 3; mỗi nút lá ↔ 1 trang)
| Nhánh cấp 2 | TV | Nút lá cấp 3 | Trang |
|---|---|---|---|
| F1 Hệ thống & nhân sự | 1 | F1.1 Đăng nhập, đổi mật khẩu | frm000 |
| | | F1.2 Phòng ban, nhân viên, tài khoản | frm101 |
| F2 Tiếp nhận yêu cầu | 1 | F2.1 Lập phiếu yêu cầu (khiếu nại, yêu cầu dịch vụ, sửa chữa) | frm102 |
| F3 Điều phối & xử lý | 1 | F3.1 Tra cứu & phân công phiếu yêu cầu | frm103 |
| | | F3.2 Xử lý phiếu yêu cầu & nhật ký | frm104 |
| F4 Đánh giá hài lòng | 1 | F4.1 Ghi phiếu khảo sát | frm105 |
| | | F4.2 Báo cáo CSKH | frm106 |
| F5 Hồ sơ khách hàng 360 | 2 | F5.1 Quản lý hồ sơ khách; F5.2 Xem lịch sử lưu trú, tương tác, điểm | frm201 |
| F6 Lưu trú | 2 | F6.1 Danh mục loại phòng & phòng | frm202 |
| | | F6.2 Đặt phòng, nhận phòng, trả phòng | frm203 |
| F7 Khách hàng thân thiết | 2 | F7.1 Danh mục quà tặng | frm204 |
| | | F7.2 Sổ điểm & đổi quà | frm205 |
| F8 Chương trình chăm sóc & báo cáo khách hàng | 2 | F8.1 Chương trình chăm sóc | frm206 |
| | | F8.2 Báo cáo khách hàng & dashboard 360 | frm207 |

### 4.3 Use Case (11)
| UC | Tên | TV | Tác nhân | Trang | Phức hợp | Liên PH |
|---|---|---|---|---|---|---|
| UC01 | Đăng nhập, đổi mật khẩu, quản lý tài khoản | 1 | mọi vai trò; tài khoản: Admin | frm000, frm101 | | |
| UC02 | Lập phiếu yêu cầu | 1 | mọi vai trò | frm102 | | ✓ L1 |
| UC03 | Tra cứu & phân công phiếu yêu cầu | 1 | xem: mọi vai trò; phân công: Quản lý, Admin | frm103 | | |
| UC04 | Xử lý phiếu yêu cầu (bắt đầu, giải quyết, đóng, mở lại) | 1 | CSKH; Quản lý; Admin | frm104 | ✓ | ✓ L2 |
| UC05 | Ghi khảo sát hài lòng | 1 | mọi vai trò | frm105 | ✓ | ✓ (phiếu chờ do L3 tạo) |
| UC06 | Báo cáo CSKH | 1 | Quản lý, Admin | frm106 | | ✓ (R2 lấy hạng khách) |
| UC07 | Quản lý hồ sơ khách 360 | 2 | Lễ tân, Quản lý, Admin; CSKH xem | frm201 | | ✓ L5 |
| UC08 | Đặt phòng & lưu trú (kèm danh mục phòng) | 2 | Lễ tân, Quản lý, Admin | frm202, frm203 | ✓ | ✓ L3 |
| UC09 | Đổi quà (kèm danh mục quà) | 2 | Lễ tân, Quản lý, Admin | frm204, frm205 | ✓ | |
| UC10 | Chương trình chăm sóc | 2 | tạo: Quản lý, Admin; phản hồi: mọi vai trò | frm206 | ✓ | ✓ L4 |
| UC11 | Báo cáo khách hàng & dashboard 360 | 2 | Quản lý, Admin | frm207 | | ✓ L6 |

Giao dịch phức hợp: UC04 (ticket + nhật ký + điểm + tương tác), UC05 (phiếu + điểm từng tiêu chí), UC08 (booking + điểm + hạng + tương tác + phiếu khảo sát), UC09 (lượt đổi + sổ điểm + số quà), UC10 (chiến dịch + danh sách khách).

### 4.4 Các luồng liên phân hệ
| Luồng | Chiều | Diễn biến | Ai viết | Test |
|---|---|---|---|---|
| L1 Tạo ticket | TV1 hỏi TV2 | `TicketService` lấy hệ số hạng khách qua `ICustomerDirectory` để nâng mức ưu tiên và tính hạn SLA; ticket sửa chữa hỏi `IStayLookup` xem khách có đang ở đúng phòng đó không | TV1 gọi, TV2 cài 2 interface | unit TV1 (Fake) |
| L2 Đóng ticket | TV1 → TV2, **1 transaction** | ticket → Đã đóng, ghi `ticket_logs` → phát sự kiện `ITicketObserver` → `GoodwillPointsObserver` (TV2) cộng điểm thiện chí vào sổ điểm, `InteractionLogger` (TV2) ghi tương tác → commit. Lỗi ở bước TV2 → rollback cả việc đóng ticket | TV1: đóng + phát sự kiện; TV2: 2 observer | integration TV1: luồng + rollback |
| L3 Trả phòng | TV2 → TV1, **1 transaction** | booking → Đã trả phòng → cộng điểm lưu trú, nâng hạng → ghi tương tác → `ISurveyScheduler` (TV1) tạo phiếu khảo sát Chờ → commit. Lỗi ở bước TV1 → rollback cả việc trả phòng | TV2: trả phòng; TV1: `SurveyScheduler` | integration TV2: luồng + rollback |
| L4 Chọn khách cho chiến dịch | TV2 hỏi TV1 | phân khúc (Strategy) → bỏ khách không đồng ý nhận ưu đãi → bỏ khách đang có khiếu nại chưa đóng (`ITicketQuery`) | TV2 gọi, TV1 cài | unit TV2 (Fake) |
| L5 Xóa khách | TV2 hỏi TV1 | chặn nếu khách có ticket (`ITicketQuery`) hoặc có lưu trú | TV2 gọi, TV1 cài | unit TV2 (Fake) |
| L6 Dashboard 360 | TV2 hỏi TV1 | số ticket, số khiếu nại, điểm hài lòng trung bình theo khách (`ICustomerCareStats`, lấy theo lô) | TV2 gọi, TV1 cài | integration TV2 (báo cáo) |

### 4.5 Hợp đồng giữa hai phân hệ (interface trong `Domain/Interfaces`, tiêm qua constructor)
Chữ ký chốt chung ở M6 (27/11). Bên **dùng** viết file khai báo (vì bên dùng biết mình cần gì), bên kia cài đặt. Đổi sau M6 phải được cả hai duyệt trong PR.

| Interface | Khai báo | Cài đặt | Dùng cho |
|---|---|---|---|
| `ICustomerDirectory` | TV1 | TV2 (`CustomerDirectory`) | tìm khách, hệ số SLA/ưu tiên theo hạng, hạng theo lô (L1, R2) |
| `IStayLookup` | TV1 | TV2 (`StayLookup`) | khách có đang ở phòng này không (L1, Q4) |
| `ITicketObserver` | TV1 | TV2 (`GoodwillPointsObserver`, `InteractionLogger`) | sự kiện đổi trạng thái ticket (L2, Q12) |
| `ITicketQuery` | TV2 | TV1 (`TicketQuery`) | khách có ticket không, có khiếu nại chưa đóng không (L4, L5) |
| `ISurveyScheduler` | TV2 | TV1 (`SurveyScheduler`) | tạo phiếu khảo sát chờ khi trả phòng (L3) |
| `ICustomerCareStats` | TV2 | TV1 (`CustomerCareStats`) | thống kê CSKH theo khách cho dashboard (L6) |
| Dùng chung | | | `IUnitOfWork`, `IClock` (TV1); `IReportExporter` (TV2) |

## 5. Thiết kế chi tiết

### 5.1 Trang web (13 trang nghiệp vụ, 7 Master-Detail, 3 danh mục đơn = 23%; thêm đăng nhập và trang chủ)
Tên trang chốt 27/11 theo khung slide 24–25 của HuongdanXDHTTTQL, viết bằng tiếng Anh: `frm` + 3 số + `_dm_` (danh mục) / `_nv_` (nghiệp vụ) / `_bc_` (báo cáo) + tên; đánh số theo người (0xx hệ thống, 1xx TV1, 2xx TV2). Trong Razor Pages: file `Pages/frm201_dm_customer.cshtml`, lớp `frm201_dm_customerModel`, đường dẫn `/frm201_dm_customer`; trang chi tiết của Master-Detail là `?id=`. Chữ trên menu và tiêu đề trang tiếng Việt.

| Trang | Nội dung | Loại | Master → Detail (tổng tính động) | Nút BFD | Biểu mẫu | TV |
|---|---|---|---|---|---|---|
| frm000_login | Đăng nhập | hệ thống | – | F1.1 | – | 1 |
| frm001_main | Trang chủ theo vai trò (ô số liệu, việc cần làm, sinh nhật 30 ngày tới, 2 biểu đồ) | hệ thống | – | – | – | 1 |
| frm101_dm_staff | Phòng ban & nhân viên/tài khoản | danh mục | phòng ban → nhân viên (số phiếu đang giữ) | F1.2 | – | 1 |
| frm102_nv_ticket_create | Lập phiếu yêu cầu (hiện ngay hạn SLA dự kiến) | nghiệp vụ | – | F2.1 | BM1 | 1 |
| frm103_nv_ticket_search | Tra cứu & phân công (lọc: trạng thái, ưu tiên, loại, nhân viên, khoảng ngày, từ khóa, chỉ phiếu trễ SLA; nút xóa lọc) | nghiệp vụ | – | F3.1 | – | 1 |
| frm104_nv_ticket_process | Xử lý phiếu yêu cầu | nghiệp vụ | phiếu → nhật ký xử lý (số bước, thời gian xử lý) | F3.2 | BM2 | 1 |
| frm105_nv_survey | Phiếu khảo sát | nghiệp vụ | phiếu chờ → điểm từng tiêu chí (điểm TB tính ngay) | F4.1 | BM3 | 1 |
| frm106_bc_customer_care | Báo cáo CSKH (R1, R2) | báo cáo | – | F4.2 | – | 1 |
| frm201_dm_customer | Hồ sơ khách 360 (lọc: từ khóa, hạng, quốc tịch, đồng ý ưu đãi) | danh mục | khách → lưu trú, tương tác, giao dịch điểm, phiếu yêu cầu (tổng chi tiêu, số lần ở, số dư) | F5 | BM4 | 2 |
| frm202_dm_room | Loại phòng & phòng | danh mục | loại phòng → phòng (số phòng) | F6.1 | – | 2 |
| frm203_nv_stay | Đặt phòng, nhận/trả phòng (lọc: ngày, trạng thái, khách) | nghiệp vụ | – | F6.2 | BM5 | 2 |
| frm204_dm_reward | Danh mục quà | danh mục | – | F7.1 | – | 2 |
| frm205_nv_loyalty | Sổ điểm & đổi quà | nghiệp vụ | khách → giao dịch điểm (số dư tính ngay) | F7.2 | BM6 | 2 |
| frm206_nv_campaign | Chương trình chăm sóc | nghiệp vụ | chương trình → khách nhận (đã gửi, phản hồi, tỷ lệ) | F8.1 | BM7 | 2 |
| frm207_bc_customer_360 | Báo cáo khách hàng & dashboard 360 (R3, R4, R5) | báo cáo | – | F8.2 | – | 2 |

**Quy ước giao diện (chốt 27/11; bản mẫu bấm được do TV1 dựng, link trong bản kế hoạch chuyển đổi):**
- Bố cục: thanh bên trái nhóm theo phân hệ, thu gọn còn icon (nhớ bằng localStorage); thanh trên có tên trang, ô tìm khách, bộ phận và vai trò người dùng, số phiên bản, Đăng xuất. Chỉ giao diện sáng, chỉ máy tính (từ 1280px), mật độ vừa (cỡ mặc định của Bootstrap), hiệu ứng nhẹ (~150 ms, tắt khi máy đặt giảm chuyển động).
- Màu và chữ: tông Navy & đồng thau ghi đè biến `--bs-*` trong `site.css`; Cormorant Garamond cho tiêu đề trang, tên khách, trang đăng nhập (mã như `KH001` vẫn dùng chữ không chân); Be Vietnam Pro cho mọi chữ khác.
- Thêm/sửa bằng Bootstrap modal, nút "Thực hiện" / "Hủy bỏ"; modal xóa hoặc hủy có tiêu đề dạng câu hỏi và nút "Thực hiện" màu đỏ; server báo lỗi thì mở lại modal kèm lỗi.
- Master-Detail: trang danh sách → trang chi tiết có tab; đầu trang chi tiết chung một kiểu (link quay lại, thẻ có tên, nhãn, thông tin); khối tổng tính khi tải trang, không lưu CSDL.
- Bảng dùng chung: phân trang và sắp xếp ở server, chọn cột (JS + localStorage), tiêu đề cố định khi cuộn (`position: sticky`); cột đầu là liên kết tới trang chi tiết; bảng rỗng có câu hướng dẫn.
- Thông báo: toast ở góc (đọc `TempData` sau khi lưu); toast lỗi không tự tắt, có nút đóng. Nút bị khoá theo vai trò hoặc quy tắc có một dòng nêu lý do.
- Lỗi nhập hiện cạnh ô (`is-invalid` + `invalid-feedback`, gắn `aria-describedby`); con trỏ nhảy tới ô lỗi đầu tiên.
- Tương phản đạt WCAG 2.1 AA: chữ ≥ 4,5:1, viền ô nhập và vòng focus ≥ 3:1; trang có `lang="vi"`.
- Nhãn trạng thái màu cho phiếu yêu cầu, hạn SLA, lưu trú, hạng thành viên; nhãn đọc được bằng chữ, không chỉ dựa vào màu.

Biểu mẫu thật cần thu thập ở M2: BM1 Phiếu yêu cầu/khiếu nại, BM2 Phiếu theo dõi xử lý, BM3 Phiếu khảo sát hài lòng (TV1); BM4 Phiếu đăng ký thành viên/hồ sơ khách, BM5 Phiếu đặt phòng/nhận–trả phòng, BM6 Phiếu đổi quà, BM7 Danh sách chăm sóc theo chương trình (TV2).

### 5.2 CSDL SQLite (17 bảng; 4 n–n có thuộc tính; 5 khóa ngoại nối hai phân hệ; 4 danh mục = 24%)
| Bảng | TV | Loại | Cột chính | Ghi chú |
|---|---|---|---|---|
| `departments` | 1 | danh mục | Id, Code, Name | |
| `employees` | 1 | | Id, Username, PasswordHash, Salt, FullName, Phone, Email, Role, DepartmentId, IsActive | Role CHECK 4 giá trị; mật khẩu PBKDF2 |
| `tickets` | 1 | giao dịch | Id, Type, **CustomerId**, **RoomId**, Title, Description, Priority, State, AssignedAgentId, CreatedById, CreatedAt, DueAt, ResolvedAt, ResolutionNote, ClosedAt | 2 khóa nối sang TV2 |
| `ticket_logs` | 1 | **n–n** ticket–nhân viên | Id, TicketId, EmployeeId, Action, FromState, ToState, Note, At | nhật ký xử lý |
| `survey_criteria` | 1 | danh mục | Id, Name, IsActive | sạch sẽ, thái độ, tiện nghi, ăn uống, giá trị |
| `surveys` | 1 | giao dịch | Id, **BookingId** (UNIQUE), Status (Chờ/Đã khảo sát), CreatedAt, CompletedAt, RecordedById, Comment | khóa nối sang TV2; không lưu CustomerId (suy từ booking, giữ 3NF) |
| `survey_answers` | 1 | **n–n** phiếu–tiêu chí | SurveyId, CriterionId, Score (CHECK 1–5) | PK (SurveyId, CriterionId) |
| `customers` | 2 | | Id, FullName, Phone, Email, IdentityNumber, Nationality, DateOfBirth, MarketingConsent, Tier, Notes, CreatedAt | Phone/Email/IdentityNumber UNIQUE |
| `interactions` | 2 | giao dịch | Id, CustomerId, OccurredAt, Channel, Summary, EmployeeId, **TicketId** | khóa nối sang TV1 |
| `room_types` | 2 | danh mục | Id, Name, PricePerNight, Capacity | |
| `rooms` | 2 | | Id, Number, Floor, RoomTypeId | |
| `bookings` | 2 | giao dịch | Id, CustomerId, RoomId, CheckInDate, CheckOutDate, RatePerNight, Status, CreatedById, CreatedAt, CheckedInAt, CheckedOutAt | **RatePerNight chốt lúc đặt** (thuộc tính lịch sử) |
| `point_transactions` | 2 | giao dịch | Id, CustomerId, Points (+/−), Reason (Lưu trú/Thiện chí/Đổi quà/Điều chỉnh), BookingId, **TicketId**, RedemptionId, CreatedAt | sổ điểm; khóa nối sang TV1 |
| `rewards` | 2 | danh mục | Id, Name, PointsCost, MinTier, Quantity, IsActive | |
| `reward_redemptions` | 2 | **n–n** khách–quà | Id, CustomerId, RewardId, PointsSpent, RedeemedAt, Status, ProcessedById | **PointsSpent chốt lúc đổi** |
| `campaigns` | 2 | giao dịch | Id, Name, SegmentCode, Offer, StartDate, EndDate, CreatedById, CreatedAt | |
| `campaign_targets` | 2 | **n–n** chiến dịch–khách | CampaignId, CustomerId, Channel, SentAt, Response, RespondedAt, RecordedById | PK (CampaignId, CustomerId) = mỗi khách một lần |

- Khóa ngoại nối hai phân hệ: `tickets.CustomerId → customers`, `tickets.RoomId → rooms`, `surveys.BookingId → bookings`, `interactions.TicketId → tickets`, `point_transactions.TicketId → tickets`. Các cột `…ById` của TV2 trỏ sang `employees` của TV1.
- Bật `PRAGMA foreign_keys=ON` mỗi kết nối; CHECK cho trạng thái, điểm, số tiền, ngày; ON DELETE khớp quan hệ UML (hợp thành ↔ CASCADE, còn lại RESTRICT); index cho cột khóa ngoại.
- `Database/schema.sql` + `Database/seed.sql` (một file chung, mỗi người viết phần bảng của mình). App tự tạo `hotel.db` khi chưa có; Admin có lệnh tạo lại CSDL mẫu.

### 5.3 Quy tắc nghiệp vụ (13; liên phân hệ: Q3, Q4, Q5, Q11, Q12, Q13)
Nguyên tắc: service tra cứu dữ liệu, lớp Domain quyết định và ném ngoại lệ. Tên lớp/hàm ở cột cuối là dự kiến, chốt ở M6.

| Mã | TV | Quy tắc | Cài ở đâu (dự kiến) |
|---|---|---|---|
| Q1 | 1 | Vòng đời ticket: Mới → Đang xử lý (phải có người được giao) → Đã giải quyết (phải có ghi chú) → Đã đóng; mở lại từ Đã giải quyết trong `ReopenDays` ngày; Đã đóng không đổi được; mỗi lần đổi ghi nhật ký | các lớp `*State`, `SupportTicket` |
| Q2 | 1 | Nhân viên CSKH không giữ quá `MaxActiveTickets` ticket đang mở; phân công tự động chọn người ít việc nhất trong bộ phận xử lý loại ticket đó | `Agent.EnsureCanTake`, `TicketService.AutoAssign` |
| Q3 | 1 | Hạn SLA = giờ chuẩn theo loại × hệ số ưu tiên × hệ số hạng khách; hạng cao được nâng mức ưu tiên | `SupportTicket.CalculateDueAt`, qua `ICustomerDirectory` |
| Q4 | 1 | Ticket sửa chữa phải gắn đúng phòng khách đang ở | `MaintenanceTicket.Validate`, qua `IStayLookup` |
| Q5 | 1 | Chỉ ghi khảo sát cho phiếu đang Chờ (tạo lúc trả phòng); mỗi lưu trú một phiếu; chấm đủ mọi tiêu chí đang dùng; điểm 1–5 | `SatisfactionSurvey.Complete` |
| Q6 | chung | Phân quyền theo ma trận ở tầng Service; Admin vượt mọi quyền; bắt đầu/giải quyết chỉ người được giao (hoặc Admin) | `Employee` + 4 lớp con; TV1 cài nền + quyền ticket, TV2 cài kiểm quyền trong service của mình |
| Q7 | 2 | SĐT 10 số bắt đầu bằng 0, email hợp lệ, CCCD 12 số; không trùng SĐT/email/CCCD | `Person`, `Customer`, `Customer.EnsureUnique` |
| Q8 | 2 | Ngày trả > ngày nhận; phòng không trùng lịch với booking chưa hủy; giá/đêm chốt lúc đặt; chỉ nhận phòng trong [ngày nhận, ngày trả); chỉ hủy booking chưa nhận phòng | `Booking`, `Room.EnsureAvailable` |
| Q9 | 2 | Trả phòng: điểm = tiền phòng / 10.000đ × hệ số tích điểm của hạng (làm tròn xuống); hạng tính theo tổng điểm đã tích (không trừ điểm đã đổi), có thể lên nhiều bậc, không bao giờ hạ | `MembershipTier` (+ hệ số tích điểm), `Customer.EarnPoints` |
| Q10 | 2 | Đổi quà: số dư ≥ điểm quy đổi; hạng ≥ hạng tối thiểu của quà; quà còn số lượng; số dư không bao giờ âm; điểm quy đổi chốt lúc đổi | `Reward`, `Customer.SpendPoints` |
| Q11 | 2 | Danh sách chiến dịch = phân khúc đã chọn ∩ khách đồng ý nhận ưu đãi − khách có khiếu nại chưa đóng; mỗi khách một lần trong một chiến dịch; chỉ ghi phản hồi khi chiến dịch chưa kết thúc | `Campaign.AddTarget`, `CampaignService`, qua `ITicketQuery` |
| Q12 | 2 | Ticket chuyển sang Đã đóng → khách được cộng điểm thiện chí theo loại ticket (khiếu nại 50, loại khác 10) và ghi một tương tác, cùng transaction với việc đóng | `GoodwillPointsObserver`, `SupportTicket.GoodwillPoints` |
| Q13 | 2 | Không xóa khách đã có lưu trú hoặc ticket | `Customer.EnsureCanBeDeleted`, qua `ITicketQuery` |

Loại ngoại lệ: sai dữ liệu một đối tượng → `ValidationException`; sai quy tắc giữa các đối tượng hoặc sai trạng thái → `BusinessRuleException`; sai quyền → `AuthorizationException`.

### 5.4 Mẫu GoF (4 mẫu, 2 nhóm; Repository/Singleton không tính)
| Mẫu | Nhóm | TV | Các lớp | Giải quyết vấn đề gì | Bài Live OCP |
|---|---|---|---|---|---|
| State | Hành vi | 1 | Context `SupportTicket`; `TicketState` + `OpenState`, `InProgressState`, `ResolvedState`, `ClosedState` | bỏ chuỗi if/switch theo trạng thái; mỗi trạng thái tự quyết thao tác hợp lệ (Q1) | không dùng (thêm trạng thái phải sửa trạng thái cũ) |
| Factory Method | Khởi tạo | 1 | `TicketFactory` → `ComplaintTicketFactory`, `ServiceRequestTicketFactory`, `MaintenanceTicketFactory`; bảng đăng ký ở `Program.cs` | service không `new` lớp ticket cụ thể | thêm `LostItemTicket` + factory, đăng ký 1 dòng |
| Strategy | Hành vi | 2 | `ISegmentStrategy` → `VipSegment`, `LapsedGuestSegment` (lâu chưa quay lại), `BirthdayMonthSegment`; Context `CampaignService`; chọn trên Form lúc chạy | đổi cách chọn khách mà không sửa service | thêm `FrequentGuestSegment` (ở ≥3 lần trong năm) |
| Observer (cầu nối) | Hành vi | 1 phát, 2 nhận | Subject: phía ticket của TV1; `ITicketObserver` → `GoodwillPointsObserver`, `InteractionLogger` (TV2) | TV1 không cần biết TV2 làm gì khi ticket đổi trạng thái | thêm observer mới |

- Đổi `switch` cứng ở `TicketState.FromName` và `TicketMapper` thành bảng đăng ký dựng ở Composition Root, để thêm lớp mới không phải sửa lớp cũ.
- Mỗi người chuẩn bị ≥2 nhược điểm của mẫu mình dùng (câu hỏi W).

### 5.5 Lớp chính theo người (dự kiến, chốt ở M6; * = abstract)
- **Chung (TV1 viết):** `Person`*, `Employee`*, `Administrator`, `Manager`, `Agent`, `Department`; cây ngoại lệ; `IClock`, `IUnitOfWork`.
- **TV1:** `SupportTicket`*, `ComplaintTicket`, `ServiceRequestTicket`, `MaintenanceTicket`, `TicketState`* + 4 state, `TicketLog`, `SatisfactionSurvey`, `SurveyAnswer`, `SurveyCriterion`, `TicketFactory`* + 3 factory; enum `TicketPriority`, `TicketAction`, `SurveyStatus`.
- **TV2:** `Customer`, `MembershipTier`* + `StandardTier`, `SilverTier`, `GoldTier`, `VipTier`, `Interaction`, `RoomType`, `Room`, `Booking`, `PointTransaction`, `Reward`, `RewardRedemption`, `Campaign`, `CampaignTarget`, `Receptionist`, `ISegmentStrategy` + 3 phân khúc; enum `BookingStatus`, `ContactChannel`, `PointReason`, `CampaignResponse`.
- Cây kế thừa cho LSP: `Employee` (4), `SupportTicket` (3), `TicketState` (4), `MembershipTier` (4). LSP xuyên phân hệ: `Receptionist` (TV2) đi qua `TicketService` (TV1) dưới kiểu `Employee`, không ép kiểu.
- Dùng lại: `Person`, `Customer`, 4 hạng, `Employee`/`Agent`/`Manager`, `Department`, `SupportTicket` + 3 lớp con + 4 state, `Room`, `RoomType`, `Booking`, `Interaction`, `SatisfactionSurvey`, `PasswordHasher`, `ErrorHandler`, phần lớn service, 22 test cũ (sửa theo thiết kế mới).

### 5.6 Kiến trúc
```
Database/            schema.sql, seed.sql
Resource/            icon, logo
src/HotelCRM.GUI     Razor Pages (Pages/frm…), wwwroot/lib; Program.cs = Composition Root (tự new, theo từng request)
src/HotelCRM.BLL     Services, Queries (cài interface ranh giới), Observers, Segments, Factories, Reporting
src/HotelCRM.Domain  Common, Staff | Tickets, Surveys (TV1) | Customers, Hotel, Loyalty, Campaigns (TV2) | Interfaces
src/HotelCRM.DAL     Sqlite (kết nối, UnitOfWork, tạo CSDL), Repositories, ReadModels (JOIN cho lưới/báo cáo)
tests/HotelCRM.Tests Unit/, Integration/, Fakes/
```
- Chiều phụ thuộc: GUI → BLL → Domain ← DAL; Domain không tham chiếu project nào; trang (PageModel) không chứa SQL hay quy tắc; BLL không dùng `HttpContext`.
- Project: `GUI` là project Razor Pages mới (`net10.0`), project WinForms bị xoá; đổi tên `Business → BLL`, `Data → DAL` (cách làm ở mục 3.1).
- Không làm Solution Folder `Dlllib` và tên solution kèm phiên bản của HuongdanXDHTTTQL (chốt 27/11, mục 3.1).
- Repository đọc CSDL khi cần (bỏ nạp hết vào bộ nhớ) để rollback không làm lệch bộ nhớ. Tham chiếu giữa các aggregate bằng Id. Lưới và báo cáo dùng truy vấn JOIN, không N+1.
- `IUnitOfWork.Execute(...)`: một connection + một transaction dùng chung; lời gọi lồng nhau tham gia transaction ngoài. L2 và L3 chạy trọn trong một transaction.
- Cây ngoại lệ: `DomainException` → `ValidationException`, `BusinessRuleException`, `AuthorizationException`; DAL bọc lỗi SQLite thành `DataAccessException`. Không `catch {}` rỗng. `ErrorHandler` hiện thông báo tiếng Việt (toast và trang lỗi chung), ghi `logs/error.log`, không lộ stack trace.
- Web: đăng nhập cookie, mỗi request một `UnitOfWork`, SQLite WAL + `busy_timeout`, HTTP tại `http://localhost:5080` chỉ nghe localhost, antiforgery mặc định, InputModel ở GUI (chi tiết mục 3.1).
- Cấu hình: một `appsettings.json` (+ `appsettings.example.json`): cổng chạy (`Urls`), đường dẫn CSDL, `MaxActiveTickets`, `ReopenDays`, số ngày "lâu chưa quay lại". Bỏ Singleton `AppConfig`: đọc một lần ở `Program.cs` rồi truyền qua constructor. Thời gian qua `IClock`.

### 5.7 Báo cáo (5; đều xuất Excel có tiêu đề, ngày xuất, dòng tổng; không tính booking đã hủy)
| Mã | Báo cáo | TV | Biểu đồ | Liên PH |
|---|---|---|---|---|
| R1 | Hiệu suất nhân viên CSKH: số ticket được giao, đã giải quyết, giờ xử lý TB, số trễ SLA | 1 | | |
| R2 | Ticket & SLA theo loại, tháng và hạng khách | 1 | ✓ | ✓ (hạng lấy theo lô qua `ICustomerDirectory`) |
| R3 | Phân khúc khách theo hạng: số khách, tổng chi tiêu, điểm đã tích/đổi | 2 | ✓ | |
| R4 | Hiệu quả chiến dịch: số khách nhận, số phản hồi, tỷ lệ quan tâm/đặt phòng | 2 | | |
| R5 | Dashboard 360: theo khách — số lần ở, chi tiêu, số khiếu nại, điểm hài lòng TB | 2 | | ✓ (qua `ICustomerCareStats`) |

### 5.8 Test (28 = mỗi người 10 unit + 4 integration)
Unit test chạy không cần CSDL, dùng Fake; integration test chạy trên CSDL SQLite tạm có seed; `IClock` cố định thời gian; giá trị kỳ vọng lấy từ bảng tính tay, không chép công thức của code.

| | TV1 | TV2 |
|---|---|---|
| Unit (10) | chuyển trạng thái hợp lệ; giải quyết thiếu ghi chú; Đã đóng bất biến; mở lại đúng mốc/quá mốc (BVA); tải tối đa đúng ngưỡng (BVA); hạn SLA khớp tính tay; ticket sửa chữa sai phòng (Fake `IStayLookup`); nâng ưu tiên theo hạng (Fake `ICustomerDirectory`); khảo sát thiếu tiêu chí, điểm 0/1/5/6 (BVA); factory trả đúng lớp con | SĐT/email/CCCD sai (BVA độ dài); trùng SĐT; đặt trùng lịch và nối tiếp ngày (BVA); nhận phòng trước ngày; tích điểm theo hạng + lên 2 bậc; đổi quà giảm số dư không hạ hạng; đổi quà thiếu điểm/thiếu hạng/hết quà; 3 phân khúc chọn đúng khách (BVA ngày); chiến dịch loại khách có khiếu nại (Fake `ITicketQuery`); xóa khách có ticket (Fake `ITicketQuery`) |
| Integration (4) | L2 luồng đầy đủ; L2 rollback khi observer TV2 lỗi giữa chừng; R2 khớp tính tay; RBAC (Lễ tân phân công bị chặn, CSKH xử lý ticket người khác bị chặn, Admin được) | L3 luồng đầy đủ; L3 rollback khi `SurveyScheduler` lỗi; R5 khớp tính tay; RBAC (CSKH đổi quà bị chặn, Lễ tân tạo chiến dịch bị chặn, Admin được) |
| Fake tự viết | `FakeCustomerDirectory`, `FakeStayLookup` | `FakeTicketQuery`, `FakeSurveyScheduler`, `FakeCustomerCareStats` |

Mỗi người tự thử tiêm lỗi vào phần mình (đổi `>=` thành `>`, bỏ rollback, bỏ kiểm quyền) và chắc chắn có test chuyển đỏ.

### 5.9 Dữ liệu mẫu (≥150 dòng, 17 bảng, tháng 9–11/2026)
- Tài khoản: 1 Admin, 2 Quản lý, 3 Lễ tân, 4 CSKH (mật khẩu ghi trong README).
- Ước lượng: 4 phòng ban, 10 nhân viên, 5 tiêu chí, 4 loại phòng, 20 phòng, 30 khách (đủ 4 hạng), 40 booking, 30 ticket, ~80 dòng nhật ký, 25 phiếu khảo sát, ~100 điểm tiêu chí, ~60 giao dịch điểm, 6 quà, 10 lượt đổi, 3 chiến dịch, 40 khách nhận, ~60 tương tác.
- Đủ trạng thái: ticket đã đóng/quá hạn SLA/mở lại; booking đã hủy/đang ở/đã trả; phiếu khảo sát chờ/đã làm; lượt đổi đã hủy; chiến dịch đã kết thúc. CRM không có "công nợ"; khi vấn đáp giải thích các trạng thái tương đương.
- Bảng đáp án tính tay cho R2 và R5 nằm trong `docs/analysis/10-kiem-thu.md`.

### 5.10 Đối chiếu định mức N2
| Hạng mục | Định mức N2 | TV1 | TV2 | Tổng |
|---|---|---|---|---|
| Nhánh BFD cấp 2 | ≥6, ≥3/TV | 4 | 4 | 8 |
| Use Case | ≥8, ≥4/TV, ≥4 phức hợp, ≥2 liên PH | 6 | 5 | 11; 5 phức hợp; 7 liên PH |
| Form nghiệp vụ (trang web) | ≥9, ≥4/TV, ≤40% danh mục | 6 | 7 | 13; danh mục 23% |
| Form Master-Detail | ≥4, ≥2/TV | 3 | 4 | 7 |
| Báo cáo | ≥5, ≥2/TV, ≥1 tổng hợp, ≥1 biểu đồ, xuất file | 2 | 3 | 5; 2 liên PH; 2 biểu đồ |
| Vai trò | ≥3 | | | 4 |
| Bảng 3NF | ≥11, ≥3 n–n, ≥2 khóa nối | 7 (2 n–n) | 10 (2 n–n) | 17; 4 n–n; 5 khóa nối |
| Lớp Domain thực chất / UML | ≥13 / ≥20 | ~20 | ~20 | ~40 |
| Quy tắc | ≥8, ≥4/TV, ≥2 liên PH | 5 | 7 | 13 (+Q6 chung); 6 liên PH |
| GoF | ≥3, ≥2 nhóm, mỗi TV ≥1, ≥1 cầu nối | State, Factory Method | Strategy | 4 với Observer cầu nối |
| Sơ đồ Sequence/State | ≥4, ≥2/TV, ≥1 xuyên PH | tuần tự L2 + trạng thái ticket | tuần tự L3 + trạng thái booking | 4; 2 xuyên PH |
| Unit / Integration / Tổng test | ≥18 (≥9/TV) / ≥8 / ≥26 | 10 / 4 | 10 / 4 | 20 / 8 / 28 |
| Seed | ≥120 dòng, ≥11 bảng, 2–3 tháng | | | ≥150, 17 bảng, 3 tháng |
| Git | ≥24 commit, ≥3 tuần, ≥2 PR, 40–60%/TV | | | đo theo các mốc ở mục 3.1 |

## 6. Phân công

### 6.1 Nguyên tắc
1. Chia dọc theo phân hệ: mỗi người có đủ Domain, mẫu thiết kế, test, giao diện, tài liệu, slide của phần mình.
2. Ai viết code thì người đó commit bằng tài khoản mình. Được hỏi nhau, review nhau; không viết hộ.
3. Mỗi người đọc hiểu phần của bạn đủ để trả lời câu hỏi chéo và đặt breakpoint trong code của bạn.
4. Hai bên chỉ nói chuyện qua interface ở mục 4.5. Phần bạn chưa xong thì dùng Fake.
5. Tỷ lệ code 40–60%: TV1 đã có sẵn nhiều code cũ, nên phần việc mới và phần chung còn lại nghiêng về TV2.

### 6.2 Ai sở hữu gì
| Tầng | TV1 | TV2 |
|---|---|---|
| Domain | `Common/**`, `Staff/**` (trừ `Receptionist`), `Tickets/**`, `Surveys/**` | `Customers/**`, `Hotel/**`, `Loyalty/**`, `Campaigns/**`, `Staff/Receptionist` |
| Interface (khai báo) | `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`, `IUnitOfWork`, `IClock` | `ITicketQuery`, `ISurveyScheduler`, `ICustomerCareStats`, `IReportExporter`, `ISegmentStrategy` |
| BLL | `AuthService`, `EmployeeService`, `TicketService`, `SurveyService`, `CustomerCareReportService`, `Factories/**`, `TicketQuery`, `SurveyScheduler`, `CustomerCareStats` | `CustomerService`, `StayService`, `LoyaltyService`, `CampaignService`, `CustomerReportService`, `DashboardService`, `CustomerDirectory`, `StayLookup`, `Observers/**`, `Segments/**`, `ExcelReportExporter` |
| DAL | `Sqlite/**` (kết nối, UnitOfWork, tạo CSDL, nạp seed); repository + truy vấn báo cáo 7 bảng TV1 | repository + truy vấn báo cáo 10 bảng TV2 |
| Database | `schema.sql`/`seed.sql`: khung file + 7 bảng TV1 | 10 bảng TV2 |
| GUI | dòng đăng ký của mình trong `Program.cs`; đăng nhập cookie, lọc menu theo vai trò, `ErrorHandler` (đẩy lỗi ra toast, trang lỗi chung), frm000, nội dung frm001, frm101–frm106 | khung web: project `HotelCRM.GUI`, khung `Program.cs` (mỗi người thêm dòng đăng ký của mình), `_Layout` (thanh bên, thanh trên), menu `AppMenu.cs`, toast, `site.js` (từ 28/11); frm201–frm207; thành phần dùng chung (bảng, thanh lọc, modal, báo lỗi cạnh ô, nhãn trạng thái, ô số liệu, `site.css`), hàm định dạng tiền/ngày, hàm vẽ Chart.js |
| Tests | 10 unit + 4 integration TV1; Fake cho interface của TV2 | 10 unit + 4 integration TV2; Fake cho interface của TV1; hạ tầng integration test dùng chung |
| Tài liệu | phần chung (4W+1H tổng, cây BFD, sơ đồ UC tổng, DFD ngữ cảnh + mức 0, ma trận quyền, kiến trúc/package) + phân hệ TV1 | phân hệ TV2 |
| Slide | phần chung + TV1 + 1 slide kiến trúc web | phần TV2; biên dịch (máy có TeX) |

### 6.3 Lịch chung
**P1 — Phân tích thiết kế (22–29/11), nộp slide 30/11.** Tài liệu trong `docs/analysis/`, sơ đồ trong `docs/analysis/diagrams/`. Danh sách việc chi tiết của từng người ở mục 6.4–6.6. Hạn trong bảng là hạn gốc; ngày 27/11 hai người chốt giữ hạn slide, TV1 làm phần mình của M2–M8 trong 27–29/11 (lịch từng module ở `docs/tasks/v2-p1-analysis.md` mục 2). Ảnh giao diện trong slide lấy từ bản mẫu web.

| Module | Hạn | TV1 | TV2 |
|---|---|---|---|
| M1 Chuẩn bị | 22/11 | `CLAUDE.md`, `docs/PLAN.md` v2, `docs/PROGRESS.md` v2, file task P1, nhánh `v2` | đọc plan, xác nhận phân công; cùng merge PR #1 |
| M2 Khảo sát (`01-khao-sat.md`) | 23/11 | 4W+1H toàn hệ thống; yêu cầu phi chức năng; BM1–BM3 | hiện trạng phần khách hàng/lưu trú/điểm; BM4–BM7; yêu cầu chức năng F5–F8 |
| M3 BFD + UC (`02-bfd.md`, `03-use-case.md`) | 24/11 | cây BFD 8 nhánh, lá F1–F4; sơ đồ UC tổng; đặc tả UC04, UC05 | lá F5–F8; đặc tả UC08, UC09, UC10 |
| M4 Quy tắc, quyền, DFD (`04-quy-tac.md`, `05-dfd.md`) | 25/11 | Q1–Q6 (tiền/hậu điều kiện, ngoại lệ); ma trận quyền; DFD ngữ cảnh + mức 0; DFD mức 1 của F3 | Q7–Q13; DFD mức 1 của F7 |
| M5 ERD + CSDL (`06-erd.md`) | 26/11 | khung `erd.puml` + phần 7 bảng TV1; giải thích 3NF | phần 10 bảng TV2; giải thích thuộc tính lịch sử (giá chốt, điểm chốt) |
| M6 Lớp + package + hợp đồng (`07-thiet-ke-lop.md`) | 27/11 | sơ đồ lớp phần chung + TV1; package diagram; khai báo 3 interface phía TV1 | sơ đồ lớp TV2; khai báo 3 interface phía TV2; **cả hai chốt bảng hợp đồng** |
| M7 Hành vi (`08-hanh-vi.md`) | 28/11 | tuần tự L2 đóng ticket (xuyên PH, có alt rollback); máy trạng thái ticket | tuần tự L3 trả phòng (xuyên PH); máy trạng thái booking |
| M8 Giao diện + test (`09-giao-dien.md`, `10-kiem-thu.md`) | 28/11 | bảng Form F1–F4; ma trận test TV1; bảng tính tay R2 | bảng Form F5–F8; ma trận test TV2; bảng tính tay R5 |
| M9 Slide + tự đánh giá L2 | 29/11 | slide phần chung + TV1 | slide TV2, biên dịch PDF; cả hai điền tự đánh giá L2 |

**P2–P5**
| Giai đoạn | Thời gian | Mục tiêu | Mốc kiểm cuối giai đoạn |
|---|---|---|---|
| P2 Hạ tầng | 1–4/12 | TV1 làm đăng nhập cookie (đưa lên trước 2/12), nền SQLite và phân quyền trên khung web của TV2 (28/11); TV2 làm Domain mới và phần dùng chung song song | đăng nhập được 4 vai trò trên SQLite qua trình duyệt; bản publish chạy trên máy sạch (4/12); TV2 có 10 unit test xanh |
| P3 Phân hệ | 5–10/12 | mỗi người làm repository, service, trang, báo cáo, seed của mình; phần dùng chung của TV2 xong 7/12 | đủ 13 trang, R1–R4, seed ≥150 dòng |
| P4 Ghép & đóng gói | 11–12/12 | luồng L2/L3, dashboard R5, publish | 28 test xanh; chạy trên máy sạch ≤3 phút theo README (có bước mở trình duyệt); video quay trên trình duyệt |
| P5 Vấn đáp | 13/12 → 30/12 | ôn, Live OCP, video, báo cáo; tập đặt breakpoint khi app chạy dạng web | buổi vấn đáp; nộp cuối 30/12 |

Nếu lịch vấn đáp của nhóm muộn hơn 13/12, vẫn giữ code xong trước buổi vấn đáp ít nhất 3 ngày.

### 6.4 Việc của TV1 — Chăm sóc & hỗ trợ khách hàng (+ phần nền chung)
Trong ngoặc là hạn. Trạng thái (đánh dấu `[x]`) theo dõi ở `docs/PROGRESS.md`, không đánh dấu ở file này.

**Tài liệu & sơ đồ (P1)**
- M1: `CLAUDE.md`, `docs/PLAN.md` v2, `docs/PROGRESS.md` v2, file task P1, nhánh `v2` (22/11)
- 4W+1H toàn hệ thống; yêu cầu phi chức năng; biểu mẫu BM1 Phiếu yêu cầu/khiếu nại, BM2 Phiếu theo dõi xử lý, BM3 Phiếu khảo sát (23/11)
- Cây BFD 8 nhánh, lá F1–F4; sơ đồ Use Case tổng; đặc tả UC04, UC05 (24/11)
- Q1–Q6 với tiền/hậu điều kiện và ngoại lệ; ma trận quyền; DFD ngữ cảnh, mức 0, mức 1 của F3 (25/11)
- Khung `erd.puml` + 7 bảng TV1; giải thích 3NF (26/11)
- Sơ đồ lớp phần chung + TV1; package diagram; khai báo `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`; chốt hợp đồng với TV2 (27/11)
- Sơ đồ tuần tự L2 (đóng ticket, có khung alt rollback); máy trạng thái ticket (28/11)
- Bảng Form F1–F4; ma trận test TC1-xx; bảng tính tay R2 (28/11)
- Slide phần chung + TV1 + 1 slide kiến trúc web (29/11)
- Đưa quyết định chuyển sang web vào `CLAUDE.md`, `docs/PLAN.md`, `docs/PROGRESS.md`, file task P1 (27/11)

**Nền chung (P2, 1–4/12)**
- Đổi tên project thành `HotelCRM.BLL`, `HotelCRM.DAL` bằng commit chỉ có `git mv`; xoá code v1 không còn dùng của TV1 (WinForms, JSON, 3 strategy phân công cũ, `UserSession`, `UiNotificationObserver`); thêm `Database/`, `Resource/` (1/12)
- Trên khung web của TV2: đăng nhập cookie (frm000), lọc menu theo vai trò, nội dung trang chủ (frm001), trang lỗi chung; đưa lên trước 2/12
- SQLite: kết nối, `UnitOfWork` theo từng request, WAL + `busy_timeout`, tạo CSDL từ `schema.sql` + nạp `seed.sql`; một repository mẫu để TV2 làm theo
- Cây ngoại lệ, `ErrorHandler` tiếng Việt, `appsettings.json` + file mẫu (có `Urls`), bỏ Singleton, `IClock`
- `Employee` + `Administrator`, `Manager`, `Agent`
- `schema.sql` 7 bảng TV1; seed tài khoản 4 vai trò; `.editorconfig` (khối `Pages/`), `.gitattributes`; sửa `.claude/rules/*` theo v2 (`winforms.md` → `web.md`)
- Publish self-contained và chạy thử trên máy sạch (4/12)

**Phân hệ CSKH (P3, 5–10/12)**
- Domain: `SupportTicket` + 3 loại ticket, 4 state (bảng đăng ký thay `switch`), `TicketLog`, `SatisfactionSurvey`, `SurveyAnswer`, `SurveyCriterion`
- Repository + truy vấn báo cáo cho 7 bảng TV1
- `TicketService` (tạo, phân công, xử lý, nhật ký, phát sự kiện), 3 factory, `SurveyService`
- Cài cho TV2 dùng: `TicketQuery`, `SurveyScheduler`, `CustomerCareStats` (8/12)
- Trang frm101–frm106 (Master-Detail: frm101, frm104, frm105) dùng thành phần chung của TV2; báo lỗi cạnh ô; tiền/ngày đúng định dạng; khối số liệu CSKH trên frm001
- R1, R2 (Chart.js), xuất Excel (tải file)
- Seed 7 bảng: đủ 3 loại ticket, đủ trạng thái, có ticket quá hạn SLA, nhật ký, phiếu khảo sát chờ/đã làm

**Test (P2–P4)**
- `FakeCustomerDirectory`, `FakeStayLookup`
- 10 unit test (mục 5.8)
- 4 integration: R2 khớp tính tay; RBAC; L2 luồng đầy đủ; L2 rollback

**Đóng gói (P4, 11–12/12)**
- Publish self-contained, chạy thử lại trên máy sạch
- README: cài đặt, mở trình duyệt, tài khoản demo, lệnh chạy test

**Vấn đáp (P5)**
- Live OCP khi tắt AI: thêm `LostItemTicket` + factory, đăng ký 1 dòng (tập ≥2 lần)
- Đặt breakpoint theo sơ đồ L2 (của mình) và L3 (của TV2) khi app chạy dạng web, đọc Call Stack
- Trả lời câu hỏi chéo phần TV2: Strategy phân khúc, sổ điểm, vì sao hạng không hạ, rollback L3
- Nêu ≥2 nhược điểm của State và Factory Method
- Demo phần mình trong video; chương báo cáo phân hệ TV1 + chương kiến trúc

**Định mức riêng của TV1 (checklist đòi / kế hoạch):** nhánh BFD ≥3 / 4 · UC ≥4 / 6 · Form (trang) ≥4 / 6 · Master-Detail ≥2 / 3 · báo cáo ≥2 / 2 · quy tắc ≥4 / 5 · mẫu GoF ≥1 / 2 · unit test ≥9 / 10 · sơ đồ hành vi ≥2 / 2 · test dùng Fake của bạn ≥2 / 2.

### 6.5 Việc của TV2 — Quan hệ khách hàng & khách hàng thân thiết (+ phần dùng chung được giao)
Trong ngoặc là hạn. Trạng thái (đánh dấu `[x]`) theo dõi ở `docs/PROGRESS.md`, không đánh dấu ở file này.

**Tài liệu & sơ đồ (P1)**
- Đọc plan, xác nhận phân công; cùng merge PR #1 (22/11)
- Hiện trạng phần khách hàng, lưu trú, điểm, chương trình chăm sóc; biểu mẫu BM4 Phiếu đăng ký thành viên, BM5 Phiếu đặt phòng/nhận–trả phòng, BM6 Phiếu đổi quà, BM7 Danh sách chăm sóc theo chương trình; yêu cầu chức năng F5–F8 (23/11)
- Lá BFD F5–F8; đặc tả UC08, UC09, UC10 (24/11)
- Q7–Q13 với tiền/hậu điều kiện và ngoại lệ; DFD mức 1 của F7 (25/11)
- Phần ERD 10 bảng TV2; giải thích thuộc tính lịch sử (giá/đêm chốt, điểm quy đổi chốt) (26/11)
- Sơ đồ lớp TV2; khai báo `ITicketQuery`, `ISurveyScheduler`, `ICustomerCareStats`; chốt hợp đồng với TV1 (27/11)
- Sơ đồ tuần tự L3 (trả phòng, có khung alt rollback); máy trạng thái booking (28/11)
- Bảng Form F5–F8; ma trận test TC2-xx; bảng tính tay R5 (28/11)
- Slide phần TV2; biên dịch PDF cả bộ (29/11)

**Khung web (28/11)**
- Project `HotelCRM.GUI` (Razor Pages, `net10.0`) trong `HotelCRM.sln`; khung `Program.cs`; `_Layout` (thanh bên thu gọn, thanh trên); menu `Navigation/AppMenu.cs`; `site.js`; thư viện và font trong `wwwroot/lib`
- Toast đọc `TempData`, dùng cho thông báo lưu và lỗi từ `ErrorHandler` (P2)

**Domain (P2, 1–4/12, chưa cần CSDL)**
- Sửa `Customer` (CCCD, ngày sinh, đồng ý nhận ưu đãi), `MembershipTier` (hệ số tích điểm), `Booking` (giá/đêm chốt)
- Thêm `PointTransaction`, `Reward`, `RewardRedemption`, `Campaign`, `CampaignTarget`, `Receptionist`
- `ISegmentStrategy` + `VipSegment`, `LapsedGuestSegment`, `BirthdayMonthSegment`
- `schema.sql` 10 bảng TV2

**Phân hệ quan hệ khách hàng (P3, 5–10/12)**
- Repository + truy vấn báo cáo cho 10 bảng TV2 (theo repository mẫu của TV1)
- `CustomerService`, `StayService`, `LoyaltyService`, `CampaignService`, có kiểm quyền trong từng service
- Cài cho TV1 dùng: `CustomerDirectory`, `StayLookup` (8/12); `GoodwillPointsObserver`, `InteractionLogger` (9/12)
- Phần dùng chung (7/12, để TV1 dùng; quy ước ở mục 5.1): thành phần bảng (phân trang, sắp xếp, chọn cột, tiêu đề cố định), thanh lọc có "Xóa lọc", khung modal, báo lỗi cạnh ô, nhãn trạng thái, ô số liệu, định dạng tiền/ngày, `site.css` theo tông đã chọn, hàm vẽ Chart.js, `ExcelReportExporter`
- Trang frm201–frm207 (Master-Detail: frm201, frm202, frm205, frm206); khối số liệu khách trên frm001
- R3 (Chart.js), R4, xuất Excel (tải file)
- Seed 10 bảng: khách đủ 4 hạng, booking đủ trạng thái, giao dịch điểm, lượt đổi (có lượt hủy), chiến dịch đã kết thúc

**Test (P2–P4)**
- `FakeTicketQuery`, `FakeSurveyScheduler`, `FakeCustomerCareStats`
- Hạ tầng integration test dùng chung: CSDL SQLite tạm từ `schema.sql` + `seed.sql`, `FixedClock` (sau khi TV1 có `DatabaseInitializer`)
- 10 unit test (mục 5.8)
- 4 integration: R5 khớp tính tay; RBAC; L3 luồng đầy đủ; L3 rollback

**Ghép (P4, 11–12/12)**
- Dashboard R5 (frm207)
- README: dữ liệu mẫu, báo cáo

**Vấn đáp (P5)**
- Live OCP khi tắt AI: thêm `FrequentGuestSegment`, đăng ký 1 dòng (tập ≥2 lần)
- Đặt breakpoint theo sơ đồ L3 (của mình) và L2 (của TV1) khi app chạy dạng web, đọc Call Stack
- Trả lời câu hỏi chéo phần TV1: State, Factory Method, cách tính hạn SLA, rollback L2
- Nêu ≥2 nhược điểm của Strategy và Observer
- Demo phần mình trong video; chương báo cáo phân hệ TV2

**Định mức riêng của TV2 (checklist đòi / kế hoạch):** nhánh BFD ≥3 / 4 · UC ≥4 / 5 · Form (trang) ≥4 / 7 · Master-Detail ≥2 / 4 · báo cáo ≥2 / 3 · quy tắc ≥4 / 7 · mẫu GoF ≥1 / Strategy + observer cầu nối · unit test ≥9 / 10 · sơ đồ hành vi ≥2 / 2 · test dùng Fake của bạn ≥2 / 2.

### 6.6 Việc cả hai cùng làm
- Merge PR #1 vào `develop` (22/11)
- Chốt chữ ký interface ở mục 4.5 (27/11)
- Điền tự đánh giá L2 (29/11)
- Review PR của nhau trong 24 giờ; mỗi người ≥2 PR được merge trên GitHub
- Cập nhật `docs/PROGRESS.md` trong PR của mình sau mỗi module
- Đo tỷ lệ `git blame` ở các mốc 28/11, 4/12, 6/12, 9/12, 13/12 (mục 3.1), rà tiến độ
- Rà bảng 5.10, đồng bộ giao diện hai phân hệ (12/12)
- Quay video 3–5 phút trên trình duyệt; tập vấn đáp chéo (P5)
- Ghép báo cáo cuối, nộp (30/12)

### 6.7 Phụ thuộc giữa hai người
| Cần gì | Ai làm | Cho ai | Hạn | Nếu chưa có |
|---|---|---|---|---|
| Chữ ký interface (mục 4.5) | cả hai | cả hai | 27/11 | — |
| Khung web (project `HotelCRM.GUI`, `_Layout`, menu, khung `Program.cs`) | TV2 | TV1 | 28/11 (PR #10) | TV1 làm tài liệu P1 trước |
| Đăng nhập cookie, `ErrorHandler` | TV1 | TV2 | 2/12 | TV2 làm Domain, service + Fake, phần dùng chung trước |
| `UnitOfWork` + repository mẫu + `schema.sql` chạy được | TV1 | TV2 | 4/12 | TV2 làm Domain + unit test trước |
| `DatabaseInitializer` (tạo CSDL từ `schema.sql` + nạp `seed.sql`) | TV1 | TV2 (hạ tầng integration test) | 4/12 | TV2 viết unit test dùng Fake trước |
| Tài khoản 4 vai trò trong seed | TV1 | cả hai | 4/12 | — |
| Thành phần giao diện dùng chung + `IReportExporter` + hàm định dạng/biểu đồ | TV2 | TV1 (frm101–frm106) | 7/12 | TV1 làm trang bằng class Bootstrap có sẵn trước, thay bằng thành phần chung sau |
| `CustomerDirectory`, `StayLookup` thật | TV2 | TV1 | 8/12 | TV1 dùng Fake |
| `TicketQuery`, `SurveyScheduler`, `CustomerCareStats` thật | TV1 | TV2 | 8/12 | TV2 dùng Fake |
| 2 observer thật | TV2 | TV1 (integration L2) | 9/12 | — |

## 7. Cách làm việc chung
- **Git:** nhánh tích hợp `v2`; nhánh việc `tv1/<việc>`, `tv2/<việc>`; mở PR vào `v2`, người kia đọc, chạy thử, comment rồi mới merge (trong 24 giờ). Mỗi module một commit, message tiếng Anh, không `Co-Authored-By`. Không force-push `v2`. Cuối cùng merge `v2` → `main`. Giữ `git blame` theo luật ở mục 3.1.
- **Tài liệu:** tiếng Việt; mỗi mục đánh dấu [TV1]/[TV2]; mã F, UC, BM, Q, R, TC dùng thống nhất. Sơ đồ chung (ERD, lớp) tách thành `-tv1.iuml`, `-tv2.iuml` rồi `!include` vào một file `.puml` để ra một sơ đồ tổng mà mỗi người sửa phần mình. Xuất ảnh: `java -jar tools/plantuml.jar -tpng docs/analysis/diagrams/*.puml`.
- **Code:** tên lớp/hàm/comment tiếng Anh; chữ trên trang, thông báo, dữ liệu mẫu tiếng Việt; tiền `#,##0` căn phải; ngày `dd/MM/yyyy`; tên trang và quy ước giao diện theo mục 5.1; mọi handler bọc `ErrorHandler.Run`; kiểm tra ở trang (báo lỗi cạnh ô) và ở Domain/Service.
- **Xong một module khi:** build 0 lỗi 0 warning; test xanh; `docs/PROGRESS.md` cập nhật; PR đã review và merge; trang đã bấm thử trên trình duyệt.
- **Các mốc đo `git blame`** (28/11, 4/12, 6/12, 9/12, 13/12): đo tỷ lệ, rà tiến độ, chỉnh phân công nếu lệch theo mục 3.1.
- **Dùng AI:** được dùng để hỏi, nháp, review; code ai commit thì người đó phải giải thích được và tự viết lại được khi tắt AI. README và báo cáo ghi rõ việc dùng Claude.

## 8. Kiểm tra
- **P1:** mọi `.puml` xuất ảnh không lỗi; bảng 5.10 đạt hết; mỗi nút lá BFD có Form, Service, UC tương ứng; TV2 review PR tài liệu; slide biên dịch được.
- **P2–P4 (mỗi module):** `dotnet build HotelCRM.sln` 0 lỗi 0 warning; `dotnet test` xanh 100%; unit test chạy không cần CSDL; integration test chạy trên CSDL tạm; người làm tự bấm thử các trang của mình trên trình duyệt.
- **P4:** publish self-contained, chép sang thư mục khác không có `src/`, xóa `hotel.db`, chạy file thực thi rồi mở `http://localhost:5080` theo README trong ≤3 phút; đăng nhập đủ 4 vai trò, làm 1 giao dịch Master-Detail, mở báo cáo, xuất Excel mở được tiếng Việt; tiêm lỗi (đổi `>=`, bỏ rollback, bỏ kiểm quyền) thì có test chuyển đỏ.

## 9. Rủi ro
| Rủi ro | Cách xử lý |
|---|---|
| CRM không có trạng thái "công nợ" mà checklist nêu làm ví dụ | seed phủ đủ trạng thái tương đương (mục 5.9); chuẩn bị câu trả lời |
| Admin làm được mọi thứ | giải thích là quyết định có chủ đích, có nhật ký; test RBAC dựa vào 3 vai trò còn lại |
| Tiêu chí 2.4 "Cây Solution chuẩn Thầy Thăng" viết cho WinForms | đã chốt 27/11 (mục 3.1): tên trang `frm…_dm_/_nv_/_bc_`, giữ `Database/`, `Resource/`; bảng đối chiếu quy ước trong `07-thiet-ke-lop.md` |
| Web không chạy được trên máy chấm (cổng sản phẩm, trần 3.5) | publish self-contained, HTTP chỉ nghe localhost, cổng trong `appsettings.json`, README 3 bước; thử máy sạch 4/12 và lại ở P4 |
| Học ASP.NET Core trong 12 ngày code | Razor Pages (ít khái niệm); TV2 đưa khung web lên 28/11; TV1 đưa đăng nhập và một trang mẫu dùng SQLite lên trước 2/12 |
| Lỗi bảo mật web (CSRF, XSS, sắp xếp ghép chuỗi SQL) | mục 3.1 kỹ thuật 10, 11; service luôn kiểm quyền |
| SQLite bị khoá khi nhiều request | WAL, `busy_timeout`, mỗi request một `UnitOfWork` |
| TV1 làm trên Linux | sau khi xoá WinForms mọi project là `net10.0`; đường dẫn dùng `Path.Combine`; `.gitattributes`; publish cả `win-x64` và `linux-x64`; thử xuất Excel trên Linux (ClosedXML cần font) |
| TeX chỉ có ở máy TV2 | TV2 biên dịch slide |
| Tỷ lệ code lệch (TV1 có sẵn ~5.800 dòng) | phần mới và phần dùng chung nghiêng về TV2; đo theo các mốc ở mục 3.1 |
| P1 trễ: M1 xong 25/11 (hạn 22/11); phần TV1 của M2–M8 chưa viết khi chuyển sang web | chốt 27/11: giữ hạn slide 30/11, TV1 làm dồn M2–M8 trong 27–29/11 |
| TV2 đã code Domain P2 (PR #2 `tv2/domain`) trước khi xong M5–M6 | sơ đồ lớp/ERD ở M5–M6 đối chiếu với code PR #2; chỗ lệch xử lý trong PR, theo thiết kế được cả hai duyệt |
| Lịch code chỉ 12 ngày | nếu trễ, cắt theo thứ tự mà vẫn giữ định mức: bớt tiện ích phụ trên trang → bỏ Master-Detail của frm202 → bỏ biểu đồ R3 (còn R2) → gộp frm204 vào frm205. Không cắt: CSDL, rollback, kiểm quyền ở Service, 5 báo cáo, 28 test |

## 10. Việc chờ người
Danh sách việc chờ người (ai, hạn, trạng thái) theo dõi ở mục "Việc chờ người" của `docs/PROGRESS.md`.
