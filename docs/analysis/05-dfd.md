# 05. Biểu đồ luồng dữ liệu (DFD)

Module M4 của P1. [TV1] viết DFD ngữ cảnh, mức 0 và mức 1 của F3; [TV2] viết mức 1 của F7.

## 1. Quy ước
- Ký hiệu: tác nhân ngoài là hình chữ nhật; tiến trình là hình tròn/elip có số hiệu theo BFD; kho dữ liệu là `Dn Tên kho`; luồng dữ liệu là mũi tên có nhãn danh từ.
- Kho dữ liệu đánh số chung cho cả hệ thống và khớp với bảng CSDL (`06-erd.md`). TV1 giữ danh sách kho ở mục 2; TV2 dùng D1–D4 dưới đây, TV1 đánh lại số khi ghép nếu trùng.
- Sơ đồ: PlantUML trong `diagrams/`, hoặc draw.io xuất PNG đặt cùng tên.

## 2. DFD ngữ cảnh và mức 0 [TV1]
> TV1 viết. Ở mức 0, F7 nhận "yêu cầu đổi quà" từ Khách hàng, "thông tin quà" từ Quản trị viên, và dùng chung kho Khách hàng, Sổ điểm với F5, F6.

## 3. DFD mức 1 của F3 [TV1]
> TV1 viết mức 1 của F3.

## 4. DFD mức 1 của F7 – Quản lý khách hàng thân thiết [TV2]
Sơ đồ: `diagrams/dfd-f7.puml`.

### 4.1 Thành phần

| Loại | Tên | Ghi chú |
|---|---|---|
| Tác nhân ngoài | Khách hàng | yêu cầu xem điểm, đổi quà, hủy đổi; nhận phiếu đổi quà (giao tiếp qua lễ tân) |
| Tác nhân ngoài | Quản trị viên | cung cấp thông tin quà |
| Tiến trình ngoài F7 | F6.2 Trả phòng (TV2), F3.2 Xử lý ticket (TV1) | ghi vào kho Sổ điểm; vẽ để thấy nguồn của điểm |
| Tiến trình | 7.1 Cập nhật danh mục quà tặng | nút lá F7.1, frm204 |
| Tiến trình | 7.2 Theo dõi sổ điểm và đổi quà | nút lá F7.2, frm205 |
| Kho | D1 Khách hàng | bảng `customers` (hạng) |
| Kho | D2 Sổ điểm | bảng `point_transactions` |
| Kho | D3 Quà tặng | bảng `rewards` |
| Kho | D4 Lượt đổi quà | bảng `reward_redemptions` |

### 4.2 Luồng dữ liệu

| Từ | Đến | Luồng | Nội dung |
|---|---|---|---|
| Quản trị viên | 7.1 | thông tin quà | tên, điểm quy đổi, hạng tối thiểu, số lượng, còn áp dụng |
| 7.1 | D3 | quà mới / quà đã sửa | |
| D3 | 7.1 | quà hiện có | để sửa |
| Khách hàng | 7.2 | yêu cầu xem điểm, đổi quà, hủy đổi | mã khách, mã quà hoặc mã lượt đổi |
| D1 | 7.2 | hạng khách | so với hạng tối thiểu (Q10) |
| D2 | 7.2 | các giao dịch điểm | tính số dư = tổng mọi dòng |
| D3 | 7.2 | điểm quy đổi, hạng tối thiểu, số lượng | kiểm tra Q10 |
| 7.2 | D4 | lượt đổi / trạng thái hủy | điểm quy đổi chốt lúc đổi |
| 7.2 | D2 | dòng −điểm đổi quà / +điểm hoàn | sổ điểm chỉ ghi thêm |
| 7.2 | D3 | số lượng −1 / +1 | |
| 7.2 | Khách hàng | sổ điểm, số dư, phiếu đổi quà, thông báo từ chối | BM6 |
| F6.2 | D2, D1 | dòng +điểm lưu trú, hạng mới | Q9, luồng L3 |
| F3.2 | D2 | dòng +điểm thiện chí | Q12, luồng L2 (qua `ITicketObserver`) |

### 4.3 Phân rã tiến trình 7.2 (đối chiếu với code)

| Bước | Việc | Đọc | Ghi | Code |
|---|---|---|---|---|
| 7.2.1 | Tra cứu sổ điểm và số dư | D1, D2 | – | `LoyaltyService.GetLedger` (P3) |
| 7.2.2 | Kiểm tra điều kiện đổi quà | D1, D2, D3 | – | `Reward.EnsureCanRedeem` |
| 7.2.3 | Ghi nhận đổi quà | – | D4, D2, D3 | `Reward.Redeem`, `PointTransaction.ForRedemption` |
| 7.2.4 | Hủy lượt đổi, hoàn điểm | D4 | D4, D2, D3 | `Reward.CancelRedemption`, `PointTransaction.ForRefund` |

Các bước 7.2.2 và 7.2.3 chạy trong cùng một giao dịch CSDL, nên không có hai lượt đổi cùng lấy món quà cuối.

### 4.4 Kiểm tra cân bằng với mức 0
Mọi luồng vào/ra F7 ở mức 0 phải xuất hiện ở mức 1: "thông tin quà" (vào 7.1), "yêu cầu đổi quà" (vào 7.2), "phiếu đổi quà" (ra từ 7.2). TV1 đối chiếu khi vẽ mức 0.
