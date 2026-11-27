# PROGRESS — Hotel CRM v2

Kế hoạch: `docs/PLAN.md` (v2). Việc chi tiết giai đoạn đang làm: `docs/tasks/v2-p1-analysis.md`. Tiến độ v1 (15–21/11) chỉ còn trong lịch sử git: `git show fd7b7e4:docs/PROGRESS.md`.

Cách cập nhật: mỗi người sửa dòng/ô của mình trong PR của mình, sau mỗi module (đánh dấu `[x]`, đổi cột trạng thái, thêm một dòng nhật ký). Không sửa dòng của người kia.

Cập nhật lần cuối: 26/11/2026 (nhánh `tv2/m9-slides`, TV2).

## 1. Trạng thái giai đoạn
| Giai đoạn | Thời gian (plan) | Trạng thái | Mốc kiểm cuối giai đoạn |
|---|---|---|---|
| P1 Phân tích thiết kế | 22–29/11, nộp slide 30/11 | **Đang làm**, trễ 3 ngày (M1 xong 25/11) | mọi `.puml` xuất ảnh; bảng 5.10 đạt; slide biên dịch được |
| P2 Hạ tầng | 1–4/12 | Chưa làm (TV2 đã bắt đầu Domain sớm, PR #2) | đăng nhập 4 vai trò trên SQLite; TV2 có 10 unit test xanh |
| P3 Phân hệ | 5–10/12 | Chưa làm | đủ 13 Form, R1–R4, seed ≥150 dòng |
| P4 Ghép & đóng gói | 11–12/12 | Chưa làm | 28 test xanh; chạy máy sạch ≤3 phút |
| P5 Vấn đáp | 13/12 → 30/12 | Chưa làm | buổi vấn đáp; nộp cuối 30/12 |

## 2. Trạng thái module P1
| Module | Hạn | TV1 | TV2 | Trạng thái (25/11) |
|---|---|---|---|---|
| M1 Chuẩn bị | 22/11 | **Xong 25/11** (trễ 3 ngày): `CLAUDE.md`, `docs/PLAN.md` v2, file này, `docs/tasks/v2-p1-analysis.md`, xóa tài liệu v1 khỏi cây thư mục, dọn đồ v1 | PR #1 đã merge (25/11); xác nhận phân công khi approve PR #4 (25/11) | **Xong 25/11**: PR #4 (`tv1/m1-setup` → `v2`) đã được TV2 approve và merge |
| M2 Khảo sát | 23/11 | Chưa làm | Đã viết (PR #3, TV1 approve 26/11); ảnh BM4–BM7 thật chưa có | **Quá hạn 2 ngày** |
| M3 BFD + UC | 24/11 | Chưa làm | Đã viết (PR #3, TV1 approve 26/11) | **Quá hạn 1 ngày** |
| M4 Quy tắc, quyền, DFD | 25/11 | Chưa làm | Đã viết (PR #3, TV1 approve 26/11) | **Đến hạn hôm nay, chưa làm** |
| M5 ERD + CSDL | 26/11 | Chưa làm | Đã viết (PR #3, TV1 approve 26/11) | Chưa tới hạn; sẽ trễ theo M2–M4 nếu không dời lịch |
| M6 Lớp + package + hợp đồng | 27/11 | Chưa làm | Đã viết (PR #3, TV1 approve 26/11); còn chốt hợp đồng 27/11; code Domain trong PR #2 | Chưa tới hạn; sẽ trễ theo M2–M4 nếu không dời lịch |
| M7 Hành vi | 28/11 | Chưa làm | Đã viết (PR #3, TV1 approve 26/11) | Chưa tới hạn; sẽ trễ theo M2–M4 nếu không dời lịch |
| M8 Giao diện + test | 28/11 | Chưa làm | Đã viết (PR #3, TV1 approve 26/11) | Chưa tới hạn; sẽ trễ theo M2–M4 nếu không dời lịch |
| M9 Slide + tự đánh giá L2 | 29/11 | Chưa làm | Chưa làm | Chưa tới hạn; slide nộp **30/11 (không dời)** |

Dời lịch M2–M9 hay giữ: chờ quyết (mục 7).

## 3. Việc của TV1 (sao từ PLAN mục 6.4)
**Tài liệu & sơ đồ (P1)**
- [x] M1: `CLAUDE.md`, `docs/PLAN.md` v2, `docs/PROGRESS.md` v2, file task P1, nhánh `v2` (22/11) — xong 25/11
- [ ] 4W+1H toàn hệ thống; yêu cầu phi chức năng; biểu mẫu BM1 Phiếu yêu cầu/khiếu nại, BM2 Phiếu theo dõi xử lý, BM3 Phiếu khảo sát (23/11)
- [ ] Cây BFD 8 nhánh, lá F1–F4; sơ đồ Use Case tổng; đặc tả UC04, UC05 (24/11)
- [ ] Q1–Q6 với tiền/hậu điều kiện và ngoại lệ; ma trận quyền; DFD ngữ cảnh, mức 0, mức 1 của F3 (25/11)
- [ ] Khung `erd.puml` + 7 bảng TV1; giải thích 3NF (26/11)
- [ ] Sơ đồ lớp phần chung + TV1; package diagram; khai báo `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`; chốt hợp đồng với TV2 (27/11)
- [ ] Sơ đồ tuần tự L2 (đóng ticket, có khung alt rollback); máy trạng thái ticket (28/11)
- [ ] Bảng Form F1–F4; ma trận test TC1-xx; bảng tính tay R2 (28/11)
- [ ] Slide phần chung + TV1 (29/11)

**Nền chung (P2, 1–4/12)**
- [ ] Đổi tên project thành `HotelCRM.GUI`, `HotelCRM.BLL`, `HotelCRM.DAL`; thêm `Database/`, `Resource/`; bỏ JSON
- [ ] SQLite: kết nối, `UnitOfWork`, tạo CSDL từ `schema.sql` + nạp `seed.sql`; một repository mẫu để TV2 làm theo
- [ ] Cây ngoại lệ, `ErrorHandler` tiếng Việt, `appsettings.json` + file mẫu, bỏ Singleton, `IClock`
- [ ] `Employee` + `Administrator`, `Manager`, `Agent`; đăng nhập (frm000); màn hình chính theo vai trò (frm001)
- [ ] `schema.sql` 7 bảng TV1; seed tài khoản 4 vai trò; sửa `.claude/rules/*` theo v2

**Phân hệ CSKH (P3, 5–10/12)**
- [ ] Domain: `SupportTicket` + 3 loại ticket, 4 state (bảng đăng ký thay `switch`), `TicketLog`, `SatisfactionSurvey`, `SurveyAnswer`, `SurveyCriterion`
- [ ] Repository + truy vấn báo cáo cho 7 bảng TV1
- [ ] `TicketService` (tạo, phân công, xử lý, nhật ký, phát sự kiện), 3 factory, `SurveyService`
- [ ] Cài cho TV2 dùng: `TicketQuery`, `SurveyScheduler`, `CustomerCareStats` (8/12)
- [ ] Form frm101–frm106 (Master-Detail: frm101, frm104, frm105); `ErrorProvider`; tiền/ngày đúng định dạng
- [ ] R1, R2 (có biểu đồ), xuất Excel
- [ ] Seed 7 bảng: đủ 3 loại ticket, đủ trạng thái, có ticket quá hạn SLA, nhật ký, phiếu khảo sát chờ/đã làm

**Test (P2–P4)**
- [ ] `FakeCustomerDirectory`, `FakeStayLookup`
- [ ] 10 unit test (PLAN mục 5.8)
- [ ] 4 integration: R2 khớp tính tay; RBAC; L2 luồng đầy đủ; L2 rollback

**Đóng gói (P4, 11–12/12)**
- [ ] Publish single-file, chạy thử trên máy sạch
- [ ] README: cài đặt, tài khoản demo, lệnh chạy test

**Vấn đáp (P5)**
- [ ] Live OCP khi tắt AI: thêm `LostItemTicket` + factory, đăng ký 1 dòng (tập ≥2 lần)
- [ ] Đặt breakpoint theo sơ đồ L2 (của mình) và L3 (của TV2), đọc Call Stack
- [ ] Trả lời câu hỏi chéo phần TV2: Strategy phân khúc, sổ điểm, vì sao hạng không hạ, rollback L3
- [ ] Nêu ≥2 nhược điểm của State và Factory Method
- [ ] Demo phần mình trong video; chương báo cáo phân hệ TV1 + chương kiến trúc

## 4. Việc của TV2 (sao từ PLAN mục 6.5)
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

**Domain (P2, 1–4/12, chưa cần CSDL)**
- [ ] Sửa `Customer` (CCCD, ngày sinh, đồng ý nhận ưu đãi), `MembershipTier` (hệ số tích điểm), `Booking` (giá/đêm chốt)
- [ ] Thêm `PointTransaction`, `Reward`, `RewardRedemption`, `Campaign`, `CampaignTarget`, `Receptionist`
- [ ] `ISegmentStrategy` + `VipSegment`, `LapsedGuestSegment`, `BirthdayMonthSegment`
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
- [ ] 10 unit test (PLAN mục 5.8)
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
- [ ] Chủ nhật 29/11, 6/12, 13/12: đo tỷ lệ `git blame`, rà tiến độ
- [ ] Rà bảng 5.10, đồng bộ giao diện hai phân hệ (12/12)
- [ ] Quay video 3–5 phút; tập vấn đáp chéo (P5)
- [ ] Ghép báo cáo cuối, nộp (30/12)

## 6. Việc đang dở
| Việc | Ai | Trạng thái |
|---|---|---|
| PR #2 `tv2/domain` → `v2` (Domain P2 đợt 1: hệ số tích điểm, sổ điểm, đổi quà, chiến dịch, 3 phân khúc + test) | TV2 mở 25/11, TV1 review | TV1 approve 26/11, góp ý 5 điểm không chặn merge (sửa ở đợt 2); chờ TV2 merge |
| PR #3 `tv2/p1-analysis` → `v2` (M2–M8 phần TV2) | TV2 mở 25/11, TV1 review | đã rebase lên `v2` (`37884a0`) và sửa theo khung của `docs/tasks/v2-p1-analysis.md` (25/11); TV1 approve 26/11, đồng ý 7 điểm hỏi; TV2 đã sửa 3 việc trước khi merge (mục 8, Q13, tiêu đề PR) (26/11); chờ TV2 merge |
| M2 Khảo sát (`docs/analysis/01-khao-sat.md`) | TV1 + TV2 | TV2: phần mình nằm trong PR #3 (M2–M8); TV1: bắt đầu sau khi M1 merge, xem `docs/tasks/v2-p1-analysis.md` |
| Sửa đường dẫn trong slide: `sec1`, `sec3`, `sec5`, `sec6` còn trỏ `docs/PLAN.md` (v1), `docs/PROGRESS.md` (v1), `docs/tasks/stage-*.md` — các file này đã xóa (bản v1 chỉ còn trong lịch sử git, `fd7b7e4`) | TV2 | làm ở M9 |

## 7. Việc chờ người
| Việc | Ai | Hạn | Trạng thái |
|---|---|---|---|
| **Dời lịch M2–M9 hay giữ** (M1 xong trễ 3 ngày; M2–M3 đã quá hạn; slide vẫn nộp 30/11) | TV1 + TV2 | 26/11 | chưa quyết |
| Ảnh/bản mẫu biểu mẫu thật BM1–BM3 | TV1 | 23/11 | quá hạn, chưa có |
| Ảnh/bản mẫu biểu mẫu thật BM4–BM7 | TV2 | 23/11 | quá hạn, chưa có |
| Đồng ý tải `plantuml.jar` vào `tools/` | TV1 | trước M3 (24/11) | quá hạn, chưa quyết |
| Cài Java (PlantUML cần Java; máy TV1 chưa có `java`) | TV1 | trước M3 | chưa làm |
| Đối chiếu HuongdanXDHTTTQL: cây BFD mẫu có 3 nhánh chung (Quản trị hệ thống, Danh mục, Báo cáo thống kê) + nhánh nghiệp vụ; plan chia 8 nhánh theo phân hệ. Giữ hay chỉnh | TV1 + TV2 | M3 | chưa quyết |
| Đối chiếu HuongdanXDHTTTQL: tên Form mẫu đánh số liên tiếp (`frm001_dm_…`, `frm002_nv_…`); plan đánh số theo người (1xx/2xx). Giữ hay đổi | TV1 + TV2 | M8 (trước P3) | chưa quyết |
| Đối chiếu HuongdanXDHTTTQL: Solution Folder `Dlllib`, tên solution kèm phiên bản. Có làm theo không | TV1 | P2 | chưa quyết |
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
| 26/11 | TV2 | PLAN 4.2 (tên nhánh BFD): F5–F8 đổi sang động từ (F5 Quản lý hồ sơ khách hàng, F6 Quản lý lưu trú, F7 Quản lý khách hàng thân thiết, F8 Chăm sóc khách hàng theo chiến dịch); thêm F7.3 Cộng điểm thiện chí (tự động); phân rã F5–F8 đến cấp 4 (`02-bfd.md` mục 2.3). Mã F giữ nguyên | góp ý của thầy: mọi chức năng là động từ, BFD thể hiện đủ mọi chức năng | chờ TV1 (PR `tv2/m9-slides`); TV1 đổi tên tiến trình 5.0–8.0 của DFD mức 0 theo |
| 26/11 | TV2 | PLAN 4.2 (cấu trúc cây): đề xuất cây tổng theo chuẩn 3 nhánh chung (Quản trị hệ thống, Quản lý danh mục, Báo cáo thống kê) + nhánh nghiệp vụ; F6.1, F7.1 xếp vào Quản lý danh mục, F8.2 vào Báo cáo thống kê (`02-bfd.md` mục 2.4) | góp ý của thầy; checklist ghi chuẩn "3 nhánh chung + nhánh riêng" | chờ TV1 chốt cây tổng (mục 7, dòng "Đối chiếu HuongdanXDHTTTQL: cây BFD mẫu") |

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
| 26/11 | TV2 | Theo góp ý của thầy: thêm "Khảo sát cái gì, tham khảo hệ thống nào" (OPERA Cloud, Marriott Bonvoy, Hilton Honors, HubSpot/Salesforce; phần chung/riêng) vào `01-khao-sat.md`; phân rã F5–F8 đến cấp 4, tên động từ, đề xuất xếp vào 3 nhánh chung, prompt + mẫu phân rã cho AI (`02-bfd.md` mục 2.3–2.5, `bfd-tv2.iuml`); kịch bản demo 8 bước và bảng "quy trình còn thiếu chức năng gì" (`03-use-case.md` mục 6). Rút slide phần TV2 cho buổi trình bày 15–20 phút, chi tiết chuyển sang phụ lục |
| 28/11 | TV2 | Sửa phần TV2 của `01-khao-sat.md` theo góp ý trong PR #8: bản web ở ô Where, tên trang mới, cột "Trên trang" với ô InputModel thay tên control, cột có lưu đổi "Tính" → "Tự ghi" (cả dòng khách của BM7), "phiếu yêu cầu", "chương trình chăm sóc". Tên chức năng giữ dạng động từ, đề nghị TV1 sửa PLAN 4.2 |
