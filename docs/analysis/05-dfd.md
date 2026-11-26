# 05. Sơ đồ luồng dữ liệu (DFD)

Module M4 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.3). Ký hiệu Gane–Sarson cho cả nhóm; tác nhân, tiến trình mức 0 và kho D1–D11 lấy đúng danh mục ở mục 1.5 của file task.

## 1. Ký hiệu, tác nhân, kho dữ liệu [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. DFD ngữ cảnh [Chung – TV1]
_[Chung – TV1] chưa điền._

## 3. DFD mức 0 [Chung – TV1]
_[Chung – TV1] chưa điền._

## 4. DFD mức 1 — F3 Điều phối & xử lý [TV1]
_[TV1] chưa điền._

## 5. DFD mức 1 — F7 Khách hàng thân thiết [TV2]
Sơ đồ: `diagrams/dfd-level1-f7.puml` (tách tiến trình 7.0 của DFD mức 0).

Cách vẽ Gane–Sarson trong PlantUML: tác nhân ngoài là hộp nền xám viền đậm; tiến trình là hộp bo góc, số hiệu ở trên, tên ở dưới vạch ngang; tiến trình của nhánh khác (3.0, 6.0) viền nét đứt; kho là hộp "Dn │ tên". PlantUML không vẽ được cạnh phải để hở của kho Gane–Sarson; nếu TV1 làm bộ ký hiệu chung ở mục 1 thì sơ đồ này đổi theo.

### 5.1 Thành phần

| Loại | Tên | Ghi chú |
|---|---|---|
| Tác nhân ngoài | Khách hàng | yêu cầu xem điểm, đổi quà, hủy đổi; nhận phiếu đổi quà (giao tiếp qua lễ tân) |
| Tác nhân ngoài | Quản trị hệ thống | cung cấp thông tin quà |
| Tiến trình nhánh khác | 3.0 Điều phối & xử lý (TV1), 6.0 Lưu trú (TV2) | ghi vào kho D9, D5; vẽ để thấy nguồn của điểm |
| Tiến trình | 7.1 Danh mục quà tặng | nút lá F7.1, frm204 |
| Tiến trình | 7.2 Sổ điểm & đổi quà | nút lá F7.2, frm205 |
| Kho | D5 Khách hàng | bảng `customers` (hạng) |
| Kho | D9 Sổ điểm | bảng `point_transactions` |
| Kho | D10 Quà tặng & lượt đổi | bảng `rewards`, `reward_redemptions` |

### 5.2 Luồng dữ liệu

| Từ | Đến | Luồng | Nội dung |
|---|---|---|---|
| Quản trị hệ thống | 7.1 | thông tin quà | tên, điểm quy đổi, hạng tối thiểu, số lượng, còn áp dụng |
| 7.1 | D10 | quà mới / quà đã sửa | |
| D10 | 7.1 | quà hiện có | để sửa |
| Khách hàng | 7.2 | yêu cầu xem điểm, đổi quà, hủy đổi | mã khách, mã quà hoặc mã lượt đổi |
| D5 | 7.2 | hạng khách | so với hạng tối thiểu (Q10) |
| D9 | 7.2 | các giao dịch điểm | tính số dư = tổng mọi dòng |
| D10 | 7.2 | điểm quy đổi, hạng tối thiểu, số lượng còn | kiểm tra Q10 |
| 7.2 | D10 | lượt đổi (điểm chốt) / trạng thái hủy; số lượng −1 / +1 | điểm quy đổi chốt lúc đổi |
| 7.2 | D9 | dòng −điểm đổi quà / +điểm hoàn | sổ điểm chỉ ghi thêm |
| 7.2 | Khách hàng | sổ điểm, số dư, phiếu đổi quà, thông báo từ chối | BM6 |
| 6.0 | D9, D5 | dòng +điểm lưu trú, hạng mới | Q9, luồng L3 |
| 3.0 | D9, D5 | dòng +điểm thiện chí, hạng mới | Q12, luồng L2 (qua `ITicketObserver`) |

### 5.3 Phân rã tiến trình 7.2 (đối chiếu với code)

| Bước | Việc | Đọc | Ghi | Code |
|---|---|---|---|---|
| 7.2.1 | Tra cứu sổ điểm và số dư | D5, D9 | – | `LoyaltyService.GetLedger` (P3) |
| 7.2.2 | Kiểm tra điều kiện đổi quà | D5, D9, D10 | – | `Reward.EnsureCanRedeem` |
| 7.2.3 | Ghi nhận đổi quà | – | D10, D9 | `Reward.Redeem`, `PointTransaction.ForRedemption` |
| 7.2.4 | Hủy lượt đổi, hoàn điểm | D10 | D10, D9 | `Reward.CancelRedemption`, `PointTransaction.ForRefund` |

Các bước 7.2.2 và 7.2.3 chạy trong cùng một giao dịch CSDL, nên không có hai lượt đổi cùng lấy món quà cuối.

### 5.4 Kiểm tra cân bằng với mức 0
Mọi luồng vào/ra tiến trình 7.0 ở mức 0 phải xuất hiện ở mức 1:

| Luồng ở mức 0 | Ở mức 1 |
|---|---|
| Quản trị hệ thống → 7.0: thông tin quà | vào 7.1 |
| Khách hàng → 7.0: yêu cầu đổi quà | vào 7.2 |
| 7.0 → Khách hàng: phiếu đổi quà, sổ điểm | ra từ 7.2 |
| 7.0 ↔ D5, D9, D10 | 7.1 dùng D10; 7.2 dùng D5, D9, D10 |

Luồng 3.0 → D9, D5 và 6.0 → D9, D5 thuộc tiến trình 3.0, 6.0 ở mức 0, không phải luồng của 7.0; mức 1 vẽ lại chỉ để thấy nguồn điểm. TV1 đối chiếu bảng này khi vẽ mức 0.
