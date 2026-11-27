# 01. Khảo sát

Module M2 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.1). Mỗi mục ghi nhãn người viết: [TV1] Chăm sóc & hỗ trợ khách hàng, [TV2] Quan hệ khách hàng & khách hàng thân thiết. Mã F, UC, BM, Q, R, L, D lấy đúng như `docs/PLAN.md`.

## 1. Đặt vấn đề và ý nghĩa [Chung – TV1]

#### Bối cảnh
Đối tượng là một khách sạn quy mô vừa: khoảng 20 phòng thuộc 4 loại, 4 bộ phận (Lễ tân, Quan hệ khách hàng, Buồng phòng, Kỹ thuật), khoảng 10 nhân viên (đúng quy mô dữ liệu mẫu ở `docs/PLAN.md` mục 5.9). Việc thu tiền, hóa đơn, công nợ do phần mềm quản lý khách sạn và kế toán lo (ngoài phạm vi, mục 7). Điều khách sạn còn thiếu là cách **chăm sóc khách**: tiếp nhận và xử lý phản ánh, đo mức hài lòng, nhận ra khách thân thiết để giữ chân họ.

#### Vấn đề
Từ hiện trạng ở mục 3.1 và 3.2, có bốn vấn đề chính:
1. **Phản ánh của khách bị rơi rớt.** Yêu cầu, khiếu nại nhận bằng lời, qua điện thoại nội bộ, ghi vào sổ giao ca; không có mã phiếu, không có hạn xử lý, không rõ ai đang giữ việc. Quản lý chỉ biết một phản ánh bị trễ khi khách phàn nàn lần hai.
2. **Không đo được mức hài lòng.** Phiếu khảo sát giấy ít khi thu về, không gắn với lần ở nào, không được tổng hợp; khách sạn không biết mình yếu ở tiêu chí nào.
3. **Không biết ai là khách thân thiết.** Hồ sơ khách nằm trong sổ và một file Excel, điểm cộng tay, hạng xét theo quý; ưu đãi gửi tràn lan, kể cả cho khách đang khiếu nại.
4. **Hai mảng công việc không nối với nhau.** Bộ phận xử lý phản ánh không biết khách là khách VIP; lễ tân không biết khách đang có khiếu nại chưa xong; khiếu nại giải quyết xong không được ghi nhận với khách.

#### Hệ thống đề xuất
Một hệ thống CRM (quản lý dịch vụ chăm sóc khách hàng) cho khách sạn, chạy dạng web trên máy của khách sạn, gồm hai phân hệ nối với nhau qua 6 luồng liên phân hệ L1–L6 (mục 2):
- **Chăm sóc & hỗ trợ khách hàng [TV1]:** phiếu yêu cầu (khiếu nại, yêu cầu dịch vụ, sửa chữa) từ lúc tiếp nhận tới lúc đóng, có hạn SLA, phân công, nhật ký xử lý; khảo sát hài lòng sau mỗi lần ở; báo cáo CSKH. Kèm phần nền chung: phòng ban, nhân viên, tài khoản, đăng nhập, phân quyền.
- **Quan hệ khách hàng & khách hàng thân thiết [TV2]:** hồ sơ khách 360, lưu trú (nguồn dữ liệu cho CRM), sổ điểm và đổi quà, chương trình chăm sóc theo phân khúc, báo cáo khách hàng và dashboard 360.

#### Ý nghĩa
| Đối tượng | Hệ thống mang lại |
|---|---|
| Khách hàng | Phản ánh được xử lý trong hạn đã hẹn; khách hạng cao được ưu tiên; khiếu nại xong được cộng điểm thiện chí; không nhận ưu đãi khi đang có khiếu nại hoặc khi đã từ chối nhận ưu đãi |
| Nhân viên | Biết mình đang giữ phiếu nào và hạn tới đâu; không bị giao quá sức; mọi bước xử lý có nhật ký để bàn giao ca |
| Quản lý | Thấy ngay phiếu trễ hạn; có số liệu để quyết định: hiệu suất nhân viên (R1), phiếu yêu cầu & SLA (R2), khách theo hạng (R3), hiệu quả chương trình chăm sóc (R4), hồ sơ 360 từng khách (R5) |
| Khách sạn | Giữ chân khách: biết khách nào thân thiết, khách nào có dấu hiệu rời bỏ (lâu chưa quay lại, hay khiếu nại, điểm hài lòng thấp) |

#### Đối chiếu khung "Khảo sát" của HuongdanXDHTTTQL
HuongdanXDHTTTQL (slide 4–7) chia bước khảo sát thành bốn nội dung, trả lời bằng 4W+1H, và đầu ra gồm biểu mẫu, báo cáo, dữ liệu, quy trình (đầu vào của bước phân tích). File này theo đúng khung đó:

| Nội dung trong HuongdanXDHTTTQL | Mục của file này |
|---|---|
| Đặt vấn đề – ý nghĩa của hệ thống (slide 7) | 1 |
| Mô tả hệ thống cũ và các nhược điểm (slide 7) | 3.1, 3.2 |
| Đề xuất hệ thống mới và các yêu cầu (slide 7) | 1 (đề xuất), 5 (chức năng), 6 (phi chức năng) |
| Phạm vi của hệ thống mới (slide 7) | 7 |
| Đặt câu hỏi 4W+1H (slide 6) | 2 |
| Output: biểu mẫu, báo cáo, dữ liệu, quy trình (slide 5) | 2 (bảng tóm tắt), 4 (chi tiết từng biểu mẫu) |

## 2. 4W+1H toàn hệ thống [Chung – TV1]
Năm câu hỏi trả lời cho cả hệ thống. 4W+1H riêng của phân hệ quan hệ khách hàng ở mục 3.2 [TV2].

| Câu hỏi | Trả lời |
|---|---|
| **What** – quản lý gì | (1) Phiếu yêu cầu của khách (khiếu nại, yêu cầu dịch vụ, sửa chữa) từ lúc tiếp nhận tới lúc đóng: hạn SLA, phân công, nhật ký xử lý. (2) Phiếu khảo sát hài lòng sau mỗi lần ở, chấm điểm từng tiêu chí. (3) Hồ sơ khách 360 và các lần lưu trú. (4) Sổ điểm, hạng thành viên, đổi quà. (5) Chương trình chăm sóc theo phân khúc khách và phản hồi của khách. (6) Phòng ban, nhân viên, tài khoản, quyền. Kết quả cho quản lý: 5 báo cáo R1–R5, xuất Excel. Không quản lý hóa đơn, thanh toán, công nợ, doanh thu (mục 7). |
| **Who** – ai dùng | 4 vai trò đăng nhập (`docs/PLAN.md` mục 4.1). **Admin**: quản trị tài khoản, danh mục; làm được mọi nghiệp vụ. **Quản lý** (trưởng bộ phận): phân công, đóng/mở lại phiếu yêu cầu, xóa khách, tạo chương trình chăm sóc, xem báo cáo. **Lễ tân**: hồ sơ khách, đặt/nhận/trả phòng, đổi quà, lập phiếu yêu cầu, ghi khảo sát. **Nhân viên CSKH** (nhân viên các bộ phận Quan hệ khách hàng, Buồng phòng, Kỹ thuật): xử lý phiếu được giao, ghi khảo sát, gọi khách theo chương trình chăm sóc. **Khách hàng** là tác nhân ngoài: nêu yêu cầu, trả lời khảo sát, nhận ưu đãi; không đăng nhập hệ thống. |
| **Where** – ở đâu | Quầy lễ tân: tiếp nhận phản ánh, hồ sơ khách, lưu trú, đổi quà, ghi khảo sát lúc trả phòng. Bộ phận Quan hệ khách hàng, Buồng phòng, Kỹ thuật: nhận và xử lý phiếu được giao. Văn phòng quản lý: phân công, đóng phiếu, lập chương trình chăm sóc, xem báo cáo. Hệ thống là ứng dụng web chạy trên một máy của khách sạn, mở bằng trình duyệt ở `http://localhost:5080`, dữ liệu trong một file SQLite (mục 6). Trong phạm vi bài tập, các vai trò dùng chung máy đó, mỗi người đăng nhập bằng tài khoản riêng; truy cập từ máy khác nằm ngoài phạm vi (mục 7). |
| **When** – khi nào | Khách phản ánh hoặc nhân viên phát hiện sự cố → lập phiếu yêu cầu, tính hạn SLA ngay (L1) → phân công → xử lý trong hạn → giải quyết → Quản lý đóng phiếu, khách được cộng điểm thiện chí (L2); khách phản ánh lại trong `ReopenDays` ngày (mặc định 7) sau khi giải quyết → mở lại. Khách đến lần đầu → lập hồ sơ. Đặt phòng, ngày nhận phòng, ngày trả phòng → ghi lưu trú; trả phòng → cộng điểm, xét hạng, tạo phiếu khảo sát Chờ (L3); lúc trả phòng hoặc khi gọi lại sau đó → ghi khảo sát. Khách yêu cầu đổi quà → đổi quà. Đầu mỗi đợt ưu đãi → lập chương trình chăm sóc (L4). Cuối tháng → báo cáo R1–R5. |
| **How** – hiện làm thế nào | Bằng giấy, Excel và điện thoại: phản ánh ghi vào sổ giao ca hoặc phiếu yêu cầu giấy (BM1), chuyển việc bằng điện thoại nội bộ, theo dõi trên phiếu theo dõi xử lý (BM2); khảo sát bằng phiếu giấy (BM3); hồ sơ khách, điểm, hạng trong sổ đăng ký và một file Excel chung (BM4); phiếu đặt phòng, phiếu đổi quà bằng giấy (BM5, BM6); danh sách chăm sóc in ra giấy (BM7); báo cáo tự đếm cuối tháng. Chi tiết và nhược điểm ở mục 3.1 [TV1], 3.2 [TV2]. Cách làm mới: chức năng ở mục 5 và sáu quy trình L1–L6 dưới đây. |

