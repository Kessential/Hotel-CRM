# PROGRESS — Hotel CRM v2

Kế hoạch: `docs/PLAN.md` (v2). Việc chi tiết giai đoạn đang làm: `docs/tasks/v2-p1-analysis.md`. Tiến độ v1 (15–21/11) chỉ còn trong lịch sử git: `git show fd7b7e4:docs/PROGRESS.md`.

Cách cập nhật: mỗi người sửa dòng/ô của mình trong PR của mình, sau mỗi module (đánh dấu `[x]`, đổi cột trạng thái, thêm một dòng nhật ký). Không sửa dòng của người kia.

Cập nhật lần cuối: 28/11/2026 (PR #5 `tv2/domain-2`, TV2).

## 1. Trạng thái giai đoạn
| Giai đoạn | Thời gian (plan) | Trạng thái | Mốc kiểm cuối giai đoạn |
|---|---|---|---|
| P1 Phân tích thiết kế | 22–29/11, nộp slide 30/11 | **Đang làm**: phần TV2 xong (PR #3); phần TV1 làm dồn 27–29/11 (chốt 27/11) | mọi `.puml` xuất ảnh; bảng 5.10 đạt; slide biên dịch được |
| P2 Hạ tầng | 1–4/12 | Chưa làm (TV2 đã làm Domain sớm: PR #2 đã merge, đợt 2 trên `tv2/domain-2`) | đăng nhập 4 vai trò trên SQLite qua trình duyệt; bản publish chạy trên máy sạch (4/12); TV2 có 10 unit test xanh |
| P3 Phân hệ | 5–10/12 | Chưa làm | đủ 13 trang, R1–R4, seed ≥150 dòng |
| P4 Ghép & đóng gói | 11–12/12 | Chưa làm | 28 test xanh; chạy máy sạch ≤3 phút |
| P5 Vấn đáp | 13/12 → 30/12 | Chưa làm | buổi vấn đáp; nộp cuối 30/12 |

## 2. Trạng thái module P1
| Module | Hạn | TV1 | TV2 | Trạng thái (27/11) |
|---|---|---|---|---|
| M1 Chuẩn bị | 22/11 | **Xong 25/11** (trễ 3 ngày): `CLAUDE.md`, `docs/PLAN.md` v2, file này, `docs/tasks/v2-p1-analysis.md`, xóa tài liệu v1 khỏi cây thư mục, dọn đồ v1 | PR #1 đã merge (25/11); xác nhận phân công khi approve PR #4 (25/11) | **Xong 25/11**: PR #4 (`tv1/m1-setup` → `v2`) đã được TV2 approve và merge |
| M2 Khảo sát | 23/11 | Đã viết 27/11 (PR #8 `tv1/m2-khao-sat`; TV2 yêu cầu sửa 27/11, đã sửa 28/11; TV2 approve 28/11); BM1–BM3 là mẫu dựng lại, chưa có bản thật; nguồn khảo sát mục 3.1 là tài liệu đã công bố (khóa luận về khách sạn Lạc Long – Hải Phòng, OPERA 5.6, Oxmaint), chưa phỏng vấn trực tiếp | Đã viết (PR #3, TV1 approve 26/11); ảnh BM4–BM7 thật chưa có | **Xong 28/11** (PR #3, PR #8); còn ảnh biểu mẫu thật (mục 7) |
| M3 BFD + UC | 24/11 | Chưa làm; làm 27/11 | Đã viết (PR #3, TV1 approve 26/11) | Quá hạn; phần TV1 làm 27/11 |
| M4 Quy tắc, quyền, DFD | 25/11 | Chưa làm; làm 28/11 | Đã viết (PR #3, TV1 approve 26/11) | Quá hạn; phần TV1 làm 28/11 |
| M5 ERD + CSDL | 26/11 | Chưa làm; làm 28/11 | Đã viết (PR #3, TV1 approve 26/11) | Quá hạn; phần TV1 làm 28/11 |
| M6 Lớp + package + hợp đồng | 27/11 | Chưa làm; làm 28/11 | Đã viết (PR #3, TV1 approve 26/11); còn chốt hợp đồng 27/11; code Domain trong PR #2 | Phần TV1 làm 28/11 |
| M7 Hành vi | 28/11 | Chưa làm; làm 29/11 | Đã viết (PR #3, TV1 approve 26/11) | Phần TV1 làm 29/11 |
| M8 Giao diện + test | 28/11 | Chưa làm; làm 29/11 | Đã viết (PR #3, TV1 approve 26/11); `09-giao-dien.md` mục 1 và tên trang cần sửa theo web | Phần TV1 làm 29/11 |
| M9 Slide + tự đánh giá L2 | 29/11 | Chưa làm | Chưa làm | Chưa tới hạn; slide nộp **30/11 (không dời)** |

Lịch: giữ hạn slide 30/11, TV1 làm dồn phần mình của M2–M8 trong 27–29/11 (chốt 27/11, mục 8; lịch từng module ở `docs/tasks/v2-p1-analysis.md` mục 2).

## 3. Việc của TV1 (sao từ PLAN mục 6.4)
**Tài liệu & sơ đồ (P1)**
- [x] M1: `CLAUDE.md`, `docs/PLAN.md` v2, `docs/PROGRESS.md` v2, file task P1, nhánh `v2` (22/11) — xong 25/11
- [x] 4W+1H toàn hệ thống; yêu cầu phi chức năng; biểu mẫu BM1 Phiếu yêu cầu/khiếu nại, BM2 Phiếu theo dõi xử lý, BM3 Phiếu khảo sát (23/11) — PR `tv1/m2-khao-sat` (27/11); ảnh biểu mẫu thật chưa có (mục 7)
- [ ] Cây BFD 8 nhánh, lá F1–F4; sơ đồ Use Case tổng; đặc tả UC04, UC05 (24/11)
- [ ] Q1–Q6 với tiền/hậu điều kiện và ngoại lệ; ma trận quyền; DFD ngữ cảnh, mức 0, mức 1 của F3 (25/11)
- [ ] Khung `erd.puml` + 7 bảng TV1; giải thích 3NF (26/11)
- [ ] Sơ đồ lớp phần chung + TV1; package diagram; khai báo `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`; chốt hợp đồng với TV2 (27/11)
- [ ] Sơ đồ tuần tự L2 (đóng ticket, có khung alt rollback); máy trạng thái ticket (28/11)
- [ ] Bảng Form F1–F4; ma trận test TC1-xx; bảng tính tay R2 (28/11)
- [ ] Slide phần chung + TV1 + 1 slide kiến trúc web (29/11)
- [x] Đưa quyết định chuyển sang web vào `CLAUDE.md`, `docs/PLAN.md`, `docs/PROGRESS.md`, file task P1 (27/11) — PR `tv1/web-plan`, chờ TV2 review

**Nền chung (P2, 1–4/12)**
- [ ] Đổi tên project thành `HotelCRM.BLL`, `HotelCRM.DAL` bằng commit chỉ có `git mv`; xoá code v1 không còn dùng của TV1 (WinForms, JSON, 3 strategy phân công cũ, `UserSession`, `UiNotificationObserver`); thêm `Database/`, `Resource/` (1/12)
- [ ] Trên khung web của TV2: đăng nhập cookie (frm000), lọc menu theo vai trò, nội dung trang chủ (frm001), trang lỗi chung; đưa lên trước 2/12
- [ ] SQLite: kết nối, `UnitOfWork` theo từng request, WAL + `busy_timeout`, tạo CSDL từ `schema.sql` + nạp `seed.sql`; một repository mẫu để TV2 làm theo
- [ ] Cây ngoại lệ, `ErrorHandler` tiếng Việt, `appsettings.json` + file mẫu (có `Urls`), bỏ Singleton, `IClock`
- [ ] `Employee` + `Administrator`, `Manager`, `Agent`
- [ ] `schema.sql` 7 bảng TV1; seed tài khoản 4 vai trò; `.editorconfig` (khối `Pages/`), `.gitattributes`; sửa `.claude/rules/*` theo v2 (`winforms.md` → `web.md`)
- [ ] Publish self-contained và chạy thử trên máy sạch (4/12)

**Phân hệ CSKH (P3, 5–10/12)**
- [ ] Domain: `SupportTicket` + 3 loại ticket, 4 state (bảng đăng ký thay `switch`), `TicketLog`, `SatisfactionSurvey`, `SurveyAnswer`, `SurveyCriterion`
- [ ] Repository + truy vấn báo cáo cho 7 bảng TV1
- [ ] `TicketService` (tạo, phân công, xử lý, nhật ký, phát sự kiện), 3 factory, `SurveyService`
- [ ] Cài cho TV2 dùng: `TicketQuery`, `SurveyScheduler`, `CustomerCareStats` (8/12)
- [ ] Trang frm101–frm106 (Master-Detail: frm101, frm104, frm105) dùng thành phần chung của TV2; báo lỗi cạnh ô; tiền/ngày đúng định dạng; khối số liệu CSKH trên frm001
- [ ] R1, R2 (Chart.js), xuất Excel (tải file)
- [ ] Seed 7 bảng: đủ 3 loại ticket, đủ trạng thái, có ticket quá hạn SLA, nhật ký, phiếu khảo sát chờ/đã làm

**Test (P2–P4)**
- [ ] `FakeCustomerDirectory`, `FakeStayLookup`
- [ ] 10 unit test (PLAN mục 5.8)
- [ ] 4 integration: R2 khớp tính tay; RBAC; L2 luồng đầy đủ; L2 rollback

**Đóng gói (P4, 11–12/12)**
- [ ] Publish self-contained, chạy thử lại trên máy sạch
- [ ] README: cài đặt, mở trình duyệt, tài khoản demo, lệnh chạy test

**Vấn đáp (P5)**
- [ ] Live OCP khi tắt AI: thêm `LostItemTicket` + factory, đăng ký 1 dòng (tập ≥2 lần)
- [ ] Đặt breakpoint theo sơ đồ L2 (của mình) và L3 (của TV2) khi app chạy dạng web, đọc Call Stack
- [ ] Trả lời câu hỏi chéo phần TV2: Strategy phân khúc, sổ điểm, vì sao hạng không hạ, rollback L3
- [ ] Nêu ≥2 nhược điểm của State và Factory Method
- [ ] Demo phần mình trong video; chương báo cáo phân hệ TV1 + chương kiến trúc

## 4. Việc của TV2 (sao từ PLAN mục 6.5)
Các dòng dưới chưa sửa theo quyết định chuyển sang web (phần dùng chung mở rộng, "Form" → trang, bỏ `ErrorProvider`); TV2 sửa dòng của mình trong PR của TV2, theo `docs/PLAN.md` mục 6.5.

**Tài liệu & sơ đồ (P1)**
- [x] Đọc plan, xác nhận phân công; cùng merge PR #1 (22/11) — PR #1 đã merge 25/11; xác nhận phân công khi approve PR #4 (25/11)
- [x] Hiện trạng phần khách hàng, lưu trú, điểm, chiến dịch; biểu mẫu BM4 Phiếu đăng ký thành viên, BM5 Phiếu đặt phòng/nhận–trả phòng, BM6 Phiếu đổi quà, BM7 Danh sách chăm sóc theo chiến dịch; yêu cầu chức năng F5–F8 (23/11) — PR #3; ảnh biểu mẫu thật chưa có (mục 7)
- [x] Lá BFD F5–F8; đặc tả UC08, UC09, UC10 (24/11) — PR #3
- [x] Q7–Q13 với tiền/hậu điều kiện và ngoại lệ; DFD mức 1 của F7 (25/11) — PR #3
- [x] Phần ERD 10 bảng TV2; giải thích thuộc tính lịch sử (giá/đêm chốt, điểm quy đổi chốt) (26/11) — PR #3
- [ ] Sơ đồ lớp TV2; khai báo `ITicketQuery`, `ISurveyScheduler`, `ICustomerCareStats`; chốt hợp đồng với TV1 (27/11) — sơ đồ lớp và khai báo đã viết (PR #3); còn chốt hợp đồng 27/11
- [x] Sơ đồ tuần tự L3 (trả phòng, có khung alt rollback); máy trạng thái booking (28/11) — PR #3
- [x] Bảng Form F5–F8; ma trận test TC2-xx; bảng tính tay R5 (28/11) — PR #3
- [ ] Slide phần TV2; biên dịch PDF cả bộ (29/11)

**Khung web (28/11)**
- [ ] Project `HotelCRM.GUI`, khung `Program.cs`, `_Layout` (thanh bên, thanh trên), menu `Navigation/AppMenu.cs`, `site.css`, `site.js` (28/11) — PR #10, chờ merge
- [ ] Toast đọc `TempData` cho thông báo lưu và lỗi từ `ErrorHandler` (trước 2/12)
- [ ] Thanh trên: bộ phận, vai trò người đăng nhập, nút Đăng xuất (sau khi TV1 có đăng nhập cookie, 2/12)

**Domain (P2, 1–4/12, chưa cần CSDL)**
- [x] Sửa `Customer` (CCCD, ngày sinh, đồng ý nhận ưu đãi), `MembershipTier` (hệ số tích điểm), `Booking` (giá/đêm chốt) — `MembershipTier` ở PR #2; `Customer`, `Interaction`, `Booking`, `Room` viết lại ở `tv2/domain-2` (26/11)
- [ ] Thêm `PointTransaction`, `Reward`, `RewardRedemption`, `Campaign`, `CampaignTarget`, `Receptionist` — đã có trừ `Receptionist` (chờ TV1 viết lại `Employee`)
- [x] `ISegmentStrategy` + `VipSegment`, `LapsedGuestSegment`, `BirthdayMonthSegment` — PR #2
- [ ] `schema.sql` 10 bảng TV2

**Phân hệ quan hệ khách hàng (P3, 5–10/12)**
- [ ] Repository + truy vấn báo cáo cho 10 bảng TV2 (theo repository mẫu của TV1)
- [ ] `CustomerService`, `StayService`, `LoyaltyService`, `CampaignService`, có kiểm quyền trong từng service
- [ ] Cài cho TV1 dùng: `CustomerDirectory`, `StayLookup` (8/12); `GoodwillPointsObserver`, `InteractionLogger` (9/12)
- [ ] Phần dùng chung: `ExcelReportExporter`, hàm định dạng tiền/ngày + căn phải + `ErrorProvider`, hàm vẽ biểu đồ (7/12, để TV1 dùng)
- [ ] Form frm201–frm207 (Master-Detail: frm201, frm202, frm205, frm206)
- [ ] R3 (có biểu đồ), R4, xuất Excel
- [ ] Seed 10 bảng: khách đủ 4 hạng, booking đủ trạng thái, giao dịch điểm, lượt đổi (có lượt hủy), chiến dịch đã kết thúc

**Test (P2–P4)**
- [ ] `FakeTicketQuery`, `FakeSurveyScheduler`, `FakeCustomerCareStats`
- [ ] 10 unit test (PLAN mục 5.8) — phần Domain đã có cho TC2-01…05, 07, 08 và một phần TC2-09, 10; còn TC2-06 và phần service (P3)
- [ ] 4 integration: R5 khớp tính tay; RBAC; L3 luồng đầy đủ; L3 rollback

**Ghép (P4, 11–12/12)**
- [ ] Dashboard R5 (frm207)
- [ ] README: dữ liệu mẫu, báo cáo

**Vấn đáp (P5)**
- [ ] Live OCP khi tắt AI: thêm `FrequentGuestSegment`, đăng ký 1 dòng (tập ≥2 lần)
- [ ] Đặt breakpoint theo sơ đồ L3 (của mình) và L2 (của TV1), đọc Call Stack
- [ ] Trả lời câu hỏi chéo phần TV1: State, Factory Method, cách tính hạn SLA, rollback L2
- [ ] Nêu ≥2 nhược điểm của Strategy và Observer
- [ ] Demo phần mình trong video; chương báo cáo phân hệ TV2

## 5. Việc cả hai cùng làm (sao từ PLAN mục 6.6)
- [x] Merge PR #1 vào `develop` (22/11) — merge 25/11 (`fd7b7e4`)
- [ ] Chốt chữ ký interface ở PLAN mục 4.5 (27/11)
- [ ] Điền tự đánh giá L2 (29/11)
- [ ] Review PR của nhau trong 24 giờ; mỗi người ≥2 PR được merge trên GitHub
- [ ] Cập nhật `docs/PROGRESS.md` trong PR của mình sau mỗi module
- [ ] Đo tỷ lệ `git blame` ở các mốc 28/11, 4/12, 6/12, 9/12, 13/12, rà tiến độ
- [ ] Rà bảng 5.10, đồng bộ giao diện hai phân hệ (12/12)
- [ ] Quay video 3–5 phút trên trình duyệt; tập vấn đáp chéo (P5)
- [ ] Ghép báo cáo cuối, nộp (30/12)

## 6. Việc đang dở
| Việc | Ai | Trạng thái |
|---|---|---|
| PR #2 `tv2/domain` → `v2` (Domain P2 đợt 1: hệ số tích điểm, sổ điểm, đổi quà, chiến dịch, 3 phân khúc + test) | TV2 mở 25/11, TV1 review | ~~chờ merge~~ đã merge 26/11 (`45f9df8`); 5 góp ý của TV1 xử lý ở `tv2/domain-2` |
| PR #3 `tv2/p1-analysis` → `v2` (M2–M8 phần TV2) | TV2 mở 25/11, TV1 review | ~~chờ merge~~ đã merge 26/11 (`e6264a3`) sau khi sửa 3 việc TV1 yêu cầu |
| PR `tv1/web-plan` → `v2` (đưa quyết định chuyển sang web vào `CLAUDE.md`, `docs/PLAN.md`, `docs/PROGRESS.md`, file task P1) | TV1 mở 27/11, TV2 review | TV2 yêu cầu sửa 3 việc + 3 góp ý nhỏ (27/11); TV1 đã sửa (28/11), chờ TV2 duyệt lại |
| Nhánh `tv2/domain-2` → `v2` (Domain đợt 2: bỏ JSON phần TV2; `Customer`, `Interaction`, `Booking`, `Room` v2; góp ý PR #2) | TV2 làm 26/11, TV1 review | PR #5, TV1 approve 28/11; đã merge 28/11 (`0beaddf`). Từ lúc merge tới khi có repository SQLite (P3), app không có khách và phòng: frm "New Ticket" của TV1 có danh sách khách rỗng |
| M2 Khảo sát (`docs/analysis/01-khao-sat.md`) | TV1 + TV2 | TV2: phần mình nằm trong PR #3 (M2–M8); TV1: PR #8, TV2 approve 28/11 |
| Sửa tài liệu P1 phần TV2 theo web: `09-giao-dien.md` mục 1 (16 lựa chọn giao diện) và tên trang frm201–frm207; `08-hanh-vi.md` mục 3.3 (Call Stack `OnPostCheckOut`); cột Form của BM6, BM7 trong `01-khao-sat.md`; F8 "chiến dịch" → "chương trình chăm sóc"; boundary `frm203_nv_stay` trong `seq-l3-checkout.puml` | TV2 | trước slide 30/11 |
| Sửa đường dẫn trong slide: `sec1`, `sec3`, `sec5`, `sec6` còn trỏ `docs/PLAN.md` (v1), `docs/PROGRESS.md` (v1), `docs/tasks/stage-*.md` — các file này đã xóa (bản v1 chỉ còn trong lịch sử git, `fd7b7e4`) | TV2 | làm ở M9 |
| PR `tv2/web-shell` → `v2`: khung web `HotelCRM.GUI` (thanh bên thu gọn, thanh trên, tông Navy & đồng thau, chạy ở `http://localhost:5080`) | TV2 mở 28/11, TV1 review | TV1 yêu cầu sửa 3 việc (28/11); TV2 đã sửa, chờ TV1 duyệt lại |

## 7. Việc chờ người
| Việc | Ai | Hạn | Trạng thái |
|---|---|---|---|
| Ảnh/bản mẫu biểu mẫu thật BM1–BM3 | TV1 | 23/11 | quá hạn, chưa có |
| Ảnh/bản mẫu biểu mẫu thật BM4–BM7 | TV2 | 23/11 | quá hạn, chưa có |
| Tải `plantuml.jar` vào `tools/` / dùng công cụ khác / để TV2 xuất ảnh | TV1 + TV2 | 28/11 (trước khi xuất ảnh M3–M7) | chưa quyết |
| Cài Java (PlantUML cần Java; máy TV1 chưa có `java`); không cần nếu TV2 xuất ảnh | TV1 | 28/11 | chưa làm |
| Thầy có tính file `.cshtml` vào `git blame` không | TV1 hỏi thầy | trước 4/12 | chưa hỏi; trong lúc chờ, nhóm tự đo cả `.cshtml`, `.js`, `.css` (PLAN 3.1) |
| ~~Dời lịch M2–M9 hay giữ~~ | TV1 + TV2 | 26/11 | xong 27/11: giữ hạn slide 30/11, TV1 làm dồn M2–M8 trong 27–29/11 |
| ~~Đối chiếu HuongdanXDHTTTQL: cây BFD 3 nhánh chung hay 8 nhánh theo phân hệ~~ | TV1 + TV2 | M3 | xong 27/11: giữ 8 nhánh theo phân hệ, ghi đối chiếu ở `02-bfd.md` mục 3 |
| ~~Đối chiếu HuongdanXDHTTTQL: tên Form đánh số liên tiếp hay theo người~~ | TV1 + TV2 | M8 | xong 27/11: theo người, tên tiếng Anh (`docs/PLAN.md` mục 5.1) |
| ~~Đối chiếu HuongdanXDHTTTQL: `Dlllib`, tên solution kèm phiên bản~~ | TV1 | P2 | xong 27/11: không làm |
| Mẫu báo cáo cuối của thầy (Word/LaTeX) | ai có | 10/12 | chưa có |
| Ngày vấn đáp cụ thể của nhóm | cả hai | khi có lịch | chưa có |
| ~~Merge PR #1 vào `develop`~~ | TV1/TV2 | 22/11 | xong 25/11 (`fd7b7e4`) |
| ~~Xác nhận phân công v2 (PLAN mục 6.2)~~ | TV2 | 22/11 | xong 25/11 (review approve PR #4) |
| ~~File HuongdanXDHTTTQL~~ | ai có | 26/11 | xong 25/11: `docs/slide_checklist/HuongdanXDHTTTQLv1_1.pdf`, file ngoài repo, mỗi người tự giữ bản |
| ~~File checklist chấm điểm~~ | ai có | — | xong: `docs/slide_checklist/Checklist_BTL_HTTTQL_v2026.xlsx`, sheet "Nhóm 02 người"; file ngoài repo, mỗi người tự giữ bản |

## 8. Quyết định lệch khỏi plan
Khi lệch: ghi ngày, ai đề xuất, lệch gì so với mục nào của `docs/PLAN.md`, lý do, ai đã duyệt.

| Ngày | Ai | Lệch gì (mục PLAN) | Lý do | Ai duyệt |
|---|---|---|---|---|
| 25/11 | TV2 | Q7 (PLAN 5.3): quốc tịch Việt Nam → CCCD đúng 12 chữ số; quốc tịch khác → hộ chiếu 6–9 ký tự chữ và số. PLAN chỉ ghi "CCCD 12 số" | khách nước ngoài không có CCCD | TV1 + TV2 (PR #3 `3ccc924`, PR #4) |
| 25/11 | TV2 | Q9 (PLAN 5.3): điểm khi trả phòng tính theo hạng khách đang có **trước khi** trả phòng; hạng mới (nếu lên hạng) áp dụng từ lần ở sau | một cách tính duy nhất để bảng tính tay R5 và test khớp nhau | TV1 + TV2 (PR #3 `3ccc924`, PR #4) |
| 25/11 | TV2 | Q11 (PLAN 5.3): chiến dịch phải có ít nhất một khách; khách trở nên không đủ điều kiện giữa lúc xem trước và lúc lưu thì bị loại, các khách còn lại vẫn được lưu và được báo rõ khách nào bị loại | PLAN chưa nói trường hợp danh sách rỗng và khách đổi điều kiện giữa chừng (UC10 A3a, E3) | TV1 + TV2 (PR #3 `3ccc924`, PR #4) |
| 25/11 | TV1 | Bản kế hoạch gốc (mục 8 "Bước làm ngay", bước 3) định chuyển tài liệu v1 vào `docs/archive/`; M1 xóa hẳn khỏi cây thư mục (bản v1 vẫn còn trong lịch sử git, `fd7b7e4`) | tránh giữ hai bộ tài liệu song song trong repo | TV1 + TV2 (PR #4) |
| 25/11 | TV1 + TV2 | File checklist và HuongdanXDHTTTQL (PLAN mục 0) không đưa lên repo; mỗi người tự giữ bản ở `docs/slide_checklist/` (đã `.gitignore`) | repo public, đây là tài liệu của giảng viên | TV1 (TV2 đề nghị có file hoặc ghi rõ là file ngoài repo, PR #4) |
| 25/11 | TV2 | Q10 (PLAN 5.3, cột "Cài ở đâu"): bỏ `Customer.SpendPoints`; điều kiện đổi quà do `Reward.EnsureCanRedeem(tier, balance)` quyết định, số dư là tổng sổ điểm và không lưu (`07-thiet-ke-lop.md` mục 4.1) | `Customer` không giữ số dư nên không có gì để trừ; tránh hai nơi cùng giữ số dư | TV1 + TV2 (PR #3) |
| 25/11 | TV2 | File task mục 1.1 ("mỗi người, mỗi module một nhánh"): P1 của TV2 nằm trên một nhánh `tv2/p1-analysis` (PR #3), mỗi module một commit | PR #3 mở trước khi có file task; tách lại thì mất lịch sử review | TV1 + TV2 (PR #3) |
| 26/11 | TV2 | Q13 (PLAN 5.3): không xóa khách đã có lưu trú, ticket **hoặc đã nằm trong chiến dịch**; `Customer.EnsureCanBeDeleted(hasStays, hasTickets, inCampaigns)`. PLAN chỉ ghi "lưu trú hoặc ticket" | khách chưa ở lần nào vẫn có thể nằm trong chiến dịch; xóa khách này thì khóa ngoại `campaign_targets.CustomerId` (RESTRICT) chặn với lỗi khó hiểu, còn `CASCADE` làm mất số liệu R4 (`06-erd.md` mục 3.5) | TV1 + TV2 (PR #3) |
| 26/11 | TV2 | PLAN 5.2 (`point_transactions.Reason`): bỏ giá trị "Điều chỉnh", chỉ còn `Stay`, `Goodwill`, `Redemption`; hoàn điểm khi hủy lượt đổi dùng chung `Redemption` (dòng dương cùng `RedemptionId`). R3: điểm đã tích = tổng dòng `Stay` + `Goodwill`; điểm đã đổi = −tổng dòng `Redemption` (ròng, lượt đã hủy góp 0) | không UC, Form hay quyền nào cho phép chỉnh điểm tay; thêm giá trị thì thành code chết | TV1 + TV2 (PR #3) |
| 26/11 | TV2 | PLAN 6.4 (nền chung P2 của TV1: "bỏ JSON"): TV2 bỏ trước phần JSON của phân hệ mình (mapper, record, 4 file dữ liệu khách/booking/phòng/loại phòng), service và Form v1 của khách/đặt phòng; `Program.cs` dùng `InMemoryRepository` rỗng cho khách, booking, phòng tới P3. Sửa kèm file của TV1: `MainForm` (bỏ menu khách/đặt phòng), `SurveyForm` (bỏ tham số `CustomerService` chưa dùng) | viết lại `Customer`/`Booking` theo v2 không cần chờ P2, và không phải viết mapper tạm sẽ bỏ ngay ở P3 | TV1 + TV2 (TV1 đồng ý trước khi làm, 26/11; xác nhận khi approve PR #5, 28/11) |
| 27/11 | TV1 | Giao diện đổi từ WinForms sang web: ASP.NET Core Razor Pages + Bootstrap 5 + Bootstrap Icons + Chart.js, bỏ `ScottPlot.WinForms`; chạy tại `http://localhost:5080`; đăng nhập cookie; `Program.cs` dựng đối tượng theo từng request (PLAN 3, 5.6, mục mới 3.1) | thầy yêu cầu ngày 26/11; thầy trả lời: chạy localhost là đủ, không bắt buộc công nghệ, tên trang theo HuongdanXDHTTTQL, mục checklist WinForms chấm theo phần tương đương, hạn không đổi | TV1 + TV2 (bản kế hoạch chuyển đổi, 27/11) |
| 27/11 | TV1 | Tên trang chốt tiếng Anh theo khung slide: `frm000_login` … `frm207_bc_customer_360`; giữ BFD 8 nhánh; bỏ `Dlllib`, tên solution kèm phiên bản, tên bảng viết hoa (PLAN 5.1, 5.6) | slide cho phép tiếng Anh; code đã toàn tiếng Anh | TV1 + TV2 (27/11) |
| 27/11 | TV1 | Chữ trên giao diện và tài liệu: "Phiếu yêu cầu" (code `SupportTicket`), "Chương trình chăm sóc" (code `Campaign`); đổi tên nút lá BFD và UC theo (PLAN 4.2, 4.3) | khớp menu với BFD/UC (tiêu chí 3.1) | TV1 + TV2 (27/11) |
| 27/11 | TV1 | TV2 nhận thêm phần dùng chung giao diện và hạ tầng integration test, hạn 7/12 (PLAN 6.2, 6.5) | cân `git blame`; hai phân hệ trông như một | TV1 + TV2 (27/11) |
| 27/11 | TV1 | Mốc đo `git blame`: 28/11, 4/12, 6/12, 9/12, 13/12 thay cho mỗi Chủ nhật; đo trên `.cs`, `.cshtml`, `.js`, `.css`, bỏ `wwwroot/lib`, `*.Designer.cs` và tài liệu; luật giữ blame (PLAN 3.1, 6.6, 7) | phát hiện lệch sớm hơn; phần dùng chung của TV2 phần lớn là `.cshtml`/`.css`/`.js` (TV2 đề nghị ở review PR #6) | TV1 + TV2 (27/11) |
| 27/11 | TV1 + TV2 | Tông màu Navy & đồng thau; font Cormorant Garamond + Be Vietnam Pro; không làm chuông báo SLA; giữ hạn P1, TV1 làm dồn M2–M8 trong 27–29/11 (PLAN 3.1, 6.3) | hai người chọn | TV1 + TV2 (27/11) |
| 28/11 | TV2 | Khung web (bản đầu của PR #6 giao TV1 làm `_Layout`, menu, khung `Program.cs`): TV2 làm project `HotelCRM.GUI`, `_Layout` (thanh bên, thanh trên), menu, toast, `site.css`, `site.js` và khung `Program.cs`. TV1 giữ đăng nhập cookie, lọc menu theo vai trò, `ErrorHandler`, nền SQLite và nội dung trang chủ | cần trang chạy thật cho slide và buổi trình bày 30/11 | TV1 + TV2: đã ghi vào PR #6 (TV2 approve 28/11); TV1 xác nhận thêm khi approve PR #10 |

## 9. Nhật ký
| Ngày | Ai | Việc |
|---|---|---|
| 22/11 | TV1 + TV2 | Chốt kế hoạch v2 (file ngoài repo; nội dung đưa vào `docs/PLAN.md` ở M1) |
| 25/11 | TV1 | Merge PR #1 (`feature/b-customer` → `develop`, `fd7b7e4`) |
| 25/11 | TV2 | Tạo nhánh `v2` từ `develop` và đẩy lên GitHub kèm commit `96149d4` "Rewrite CLAUDE.md for the v2 plan" (đẩy thẳng, không qua PR). Mở PR #2 `tv2/domain` → `v2` |
| 25/11 | TV1 | **M1** trên nhánh `tv1/m1-setup` (tách từ `v2` = `96149d4`, để `v2` không phải force-push): viết lại `CLAUDE.md` từ bản `96149d4` (bỏ đoạn "dừng nếu PLAN còn v1", thêm ghi chú rules v1 → PLAN v1 trong lịch sử git, bỏ nhắc patch/`local/b-work`, thêm cột "Dùng cho" của interface, interface khai báo theo người); xóa tài liệu v1 khỏi cây thư mục (`docs/PLAN.md`, `docs/PROGRESS.md` bản v1 và 5 file `docs/tasks/stage-*.md`; vẫn xem được bằng `git show fd7b7e4:<đường dẫn>`); không đưa bản kế hoạch gốc vào repo; viết `docs/PLAN.md` v2, `docs/PROGRESS.md` v2, `docs/tasks/v2-p1-analysis.md` |
| 25/11 | TV1 | Dọn đồ v1 (chỉ ở máy TV1, không đụng GitHub): xóa `docs/TEAM-BRIEF.md` và thư mục `handoff/` (patch 01–09 kèm `.msg.txt`, `README.txt`, `export-patch-SKILL.draft.md`, `stage3-notes.md`; cả hai đều bị `.gitignore`, không có trong git); bỏ 2 mục `handoff/` và `docs/TEAM-BRIEF.md` khỏi `.gitignore`; xóa worktree `.claude/worktrees/b-work` (sạch); xóa nhánh cục bộ `local/b-work` (`d837bb1`, chưa từng push) và `feature/a-ticket` (`2d06e0b`, đã merge vào `develop`) |
| 25/11 | TV2 | Review PR #4: approve, xác nhận phân công v2; đề nghị ghi các quyết định lệch vào mục 8 và xử lý `docs/slide_checklist/`; nhận sửa PR #3 theo file task |
| 25/11 | TV1 | Sửa PR #4 theo review (mục 8, `docs/slide_checklist/` ngoài repo + `.gitignore`), merge PR #4 vào `v2` |
| 25/11 | TV2 | Rebase PR #3 lên `v2` (`37884a0`; commit "Record the three decisions agreed with TV1" đổi mã từ `284ee92` thành `3ccc924`). Sửa 10 file `docs/analysis/` theo khung của file task (mục của TV1 và mục chung TV1 để dòng chờ); bỏ mã YC tự đặt, dùng mã F; ảnh biểu mẫu vào `docs/analysis/bieu-mau/`; đổi tên `use-case-tv2.iuml`, `dfd-level1-f7.puml`; DFD F7 theo Gane–Sarson với kho D5, D9, D10; mã test TC2-11…TC2-14; thêm loại ngoại lệ + thông báo cho UC08–UC10, Strategy (7.2), Observer phía nhận (7.3), cây `MembershipTier` (8.2), phác thảo bố cục 7 Form, kế hoạch tiêm lỗi |
| 26/11 | TV2 | Sửa PR #3 theo review approve của TV1 (26/11): mục 8 đổi 2 dòng sang "TV1 + TV2", thêm dòng Q13 mở rộng và dòng bỏ "Điều chỉnh" của `PointReason` (kèm cách R3 tính điểm đã đổi); Q13 mở rộng ở `04`, `06` mục 3.5, `07` mục 4.1/4.3, `class-tv2.iuml`; thêm ca "khách chỉ nằm trong chiến dịch" vào TC2-10; sửa tiêu đề, mô tả PR |
| 26/11 | TV2 | Merge PR #2 (`45f9df8`) và PR #3 (`e6264a3`) vào `v2`. Nhánh `tv2/domain-2` (Domain đợt 2, 5 commit): (1) bỏ JSON, service, Form và test service v1 của phân hệ TV2 (TV1 đồng ý trước); (2) viết lại `Customer` (Q7 giấy tờ theo quốc tịch, ngày sinh, đồng ý ưu đãi; Q9 `EarnPoints`; Q13 `EnsureCanBeDeleted(hasStays, hasTickets, inCampaigns)`) và `Interaction` thành thực thể riêng, bỏ `InteractionHistory`; (3) viết lại `Booking` (tham chiếu bằng Id, `RatePerNight` chốt, `CheckOut(now)`) và `Room` (Q8, thông báo nêu ngày booking đang giữ phòng); (4) xử lý góp ý PR #2 (`ForRedemption` kiểm trạng thái, `MembershipTier.FromName` đi dọc `Next()`, giữ `ResponseRate` và ghi phản hồi trước ngày bắt đầu có chủ ý); (5) tài liệu. Test 107/109 xanh (2 đỏ là `SurveyServiceTests`, đỏ sẵn trên `v2`) |
| 26/11 | TV1 + TV2 | Thầy yêu cầu chuyển giao diện sang web và trả lời 5 câu hỏi (chạy localhost là đủ, không bắt buộc công nghệ, tên trang theo HuongdanXDHTTTQL, mục WinForms chấm theo phần tương đương, hạn không đổi) |
| 27/11 | TV2 | Đồng ý (miệng, 27/11) toàn bộ bản kế hoạch chuyển giao diện sang web của TV1; cùng chọn tông Navy & đồng thau, font Cormorant Garamond + Be Vietnam Pro, không làm chuông SLA, giữ hạn P1. Góp ý trong review PR #6: đo `git blame` tính cả `.cshtml`, `.js`, `.css` (bỏ `wwwroot/lib`, `*.Designer.cs`); test v1 của TV2 do TV2 tự xóa |
| 27/11 | TV2 | Xóa 2 file test v1 do TV2 viết: `Assignment/AssignmentStrategyTests.cs` (3 strategy phân công cũ, TV1 xóa phần code ở P2) và `Customers/SurveyServiceTests.cs` (khảo sát nay thuộc TV1, TV1 viết test mới). Build 0 warning, 60 test xanh |
| 27/11 | TV1 | Mở PR `tv1/web-plan`: đưa quyết định chuyển sang web vào `CLAUDE.md`, `docs/PLAN.md` (mục mới 3.1, sửa 0, 3, 4.2, 4.3, 5.1, 5.6, 6, 7, 8, 9), file này, `docs/tasks/v2-p1-analysis.md` |
| 27/11 | TV1 | **M2** trên nhánh `tv1/m2-khao-sat` (từ `v2`): điền phần TV1 của `docs/analysis/01-khao-sat.md`, chỉ thay 7 dòng chờ, không sửa phần TV2: mục 1 (đặt vấn đề, ý nghĩa, đối chiếu khung "Khảo sát" của HuongdanXDHTTTQL slide 4–7), mục 2 (4W+1H toàn hệ thống + bảng đầu ra BM1–BM7, R1–R5, D1–D11, L1–L6), mục 3.1 (hiện trạng, 9 nhược điểm → Q/F), mục 4.1 (BM1–BM3 dựng lại, ô → cột → trang), mục 5.1 (F1.1–F4.2 + số liệu nghiệp vụ), mục 6 (21 yêu cầu phi chức năng theo bản web), mục 7 (phạm vi). Góp ý phần TV2 ghi trong PR |
| 28/11 | TV1 | Sửa PR #6 theo review TV2 (27/11): phạm vi đo `git blame` (`.cs`, `.cshtml`, `.js`, `.css`, bỏ `wwwroot/lib`) ở `CLAUDE.md` và PLAN 3.1; file v1 ai viết người đó xoá (PLAN 3.1 việc 2); bỏ dòng nhật ký 27/11 của TV2; thêm "3 strategy phân công cũ" vào danh sách xoá (PLAN 6.4, mục 3), dòng `DatabaseInitializer` ở PLAN 6.7, ContentRoot = `AppContext.BaseDirectory` (PLAN 3.1 việc 8). Theo PR #10: khung web (`HotelCRM.GUI`, `_Layout`, menu, toast, khung `Program.cs`) chuyển sang TV2; lọc menu theo vai trò giữ ở TV1 (theo đề xuất của TV2 ngày 28/11) (`CLAUDE.md` Phân công, PLAN 3.1, 5.6, 6.2–6.5, 6.7, 9). Theo PR #8: "chiến dịch", "Ticket" → "chương trình chăm sóc", "phiếu yêu cầu" ở tên BM7 (PLAN 5.1, 6.5) và danh mục chung (file task mục 1.5) |
| 28/11 | TV2 | Dựng khung web trên nhánh `tv2/web-shell`: project `HotelCRM.GUI` (Razor Pages, `net10.0`) trong `HotelCRM.sln`; font và Bootstrap Icons để trong `wwwroot/lib` để chạy không cần mạng; mở PR vào `v2` |
| 28/11 | TV2 | Sửa PR #10 theo review TV1: bỏ jQuery, jquery-validation, jquery-validation-unobtrusive và `_ValidationScriptsPartial` (ngoài danh sách thư viện đã chốt); thẻ nền trắng, chữ phụ dùng màu của tông (tương phản 6,53:1 trên thẻ); content root đặt là thư mục chứa file chạy, đã thử `dotnet run`, DLL và bản publish chạy từ thư mục khác. Thêm việc khung web vào mục 4 |
| 28/11 | TV1 | Sửa PR #8 theo review TV2 (27/11), `01-khao-sat.md`: mục 3.1 trích đủ khóa luận (lễ tân "tiếp nhận, giải quyết" phàn nàn, mục 1.1.5, 2.2.6; "lễ tân không xử lý" ghi là quyết định thiết kế), ghi rõ phần nào là kịch bản nhóm dựng (thêm cột "Căn cứ"), câu Oxmaint về trạng thái; mục 2 ô When (chỉ mở lại từ Đã giải quyết); mục 6 dòng 12 (giao dịch nghiệp vụ, liệt kê cột người làm), dòng 18 (≤ 2 giây trên máy của nhóm), dòng 19 (hai lớp + một dòng đăng ký; không `CHECK` trên `tickets.Type`) |
| 28/11 | TV1 | Sửa PR #8 theo review approve của TV2 (28/11): ý "nhân viên kỹ thuật giúp" dẫn mục 2.2.10 của khóa luận (không phải 2.2.6); mục 6 dòng 12 nói rõ 4 bảng không lưu người sửa, nhận/trả phòng chỉ lưu thời điểm; mục 3.1 ghi việc lễ tân làm tại quầy nằm trong ghi chú giải quyết (Q6). Merge PR #8 |
