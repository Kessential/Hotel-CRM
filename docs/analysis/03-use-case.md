# 03. Use Case

Module M3 của P1. [TV1] viết sơ đồ Use Case tổng, đặc tả UC04, UC05; [TV2] viết đặc tả UC08, UC09, UC10.

## 1. Danh sách Use Case và sơ đồ tổng [TV1]
> TV1 viết: bảng UC01–UC11, sơ đồ `diagrams/use-case.puml`. File khung khai báo 4 tác nhân với alias `ADM`, `QL`, `LT`, `CSKH`, cho Admin tổng quát hóa ba vai trò còn lại, rồi `!include uc-tv1.iuml` và `!include uc-tv2.iuml`.

Phần TV2 của sơ đồ: `diagrams/uc-tv2.iuml`.

| UC | Tên | Tác nhân | Form | Phức hợp | Liên phân hệ |
|---|---|---|---|---|---|
| UC07 | Quản lý hồ sơ khách 360 | Lễ tân, Quản lý; CSKH chỉ xem | frm201 | | L5: hỏi `ITicketQuery` trước khi xóa |
| UC08 | Đặt phòng và lưu trú (mở rộng: danh mục phòng) | Lễ tân, Quản lý | frm202, frm203 | ✓ | L3: trả phòng gọi `ISurveyScheduler` |
| UC09 | Đổi quà (mở rộng: danh mục quà) | Lễ tân, Quản lý | frm204, frm205 | ✓ | |
| UC10 | Chiến dịch chăm sóc (mở rộng: ghi phản hồi) | tạo: Quản lý; ghi phản hồi: Lễ tân, CSKH, Quản lý | frm206 | ✓ | L4: hỏi `ITicketQuery` khiếu nại chưa đóng |
| UC11 | Báo cáo khách hàng và dashboard 360 | Quản lý | frm207 | | L6: hỏi `ICustomerCareStats` |

Admin làm được mọi Use Case (tổng quát hóa trong sơ đồ tổng), nên không ghi lại ở từng dòng.

## 2. Đặc tả UC04, UC05 [TV1]
> TV1 viết.

## 3. UC08 – Đặt phòng và lưu trú [TV2]

| Mục | Nội dung |
|---|---|
| Tác nhân chính | Lễ tân; Quản lý, Admin cũng làm được |
| Mục tiêu | Giữ phòng cho khách theo khoảng ngày; ghi nhận khách nhận phòng và trả phòng; khi trả phòng thì tích điểm cho khách và chuyển lần ở sang khảo sát hài lòng |
| Form | frm203_nv_luu_tru (mở rộng: frm202_dm_phong) |
| Service | `StayService` |
| Biểu mẫu | BM5 Phiếu đặt phòng / nhận–trả phòng |
| Quy tắc | Q6 (quyền), Q8 (ngày, trùng lịch, giá chốt, trạng thái), Q9 (tích điểm, nâng hạng) |
| Bảng dữ liệu | đọc `customers`, `rooms`, `room_types`; ghi `bookings`, `point_transactions`, `customers.Tier`, `interactions`; qua TV1: `surveys` |
| Tiền điều kiện | Người dùng đã đăng nhập với vai trò Lễ tân, Quản lý hoặc Admin; khách đã có hồ sơ (UC07) |
| Hậu điều kiện | Đặt phòng: có booking "Đã đặt" với giá/đêm đã chốt. Trả phòng: booking "Đã trả phòng", sổ điểm có thêm một dòng Lưu trú, hạng khách đã xét lại, có một tương tác và một phiếu khảo sát "Chờ" |

**Luồng chính A – Đặt phòng**
1. Lễ tân chọn khách (tìm theo tên hoặc SĐT), chọn ngày nhận, ngày trả.
2. Hệ thống kiểm tra ngày trả > ngày nhận (Q8), rồi hiện các phòng còn trống trong khoảng đó, nhóm theo loại phòng, kèm giá/đêm hiện tại.
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
   2. tính điểm = thành tiền / 10.000đ × hệ số tích điểm của hạng khách đang có **trước khi** trả phòng, làm tròn xuống (Q9; `MembershipTier.PointsFor`); hạng mới (nếu lên hạng ở bước 4) áp dụng từ lần ở sau;
   3. ghi một dòng sổ điểm lý do Lưu trú gắn với booking (`PointTransaction.ForStay`);
   4. xét lại hạng theo tổng điểm đã tích; có thể lên nhiều bậc, không bao giờ hạ (Q9);
   5. ghi một tương tác "Trả phòng, cộng N điểm";
   6. gọi `ISurveyScheduler` (phân hệ TV1) để tạo phiếu khảo sát "Chờ" cho booking này;
   7. commit.
3. Hệ thống báo số điểm vừa cộng; nếu khách lên hạng thì báo hạng mới.