#### Đầu ra của khảo sát
Theo HuongdanXDHTTTQL (slide 5), khảo sát cho ra bốn thứ làm đầu vào của bước phân tích: biểu mẫu, báo cáo, dữ liệu, quy trình. Mã lấy đúng như `docs/PLAN.md` (mục 4.4, 5.1, 5.2, 5.7) và kho dữ liệu D1–D11 của `docs/tasks/v2-p1-analysis.md` mục 1.5.

**A. Biểu mẫu (7)** – chi tiết ô → cột → trang ở mục 4.

| Mã | Biểu mẫu | Ai lập | Trang | Bảng | Mục |
|---|---|---|---|---|---|
| BM1 | Phiếu yêu cầu/khiếu nại | Lễ tân (hoặc nhân viên nhận phản ánh) | `frm102_nv_ticket_create` | `tickets` | 4.1 [TV1] |
| BM2 | Phiếu theo dõi xử lý | nhân viên được giao; Quản lý | `frm104_nv_ticket_process` | `tickets`, `ticket_logs` | 4.1 [TV1] |
| BM3 | Phiếu khảo sát hài lòng | khách chấm; Lễ tân hoặc CSKH ghi vào hệ thống | `frm105_nv_survey` | `surveys`, `survey_answers` | 4.1 [TV1] |
| BM4 | Phiếu đăng ký thành viên/hồ sơ khách | Lễ tân | `frm201_dm_customer` | `customers` | 4.2 [TV2] |
| BM5 | Phiếu đặt phòng/nhận–trả phòng | Lễ tân | `frm203_nv_stay` | `bookings` | 4.2 [TV2] |
| BM6 | Phiếu đổi quà | Lễ tân | `frm205_nv_loyalty` | `reward_redemptions` | 4.2 [TV2] |
| BM7 | Danh sách chăm sóc theo chương trình | Quản lý; CSKH ghi phản hồi | `frm206_nv_campaign` | `campaigns`, `campaign_targets` | 4.2 [TV2] |

**B. Báo cáo (5)** – đều xuất Excel có tiêu đề, ngày xuất, dòng tổng; không tính lưu trú đã hủy (`docs/PLAN.md` mục 5.7).

| Mã | Báo cáo | Trang | Biểu đồ | Lấy từ kho | Hiện nay |
|---|---|---|---|---|---|
| R1 | Hiệu suất nhân viên CSKH: số phiếu được giao, đã giải quyết, giờ xử lý trung bình, số phiếu trễ SLA | `frm106_bc_customer_care` | – | D1, D2 | không có; quản lý đoán qua sổ giao ca |
| R2 | Phiếu yêu cầu & SLA theo loại, tháng và hạng khách | `frm106_bc_customer_care` | ✓ | D2; hạng khách lấy theo lô từ D5 qua `ICustomerDirectory` | đếm tay số khiếu nại cuối tháng, không có hạn xử lý để so |
| R3 | Phân khúc khách theo hạng: số khách, tổng chi tiêu, điểm đã tích/đổi | `frm207_bc_customer_360` | ✓ | D5, D8, D9 | tự lọc Excel (mục 3.2) |
| R4 | Hiệu quả chương trình chăm sóc: số khách nhận, số phản hồi, tỷ lệ quan tâm/đặt phòng | `frm207_bc_customer_360` | – | D11 | không có (mục 3.2) |
| R5 | Dashboard 360 theo khách: số lần ở, chi tiêu, số khiếu nại, điểm hài lòng trung bình | `frm207_bc_customer_360` | – | D5, D8; D2, D4 qua `ICustomerCareStats` | không có; thông tin nằm rải rác (mục 3.2) |

**C. Dữ liệu (11 kho, 17 bảng)** – dùng chung tên cho DFD của hai người.

| Mã | Kho | Bảng | Nguồn ghi | Chủ |
|---|---|---|---|---|
| D1 | Nhân viên & phòng ban | `departments`, `employees` | Admin nhập ở `frm101_dm_staff` | TV1 |
| D2 | Phiếu yêu cầu | `tickets` | BM1, BM2 | TV1 |
| D3 | Nhật ký xử lý | `ticket_logs` | BM2 | TV1 |
| D4 | Khảo sát | `survey_criteria`, `surveys`, `survey_answers` | BM3; phiếu Chờ tạo tự động khi trả phòng (L3) | TV1 |
| D5 | Khách hàng | `customers` | BM4 | TV2 |
| D6 | Tương tác | `interactions` | tự ghi khi trả phòng (L3) và khi đóng phiếu yêu cầu (L2) | TV2 |
| D7 | Phòng | `room_types`, `rooms` | Admin nhập ở `frm202_dm_room` | TV2 |
| D8 | Lưu trú | `bookings` | BM5 | TV2 |
| D9 | Sổ điểm | `point_transactions` | BM5 (trả phòng), BM6, điểm thiện chí (L2) | TV2 |
| D10 | Quà tặng & lượt đổi | `rewards`, `reward_redemptions` | Admin nhập quà ở `frm204_dm_reward`; BM6 | TV2 |
| D11 | Chương trình chăm sóc | `campaigns`, `campaign_targets` | BM7 | TV2 |

**D. Quy trình (6 luồng liên phân hệ)** – `docs/PLAN.md` mục 4.4; quy trình trong từng phân hệ ở mục 3.1, 3.2.

| Mã | Quy trình | Bắt đầu khi | Diễn biến | Interface | Biểu mẫu |
|---|---|---|---|---|---|
| L1 | Tạo phiếu yêu cầu | khách phản ánh | TV1 hỏi TV2: hạng khách (nâng mức ưu tiên, hệ số SLA); với phiếu sửa chữa, khách có đang ở đúng phòng ghi trên phiếu không | `ICustomerDirectory`, `IStayLookup` | BM1 |
| L2 | Đóng phiếu yêu cầu | Quản lý đóng phiếu đã giải quyết | TV1 → TV2, **một transaction**: phiếu Đã đóng, ghi nhật ký → cộng điểm thiện chí, ghi tương tác → commit; lỗi ở bước TV2 thì rollback cả việc đóng | `ITicketObserver` | BM2 |
| L3 | Trả phòng | khách trả phòng | TV2 → TV1, **một transaction**: lưu trú Đã trả phòng → cộng điểm, nâng hạng, ghi tương tác → tạo phiếu khảo sát Chờ → commit; lỗi ở bước TV1 thì rollback cả việc trả phòng | `ISurveyScheduler` | BM5 → BM3 |
| L4 | Chọn khách cho chương trình chăm sóc | Quản lý lập chương trình | TV2 hỏi TV1: bỏ khách đang có khiếu nại chưa đóng | `ITicketQuery` | BM7 |
| L5 | Xóa khách | Quản lý hoặc Admin xóa hồ sơ | TV2 hỏi TV1: khách đã có phiếu yêu cầu thì không cho xóa | `ITicketQuery` | BM4 |
| L6 | Dashboard 360 | mở báo cáo R5 | TV2 hỏi TV1: số phiếu, số khiếu nại, điểm hài lòng trung bình theo khách (lấy theo lô) | `ICustomerCareStats` | – |

## 3. Hiện trạng và nhược điểm

### 3.1 Chăm sóc & hỗ trợ khách hàng [TV1]

#### Nguồn khảo sát
> Cần điền trước khi nộp (giống mục 3.2): nguồn đã khảo sát cho phần chăm sóc khách hàng, người được hỏi (vai trò), ngày, cách thu thập. Nếu không khảo sát được một khách sạn thật thì ghi rõ quy trình dưới đây là kịch bản do nhóm dựng và nêu tên tài liệu tham khảo.

| Nguồn | Người cung cấp / vai trò | Ngày | Cách thu thập |
|---|---|---|---|
| … | … | …/…/2026 | … |

#### Phạm vi phân hệ
Phân hệ theo dõi mọi yêu cầu của khách từ lúc tiếp nhận tới lúc đóng (F2, F3), đo mức hài lòng sau mỗi lần ở (F4.1) và báo cáo chất lượng chăm sóc (F4.2). TV1 viết luôn phần nền chung: phòng ban, nhân viên, tài khoản, đăng nhập, phân quyền (F1). Phân hệ không giữ hồ sơ khách, lưu trú hay điểm: cần thông tin đó thì hỏi phân hệ TV2 qua interface (L1); ngược lại phân hệ TV2 hỏi phân hệ này về phiếu yêu cầu và khảo sát (L3–L6).

#### Bộ phận liên quan
| Bộ phận | Vai trò trong hệ thống | Loại phiếu yêu cầu xử lý |
|---|---|---|
| Lễ tân | Lễ tân | tiếp nhận mọi loại; không xử lý |
| Quan hệ khách hàng | Nhân viên CSKH; trưởng bộ phận là Quản lý | Khiếu nại (thái độ phục vụ, ồn, tính sai tiền…) |
| Buồng phòng | Nhân viên CSKH | Yêu cầu dịch vụ (dọn phòng, thêm khăn, gối, đồ dùng…) |
| Kỹ thuật | Nhân viên CSKH | Sửa chữa (điện, nước, điều hòa, khóa cửa…) |

#### Cách làm hiện tại
| Bước | Cách làm hiện tại | Ai làm | Giấy tờ |
|---|---|---|---|
| Tiếp nhận | Khách nói ở quầy hoặc gọi điện nội bộ; lễ tân ghi vào sổ giao ca, việc lớn thì lập phiếu yêu cầu giấy (loại, phòng, nội dung, giờ nhận). Mức độ gấp do người nhận tự đánh giá; không phân biệt khách quen hay khách mới. | Lễ tân | BM1, sổ giao ca |
| Chuyển việc | Lễ tân gọi điện hoặc nhắn nhóm chat của bộ phận (Buồng phòng, Kỹ thuật); khiếu nại báo trưởng bộ phận Quan hệ khách hàng. Ai rảnh thì nhận; không ghi tên người nhận, không hẹn giờ xong. | Lễ tân, trưởng bộ phận | điện thoại, nhóm chat |
| Xử lý | Người nhận việc làm xong thì báo miệng về quầy; việc kéo dài thì ghi vào phiếu theo dõi xử lý (giờ, người làm, việc đã làm). | nhân viên bộ phận | BM2 |
| Đóng | Trưởng bộ phận ký phiếu theo dõi cuối ca. Khách phàn nàn lại thì lập phiếu mới. | trưởng bộ phận | BM2 |
| Khảo sát | Phiếu khảo sát giấy để trong phòng hoặc đưa lúc trả phòng; khách tự điền, bỏ vào hộp góp ý. Cuối tháng quản lý đọc và đếm tay. | khách, Lễ tân, Quản lý | BM3 |
| Báo cáo | Cuối tháng quản lý đếm số khiếu nại trong sổ giao ca; không có số liệu theo nhân viên hay theo hạn xử lý. | Quản lý | – |

