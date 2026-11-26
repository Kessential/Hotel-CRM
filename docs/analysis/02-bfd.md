# 02. Sơ đồ phân cấp chức năng (BFD)

Module M3 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.2). Sơ đồ: `diagrams/bfd.puml` (TV1) `!include` `bfd-tv1.iuml` và `bfd-tv2.iuml`; mỗi người chỉ sửa file `.iuml` của mình.

## 1. Cây BFD (ảnh) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. Nút lá ↔ Form ↔ UC ↔ Service

### 2.1 F1–F4 [TV1]
_[TV1] chưa điền._

### 2.2 F5–F8 [TV2]
File sơ đồ: `diagrams/bfd-tv2.iuml` (nhánh cấp 2 F5–F8 và nút lá cấp 3). Tên nhánh cấp 2 trùng PLAN 4.2 và tên tiến trình 5.0–8.0 của DFD mức 0; tên nút lá là cụm động từ, mỗi nút lá ứng với một Form.

| Cấp 2 | Nút lá (cấp 3) | Mô tả | Form | UC | Service | Hàm (dự kiến) | Lớp Domain quyết định |
|---|---|---|---|---|---|---|---|
| F5 Hồ sơ khách hàng 360 | F5.1 Cập nhật hồ sơ khách | Thêm, sửa, xóa, tìm khách; lọc theo từ khóa, hạng, quốc tịch, đồng ý ưu đãi | frm201_dm_khach_hang | UC07 | `CustomerService` | `Search`, `Create`, `Update`, `Delete` | `Customer` (Q7, Q13) |
| | F5.2 Tra cứu hồ sơ 360 của khách | Xem lưu trú, tương tác, giao dịch điểm của khách đang chọn; tổng chi tiêu, số lần ở, số dư | frm201 (phần chi tiết) | UC07 | `CustomerService` | `GetProfile` | – (chỉ đọc) |
| F6 Lưu trú | F6.1 Cập nhật danh mục loại phòng và phòng | Loại phòng (giá/đêm, sức chứa) và các phòng thuộc loại đó | frm202_dm_phong | UC08 | `StayService` | `GetRoomTypes`, `SaveRoomType`, `SaveRoom` | `RoomType`, `Room` |
| | F6.2 Đặt phòng, nhận phòng, trả phòng | Đặt, nhận, trả, hủy; trả phòng cộng điểm và sinh phiếu khảo sát chờ | frm203_nv_luu_tru | UC08 | `StayService` | `FindAvailableRooms`, `Book`, `CheckIn`, `CheckOut`, `Cancel`, `Search` | `Booking`, `Room` (Q8), `MembershipTier`, `Customer` (Q9) |
| F7 Khách hàng thân thiết | F7.1 Cập nhật danh mục quà tặng | Quà, điểm quy đổi, hạng tối thiểu, số lượng | frm204_dm_qua_tang | UC09 | `LoyaltyService` | `GetRewards`, `SaveReward` | `Reward` |
| | F7.2 Theo dõi sổ điểm và đổi quà | Xem sổ điểm, đổi quà, hủy lượt đổi | frm205_nv_doi_qua | UC09 | `LoyaltyService` | `GetLedger`, `Redeem`, `CancelRedemption` | `Reward`, `RewardRedemption`, `PointTransaction` (Q10) |
| F8 Chiến dịch & báo cáo khách hàng | F8.1 Lập và theo dõi chiến dịch chăm sóc | Chọn phân khúc, lập danh sách, ghi phản hồi | frm206_nv_chien_dich | UC10 | `CampaignService` | `GetSegments`, `PreviewTargets`, `Create`, `RecordResponse` | `Campaign` (Q11), `ISegmentStrategy` |
| | F8.2 Lập báo cáo khách hàng và dashboard 360 | R3, R4, R5, xuất Excel | frm207_bc_khach_hang | UC11 | `CustomerReportService`, `DashboardService` | `GetTierReport` (R3), `GetCampaignReport` (R4), `GetDashboard` (R5) | – (chỉ đọc) |

- Tên hàm service là dự kiến, chốt ở M6 (`07-thiet-ke-lop.md` mục 4). Mọi hàm ghi dữ liệu nhận `actor` để kiểm quyền (Q6).
- F5.1 và F5.2 cùng nằm trên frm201 (PLAN 5.1) vì đây là Form Master-Detail: lưới khách là Master (F5.1), các tab lưu trú/tương tác/sổ điểm là Detail (F5.2). Hai nút lá vẫn là hai chức năng riêng với hai nhóm hàm service khác nhau (sửa dữ liệu và chỉ đọc).
- Lớp Domain đã có code (PR #2): `MembershipTier` (`EarnRate`, `PointsFor`, `IsAtLeast`), `Reward`, `RewardRedemption`, `PointTransaction`, `Campaign`, `CampaignTarget`; `ISegmentStrategy` + 3 phân khúc (ở BLL).

## 3. Đối chiếu HuongdanXDHTTTQL [Chung – TV1]
_[Chung – TV1] chưa điền._