**Luồng thay thế**
- A2a. Không còn phòng trống trong khoảng ngày: hệ thống báo, lễ tân đổi ngày hoặc loại phòng.
- A4a. Phòng vừa bị người khác đặt chồng: hệ thống báo phòng đã được đặt trong khoảng ngày, không tạo booking.
- D. Hủy: lễ tân chọn booking "Đã đặt", bấm Hủy; hệ thống chuyển sang "Đã hủy". Booking đã hủy không chặn phòng và không tính vào báo cáo.
- E. Danh mục phòng (mở rộng, frm202, Admin): thêm/sửa loại phòng (tên, giá/đêm > 0, sức chứa > 0) và phòng (số phòng, tầng > 0, loại). Đổi giá/đêm của loại phòng không làm đổi giá của các booking đã đặt.

**Luồng ngoại lệ**
- E1. Vai trò CSKH thao tác: hệ thống từ chối ở tầng Service (Q6) dù nút đã bị ẩn.
- E2. Nhận phòng trước ngày nhận hoặc từ ngày trả trở đi; nhận/trả/hủy sai trạng thái: báo lỗi, booking giữ nguyên (Q8).
- E3. Bước C2.6 lỗi (phân hệ TV1 không tạo được phiếu khảo sát) hoặc bất kỳ bước nào trong C2 lỗi: **rollback toàn bộ**; booking vẫn "Đang ở", không có dòng sổ điểm, hạng và tương tác không đổi. Hệ thống báo lỗi; lễ tân thử lại sau.

## 4. UC09 – Đổi quà [TV2]

| Mục | Nội dung |
|---|---|
| Tác nhân chính | Lễ tân; Quản lý, Admin cũng làm được |
| Mục tiêu | Cho khách dùng điểm đã tích để đổi lấy quà, đúng hạng và còn hàng; hủy được lượt đổi nhầm |
| Form | frm205_nv_doi_qua (mở rộng: frm204_dm_qua_tang) |
| Service | `LoyaltyService` |
| Biểu mẫu | BM6 Phiếu đổi quà |
| Quy tắc | Q6, Q10 |
| Bảng dữ liệu | đọc `customers`, `point_transactions`, `rewards`; ghi `reward_redemptions`, `point_transactions`, `rewards.Quantity` |
| Tiền điều kiện | Đăng nhập với vai trò Lễ tân, Quản lý hoặc Admin; khách có hồ sơ |
| Hậu điều kiện | Có một lượt đổi "Đã đổi" với điểm quy đổi đã chốt; sổ điểm có một dòng âm bằng đúng số điểm đó; kho quà giảm 1; hạng khách không đổi |

Số dư điểm = tổng mọi dòng sổ điểm của khách. Điểm đã tích (dùng xét hạng) = tổng các dòng Lưu trú và Thiện chí. Vì vậy đổi quà làm giảm số dư nhưng không làm giảm điểm đã tích, nên không làm tụt hạng.

**Luồng chính**
1. Lễ tân chọn khách. Hệ thống hiện hạng, số dư và sổ điểm (từng dòng: ngày, lý do, +/− điểm), tính số dư ngay trên Form.
2. Hệ thống hiện danh sách quà đang áp dụng: điểm quy đổi, hạng tối thiểu, số lượng còn; đánh dấu quà khách chưa đổi được (thiếu điểm hoặc chưa đủ hạng).
3. Lễ tân chọn quà, bấm Đổi quà; hệ thống hiện số dư sau khi đổi để xác nhận.
4. Hệ thống mở một giao dịch: kiểm tra quyền (Q6); đọc lại số dư, hạng và số lượng quà; kiểm tra quà còn áp dụng, còn hàng, hạng khách ≥ hạng tối thiểu, số dư ≥ điểm quy đổi (Q10, `Reward.Redeem`); tạo lượt đổi, chốt `PointsSpent`; giảm số lượng quà; ghi dòng sổ điểm âm (`PointTransaction.ForRedemption`); commit.
5. Hệ thống cập nhật số dư và lưới lượt đổi.

**Luồng thay thế**
- A. Hủy lượt đổi: lễ tân chọn lượt đổi "Đã đổi", bấm Hủy. Trong một giao dịch: lượt đổi chuyển "Đã hủy", quà trả về kho (`Reward.CancelRedemption`), ghi một dòng sổ điểm dương bằng đúng điểm đã đổi (`PointTransaction.ForRefund`). Dòng âm cũ được giữ lại: sổ điểm chỉ ghi thêm, không sửa hay xóa.
- B. Danh mục quà (mở rộng, frm204, Admin): thêm/sửa quà (tên, điểm quy đổi > 0, hạng tối thiểu, số lượng ≥ 0, còn áp dụng). Đổi điểm quy đổi không làm đổi các lượt đổi cũ.

**Luồng ngoại lệ**
- E1. Thiếu điểm, chưa đủ hạng, quà hết hoặc đã ngừng áp dụng: báo lý do cụ thể; không có gì được ghi, kho quà giữ nguyên.
- E2. Hai lễ tân cùng đổi món quà cuối: bước 4 đọc lại số lượng trong giao dịch, người sau nhận lỗi "quà đã hết". CSDL có thêm ràng buộc `CHECK (Quantity >= 0)`.
- E3. Hủy một lượt đã hủy: báo lỗi, không hoàn điểm lần hai.
- E4. CSKH đổi quà: bị từ chối ở tầng Service (Q6).