#### Nhược điểm → hệ thống giải quyết bằng gì

| # | Nhược điểm | Hậu quả | Hệ thống giải quyết bằng | Mã |
|---|---|---|---|---|
| 1 | Phản ánh ghi rải rác (sổ giao ca, điện thoại, nhóm chat), không có mã phiếu | Yêu cầu bị quên khi giao ca; không tra lại được khách đã phản ánh gì | Mỗi phản ánh là một phiếu yêu cầu có mã, loại, khách, phòng, người lập; tra cứu và lọc theo trạng thái, ưu tiên, loại, nhân viên, khoảng ngày, từ khóa | F2.1, F3.1 |
| 2 | Không có hạn xử lý; mức gấp theo cảm tính; khách hạng cao chờ như mọi khách | Khách VIP phải chờ; quản lý không biết phiếu nào đã trễ | Hạn SLA = giờ chuẩn theo loại × hệ số ưu tiên × hệ số hạng khách, tính ngay lúc lập và chốt vào phiếu; hạng cao được nâng mức ưu tiên; lọc riêng phiếu trễ SLA | Q3, F2.1, F3.1, L1 |
| 3 | Chuyển việc bằng lời, không ghi ai nhận; việc dồn vào người hay nhận | Có phiếu không ai giữ; một người ôm quá nhiều việc nên trễ cả loạt | Phân công ghi tên người được giao; mỗi nhân viên giữ không quá `MaxActiveTickets` (mặc định 5) phiếu đang mở; phân công tự động chọn người ít việc nhất trong bộ phận xử lý loại phiếu đó | Q2, F3.1 |
| 4 | Trạng thái không rõ: báo "xong" mà không ghi cách giải quyết; phiếu đã ký vẫn bị sửa; khách phàn nàn lại thì lập phiếu mới | Không biết đã làm gì; mất lịch sử; số khiếu nại bị đếm trùng | Vòng đời Mới → Đang xử lý → Đã giải quyết → Đã đóng; giải quyết bắt buộc có ghi chú; mở lại trong `ReopenDays` (mặc định 7) ngày; Đã đóng không đổi được; mỗi lần đổi trạng thái ghi nhật ký | Q1, F3.2 |
| 5 | Phiếu sửa chữa ghi sai số phòng hoặc ghi phòng khách đã trả | Thợ lên nhầm phòng, làm phiền khách khác | Phiếu sửa chữa phải gắn đúng phòng khách đang ở (hỏi phân hệ TV2) | Q4, F2.1, L1 |
| 6 | Ai cũng ghi, sửa được sổ; không biết ai làm bước nào; nhân viên làm thay việc của người khác | Không truy được trách nhiệm | Đăng nhập bằng tài khoản riêng; quyền theo vai trò kiểm ở tầng Service; chỉ người được giao (hoặc Admin) bắt đầu, giải quyết; chỉ Quản lý, Admin phân công, đóng, mở lại; nhật ký ghi người làm | Q6, F1.1, F1.2, F3.2 |
| 7 | Phiếu khảo sát giấy không ghi lần ở, bỏ trống tiêu chí, chấm ngoài thang; một lần ở có thể có nhiều phiếu hoặc không phiếu nào | Không tổng hợp được; không biết khách nào chưa được hỏi | Trả phòng tự tạo một phiếu khảo sát Chờ gắn với lần ở đó; mỗi lưu trú một phiếu; phải chấm đủ mọi tiêu chí đang dùng, điểm 1–5 | Q5, F4.1, L3 |
| 8 | Báo cáo đếm tay cuối tháng, chỉ có tổng số khiếu nại | Không biết nhân viên nào chậm, loại phản ánh nào nhiều, khách hạng nào hay bị trễ | Báo cáo hiệu suất nhân viên (R1) và phiếu yêu cầu & SLA theo loại, tháng, hạng khách có biểu đồ (R2); xuất Excel | F4.2, R1, R2 |
| 9 | Thông tin chăm sóc chỉ nằm ở bộ phận xử lý | Ưu đãi gửi cho khách đang khiếu nại; hồ sơ khách không có số khiếu nại, điểm hài lòng; khiếu nại xong không ai ghi nhận với khách | Phân hệ này báo cho phân hệ TV2 qua interface: đóng phiếu → điểm thiện chí, tương tác; khách có khiếu nại chưa đóng không; thống kê CSKH theo khách | L2, L4, L5, L6 (Q11, Q12, Q13 do TV2 cài) |

### 3.2 Quan hệ khách hàng & khách hàng thân thiết [TV2]

#### Nguồn khảo sát
> Cần điền trước khi nộp: khách sạn hoặc nguồn tham khảo đã khảo sát, người được hỏi (vai trò), ngày khảo sát, cách thu thập (phỏng vấn, quan sát, xin mẫu phiếu). Nếu không khảo sát được một khách sạn thật thì ghi rõ đây là kịch bản do nhóm dựng từ tài liệu tham khảo, và nêu tên tài liệu.

| Nguồn | Người cung cấp / vai trò | Ngày | Cách thu thập |
|---|---|---|---|
| … | … | …/…/2026 | … |

#### Phạm vi phân hệ
Phân hệ quản lý mọi thứ khách sạn biết về một khách và dùng nó để giữ chân khách: hồ sơ khách 360 (F5), lưu trú làm nguồn dữ liệu cho CRM (F6), sổ điểm và đổi quà (F7), chiến dịch chăm sóc theo phân khúc và báo cáo khách hàng (F8). Phân hệ không quản lý hóa đơn, công nợ, doanh thu hay công suất phòng: đó là việc của phần mềm quản lý khách sạn (PMS), nằm ngoài đề CRM.

#### 4W+1H của phân hệ

| Câu hỏi | Kết quả khảo sát |
|---|---|
| **What** – làm gì | Lưu hồ sơ khách và lịch sử lưu trú, tương tác; tích điểm sau mỗi lần ở, xét hạng thành viên, cho đổi điểm lấy quà; chọn nhóm khách để gửi ưu đãi và theo dõi phản hồi; báo cáo khách theo hạng, hiệu quả chiến dịch, hồ sơ 360 từng khách. |
| **Who** – ai làm | **Lễ tân**: lập/sửa hồ sơ khách, đặt phòng, nhận/trả phòng, đổi quà. **Quản lý** (trưởng bộ phận): xóa khách, tạo chiến dịch, xem báo cáo. **Nhân viên CSKH**: xem hồ sơ khách, gọi khách theo danh sách chiến dịch và ghi phản hồi. **Admin**: làm được mọi việc. **Khách hàng**: người được phục vụ, không đăng nhập hệ thống. |
| **Where** – ở đâu | Quầy lễ tân (hồ sơ, lưu trú, đổi quà); văn phòng bộ phận CSKH (chiến dịch, gọi điện); văn phòng quản lý (báo cáo). Phần mềm desktop nội bộ, một CSDL dùng chung. |
| **When** – khi nào | Khách đến lần đầu → lập hồ sơ. Khách đặt phòng → lập phiếu đặt. Ngày nhận phòng → nhận phòng. Khách trả phòng → cộng điểm, xét hạng, ghi tương tác, sinh phiếu khảo sát chờ cho phân hệ TV1. Khách yêu cầu đổi quà → kiểm tra điểm, hạng, kho quà. Ticket của khách được đóng → cộng điểm thiện chí. Đầu mỗi đợt ưu đãi (cuối năm, tháng sinh nhật, khách lâu chưa quay lại) → lập chiến dịch. Cuối tháng → báo cáo. |
| **How** – hiện làm thế nào | Xem "Cách làm hiện tại" và "Nhược điểm" dưới đây. |

#### Cách làm hiện tại

| Nghiệp vụ | Cách làm hiện tại | Giấy tờ dùng |
|---|---|---|
| Hồ sơ khách | Lễ tân ghi thông tin khách vào sổ đăng ký lưu trú và một file Excel chung. Mỗi lần khách quay lại thì tìm trong Excel theo tên; trùng tên thì hỏi lại SĐT. | BM4 |
| Đặt phòng, nhận–trả phòng | Lập phiếu đặt phòng, ghi giá phòng theo bảng giá đang dán ở quầy. Ngày nhận phòng thì đối chiếu phiếu; ngày trả phòng thì ký xác nhận trên phiếu. | BM5 |
| Tích điểm, hạng | Sau khi trả phòng, lễ tân cộng điểm bằng tay vào cột "điểm" trong Excel. Hạng thành viên xét lại vào cuối quý. | BM5 (ô điểm), Excel |
| Đổi quà | Khách hỏi đổi quà thì lễ tân xem cột điểm, ghi phiếu đổi quà, trừ điểm bằng tay. Quà trong kho đếm riêng ở bộ phận khác. | BM6 |
| Chiến dịch chăm sóc | Quản lý xuất danh sách khách từ Excel, CSKH gửi tin hoặc gọi lần lượt và ghi phản hồi vào bản in danh sách. | BM7 |
| Báo cáo | Quản lý tự lọc Excel để đếm khách theo hạng; không có báo cáo hiệu quả chiến dịch. | – |

#### Nhược điểm → hệ thống giải quyết bằng gì

