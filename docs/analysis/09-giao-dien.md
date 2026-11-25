# 09. Thiết kế giao diện

Module M8 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.7). Tên Form theo PLAN 5.1 (tên tạm, rà theo HuongdanXDHTTTQL ở mục 5).

## 1. Quy ước giao diện chung [Chung – TV2]
Để hai phân hệ trông như một tác giả. TV2 viết các hàm dùng chung tương ứng ở P3 (hàm định dạng tiền/ngày, căn phải, `ErrorProvider`, hàm vẽ biểu đồ; hạn 7/12 để TV1 dùng).

- **Chữ trên giao diện:** tiêu đề Form, nhãn, tiêu đề cột lưới, thông báo đều bằng tiếng Việt.
- **Tên control:** tiếng Anh, có tiền tố `txt`, `btn`, `cbo`, `dgv`, `lbl`, `dtp`, `num`, `chk`, `tab`, `err` (ErrorProvider).
- **Màu:** giữ màu mặc định của Windows cho mọi Form, không tô màu riêng từng Form; lỗi nhập báo bằng biểu tượng `ErrorProvider`, không đổi màu ô.
- **Font và bố cục:** Segoe UI 9pt cho mọi Form; vùng lọc ở trên, lưới ở giữa (neo bốn cạnh), vùng nhập và nút ở dưới hoặc bên phải; nút chính (Lưu, Đặt phòng, Đổi quà…) đặt cuối hàng nút.
- **Định dạng:**
  - Tiền VND: `#,##0`, căn phải, không có chữ "đ" trong ô (tiêu đề cột ghi "(đ)").
  - Ngày: `dd/MM/yyyy`. Thời điểm: `dd/MM/yyyy HH:mm`.
  - Số: căn phải. Tỷ lệ: `0.0%`.
  - Một hàm dùng chung (`GridFormat`) đặt định dạng và căn lề cho cột theo kiểu dữ liệu, để mọi Form giống nhau.
- **Kiểm tra dữ liệu hai lớp:**
  - Form dùng `ErrorProvider` báo ngay ô trống hoặc sai dạng (biểu tượng cạnh ô, nút Lưu vẫn bấm được).
  - Domain/Service kiểm tra lại và là nơi quyết định. Lỗi từ Domain hiện bằng hộp thoại qua `ErrorHandler`.
- **Xử lý lỗi:** mọi handler bọc `ErrorHandler.Run`; Form không `try/catch`, không có SQL, không có quy tắc nghiệp vụ.
- **Nút chung:** Thêm mới (`btnNew`), Lưu (`btnSave`), Xóa (`btnDelete`, hỏi xác nhận), Lọc (`btnFilter`), Xóa lọc (`btnClearFilter`), Xuất Excel (`btnExport`).
- **Phân quyền trên Form:** nút không được phép thì ẩn hoặc tắt theo vai trò. Đây chỉ là lớp chặn thứ hai; service vẫn kiểm quyền (Q6).
- **Lọc:** mỗi Form có lọc nhiều điều kiện thì có nút "Xóa lọc" đưa mọi ô lọc về mặc định và nạp lại.
- **Master-Detail:** chọn dòng ở lưới Master thì lưới Detail tự nạp lại; các tổng ở phần Detail tính lại ngay, không lưu CSDL.

## 2. Đăng nhập và màn hình chính (frm000, frm001) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 3. Form phân hệ CSKH (frm101–frm106) [TV1]
_[TV1] chưa điền._

## 4. Form phân hệ quan hệ khách hàng (frm201–frm207) [TV2]

### 4.1 Tổng hợp

