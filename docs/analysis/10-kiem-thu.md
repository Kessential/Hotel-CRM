# 10. Kiểm thử

Module M8 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.7). Mã ca test: TV1 `TC1-01…TC1-14`, TV2 `TC2-01…TC2-14` (01–10 unit, 11–14 integration).

## 1. Chiến lược (unit / integration, Fake, IClock, tiêm lỗi) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. Ma trận ca test TC1-01…TC1-14 [TV1]
_[TV1] chưa điền._

## 3. Ma trận ca test TC2-01…TC2-14 [TV2]
Unit test không cần CSDL, dùng Fake cho interface của phân hệ TV1 và `IClock` cố định; integration test chạy trên CSDL SQLite tạm có seed. Giá trị kỳ vọng lấy từ bảng tính tay. Lỗi tiêm thử của từng ca ở mục 6.

| TC | Loại | Kỹ thuật | Q/UC/R/L | Dữ liệu vào | Kết quả mong đợi | Fake | Lớp test dự kiến |
|---|---|---|---|---|---|---|---|
| TC2-01 | Unit | BVA độ dài, EP | Q7, UC07 | SĐT 9 / 10 / 11 số, SĐT không bắt đầu bằng 0; CCCD (Việt Nam) 11 / 12 / 13 số; hộ chiếu (Nhật) 5 / 6 / 9 / 10 ký tự; email thiếu `@` | SĐT 10 số, CCCD 12 số, hộ chiếu 6 và 9 ký tự → được; các giá trị còn lại → `ValidationException` | – | `CustomerTests` |
| TC2-02 | Unit | EP | Q7, UC07 | khách B đổi SĐT trùng khách A; khách A sửa hồ sơ giữ nguyên SĐT của mình | B → `BusinessRuleException`; A → được | – | `CustomerTests` |
| TC2-03 | Unit | BVA ngày | Q8, UC08 | phòng có booking 10–12; đặt 11–13; đặt 12–14; booking 10–12 đã hủy rồi đặt 10–12 | 11–13 → `BusinessRuleException`; 12–14 → được; 10–12 sau khi hủy → được | – | `RoomTests` |
| TC2-04 | Unit | BVA thời điểm, chuyển trạng thái | Q8, UC08 | booking Đã đặt 10–12; nhận phòng lúc 09/xx 23:59, 10/xx 00:00, 12/xx 00:00 | 10/xx 00:00 → Đang ở; hai mốc còn lại → `BusinessRuleException`, trạng thái giữ nguyên | – | `BookingTests` |
| TC2-05 | Unit | tính tay | Q9, UC08 | khách Standard 900 điểm đã tích, thành tiền 42.000.000đ; khách Silver, thành tiền 2.500.000đ | +4.200 → 5.100 điểm → Gold (lên 2 bậc); khách Silver +300 điểm | – | `MembershipTierTests`, `CustomerTests` |
| TC2-06 | Unit | tính tay | Q9, Q10, UC09 | khách Gold 5.100 điểm đã tích, số dư 5.100; đổi quà 600 điểm | số dư 4.500; điểm đã tích 5.100; vẫn Gold | – | `CustomerTests` |
| TC2-07 | Unit | BVA số dư, EP | Q10, UC09 | quà 500 điểm: số dư 500 / 499; khách Silver đổi quà hạng Gold; quà số lượng 0; quà ngừng áp dụng | số dư 500 → được; còn lại → `BusinessRuleException`, số lượng quà không đổi | – | `RewardTests`, `PointTransactionTests` |
| TC2-08 | Unit | BVA ngày | Q11, UC10 | chiến dịch bắt đầu 01/12; lần trả phòng gần nhất cách 180 / 179 ngày; sinh 01/12, 31/12, 30/11; khách hạng Gold, VIP | lâu chưa quay lại: 180 có, 179 không; sinh nhật: 01/12 và 31/12 có, 30/11 không; VIP: chỉ khách VIP | – | `SegmentStrategyTests` |
| TC2-09 | Unit | EP | Q11, UC10, L4 | 4 khách thuộc phân khúc: 1 không đồng ý ưu đãi, 1 đang khiếu nại (Fake trả về), 2 hợp lệ; phân khúc không có khách nào hợp lệ | xem trước còn 2 khách, báo loại 1 + 1; danh sách rỗng → `BusinessRuleException` khi lưu | `FakeTicketQuery` | `CampaignTests`, `CampaignServiceTests` |
| TC2-10 | Unit | EP | Q13, UC07, L5 | khách có 1 ticket đã đóng (Fake); khách có 1 booking đã hủy; khách không có gì | hai khách đầu → `BusinessRuleException`; khách cuối → xóa được | `FakeTicketQuery` | `CustomerServiceTests` |
| TC2-11 | Integration | luồng đầy đủ | L3, Q8, Q9, UC08 | CSDL tạm; khách Standard 900 điểm; booking Đang ở 2 đêm × 2.500.000đ; `StayService.CheckOut(lễ tân, booking)` | booking Đã trả phòng; 1 dòng sổ điểm Lưu trú +500; hạng Silver (1.400 điểm); 1 tương tác; 1 phiếu khảo sát Chờ của booking | – (`SurveyScheduler` thật khi có, trước đó Fake ghi nhận lời gọi) | `StayServiceIntegrationTests` |
| TC2-12 | Integration | Rollback | L3, UC08 | như TC2-11, `ISurveyScheduler` là Fake ném ngoại lệ | ngoại lệ tới người gọi; đọc lại CSDL: booking vẫn Đang ở, 0 dòng sổ điểm mới, hạng Standard, 0 tương tác mới | `FakeSurveyScheduler` | `StayServiceIntegrationTests` |
| TC2-13 | Integration | tính tay | R5, L6, UC11 | bộ dữ liệu ở mục 5.2; `DashboardService.GetDashboard()` | từng dòng và dòng tổng khớp bảng 5.3 | `FakeCustomerCareStats` (tới khi `CustomerCareStats` của TV1 xong, 8/12) | `DashboardServiceIntegrationTests` |
| TC2-14 | Integration | RBAC | Q6, Q10, Q11 | tài khoản 4 vai trò; CSKH gọi `LoyaltyService.Redeem`; Lễ tân gọi `CampaignService.Create`; Admin gọi cả hai | CSKH, Lễ tân → `AuthorizationException`, CSDL không đổi; Admin → thành công | – | `RbacIntegrationTests` |