| # | Nhược điểm | Hậu quả | Hệ thống giải quyết bằng | Mã |
|---|---|---|---|---|
| 1 | Hồ sơ khách tìm theo tên, không ràng buộc SĐT/email/CCCD | Một khách có nhiều dòng, điểm bị chia nhỏ | Kiểm tra định dạng và không cho trùng SĐT, email, số giấy tờ | Q7, F5.1 |
| 2 | Giá phòng lấy theo bảng giá hiện tại khi tính lại | Đổi bảng giá thì tiền và điểm của các lần ở cũ bị tính sai | Giá/đêm chốt vào phiếu đặt lúc đặt (`bookings.RatePerNight`) | Q8, F6.2 |
| 3 | Phòng có thể bị ghi trùng lịch, nhận phòng trước ngày đặt | Hai khách cùng một phòng, cộng điểm khống | Chặn trùng lịch với booking chưa hủy; chỉ nhận phòng trong [ngày nhận, ngày trả) | Q8, F6.2 |
| 4 | Cộng điểm bằng tay, xét hạng theo quý | Quên cộng, cộng sai; khách đã đủ điểm vẫn chưa được lên hạng | Tự cộng điểm khi trả phòng theo hệ số của hạng, tự nâng hạng ngay, có thể lên nhiều bậc | Q9, F6.2, L3 |
| 5 | Chỉ có một cột "điểm", đổi quà thì trừ thẳng vào cột đó | Không biết điểm đến từ đâu; trừ điểm đổi quà làm khách bị tụt hạng | Sổ điểm ghi từng giao dịch (+/−, lý do); hạng tính theo tổng điểm đã tích, không trừ điểm đã đổi | Q9, Q10, F7.2 |
| 6 | Đổi quà không kiểm tra hạng và kho | Khách hạng thấp đổi quà dành cho VIP; hứa quà đã hết | Kiểm tra số dư, hạng tối thiểu, số lượng còn; điểm quy đổi chốt lúc đổi | Q10, F7.2 |
| 7 | Danh sách chiến dịch lấy cả khách không muốn nhận quảng cáo và khách đang khiếu nại | Khách phàn nàn; gửi ưu đãi cho người đang bức xúc | Chỉ lấy khách đồng ý nhận ưu đãi; bỏ khách có khiếu nại chưa đóng (hỏi phân hệ TV1) | Q11, F8.1, L4 |
| 8 | Không ghi phản hồi chiến dịch theo hệ thống | Không đo được chiến dịch nào hiệu quả | Ghi phản hồi từng khách; báo cáo tỷ lệ phản hồi, tỷ lệ đặt phòng | Q11, F8.2, R4 |
| 9 | Thông tin khách nằm rải rác (Excel, sổ, ticket) | Nhân viên không thấy toàn cảnh một khách khi tiếp | Hồ sơ 360: lưu trú, tương tác, sổ điểm, số khiếu nại, điểm hài lòng trên một màn hình | F5.2, F8.2, R5, L6 |
| 10 | Khách có khiếu nại được xử lý xong không được ghi nhận | Mất cơ hội giữ chân khách | Ticket đóng → tự cộng điểm thiện chí và ghi tương tác (nhận sự kiện từ phân hệ TV1) | Q12, L2 |

## 4. Biểu mẫu thu thập

### 4.1 BM1–BM3 [TV1]
Mỗi biểu mẫu có bản dựng lại của phiếu giấy và bảng ánh xạ ô trên phiếu → cột CSDL → ô trên trang web, để truy vết hai chiều khi vấn đáp. Cột "Loại" dùng chung chú giải với mục 4.2: **Nhập** (người dùng nhập/chọn), **Tính** (hệ thống tính, không lưu), **Chốt** (ghi một lần lúc lập, không đổi theo dữ liệu gốc về sau), **Tự ghi** (hệ thống ghi thời điểm, người làm hoặc trạng thái). Cột "Trên trang" ghi tên ô của lớp InputModel (`Input.…`, dự kiến, chốt ở M8) hoặc cột/nhãn hiển thị. Ô ghi "ô mới" là ô phiếu giấy chưa có, hệ thống thêm để giải quyết nhược điểm ở mục 3.1.

> **Mẫu dựng lại, chưa có bản thật.** Phiếu và bảng dưới đây do nhóm dựng lại. Khi có phiếu thật thì đặt ảnh vào `docs/analysis/bieu-mau/` (`BM1-phieu-yeu-cau.png`, `BM2-phieu-theo-doi-xu-ly.png`, `BM3-phieu-khao-sat.png`) rồi đối chiếu và sửa bảng cho khớp.

#### BM1 – Phiếu yêu cầu/khiếu nại
- **Mục đích:** ghi một phản ánh hoặc yêu cầu của khách để chuyển cho bộ phận xử lý và theo dõi tới khi xong.
- **Ai lập:** Lễ tân, hoặc bất kỳ nhân viên nào nhận phản ánh (UC02: mọi vai trò). **Khi nào:** ngay khi khách phản ánh tại quầy, qua điện thoại nội bộ, hoặc khi nhân viên phát hiện sự cố.
- **Trang:** `frm102_nv_ticket_create` (F2.1, UC02). **Bảng:** `tickets`.

Bản dựng lại:
```
PHIẾU YÊU CẦU / KHIẾU NẠI                                  Số: ..........
Ngày giờ nhận: ..../..../........ ....:....    Người nhận: .....................
Khách hàng: ...........................  SĐT: ................  Phòng: .......
Loại:    [ ] Khiếu nại    [ ] Yêu cầu dịch vụ    [ ] Sửa chữa
Mức độ:  [ ] Thấp    [ ] Trung bình    [ ] Cao    [ ] Khẩn
Tiêu đề: ......................................................................
Nội dung: .....................................................................
Chuyển cho bộ phận: ...........................
Khách ký                                                   Người nhận ký
```

| Ô trên phiếu | Loại | Bảng.cột | Trên trang | Ghi chú |
|---|---|---|---|---|
| Số phiếu | Tự ghi | `tickets.Id` | toast sau khi lưu; cột "Mã" ở `frm103` | |
| Ngày giờ nhận | Tự ghi | `tickets.CreatedAt` | cột "Ngày lập" ở `frm103` | lấy từ `IClock` |
| Người nhận | Tự ghi | `tickets.CreatedById` | – | nhân viên đang đăng nhập (`actor`) |
| Khách hàng, SĐT | Nhập | `tickets.CustomerId` → `customers` | `Input.CustomerId` (ô tìm khách theo tên/SĐT) | tìm qua `ICustomerDirectory` (L1); SĐT không lưu lại trong `tickets` |
| Hạng khách | Tính | – (hạng của khách, lấy qua `ICustomerDirectory`) | nhãn hạng cạnh tên khách | ô mới; dùng cho Q3 |
| Phòng | Nhập | `tickets.RoomId` → `rooms` | `Input.RoomId` | bắt buộc với Sửa chữa; Q4: khách phải đang ở phòng này (`IStayLookup`, L1) |
| Loại | Nhập | `tickets.Type` | `Input.Type` | quyết định lớp con được tạo (Factory Method), giờ chuẩn SLA và bộ phận xử lý |
| Mức độ | Nhập | `tickets.Priority` | `Input.Priority` | Q3: hệ thống nâng theo hạng khách (Gold +1, VIP +2 bậc), không vượt Khẩn; lưu mức đã nâng |
| Tiêu đề | Nhập | `tickets.Title` | `Input.Title` | bắt buộc |
| Nội dung | Nhập | `tickets.Description` | `Input.Description` | |
| Hạn xử lý (SLA) | **Chốt** | `tickets.DueAt` | hiện ngay trên trang trước khi lưu; cột "Hạn SLA" ở `frm103` | ô mới; Q3: giờ lập + giờ chuẩn × hệ số ưu tiên × hệ số hạng |
| Chuyển cho bộ phận | Tính | – (suy từ loại) | – | Khiếu nại → Quan hệ khách hàng; Yêu cầu dịch vụ → Buồng phòng; Sửa chữa → Kỹ thuật |
| Người được giao | Nhập | `tickets.AssignedAgentId` | phân công ở `frm103` | ô mới; Q2; lúc lập để trống |
| Trạng thái | Tự ghi | `tickets.State` | nhãn trạng thái | ô mới; lúc lập là Mới (Q1) |
| Chữ ký khách, người nhận | – | – | – | chỉ có trên giấy; thay bằng đăng nhập và `CreatedById` |

#### BM2 – Phiếu theo dõi xử lý
- **Mục đích:** theo dõi một phiếu yêu cầu từ lúc giao tới lúc đóng: ai làm gì, lúc nào, kết quả ra sao; là căn cứ để trưởng bộ phận đóng phiếu.
- **Ai lập:** nhân viên được giao ghi từng bước; Quản lý ghi phân công, đóng, mở lại. **Khi nào:** mỗi lần phiếu đổi trạng thái.
- **Trang:** `frm104_nv_ticket_process` (F3.2, UC04), Master-Detail: phiếu → nhật ký xử lý (số bước, thời gian xử lý). **Bảng:** `tickets` (đầu phiếu), `ticket_logs` (từng dòng).

Bản dựng lại:
```
PHIẾU THEO DÕI XỬ LÝ                        Kèm phiếu yêu cầu số: ..........
Bộ phận xử lý: ...........................

| Ngày giờ          | Người thực hiện   | Việc đã làm / kết quả          | Ký   |
|-------------------|-------------------|--------------------------------|------|
|                   |                   |                                |      |
|                   |                   |                                |      |

Kết quả cuối cùng: ..........................   Xong lúc: ....:.... ngày ..../..../........
Trưởng bộ phận xác nhận đóng: ...................... (ký)
```

