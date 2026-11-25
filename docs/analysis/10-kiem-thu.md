# 10. Kế hoạch kiểm thử

Module M8 của P1. [TV1] viết ma trận TC1-xx và bảng tính tay R2; [TV2] viết ma trận TC2-xx và bảng tính tay R5.

## 1. Quy ước [TV1][TV2]
- **Unit test:** không cần CSDL; dùng Fake tự viết cho interface của phân hệ kia; thời gian cố định (không `DateTime.Now`).
- **Integration test:** chạy trên CSDL SQLite tạm, tạo từ `schema.sql` cùng một bộ dữ liệu riêng cho test.
- **Giá trị kỳ vọng:** lấy từ bảng tính tay, không chép công thức của code.
- **Kỹ thuật thiết kế test:** BVA (giá trị biên), EP (phân lớp tương đương), chuyển trạng thái.
- **Tiêm lỗi:** mỗi test ghi một lỗi mà nó phải bắt được. Tiêm lỗi đó vào code thì test phải chuyển đỏ (chống "test hình thức").

## 2. Ma trận TC1-xx và bảng tính tay R2 [TV1]
> TV1 viết.

## 3. Ma trận TC2-xx [TV2]

### 3.1 Unit test (10)

| Mã | Quy tắc | Mục tiêu | Kỹ thuật | Dữ liệu và kỳ vọng | Lỗi tiêm thử phải bắt | Fake | Trạng thái |
|---|---|---|---|---|---|---|---|
| TC2-01 | Q7 | SĐT, email, CCCD/hộ chiếu sai bị từ chối | BVA độ dài, EP | SĐT 9/11 số, không bắt đầu bằng 0 → lỗi, 10 số → được; CCCD (Việt Nam) 11/13 → lỗi, 12 → được; hộ chiếu (Nhật) 5/10 → lỗi, 6/9 → được; email thiếu `@` → lỗi | đổi `== 12` thành `>= 12` | – | đợt 2 (2 test SĐT/email có từ v1) |
| TC2-02 | Q7 | Trùng SĐT bị từ chối, kể cả khi sửa | EP | khách B đổi SĐT trùng khách A → `BusinessRuleException`; A sửa giữ nguyên SĐT của mình → được | bỏ điều kiện "khác chính khách đó" | – | v1 có `Register_DuplicatePhone_ThrowsBusinessRule`; thêm trường hợp sửa ở đợt 2 |
| TC2-03 | Q8 | Đặt trùng lịch bị chặn, đặt nối tiếp được | BVA ngày | có booking 10–12; đặt 11–13 → lỗi; đặt 12–14 → được; booking 10–12 đã hủy thì đặt 10–12 → được | đổi `start < b.CheckOutDate` thành `<=` | – | v1 có `Book_OverlappingDates_ThrowsBusinessRule`; thêm trường hợp nối tiếp và đã hủy ở đợt 2 |
| TC2-04 | Q8 | Không nhận phòng ngoài [ngày nhận, ngày trả) | BVA thời điểm | booking 10–12: 09/xx 23:59 → lỗi; 10/xx 00:00 → được; 12/xx 00:00 → lỗi | bỏ kiểm tra `now.Date < CheckInDate` | – | đợt 2 |
| TC2-05 | Q9 | Tích điểm theo hạng và lên 2 bậc một lần | tính tay | khách Standard 900 điểm, trả phòng 42.000.000đ → +4.200 → 5.100 → Gold; khách Silver 2.500.000đ → 300 điểm | bỏ vòng lặp `Next()` (chỉ lên 1 bậc) | – | `MembershipTierTests` có (11 trường hợp); phần `EarnPoints` đợt 2 |
| TC2-06 | Q9, Q10 | Đổi quà giảm số dư nhưng không hạ hạng | tính tay | khách Gold 5.100 điểm đã tích, đổi quà 600 → số dư 4.500, vẫn Gold | tính hạng theo số dư thay vì điểm đã tích | – | đợt 2 |
| TC2-07 | Q10 | Đổi quà thiếu điểm, thiếu hạng, hết quà, ngừng áp dụng bị từ chối; kho không đổi | BVA số dư, EP | số dư = giá → được; giá − 1 → lỗi; Silver đổi quà Gold → lỗi; số lượng 0 → lỗi | đổi `<` thành `<=`; trừ kho trước khi kiểm tra | – | **có** (`RewardTests` 6, `PointTransactionTests` 3) |
| TC2-08 | Q11 | 3 phân khúc chọn đúng khách | BVA ngày | 180 ngày → lâu chưa quay lại, 179 → không; sinh 01/12, 31/12 → có, 30/11 → không (chiến dịch từ 01/12) | đổi `>=` thành `>` trong `LapsedGuestSegment` | – | **có** (`SegmentStrategyTests` 11) |
| TC2-09 | Q11 | Chiến dịch loại khách không đồng ý và khách đang khiếu nại; danh sách rỗng không lưu | EP | 4 khách thuộc phân khúc: 1 không đồng ý, 1 đang khiếu nại → còn 2 | bỏ lời gọi `ITicketQuery` | `FakeTicketQuery` | Domain **có** (`CampaignTests` 10); phần service P3 |
| TC2-10 | Q13 | Không xóa khách có ticket hoặc lưu trú | EP | khách có 1 ticket đã đóng → lỗi; khách có booking đã hủy → lỗi; khách không có gì → xóa được | chỉ xét ticket chưa đóng (cách của v1) | `FakeTicketQuery` | v1 có `Delete_CustomerWithOpenTicket_ThrowsBusinessRule`; sửa theo Q13 mới ở đợt 2 |

