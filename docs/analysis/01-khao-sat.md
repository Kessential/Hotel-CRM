# 01. Khảo sát

Module M2 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.1). Mỗi mục ghi nhãn người viết: [TV1] Chăm sóc & hỗ trợ khách hàng, [TV2] Quan hệ khách hàng & khách hàng thân thiết. Mã F, UC, BM, Q, R, L, D lấy đúng như `docs/PLAN.md`.

## 1. Đặt vấn đề và ý nghĩa [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. 4W+1H toàn hệ thống [Chung – TV1]
_[Chung – TV1] chưa điền._

## 3. Hiện trạng và nhược điểm

### 3.1 Chăm sóc & hỗ trợ khách hàng [TV1]
_[TV1] chưa điền._

### 3.2 Quan hệ khách hàng & khách hàng thân thiết [TV2]

#### Nguồn khảo sát
> Cần điền trước khi nộp: khách sạn hoặc nguồn tham khảo đã khảo sát, người được hỏi (vai trò), ngày khảo sát, cách thu thập (phỏng vấn, quan sát, xin mẫu phiếu). Nếu không khảo sát được một khách sạn thật thì ghi rõ đây là kịch bản do nhóm dựng từ tài liệu tham khảo, và nêu tên tài liệu.

| Nguồn | Người cung cấp / vai trò | Ngày | Cách thu thập |
|---|---|---|---|
| … | … | …/…/2026 | … |

#### Phạm vi phân hệ
Phân hệ quản lý mọi thứ khách sạn biết về một khách và dùng nó để giữ chân khách: hồ sơ khách 360 (F5), lưu trú làm nguồn dữ liệu cho CRM (F6), sổ điểm và đổi quà (F7), chương trình chăm sóc theo phân khúc và báo cáo khách hàng (F8). Phân hệ không quản lý hóa đơn, công nợ, doanh thu hay công suất phòng: đó là việc của phần mềm quản lý khách sạn (PMS), nằm ngoài đề CRM.

#### Khảo sát cái gì, tham khảo hệ thống nào
Nội dung khảo sát (What):
1. Quy trình nghiệp vụ hiện tại ở quầy lễ tân và bộ phận CSKH: lập hồ sơ, đặt–nhận–trả phòng, cộng điểm, đổi quà, gửi ưu đãi, ghi phản hồi (mục "Cách làm hiện tại").
2. Biểu mẫu đang dùng: BM4–BM7 (mục 4.2).
3. Quy tắc nghiệp vụ: giá phòng, cách tính điểm, ngưỡng hạng, điều kiện đổi quà, điều kiện nhận ưu đãi (Q7–Q13, `04-quy-tac.md`).
4. Báo cáo người quản lý cần: khách theo hạng, hiệu quả chương trình chăm sóc, toàn cảnh một khách (R3–R5).
5. Các hệ thống cùng loại đang có trên thị trường, để tách phần chung (hệ thống nào cũng có, nên làm) và phần riêng (không đưa vào, có lý do).

Hệ thống tham khảo (TV2 bổ sung link và ngày xem vào bảng "Nguồn khảo sát"):

| Hệ thống | Loại | Chức năng đã xem | Phần chung → đưa vào phân hệ | Phần riêng → không đưa vào (lý do) |
|---|---|---|---|---|
| Oracle OPERA Cloud (hồ sơ khách) | PMS khách sạn | hồ sơ khách, lịch sử lưu trú, ghi chú sở thích, đặt/nhận/trả phòng | hồ sơ một khách duy nhất, lịch sử lưu trú, giá chốt trên phiếu (F5, F6) | hóa đơn, công nợ, công suất phòng, kênh bán phòng: việc của PMS |
| Marriott Bonvoy, Hilton Honors | chương trình thành viên khách sạn | tích điểm theo chi tiêu, hạng thành viên, đổi điểm lấy quà | sổ điểm từng giao dịch, hạng theo điểm đã tích, đổi quà theo hạng và số dư (F7) | điểm hết hạn, tích điểm qua đối tác (hãng bay, thẻ), xét hạng theo số đêm: vượt quy mô một khách sạn |
| HubSpot CRM, Salesforce | CRM đa ngành | phân khúc danh sách, chương trình tiếp thị, ghi phản hồi, báo cáo hiệu quả, nhật ký tương tác | chọn khách theo phân khúc, danh sách chăm sóc theo chương trình, phản hồi, tỷ lệ (F8, R4); nhật ký tương tác (F5.2) | gửi email/SMS tự động, chấm điểm khách tiềm năng, quy trình bán hàng: không phải nghiệp vụ khách sạn nhỏ |