| Ô trên phiếu | Loại | Bảng.cột | Trên trang | Ghi chú |
|---|---|---|---|---|
| Kèm phiếu yêu cầu số (và thông tin của BM1) | Tự ghi | `tickets.Id` và các cột của BM1 | đầu trang chi tiết (`?id=`) | đọc lại từ BM1 |
| Hạn xử lý | Chốt | `tickets.DueAt` | nhãn hạn SLA | đã chốt ở BM1 |
| Người được giao | Nhập | `tickets.AssignedAgentId` | đầu trang chi tiết | Q2; Quản lý, Admin đổi ở `frm103` |
| Trạng thái hiện tại | Tự ghi | `tickets.State` | nhãn trạng thái; nút theo trạng thái: Bắt đầu, Giải quyết, Đóng, Mở lại | ô mới; Q1 |
| Dòng: ngày giờ | Tự ghi | `ticket_logs.At` | cột "Thời điểm" | lấy từ `IClock` |
| Dòng: người thực hiện | Tự ghi | `ticket_logs.EmployeeId` | cột "Người thực hiện" | `actor`; Q6: bắt đầu, giải quyết chỉ người được giao hoặc Admin |
| Dòng: việc đã làm | Tự ghi | `ticket_logs.Action` | cột "Thao tác" | lập, phân công, bắt đầu, giải quyết, đóng, mở lại (enum `TicketAction`, chốt ở M6) |
| Dòng: trạng thái trước → sau | Tự ghi | `ticket_logs.FromState`, `ToState` | cột "Trạng thái" | ô mới; Q1 |
| Dòng: kết quả, ghi chú | Nhập | `ticket_logs.Note` | `Input.Note` | |
| Kết quả cuối cùng | Nhập | `tickets.ResolutionNote` | `Input.Note` khi bấm "Giải quyết" | Q1: bắt buộc khi giải quyết |
| Xong lúc | Tự ghi | `tickets.ResolvedAt` | cột "Giải quyết lúc" | mở lại thì ghi lại lần giải quyết mới nhất |
| Trưởng bộ phận xác nhận đóng | Tự ghi | `tickets.ClosedAt` + một dòng `ticket_logs` (đóng) | nút "Đóng phiếu" (Quản lý, Admin) | chữ ký thay bằng đăng nhập và nhật ký; đóng → L2 |
| Số bước, thời gian xử lý | Tính | – | ô tổng ở trang chi tiết | số bước = số dòng nhật ký; thời gian xử lý = `ResolvedAt − CreatedAt` (giờ); tính khi tải trang |

#### BM3 – Phiếu khảo sát hài lòng
- **Mục đích:** đo mức hài lòng của khách sau một lần ở theo từng tiêu chí; làm số liệu cho dashboard 360 (R5) và để quản lý biết dịch vụ yếu ở đâu.
- **Ai lập:** hệ thống tạo phiếu trạng thái Chờ khi khách trả phòng (L3); khách chấm trên giấy hoặc trả lời qua điện thoại; Lễ tân hoặc CSKH ghi vào hệ thống (UC05: mọi vai trò). **Khi nào:** lúc trả phòng hoặc khi gọi lại sau đó.
- **Trang:** `frm105_nv_survey` (F4.1, UC05), Master-Detail: danh sách phiếu Chờ → điểm từng tiêu chí (điểm trung bình tính ngay). **Bảng:** `surveys`, `survey_answers`, `survey_criteria`.

Bản dựng lại:
```
PHIẾU KHẢO SÁT MỨC ĐỘ HÀI LÒNG
Phòng: .......   Ngày nhận: ..../..../........   Ngày trả: ..../..../........
Họ tên khách (không bắt buộc): ...............................
Xin quý khách chấm điểm từ 1 (rất không hài lòng) đến 5 (rất hài lòng):
                                     1     2     3     4     5
Sạch sẽ                             [ ]   [ ]   [ ]   [ ]   [ ]
Thái độ phục vụ                     [ ]   [ ]   [ ]   [ ]   [ ]
Tiện nghi phòng                     [ ]   [ ]   [ ]   [ ]   [ ]
Ăn uống                             [ ]   [ ]   [ ]   [ ]   [ ]
Giá trị so với giá tiền             [ ]   [ ]   [ ]   [ ]   [ ]
Ý kiến khác: ..................................................................
```

| Ô trên phiếu | Loại | Bảng.cột | Trên trang | Ghi chú |
|---|---|---|---|---|
| Số phiếu | Tự ghi | `surveys.Id` | cột "Mã" ở danh sách phiếu Chờ | ô mới |
| Phòng, ngày nhận, ngày trả | Tự ghi | `surveys.BookingId` (UNIQUE) → `bookings` | đầu trang chi tiết | trên giấy khách tự ghi (hay bỏ trống); hệ thống gắn phiếu với lần ở lúc trả phòng (L3); Q5: mỗi lưu trú một phiếu |
| Họ tên khách | Tính | – (suy từ `bookings.CustomerId`) | tên khách ở đầu trang | không lưu `CustomerId` trong `surveys` |
| Ngày tạo phiếu | Tự ghi | `surveys.CreatedAt` | cột "Ngày tạo" | ô mới; lúc trả phòng |
| Điểm từng tiêu chí | Nhập | `survey_answers.CriterionId`, `Score` (khóa chính `SurveyId` + `CriterionId`) | `Input.Scores` (mỗi tiêu chí một nhóm nút 1–5) | Q5: chấm đủ mọi tiêu chí đang dùng (`survey_criteria.IsActive`); điểm 1–5 (CHECK) |
| Tên tiêu chí | Tự ghi | `survey_criteria.Name` | nhãn từng dòng | danh mục: sạch sẽ, thái độ, tiện nghi, ăn uống, giá trị |
| Điểm trung bình | Tính | – | ô "Điểm TB", tính ngay khi chọn điểm | ô mới; R5 tính lại từ `survey_answers` |
| Ý kiến khác | Nhập | `surveys.Comment` | `Input.Comment` | không bắt buộc |
| Trạng thái | Tự ghi | `surveys.Status` | nhãn Chờ / Đã khảo sát | ô mới; Q5: chỉ ghi cho phiếu đang Chờ |
| Ngày khảo sát, người ghi | Tự ghi | `surveys.CompletedAt`, `RecordedById` | – | ô mới |

#### Truy vết ngược trang → biểu mẫu (ví dụ cho vấn đáp)
- **Cột "Hạn SLA" trên `frm103`** là ô mới của BM1, lưu ở `tickets.DueAt` và **chốt** lúc lập: tính một lần từ giờ chuẩn của loại phiếu, mức ưu tiên (đã nâng theo hạng) và hệ số hạng của khách lúc đó. Khách lên hạng về sau thì hạn của phiếu cũ không đổi, vì hạn SLA là lời hẹn với khách lúc nhận phản ánh, và R2 đếm "trễ SLA" phải ra cùng một kết quả mỗi lần chạy.
- **Ô "Thời gian xử lý" trên `frm104`** lấy từ ô "Xong lúc" của BM2 trừ "Ngày giờ nhận" của BM1 (`ResolvedAt − CreatedAt`), tính khi tải trang, không lưu: lưu thì cột này phụ thuộc hai cột không khóa khác của cùng dòng (vi phạm 3NF) và R1 sẽ có hai nguồn số liệu.
- **Ô "Điểm TB" trên `frm105`** là trung bình các ô điểm của BM3, tính ngay trên trang, không lưu; R5 tính lại từ `survey_answers`.
- **Tên khách trên `frm105`** không lưu trong `surveys`: suy từ `surveys.BookingId → bookings.CustomerId`. Lưu thêm thì cùng một sự thật nằm ở hai bảng và có thể lệch nhau (bàn kỹ ở `06-erd.md` mục 5, M5).

### 4.2 BM4–BM7 [TV2]
Mỗi biểu mẫu có bảng ánh xạ ô trên phiếu → cột CSDL → control trên Form, để truy vết hai chiều khi vấn đáp.

Cột "Loại" trong bảng ánh xạ:
- **Nhập**: người dùng nhập hoặc chọn.
- **Tính**: hệ thống tính, không lưu (tính lại mỗi lần hiển thị).
- **Chốt**: hệ thống ghi một lần lúc lập phiếu và không đổi theo dữ liệu gốc về sau (thuộc tính lịch sử).
- **Tự ghi**: hệ thống ghi thời điểm hoặc người làm.

> **Mẫu dựng lại, chưa có bản thật.** Các bảng dưới đây là danh sách ô do nhóm dựng lại. Khi có phiếu thật thì đặt ảnh vào `docs/analysis/bieu-mau/` (`BM4-phieu-dang-ky-thanh-vien.png`, `BM5-phieu-dat-phong.png`, `BM6-phieu-doi-qua.png`, `BM7-danh-sach-cham-soc.png`) rồi đối chiếu và sửa bảng cho khớp.

#### BM4 – Phiếu đăng ký thành viên / hồ sơ khách
- **Mục đích:** lưu thông tin nhận diện và liên lạc của khách, làm gốc cho tích điểm và chăm sóc.
- **Ai lập:** Lễ tân. **Khi nào:** khách đến lần đầu; sửa khi khách đổi thông tin.
- **Form:** `frm201_dm_khach_hang`. **Bảng:** `customers`.

| Ô trên phiếu | Loại | Bảng.cột | Control trên Form | Ghi chú |
|---|---|---|---|---|
| Mã khách | Tự ghi | `customers.Id` | cột lưới "Mã KH" | |
| Họ và tên | Nhập | `customers.FullName` | `txtFullName` | bắt buộc |
| Số điện thoại | Nhập | `customers.Phone` | `txtPhone` | Q7: 10 số, bắt đầu bằng 0, không trùng |
| Email | Nhập | `customers.Email` | `txtEmail` | Q7: đúng định dạng, không trùng |
| Số CCCD / hộ chiếu | Nhập | `customers.IdentityNumber` | `txtIdentityNumber` | Q7: quốc tịch Việt Nam → CCCD 12 chữ số; quốc tịch khác → hộ chiếu 6–9 ký tự chữ và số; không trùng (quyết định ở `docs/PROGRESS.md` mục 8) |
| Quốc tịch | Nhập | `customers.Nationality` | `cboNationality` | lọc được trên lưới |
| Ngày sinh | Nhập | `customers.DateOfBirth` | `dtpDateOfBirth` | không bắt buộc; dùng cho phân khúc sinh nhật |
| Đồng ý nhận ưu đãi | Nhập | `customers.MarketingConsent` | `chkMarketingConsent` | Q11: không đồng ý thì không vào chiến dịch |
| Ghi chú (sở thích, dị ứng…) | Nhập | `customers.Notes` | `txtNotes` | |
| Hạng thành viên | Tính | `customers.Tier` | cột lưới "Hạng" | Q9: hệ thống tự nâng, không nhập tay |
| Ngày đăng ký | Tự ghi | `customers.CreatedAt` | cột lưới "Ngày đăng ký" | |
| Chữ ký khách, nhân viên lập | – | – | – | chỉ có trên giấy |

