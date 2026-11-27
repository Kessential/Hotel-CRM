# 09. Thiết kế giao diện

Module M8 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.7). Giao diện là web ASP.NET Core Razor Pages, chạy tại máy (chốt 27/11). Tên trang theo `docs/PLAN.md` mục 5.1 (đối chiếu HuongdanXDHTTTQL ở mục 5).

## 1. Quy ước giao diện chung [Chung – TV2]
Để hai phân hệ trông như một tác giả. TV2 viết thành phần dùng chung (hạn 7/12 để TV1 dùng): bảng, thanh lọc, khung modal, báo lỗi cạnh ô, nhãn trạng thái, ô số liệu, `site.css`, hàm định dạng tiền/ngày, hàm vẽ Chart.js. `_Layout` (thanh bên, thanh trên, toast), đăng nhập và `ErrorHandler` do TV1 viết.

- **Bố cục:** thanh bên trái nhóm theo phân hệ, thu gọn còn icon (nhớ bằng localStorage); thanh trên có tên trang, ô tìm khách, bộ phận và vai trò người dùng, số phiên bản, Đăng xuất. Chỉ giao diện sáng, chỉ máy tính (từ 1280px), mật độ vừa (cỡ mặc định của Bootstrap 5); hiệu ứng khoảng 150 ms, tắt khi máy đặt giảm chuyển động.
- **Màu và chữ:** tông Navy & đồng thau (thanh bên `#16213a`, màu chính `#1f3a68`, màu nhấn `#b08d57`, nền `#f2f4f8`) ghi đè biến `--bs-*` trong `site.css`. Cormorant Garamond cho tiêu đề trang, tên khách, trang đăng nhập; Be Vietnam Pro cho mọi chữ khác (mã như `KH001` dùng chữ không chân). Icon Bootstrap Icons. Thư viện và font là file tĩnh trong `wwwroot/lib`.
- **Chữ trên giao diện:** tiêu đề trang, nhãn, tiêu đề cột, thông báo đều bằng tiếng Việt; dùng "phiếu yêu cầu", "chương trình chăm sóc". Trang có `lang="vi"`.
- **Tên:** file `Pages/frm201_dm_customer.cshtml`, lớp `frm201_dm_customerModel`, đường dẫn `/frm201_dm_customer`, trang chi tiết thêm `?id=`. Ô nhập là thuộc tính của lớp InputModel (`Input.FullName`), không gắn thẳng vào Entity; handler tên `OnGet…`, `OnPost…`. Không dùng tiền tố control kiểu `txt`, `m_txt_`; bảng đối chiếu quy ước đặt tên ở `07-thiet-ke-lop.md` (TV1).
- **Bảng dùng chung:** phân trang và sắp xếp ở server (`?page=&sort=`, `LIMIT/OFFSET`; cột sắp xếp chỉ lấy từ danh sách cho phép, không ghép chuỗi SQL), chọn cột (JS + localStorage), tiêu đề đứng yên khi cuộn (`position: sticky`), tiêu đề cột có `aria-sort`. Trang có trang chi tiết thì cột đầu là liên kết tới đó. Bảng rỗng có câu hướng dẫn "Bấm Xóa lọc".
- **Thanh lọc:** nút "Lọc" và "Xóa lọc" (đưa mọi ô lọc về mặc định rồi nạp lại).
- **Thêm, sửa, xóa:** Bootstrap modal chứa form gửi về handler của trang; nút "Thực hiện" / "Hủy bỏ". Modal xóa hoặc hủy có tiêu đề dạng câu hỏi, nút "Thực hiện" màu đỏ. Server báo lỗi thì mở lại modal kèm lỗi. Modal giữ focus bên trong; đóng thì focus về nút đã mở.
- **Master-Detail:** trang danh sách → trang chi tiết `?id=` có tab (`nav-tabs`). Đầu trang chi tiết cùng một kiểu: link quay lại (nút thao tác bên phải), thẻ có tên, nhãn, thông tin. Ô số liệu tổng tính khi tải trang, không lưu CSDL.
- **Định dạng:**
  - Tiền VND: `#,##0`, căn phải, không có chữ "đ" trong ô (tiêu đề cột ghi "(đ)").
  - Ngày: `dd/MM/yyyy`. Thời điểm: `dd/MM/yyyy HH:mm`.
  - Số: căn phải. Tỷ lệ: `0.0%`.
  - Một bộ hàm định dạng dùng chung cho cả hai phân hệ.
