# Hotel CRM

Bài tập lớn OOP nhóm 2 người: hệ thống quản lý chăm sóc khách hàng cho khách sạn. C# / .NET 10, WinForms, lưu JSON, test xUnit.

## Đọc trước khi làm
1. `docs/PROGRESS.md` — giai đoạn nào đã xong, việc đang dở, cách commit của suất B, quyết định lệch khỏi plan.
2. File task của giai đoạn đang làm trong `docs/tasks/`.
3. `docs/PLAN.md` — chỉ mở những mục mà file task nêu tên. Không đọc cả file.

Quy ước riêng từng tầng nằm trong `.claude/rules/`. Các file này chỉ tự nạp khi đã có file khớp đường dẫn; khi tạo file đầu tiên trong một thư mục thì tự đọc file rules tương ứng trước.

## Lệnh
Solution được tạo ở GĐ 1; trước đó các lệnh này chưa chạy được.

```
dotnet build HotelCRM.sln
dotnet test HotelCRM.sln
dotnet run --project src/HotelCRM.WinForms
dotnet publish src/HotelCRM.WinForms -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## Kiến trúc
- Tham chiếu project: `WinForms → Business → Domain`, `Data → Domain`. `WinForms` tham chiếu `Data` chỉ để `Program.cs` khởi tạo. `Domain` không tham chiếu project nào. `Tests` tham chiếu `Domain`, `Business`, `Data`.
- Hai phân hệ (Ticket và Khách hàng/Lưu trú) chỉ nối với nhau qua 5 interface: `ICustomerDirectory`, `IStayLookup`, `ITicketQuery`, `ITicketObserver` (trong `Domain/Interfaces`) và `IAssignmentStrategy` (trong `Business/Assignment`). Service của phân hệ này không gọi service của phân hệ kia. `Domain/Tickets` không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`.
- Quy tắc nghiệp vụ: service tra cứu, lớp Domain quyết định và ném ngoại lệ.
- Khởi tạo đối tượng bằng tay trong `Program.cs`. Không dùng thư viện DI.
- Không thêm gói NuGet ngoài bộ xUnit mặc định. Cần thêm thì dừng và hỏi.

## Ngôn ngữ
- Trong sản phẩm, mọi thứ bằng tiếng Anh: tên lớp/hàm, comment, chữ trên giao diện, thông báo lỗi, dữ liệu mẫu.
- Trả lời người dùng bằng tiếng Việt. Tài liệu trong `docs/` viết tiếng Việt.

## Phân công (theo file)
- **Suất A** (chủ repo), phân hệ Ticket: `Domain/Tickets/**`, hàm `Agent.EnsureCanTake`, `TicketService`, `Queries/TicketQuery`, `Business/Factories/**`, `Forms/Tickets/**`, `tests/HotelCRM.Tests/Tickets/**`, `FakeCustomerDirectory`, `FakeStayLookup`.
- **Suất B** (thành viên còn lại), phân hệ Khách hàng/Lưu trú: `Domain/Customers/**`, `Domain/Hotel/**`, `CustomerService`, `BookingService`, `SurveyService`, `ReportService`, `Queries/CustomerDirectory`, `Queries/StayLookup`, `Business/Assignment/**`, `Business/Observers/**`, `Forms/Customers/**`, `Forms/Bookings/**`, `Forms/Reports/**`, `UiNotificationObserver`, `tests/HotelCRM.Tests/Customers/**`, `tests/HotelCRM.Tests/Assignment/**`, `FakeTicketQuery`.
- **Phần chung** (viết xong ở GĐ 1): mọi thứ còn lại, gồm toàn bộ `Data/`, `Program.cs`, `MainForm`, `ErrorHandler`. Sau GĐ 1 không sửa các file này.

## Luật làm việc
- **Không tự commit.** Đến điểm dừng thì đề xuất danh sách commit; chỉ commit khi người dùng đồng ý.
- **Commit message không kèm dòng `Co-Authored-By` hay dòng quảng cáo công cụ nào.** Việc dùng Claude được nêu trong README và báo cáo.
- **Code suất B** làm trên nhánh cục bộ `local/b-work` và xuất patch vào `handoff/` theo mục "Git" của `docs/PLAN.md`, trừ khi `docs/PROGRESS.md` ghi thành viên B tự code. Không bao giờ push `local/b-work`.
- **Không đổi khung:** không đổi tên lớp, chữ ký public, cấu trúc thư mục; không sửa test có sẵn. Thấy cần đổi thì dừng, ghi lý do vào `docs/PROGRESS.md` và hỏi người dùng.
- **Dừng sau mỗi module.** Một module là một dòng trong danh sách đánh số của file task. Khi dừng: tóm tắt thiết kế và những điểm cần nhớ khi vấn đáp, chờ duyệt rồi làm tiếp.
- Giao diện WinForms không tự kiểm tra được: nêu rõ Form nào người dùng cần mở và bấm thử.

## Điều kiện xong của một giai đoạn
- `dotnet build` không lỗi, không warning.
- Các test mà file task yêu cầu đều xanh.
- `docs/PROGRESS.md` đã cập nhật.