| Form | Nút BFD | UC | Biểu mẫu | Loại | Master → Detail | Service |
|---|---|---|---|---|---|---|
| frm201_dm_khach_hang | F5.1, F5.2 | UC07 | BM4 | danh mục | khách → lưu trú, tương tác, giao dịch điểm | `CustomerService` |
| frm202_dm_phong | F6.1 | UC08 | – | danh mục | loại phòng → phòng | `StayService` |
| frm203_nv_luu_tru | F6.2 | UC08 | BM5 | nghiệp vụ | – | `StayService` |
| frm204_dm_qua_tang | F7.1 | UC09 | – | danh mục | – | `LoyaltyService` |
| frm205_nv_doi_qua | F7.2 | UC09 | BM6 | nghiệp vụ | khách → giao dịch điểm, lượt đổi | `LoyaltyService` |
| frm206_nv_chien_dich | F8.1 | UC10 | BM7 | nghiệp vụ | chiến dịch → khách nhận | `CampaignService` |
| frm207_bc_khach_hang | F8.2 | UC11 | – | báo cáo | – | `CustomerReportService`, `DashboardService` |

7 Form; 4 Form Master-Detail (frm201, frm202, frm205, frm206); 3 Form danh mục (frm201, frm202, frm204), trong đó chỉ frm204 là danh mục một bảng không có Detail.

### 4.2 Quyền theo vai trò

| Form | Admin | Quản lý | Lễ tân | CSKH |
|---|---|---|---|---|
| frm201 | sửa, xóa | sửa, xóa | sửa | chỉ xem |
| frm202 | sửa | xem | xem | xem |
| frm203 | thao tác | thao tác | thao tác | không mở |
| frm204 | sửa | xem | xem | xem |
| frm205 | đổi, hủy | đổi, hủy | đổi, hủy | không mở |
| frm206 | tạo, ghi phản hồi | tạo, ghi phản hồi | ghi phản hồi | ghi phản hồi |
| frm207 | xem, xuất | xem, xuất | không mở | không mở |

Ô "không mở": menu ở frm001 không hiện Form đó cho vai trò này. Nút bị ẩn/tắt theo bảng trên; service vẫn kiểm quyền (Q6).

### 4.3 frm201_dm_khach_hang – Hồ sơ khách 360 (Master-Detail)
- **Nút BFD / UC / loại:** F5.1, F5.2 / UC07 / danh mục, Master-Detail.
- **Bố cục:**
```
┌ Lọc: [txtKeyword] [cboTier] [cboNationality] [chkConsentOnly] (Lọc) (Xóa lọc) ┐
│ dgvCustomers (Master)                         │ Nhập (BM4): Họ tên, SĐT, Email, │
│                                               │ Giấy tờ, Quốc tịch, Ngày sinh,  │
│                                               │ Đồng ý ưu đãi, Ghi chú          │
│                                               │ (Thêm mới) (Lưu) (Xóa)          │
├ [tabStays | tabInteractions | tabPoints] (Detail) ───────────────────────────────┤
│ dgvStays / dgvInteractions / dgvPoints                                          │
└ Số lần ở: … · Tổng chi tiêu: … · Điểm đã tích: … · Số dư: …                     ┘
```
- **Lọc:** `txtKeyword` (tên, SĐT, email, giấy tờ), `cboTier` (Tất cả + 4 hạng), `cboNationality`, `chkConsentOnly` (chỉ khách đồng ý ưu đãi), `btnFilter`, `btnClearFilter`.
- **Master `dgvCustomers`:** Mã KH · Họ tên · SĐT · Email · Quốc tịch · Hạng · Đồng ý ưu đãi (ô tích). Mỗi cột ↔ một ô của BM4 (`01-khao-sat.md` mục 4.2).
- **Nhập (BM4):** `txtFullName`, `txtPhone`, `txtEmail`, `txtIdentityNumber`, `cboNationality`, `dtpDateOfBirth` (có ô "không nhập"), `chkMarketingConsent`, `txtNotes`; `btnNew`, `btnSave`, `btnDelete`.
- **Kiểm tra ở Form (`ErrorProvider`):** ô bắt buộc còn trống; SĐT không đủ 10 số; CCCD không đủ 12 số khi quốc tịch là Việt Nam.
- **Detail (theo khách đang chọn):**
  - `tabStays` → `dgvStays`: Mã ĐP · Phòng · Ngày nhận · Ngày trả · Số đêm · Giá/đêm (đ) · Thành tiền (đ) · Trạng thái.
  - `tabInteractions` → `dgvInteractions`: Thời điểm · Kênh · Nội dung · Nhân viên · Ticket.
  - `tabPoints` → `dgvPoints`: Ngày · Lý do · Điểm (+/−) · Liên kết (mã booking/ticket/lượt đổi).