Phần chung cả ba nhóm hệ thống đều có, và phân hệ giữ lại: một hồ sơ duy nhất cho mỗi khách (khóa theo SĐT, email, giấy tờ), lịch sử tương tác, phân khúc khách, báo cáo theo khách. Những chức năng các hệ thống có mà phân hệ chưa có được rà ở `03-use-case.md` mục 6 (kịch bản demo).

#### 4W+1H của phân hệ

| Câu hỏi | Kết quả khảo sát |
|---|---|
| **What** – làm gì | Lưu hồ sơ khách và lịch sử lưu trú, tương tác; tích điểm sau mỗi lần ở, xét hạng thành viên, cho đổi điểm lấy quà; chọn nhóm khách để gửi ưu đãi và theo dõi phản hồi; báo cáo khách theo hạng, hiệu quả chương trình chăm sóc, hồ sơ 360 từng khách. |
| **Who** – ai làm | **Lễ tân**: lập/sửa hồ sơ khách, đặt phòng, nhận/trả phòng, đổi quà. **Quản lý** (trưởng bộ phận): xóa khách, tạo chương trình chăm sóc, xem báo cáo. **Nhân viên CSKH**: xem hồ sơ khách, gọi khách theo danh sách của chương trình và ghi phản hồi. **Admin**: làm được mọi việc. **Khách hàng**: người được phục vụ, không đăng nhập hệ thống. |
| **Where** – ở đâu | Quầy lễ tân (hồ sơ, lưu trú, đổi quà); văn phòng bộ phận CSKH (chương trình chăm sóc, gọi điện); văn phòng quản lý (báo cáo). Ứng dụng web chạy tại máy của khách sạn, mở bằng trình duyệt ở `http://localhost:5080`, một CSDL SQLite dùng chung. |
| **When** – khi nào | Khách đến lần đầu → lập hồ sơ. Khách đặt phòng → lập phiếu đặt. Ngày nhận phòng → nhận phòng. Khách trả phòng → cộng điểm, xét hạng, ghi tương tác, sinh phiếu khảo sát chờ cho phân hệ TV1. Khách yêu cầu đổi quà → kiểm tra điểm, hạng, kho quà. Phiếu yêu cầu của khách được đóng → cộng điểm thiện chí. Đầu mỗi đợt ưu đãi (cuối năm, tháng sinh nhật, khách lâu chưa quay lại) → lập chương trình chăm sóc. Cuối tháng → báo cáo. |
| **How** – hiện làm thế nào | Xem "Cách làm hiện tại" và "Nhược điểm" dưới đây. |

#### Cách làm hiện tại

| Nghiệp vụ | Cách làm hiện tại | Giấy tờ dùng |
|---|---|---|
| Hồ sơ khách | Lễ tân ghi thông tin khách vào sổ đăng ký lưu trú và một file Excel chung. Mỗi lần khách quay lại thì tìm trong Excel theo tên; trùng tên thì hỏi lại SĐT. | BM4 |
| Đặt phòng, nhận–trả phòng | Lập phiếu đặt phòng, ghi giá phòng theo bảng giá đang dán ở quầy. Ngày nhận phòng thì đối chiếu phiếu; ngày trả phòng thì ký xác nhận trên phiếu. | BM5 |
| Tích điểm, hạng | Sau khi trả phòng, lễ tân cộng điểm bằng tay vào cột "điểm" trong Excel. Hạng thành viên xét lại vào cuối quý. | BM5 (ô điểm), Excel |
| Đổi quà | Khách hỏi đổi quà thì lễ tân xem cột điểm, ghi phiếu đổi quà, trừ điểm bằng tay. Quà trong kho đếm riêng ở bộ phận khác. | BM6 |
| Chương trình chăm sóc | Quản lý xuất danh sách khách từ Excel, CSKH gửi tin hoặc gọi lần lượt và ghi phản hồi vào bản in danh sách. | BM7 |
| Báo cáo | Quản lý tự lọc Excel để đếm khách theo hạng; không có báo cáo hiệu quả chương trình chăm sóc. | – |

