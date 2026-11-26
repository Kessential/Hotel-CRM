# 03. Use Case

Module M3 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.2). Sơ đồ: `diagrams/use-case.puml` (TV1, khai báo 4 tác nhân) `!include` `use-case-tv1.iuml` và `use-case-tv2.iuml`.

## 1. Tác nhân [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. Sơ đồ Use Case tổng (ảnh) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 3. Danh sách Use Case

### 3.1 UC01–UC06 [TV1]
_[TV1] chưa điền._

### 3.2 UC07–UC11 [TV2]
Phần TV2 của sơ đồ: `diagrams/use-case-tv2.iuml`. Sơ đồ dùng alias tác nhân `ADM`, `QL`, `LT`, `CSKH` do `use-case.puml` khai báo; Admin tổng quát hóa ba vai trò còn lại nên làm được mọi Use Case và không ghi lại ở từng dòng.

| UC | Tên | Tác nhân | Form | Phức hợp | Liên phân hệ |
|---|---|---|---|---|---|
| UC07 | Quản lý hồ sơ khách 360 | Lễ tân, Quản lý; CSKH chỉ xem | frm201 | | L5: hỏi `ITicketQuery` trước khi xóa |
| UC08 | Đặt phòng & lưu trú (mở rộng: danh mục phòng; bao gồm: trả phòng) | Lễ tân, Quản lý | frm202, frm203 | ✓ | L3: trả phòng gọi `ISurveyScheduler` |
| UC09 | Đổi quà (mở rộng: danh mục quà) | Lễ tân, Quản lý | frm204, frm205 | ✓ | |
| UC10 | Chiến dịch chăm sóc (mở rộng: ghi phản hồi) | tạo: Quản lý; ghi phản hồi: Lễ tân, CSKH, Quản lý | frm206 | ✓ | L4: hỏi `ITicketQuery` khiếu nại chưa đóng |
| UC11 | Báo cáo khách hàng & dashboard 360 | Quản lý | frm207 | | L6: hỏi `ICustomerCareStats` |

## 4. Luồng liên phân hệ L1–L6 [Chung – TV1]
_[Chung – TV1] chưa điền._

## 5. Đặc tả Use Case phức hợp

### 5.1 UC04 Xử lý ticket [TV1]
_[TV1] chưa điền._

### 5.2 UC05 Ghi khảo sát hài lòng [TV1]
_[TV1] chưa điền._

### 5.3 UC08 Đặt phòng & lưu trú [TV2]

| Mục | Nội dung |
|---|---|
| Mã, tên | UC08 – Đặt phòng & lưu trú |
| Tác nhân | Lễ tân; Quản lý, Admin cũng làm được. Danh mục phòng (mở rộng): Admin |
| Mô tả ngắn | Giữ phòng cho khách theo khoảng ngày với giá chốt; ghi nhận khách nhận phòng và trả phòng; khi trả phòng thì tích điểm cho khách và chuyển lần ở sang khảo sát hài lòng |
| Tiền điều kiện | Người dùng đã đăng nhập với vai trò Lễ tân, Quản lý hoặc Admin; khách đã có hồ sơ (UC07) |
| Hậu điều kiện | Đặt phòng: có booking "Đã đặt" với giá/đêm đã chốt. Nhận phòng: booking "Đang ở". Trả phòng: booking "Đã trả phòng", sổ điểm có thêm một dòng Lưu trú, hạng khách đã xét lại, có một tương tác và một phiếu khảo sát "Chờ". Hủy: booking "Đã hủy" |
| Quy tắc | Q6 (quyền), Q8 (ngày, trùng lịch, giá chốt, trạng thái), Q9 (tích điểm, nâng hạng) |
| Bảng dữ liệu | đọc `customers`, `rooms`, `room_types`; ghi `bookings`, `point_transactions`, `customers.Tier`, `interactions`; qua TV1: `surveys` |
| Form | frm203_nv_luu_tru (mở rộng: frm202_dm_phong) |
| Biểu mẫu | BM5 Phiếu đặt phòng / nhận–trả phòng |
| Liên phân hệ | L3: `ISurveyScheduler.ScheduleFor` (TV1 cài), chạy trong giao dịch của `StayService.CheckOut` |
| Service | `StayService` |