- **Tổng tính động:** `lblStayCount` (số lần đã trả phòng), `lblTotalSpend` (tổng thành tiền các booking Đã trả phòng), `lblLifetimePoints` (điểm đã tích), `lblBalance` (số dư).
- **Nút ↔ vai trò:** `btnSave` (Admin, Quản lý, Lễ tân), `btnDelete` (Admin, Quản lý); CSKH chỉ xem.
- **Service/hàm:** `CustomerService.Search`, `Create`, `Update`, `Delete` (F5.1); `GetProfile` (F5.2).

### 4.4 frm202_dm_phong – Loại phòng và phòng (Master-Detail)
- **Nút BFD / UC / loại:** F6.1 / UC08 (mở rộng) / danh mục, Master-Detail.
- **Bố cục:**
```
┌ dgvRoomTypes (Master)            │ Tên loại, Giá/đêm, Sức chứa (Lưu loại) ┐
├ dgvRooms (Detail: phòng của loại) │ Số phòng, Tầng (Lưu phòng)            ┤
└ "Đổi giá chỉ áp dụng cho đặt phòng mới"                                  ┘
```
- **Master `dgvRoomTypes`:** Tên loại · Giá/đêm (đ) · Sức chứa · Số phòng (tính). Nhập: `txtRoomTypeName`, `numPricePerNight`, `numCapacity`; `btnSaveRoomType`.
- **Detail `dgvRooms`:** Số phòng · Tầng. Nhập: `txtRoomNumber`, `numFloor`; `btnSaveRoom`.
- **Kiểm tra ở Form:** tên trống; giá/đêm, sức chứa, tầng phải > 0.
- **Tổng tính động:** cột "Số phòng" của loại đang chọn.
- **Ghi chú:** đổi giá/đêm chỉ áp dụng cho booking đặt sau đó; Form hiện dòng nhắc cạnh ô giá.
- **Nút ↔ vai trò:** `btnSaveRoomType`, `btnSaveRoom` chỉ Admin; vai trò khác chỉ xem.
- **Service/hàm:** `StayService.GetRoomTypes`, `SaveRoomType`, `SaveRoom`.

### 4.5 frm203_nv_luu_tru – Đặt phòng, nhận/trả phòng
- **Nút BFD / UC / loại:** F6.2 / UC08 / nghiệp vụ.
- **Bố cục:**
```
┌ Lọc: [dtpFrom]–[dtpTo] [cboStatus] [txtCustomerKeyword] (Lọc) (Xóa lọc)     ┐
│ dgvBookings                                                                 │
├ Đặt phòng (BM5): [cboCustomer] [dtpCheckIn]–[dtpCheckOut] (Tìm phòng)       ┤
│                  [cboRoom: 305 – Deluxe – 1.200.000] (Đặt phòng)            │
└ (Nhận phòng) (Trả phòng) (Hủy)                                              ┘
```
- **Lọc:** `dtpFrom`, `dtpTo` (khoảng ngày lưu trú giao nhau), `cboStatus`, `txtCustomerKeyword`, `btnFilter`, `btnClearFilter`.
- **Lưới `dgvBookings`:** Mã ĐP · Khách · Phòng · Loại phòng · Ngày nhận · Ngày trả · Số đêm · Giá/đêm (đ) · Thành tiền (đ) · Trạng thái. Cột ↔ ô của BM5 (`01-khao-sat.md` mục 4.2); "Giá/đêm" là giá đã chốt.
- **Đặt phòng (BM5):** `cboCustomer`, `dtpCheckIn`, `dtpCheckOut`, `btnFindRooms`. Sau đó `cboRoom` hiện "305 – Deluxe – 1.200.000" chỉ gồm phòng trống; `btnBook`.
- **Kiểm tra ở Form:** chưa chọn khách hoặc phòng; ngày trả ≤ ngày nhận.
- **Thao tác:** `btnCheckIn`, `btnCheckOut`, `btnCancel`, bật/tắt theo trạng thái của dòng đang chọn (bảng 4.1 của `08-hanh-vi.md`).
- Trả phòng xong Form báo "Đã trả phòng, cộng N điểm", kèm "Khách lên hạng …" nếu có.
- **Nút ↔ vai trò:** mọi nút cho Admin, Quản lý, Lễ tân; CSKH không mở Form.
- **Service/hàm:** `StayService.Search`, `FindAvailableRooms`, `Book`, `CheckIn`, `CheckOut` (L3), `Cancel`.