#### Nhược điểm → hệ thống giải quyết bằng gì

| # | Nhược điểm | Hậu quả | Hệ thống giải quyết bằng | Mã |
|---|---|---|---|---|
| 1 | Hồ sơ khách tìm theo tên, không ràng buộc SĐT/email/CCCD | Một khách có nhiều dòng, điểm bị chia nhỏ | Kiểm tra định dạng và không cho trùng SĐT, email, số giấy tờ | Q7, F5.1 |
| 2 | Giá phòng lấy theo bảng giá hiện tại khi tính lại | Đổi bảng giá thì tiền và điểm của các lần ở cũ bị tính sai | Giá/đêm chốt vào phiếu đặt lúc đặt (`bookings.RatePerNight`) | Q8, F6.2 |
| 3 | Phòng có thể bị ghi trùng lịch, nhận phòng trước ngày đặt | Hai khách cùng một phòng, cộng điểm khống | Chặn trùng lịch với booking chưa hủy; chỉ nhận phòng trong [ngày nhận, ngày trả) | Q8, F6.2 |
| 4 | Cộng điểm bằng tay, xét hạng theo quý | Quên cộng, cộng sai; khách đã đủ điểm vẫn chưa được lên hạng | Tự cộng điểm khi trả phòng theo hệ số của hạng, tự nâng hạng ngay, có thể lên nhiều bậc | Q9, F6.2, L3 |
| 5 | Chỉ có một cột "điểm", đổi quà thì trừ thẳng vào cột đó | Không biết điểm đến từ đâu; trừ điểm đổi quà làm khách bị tụt hạng | Sổ điểm ghi từng giao dịch (+/−, lý do); hạng tính theo tổng điểm đã tích, không trừ điểm đã đổi | Q9, Q10, F7.2 |
| 6 | Đổi quà không kiểm tra hạng và kho | Khách hạng thấp đổi quà dành cho VIP; hứa quà đã hết | Kiểm tra số dư, hạng tối thiểu, số lượng còn; điểm quy đổi chốt lúc đổi | Q10, F7.2 |
| 7 | Danh sách chăm sóc lấy cả khách không muốn nhận quảng cáo và khách đang khiếu nại | Khách phàn nàn; gửi ưu đãi cho người đang bức xúc | Chỉ lấy khách đồng ý nhận ưu đãi; bỏ khách có khiếu nại chưa đóng (hỏi phân hệ TV1) | Q11, F8.1, L4 |
| 8 | Không ghi phản hồi của khách một cách có hệ thống | Không đo được chương trình nào hiệu quả | Ghi phản hồi từng khách; báo cáo tỷ lệ phản hồi, tỷ lệ đặt phòng | Q11, F8.2, R4 |
| 9 | Thông tin khách nằm rải rác (Excel, sổ, phiếu yêu cầu) | Nhân viên không thấy toàn cảnh một khách khi tiếp | Hồ sơ 360: lưu trú, tương tác, sổ điểm, số khiếu nại, điểm hài lòng trên một màn hình | F5.2, F8.2, R5, L6 |
| 10 | Khách có khiếu nại được xử lý xong không được ghi nhận | Mất cơ hội giữ chân khách | Đóng phiếu yêu cầu → tự cộng điểm thiện chí và ghi tương tác (nhận sự kiện từ phân hệ TV1) | Q12, L2 |

## 4. Biểu mẫu thu thập

### 4.1 BM1–BM3 [TV1]
_[TV1] chưa điền._

### 4.2 BM4–BM7 [TV2]
Mỗi biểu mẫu có bảng ánh xạ ô trên phiếu → cột CSDL → ô trên trang web, để truy vết hai chiều khi vấn đáp. Cột "Trên trang" ghi tên ô của lớp InputModel (`Input.…`, dự kiến, chốt ở M8) hoặc cột, nhãn hiển thị.