#### BM5 – Phiếu đặt phòng / nhận–trả phòng
- **Mục đích:** giữ phòng cho khách theo khoảng ngày với giá đã thỏa thuận; xác nhận lúc nhận và trả phòng.
- **Ai lập:** Lễ tân. **Khi nào:** lúc đặt phòng; ký thêm lúc nhận phòng và trả phòng.
- **Form:** `frm203_nv_luu_tru`. **Bảng:** `bookings` (+ `point_transactions` lúc trả phòng).

| Ô trên phiếu | Loại | Bảng.cột | Control trên Form | Ghi chú |
|---|---|---|---|---|
| Số phiếu | Tự ghi | `bookings.Id` | cột lưới "Mã ĐP" | |
| Khách hàng | Nhập | `bookings.CustomerId` | `cboCustomer` (tìm theo tên/SĐT) | |
| Loại phòng, số phòng | Nhập | `bookings.RoomId` → `rooms.RoomTypeId` | `cboRoom` | chỉ hiện phòng còn trống trong khoảng ngày (Q8) |
| Ngày nhận, ngày trả | Nhập | `bookings.CheckInDate`, `CheckOutDate` | `dtpCheckIn`, `dtpCheckOut` | Q8: ngày trả > ngày nhận |
| Số đêm | Tính | – | cột lưới "Số đêm" | ngày trả − ngày nhận |
| Đơn giá/đêm | **Chốt** | `bookings.RatePerNight` | cột lưới "Giá/đêm (đ)" | lấy từ `room_types.PricePerNight` lúc đặt rồi giữ nguyên |
| Thành tiền | Tính | – | cột lưới "Thành tiền (đ)" | số đêm × đơn giá/đêm đã chốt |
| Trạng thái | Tính | `bookings.Status` | cột lưới "Trạng thái" | Đã đặt / Đang ở / Đã trả phòng / Đã hủy |
| Giờ nhận phòng thực tế | Tự ghi | `bookings.CheckedInAt` | | Q8: chỉ nhận trong [ngày nhận, ngày trả) |
| Giờ trả phòng thực tế | Tự ghi | `bookings.CheckedOutAt` | | |
| Điểm được cộng | Tự ghi | `point_transactions.Points` (lý do Lưu trú) | | Q9: thành tiền / 10.000đ × hệ số hạng, làm tròn xuống |
| Nhân viên lập phiếu | Tự ghi | `bookings.CreatedById` | | |

#### BM6 – Phiếu đổi quà
- **Mục đích:** ghi nhận khách dùng điểm đổi lấy quà, làm chứng từ trừ điểm và xuất kho quà.
- **Ai lập:** Lễ tân. **Khi nào:** khách yêu cầu đổi quà tại quầy.
- **Form:** `frm205_nv_doi_qua`. **Bảng:** `reward_redemptions` (+ `point_transactions`, `rewards.Quantity`).

| Ô trên phiếu | Loại | Bảng.cột | Control trên Form | Ghi chú |
|---|---|---|---|---|
| Số phiếu | Tự ghi | `reward_redemptions.Id` | cột lưới "Mã lượt đổi" | |
| Khách hàng, hạng | Nhập / Tính | `reward_redemptions.CustomerId`; `customers.Tier` | `cboCustomer`, `lblTier` | |
| Số dư điểm trước khi đổi | Tính | tổng `point_transactions.Points` của khách | `lblBalance` | |
| Quà tặng | Nhập | `reward_redemptions.RewardId` | `dgvRewards` (dòng đang chọn) | Q10: còn áp dụng, còn hàng, đủ hạng tối thiểu |
| Điểm quy đổi | **Chốt** | `reward_redemptions.PointsSpent` | cột lưới "Điểm đã đổi" | lấy từ `rewards.PointsCost` lúc đổi rồi giữ nguyên |
| Số dư sau khi đổi | Tính | – | `lblBalanceAfter` | Q10: không bao giờ âm |
| Ngày đổi | Tự ghi | `reward_redemptions.RedeemedAt` | cột lưới "Ngày đổi" | |
| Trạng thái | Tính | `reward_redemptions.Status` | cột lưới "Trạng thái" | Đã đổi / Đã hủy; hủy thì trả quà về kho và hoàn điểm |
| Nhân viên xử lý | Tự ghi | `reward_redemptions.ProcessedById` | | |
| Chữ ký khách | – | – | – | chỉ có trên giấy |

#### BM7 – Danh sách chăm sóc theo chiến dịch
- **Mục đích:** danh sách khách nhận một ưu đãi và phản hồi của từng khách, để đo hiệu quả chiến dịch.
- **Ai lập:** Quản lý lập phần đầu; nhân viên CSKH (hoặc Lễ tân) điền phản hồi từng dòng. **Khi nào:** đầu mỗi đợt ưu đãi; phản hồi ghi tới hết ngày kết thúc.
- **Form:** `frm206_nv_chien_dich` (Master: chiến dịch, Detail: khách nhận). **Bảng:** `campaigns`, `campaign_targets`.

| Ô trên phiếu | Loại | Bảng.cột | Control trên Form | Ghi chú |
|---|---|---|---|---|
| Tên chiến dịch, nội dung ưu đãi | Nhập | `campaigns.Name`, `Offer` | `txtCampaignName`, `txtOffer` | |
| Nhóm khách (phân khúc) | Nhập | `campaigns.SegmentCode` | `cboSegment` | chọn Strategy lúc chạy |
| Từ ngày – đến ngày | Nhập | `campaigns.StartDate`, `EndDate` | `dtpStartDate`, `dtpEndDate` | |
| Người lập | Tự ghi | `campaigns.CreatedById` | | |
| Dòng: khách, SĐT/email | Tính | `campaign_targets.CustomerId` | `dgvTargets` | Q11: phân khúc ∩ đồng ý ưu đãi − có khiếu nại chưa đóng; mỗi khách một dòng |
| Dòng: kênh, ngày gửi | Tự ghi | `campaign_targets.Channel`, `SentAt` | `dgvTargets` | |
| Dòng: phản hồi, ngày phản hồi, người ghi | Nhập / Tự ghi | `campaign_targets.Response`, `RespondedAt`, `RecordedById` | `cboResponse` | Q11: chỉ ghi tới hết ngày kết thúc |
| Cuối danh sách: số khách, số phản hồi, tỷ lệ | Tính | – | `lblTargetCount`, `lblResponseCount`, `lblResponseRate` | tính lại ngay khi ghi phản hồi |

#### Truy vết ngược Form → biểu mẫu (ví dụ cho vấn đáp)
- **Cột "Giá/đêm" trên lưới `frm203`** lấy từ ô "Đơn giá/đêm" của BM5, lưu ở `bookings.RatePerNight`. Không lấy thẳng `room_types.PricePerNight` vì giá phòng đổi theo mùa: nếu lấy giá hiện tại thì thành tiền và điểm của các lần ở cũ bị tính lại sai. Đây là thuộc tính lịch sử, không vi phạm 3NF vì nó là sự thật của từng lần đặt, không phụ thuộc vào loại phòng (`06-erd.md` mục 6).
- **Cột "Điểm đã đổi" trên lưới `frm205`** lấy từ ô "Điểm quy đổi" của BM6, lưu ở `reward_redemptions.PointsSpent` (trong code: `RewardRedemption.PointsSpent`, chốt trong `Reward.Redeem`). Lý do giống trên: quà tăng giá điểm về sau thì lượt đổi cũ vẫn đúng.
- **Nhãn "Tỷ lệ phản hồi" trên `frm206`** là ô cuối danh sách của BM7, tính từ các dòng chi tiết (`Campaign.ResponseRate`), không lưu vào CSDL.

## 5. Yêu cầu chức năng

### 5.1 F1–F4 [TV1]
Tên chức năng lá, trang và UC theo `docs/PLAN.md` mục 4.2, 4.3, 5.1; vai trò theo ma trận quyền ở mục 4.1 của PLAN (kiểm ở tầng Service, Q6).

| Mã | Tên | Mô tả | Vai trò dùng | Trang | UC | Quy tắc / báo cáo |
|---|---|---|---|---|---|---|
| F1.1 | Đăng nhập, đổi mật khẩu | Đăng nhập bằng tên đăng nhập và mật khẩu; menu hiện theo vai trò; đổi mật khẩu của mình; đăng xuất. Tài khoản bị khóa không đăng nhập được | mọi vai trò | `frm000_login` | UC01 | Q6 |
| F1.2 | Phòng ban, nhân viên, tài khoản | Master-Detail: phòng ban → nhân viên (số phiếu đang giữ tính khi tải trang). Thêm, sửa phòng ban; thêm, sửa nhân viên và tài khoản (tên đăng nhập, vai trò, bộ phận, khóa/mở) | Admin; Quản lý xem | `frm101_dm_staff` | UC01 | Q6 |
| F2.1 | Lập phiếu yêu cầu (khiếu nại, yêu cầu dịch vụ, sửa chữa) | Chọn loại, tìm khách, chọn phòng, nhập tiêu đề, nội dung, mức ưu tiên; hệ thống nâng ưu tiên theo hạng khách và hiện ngay hạn SLA dự kiến trước khi lưu | mọi vai trò | `frm102_nv_ticket_create` | UC02 | Q3, Q4; L1; BM1 |
| F3.1 | Tra cứu & phân công phiếu yêu cầu | Lọc theo trạng thái, ưu tiên, loại, nhân viên, khoảng ngày, từ khóa, chỉ phiếu trễ SLA; nút "Xóa lọc"; phân công cho một nhân viên hoặc phân công tự động | xem: mọi vai trò; phân công: Quản lý, Admin | `frm103_nv_ticket_search` | UC03 | Q2, Q6 |
| F3.2 | Xử lý phiếu yêu cầu & nhật ký | Master-Detail: phiếu → nhật ký xử lý (số bước, thời gian xử lý). Bắt đầu, giải quyết (bắt buộc ghi chú), đóng, mở lại; mỗi lần đổi trạng thái ghi nhật ký. Đóng phiếu → cộng điểm thiện chí và ghi tương tác bên phân hệ TV2, trong một giao dịch | bắt đầu, giải quyết: CSKH (phiếu của mình), Admin; đóng, mở lại: Quản lý, Admin; xem: mọi vai trò | `frm104_nv_ticket_process` | UC04 | Q1, Q6, Q12; L2; BM2 |
| F4.1 | Ghi phiếu khảo sát | Master-Detail: danh sách phiếu Chờ (tạo khi trả phòng) → chấm 1–5 cho từng tiêu chí, điểm trung bình tính ngay; ghi ý kiến khác | mọi vai trò | `frm105_nv_survey` | UC05 | Q5; L3; BM3 |
| F4.2 | Báo cáo CSKH | R1 hiệu suất nhân viên CSKH; R2 phiếu yêu cầu & SLA theo loại, tháng, hạng khách có biểu đồ; lọc theo khoảng ngày; xuất Excel (tiêu đề, ngày xuất, dòng tổng) | Quản lý, Admin | `frm106_bc_customer_care` | UC06 | R1, R2 |

