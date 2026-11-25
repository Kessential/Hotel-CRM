# 02. Biểu đồ phân rã chức năng (BFD)

Module M3 của P1. [TV1] viết gốc, nhánh F1–F4 và khung sơ đồ; [TV2] viết nhánh F5–F8.

## 1. Quy ước
- 3 cấp: cấp 1 là hệ thống; cấp 2 là nhóm chức năng (F1–F8); cấp 3 là nút lá, mỗi nút lá ứng với một Form.
- Tên chức năng là cụm động từ ("Cập nhật…", "Lập…", "Theo dõi…").
- Sơ đồ: `diagrams/bfd.puml` (TV1, `@startwbs`) gồm nút gốc rồi `!include bfd-tv1.iuml` và `!include bfd-tv2.iuml`. Mỗi người chỉ sửa file `.iuml` của mình.

Khung gợi ý cho `bfd.puml` (TV1 tạo):
```
@startwbs
* Hệ thống CRM khách sạn
!include bfd-tv1.iuml
!include bfd-tv2.iuml
@endwbs
```

## 2. Cây BFD và nhánh F1–F4 [TV1]
> TV1 viết.

## 3. Nhánh F5–F8 [TV2]
File sơ đồ: `diagrams/bfd-tv2.iuml`.

| Cấp 2 | Cấp 3 (nút lá) | Mô tả | Form | UC | Yêu cầu |
|---|---|---|---|---|---|
| F5 Quản lý hồ sơ khách hàng 360 | F5.1 Cập nhật hồ sơ khách | Thêm, sửa, xóa, tìm khách; lọc theo từ khóa, hạng, quốc tịch, đồng ý ưu đãi | frm201_dm_khach_hang | UC07 | YC5.1 |
| | F5.2 Tra cứu hồ sơ 360 của khách | Xem lưu trú, tương tác, giao dịch điểm của khách đang chọn; tổng chi tiêu, số lần ở, số dư | frm201 (phần chi tiết) | UC07 | YC5.2 |
| F6 Quản lý lưu trú | F6.1 Cập nhật danh mục loại phòng và phòng | Loại phòng (giá/đêm, sức chứa) và các phòng thuộc loại đó | frm202_dm_phong | UC08 (mở rộng) | YC6.1 |
| | F6.2 Đặt phòng, nhận phòng, trả phòng | Đặt, nhận, trả, hủy; trả phòng cộng điểm và sinh phiếu khảo sát chờ | frm203_nv_luu_tru | UC08 | YC6.2 |
| F7 Quản lý khách hàng thân thiết | F7.1 Cập nhật danh mục quà tặng | Quà, điểm quy đổi, hạng tối thiểu, số lượng | frm204_dm_qua_tang | UC09 (mở rộng) | YC7.1 |
| | F7.2 Theo dõi sổ điểm và đổi quà | Xem sổ điểm, đổi quà, hủy lượt đổi | frm205_nv_doi_qua | UC09 | YC7.2 |
| F8 Chăm sóc theo chiến dịch và báo cáo khách hàng | F8.1 Lập và theo dõi chiến dịch chăm sóc | Chọn phân khúc, lập danh sách, ghi phản hồi | frm206_nv_chien_dich | UC10 | YC8.1 |
| | F8.2 Lập báo cáo khách hàng và dashboard 360 | R3, R4, R5, xuất Excel | frm207_bc_khach_hang | UC11 | YC8.2 |

F5.1 và F5.2 cùng nằm trên frm201 vì đây là Form Master-Detail: lưới khách là Master (F5.1), các tab lưu trú/tương tác/sổ điểm là Detail (F5.2). Hai nút lá vẫn là hai chức năng riêng với hai nhóm hàm service khác nhau (sửa dữ liệu và chỉ đọc).

## 4. Truy vết BFD → UC → Form → Service [TV2]
Tên hàm service là dự kiến, chốt ở M6 (`07-thiet-ke-lop.md`). Mọi hàm ghi dữ liệu nhận `actor` để kiểm quyền (Q6).

| Nút lá | Form | Service | Hàm (dự kiến) | Lớp Domain quyết định |
|---|---|---|---|---|
| F5.1 | frm201 | `CustomerService` | `Search`, `Create`, `Update`, `Delete` | `Customer` (Q7, Q13) |
| F5.2 | frm201 | `CustomerService` | `GetProfile` | – (chỉ đọc) |
| F6.1 | frm202 | `StayService` | `GetRoomTypes`, `SaveRoomType`, `SaveRoom` | `RoomType`, `Room` |
| F6.2 | frm203 | `StayService` | `FindAvailableRooms`, `Book`, `CheckIn`, `CheckOut`, `Cancel`, `Search` | `Booking`, `Room` (Q8), `MembershipTier` (Q9) |
| F7.1 | frm204 | `LoyaltyService` | `GetRewards`, `SaveReward` | `Reward` |
| F7.2 | frm205 | `LoyaltyService` | `GetLedger`, `Redeem`, `CancelRedemption` | `Reward`, `RewardRedemption`, `PointTransaction` (Q10) |
| F8.1 | frm206 | `CampaignService` | `GetSegments`, `PreviewTargets`, `Create`, `RecordResponse` | `Campaign` (Q11), `ISegmentStrategy` |
| F8.2 | frm207 | `CustomerReportService`, `DashboardService` | `GetTierReport` (R3), `GetCampaignReport` (R4), `GetDashboard` (R5) | – (chỉ đọc) |

Lớp Domain đã có code (PR #2): `MembershipTier` (`EarnRate`, `PointsFor`, `IsAtLeast`), `Reward`, `RewardRedemption`, `PointTransaction`, `Campaign`, `CampaignTarget`, `ISegmentStrategy` + 3 phân khúc.