Cột "Loại" trong bảng ánh xạ:
- **Nhập**: người dùng nhập hoặc chọn.
- **Tính**: hệ thống tính, không lưu (tính lại mỗi lần hiển thị).
- **Chốt**: hệ thống ghi một lần lúc lập phiếu và không đổi theo dữ liệu gốc về sau (thuộc tính lịch sử).
- **Tự ghi**: hệ thống tự ghi và lưu: thời điểm, người làm, trạng thái hoặc giá trị hệ thống tự xác định (mã, hạng, khách được chọn).

> **Mẫu dựng lại, chưa có bản thật.** Các bảng dưới đây là danh sách ô do nhóm dựng lại. Khi có phiếu thật thì đặt ảnh vào `docs/analysis/bieu-mau/` (`BM4-phieu-dang-ky-thanh-vien.png`, `BM5-phieu-dat-phong.png`, `BM6-phieu-doi-qua.png`, `BM7-danh-sach-cham-soc.png`) rồi đối chiếu và sửa bảng cho khớp.

#### BM4 – Phiếu đăng ký thành viên / hồ sơ khách
- **Mục đích:** lưu thông tin nhận diện và liên lạc của khách, làm gốc cho tích điểm và chăm sóc.
- **Ai lập:** Lễ tân. **Khi nào:** khách đến lần đầu; sửa khi khách đổi thông tin.
- **Trang:** `frm201_dm_customer`. **Bảng:** `customers`.

| Ô trên phiếu | Loại | Bảng.cột | Trên trang | Ghi chú |
|---|---|---|---|---|
| Mã khách | Tự ghi | `customers.Id` | cột "Mã KH" | |
| Họ và tên | Nhập | `customers.FullName` | `Input.FullName` | bắt buộc |
| Số điện thoại | Nhập | `customers.Phone` | `Input.Phone` | Q7: 10 số, bắt đầu bằng 0, không trùng |
| Email | Nhập | `customers.Email` | `Input.Email` | Q7: đúng định dạng, không trùng |
| Số CCCD / hộ chiếu | Nhập | `customers.IdentityNumber` | `Input.IdentityNumber` | Q7: quốc tịch Việt Nam → CCCD 12 chữ số; quốc tịch khác → hộ chiếu 6–9 ký tự chữ và số; không trùng (quyết định ở `docs/PROGRESS.md` mục 8) |
| Quốc tịch | Nhập | `customers.Nationality` | `Input.Nationality` | lọc được trên bảng |
| Ngày sinh | Nhập | `customers.DateOfBirth` | `Input.DateOfBirth` | không bắt buộc; dùng cho phân khúc sinh nhật |
| Đồng ý nhận ưu đãi | Nhập | `customers.MarketingConsent` | `Input.MarketingConsent` | Q11: không đồng ý thì không vào chương trình chăm sóc |
| Ghi chú (sở thích, dị ứng…) | Nhập | `customers.Notes` | `Input.Notes` | |
| Hạng thành viên | Tự ghi | `customers.Tier` | cột "Hạng" | Q9: hệ thống tự nâng, không nhập tay |
| Ngày đăng ký | Tự ghi | `customers.CreatedAt` | cột "Ngày đăng ký" | |
| Chữ ký khách, nhân viên lập | – | – | – | chỉ có trên giấy |

#### BM5 – Phiếu đặt phòng / nhận–trả phòng
- **Mục đích:** giữ phòng cho khách theo khoảng ngày với giá đã thỏa thuận; xác nhận lúc nhận và trả phòng.
- **Ai lập:** Lễ tân. **Khi nào:** lúc đặt phòng; ký thêm lúc nhận phòng và trả phòng.
- **Trang:** `frm203_nv_stay`. **Bảng:** `bookings` (+ `point_transactions` lúc trả phòng).