### 4.6 frm204_dm_qua_tang – Danh mục quà
- **Nút BFD / UC / loại:** F7.1 / UC09 (mở rộng) / danh mục một bảng.
- **Bố cục:**
```
┌ dgvRewards                    │ Tên quà, Điểm quy đổi, Hạng tối thiểu, ┐
│                               │ Số lượng, Còn áp dụng                  │
└                               │ (Thêm mới) (Lưu)                       ┘
```
- **Lưới `dgvRewards`:** Tên quà · Điểm quy đổi · Hạng tối thiểu · Số lượng còn · Còn áp dụng.
- **Nhập:** `txtRewardName`, `numPointsCost`, `cboMinTier`, `numQuantity`, `chkIsActive`; `btnNew`, `btnSave`.
- **Kiểm tra ở Form:** tên trống; điểm quy đổi ≤ 0; số lượng < 0.
- **Nút ↔ vai trò:** `btnNew`, `btnSave` chỉ Admin.
- **Service/hàm:** `LoyaltyService.GetRewards`, `SaveReward`.

### 4.7 frm205_nv_doi_qua – Sổ điểm và đổi quà (Master-Detail)
- **Nút BFD / UC / loại:** F7.2 / UC09 / nghiệp vụ, Master-Detail.
- **Bố cục:**
```
┌ Khách: [txtCustomerKeyword][cboCustomer]  Hạng: … Điểm đã tích: … Số dư: … ┐
├ dgvLedger (Detail: sổ điểm)          │ dgvRewards (quà, đổi được?)         ┤
│                                      │ Số dư sau khi đổi: … (Đổi quà)      │
├ dgvRedemptions (lượt đổi của khách)                    (Hủy lượt đổi)      ┤
└─────────────────────────────────────────────────────────────────────────────┘
```
- **Master:** `txtCustomerKeyword` + `cboCustomer`; `lblTier`, `lblLifetimePoints`, `lblBalance`.
- **Detail `dgvLedger`:** Ngày · Lý do · Điểm (+/−) · Số dư sau giao dịch (lũy kế, tính trên Form). Số dư hiện tại = dòng cuối.
- **Quà `dgvRewards`:** Tên · Điểm quy đổi · Hạng tối thiểu · Còn · Đổi được? (Có/Thiếu điểm/Chưa đủ hạng/Hết quà). Chọn quà thì `lblBalanceAfter` hiện số dư sau khi đổi; `btnRedeem`.
- **Lượt đổi `dgvRedemptions`:** Mã · Ngày · Quà · Điểm đã đổi · Trạng thái; `btnCancelRedemption`. Cột ↔ ô của BM6 (`01-khao-sat.md` mục 4.2); "Điểm đã đổi" là điểm đã chốt.
- **Kiểm tra ở Form:** chưa chọn khách hoặc quà.
- **Tổng tính động:** `lblBalance`, `lblBalanceAfter`, cột số dư lũy kế.
- **Nút ↔ vai trò:** `btnRedeem`, `btnCancelRedemption` cho Admin, Quản lý, Lễ tân; CSKH không mở Form.
- **Service/hàm:** `LoyaltyService.GetLedger`, `Redeem`, `CancelRedemption`.