**Trạng thái so với code:**
- Đã có test trong PR #2: TC2-05 (phần `MembershipTierTests`, 11 trường hợp), TC2-07 (`RewardTests` 6, `PointTransactionTests` 3), TC2-08 (`SegmentStrategyTests` 11), phần Domain của TC2-09 (`CampaignTests` 10).
- Có từ v1, sửa ở đợt 2: TC2-01, TC2-02 (`Register_DuplicatePhone_ThrowsBusinessRule`), TC2-03 (`Book_OverlappingDates_ThrowsBusinessRule`), TC2-10 (`Delete_CustomerWithOpenTicket_ThrowsBusinessRule`, đổi theo Q13 mới).
- Chưa có: TC2-04, TC2-06, phần service của TC2-09 (P3), TC2-11…TC2-14 (P3–P4).
- Ngoài 14 mã trên, PR #2 có thêm test hỗ trợ (`PointTransactionTests`, phần `CampaignTests` về ngày kết thúc và tỷ lệ phản hồi). Đếm theo định mức thì tính 14 mã TC2.
- Ca dùng Fake interface của TV1: TC2-09, TC2-10 (`FakeTicketQuery`), TC2-12 (`FakeSurveyScheduler`), TC2-13 (`FakeCustomerCareStats`).

## 4. Bảng tính tay R2 [TV1]
_[TV1] chưa điền._

## 5. Bảng tính tay R5 – Dashboard 360 [TV2]

### 5.1 Định nghĩa
- **Số lần ở:** số booking Đã trả phòng của khách.
- **Tổng chi tiêu:** Σ (số đêm × `RatePerNight`) của các booking Đã trả phòng. Không tính Đã đặt, Đang ở, Đã hủy.
- **Số ticket, số khiếu nại:** lấy từ `ICustomerCareStats.GetTicketCounts` (TV1), mọi trạng thái.
- **Điểm hài lòng TB của khách:** trung bình cộng điểm TB các phiếu đã khảo sát của khách. Điểm TB một phiếu = trung bình điểm các tiêu chí. Khách chưa có phiếu nào → để trống. Làm tròn 2 chữ số khi hiển thị.
- **Dòng tổng:** cộng các cột đếm và tiền. Cột điểm hài lòng = trung bình điểm TB của **mọi phiếu** đã khảo sát, không phải trung bình các dòng.

### 5.2 Dữ liệu
Loại phòng Deluxe đổi giá từ 1.200.000đ lên 1.500.000đ giữa B1 và B2; giá hiện tại 1.500.000đ.

| Booking | Khách | Phòng | Số đêm | Giá/đêm chốt (đ) | Trạng thái | Phiếu khảo sát (5 tiêu chí) |
|---|---|---|---|---|---|---|
| B1 | KH1 An | 305 Deluxe | 3 | 1.200.000 | Đã trả phòng | 5, 4, 5, 4, 5 |
| B2 | KH1 An | 306 Deluxe | 2 | 1.500.000 | Đã trả phòng | 4, 4, 3, 4, 5 |
| B3 | KH1 An | 305 Deluxe | 2 | 1.200.000 | Đã hủy | – |
| B4 | KH2 Bình | 201 Standard | 1 | 800.000 | Đã trả phòng | Chờ (chưa khảo sát) |
| B5 | KH2 Bình | 201 Standard | 2 | 800.000 | Đang ở | – |
| B6 | KH2 Bình | 202 Standard | 1 | 800.000 | Đã trả phòng | 3, 3, 3, 3, 3 |
| – | KH3 Chi | – | – | – | không có booking | – |