| Ô trên phiếu | Loại | Bảng.cột | Trên trang | Ghi chú |
|---|---|---|---|---|
| Số phiếu | Tự ghi | `bookings.Id` | cột "Mã ĐP" | |
| Khách hàng | Nhập | `bookings.CustomerId` | `Input.CustomerId` (ô tìm khách theo tên/SĐT) | |
| Loại phòng, số phòng | Nhập | `bookings.RoomId` → `rooms.RoomTypeId` | `Input.RoomId` | chỉ hiện phòng còn trống trong khoảng ngày (Q8) |
| Ngày nhận, ngày trả | Nhập | `bookings.CheckInDate`, `CheckOutDate` | `Input.CheckInDate`, `Input.CheckOutDate` | Q8: ngày trả > ngày nhận |
| Số đêm | Tính | – | cột "Số đêm" | ngày trả − ngày nhận |
| Đơn giá/đêm | **Chốt** | `bookings.RatePerNight` | cột "Giá/đêm (đ)" | lấy từ `room_types.PricePerNight` lúc đặt rồi giữ nguyên |
| Thành tiền | Tính | – | cột "Thành tiền (đ)" | số đêm × đơn giá/đêm đã chốt |
| Trạng thái | Tự ghi | `bookings.Status` | cột "Trạng thái" | Đã đặt / Đang ở / Đã trả phòng / Đã hủy |
| Giờ nhận phòng thực tế | Tự ghi | `bookings.CheckedInAt` | | Q8: chỉ nhận trong [ngày nhận, ngày trả) |
| Giờ trả phòng thực tế | Tự ghi | `bookings.CheckedOutAt` | | |
| Điểm được cộng | Tự ghi | `point_transactions.Points` (lý do Lưu trú) | | Q9: thành tiền / 10.000đ × hệ số hạng, làm tròn xuống |
| Nhân viên lập phiếu | Tự ghi | `bookings.CreatedById` | | |

#### BM6 – Phiếu đổi quà
- **Mục đích:** ghi nhận khách dùng điểm đổi lấy quà, làm chứng từ trừ điểm và xuất kho quà.
- **Ai lập:** Lễ tân. **Khi nào:** khách yêu cầu đổi quà tại quầy.
- **Trang:** `frm205_nv_loyalty`. **Bảng:** `reward_redemptions` (+ `point_transactions`, `rewards.Quantity`).

| Ô trên phiếu | Loại | Bảng.cột | Trên trang | Ghi chú |
|---|---|---|---|---|
| Số phiếu | Tự ghi | `reward_redemptions.Id` | cột "Mã lượt đổi" | |
| Khách hàng, hạng | Nhập / Tự ghi | `reward_redemptions.CustomerId`; `customers.Tier` | `Input.CustomerId`, nhãn hạng | hạng do hệ thống ghi khi trả phòng (Q9), ở đây chỉ hiển thị |
| Số dư điểm trước khi đổi | Tính | tổng `point_transactions.Points` của khách | ô "Số dư điểm" | |
| Quà tặng | Nhập | `reward_redemptions.RewardId` | `Input.RewardId` (chọn trong bảng quà) | Q10: còn áp dụng, còn hàng, đủ hạng tối thiểu |
| Điểm quy đổi | **Chốt** | `reward_redemptions.PointsSpent` | cột "Điểm đã đổi" | lấy từ `rewards.PointsCost` lúc đổi rồi giữ nguyên |
| Số dư sau khi đổi | Tính | – | ô "Số dư sau khi đổi" | Q10: không bao giờ âm |
| Ngày đổi | Tự ghi | `reward_redemptions.RedeemedAt` | cột "Ngày đổi" | |
| Trạng thái | Tự ghi | `reward_redemptions.Status` | cột "Trạng thái" | Đã đổi / Đã hủy; hủy thì trả quà về kho và hoàn điểm |
| Nhân viên xử lý | Tự ghi | `reward_redemptions.ProcessedById` | | |
| Chữ ký khách | – | – | – | chỉ có trên giấy |

#### BM7 – Danh sách chăm sóc theo chương trình
- **Mục đích:** danh sách khách nhận một ưu đãi và phản hồi của từng khách, để đo hiệu quả chương trình.
- **Ai lập:** Quản lý lập phần đầu; nhân viên CSKH (hoặc Lễ tân) điền phản hồi từng dòng. **Khi nào:** đầu mỗi đợt ưu đãi; phản hồi ghi tới hết ngày kết thúc.
- **Trang:** `frm206_nv_campaign` (Master: chương trình, Detail: khách nhận). **Bảng:** `campaigns`, `campaign_targets`.