### 4.8 frm206_nv_chien_dich – Chiến dịch chăm sóc (Master-Detail)
- **Nút BFD / UC / loại:** F8.1 / UC10 / nghiệp vụ, Master-Detail.
- **Bố cục:**
```
┌ dgvCampaigns (Master)                │ Tạo (BM7): Tên, Ưu đãi, Từ–Đến ngày, ┐
│                                      │ [cboSegment] [cboChannel] (Xem trước)│
│                                      │ N khách · loại X · loại Y (Lưu)      │
├ dgvTargets (Detail: khách nhận)        [cboResponse] (Ghi phản hồi)       ┤
└ Số khách: … · Số phản hồi: … · Tỷ lệ phản hồi: …                          ┘
```
- **Master `dgvCampaigns`:** Tên · Phân khúc · Từ ngày · Đến ngày · Số khách · Tỷ lệ phản hồi · Trạng thái (Đang chạy/Đã kết thúc).
- **Tạo (BM7 phần đầu, Quản lý/Admin):** `txtCampaignName`, `txtOffer`, `dtpStartDate`, `dtpEndDate`, `cboSegment` (nạp từ các Strategy đã đăng ký), `cboChannel`; `btnPreview`.
  - Sau khi Xem trước, `lblPreview` hiện "N khách · loại X không đồng ý · loại Y đang khiếu nại"; `btnSave`.
- **Detail `dgvTargets`:** Khách · SĐT · Kênh · Ngày gửi · Phản hồi · Ngày phản hồi · Người ghi. Cột ↔ dòng của BM7.
- **Ghi phản hồi:** `cboResponse` + `btnRecordResponse`, tắt khi chiến dịch đã kết thúc.
- **Kiểm tra ở Form:** tên, ưu đãi trống; chưa chọn phân khúc; ngày kết thúc trước ngày bắt đầu; chưa Xem trước mà bấm Lưu.
- **Tổng tính động:** `lblTargetCount`, `lblResponseCount`, `lblResponseRate`.
- **Nút ↔ vai trò:** `btnPreview`, `btnSave` cho Admin, Quản lý; `btnRecordResponse` cho mọi vai trò.
- **Service/hàm:** `CampaignService.GetSegments`, `PreviewTargets` (L4), `Create`, `RecordResponse`.

### 4.9 frm207_bc_khach_hang – Báo cáo khách hàng và dashboard 360
- **Nút BFD / UC / loại:** F8.2 / UC11 / báo cáo.
- **Bố cục:** ba tab; mỗi tab có vùng lọc ở trên, lưới có dòng tổng ở giữa, `btnExport` ở dưới; tab R3 có biểu đồ cột bên phải lưới.
- Mỗi tab có `btnExport` (`SaveFileDialog` → `IReportExporter`). File Excel có tiêu đề, ngày xuất, dòng tổng, định dạng như trên lưới.

| Tab | Báo cáo | Lọc | Cột | Biểu đồ |
|---|---|---|---|---|
| `tabTiers` | R3 Phân khúc khách theo hạng | khoảng ngày trả phòng | Hạng · Số khách · Tổng chi tiêu (đ) · Điểm đã tích · Điểm đã đổi | cột: số khách và tổng chi tiêu theo hạng (ScottPlot) |
| `tabCampaigns` | R4 Hiệu quả chiến dịch | khoảng ngày bắt đầu | Chiến dịch · Phân khúc · Số khách · Số phản hồi · Quan tâm · Đặt phòng · Tỷ lệ phản hồi · Tỷ lệ đặt phòng | – |
| `tabDashboard` | R5 Dashboard 360 | từ khóa, hạng | Mã KH · Họ tên · Hạng · Số lần ở · Tổng chi tiêu (đ) · Số ticket · Số khiếu nại · Điểm hài lòng TB | – |

- **Kiểm tra ở Form:** ngày "đến" trước ngày "từ".
- **Nút ↔ vai trò:** Form và `btnExport` chỉ cho Admin, Quản lý.
- **Service/hàm:** `CustomerReportService.GetTierReport` (R3), `GetCampaignReport` (R4); `DashboardService.GetDashboard` (R5, L6); `IReportExporter.Export`.

Mọi báo cáo bỏ qua booking Đã hủy; chi tiêu chỉ tính booking Đã trả phòng, theo giá/đêm đã chốt. Định nghĩa đầy đủ và bảng tính tay R5 ở `10-kiem-thu.md` mục 5.

## 5. Đối chiếu tên Form với HuongdanXDHTTTQL [Chung – TV1]
_[Chung – TV1] chưa điền._
