# 02. Sơ đồ phân cấp chức năng (BFD)

Module M3 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.2). Sơ đồ: `diagrams/bfd.puml` (TV1) `!include` `bfd-tv1.iuml` và `bfd-tv2.iuml`; mỗi người chỉ sửa file `.iuml` của mình.

## 1. Cây BFD (ảnh) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. Nút lá ↔ Form ↔ UC ↔ Service

### 2.1 F1–F4 [TV1]
_[TV1] chưa điền._

### 2.2 F5–F8 [TV2]
File sơ đồ: `diagrams/bfd-tv2.iuml` (nhánh cấp 2 F5–F8, chức năng cấp 3, chức năng con cấp 4). Mọi tên là cụm động từ (góp ý của thầy), nên tên nhánh cấp 2 đổi từ danh từ của PLAN 4.2 sang động từ: F5 Quản lý hồ sơ khách hàng, F6 Quản lý lưu trú, F7 Quản lý khách hàng thân thiết, F8 Chăm sóc khách hàng theo chiến dịch; mã F giữ nguyên. Tên tiến trình 5.0–8.0 của DFD mức 0 (TV1) và PLAN 4.2 cần đổi theo (ghi ở `docs/PROGRESS.md` mục 8). Bảng dưới: mỗi chức năng cấp 3 ứng với một Form; phân rã cấp 4 ở mục 2.3.

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
- Lớp Domain đã có code: PR #2 (`MembershipTier`, `Reward`, `RewardRedemption`, `PointTransaction`, `Campaign`, `CampaignTarget`; `ISegmentStrategy` + 3 phân khúc ở BLL) và đợt 2 (`Customer`, `Interaction`, `Booking`, `Room`, `RoomType`).

### 2.3 Phân rã chức năng F5–F8 đến cấp 4 [TV2]
Theo góp ý của thầy: sơ đồ phân rã thể hiện **tất cả** chức năng mà chương trình có; mọi tên chức năng là **động từ**; chỉ trình bày chức năng, không ghi dữ liệu (dữ liệu thuộc phần CSDL, `06-erd.md`). Mã cấp 4 của F7.2 trùng tiến trình con của DFD mức 1 F7 (`05-dfd.md` mục 5).

| Cấp 2 | Cấp 3 | Cấp 4 |
|---|---|---|
| F5 Quản lý hồ sơ khách hàng | F5.1 Cập nhật hồ sơ khách | F5.1.1 Thêm khách mới · F5.1.2 Sửa hồ sơ khách · F5.1.3 Xóa khách · F5.1.4 Tìm và lọc khách |
| | F5.2 Tra cứu hồ sơ 360 của khách | F5.2.1 Xem lịch sử lưu trú · F5.2.2 Xem lịch sử tương tác · F5.2.3 Xem sổ điểm · F5.2.4 Tính tổng chi tiêu, số lần ở, số dư điểm |
| F6 Quản lý lưu trú | F6.1 Cập nhật danh mục phòng | F6.1.1 Thêm, sửa loại phòng · F6.1.2 Thêm, sửa phòng |
| | F6.2 Đặt, nhận, trả phòng | F6.2.1 Tìm phòng trống · F6.2.2 Đặt phòng · F6.2.3 Nhận phòng · F6.2.4 Trả phòng (cộng điểm, xét hạng, tạo phiếu khảo sát chờ) · F6.2.5 Hủy đặt phòng · F6.2.6 Tìm và lọc đặt phòng |
| F7 Quản lý khách hàng thân thiết | F7.1 Cập nhật danh mục quà tặng | F7.1.1 Thêm, sửa quà · F7.1.2 Ngừng áp dụng quà |
| | F7.2 Theo dõi sổ điểm và đổi quà | F7.2.1 Tra cứu sổ điểm và số dư · F7.2.2 Kiểm tra điều kiện đổi quà · F7.2.3 Ghi nhận đổi quà · F7.2.4 Hủy lượt đổi, hoàn điểm |
| | F7.3 Cộng điểm thiện chí (tự động khi ticket của khách đóng, Q12, L2) | – (không có Form; chạy trong giao dịch đóng ticket của TV1) |
| F8 Chăm sóc khách hàng theo chiến dịch | F8.1 Lập và theo dõi chiến dịch | F8.1.1 Chọn phân khúc khách · F8.1.2 Xem trước danh sách khách · F8.1.3 Lưu chiến dịch · F8.1.4 Ghi phản hồi của khách · F8.1.5 Tìm và xem chiến dịch |
| | F8.2 Lập báo cáo khách hàng | F8.2.1 Lập báo cáo phân khúc khách theo hạng (R3) · F8.2.2 Lập báo cáo hiệu quả chiến dịch (R4) · F8.2.3 Xem dashboard khách hàng 360 (R5) · F8.2.4 Xuất báo cáo ra Excel |