| Ô trên phiếu | Loại | Bảng.cột | Trên trang | Ghi chú |
|---|---|---|---|---|
| Tên chương trình, nội dung ưu đãi | Nhập | `campaigns.Name`, `Offer` | `Input.Name`, `Input.Offer` | |
| Nhóm khách (phân khúc) | Nhập | `campaigns.SegmentCode` | `Input.SegmentCode` | chọn Strategy lúc chạy |
| Từ ngày – đến ngày | Nhập | `campaigns.StartDate`, `EndDate` | `Input.StartDate`, `Input.EndDate` | |
| Người lập | Tự ghi | `campaigns.CreatedById` | | |
| Dòng: khách, SĐT/email | Tự ghi | `campaign_targets.CustomerId` | bảng khách nhận | Q11: phân khúc ∩ đồng ý ưu đãi − có khiếu nại chưa đóng; mỗi khách một dòng. Hệ thống chọn khách và lưu `CustomerId`; SĐT, email đọc từ `customers` |
| Dòng: kênh, ngày gửi | Tự ghi | `campaign_targets.Channel`, `SentAt` | bảng khách nhận | |
| Dòng: phản hồi, ngày phản hồi, người ghi | Nhập / Tự ghi | `campaign_targets.Response`, `RespondedAt`, `RecordedById` | `Input.Response` | Q11: chỉ ghi tới hết ngày kết thúc |
| Cuối danh sách: số khách, số phản hồi, tỷ lệ | Tính | – | ô "Số khách", "Số phản hồi", "Tỷ lệ phản hồi" | tính lại ngay khi ghi phản hồi |

#### Truy vết ngược trang → biểu mẫu (ví dụ cho vấn đáp)
- **Cột "Giá/đêm" trên bảng của `frm203`** lấy từ ô "Đơn giá/đêm" của BM5, lưu ở `bookings.RatePerNight`. Không lấy thẳng `room_types.PricePerNight` vì giá phòng đổi theo mùa: nếu lấy giá hiện tại thì thành tiền và điểm của các lần ở cũ bị tính lại sai. Đây là thuộc tính lịch sử, không vi phạm 3NF vì nó là sự thật của từng lần đặt, không phụ thuộc vào loại phòng (`06-erd.md` mục 6).
- **Cột "Điểm đã đổi" trên bảng của `frm205`** lấy từ ô "Điểm quy đổi" của BM6, lưu ở `reward_redemptions.PointsSpent` (trong code: `RewardRedemption.PointsSpent`, chốt trong `Reward.Redeem`). Lý do giống trên: quà tăng giá điểm về sau thì lượt đổi cũ vẫn đúng.
- **Nhãn "Tỷ lệ phản hồi" trên `frm206`** là ô cuối danh sách của BM7, tính từ các dòng chi tiết (`Campaign.ResponseRate`), không lưu vào CSDL.

## 5. Yêu cầu chức năng

### 5.1 F1–F4 [TV1]
_[TV1] chưa điền._

### 5.2 F5–F8 [TV2]