## 5. UC10 – Chiến dịch chăm sóc [TV2]

| Mục | Nội dung |
|---|---|
| Tác nhân chính | Quản lý (tạo chiến dịch); Lễ tân, CSKH, Quản lý (ghi phản hồi); Admin làm được cả hai |
| Mục tiêu | Gửi một ưu đãi tới đúng nhóm khách, không làm phiền khách không đồng ý hoặc đang khiếu nại, và đo được phản hồi |
| Form | frm206_nv_chien_dich (Master: chiến dịch; Detail: khách nhận) |
| Service | `CampaignService` (Context của Strategy) |
| Biểu mẫu | BM7 Danh sách chăm sóc theo chiến dịch |
| Quy tắc | Q6, Q11 |
| Bảng dữ liệu | đọc `customers`, `bookings`; qua TV1: ticket khiếu nại chưa đóng; ghi `campaigns`, `campaign_targets` |
| Tiền điều kiện | Đăng nhập; tạo chiến dịch cần vai trò Quản lý hoặc Admin |
| Hậu điều kiện | Có một chiến dịch và danh sách khách nhận, mỗi khách một dòng, đã ghi kênh và ngày gửi |

**Luồng chính A – Tạo chiến dịch**
1. Quản lý nhập tên, nội dung ưu đãi, ngày bắt đầu, ngày kết thúc, chọn kênh gửi và phân khúc trong danh sách (VIP, Lâu chưa quay lại, Sinh nhật trong tháng). Danh sách phân khúc lấy từ các Strategy đã đăng ký.
2. Quản lý bấm Xem trước. Hệ thống:
   1. đọc dữ liệu tất cả khách trong một truy vấn (hạng, ngày sinh, các ngày trả phòng) thành `SegmentCandidate`;
   2. giữ khách mà Strategy đã chọn trả lời "thuộc phân khúc" theo ngày bắt đầu chiến dịch;
   3. bỏ khách không đồng ý nhận ưu đãi;
   4. hỏi phân hệ TV1 qua `ITicketQuery` một lần cho cả danh sách: bỏ khách đang có khiếu nại chưa đóng (luồng L4).
3. Hệ thống hiện danh sách và số khách bị loại ở từng bước (không đồng ý, đang khiếu nại).
4. Quản lý bấm Lưu. Trong một giao dịch: kiểm tra quyền (Q6), tạo chiến dịch, thêm từng khách vào danh sách (`Campaign.AddTarget`: kiểm tra lại đồng ý, khiếu nại, không trùng khách), commit.

**Luồng chính B – Ghi phản hồi (mở rộng)**
1. Nhân viên mở chiến dịch, chọn một khách trong danh sách, chọn phản hồi: Quan tâm, Đã đặt phòng, Từ chối.
2. Hệ thống kiểm tra chiến dịch chưa kết thúc (Q11; ghi được tới hết ngày kết thúc), ghi phản hồi, thời điểm và người ghi. Ghi lại lần sau thì thay phản hồi cũ (ví dụ Quan tâm → Đã đặt phòng).
3. Hệ thống cập nhật ngay số phản hồi và tỷ lệ phản hồi ở cuối danh sách.

**Luồng thay thế**
- A3a. Danh sách rỗng: hệ thống báo không có khách phù hợp và không cho lưu (Q11: chiến dịch phải có ít nhất một khách); quản lý đổi phân khúc hoặc ngày.
- A3b. Quản lý đổi phân khúc rồi Xem trước lại: danh sách được tính lại từ đầu.

**Luồng ngoại lệ**
- E1. Lễ tân hoặc CSKH tạo chiến dịch: bị từ chối ở tầng Service (Q6).
- E2. Ghi phản hồi sau ngày kết thúc, hoặc cho khách không có trong danh sách: báo lỗi, không ghi.
- E3. Giữa lúc xem trước và lúc lưu, có khách vừa mở khiếu nại hoặc vừa rút đồng ý nhận ưu đãi: bước A4 kiểm tra lại, loại khách đó, vẫn lưu các khách còn lại và báo rõ khách nào bị loại, vì sao. Nếu loại xong danh sách rỗng thì áp dụng A3a.
- E4. Ngày kết thúc trước ngày bắt đầu, thiếu tên/ưu đãi/phân khúc: báo lỗi dữ liệu.

## 6. Quyết định và câu hỏi còn mở [TV2]
1. `ITicketQuery` cần một hàm theo lô cho L4, ví dụ trả tập Id khách đang có khiếu nại chưa đóng, để không hỏi từng khách (N+1). Chốt chữ ký ở M6 với TV1.
2. **Đã chốt 25/11 (TV1 đồng ý):** chiến dịch phải có ít nhất một khách (UC10 A3a); khách trở nên không đủ điều kiện giữa lúc xem trước và lúc lưu thì bị loại, các khách còn lại vẫn được lưu (UC10 E3).
3. **Đã chốt 25/11 (TV1 đồng ý):** điểm khi trả phòng tính theo hạng trước khi trả phòng; hạng mới áp dụng từ lần ở sau (UC08 C2.2). Bảng tính tay R5 và test dùng cách này.
