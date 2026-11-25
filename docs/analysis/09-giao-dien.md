# 09. Thiết kế giao diện

Module M8 của P1. [TV1] viết Form F1–F4 (frm000, frm001, frm101–frm106); [TV2] viết Form F5–F8 (frm201–frm207) và quy ước giao diện dùng chung (TV2 phụ trách hàm định dạng, `ErrorProvider`, hàm vẽ biểu đồ dùng chung).

## 1. Quy ước giao diện dùng chung [TV2]
- **Chữ trên giao diện:** tiêu đề Form, nhãn, tiêu đề cột lưới, thông báo đều bằng tiếng Việt.
- **Tên control:** tiếng Anh, có tiền tố `txt`, `btn`, `cbo`, `dgv`, `lbl`, `dtp`, `num`, `chk`, `tab`, `err` (ErrorProvider).
- **Định dạng:**
  - Tiền VND: `#,##0`, căn phải, không có chữ "đ" trong ô (tiêu đề cột ghi "(đ)").
  - Ngày: `dd/MM/yyyy`. Thời điểm: `dd/MM/yyyy HH:mm`.
  - Số: căn phải. Tỷ lệ: `0.0%`.
  - Một hàm dùng chung (`GridFormat`) đặt định dạng và căn lề cho cột theo kiểu dữ liệu, để mọi Form giống nhau.
- **Kiểm tra dữ liệu hai lớp:**
  - Form dùng `ErrorProvider` báo ngay ô trống hoặc sai dạng (biểu tượng cạnh ô, nút Lưu vẫn bấm được).
  - Domain/Service kiểm tra lại và là nơi quyết định. Lỗi từ Domain hiện bằng hộp thoại qua `ErrorHandler`.
- **Xử lý lỗi:** mọi handler bọc `ErrorHandler.Run`; Form không `try/catch`, không có SQL, không có quy tắc nghiệp vụ.
- **Phân quyền trên Form:** nút không được phép thì ẩn hoặc tắt theo vai trò. Đây chỉ là lớp chặn thứ hai; service vẫn kiểm quyền (Q6).
- **Lọc:** mỗi Form có lọc nhiều điều kiện thì có nút "Xóa lọc" đưa mọi ô lọc về mặc định và nạp lại.
- **Master-Detail:** chọn dòng ở lưới Master thì lưới Detail tự nạp lại; các tổng ở phần Detail tính lại ngay, không lưu CSDL.

## 2. Form F1–F4 [TV1]
> TV1 viết frm000, frm001, frm101–frm106.

## 3. Form F5–F8 [TV2]

### 3.1 Tổng hợp

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

### 3.2 Quyền theo vai trò

| Form | Admin | Quản lý | Lễ tân | CSKH |
|---|---|---|---|---|
| frm201 | sửa, xóa | sửa, xóa | sửa | chỉ xem |
| frm202 | sửa | xem | xem | xem |
| frm203 | thao tác | thao tác | thao tác | không mở |
| frm204 | sửa | xem | xem | xem |
| frm205 | đổi, hủy | đổi, hủy | đổi, hủy | không mở |
| frm206 | tạo, ghi phản hồi | tạo, ghi phản hồi | ghi phản hồi | ghi phản hồi |
| frm207 | xem, xuất | xem, xuất | không mở | không mở |

### 3.3 frm201_dm_khach_hang – Hồ sơ khách 360 (Master-Detail)
- **Lọc:** `txtKeyword` (tên, SĐT, email, giấy tờ), `cboTier` (Tất cả + 4 hạng), `cboNationality`, `chkConsentOnly` (chỉ khách đồng ý ưu đãi), `btnFilter`, `btnClearFilter`.
- **Master `dgvCustomers`:** Mã KH · Họ tên · SĐT · Email · Quốc tịch · Hạng · Đồng ý ưu đãi (ô tích).
- **Nhập (BM4):** `txtFullName`, `txtPhone`, `txtEmail`, `txtIdentityNumber`, `cboNationality`, `dtpDateOfBirth` (có ô "không nhập"), `chkMarketingConsent`, `txtNotes`; `btnNew`, `btnSave`, `btnDelete`.
  - `ErrorProvider` báo các ô bắt buộc còn trống; SĐT không đủ 10 số; CCCD không đủ 12 số khi quốc tịch là Việt Nam.
- **Detail (theo khách đang chọn):**
  - `tabStays` → `dgvStays`: Mã ĐP · Phòng · Ngày nhận · Ngày trả · Số đêm · Giá/đêm (đ) · Thành tiền (đ) · Trạng thái.
  - `tabInteractions` → `dgvInteractions`: Thời điểm · Kênh · Nội dung · Nhân viên · Ticket.
  - `tabPoints` → `dgvPoints`: Ngày · Lý do · Điểm (+/−) · Liên kết (mã booking/ticket/lượt đổi).
- **Tổng tính động:** `lblStayCount` (số lần đã trả phòng), `lblTotalSpend` (tổng thành tiền các booking Đã trả phòng), `lblLifetimePoints` (điểm đã tích), `lblBalance` (số dư).