- **Kiểm tra dữ liệu hai lớp:**
  - Trang báo lỗi cạnh ô (`is-invalid` + `invalid-feedback`, gắn `aria-describedby`); con trỏ nhảy tới ô lỗi đầu tiên.
  - Domain/Service kiểm tra lại và là nơi quyết định.
- **Thông báo và xử lý lỗi:** lưu thành công thì toast ở góc (đọc `TempData`); toast lỗi không tự tắt, có nút đóng, `role="alert"`. Mọi handler bọc `ErrorHandler.Run`; PageModel không `try/catch`, không SQL, không quy tắc nghiệp vụ. Lỗi không lường trước: trang lỗi chung, ghi `logs/error.log`, không lộ stack trace.
- **Phân quyền trên trang:** trang không được mở thì ẩn khỏi thanh bên; nút bị khóa theo vai trò hoặc quy tắc có một dòng nêu lý do dưới đầu trang, không chỉ dựa vào tooltip. Đây (cùng `[Authorize]`) chỉ là lớp chặn thứ hai; service vẫn kiểm quyền (Q6).
- **Bảo mật:** giữ antiforgery mặc định cho form POST; không dùng `Html.Raw` với dữ liệu người dùng nhập.
- **Tiếp cận:** tương phản đạt WCAG 2.1 AA (chữ ≥ 4,5:1; viền ô nhập và vòng focus ≥ 3:1); nhãn trạng thái đọc được bằng chữ, không chỉ bằng màu.
- **Biểu đồ và xuất file:** Chart.js (PageModel trả dữ liệu, trình duyệt vẽ); nút "Xuất Excel" tải file `.xlsx` về máy.

## 2. Đăng nhập và màn hình chính (frm000, frm001) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 3. Form phân hệ CSKH (frm101–frm106) [TV1]
_[TV1] chưa điền._

## 4. Trang phân hệ quan hệ khách hàng (frm201–frm207) [TV2]

### 4.1 Tổng hợp

| Trang | Nút BFD | UC | Biểu mẫu | Loại | Master → Detail | Service |
|---|---|---|---|---|---|---|
| `frm201_dm_customer` | F5.1, F5.2 | UC07 | BM4 | danh mục | khách → lưu trú, tương tác, giao dịch điểm, phiếu yêu cầu | `CustomerService` |
| `frm202_dm_room` | F6.1 | UC08 | – | danh mục | loại phòng → phòng | `StayService` |
| `frm203_nv_stay` | F6.2 | UC08 | BM5 | nghiệp vụ | – | `StayService` |
| `frm204_dm_reward` | F7.1 | UC09 | – | danh mục | – | `LoyaltyService` |
| `frm205_nv_loyalty` | F7.2 | UC09 | BM6 | nghiệp vụ | khách → giao dịch điểm, lượt đổi | `LoyaltyService` |
| `frm206_nv_campaign` | F8.1 | UC10 | BM7 | nghiệp vụ | chương trình → khách nhận | `CampaignService` |
| `frm207_bc_customer_360` | F8.2 | UC11 | – | báo cáo | – | `CustomerReportService`, `DashboardService` |

7 trang; 4 trang Master-Detail (frm201, frm202, frm205, frm206); 3 trang danh mục (frm201, frm202, frm204), trong đó chỉ frm204 là danh mục một bảng không có Detail.

### 4.2 Quyền theo vai trò

| Trang | Admin | Quản lý | Lễ tân | CSKH |
|---|---|---|---|---|
| frm201 | sửa, xóa | sửa, xóa | sửa | chỉ xem |
| frm202 | sửa | xem | xem | xem |
| frm203 | thao tác | thao tác | thao tác | không mở |
| frm204 | sửa | xem | xem | xem |
| frm205 | đổi, hủy | đổi, hủy | đổi, hủy | không mở |
| frm206 | tạo, ghi phản hồi | tạo, ghi phản hồi | ghi phản hồi | ghi phản hồi |
| frm207 | xem, xuất | xem, xuất | không mở | không mở |