F7.3 là chức năng mới có tên trên cây: trước đây chỉ ghi ở `01-khao-sat.md` mục 5.2 là "việc tự động không có Form". Mã F khác không đổi.

### 2.4 Xếp vào cây theo chuẩn 3 nhánh chung (đề xuất, chờ TV1 chốt cây tổng)
Thầy yêu cầu hệ thống nào cũng có nhánh **Quản trị hệ thống**, **Quản lý danh mục**, **Báo cáo thống kê**; checklist ghi chuẩn "3 nhánh chung + nhánh nghiệp vụ riêng". Đề xuất của TV2, **giữ nguyên mã F** để không phải sửa các tài liệu khác:

| Nhánh trên cây tổng | Chức năng của TV2 xếp vào |
|---|---|
| Quản trị hệ thống (TV1) | – (đăng nhập, nhân viên, phân quyền thuộc TV1) |
| Quản lý danh mục (chung) | F6.1 Cập nhật danh mục phòng, F7.1 Cập nhật danh mục quà tặng |
| Nghiệp vụ quan hệ khách hàng (TV2) | F5 Quản lý hồ sơ khách hàng, F6.2 Đặt, nhận, trả phòng, F7.2–F7.3 sổ điểm, đổi quà, điểm thiện chí, F8.1 Lập và theo dõi chiến dịch |
| Báo cáo thống kê (chung) | F8.2.1 Báo cáo phân khúc khách theo hạng (R3) · F8.2.2 Báo cáo hiệu quả chiến dịch (R4) · F8.2.3 Dashboard khách hàng 360 (R5) |

### 2.5 Dùng AI rà soát đủ chức năng: prompt và mẫu phân rã [TV2]
Thầy gợi ý đưa cho AI một mẫu sơ đồ phân rã để AI học cách trình bày, rồi yêu cầu AI liệt kê đủ chức năng nghiệp vụ từ các hệ thống tham khảo. Prompt TV2 dùng:

```text
Bạn là chuyên viên phân tích hệ thống thông tin quản lý.
Mẫu sơ đồ phân rã chức năng (học cách trình bày, mọi tên là động từ, không ghi dữ liệu):
  Quản lý thư viện
  ├─ Quản lý danh mục: Cập nhật đầu sách · Cập nhật độc giả
  ├─ Mượn trả sách: Lập phiếu mượn · Ghi nhận trả sách · Gia hạn · Tính tiền phạt
  └─ Báo cáo thống kê: Lập báo cáo sách mượn nhiều · Lập báo cáo độc giả quá hạn
Hãy lập sơ đồ phân rã tương tự cho phân hệ "Quan hệ khách hàng và khách hàng thân thiết"
của CRM khách sạn, tham khảo chức năng của Oracle OPERA Cloud (hồ sơ khách), Marriott Bonvoy,
Hilton Honors (thành viên) và HubSpot CRM (chiến dịch). Cuối cùng liệt kê các chức năng mà
các hệ thống đó có nhưng sơ đồ dưới đây chưa có: <dán bảng mục 2.3>.
```

Kết quả rà soát và quyết định của nhóm ghi ở `03-use-case.md` mục 6.

## 3. Đối chiếu HuongdanXDHTTTQL [Chung – TV1]
_[Chung – TV1] chưa điền._