Ticket (số liệu `ICustomerCareStats` trả về): KH1 3 ticket, 1 khiếu nại; KH2 2 ticket, 2 khiếu nại; KH3 0 ticket.

### 5.3 Kết quả kỳ vọng

Điểm TB từng phiếu: B1 = 23 / 5 = 4,60; B2 = 20 / 5 = 4,00; B6 = 15 / 5 = 3,00.

| Mã KH | Họ tên | Số lần ở | Tổng chi tiêu (đ) | Số ticket | Số khiếu nại | Điểm hài lòng TB |
|---|---|---|---|---|---|---|
| KH1 | An | 2 (B1, B2) | 3 × 1.200.000 + 2 × 1.500.000 = **6.600.000** | 3 | 1 | (4,60 + 4,00) / 2 = **4,30** |
| KH2 | Bình | 2 (B4, B6) | 800.000 + 800.000 = **1.600.000** | 2 | 2 | 3,00 / 1 = **3,00** |
| KH3 | Chi | 0 | **0** | 0 | 0 | (trống) |
| **Tổng** | | **4** | **8.200.000** | **5** | **3** | (4,60 + 4,00 + 3,00) / 3 = 3,8667 → **3,87** |

### 5.4 Những sai lầm bảng này bắt được
- **Tính chi tiêu theo giá hiện tại của loại phòng:** KH1 thành 5 × 1.500.000 = 7.500.000 (sai 900.000). Đây là lý do cần thuộc tính lịch sử `RatePerNight`.
- **Tính cả booking Đã hủy hoặc Đang ở:** KH1 thêm 2.400.000 (B3), KH2 thêm 1.600.000 (B5).
- **Tính phiếu Chờ thành 0 điểm:** KH2 thành (0 + 3,00) / 2 = 1,50.
- **Dòng tổng lấy trung bình các dòng:** (4,30 + 3,00) / 2 = 3,65 thay vì 3,87.
- **Đếm khách không có booking thành lỗi hoặc bỏ mất dòng:** KH3 phải có dòng toàn số 0 và điểm trống.

## 6. Kế hoạch tiêm lỗi [mỗi người dòng của mình]
Tiêm từng lỗi vào code, chạy test: ca tương ứng phải chuyển đỏ; bỏ lỗi thì xanh lại. Ca nào không đỏ thì ca đó là "test hình thức" và phải sửa.

| TC | Lỗi tiêm thử | Nơi tiêm | Người |
|---|---|---|---|
| TC1-xx | _[TV1] chưa điền._ | | TV1 |
| TC2-01 | đổi `== 12` thành `>= 12` khi kiểm tra CCCD | `Customer` | TV2 |
| TC2-02 | bỏ điều kiện "khác chính khách đó" khi so trùng | `CustomerService` / `Customer.EnsureUnique` | TV2 |
| TC2-03 | đổi `start < b.CheckOutDate` thành `<=` | `Room.EnsureAvailable` | TV2 |
| TC2-04 | bỏ kiểm tra `now.Date < CheckInDate` | `Booking.CheckIn` | TV2 |
| TC2-05 | bỏ vòng lặp `Next()` (chỉ lên 1 bậc) | `Customer.EarnPoints` | TV2 |
| TC2-06 | tính hạng theo số dư thay vì điểm đã tích | `Customer` / `LoyaltyService` | TV2 |
| TC2-07 | đổi `<` thành `<=` khi so số dư; trừ kho trước khi kiểm tra | `Reward.EnsureCanRedeem`, `Reward.Redeem` | TV2 |
| TC2-08 | đổi `>=` thành `>` | `LapsedGuestSegment.Includes` | TV2 |
| TC2-09 | bỏ lời gọi `ITicketQuery` | `CampaignService.PreviewTargets` | TV2 |
| TC2-10 | chỉ xét ticket chưa đóng (cách của v1) | `Customer.EnsureCanBeDeleted` | TV2 |
| TC2-11 | bỏ lời gọi `ScheduleFor` | `StayService.CheckOut` | TV2 |
| TC2-12 | bỏ `IUnitOfWork.Execute` (ghi từng bước) | `StayService.CheckOut` | TV2 |
| TC2-13 | tính chi tiêu theo giá hiện tại của loại phòng; tính cả booking chưa trả phòng | truy vấn R5 (DAL) / `DashboardService` | TV2 |
| TC2-14 | bỏ dòng kiểm quyền | `LoyaltyService.Redeem`, `CampaignService.Create` | TV2 |
