---
paths:
  - "src/HotelCRM.WinForms/**"
---

# Quy ước tầng giao diện (WinForms)

- Form chỉ gọi service của tầng Business. Form không chứa quy tắc nghiệp vụ, không đọc/ghi file, không dùng repository.
- `Program.cs` là nơi duy nhất khởi tạo `AppConfig`, repository, observer, bảng strategy, bảng factory và service, rồi truyền service vào constructor của Form.
- `Program.cs`, `MainForm` và `ErrorHandler` viết xong ở GĐ 1. Các giai đoạn sau chỉ điền nội dung vào vỏ Form của mình; không sửa ba file này, không đổi constructor của Form.
- Mỗi Form gồm `XxxForm.cs` và `XxxForm.Designer.cs`, cả hai là `partial class`.
  - `InitializeComponent()` trong file Designer chỉ tạo control, gán thuộc tính và đăng ký sự kiện. Không có `if`, vòng lặp, gọi hàm tự viết, lambda hay biểu thức tính toán. Vi phạm sẽ làm Designer của Visual Studio không mở được Form.
  - Khai báo control là field `private` ở cuối file Designer.
  - Code nạp dữ liệu, gắn danh sách vào lưới đặt trong `XxxForm.cs`.
- Chép cấu trúc của `LoginForm` và `MainForm` (form mẫu của GĐ 1) cho mọi Form: cách nhận service, cách đặt tên control, cách bố trí.
- File `.Designer.cs` viết đúng kiểu Visual Studio sinh ra (xem `LoginForm.Designer.cs`): `components`, `Dispose`, vùng `#region Windows Form Designer generated code`, khai báo control ở cuối. Nếu Visual Studio tạo file `.resx` cho Form thì commit kèm.
- Tên control có tiền tố theo loại: `txt`, `btn`, `cbo`, `dgv`, `lbl`, `dtp`, `num`, `chk`, `mnu` (mục menu), `sts` (thanh trạng thái), `tab`.
- Tên handler theo kiểu Visual Studio: `btnSave_Click(object? sender, EventArgs e)`.
- Mọi event handler bọc trong `ErrorHandler.Run(() => { ... })`. Không tự viết `try/catch` trong Form. `ErrorHandler.Run` trả `true` khi không có lỗi, dùng khi cần làm thêm việc giao diện sau thao tác (xem `LoginForm.btnLogin_Click`).
- Form con mở dạng hộp thoại: `using` + `ShowDialog(this)`; đóng hộp thoại thì nạp lại dữ liệu của Form cha.
- Constructor của Form chỉ nhận service và không đổi sau GĐ 1. Dữ liệu ngữ cảnh (Id ticket, Id khách) truyền qua một hàm public của Form gọi trước `ShowDialog`, ví dụ `ShowTicket(int ticketId)`. Mọi Form phải dùng được cả khi mở trống từ menu của `MainForm`.
- Thời điểm `now` truyền xuống service là `DateTime.Now` lấy trong Form.
- Thao tác cần phân quyền truyền `UserSession.Current` vào tham số `actor` của service.
- Menu và nút hiển thị theo `UserSession.Current.Role`; việc kiểm tra quyền thật sự vẫn nằm ở tầng dưới.
- Chữ trên giao diện và thông báo bằng tiếng Anh.
- Dữ liệu hiển thị trên lưới lấy lại từ service sau mỗi thao tác, không tự sửa danh sách đang hiển thị.
- `appsettings.json` và `Data/*.json` được chép ra thư mục output và nằm cạnh file `.exe` khi publish.
- Sau khi thêm hoặc sửa Form, báo cho người dùng tên Form cần mở bằng Designer và các thao tác cần bấm thử.