Ngoài 7 chức năng lá, phân hệ còn:
- **Trang chủ `frm001_main`** (không phải nút lá BFD, `docs/PLAN.md` mục 5.1): TV1 làm khung trang và khối số liệu CSKH (phiếu đang mở, phiếu trễ SLA, việc cần làm của người đăng nhập); khối của phân hệ TV2 do TV2 viết.
- **Ba việc không có trang, cài cho phân hệ TV2 gọi:** `SurveyScheduler` tạo phiếu khảo sát Chờ khi trả phòng (L3); `TicketQuery` trả lời khách có phiếu yêu cầu không, có khiếu nại chưa đóng không (L4, L5); `CustomerCareStats` thống kê CSKH theo khách (L6).

#### Số liệu nghiệp vụ dùng trong phân hệ
Giá trị lấy từ code v1 (`SupportTicket` và các lớp con, `TicketPriority`, `appsettings.json`); chốt ở M4 (Q1–Q5 trong `04-quy-tac.md`).

| Số liệu | Giá trị | Nơi đặt trong code | Quy tắc |
|---|---|---|---|
| Giờ chuẩn theo loại phiếu | Khiếu nại 24 · Yêu cầu dịch vụ 8 · Sửa chữa 12 (giờ) | `SupportTicket.BaseSlaHours` (mỗi lớp con) | Q3 |
| Hệ số theo mức ưu tiên | Thấp 2,0 · Trung bình 1,0 · Cao 0,5 · Khẩn 0,25 | `TicketPriority` → hệ số trong `SupportTicket` | Q3 |
| Hệ số SLA và số bậc ưu tiên được nâng theo hạng | lấy từ phân hệ TV2 (mục 5.2): hệ số Standard 1,0 · Silver 0,9 · Gold 0,75 · VIP 0,5; nâng Gold 1 bậc, VIP 2 bậc | `MembershipTier.SlaFactor`, `PriorityBoost`, đọc qua `ICustomerDirectory` | Q3, L1 |
| Bộ phận xử lý theo loại | Khiếu nại → Quan hệ khách hàng · Yêu cầu dịch vụ → Buồng phòng · Sửa chữa → Kỹ thuật | `SupportTicket.HandlingDepartmentCode` | Q2 |
| Số phiếu đang mở tối đa của một nhân viên | 5 (cấu hình) | `appsettings.json` → `MaxActiveTickets` | Q2 |
| Số ngày được mở lại sau khi giải quyết | 7 (cấu hình) | `appsettings.json` → `ReopenDays` | Q1 |
| Thang điểm khảo sát | 1–5; 5 tiêu chí đang dùng | CHECK `survey_answers.Score`; `survey_criteria.IsActive` | Q5 |
| Điểm thiện chí khi đóng phiếu | khiếu nại 50 · loại khác 10 | `SupportTicket.GoodwillPoints` (TV2 cộng điểm) | Q12 |

Ví dụ tính tay (Q3): phiếu khiếu nại lập lúc 10:00 ngày 27/11/2026, người lập chọn mức Trung bình, khách hạng Gold. Gold nâng 1 bậc → Cao (hệ số 0,5); hệ số SLA của Gold 0,75 → 24 × 0,5 × 0,75 = 9 giờ → hạn xử lý 19:00 ngày 27/11/2026.

### 5.2 F5–F8 [TV2]

| Mã | Tên | Mô tả | Vai trò dùng | Form | UC | Quy tắc / báo cáo |
|---|---|---|---|---|---|---|
| F5.1 | Cập nhật hồ sơ khách | Thêm, sửa, xóa, tìm hồ sơ khách; lọc theo từ khóa, hạng, quốc tịch, đồng ý ưu đãi; xóa lọc | Lễ tân, Quản lý, Admin (xóa: Quản lý, Admin); CSKH chỉ xem | frm201 | UC07 | Q7, Q13; L5 |
| F5.2 | Tra cứu hồ sơ 360 của khách | Xem lưu trú, tương tác, giao dịch điểm của khách đang chọn; tổng chi tiêu, số lần ở, số dư điểm tính ngay | mọi vai trò | frm201 (phần chi tiết) | UC07 | – |
| F6.1 | Cập nhật danh mục loại phòng và phòng | Loại phòng (tên, giá/đêm, sức chứa) và phòng (số, tầng, loại) | Admin; vai trò khác xem | frm202 | UC08 | – |
| F6.2 | Đặt phòng, nhận phòng, trả phòng | Đặt, nhận, trả, hủy; lọc theo khoảng ngày, trạng thái, khách. Trả phòng: cộng điểm, nâng hạng, ghi tương tác, tạo phiếu khảo sát chờ, trong một giao dịch | Lễ tân, Quản lý, Admin | frm203 | UC08 | Q8, Q9; L3 |
| F7.1 | Cập nhật danh mục quà tặng | Tên quà, điểm quy đổi, hạng tối thiểu, số lượng, còn áp dụng | Admin; vai trò khác xem | frm204 | UC09 | – |
| F7.2 | Theo dõi sổ điểm và đổi quà | Xem sổ điểm của khách; đổi quà; hủy lượt đổi (trả quà về kho, hoàn điểm) | Lễ tân, Quản lý, Admin | frm205 | UC09 | Q10 |
| F8.1 | Lập và theo dõi chiến dịch chăm sóc | Tạo chiến dịch, chọn phân khúc (VIP, lâu chưa quay lại, sinh nhật trong tháng), lập danh sách khách, ghi phản hồi | tạo: Quản lý, Admin; ghi phản hồi: mọi vai trò | frm206 | UC10 | Q11; L4 |
| F8.2 | Lập báo cáo khách hàng và dashboard 360 | Báo cáo khách theo hạng (có biểu đồ), hiệu quả chiến dịch, dashboard 360; xuất Excel | Quản lý, Admin | frm207 | UC11 | R3, R4, R5; L6 |

Ngoài 8 chức năng trên, phân hệ còn một việc tự động không có Form: khi ticket của khách chuyển sang Đã đóng (UC04 của TV1), cộng điểm thiện chí và ghi một tương tác, cùng giao dịch với việc đóng ticket (Q12, L2).

#### Số liệu nghiệp vụ dùng trong phân hệ

| Số liệu | Giá trị | Nơi đặt trong code | Quy tắc |
|---|---|---|---|
| Ngưỡng điểm của hạng (tổng điểm đã tích) | Standard 0 · Silver 1.000 · Gold 5.000 · VIP 15.000 | `MembershipTier.MinPoints` (mỗi lớp hạng) | Q9 |
| Hệ số tích điểm | Standard 1,0 · Silver 1,2 · Gold 1,5 · VIP 2,0 | `MembershipTier.EarnRate` | Q9 |
| Số tiền cho 1 điểm (trước hệ số) | 10.000đ | `MembershipTier.VndPerPoint` | Q9 |
| Hệ số SLA theo hạng (TV1 dùng) | Standard 1,0 · Silver 0,9 · Gold 0,75 · VIP 0,5 | `MembershipTier.SlaFactor` | Q3 |
| Nâng mức ưu tiên theo hạng (TV1 dùng) | Standard 0 · Silver 0 · Gold 1 · VIP 2 | `MembershipTier.PriorityBoost` | Q3 |
| Điểm thiện chí khi đóng ticket | khiếu nại 50 · loại khác 10 | `SupportTicket.GoodwillPoints` (TV1) | Q12 |
| Số ngày "lâu chưa quay lại" | 180 (cấu hình) | `appsettings.json` → `LapsedGuestSegment` | Q11 |

Ví dụ tính tay (Q9): khách Silver ở 2 đêm, giá chốt 1.250.000đ/đêm → thành tiền 2.500.000đ → 2.500.000 / 10.000 × 1,2 = 300 điểm.

Hệ số tích điểm do TV2 đề xuất (PLAN chỉ nêu "hệ số tích điểm của hạng", chưa có số); TV1 xem lại khi review.

## 6. Yêu cầu phi chức năng [Chung – TV1]
Viết theo bản web chốt 27/11 (`docs/PLAN.md` mục 3.1, 5.1, 5.6). Cột cuối ghi cách đạt và cách kiểm tra để đối chiếu khi chấm.