Ô "không mở": thanh bên không hiện trang đó cho vai trò này; gõ thẳng đường dẫn thì service vẫn chặn (Q6). Nút không được phép thì khóa theo quy ước ở mục 1.

### 4.3 frm201_dm_customer – Hồ sơ khách 360 (Master-Detail)
- **Nút BFD / UC / loại:** F5.1, F5.2 / UC07 / danh mục, Master-Detail.
- **Đường dẫn:** `/frm201_dm_customer` (danh sách), `/frm201_dm_customer?id=…` (chi tiết).
- **Bố cục:**
```
Danh sách
┌ Lọc: [Từ khóa] [Hạng] [Quốc tịch] [☐ Đồng ý ưu đãi]            (Lọc) (Xóa lọc) ┐
│ (Thêm khách)                                                          (Cột ▾)   │
│ Bảng khách                                                                      │
└ ‹ 1 2 3 ›                                                                       ┘
Chi tiết ?id=
┌ ‹ Danh sách khách                                                  (Sửa) (Xóa) ┐
│ Thẻ khách: Họ tên · hạng · SĐT · email · giấy tờ · quốc tịch · ngày sinh        │
│ [Số lần ở] [Tổng chi tiêu (đ)] [Điểm đã tích] [Số dư] [Số phiếu yêu cầu]        │
│ [ Lưu trú | Tương tác | Sổ điểm | Phiếu yêu cầu ]                               │
└ Bảng của tab đang chọn                                                          ┘
```
- **Lọc:** từ khóa (tên, SĐT, email, giấy tờ), hạng (Tất cả + 4 hạng), quốc tịch, chỉ khách đồng ý ưu đãi.
- **Bảng khách:** Mã KH (liên kết) · Họ tên · SĐT · Email · Quốc tịch · Hạng · Đồng ý ưu đãi. Mỗi cột ↔ một ô của BM4 (`01-khao-sat.md` mục 4.2).
- **Modal thêm/sửa (BM4):** `Input.FullName`, `Input.Phone`, `Input.Email`, `Input.IdentityNumber`, `Input.Nationality`, `Input.DateOfBirth` (không bắt buộc), `Input.MarketingConsent`, `Input.Notes`. Modal xóa hỏi "Xóa khách …?".
- **Kiểm tra ở trang:** ô bắt buộc còn trống; SĐT không đủ 10 số; CCCD không đủ 12 số khi quốc tịch là Việt Nam.
- **Tab (trang chi tiết):**
  - Lưu trú: Mã ĐP · Phòng · Ngày nhận · Ngày trả · Số đêm · Giá/đêm (đ) · Thành tiền (đ) · Trạng thái.
  - Tương tác: Thời điểm · Kênh · Nội dung · Nhân viên · Phiếu yêu cầu.
  - Sổ điểm: Ngày · Lý do · Điểm (+/−) · Liên kết (mã booking, phiếu hoặc lượt đổi).
  - Phiếu yêu cầu (mới nhất trước): Mã phiếu · Ngày lập · Loại · Tiêu đề · Trạng thái.
- **Tổng tính động:** số lần ở (booking Đã trả phòng), tổng chi tiêu (thành tiền các booking Đã trả phòng), điểm đã tích, số dư, số phiếu yêu cầu (đếm từ danh sách phiếu).
- **Nút ↔ vai trò:** Thêm, Sửa: Admin, Quản lý, Lễ tân; Xóa: Admin, Quản lý; CSKH chỉ xem.
- **Handler → Service:** `OnGet` → `CustomerService.Search` (danh sách), `GetProfile` (chi tiết, F5.2; lấy phiếu qua `ITicketQuery.GetCustomerTickets`); `OnPostCreate`, `OnPostUpdate`, `OnPostDelete` → `Create`, `Update`, `Delete` (F5.1).
- **Còn chờ chốt:** (a) hàm `ITicketQuery.GetCustomerTickets` cho tab Phiếu yêu cầu đang chờ TV1 duyệt (`07-thiet-ke-lop.md` mục 6.2); (b) cột "Nhân viên" cần tên nhân viên, mà TV2 không đọc bảng `employees` và chưa interface nào trả tên.