**Luồng chính A – Đặt phòng**
1. Lễ tân chọn khách (tìm theo tên hoặc SĐT), chọn ngày nhận, ngày trả.
2. Hệ thống kiểm tra ngày trả > ngày nhận (Q8), rồi hiện các phòng còn trống trong khoảng đó, kèm loại phòng và giá/đêm hiện tại.
3. Lễ tân chọn phòng và bấm Đặt phòng.
4. Hệ thống kiểm tra quyền (Q6), kiểm tra lại phòng còn trống (Q8; người khác có thể vừa đặt), tạo booking "Đã đặt", chốt `RatePerNight` bằng giá/đêm hiện tại của loại phòng, ghi người lập và thời điểm.
5. Hệ thống hiện booking mới trên lưới: số đêm, giá/đêm, thành tiền.

**Luồng chính B – Nhận phòng**
1. Lễ tân lọc booking theo ngày hoặc khách, chọn booking "Đã đặt", bấm Nhận phòng.
2. Hệ thống kiểm tra hôm nay nằm trong [ngày nhận, ngày trả) (Q8), chuyển booking sang "Đang ở", ghi giờ nhận phòng thực tế.

**Luồng chính C – Trả phòng (luồng L3, một giao dịch CSDL)**
1. Lễ tân chọn booking "Đang ở", bấm Trả phòng.
2. Hệ thống mở một giao dịch và lần lượt:
   1. chuyển booking sang "Đã trả phòng", ghi giờ trả phòng thực tế;
   2. tính điểm = thành tiền / 10.000đ × hệ số tích điểm của hạng khách đang có **trước khi** trả phòng, làm tròn xuống (Q9; `MembershipTier.PointsFor`); hạng mới (nếu lên hạng ở bước 4) áp dụng từ lần ở sau (quyết định ở `docs/PROGRESS.md` mục 8);
   3. ghi một dòng sổ điểm lý do Lưu trú gắn với booking (`PointTransaction.ForStay`);
   4. xét lại hạng theo tổng điểm đã tích; có thể lên nhiều bậc, không bao giờ hạ (Q9);
   5. ghi một tương tác "Trả phòng, cộng N điểm";
   6. gọi `ISurveyScheduler` (phân hệ TV1) để tạo phiếu khảo sát "Chờ" cho booking này;
   7. commit.
3. Hệ thống báo số điểm vừa cộng; nếu khách lên hạng thì báo hạng mới.

**Luồng phụ**
- A2a. Không còn phòng trống trong khoảng ngày: hệ thống báo, lễ tân đổi ngày hoặc phòng.
- D. Hủy: lễ tân chọn booking "Đã đặt", bấm Hủy; hệ thống chuyển sang "Đã hủy". Booking đã hủy không chặn phòng và không tính vào báo cáo.
- E. Danh mục phòng (mở rộng, frm202, Admin): thêm/sửa loại phòng (tên, giá/đêm > 0, sức chứa > 0) và phòng (số phòng, tầng > 0, loại). Đổi giá/đêm của loại phòng không làm đổi giá của các booking đã đặt.

**Luồng ngoại lệ** (loại ngoại lệ → thông báo tiếng Việt; dữ liệu giữ nguyên)

| Mã | Tình huống | Ngoại lệ | Thông báo (ví dụ) |
|---|---|---|---|
| E1 | CSKH gọi thao tác lưu trú (nút đã ẩn nhưng service vẫn kiểm tra, Q6) | `AuthorizationException` | "Bạn không có quyền thao tác đặt phòng, nhận phòng, trả phòng." |
| E2 | Ngày trả ≤ ngày nhận (bước A2) | `ValidationException` | "Ngày trả phòng phải sau ngày nhận phòng." |
| E3 | Phòng vừa bị người khác đặt chồng khoảng ngày (bước A4) | `BusinessRuleException` | "Phòng 305 đã có đặt phòng từ 10/12/2026 đến 12/12/2026." |
| E4 | Nhận phòng trước ngày nhận hoặc từ ngày trả trở đi (bước B2) | `BusinessRuleException` | "Chỉ nhận phòng từ ngày 10/12/2026 đến trước ngày 12/12/2026." |
| E5 | Nhận, trả hoặc hủy sai trạng thái | `BusinessRuleException` | "Đặt phòng 15 đang ở trạng thái Đã hủy, không thể nhận phòng." |
| E6 | Một bước trong C2 lỗi, kể cả bước C2.6 của phân hệ TV1 | ngoại lệ của bước lỗi (do TV1 ném, hoặc `DataAccessException`) được ném lại sau khi **rollback toàn bộ** | thông báo của ngoại lệ gốc qua `ErrorHandler`; booking vẫn "Đang ở", không có dòng sổ điểm, hạng và tương tác không đổi; lễ tân thử lại sau |