### 3.4 frm202_dm_phong – Loại phòng và phòng (Master-Detail)
- **Master `dgvRoomTypes`:** Tên loại · Giá/đêm (đ) · Sức chứa · Số phòng (tính). Nhập: `txtRoomTypeName`, `numPricePerNight`, `numCapacity`; `btnSaveRoomType`.
- **Detail `dgvRooms`:** Số phòng · Tầng. Nhập: `txtRoomNumber`, `numFloor`; `btnSaveRoom`.
- **Ghi chú:** đổi giá/đêm chỉ áp dụng cho booking đặt sau đó; Form hiện dòng nhắc cạnh ô giá.

### 3.5 frm203_nv_luu_tru – Đặt phòng, nhận/trả phòng
- **Lọc:** `dtpFrom`, `dtpTo` (khoảng ngày lưu trú giao nhau), `cboStatus`, `txtCustomerKeyword`, `btnFilter`, `btnClearFilter`.
- **Lưới `dgvBookings`:** Mã ĐP · Khách · Phòng · Loại phòng · Ngày nhận · Ngày trả · Số đêm · Giá/đêm (đ) · Thành tiền (đ) · Trạng thái.
- **Đặt phòng (BM5):** `cboCustomer`, `dtpCheckIn`, `dtpCheckOut`, `btnFindRooms`. Sau đó `cboRoom` hiện "305 – Deluxe – 1.200.000" chỉ gồm phòng trống; `btnBook`.
- **Thao tác:** `btnCheckIn`, `btnCheckOut`, `btnCancel`, bật/tắt theo trạng thái của dòng đang chọn (bảng 3.1 của `08-hanh-vi.md`).
- Trả phòng xong Form báo "Đã trả phòng, cộng N điểm", kèm "Khách lên hạng …" nếu có.

### 3.6 frm204_dm_qua_tang – Danh mục quà
- **Lưới `dgvRewards`:** Tên quà · Điểm quy đổi · Hạng tối thiểu · Số lượng còn · Còn áp dụng.
- **Nhập:** `txtRewardName`, `numPointsCost`, `cboMinTier`, `numQuantity`, `chkIsActive`; `btnNew`, `btnSave`.

### 3.7 frm205_nv_doi_qua – Sổ điểm và đổi quà (Master-Detail)
- **Master:** `txtCustomerKeyword` + `cboCustomer`; `lblTier`, `lblLifetimePoints`, `lblBalance`.
- **Detail `dgvLedger`:** Ngày · Lý do · Điểm (+/−) · Số dư sau giao dịch (lũy kế, tính trên Form). Số dư hiện tại = dòng cuối.
- **Quà `dgvRewards`:** Tên · Điểm quy đổi · Hạng tối thiểu · Còn · Đổi được? (Có/Thiếu điểm/Chưa đủ hạng/Hết quà). Chọn quà thì `lblBalanceAfter` hiện số dư sau khi đổi; `btnRedeem`.
- **Lượt đổi `dgvRedemptions`:** Mã · Ngày · Quà · Điểm đã đổi · Trạng thái; `btnCancelRedemption`.

### 3.8 frm206_nv_chien_dich – Chiến dịch chăm sóc (Master-Detail)
- **Master `dgvCampaigns`:** Tên · Phân khúc · Từ ngày · Đến ngày · Số khách · Tỷ lệ phản hồi · Trạng thái (Đang chạy/Đã kết thúc).
- **Tạo (BM7 phần đầu, Quản lý/Admin):** `txtCampaignName`, `txtOffer`, `dtpStartDate`, `dtpEndDate`, `cboSegment` (nạp từ các Strategy đã đăng ký), `cboChannel`; `btnPreview`.
  - Sau khi Xem trước, `lblPreview` hiện "N khách · loại X không đồng ý · loại Y đang khiếu nại"; `btnSave`.
- **Detail `dgvTargets`:** Khách · SĐT · Kênh · Ngày gửi · Phản hồi · Ngày phản hồi · Người ghi.
- **Ghi phản hồi:** `cboResponse` + `btnRecordResponse`, tắt khi chiến dịch đã kết thúc.
- **Tổng tính động:** `lblTargetCount`, `lblResponseCount`, `lblResponseRate`.

### 3.9 frm207_bc_khach_hang – Báo cáo khách hàng và dashboard 360
Ba tab. Mỗi tab có lưới, dòng tổng và `btnExport` (`SaveFileDialog` → `IReportExporter`). File Excel có tiêu đề, ngày xuất, dòng tổng, định dạng như trên lưới.

| Tab | Báo cáo | Lọc | Cột | Biểu đồ |
|---|---|---|---|---|
| `tabTiers` | R3 Phân khúc khách theo hạng | khoảng ngày trả phòng | Hạng · Số khách · Tổng chi tiêu (đ) · Điểm đã tích · Điểm đã đổi | cột: số khách và tổng chi tiêu theo hạng (ScottPlot) |
| `tabCampaigns` | R4 Hiệu quả chiến dịch | khoảng ngày bắt đầu | Chiến dịch · Phân khúc · Số khách · Số phản hồi · Quan tâm · Đặt phòng · Tỷ lệ phản hồi · Tỷ lệ đặt phòng | – |
| `tabDashboard` | R5 Dashboard 360 | từ khóa, hạng | Mã KH · Họ tên · Hạng · Số lần ở · Tổng chi tiêu (đ) · Số ticket · Số khiếu nại · Điểm hài lòng TB | – |

Mọi báo cáo bỏ qua booking Đã hủy; chi tiêu chỉ tính booking Đã trả phòng, theo giá/đêm đã chốt. Định nghĩa đầy đủ và bảng tính tay R5 ở `10-kiem-thu.md`.