### 4.4 frm202_dm_room – Loại phòng và phòng (Master-Detail)
- **Nút BFD / UC / loại:** F6.1 / UC08 (mở rộng) / danh mục, Master-Detail.
- **Đường dẫn:** `/frm202_dm_room` (loại phòng), `/frm202_dm_room?id=…` (phòng của một loại).
- **Bố cục:**
```
Danh sách:  (Thêm loại phòng)   Bảng loại phòng … mỗi dòng (Sửa)
Chi tiết:   ‹ Loại phòng        Thẻ loại phòng: tên · giá/đêm · sức chứa · số phòng
            (Thêm phòng)        Bảng phòng của loại … mỗi dòng (Sửa)
```
- **Bảng loại phòng:** Tên loại (liên kết) · Giá/đêm (đ) · Sức chứa · Số phòng (tính).
- **Bảng phòng:** Số phòng · Tầng.
- **Modal:** loại phòng: `Input.Name`, `Input.PricePerNight`, `Input.Capacity`; phòng: `Input.Number`, `Input.Floor`. Dưới ô giá có dòng nhắc "Đổi giá chỉ áp dụng cho đặt phòng mới".
- **Kiểm tra ở trang:** tên, số phòng trống; giá/đêm, sức chứa, tầng phải > 0.
- **Tổng tính động:** số phòng của loại.
- **Nút ↔ vai trò:** thêm, sửa chỉ Admin; vai trò khác chỉ xem.
- **Handler → Service:** `OnGet` → `StayService.GetRoomTypes`; `OnPostSaveRoomType` → `SaveRoomType`; `OnPostSaveRoom` → `SaveRoom`.

### 4.5 frm203_nv_stay – Đặt phòng, nhận/trả phòng
- **Nút BFD / UC / loại:** F6.2 / UC08 / nghiệp vụ.
- **Bố cục:**
```
┌ Lọc: [Từ ngày]–[Đến ngày] [Trạng thái] [Khách]                  (Lọc) (Xóa lọc) ┐
│ (Đặt phòng)                                                           (Cột ▾)   │
│ Bảng đặt phòng            … mỗi dòng (Nhận phòng) (Trả phòng) (Hủy)             │
└ ‹ 1 2 3 ›                                                                       ┘
Modal Đặt phòng: [Khách] [Ngày nhận] [Ngày trả] (Tìm phòng)
                 [Phòng trống: 305 – Deluxe – 1.250.000]     (Thực hiện) (Hủy bỏ)
```
- **Lọc:** từ ngày, đến ngày (khoảng ngày lưu trú giao nhau), trạng thái, từ khóa khách.
- **Bảng:** Mã ĐP · Khách · Phòng · Loại phòng · Ngày nhận · Ngày trả · Số đêm · Giá/đêm (đ) · Thành tiền (đ) · Trạng thái. Cột ↔ ô của BM5 (`01-khao-sat.md` mục 4.2); "Giá/đêm" là giá đã chốt.
- **Modal đặt phòng (BM5):** `Input.CustomerId` (tìm theo tên/SĐT), `Input.CheckInDate`, `Input.CheckOutDate`; "Tìm phòng" gửi về server, mở lại modal với ô `Input.RoomId` chỉ gồm phòng trống; "Thực hiện" đặt phòng.
- **Kiểm tra ở trang:** chưa chọn khách hoặc phòng; ngày trả ≤ ngày nhận.
- **Thao tác theo dòng:** nhận phòng, trả phòng, hủy (modal "Hủy đặt phòng …?"); nút khóa theo trạng thái của dòng (bảng 4.1 của `08-hanh-vi.md`).
- Trả phòng xong có toast "Đã trả phòng, cộng N điểm", kèm "Khách lên hạng …" nếu có.
- **Nút ↔ vai trò:** mọi nút cho Admin, Quản lý, Lễ tân; CSKH không mở trang.
- **Handler → Service:** `OnGet` → `StayService.Search`; `OnPostFindRooms` → `FindAvailableRooms`; `OnPostBook`, `OnPostCheckIn`, `OnPostCheckOut` (L3), `OnPostCancel` → `Book`, `CheckIn`, `CheckOut`, `Cancel`.