### 5.4 UC09 Đổi quà [TV2]

| Mục | Nội dung |
|---|---|
| Mã, tên | UC09 – Đổi quà |
| Tác nhân | Lễ tân; Quản lý, Admin cũng làm được. Danh mục quà (mở rộng): Admin |
| Mô tả ngắn | Cho khách dùng điểm đã tích để đổi lấy quà, đúng hạng và còn hàng; hủy được lượt đổi nhầm |
| Tiền điều kiện | Đăng nhập với vai trò Lễ tân, Quản lý hoặc Admin; khách có hồ sơ |
| Hậu điều kiện | Có một lượt đổi "Đã đổi" với điểm quy đổi đã chốt; sổ điểm có một dòng âm bằng đúng số điểm đó; kho quà giảm 1; hạng khách không đổi |
| Quy tắc | Q6, Q10 |
| Bảng dữ liệu | đọc `customers`, `point_transactions`, `rewards`; ghi `reward_redemptions`, `point_transactions`, `rewards.Quantity` |
| Form | frm205_nv_doi_qua (mở rộng: frm204_dm_qua_tang) |
| Biểu mẫu | BM6 Phiếu đổi quà |
| Liên phân hệ | không |
| Service | `LoyaltyService` |

Số dư điểm = tổng mọi dòng sổ điểm của khách. Điểm đã tích (dùng xét hạng) = tổng các dòng Lưu trú và Thiện chí. Vì vậy đổi quà làm giảm số dư nhưng không làm giảm điểm đã tích, nên không làm tụt hạng.

**Luồng chính**
1. Lễ tân chọn khách. Hệ thống hiện hạng, số dư và sổ điểm (từng dòng: ngày, lý do, +/− điểm), tính số dư ngay trên Form.
2. Hệ thống hiện danh sách quà đang áp dụng: điểm quy đổi, hạng tối thiểu, số lượng còn; đánh dấu quà khách chưa đổi được (thiếu điểm hoặc chưa đủ hạng).
3. Lễ tân chọn quà, bấm Đổi quà; hệ thống hiện số dư sau khi đổi để xác nhận.
4. Hệ thống mở một giao dịch: kiểm tra quyền (Q6); đọc lại số dư, hạng và số lượng quà; kiểm tra quà còn áp dụng, còn hàng, hạng khách ≥ hạng tối thiểu, số dư ≥ điểm quy đổi (Q10, `Reward.Redeem`); tạo lượt đổi, chốt `PointsSpent`; giảm số lượng quà; ghi dòng sổ điểm âm (`PointTransaction.ForRedemption`); commit.
5. Hệ thống cập nhật số dư và lưới lượt đổi.

**Luồng phụ**
- A. Hủy lượt đổi: lễ tân chọn lượt đổi "Đã đổi", bấm Hủy. Trong một giao dịch: lượt đổi chuyển "Đã hủy", quà trả về kho (`Reward.CancelRedemption`), ghi một dòng sổ điểm dương bằng đúng điểm đã đổi (`PointTransaction.ForRefund`). Dòng âm cũ được giữ lại: sổ điểm chỉ ghi thêm, không sửa hay xóa.
- B. Danh mục quà (mở rộng, frm204, Admin): thêm/sửa quà (tên, điểm quy đổi > 0, hạng tối thiểu, số lượng ≥ 0, còn áp dụng). Đổi điểm quy đổi không làm đổi các lượt đổi cũ.