| # | Nhóm | Yêu cầu | Cách đạt / cách kiểm tra |
|---|---|---|---|
| 1 | Cài đặt, chạy | Ứng dụng web chạy tại máy, mở bằng trình duyệt (Chrome, Edge) ở `http://localhost:5080`; chỉ nghe localhost, dùng HTTP nên không cần chứng chỉ HTTPS | Cổng đặt ở khóa `Urls` của `appsettings.json`; mở từ máy khác thì không vào được |
| 2 | Cài đặt, chạy | Máy chấm không cần cài .NET hay máy chủ CSDL; từ lúc giải nén tới lúc đăng nhập không quá 3 phút theo README | `dotnet publish -c Release -r win-x64 --self-contained` (thêm `linux-x64` khi cần); thử trên máy sạch ngày 4/12 và lại ở P4 |
| 3 | Cài đặt, chạy | Lần chạy đầu tự tạo CSDL có dữ liệu mẫu; Admin tạo lại được CSDL mẫu | Tạo `hotel.db` từ `Database/schema.sql` + `Database/seed.sql` khi chưa có file |
| 4 | Dữ liệu | Toàn bộ dữ liệu trong một file SQLite nằm cạnh ứng dụng; toàn vẹn do CSDL giữ | `Microsoft.Data.Sqlite`; `PRAGMA foreign_keys=ON` mỗi kết nối; CHECK cho trạng thái, điểm, số tiền, ngày; UNIQUE cho SĐT, email, giấy tờ, tên đăng nhập; 17 bảng đạt 3NF |
| 5 | Dữ liệu | Không mất hay lệch dữ liệu khi lỗi giữa chừng: thao tác ghi nhiều bảng hoặc thành công trọn vẹn, hoặc không để lại gì | Một `IUnitOfWork.Execute(...)` = một connection + một transaction; lời gọi lồng nhau tham gia transaction ngoài. L2 (đóng phiếu yêu cầu) và L3 (trả phòng) chạy trọn trong một transaction; integration test L2 rollback, L3 rollback |
| 6 | Dữ liệu | Nhiều thao tác cùng lúc (nhiều tab, nhiều người dùng chung máy) không làm khóa hay hỏng CSDL | Mỗi request một `UnitOfWork`; SQLite bật `journal_mode=WAL` và `busy_timeout` |
| 7 | Dữ liệu | Số liệu đã chốt không bị tính lại khi dữ liệu gốc đổi | Giá/đêm (`bookings.RatePerNight`), điểm quy đổi (`reward_redemptions.PointsSpent`), hạn SLA (`tickets.DueAt`) ghi một lần lúc lập |
| 8 | Bảo mật | Đăng nhập bằng tài khoản riêng; không lưu mật khẩu dạng đọc được; tài khoản bị khóa không vào được | Mật khẩu băm PBKDF2 kèm salt riêng (`employees.PasswordHash`, `Salt`); `employees.IsActive` |
| 9 | Bảo mật | Phiên đăng nhập không dùng biến tĩnh dùng chung | Cookie lưu mã nhân viên; mỗi request nạp lại `Employee` và truyền làm `actor` |
| 10 | Bảo mật | Phân quyền theo ma trận 4 vai trò (`docs/PLAN.md` mục 4.1); gọi thẳng handler hay đường dẫn cũng bị chặn | Service gọi `actor.EnsureCan…()` trước khi làm, quyền là hàm đa hình của 4 lớp con `Employee` (Q6); ẩn/khóa nút và `[Authorize]` chỉ là lớp chặn thứ hai; integration test RBAC của mỗi người |
| 11 | Bảo mật | Chặn các lỗi web cơ bản | Antiforgery mặc định cho mọi form POST; mọi câu SQL có tham số; cột sắp xếp lấy từ danh sách cho phép; không dùng `Html.Raw` với dữ liệu người dùng nhập; trang lỗi không lộ stack trace |
| 12 | Truy vết | Biết ai làm gì, lúc nào | Mọi thao tác ghi người làm (`CreatedById`, `RecordedById`, `ProcessedById`, `ticket_logs.EmployeeId`); thời gian lấy qua `IClock`; lỗi hệ thống ghi `logs/error.log` |
| 13 | Giao diện | Toàn bộ chữ trên giao diện, thông báo lỗi, dữ liệu mẫu bằng tiếng Việt | Trang có `lang="vi"`; `ErrorHandler` đổi ngoại lệ thành câu tiếng Việt nói rõ sai gì |
| 14 | Giao diện | Tiền VND định dạng `#,##0`, căn phải (ví dụ 1.250.000); ngày `dd/MM/yyyy`, có giờ thì `dd/MM/yyyy HH:mm` | Hàm định dạng tiền/ngày dùng chung của GUI (TV2 viết, `docs/PLAN.md` mục 6.2); file Excel xuất ra dùng cùng định dạng |
| 15 | Giao diện | Chỉ dùng trên máy tính, màn hình từ 1280px; chỉ giao diện sáng | Bố cục thanh bên + thanh trên của `_Layout`; không làm bố cục điện thoại, không làm giao diện tối |
| 16 | Giao diện | Đạt WCAG 2.1 mức AA về tương phản và nhận biết | Chữ ≥ 4,5:1; viền ô nhập và vòng focus ≥ 3:1; nhãn trạng thái có chữ, không chỉ dựa vào màu; lỗi nhập hiện cạnh ô (`is-invalid` + `invalid-feedback`, gắn `aria-describedby`), con trỏ nhảy tới ô lỗi đầu tiên; hiệu ứng tắt khi máy đặt giảm chuyển động |
| 17 | Giao diện | Sai dữ liệu được báo ngay tại ô và được kiểm lại ở tầng dưới | Kiểm ở trang (InputModel) và ở Domain/Service; ngoại lệ `ValidationException`, `BusinessRuleException`, `AuthorizationException` hiện bằng toast hoặc mở lại modal kèm lỗi |
| 18 | Hiệu năng | Mỗi trang danh sách, báo cáo và việc xuất Excel xong trong ≤ 2 giây với dữ liệu mẫu (≥ 150 dòng) trên máy chấm | Phân trang, sắp xếp ở server (`LIMIT/OFFSET`); lưới và báo cáo dùng truy vấn JOIN, không N+1; repository đọc CSDL khi cần, không nạp hết vào bộ nhớ; bấm thử bằng tay ở P4 |
| 19 | Bảo trì | Thêm loại phiếu yêu cầu, phân khúc khách hay observer mới không sửa lớp cũ | Thêm một lớp và một dòng đăng ký ở `Program.cs` (Live OCP: `LostItemTicket`, `FrequentGuestSegment`); không `switch` cứng theo tên |
| 20 | Bảo trì | Tầng rõ ràng, hai phân hệ tách nhau | `GUI → BLL → Domain ← DAL`; trang không chứa SQL hay quy tắc; hai phân hệ chỉ gọi nhau qua 6 interface (L1–L6); cấu hình đọc một lần từ `appsettings.json`, truyền qua constructor |
| 21 | Kiểm thử | Lỗi cố ý cài vào code phải làm test đỏ | 28 test: mỗi người 10 unit (dùng Fake, không cần CSDL) + 4 integration (CSDL SQLite tạm có seed); `IClock` cố định thời gian; giá trị kỳ vọng lấy từ bảng tính tay (`docs/PLAN.md` mục 5.8) |

## 7. Phạm vi hệ thống [Chung – TV1]

#### Trong phạm vi
| Nhánh BFD | Chức năng lá | Trang | Phân hệ |
|---|---|---|---|
| F1 Hệ thống & nhân sự | F1.1, F1.2 | `frm000_login`, `frm101_dm_staff` | TV1 |
| F2 Tiếp nhận yêu cầu | F2.1 | `frm102_nv_ticket_create` | TV1 |
| F3 Điều phối & xử lý | F3.1, F3.2 | `frm103_nv_ticket_search`, `frm104_nv_ticket_process` | TV1 |
| F4 Đánh giá hài lòng | F4.1, F4.2 | `frm105_nv_survey`, `frm106_bc_customer_care` | TV1 |
| F5 Hồ sơ khách hàng 360 | F5.1, F5.2 | `frm201_dm_customer` | TV2 |
| F6 Lưu trú | F6.1, F6.2 | `frm202_dm_room`, `frm203_nv_stay` | TV2 |
| F7 Khách hàng thân thiết | F7.1, F7.2 | `frm204_dm_reward`, `frm205_nv_loyalty` | TV2 |
| F8 Chương trình chăm sóc & báo cáo khách hàng | F8.1, F8.2 | `frm206_nv_campaign`, `frm207_bc_customer_360` | TV2 |

Tổng: 15 chức năng lá; 13 trang nghiệp vụ, thêm `frm000_login` và trang chủ `frm001_main`; 11 use case (UC01–UC11); 13 quy tắc (Q1–Q13); 5 báo cáo (R1–R5); 17 bảng trong 11 kho (D1–D11); 6 luồng liên phân hệ (L1–L6); 4 vai trò.

Giả định: một khách sạn khoảng 20 phòng, khoảng 10 nhân viên; người dùng là nhân viên khách sạn, khách hàng không đăng nhập; hệ thống chạy trên một máy, mở bằng trình duyệt tại máy đó; dữ liệu mẫu tháng 9–11/2026 (`docs/PLAN.md` mục 5.9).

#### Ngoài phạm vi
| Việc | Lý do |
|---|---|
| Hóa đơn, thanh toán, công nợ; dịch vụ phát sinh (ăn uống, giặt là, minibar); đặt nhiều phòng trong một phiếu; báo cáo doanh thu, công suất phòng; chính sách giảm giá | Việc của phần mềm quản lý khách sạn (PMS), không phải CRM; đã bỏ so với bản 21/11 (`docs/PLAN.md` mục 3) |
| Khách tự đặt phòng, tự điền khảo sát, tự xem điểm qua mạng | Khách không đăng nhập; nhân viên ghi thay (UC05, UC08, UC09) |
| Gửi email, SMS, tin nhắn thật cho khách | Chương trình chăm sóc chỉ lưu kênh, ngày gửi và phản hồi do nhân viên ghi (BM7) |
| Truy cập từ máy khác trong mạng hay qua Internet; HTTPS | Thầy chấp nhận chạy localhost (26/11); ứng dụng chỉ nghe localhost (mục 6) |
| Giao diện điện thoại, máy tính bảng; giao diện tối; ngôn ngữ khác tiếng Việt | Chốt 27/11: chỉ máy tính từ 1280px, chỉ giao diện sáng (`docs/PLAN.md` mục 5.1) |
| Chuông báo phiếu sắp trễ SLA | Chốt 27/11: không làm, trang chủ đã có "Việc cần làm" (`docs/PLAN.md` mục 3.1) |
| Xuất PDF | Báo cáo chỉ xuất Excel (`docs/PLAN.md` mục 3) |
| Nhiều khách sạn, chi nhánh; đồng bộ với PMS hay hệ thống khác | Ngoài quy mô bài tập nhóm 2 người |