### 4.6 frm204_dm_reward – Danh mục quà
- **Nút BFD / UC / loại:** F7.1 / UC09 (mở rộng) / danh mục một bảng.
- **Bố cục:**
```
(Thêm quà)                                   (Cột ▾)
Bảng quà                         … mỗi dòng (Sửa)
```
- **Bảng:** Tên quà · Điểm quy đổi · Hạng tối thiểu · Số lượng còn · Còn áp dụng.
- **Modal thêm/sửa:** `Input.Name`, `Input.PointsCost`, `Input.MinTier`, `Input.Quantity`, `Input.IsActive`.
- **Kiểm tra ở trang:** tên trống; điểm quy đổi ≤ 0; số lượng < 0.
- **Nút ↔ vai trò:** thêm, sửa chỉ Admin.
- **Handler → Service:** `OnGet` → `LoyaltyService.GetRewards`; `OnPostSave` → `SaveReward`.

### 4.7 frm205_nv_loyalty – Sổ điểm và đổi quà (Master-Detail)
- **Nút BFD / UC / loại:** F7.2 / UC09 / nghiệp vụ, Master-Detail.
- **Đường dẫn:** `/frm205_nv_loyalty` (chọn khách), `/frm205_nv_loyalty?id=…` (sổ điểm của khách).
- **Bố cục:**
```
Danh sách:  Lọc [Từ khóa] (Lọc) (Xóa lọc)    Bảng khách
Chi tiết:   ‹ Chọn khách     Thẻ khách: tên · hạng     [Điểm đã tích] [Số dư]
            [ Sổ điểm | Đổi quà | Lượt đổi ]
            Bảng của tab đang chọn
Modal đổi quà: "Đổi quà …?  Số dư sau khi đổi: N"        (Thực hiện) (Hủy bỏ)
```
- **Bảng khách:** Mã KH (liên kết) · Họ tên · SĐT · Hạng.
- **Tab Sổ điểm:** Ngày · Lý do · Điểm (+/−) · Số dư sau giao dịch (lũy kế, tính trên toàn bộ sổ trước khi phân trang). Số dư hiện tại là số dư sau giao dịch mới nhất.
- **Tab Đổi quà:** Tên quà · Điểm quy đổi · Hạng tối thiểu · Còn · Đổi được? (Có / Thiếu điểm / Chưa đủ hạng / Hết quà) · nút "Đổi quà" kiểu viền, khóa kèm lý do.
- **Tab Lượt đổi:** Mã · Ngày · Quà · Điểm đã đổi · Trạng thái · (Hủy lượt đổi, modal xác nhận). Cột ↔ ô của BM6 (`01-khao-sat.md` mục 4.2); "Điểm đã đổi" là điểm đã chốt.
- **Kiểm tra ở trang:** yêu cầu đổi quà thiếu mã khách hoặc mã quà.
- **Tổng tính động:** số dư, số dư sau khi đổi, cột số dư lũy kế.
- **Nút ↔ vai trò:** đổi, hủy cho Admin, Quản lý, Lễ tân; CSKH không mở trang.
- **Handler → Service:** `OnGet` → `CustomerService.Search` (danh sách), `LoyaltyService.GetLedger`, `GetRewards` (chi tiết); `OnPostRedeem` → `Redeem`; `OnPostCancelRedemption` → `CancelRedemption`.

