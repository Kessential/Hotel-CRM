# Hotel CRM — tóm tắt kế hoạch BTL và việc cần bạn xác nhận

Bản này để hai đứa mình chốt cách làm trước khi bắt đầu code. Đặc tả kỹ thuật đầy đủ nằm ở `docs/PLAN.md`; bạn không cần đọc hết, chỉ cần đọc trang này rồi trả lời 7 câu ở mục 7 **trước ngày 17/11**.

## 1. Dự án
- Đề: "Quản lý dịch vụ chăm sóc khách hàng (CRM)", mình áp dụng cho khách sạn. Lớp gợi ý của đề: `Customer`, `SupportTicket`, `TicketState (Open, InProgress, Resolved)`, `Agent`, `InteractionHistory`.
- Hệ thống gồm hai phân hệ: **Ticket** (tiếp nhận, phân công, xử lý yêu cầu/khiếu nại của khách) và **Khách hàng/Lưu trú** (khách hàng, hạng thành viên, đặt phòng, khảo sát, báo cáo).
- Chấm theo sheet "Nhóm 02 người" của file checklist: 20 tiêu chí, mỗi tiêu chí 1 điểm. Nhiều tiêu chí ghi rõ "chia đều 2 thành viên": Use Case, sơ đồ tuần tự, Design Pattern, test, commit. Khi vấn đáp, **cả hai phải hiểu code của nhau và mở rộng thêm lớp mới ngay tại chỗ** (tiêu chí 12 và 20).

## 2. Mình đã chọn sẵn, cần bạn đồng ý
| Hạng mục | Lựa chọn |
|---|---|
| Ngôn ngữ, giao diện | C# / .NET 10, WinForms, làm bằng Visual Studio |
| Lưu dữ liệu | File JSON (không cần cài CSDL, chạy được trên máy khác) |
| Ngôn ngữ trong sản phẩm | Code, comment, giao diện, dữ liệu mẫu đều tiếng Anh; báo cáo tiếng Việt |
| Tác nhân | Manager và Agent, có đăng nhập và phân quyền |
| Quy mô | 10 Use Case, 26 lớp thực thể, 11 quy tắc nghiệp vụ, 5 Design Pattern, 22 unit test (đều vượt mức tối thiểu của checklist) |
| Kiến trúc | 3 tầng: giao diện → nghiệp vụ → dữ liệu, cộng một project Domain chứa lớp thực thể |
| Thời gian | 14 ngày, 16/11 → 29/11 |
| Tài liệu | UML vẽ bằng PlantUML, báo cáo viết bằng LaTeX |

## 3. Hai suất công việc (đề xuất, đổi được)
| | Suất A — phân hệ Ticket | Suất B — phân hệ Khách hàng/Lưu trú |
|---|---|---|
| Use Case | UC1 Đăng nhập, UC2 Tạo ticket, UC3 Phân công ticket, UC4 Xử lý ticket, UC5 Tìm kiếm ticket | UC6 Quản lý khách hàng, UC7 Đặt phòng/check-in/check-out, UC8 Khảo sát hài lòng, UC9 Lịch sử tương tác, UC10 Báo cáo |
| Quy tắc nghiệp vụ | 5 quy tắc: chuyển trạng thái ticket, hạn mở lại, giới hạn số ticket của Agent, hạn xử lý theo hạng khách, ticket bảo trì phải gắn phòng đang ở | 5 quy tắc: định dạng và trùng SĐT/Email, đặt phòng không chồng ngày, tích điểm và nâng hạng, điều kiện khảo sát, không xóa khách còn ticket |
| Design Pattern | State, Strategy, Factory Method | Observer, Repository + Singleton |
| Sơ đồ tuần tự | UC2, UC4 | UC7, UC8 |
| Unit test | 11 | 11 |
| Tài liệu riêng | sơ đồ use case, sơ đồ 3 pattern, chương báo cáo về Ticket, README, kịch bản demo | class diagram tổng thể, sơ đồ 2 pattern, chương báo cáo về Khách hàng/Lưu trú, tài liệu ôn vấn đáp |
| Giai đoạn code | 18–20/11 | 21–25/11 |

Phần chung, không tính cho ai: khung dự án (16–17/11), quy tắc phân quyền Manager/Agent, xử lý lỗi chung, đóng gói file chạy, các chương mở đầu/kiến trúc/kết luận của báo cáo.

Hai suất chỉ nối với nhau qua 4 interface, nên đổi người hoặc hoán đổi vài mục giữa hai suất không làm hỏng thiết kế.

## 4. Cách làm việc
Nói thẳng để bạn quyết định: **code của cả hai suất do mình viết cùng Claude (trợ lý AI lập trình)**, làm theo đặc tả trong `docs/PLAN.md`. Vì checklist yêu cầu commit chia đều, mình đề xuất:

- **Cách 1 — nhận patch (đề xuất):** phần code thuộc suất của bạn được tách thành các file patch đánh số, mỗi patch là một bước nhỏ kèm gợi ý commit message. Bạn kéo repo về, áp từng patch, đọc hiểu, chạy thử, rồi tự commit và push bằng tài khoản GitHub của bạn.
- **Cách 2 — tự code:** bạn tự viết suất của mình theo đặc tả, mình lo suất còn lại và khung chung.

Dù chọn cách nào, khi vấn đáp bạn vẫn phải giải thích được code của cả hai suất và tự thêm được một lớp mới vào pattern (ví dụ thêm một trạng thái ticket hoặc một kênh thông báo). Vì vậy phần đọc hiểu và tự làm lại bài mở rộng ở mục 5 là bắt buộc, không bỏ được.

## 5. Việc của bạn theo ngày
| Ngày | Việc |
|---|---|
| 16–17/11 | Cài môi trường (mục 6), nhận quyền vào repo, clone, build thử; trả lời 7 câu ở mục 7 |
| 18–20/11 nếu nhận suất A, hoặc 21–25/11 nếu nhận suất B | Áp từng patch (hoặc tự code), đọc hiểu, commit ít nhất 12 lần, chạy thử các Use Case của suất mình |
| 26–27/11 | Đọc tài liệu ôn vấn đáp; tự làm lại các bài mở rộng pattern của suất mình và ít nhất một bài của suất kia |
| 28–29/11 | Đọc báo cáo, chạy bản đóng gói trên máy bạn, hai đứa tập vấn đáp |

## 6. Chuẩn bị máy
- .NET 10 SDK
- Visual Studio, có cài workload ".NET desktop development"
- Git
- Tài khoản GitHub (để mình thêm vào repo)

## 7. Cần bạn trả lời trước 17/11
1. Bạn nhận **suất A hay suất B**?
2. Có muốn **đổi** Use Case hoặc Design Pattern nào giữa hai suất không?
3. Chọn **cách 1 (nhận patch)** hay **cách 2 (tự code)**?
4. **Tên tài khoản GitHub** của bạn?
5. Bạn có **mẫu báo cáo** của giảng viên không (mình cần bản LaTeX trước 26/11)?
6. Trong 16–29/11 có **ngày nào bạn không làm được** không?
7. Có **không đồng ý** lựa chọn nào ở mục 2 không?
