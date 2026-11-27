# Task P1 — Phân tích thiết kế (M2–M9)

Giai đoạn P1 của `docs/PLAN.md` (mục 6.3). Kết quả: bộ tài liệu `docs/analysis/`, sơ đồ `docs/analysis/diagrams/`, slide nộp **30/11**.

File này đủ để mỗi người tự làm phần mình song song. Mã F, UC, BM, Q, R, L, TC lấy đúng như `docs/PLAN.md`; không tự đặt mã mới. Chỗ nào file này không nói thì theo `docs/PLAN.md`; chỗ nào còn phải quyết thì xem "Việc chờ người" trong `docs/PROGRESS.md`, chưa có quyết định thì làm theo plan và ghi chú lại.

Cột "Hạn (plan)" ở mục 2 là hạn gốc. M1 xong trễ 3 ngày; ngày 27/11 hai người chốt giữ hạn nộp slide 30/11, TV1 làm dồn phần mình của M2–M8 trong 27–29/11 theo cột "Hạn TV1 (27/11)". Phần TV2 của M2–M8 đã xong (PR #3).

Ngày 26/11 thầy yêu cầu đổi giao diện sang web; quyết định chốt 27/11 ở `docs/PLAN.md` mục 3.1. Tài liệu P1 viết theo bản web (mục 1.6).

## 1. Quy ước chung cho P1

### 1.1 Nhánh, PR, tiến độ
- Mỗi người, mỗi module một nhánh tạo từ `v2`: `tv1/m2-khao-sat`, `tv2/m2-khao-sat`, `tv1/m3-bfd-uc`, … Mỗi module một commit (message tiếng Anh, không `Co-Authored-By`), mở PR vào `v2`; người kia review trong 24 giờ.
- Trong PR của mình: đánh dấu `[x]` dòng của mình ở `docs/PROGRESS.md` (mục 3 hoặc 4), sửa cột trạng thái ở mục 2, thêm một dòng nhật ký.
- Không cần chờ người kia xong module mới làm module sau, trừ những phụ thuộc ghi ở mục 2.

### 1.2 File dùng chung, không giẫm chân nhau
- Mỗi file `docs/analysis/NN-*.md` có **khung mục cố định** ghi ở mục 3 dưới đây. Ai mở PR đầu tiên cho file đó thì tạo file với **đủ khung**, mục của người kia để dòng `_[TV2] chưa điền._` (hoặc `_[TV1] chưa điền._`). Người sau rebase lên `v2` rồi chỉ thay dòng chờ của mình.
- Chỉ sửa mục mang nhãn của mình. Thấy mục của người kia sai thì comment trong PR, không sửa hộ.
- Đầu mỗi mục ghi nhãn `[TV1]`, `[TV2]` hoặc `[Chung – TV1]`, `[Chung – TV2]` (phần chung do ai viết).

### 1.3 Sơ đồ
- Thư mục `docs/analysis/diagrams/`. PlantUML (`.puml`, `.iuml`) cho BFD, Use Case, ERD, lớp, package, tuần tự, trạng thái. DFD vẽ bằng draw.io (`.drawio`) hoặc PlantUML; dùng ký hiệu **Gane–Sarson** cho cả nhóm.
- Sơ đồ chung tách phần: file tổng `X.puml` do TV1 tạo, `!include X-tv1.iuml` và `!include X-tv2.iuml`; mỗi người chỉ sửa file `.iuml` của mình.
- Xuất ảnh PNG cùng tên, cùng thư mục, commit cả `.png` (slide và tài liệu dùng ảnh này): `java -jar tools/plantuml.jar -tpng docs/analysis/diagrams/*.puml`. Máy TV1 chưa có Java và `tools/plantuml.jar` chưa có (xem "Việc chờ người"). Chưa xuất được thì vẫn commit `.puml`, xuất ảnh trước M9.
- Chữ trên sơ đồ tiếng Việt, trừ tên lớp/hàm/bảng/cột (tiếng Anh, khớp code).

### 1.4 Biểu mẫu thật
- Ảnh hoặc bản scan đặt ở `docs/analysis/bieu-mau/BMn-<ten>.png` (ví dụ `BM1-phieu-yeu-cau.png`). Chưa có bản thật thì dựng lại bảng trường trong file `.md` và ghi rõ "mẫu dựng lại, chưa có bản thật".

### 1.5 Danh mục dùng chung để hai người vẽ khớp nhau
**Tác nhân (Use Case, DFD):** Quản trị hệ thống (Admin), Quản lý, Lễ tân, Nhân viên CSKH; tác nhân ngoài cho DFD: Khách hàng.

**Tiến trình DFD mức 0** (trùng 8 nhánh BFD): 1.0 Hệ thống & nhân sự · 2.0 Tiếp nhận yêu cầu · 3.0 Điều phối & xử lý · 4.0 Đánh giá hài lòng · 5.0 Hồ sơ khách hàng 360 · 6.0 Lưu trú · 7.0 Khách hàng thân thiết · 8.0 Chiến dịch & báo cáo khách hàng.

**Kho dữ liệu DFD** (khớp bảng ở PLAN 5.2):
| Mã | Kho | Bảng | Chủ |
|---|---|---|---|
| D1 | Nhân viên & phòng ban | `departments`, `employees` | TV1 |
| D2 | Ticket | `tickets` | TV1 |
| D3 | Nhật ký xử lý | `ticket_logs` | TV1 |
| D4 | Khảo sát | `survey_criteria`, `surveys`, `survey_answers` | TV1 |
| D5 | Khách hàng | `customers` | TV2 |
| D6 | Tương tác | `interactions` | TV2 |
| D7 | Phòng | `room_types`, `rooms` | TV2 |
| D8 | Lưu trú | `bookings` | TV2 |
| D9 | Sổ điểm | `point_transactions` | TV2 |
| D10 | Quà tặng & lượt đổi | `rewards`, `reward_redemptions` | TV2 |
| D11 | Chiến dịch | `campaigns`, `campaign_targets` | TV2 |

**Mã ca test:** TV1 `TC1-01` … `TC1-14` (01–10 unit, 11–14 integration); TV2 `TC2-01` … `TC2-14` (cùng cách chia).

### 1.6 Viết theo bản web (chốt 27/11)
- "Form" trong tài liệu là trang web; tên trang theo `docs/PLAN.md` mục 5.1 (`frm000_login`, `frm102_nv_ticket_create`, `frm201_dm_customer`, …). Chỗ nào checklist ghi Form, ErrorProvider, DataGridView, App.config thì ghi phần tương đương trên web: trang Razor, báo lỗi cạnh ô, bảng dùng chung, `appsettings.json`.
- Chữ trong tài liệu và trên sơ đồ dùng "phiếu yêu cầu" (tên lớp, bảng giữ `SupportTicket`, `tickets`) và "chương trình chăm sóc" (giữ `Campaign`, `campaigns`). Tên nút lá BFD và tên UC theo `docs/PLAN.md` mục 4.2, 4.3. Tiêu đề mục đã có trong file của người kia thì không đổi hộ.
- Ảnh giao diện trong tài liệu và slide lấy từ bản mẫu web của TV1 (link trong bản kế hoạch chuyển đổi), không dùng ảnh WinForms v1.

## 2. Bảng việc M2–M9
| Module | Hạn (plan) | Hạn TV1 (27/11) | TV1 | TV2 | Phụ thuộc |
|---|---|---|---|---|---|
| M2 Khảo sát | 23/11 | 27/11 | 3.1 [TV1] | 3.1 [TV2] | — |
| M3 BFD + UC | 24/11 | 27/11 | 3.2 [TV1] | 3.2 [TV2] | giữ cây BFD 8 nhánh (chốt 27/11; TV1 tạo khung trước, TV2 thêm lá) |
| M4 Quy tắc, quyền, DFD | 25/11 | 28/11 | 3.3 [TV1] | 3.3 [TV2] | DFD mức 1 F7 dùng ký hiệu + kho ở 1.5 (không cần chờ mức 0) |
| M5 ERD + CSDL | 26/11 | 28/11 | 3.4 [TV1] | 3.4 [TV2] | TV1 tạo `erd.puml` khung trước để TV2 `!include` |
| M6 Lớp + package + hợp đồng | 27/11 | 28/11 | 3.5 [TV1] | 3.5 [TV2] | **họp chốt hợp đồng interface 27/11** |
| M7 Hành vi | 28/11 | 29/11 | 3.6 [TV1] | 3.6 [TV2] | tên lớp/hàm theo M6 |
| M8 Giao diện + test | 28/11 | 29/11 | 3.7 [TV1] | 3.7 [TV2] | quy tắc M4, lớp M6 |
| M9 Slide + tự đánh giá L2 | 29/11 | 29/11 | 3.8 [TV1] | 3.8 [TV2] | ảnh sơ đồ M3–M7 đã xuất |

## 3. Chi tiết từng module

### 3.1 M2 — Khảo sát (hạn 23/11)
**File đích:** `docs/analysis/01-khao-sat.md`; ảnh biểu mẫu trong `docs/analysis/bieu-mau/`.

**Khung file** (theo phần "Khảo sát" của HuongdanXDHTTTQL: đặt vấn đề, mô tả hệ thống cũ và nhược điểm, đề xuất hệ thống mới và yêu cầu, phạm vi; đầu ra gồm biểu mẫu, báo cáo, dữ liệu, quy trình):
```
# 01. Khảo sát
## 1. Đặt vấn đề và ý nghĩa                         [Chung – TV1]
## 2. 4W+1H toàn hệ thống                            [Chung – TV1]
## 3. Hiện trạng và nhược điểm
### 3.1 Chăm sóc & hỗ trợ khách hàng                 [TV1]
### 3.2 Quan hệ khách hàng & khách hàng thân thiết   [TV2]
## 4. Biểu mẫu thu thập
### 4.1 BM1–BM3                                      [TV1]
### 4.2 BM4–BM7                                      [TV2]
## 5. Yêu cầu chức năng
### 5.1 F1–F4                                        [TV1]
### 5.2 F5–F8                                        [TV2]
## 6. Yêu cầu phi chức năng                          [Chung – TV1]
## 7. Phạm vi hệ thống                               [Chung – TV1]
```

**[TV1] — Kessential**
- Mục 1: vì sao khách sạn cần CRM (ticket rơi rớt, không đo hài lòng, không biết khách thân thiết); ý nghĩa của hệ thống.
- Mục 2: 4W+1H: WHAT (hệ thống quản lý gì), WHO (4 vai trò + khách), WHERE (quầy lễ tân, bộ phận CSKH, ban quản lý), WHEN (khi khách phản ánh, khi trả phòng, khi chạy chiến dịch), HOW (quy trình hiện tại bằng sổ, Excel, điện thoại). Thêm bảng tóm tắt đầu ra khảo sát: biểu mẫu (BM1–BM7), báo cáo (R1–R5), dữ liệu (D1–D11), quy trình (L1–L6).
- Mục 3.1: quy trình tiếp nhận, phân công, xử lý, khảo sát hiện tại; ít nhất 3 nhược điểm, mỗi nhược điểm chỉ ra Q hoặc F nào sẽ giải quyết.
- Mục 4.1: BM1 Phiếu yêu cầu/khiếu nại, BM2 Phiếu theo dõi xử lý, BM3 Phiếu khảo sát hài lòng. Mỗi BM: ảnh/bản dựng lại, mục đích, ai lập, khi nào, bảng trường → cột CSDL dự kiến (`tickets`, `ticket_logs`, `surveys`, `survey_answers`) → Form (frm102, frm104, frm105).
- Mục 5.1: bảng F1.1, F1.2, F2.1, F3.1, F3.2, F4.1, F4.2: mã, tên, mô tả, vai trò dùng, Form, UC.
- Mục 6: phi chức năng: web chạy tại máy, mở bằng trình duyệt ở `http://localhost:5080` (publish self-contained, không cài .NET, không cần chứng chỉ HTTPS, chỉ nghe localhost), chỉ dùng trên máy tính, SQLite một file, đăng nhập + mật khẩu băm PBKDF2, phân quyền ở Service, giao diện tiếng Việt, tiền `#,##0`, ngày `dd/MM/yyyy`, thời gian mở báo cáo, không mất dữ liệu khi lỗi giữa chừng (transaction).
- Mục 7: trong phạm vi (F1–F8); ngoài phạm vi: các mục "Đã bỏ" ở PLAN mục 3 (đặt nhiều phòng, dịch vụ phát sinh, hóa đơn, công nợ, doanh thu/công suất phòng…).
- Mã liên quan: F1–F4, BM1–BM3, R1–R2, L1–L2, D1–D4.

**[TV2] — munn8726**
- Mục 3.2: quy trình hiện tại quản lý hồ sơ khách, đặt/nhận/trả phòng, tích điểm, đổi quà, gọi chăm sóc; ít nhất 3 nhược điểm, mỗi nhược điểm chỉ ra Q hoặc F nào sẽ giải quyết.
- Mục 4.2: BM4 Phiếu đăng ký thành viên/hồ sơ khách, BM5 Phiếu đặt phòng/nhận–trả phòng, BM6 Phiếu đổi quà, BM7 Danh sách chăm sóc theo chiến dịch. Mỗi BM: ảnh/bản dựng lại, mục đích, ai lập, khi nào, bảng trường → cột CSDL dự kiến (`customers`, `bookings`, `reward_redemptions`, `campaign_targets`) → Form (frm201, frm203, frm205, frm206).
- Mục 5.2: bảng F5.1, F5.2, F6.1, F6.2, F7.1, F7.2, F8.1, F8.2: mã, tên, mô tả, vai trò dùng, Form, UC.
- Mã liên quan: F5–F8, BM4–BM7, R3–R5, L3–L6, D5–D11.

**Điều kiện xong (mỗi người, phần mình):** đủ mục mang nhãn của mình; mỗi BM có bảng trường → cột → Form; mỗi chức năng lá có Form và UC; PR đã merge; PROGRESS cập nhật.

### 3.2 M3 — BFD + Use Case (hạn 24/11)
**File đích:** `docs/analysis/02-bfd.md`, `docs/analysis/03-use-case.md`; sơ đồ `diagrams/bfd.puml` (+ `bfd-tv1.iuml`, `bfd-tv2.iuml`), `diagrams/use-case.puml` (+ `use-case-tv1.iuml`, `use-case-tv2.iuml`).

**Khung `02-bfd.md`:**
```
# 02. Sơ đồ phân cấp chức năng (BFD)
## 1. Cây BFD (ảnh)                                  [Chung – TV1]
## 2. Nút lá ↔ Form ↔ UC ↔ Service
### 2.1 F1–F4                                        [TV1]
### 2.2 F5–F8                                        [TV2]
## 3. Đối chiếu HuongdanXDHTTTQL                     [Chung – TV1]
```
**Khung `03-use-case.md`:**
```
# 03. Use Case
## 1. Tác nhân                                       [Chung – TV1]
## 2. Sơ đồ Use Case tổng (ảnh)                      [Chung – TV1]
## 3. Danh sách Use Case
### 3.1 UC01–UC06                                    [TV1]
### 3.2 UC07–UC11                                    [TV2]
## 4. Luồng liên phân hệ L1–L6                       [Chung – TV1]
## 5. Đặc tả Use Case phức hợp
### 5.1 UC04 Xử lý ticket                            [TV1]
### 5.2 UC05 Ghi khảo sát hài lòng                   [TV1]
### 5.3 UC08 Đặt phòng & lưu trú                     [TV2]
### 5.4 UC09 Đổi quà                                 [TV2]
### 5.5 UC10 Chiến dịch chăm sóc                     [TV2]
```
**Mẫu đặc tả UC** (dùng chung): Mã, tên · Tác nhân · Mô tả ngắn · Tiền điều kiện · Luồng chính (đánh số) · Luồng phụ / ngoại lệ (mỗi ngoại lệ ghi loại ngoại lệ + thông báo tiếng Việt) · Hậu điều kiện · Quy tắc Q liên quan · Bảng dữ liệu đọc/ghi · Form · Biểu mẫu · Luồng liên phân hệ (L) và interface dùng.

**[TV1]**
- `bfd.puml`: gốc "Hệ thống CRM khách sạn" → 8 nhánh F1–F8; `bfd-tv1.iuml`: lá F1.1, F1.2, F2.1, F3.1, F3.2, F4.1, F4.2 (PLAN 4.2). Mỗi lá ↔ 1 Form (Huongdan: chức năng chương trình, không phải chức năng nghiệp vụ).
- `02-bfd.md` mục 1, 2.1, 3. Mục 3: Huongdan gợi ý khoảng 5 nhánh, 3 cấp, gồm 3 nhánh chung (Quản trị hệ thống, Quản lý danh mục, Báo cáo thống kê). Nhóm chốt 27/11 giữ 8 nhánh theo phân hệ, danh mục và báo cáo nằm trong từng nhánh; ghi lập luận (slide chỉ ghi "thông thường"; checklist đòi ≥3 nhánh/TV; chia dọc dễ chứng minh phần của từng người).
- `use-case.puml`: khai báo 4 tác nhân + `!include` 2 file; `use-case-tv1.iuml`: UC01–UC06 và quan hệ với tác nhân (PLAN 4.3).
- `03-use-case.md` mục 1, 2, 3.1, 4 (bảng L1–L6 như PLAN 4.4, thêm cột UC), 5.1, 5.2.
- UC04: 4 thao tác bắt đầu/giải quyết/đóng/mở lại; Q1, Q2, Q6, Q12; L2 (đóng ticket → observer TV2, 1 transaction). UC05: Q5; phiếu Chờ do L3 tạo.
- Mã: F1–F4, UC01–UC06, Q1–Q6, L1, L2, BM1–BM3.

**[TV2]**
- `bfd-tv2.iuml`: lá F5.1, F5.2, F6.1, F6.2, F7.1, F7.2, F8.1, F8.2; `02-bfd.md` mục 2.2.
- `use-case-tv2.iuml`: UC07–UC11 và quan hệ với tác nhân; `03-use-case.md` mục 3.2, 5.3, 5.4, 5.5.
- UC08: đặt, nhận, trả, hủy; Q8, Q9, Q6; L3 (trả phòng → `ISurveyScheduler` của TV1, 1 transaction). UC09: Q10. UC10: Q11; L4 (`ITicketQuery`), Strategy phân khúc.
- Mã: F5–F8, UC07–UC11, Q7–Q13, L3–L6, BM4–BM7.

**Điều kiện xong:** sơ đồ của mình xuất ảnh được (hoặc `.puml` hợp lệ nếu chưa có Java); mỗi lá BFD có đúng 1 Form trùng PLAN 5.1; đủ cột UC theo PLAN 4.3; mỗi đặc tả UC đủ các mục của mẫu; PR merge; PROGRESS cập nhật.

### 3.3 M4 — Quy tắc, quyền, DFD (hạn 25/11)
**File đích:** `docs/analysis/04-quy-tac.md`, `docs/analysis/05-dfd.md`; sơ đồ `diagrams/dfd-context.*`, `diagrams/dfd-level0.*`, `diagrams/dfd-level1-f3.*`, `diagrams/dfd-level1-f7.*` (`.drawio` + `.png`, hoặc `.puml` + `.png`).

**Khung `04-quy-tac.md`:**
```
# 04. Quy tắc nghiệp vụ và phân quyền
## 1. Vai trò và ma trận quyền                       [Chung – TV1]
## 2. Loại ngoại lệ                                  [Chung – TV1]
## 3. Quy tắc phân hệ CSKH Q1–Q6                     [TV1]
## 4. Quy tắc phân hệ quan hệ khách hàng Q7–Q13      [TV2]
```
**Mẫu một quy tắc** (bảng hoặc khối): Mã · Phát biểu · Tiền điều kiện · Hậu điều kiện · Vi phạm → loại ngoại lệ + thông báo tiếng Việt · Lớp/hàm dự kiến (PLAN 5.3) · Liên phân hệ (interface nào) · Ca test (TC, điền ở M8) · Giá trị biên (nếu có).

**Khung `05-dfd.md`:**
```
# 05. Sơ đồ luồng dữ liệu (DFD)
## 1. Ký hiệu, tác nhân, kho dữ liệu                 [Chung – TV1]
## 2. DFD ngữ cảnh                                   [Chung – TV1]
## 3. DFD mức 0                                      [Chung – TV1]
## 4. DFD mức 1 — F3 Điều phối & xử lý               [TV1]
## 5. DFD mức 1 — F7 Khách hàng thân thiết           [TV2]
```

**[TV1]**
- `04-quy-tac.md` mục 1 (ma trận PLAN 4.1, giải thích Admin vượt quyền có chủ đích, kiểm quyền ở Service, ẩn nút là lớp thứ hai), mục 2 (cây ngoại lệ PLAN 5.3/5.6), mục 3 Q1–Q6. Q1 ghi rõ `ReopenDays`; Q2 ghi `MaxActiveTickets` và cách chọn người ít việc nhất; Q3 ghi công thức hạn SLA và bảng hệ số; Q6 là quy tắc chung (TV1 viết nền, mỗi service của TV2 tự kiểm quyền).
- `05-dfd.md` mục 1 (danh mục ở 1.5 của file này), 2, 3 (8 tiến trình 1.0–8.0, kho D1–D11), 4 (F3 tách thành 3.1 Tra cứu & phân công, 3.2 Xử lý ticket & nhật ký; luồng ra `ITicketObserver` sang D6, D9 của TV2).
- Mã: Q1–Q6, F3, D1–D4.

**[TV2]**
- `04-quy-tac.md` mục 4 Q7–Q13. Q7 ghi giá trị biên (SĐT 10 số bắt đầu 0; quốc tịch Việt Nam CCCD 12 chữ số, quốc tịch khác hộ chiếu 6–9 ký tự chữ và số — xem `docs/PROGRESS.md` mục 8); Q8 ghi khoảng nhận phòng [ngày nhận, ngày trả); Q9 ghi công thức điểm (theo hạng trước khi trả phòng — PROGRESS mục 8), bảng hệ số tích điểm và ngưỡng hạng; Q10 ghi 3 điều kiện đổi quà; Q11 công thức danh sách chiến dịch (ít nhất một khách; khách mất điều kiện lúc lưu thì bị loại — PROGRESS mục 8); Q12 bảng điểm thiện chí (khiếu nại 50, loại khác 10); Q13 điều kiện xóa khách.
- `05-dfd.md` mục 5: F7 tách thành 7.1 Danh mục quà tặng, 7.2 Sổ điểm & đổi quà; kho D5, D9, D10; luồng vào từ 6.0 (điểm lưu trú) và từ 3.0 (điểm thiện chí qua observer).
- Mã: Q7–Q13, F7, D5, D9, D10.

**Điều kiện xong:** mỗi quy tắc đủ các mục của mẫu; mỗi tiến trình DFD có vào/ra; tên kho/tác nhân đúng 1.5; DFD mức 1 cân bằng với mức 0; PR merge; PROGRESS cập nhật.

### 3.4 M5 — ERD + CSDL (hạn 26/11)
**File đích:** `docs/analysis/06-erd.md`; sơ đồ `diagrams/erd.puml` (+ `erd-tv1.iuml`, `erd-tv2.iuml`).

**Khung `06-erd.md`:**
```
# 06. Sơ đồ thực thể liên kết và CSDL
## 1. ERD tổng (ảnh)                                 [Chung – TV1]
## 2. Bảng phân hệ CSKH (7 bảng)                     [TV1]
## 3. Bảng phân hệ quan hệ khách hàng (10 bảng)      [TV2]
## 4. Khóa ngoại nối hai phân hệ                     [Chung – TV1 + TV2, mỗi người dòng của bảng mình]
## 5. Chuẩn hóa 3NF                                  [Chung – TV1]
## 6. Thuộc tính lịch sử                             [TV2]
## 7. Đối chiếu định mức (PLAN 5.10)                 [Chung – TV1]
```
**Mẫu một bảng:** tên bảng · loại (danh mục / giao dịch / n–n) · cột (tên, kiểu SQLite, NULL?, PK/FK/UNIQUE/CHECK, ý nghĩa tiếng Việt) · ON DELETE (CASCADE cho quan hệ hợp thành, còn lại RESTRICT) · index.

**[TV1]**
- `erd.puml` khung (`hide circle`, `skinparam linetype ortho`, `!include` 2 file); `erd-tv1.iuml`: `departments`, `employees`, `tickets`, `ticket_logs`, `survey_criteria`, `surveys`, `survey_answers` (PLAN 5.2). Đường quan hệ do bảng chứa khóa ngoại vẽ: TV1 vẽ `tickets.CustomerId → customers`, `tickets.RoomId → rooms`, `surveys.BookingId → bookings`.
- `06-erd.md` mục 1, 2, phần TV1 của mục 4, mục 5 (vì sao `surveys` không lưu `CustomerId`; `ticket_logs`, `survey_answers` là n–n có thuộc tính), mục 7.

**[TV2]**
- `erd-tv2.iuml`: `customers`, `interactions`, `room_types`, `rooms`, `bookings`, `point_transactions`, `rewards`, `reward_redemptions`, `campaigns`, `campaign_targets`. TV2 vẽ `interactions.TicketId → tickets`, `point_transactions.TicketId → tickets` và các cột `…ById → employees`.
- `06-erd.md` mục 3, phần TV2 của mục 4, mục 6 (`bookings.RatePerNight` chốt lúc đặt, `reward_redemptions.PointsSpent` chốt lúc đổi: vì sao không tính lại từ bảng danh mục).

**Điều kiện xong:** đủ 17 bảng, ≥3 n–n có thuộc tính (đạt 4), đủ 5 khóa nối hai phân hệ, danh mục ≤40% (4/17); `erd.puml` xuất một ảnh tổng; PR merge; PROGRESS cập nhật.

### 3.5 M6 — Lớp + package + hợp đồng interface (hạn 27/11)
**File đích:** `docs/analysis/07-thiet-ke-lop.md`; sơ đồ `diagrams/class.puml` (+ `class-common.iuml`, `class-tv1.iuml`, `class-tv2.iuml`), `diagrams/package.puml`.

**Khung `07-thiet-ke-lop.md`:**
```
# 07. Thiết kế lớp
## 1. Sơ đồ lớp tổng (ảnh)                           [Chung – TV1]
## 2. Lớp phần chung                                 [Chung – TV1]
## 3. Lớp phân hệ CSKH                               [TV1]
## 4. Lớp phân hệ quan hệ khách hàng                 [TV2]
## 5. Package diagram và chiều phụ thuộc             [Chung – TV1]
## 6. Hợp đồng interface giữa hai phân hệ
### 6.1 Interface TV1 khai báo                       [TV1]
### 6.2 Interface TV2 khai báo                       [TV2]
### 6.3 Biên bản chốt (27/11)                        [Chung – TV1 + TV2]
## 7. Mẫu GoF
### 7.1 State, Factory Method                        [TV1]
### 7.2 Strategy                                     [TV2]
### 7.3 Observer (cầu nối)                           [Chung – TV1 phát, TV2 nhận]
## 8. Cây kế thừa và LSP                             [mỗi người cây của mình]
```

**[TV1]**
- `class-common.iuml`: `Person`*, `Employee`* + `Administrator`, `Manager`, `Agent`, `Department`, cây ngoại lệ, `IClock`, `IUnitOfWork`. `Receptionist` do TV2 vẽ trong `class-tv2.iuml`.
- `class-tv1.iuml`: các lớp TV1 ở PLAN 5.5 (`SupportTicket`* + 3 lớp con, `TicketState`* + 4 state, `TicketLog`, `SatisfactionSurvey`, `SurveyAnswer`, `SurveyCriterion`, `TicketFactory`* + 3 factory, enum). Thể hiện đủ 6 quan hệ (kế thừa, cài đặt, kết hợp, kết tập, hợp thành, phụ thuộc).
- `package.puml`: GUI, BLL, Domain, DAL (PLAN 5.6), module TV1/TV2/chung bên trong; không có phụ thuộc vòng.
- Mục 6.1: chữ ký C# đầy đủ của `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`, `IUnitOfWork`, `IClock` (tên hàm, tham số, kiểu trả về, ngoại lệ có thể ném, ai gọi, ai cài).
- Mục 7.1: vai trò từng lớp trong mẫu, vấn đề giải quyết, bài Live OCP (`LostItemTicket`), ≥2 nhược điểm mỗi mẫu. Mục 7.3 phần phát sự kiện.
- Mục 8: cây `Employee`, `SupportTicket`, `TicketState`; LSP xuyên phân hệ (`Receptionist` đi qua `TicketService` dưới kiểu `Employee`).

**[TV2]**
- `class-tv2.iuml`: các lớp TV2 ở PLAN 5.5 (`Customer`, `MembershipTier`* + 4 hạng, `Interaction`, `RoomType`, `Room`, `Booking`, `PointTransaction`, `Reward`, `RewardRedemption`, `Campaign`, `CampaignTarget`, `Receptionist`, `ISegmentStrategy` + 3 phân khúc, enum).
- Code Domain trong PR #2 (`tv2/domain`) đã có một phần các lớp này: sơ đồ phải khớp code. Chỗ code khác PLAN thì ghi vào "Quyết định lệch khỏi plan" của `docs/PROGRESS.md` để cả hai duyệt.
- Mục 6.2: chữ ký C# đầy đủ của `ITicketQuery`, `ISurveyScheduler`, `ICustomerCareStats`, `IReportExporter`, `ISegmentStrategy`.
- Mục 7.2: Strategy (Context `CampaignService`, chọn phân khúc trên Form lúc chạy), Live OCP (`FrequentGuestSegment`), ≥2 nhược điểm. Mục 7.3 phần nhận (`GoodwillPointsObserver`, `InteractionLogger`).
- Mục 8: cây `MembershipTier`.

**Chốt hợp đồng 27/11 (cả hai):** đọc chéo mục 6.1 và 6.2, sửa đến khi cả hai đồng ý; ghi biên bản ở 6.3 (ngày, chữ ký đã chốt, ai duyệt). File `.cs` của interface viết ở P2 bởi bên khai báo, khớp đúng biên bản. Đổi sau ngày chốt phải qua PR có cả hai duyệt.

**Điều kiện xong:** sơ đồ lớp tổng ≥20 lớp thực chất (không lớp rỗng), có đủ 6 loại quan hệ; package không vòng; 6.3 có biên bản; PR merge; PROGRESS cập nhật (đánh dấu cả dòng "Chốt chữ ký interface" ở mục 5 của PROGRESS).

### 3.6 M7 — Hành vi (hạn 28/11)
**File đích:** `docs/analysis/08-hanh-vi.md`; sơ đồ `diagrams/seq-l2-close-ticket.puml`, `diagrams/state-ticket.puml` (TV1), `diagrams/seq-l3-checkout.puml`, `diagrams/state-booking.puml` (TV2).

**Khung `08-hanh-vi.md`:**
```
# 08. Sơ đồ hành vi
## 1. Tuần tự L2 — Đóng ticket (xuyên phân hệ)      [TV1]
## 2. Máy trạng thái ticket                          [TV1]
## 3. Tuần tự L3 — Trả phòng (xuyên phân hệ)         [TV2]
## 4. Máy trạng thái booking                         [TV2]
```

**[TV1]**
- L2: `frm104` → `TicketService.Close` → kiểm quyền `actor` → `IUnitOfWork.Execute` → repository nạp ticket → `SupportTicket` / `ResolvedState` → `ClosedState` → ghi `ticket_logs` → phát sự kiện tới từng `ITicketObserver` (`GoodwillPointsObserver`, `InteractionLogger` của TV2, vẽ ở mức lời gọi interface + bảng ghi) → commit. Khung `alt` lỗi ở bước TV2 → rollback toàn bộ. Tên hàm khớp M6.
- Máy trạng thái ticket: Mới, Đang xử lý, Đã giải quyết, Đã đóng; guard ghi theo Q1 (có người được giao, có ghi chú, trong `ReopenDays`); Đã đóng là trạng thái cuối.
- Mỗi sơ đồ kèm đoạn giải thích: nơi đặt breakpoint khi vấn đáp, Call Stack mong đợi.

**[TV2]**
- L3: `frm203` → `StayService.CheckOut` → kiểm quyền → `IUnitOfWork.Execute` → `Booking` → Đã trả phòng → `Customer.EarnPoints` (điểm, nâng hạng) → ghi `point_transactions`, `interactions` → `ISurveyScheduler` (TV1, vẽ ở mức lời gọi interface) tạo phiếu Chờ → commit. Khung `alt` lỗi ở `SurveyScheduler` → rollback toàn bộ.
- Máy trạng thái booking theo `BookingStatus` (đặt, đang ở, đã trả phòng, đã hủy); guard theo Q8.
- Mỗi sơ đồ kèm đoạn giải thích breakpoint và Call Stack.

**Điều kiện xong:** mỗi người 2 sơ đồ, sơ đồ tuần tự xuyên phân hệ có `alt` rollback; tên lớp/hàm trùng M6; PR merge; PROGRESS cập nhật.

### 3.7 M8 — Giao diện + kiểm thử (hạn 28/11)
**File đích:** `docs/analysis/09-giao-dien.md`, `docs/analysis/10-kiem-thu.md`.

**Khung `09-giao-dien.md`:**
```
# 09. Thiết kế giao diện
## 1. Quy ước giao diện chung                        [Chung – TV2]
## 2. Đăng nhập và màn hình chính (frm000, frm001)   [Chung – TV1]
## 3. Form phân hệ CSKH (frm101–frm106)              [TV1]
## 4. Form phân hệ quan hệ khách hàng (frm201–frm207) [TV2]
## 5. Đối chiếu tên Form với HuongdanXDHTTTQL        [Chung – TV1]
```
**Mẫu một trang:** tên trang (`frm…`, đường dẫn) · nút BFD · UC · loại (danh mục / nghiệp vụ / báo cáo; Master-Detail?) · ảnh bản mẫu web hoặc phác thảo bố cục · bảng: cột (cột nào ↔ trường biểu mẫu nào) · bộ lọc · nút và vai trò được bấm (nút bị khoá ghi lý do) · modal thêm/sửa · kiểm tra ở trang (báo lỗi cạnh ô) · handler (`OnPost…`) → Service/hàm gọi · tổng tính động (nếu Master-Detail).

**Khung `10-kiem-thu.md`:**
```
# 10. Kiểm thử
## 1. Chiến lược (unit / integration, Fake, IClock, tiêm lỗi) [Chung – TV1]
## 2. Ma trận ca test TC1-01…TC1-14                  [TV1]
## 3. Ma trận ca test TC2-01…TC2-14                  [TV2]
## 4. Bảng tính tay R2                               [TV1]
## 5. Bảng tính tay R5                               [TV2]
## 6. Kế hoạch tiêm lỗi                              [mỗi người dòng của mình]
```
**Cột ma trận test:** TC · loại (Unit/Integration) · kỹ thuật (EP, BVA, State, Ngoại lệ, RBAC, Rollback) · Q/UC/R/L liên quan · dữ liệu vào · kết quả mong đợi · Fake dùng · lớp test dự kiến.

**[TV1]**
- `09-giao-dien.md` mục 2, 3 (frm101–frm106 theo PLAN 5.1; Master-Detail: frm101, frm104, frm105; frm103 đủ 6 bộ lọc + nút xóa lọc; frm106 R1, R2 có biểu đồ, xuất Excel), mục 5 (đối chiếu với Huongdan: slide đánh số liên tiếp `frm001_dm_…`, `frm002_nv_…`; nhóm chốt 27/11 đánh số theo người, tên tiếng Anh, thêm `_bc_` cho trang báo cáo; bảng tên cũ → tên mới theo `docs/PLAN.md` mục 5.1).
- `10-kiem-thu.md` mục 1, 2 (10 unit + 4 integration ở PLAN 5.8 cột TV1, đủ BVA cho `ReopenDays`, `MaxActiveTickets`, điểm 0/1/5/6), 4 (bộ dữ liệu nhỏ dùng cho integration test, tính tay từng ô của R2: số ticket, số trễ SLA theo loại/tháng/hạng), dòng TV1 ở mục 6.
- Điền cột "Ca test" cho Q1–Q6 trong `04-quy-tac.md`.

**[TV2]**
- `09-giao-dien.md` mục 1 (quy ước giao diện web theo `docs/PLAN.md` mục 5.1: bố cục, tông màu, font, modal "Thực hiện"/"Hủy bỏ", bảng dùng chung, báo lỗi cạnh ô, toast, tiền `#,##0` căn phải, ngày `dd/MM/yyyy`; để hai phân hệ trông như một tác giả), mục 4 (frm201–frm207; Master-Detail: frm201, frm202, frm205, frm206; frm201 lọc từ khóa/hạng/quốc tịch/đồng ý ưu đãi; frm207 R3 có biểu đồ, R4, R5).
- `10-kiem-thu.md` mục 3 (10 unit + 4 integration ở PLAN 5.8 cột TV2, đủ BVA độ dài SĐT/CCCD, ngày nối tiếp, ngưỡng hạng), mục 5 (bộ dữ liệu nhỏ, tính tay R5 theo khách: số lần ở, chi tiêu, số khiếu nại, điểm hài lòng TB; không tính booking đã hủy), dòng TV2 ở mục 6.
- Điền cột "Ca test" cho Q7–Q13 trong `04-quy-tac.md`.

**Điều kiện xong:** mỗi Form trong PLAN 5.1 có một mục theo mẫu; mỗi người 14 ca test, ≥2 ca dùng Fake interface của người kia; bảng tính tay có số cụ thể (không ghi công thức thay số); PR merge; PROGRESS cập nhật.

### 3.8 M9 — Slide + tự đánh giá L2 (hạn 29/11, nộp 30/11)
**File đích:** `docs/slides/*.tex`, ảnh trong `docs/slides/img/`; `docs/analysis/11-tu-danh-gia-l2.md`; bảng tự đánh giá `docs/slides/Checklist_BTL_HTTTQL_v2026_TuDanhGia_L2.xlsx`.

**Khung `11-tu-danh-gia-l2.md`:**
```
# 11. Tự đánh giá L2 (sheet "Nhóm 02 người")
## 1. Tự kê khai khối lượng                          [Chung – TV1]
## 2. Minh chứng tiêu chí phần TV1                   [TV1]
## 3. Minh chứng tiêu chí phần TV2                   [TV2]
## 4. Điểm tự chấm và việc còn thiếu                 [Chung – TV1 + TV2]
```

**[TV2] (làm trước, vì là chủ bộ Beamer)**
- Đầu M9 chốt cấu trúc bộ slide: phần của TV1 mỗi phần một file `.tex` riêng được `\input`, để TV1 chỉ sửa file của mình. Báo cấu trúc cho TV1 trong PR hoặc tin nhắn.
- Sửa đường dẫn cũ trong `sec1-khaosat.tex`, `sec3-thietke.tex`, `sec5-kiemthu.tex`, `sec6-tudanhgia.tex`: các file v1 này đã xóa khỏi cây thư mục. Câu còn đúng với v2 thì trỏ sang `docs/PLAN.md`, `docs/PROGRESS.md` v2; câu nói về v1 thì ghi "kế hoạch/tiến độ v1 (lịch sử git, commit `fd7b7e4`)".
- Slide phần TV2: khảo sát BM4–BM7, BFD F5–F8, UC08–UC10, Q7–Q13, ERD 10 bảng, lớp TV2, Strategy, L3, trạng thái booking, Form F5–F8, ma trận TC2.
- Mục 3 của `11-tu-danh-gia-l2.md`; tạo file `.xlsx` L2 từ bản L1, điền cột Minh chứng với tiền tố `[TV1]`/`[TV2]` theo mục 2–3 của file `.md`. Biên dịch PDF cả bộ.

**[TV1]**
- Slide phần chung + TV1: 4W+1H, BFD tổng, UC tổng, DFD, ma trận quyền, kiến trúc/package, 1 slide kiến trúc web (trình duyệt → Razor Pages → BLL → Domain ← DAL → SQLite), BM1–BM3, Q1–Q6, ERD 7 bảng, lớp chung + TV1, State + Factory Method, Observer, L2, trạng thái ticket, Form F1–F4, ma trận TC1.
- Mục 1, 2 của `11-tu-danh-gia-l2.md` (đối chiếu từng hạng mục PLAN 5.10).

**Điều kiện xong:** slide biên dịch PDF không lỗi trên máy TV2; mỗi slide có nhãn [TV1]/[TV2]; mọi ảnh sơ đồ lấy từ `docs/analysis/diagrams/*.png`; `.xlsx` L2 điền đủ cột Minh chứng; PR merge; PROGRESS cập nhật; nộp 30/11.

## 4. Kiểm tra cuối P1 (PLAN mục 8)
- Mọi `.puml` xuất ảnh không lỗi.
- Bảng 5.10 đạt hết (ghi ở `11-tu-danh-gia-l2.md` mục 1).
- Mỗi nút lá BFD có Form, Service, UC tương ứng.
- Mỗi PR tài liệu đã được người kia review.
- Slide biên dịch được.