### 4.8 frm206_nv_campaign – Chương trình chăm sóc (Master-Detail)
- **Nút BFD / UC / loại:** F8.1 / UC10 / nghiệp vụ, Master-Detail.
- **Đường dẫn:** `/frm206_nv_campaign` (danh sách), `/frm206_nv_campaign?id=…` (khách nhận).
- **Bố cục:**
```
Danh sách:  (Tạo chương trình)               Bảng chương trình
Chi tiết:   ‹ Chương trình    Thẻ: tên · ưu đãi · phân khúc · từ–đến · trạng thái
            [Số khách] [Số phản hồi] [Tỷ lệ phản hồi]
            Bảng khách nhận                  … mỗi dòng (Ghi phản hồi)
Modal Tạo:  Tên, Ưu đãi, Từ ngày, Đến ngày, [Phân khúc] [Kênh]  (Xem trước)
            N khách · loại X · loại Y                (Thực hiện) (Hủy bỏ)
```
- **Bảng chương trình:** Tên (liên kết) · Phân khúc · Từ ngày · Đến ngày · Số khách · Tỷ lệ phản hồi · Trạng thái (Đang chạy / Đã kết thúc).
- **Modal tạo (BM7 phần đầu):** `Input.Name`, `Input.Offer`, `Input.StartDate`, `Input.EndDate`, `Input.SegmentCode` (nạp từ các Strategy đã đăng ký), `Input.Channel`. "Xem trước" mở lại modal kèm "N khách · loại X không đồng ý · loại Y đang khiếu nại"; nút "Thực hiện" chỉ bật sau khi đã xem trước.
- **Bảng khách nhận:** Khách · SĐT · Kênh · Ngày gửi · Phản hồi · Ngày phản hồi · Người ghi. Cột ↔ dòng của BM7.
- **Ghi phản hồi:** modal chọn `Input.Response`; nút khóa khi chương trình đã kết thúc, kèm lý do.
- **Kiểm tra ở trang:** tên, ưu đãi trống; chưa chọn phân khúc; ngày kết thúc trước ngày bắt đầu.
- **Tổng tính động:** số khách, số phản hồi, tỷ lệ phản hồi.
- **Nút ↔ vai trò:** xem trước, tạo cho Admin, Quản lý; ghi phản hồi cho mọi vai trò.
- **Handler → Service:** `CampaignService.GetSegments`; `OnPostPreview` → `PreviewTargets` (L4); `OnPostCreate` → `Create`; `OnPostRecordResponse` → `RecordResponse`.
- **Còn chờ chốt:** cột "Người ghi" cần tên nhân viên (xem mục 4.3).

### 4.9 frm207_bc_customer_360 – Báo cáo khách hàng và dashboard 360
- **Nút BFD / UC / loại:** F8.2 / UC11 / báo cáo.
- **Bố cục:** ba tab; mỗi tab có thanh lọc ở trên, bảng dùng chung có dòng tổng ở giữa, nút "Xuất Excel"; tab R3 có biểu đồ cột bên phải bảng.
- Nút "Xuất Excel" gọi `IReportExporter` rồi trả file cho trình duyệt tải về. File có tiêu đề, ngày xuất, dòng tổng, định dạng như trên bảng. Dòng tổng tính trên toàn bộ kết quả lọc, không chỉ trang đang xem.

| Tab | Báo cáo | Lọc | Cột | Biểu đồ |
|---|---|---|---|---|
| Theo hạng | R3 Phân khúc khách theo hạng | khoảng ngày trả phòng | Hạng · Số khách · Tổng chi tiêu (đ) · Điểm đã tích · Điểm đã đổi | cột: số khách và tổng chi tiêu theo hạng (Chart.js) |
| Chương trình | R4 Hiệu quả chương trình chăm sóc | khoảng ngày bắt đầu | Chương trình · Phân khúc · Số khách · Số phản hồi · Quan tâm · Đặt phòng · Tỷ lệ phản hồi · Tỷ lệ đặt phòng | – |
| Khách hàng 360 | R5 Dashboard 360 | từ khóa, hạng | Mã KH · Họ tên · Hạng · Số lần ở · Tổng chi tiêu (đ) · Số phiếu yêu cầu · Số khiếu nại · Điểm hài lòng TB | – |

- **Kiểm tra ở trang:** ngày "đến" trước ngày "từ".
- **Nút ↔ vai trò:** trang và xuất Excel chỉ cho Admin, Quản lý.
- **Handler → Service:** `OnGet` → `CustomerReportService.GetTierReport` (R3), `GetCampaignReport` (R4), `DashboardService.GetDashboard` (R5, L6); `OnGetExport` → `IReportExporter.Export`.
- **Còn chờ chốt:** chữ ký ở `07-thiet-ke-lop.md` mục 6.2 là `Export(report, filePath)`; bản web cần chọn ghi file tạm rồi trả về, hoặc đổi tham số thành `Stream`.

Mọi báo cáo bỏ qua booking Đã hủy; chi tiêu chỉ tính booking Đã trả phòng, theo giá/đêm đã chốt. Định nghĩa đầy đủ và bảng tính tay R5 ở `10-kiem-thu.md` mục 5.

## 5. Đối chiếu tên Form với HuongdanXDHTTTQL [Chung – TV1]
_[Chung – TV1] chưa điền._