**Luồng ngoại lệ** (không có gì được ghi, kho quà giữ nguyên; thông báo của `ValidationException`, `BusinessRuleException` lấy đúng từ code PR #2, thông báo của `AuthorizationException` là ví dụ, TV1 chốt khi viết `Employee`)

| Mã | Tình huống | Ngoại lệ | Thông báo |
|---|---|---|---|
| E1 | Quà đã ngừng áp dụng | `BusinessRuleException` | "Quà "Bữa tối cho hai người" đã ngừng áp dụng." |
| E2 | Quà đã hết, kể cả khi hai lễ tân cùng đổi món cuối (bước 4 đọc lại số lượng trong giao dịch; CSDL có thêm `CHECK (Quantity >= 0)`) | `BusinessRuleException` | "Quà "Bữa tối cho hai người" đã hết." |
| E3 | Chưa đủ hạng | `BusinessRuleException` | "Quà "Bữa tối cho hai người" dành cho hạng Gold trở lên; khách đang ở hạng Silver." |
| E4 | Thiếu điểm | `BusinessRuleException` | "Quà "Bữa tối cho hai người" cần 500 điểm; khách chỉ còn 499 điểm." |
| E5 | Hủy một lượt đã hủy | `BusinessRuleException` | "Lượt đổi quà 8 đã bị hủy trước đó." |
| E6 | CSKH đổi quà (Q6) | `AuthorizationException` | "Bạn không có quyền đổi quà." |
| E7 | Danh mục quà nhập sai (luồng B) | `ValidationException` | "Điểm quy đổi phải lớn hơn 0." / "Số lượng quà không được âm." |

### 5.5 UC10 Chiến dịch chăm sóc [TV2]

| Mục | Nội dung |
|---|---|
| Mã, tên | UC10 – Chiến dịch chăm sóc |
| Tác nhân | Quản lý (tạo chiến dịch); Lễ tân, CSKH, Quản lý (ghi phản hồi); Admin làm được cả hai |
| Mô tả ngắn | Gửi một ưu đãi tới đúng nhóm khách (phân khúc chọn lúc chạy), không làm phiền khách không đồng ý hoặc đang khiếu nại, và đo được phản hồi |
| Tiền điều kiện | Đăng nhập; tạo chiến dịch cần vai trò Quản lý hoặc Admin |
| Hậu điều kiện | Có một chiến dịch và danh sách khách nhận (ít nhất một khách), mỗi khách một dòng, đã ghi kênh và ngày gửi |
| Quy tắc | Q6, Q11 |
| Bảng dữ liệu | đọc `customers`, `bookings`; qua TV1: ticket khiếu nại chưa đóng; ghi `campaigns`, `campaign_targets` |
| Form | frm206_nv_chien_dich (Master: chiến dịch; Detail: khách nhận) |
| Biểu mẫu | BM7 Danh sách chăm sóc theo chiến dịch |
| Liên phân hệ | L4: `ITicketQuery.GetCustomersWithOpenComplaints` (TV1 cài), hỏi một lần cho cả danh sách |
| Service | `CampaignService` (Context của Strategy `ISegmentStrategy`) |

**Luồng chính A – Tạo chiến dịch**
1. Quản lý nhập tên, nội dung ưu đãi, ngày bắt đầu, ngày kết thúc, chọn kênh gửi và phân khúc trong danh sách (VIP, Lâu chưa quay lại, Sinh nhật trong tháng). Danh sách phân khúc lấy từ các Strategy đã đăng ký.
2. Quản lý bấm Xem trước. Hệ thống:
   1. đọc dữ liệu tất cả khách trong một truy vấn (hạng, ngày sinh, các ngày trả phòng) thành `SegmentCandidate`;
   2. giữ khách mà Strategy đã chọn trả lời "thuộc phân khúc" theo ngày bắt đầu chiến dịch;
   3. bỏ khách không đồng ý nhận ưu đãi;
   4. hỏi phân hệ TV1 qua `ITicketQuery` một lần cho cả danh sách: bỏ khách đang có khiếu nại chưa đóng (luồng L4).
3. Hệ thống hiện danh sách và số khách bị loại ở từng bước (không đồng ý, đang khiếu nại).
4. Quản lý bấm Lưu. Trong một giao dịch: kiểm tra quyền (Q6), tạo chiến dịch, thêm từng khách vào danh sách (`Campaign.AddTarget`: kiểm tra lại đồng ý, khiếu nại, không trùng khách), kiểm tra danh sách không rỗng (`Campaign.EnsureHasTargets`), commit.

**Luồng chính B – Ghi phản hồi (mở rộng)**
1. Nhân viên mở chiến dịch, chọn một khách trong danh sách, chọn phản hồi: Quan tâm, Đã đặt phòng, Từ chối.
2. Hệ thống kiểm tra chiến dịch chưa kết thúc (Q11; ghi được tới hết ngày kết thúc), ghi phản hồi, thời điểm và người ghi. Ghi lại lần sau thì thay phản hồi cũ (ví dụ Quan tâm → Đã đặt phòng).
3. Hệ thống cập nhật ngay số phản hồi và tỷ lệ phản hồi ở cuối danh sách.

**Luồng phụ**
- A3a. Danh sách rỗng: hệ thống báo không có khách phù hợp và không cho lưu (Q11: chiến dịch phải có ít nhất một khách; quyết định ở `docs/PROGRESS.md` mục 8); quản lý đổi phân khúc hoặc ngày.
- A3b. Quản lý đổi phân khúc rồi Xem trước lại: danh sách được tính lại từ đầu.
- A4a. Giữa lúc xem trước và lúc lưu, có khách vừa mở khiếu nại hoặc vừa rút đồng ý nhận ưu đãi: bước A4 kiểm tra lại, loại khách đó, vẫn lưu các khách còn lại và báo rõ khách nào bị loại, vì sao. Nếu loại xong danh sách rỗng thì áp dụng A3a.

**Luồng ngoại lệ** (thông báo của `ValidationException`, `BusinessRuleException` lấy đúng từ code PR #2; thông báo của `AuthorizationException` là ví dụ, TV1 chốt khi viết `Employee`)

| Mã | Tình huống | Ngoại lệ | Thông báo |
|---|---|---|---|
| E1 | Lễ tân hoặc CSKH tạo chiến dịch (Q6) | `AuthorizationException` | "Bạn không có quyền tạo chiến dịch." |
| E2 | Thiếu tên, ưu đãi hoặc phân khúc | `ValidationException` | "Tên chiến dịch không được để trống." / "Nội dung ưu đãi không được để trống." / "Phải chọn phân khúc khách hàng." |
| E3 | Ngày kết thúc trước ngày bắt đầu | `ValidationException` | "Ngày kết thúc phải bằng hoặc sau ngày bắt đầu." |
| E4 | Danh sách rỗng khi lưu (A3a) | `BusinessRuleException` | "Chiến dịch "Tri ân cuối năm" chưa có khách nào phù hợp; hãy đổi phân khúc hoặc khoảng ngày." |
| E5 | Ghi phản hồi sau ngày kết thúc | `BusinessRuleException` | "Chiến dịch "Tri ân cuối năm" đã kết thúc ngày 31/12/2026." |
| E6 | Ghi phản hồi cho khách không có trong danh sách | `BusinessRuleException` | "Khách 12 không có trong danh sách của chiến dịch này." |

Trong A4a, `CampaignService` không để `Campaign.AddTarget` ném lỗi cho khách bị loại mà hỏi `Campaign.IsEligible` trước, gom các khách bị loại vào kết quả trả về để Form báo lại.

## 6. Kịch bản demo và rà soát chức năng phân hệ TV2 [TV2]
Theo góp ý của thầy: có kịch bản demo nêu người dùng mong muốn gì, đi qua quy trình theo từng bước, và chỉ ra quy trình còn thiếu chức năng gì. Kịch bản được dựng với sự hỗ trợ của AI (Claude) từ prompt ở `02-bfd.md` mục 2.5; TV2 đối chiếu từng bước với BFD và quyết định.

### 6.1 Người dùng mong muốn gì
| Vai trò | Mong muốn | Chức năng đáp ứng |
|---|---|---|
| Lễ tân | Khách quay lại thì tìm thấy đúng một hồ sơ và thấy toàn bộ lịch sử | F5.1.4, F5.2 |
| Lễ tân | Trả phòng xong không phải tự cộng điểm, tự xét hạng | F6.2.4 (Q9, L3) |
| Lễ tân | Đổi quà thì hệ thống tự chặn khi thiếu điểm, thiếu hạng, hết quà | F7.2.2 (Q10) |
| Quản lý | Gửi ưu đãi đúng nhóm khách, không làm phiền khách đang khiếu nại, đo được hiệu quả | F8.1 (Q11, L4), F8.2.2 (R4) |
| Quản lý | Xem toàn cảnh một khách: chi tiêu, số ticket, mức hài lòng | F8.2.3 (R5, L6) |

### 6.2 Kịch bản demo (khoảng 4 phút)
Dữ liệu chuẩn bị: khách Nguyễn Văn An hạng Standard, 900 điểm đã tích; phòng 305 loại Deluxe đôi 1.250.000đ/đêm; quà "Voucher spa" 800 điểm và quà "Đêm phòng miễn phí" 2.000 điểm, cả hai hạng tối thiểu Standard.

| Bước | Ai | Thao tác | Kết quả phải thấy | Chức năng, quy tắc |
|---|---|---|---|---|
| 1 | Lễ tân | Thêm khách mới quốc tịch Việt Nam, nhập CCCD 11 số, rồi sửa thành 12 số | Lần đầu báo "Số CCCD phải gồm đúng 12 chữ số.", lần sau lưu được, hạng Standard | F5.1.1, Q7 |
| 2 | Lễ tân | Đặt phòng 305 cho khách An từ 10/12 đến 14/12, sau đó đặt thử 11/12–13/12 cho khách khác | Booking Đã đặt, giá/đêm 1.250.000đ được chốt; lần đặt thứ hai bị chặn, thông báo nêu ngày đang bị giữ | F6.2.2, Q8 |
| 3 | Lễ tân | Nhận phòng ngày 10/12, trả phòng ngày 14/12 | 4 đêm × 1.250.000 = 5.000.000đ → +500 điểm → 1.400 điểm, lên Silver; bên TV1 có phiếu khảo sát Chờ | F6.2.3, F6.2.4, Q9, L3 |
| 4 | Lễ tân | Đổi "Đêm phòng miễn phí" (2.000 điểm, thiếu điểm), rồi đổi Voucher spa, rồi hủy lượt đổi | Lần đầu bị chặn; lần hai số dư 600; hủy thì số dư về 1.400, hạng vẫn Silver | F7.2.2–F7.2.4, Q10 |
| 5 | Quản lý | Tạo chiến dịch "Sinh nhật tháng 12", Xem trước | Danh sách bỏ khách không đồng ý và khách đang có khiếu nại chưa đóng (hỏi phân hệ TV1) | F8.1.1–F8.1.3, Q11, L4 |
| 6 | CSKH | Ghi phản hồi "Đã đặt phòng" cho một khách | Tỷ lệ phản hồi ở cuối danh sách tăng ngay | F8.1.4 |
| 7 | Quản lý | Mở báo cáo hiệu quả chiến dịch, dashboard 360, xuất Excel | Số liệu khớp các bước trên; file .xlsx mở được | F8.2.2–F8.2.4 |
| 8 | Lễ tân | Thử tạo chiến dịch | Bị chặn: "Bạn không có quyền tạo chiến dịch." | Q6 |

### 6.3 Quy trình còn thiếu chức năng gì (so với hệ thống tham khảo)
| Chức năng hệ thống tham khảo có | Có ở | Phân hệ hiện tại | Quyết định |
|---|---|---|---|
| Ghi tương tác thủ công (khách gọi điện, gửi email) | HubSpot, OPERA | hồ sơ 360 chỉ có tương tác do hệ thống ghi (trả phòng, ticket đóng) | **đề xuất thêm** F5.2.5 "Ghi tương tác" trên frm201 (`Interaction` đã hỗ trợ kênh có nhân viên); chờ chốt |
| Gộp hai hồ sơ trùng của một khách | OPERA | Q7 chặn trùng SĐT, email, giấy tờ ngay lúc nhập | không làm: chặn từ đầu thay cho gộp về sau |
| Điểm hết hạn khi không hoạt động | Marriott Bonvoy, Hilton Honors | điểm không hết hạn | ngoài phạm vi; ghi ở hướng phát triển |
| Gửi email/SMS tự động | HubSpot | hệ thống lập danh sách; CSKH gửi tay, ghi kênh và phản hồi | ngoài phạm vi (cần dịch vụ gửi tin bên ngoài) |
| Khách tự xem điểm, tự đổi quà | Marriott Bonvoy, Hilton Honors | khách không đăng nhập (4W+1H) | ngoài phạm vi: app nội bộ cho nhân viên |