| Mã | Tên | Mô tả | Vai trò dùng | Trang | UC | Quy tắc / báo cáo |
|---|---|---|---|---|---|---|
| F5.1 | Cập nhật hồ sơ khách | Thêm, sửa, xóa, tìm hồ sơ khách; lọc theo từ khóa, hạng, quốc tịch, đồng ý ưu đãi; xóa lọc | Lễ tân, Quản lý, Admin (xóa: Quản lý, Admin); CSKH chỉ xem | `frm201_dm_customer` | UC07 | Q7, Q13; L5 |
| F5.2 | Tra cứu hồ sơ 360 của khách | Xem lưu trú, tương tác, giao dịch điểm của khách đang chọn; tổng chi tiêu, số lần ở, số dư điểm tính ngay | mọi vai trò | `frm201_dm_customer` (trang chi tiết) | UC07 | – |
| F6.1 | Cập nhật danh mục loại phòng và phòng | Loại phòng (tên, giá/đêm, sức chứa) và phòng (số, tầng, loại) | Admin; vai trò khác xem | `frm202_dm_room` | UC08 | – |
| F6.2 | Đặt phòng, nhận phòng, trả phòng | Đặt, nhận, trả, hủy; lọc theo khoảng ngày, trạng thái, khách. Trả phòng: cộng điểm, nâng hạng, ghi tương tác, tạo phiếu khảo sát chờ, trong một giao dịch | Lễ tân, Quản lý, Admin | `frm203_nv_stay` | UC08 | Q8, Q9; L3 |
| F7.1 | Cập nhật danh mục quà tặng | Tên quà, điểm quy đổi, hạng tối thiểu, số lượng, còn áp dụng | Admin; vai trò khác xem | `frm204_dm_reward` | UC09 | – |
| F7.2 | Theo dõi sổ điểm và đổi quà | Xem sổ điểm của khách; đổi quà; hủy lượt đổi (trả quà về kho, hoàn điểm) | Lễ tân, Quản lý, Admin | `frm205_nv_loyalty` | UC09 | Q10 |
| F8.1 | Lập và theo dõi chương trình chăm sóc | Tạo chương trình, chọn phân khúc (VIP, lâu chưa quay lại, sinh nhật trong tháng), lập danh sách khách, ghi phản hồi | tạo: Quản lý, Admin; ghi phản hồi: mọi vai trò | `frm206_nv_campaign` | UC10 | Q11; L4 |
| F8.2 | Lập báo cáo khách hàng và dashboard 360 | Báo cáo khách theo hạng (có biểu đồ), hiệu quả chương trình chăm sóc, dashboard 360; xuất Excel | Quản lý, Admin | `frm207_bc_customer_360` | UC11 | R3, R4, R5; L6 |

Ngoài 8 chức năng trên, phân hệ còn một việc tự động không có trang: khi phiếu yêu cầu của khách chuyển sang Đã đóng (UC04 của TV1), cộng điểm thiện chí và ghi một tương tác, cùng giao dịch với việc đóng phiếu (Q12, L2).

#### Số liệu nghiệp vụ dùng trong phân hệ

| Số liệu | Giá trị | Nơi đặt trong code | Quy tắc |
|---|---|---|---|
| Ngưỡng điểm của hạng (tổng điểm đã tích) | Standard 0 · Silver 1.000 · Gold 5.000 · VIP 15.000 | `MembershipTier.MinPoints` (mỗi lớp hạng) | Q9 |
| Hệ số tích điểm | Standard 1,0 · Silver 1,2 · Gold 1,5 · VIP 2,0 | `MembershipTier.EarnRate` | Q9 |
| Số tiền cho 1 điểm (trước hệ số) | 10.000đ | `MembershipTier.VndPerPoint` | Q9 |
| Hệ số SLA theo hạng (TV1 dùng) | Standard 1,0 · Silver 0,9 · Gold 0,75 · VIP 0,5 | `MembershipTier.SlaFactor` | Q3 |
| Nâng mức ưu tiên theo hạng (TV1 dùng) | Standard 0 · Silver 0 · Gold 1 · VIP 2 | `MembershipTier.PriorityBoost` | Q3 |
| Điểm thiện chí khi đóng phiếu yêu cầu | khiếu nại 50 · loại khác 10 | `SupportTicket.GoodwillPoints` (TV1) | Q12 |
| Số ngày "lâu chưa quay lại" | 180 (cấu hình) | `appsettings.json` → `LapsedGuestSegment` | Q11 |

Ví dụ tính tay (Q9): khách Silver ở 2 đêm, giá chốt 1.250.000đ/đêm → thành tiền 2.500.000đ → 2.500.000 / 10.000 × 1,2 = 300 điểm.

Hệ số tích điểm do TV2 đề xuất (PLAN chỉ nêu "hệ số tích điểm của hạng", chưa có số); TV1 xem lại khi review.

## 6. Yêu cầu phi chức năng [Chung – TV1]
_[Chung – TV1] chưa điền._

## 7. Phạm vi hệ thống [Chung – TV1]
_[Chung – TV1] chưa điền._