Ngoài 10 mã trên, PR #2 đã có thêm các test hỗ trợ (`PointTransactionTests`, phần `CampaignTests` về ngày kết thúc và tỷ lệ phản hồi). Đếm theo định mức thì tính 10 mã TC2.

### 3.2 Integration test (4)

| Mã | Mục tiêu | Chuẩn bị | Bước | Kỳ vọng | Lỗi tiêm thử phải bắt |
|---|---|---|---|---|---|
| TC2-I1 | L3 trả phòng đầy đủ | CSDL tạm; khách Standard 900 điểm; booking Đang ở 2 đêm × 2.500.000đ | `StayService.CheckOut(lễ tân, booking)` | booking Đã trả phòng; 1 dòng sổ điểm Lưu trú +500; hạng Silver (1.400 điểm); 1 tương tác; 1 phiếu khảo sát Chờ của booking | bỏ lời gọi `ScheduleFor` |
| TC2-I2 | L3 rollback khi phân hệ TV1 lỗi | như TC2-I1, `ISurveyScheduler` là Fake ném ngoại lệ | gọi `CheckOut` | ngoại lệ tới người gọi; đọc lại CSDL: booking vẫn Đang ở, 0 dòng sổ điểm mới, hạng Standard, 0 tương tác mới | bỏ `IUnitOfWork.Execute` (ghi từng bước) |
| TC2-I3 | R5 khớp tính tay | bộ dữ liệu mục 4 | `DashboardService.GetDashboard()` | từng dòng và dòng tổng khớp bảng 4.3 | tính chi tiêu theo giá hiện tại của loại phòng; tính cả booking chưa trả phòng |
| TC2-I4 | RBAC ở tầng Service | tài khoản 4 vai trò | CSKH `LoyaltyService.Redeem`; Lễ tân `CampaignService.Create`; Admin làm cả hai | CSKH, Lễ tân → `AuthorizationException`, CSDL không đổi; Admin → thành công | bỏ dòng kiểm quyền trong service |

Fake dùng cho integration: `FakeSurveyScheduler` (TC2-I2). TC2-I3 dùng `FakeCustomerCareStats` trả số liệu ở bảng 4.2 cho tới khi `CustomerCareStats` thật của TV1 xong (8/12), sau đó chạy với bản thật trên CSDL có cả bảng TV1.

## 4. Bảng tính tay R5 – Dashboard 360 [TV2]

### 4.1 Định nghĩa
- **Số lần ở:** số booking Đã trả phòng của khách.
- **Tổng chi tiêu:** Σ (số đêm × `RatePerNight`) của các booking Đã trả phòng. Không tính Đã đặt, Đang ở, Đã hủy.
- **Số ticket, số khiếu nại:** lấy từ `ICustomerCareStats.GetTicketCounts` (TV1), mọi trạng thái.
- **Điểm hài lòng TB của khách:** trung bình cộng điểm TB các phiếu đã khảo sát của khách. Điểm TB một phiếu = trung bình điểm các tiêu chí. Khách chưa có phiếu nào → để trống. Làm tròn 2 chữ số khi hiển thị.
- **Dòng tổng:** cộng các cột đếm và tiền. Cột điểm hài lòng = trung bình điểm TB của **mọi phiếu** đã khảo sát, không phải trung bình các dòng.

### 4.2 Dữ liệu
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

### 4.3 Kết quả kỳ vọng

Điểm TB từng phiếu: B1 = 23 / 5 = 4,60; B2 = 20 / 5 = 4,00; B6 = 15 / 5 = 3,00.

| Mã KH | Họ tên | Số lần ở | Tổng chi tiêu (đ) | Số ticket | Số khiếu nại | Điểm hài lòng TB |
|---|---|---|---|---|---|---|
| KH1 | An | 2 (B1, B2) | 3 × 1.200.000 + 2 × 1.500.000 = **6.600.000** | 3 | 1 | (4,60 + 4,00) / 2 = **4,30** |
| KH2 | Bình | 2 (B4, B6) | 800.000 + 800.000 = **1.600.000** | 2 | 2 | 3,00 / 1 = **3,00** |
| KH3 | Chi | 0 | **0** | 0 | 0 | (trống) |
| **Tổng** | | **4** | **8.200.000** | **5** | **3** | (4,60 + 4,00 + 3,00) / 3 = 3,8667 → **3,87** |

### 4.4 Những sai lầm bảng này bắt được
- **Tính chi tiêu theo giá hiện tại của loại phòng:** KH1 thành 5 × 1.500.000 = 7.500.000 (sai 900.000). Đây là lý do cần thuộc tính lịch sử `RatePerNight`.
- **Tính cả booking Đã hủy hoặc Đang ở:** KH1 thêm 2.400.000 (B3), KH2 thêm 1.600.000 (B5).
- **Tính phiếu Chờ thành 0 điểm:** KH2 thành (0 + 3,00) / 2 = 1,50.
- **Dòng tổng lấy trung bình các dòng:** (4,30 + 3,00) / 2 = 3,65 thay vì 3,87.
- **Đếm khách không có booking thành lỗi hoặc bỏ mất dòng:** KH3 phải có dòng toàn số 0 và điểm trống.
