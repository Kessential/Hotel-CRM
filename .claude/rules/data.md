---
paths:
  - "src/HotelCRM.Data/**"
---

# Quy ước tầng Data

- Toàn bộ project này là phần chung, viết xong ở GĐ 1 (gồm record và mapper của mọi thực thể). Các giai đoạn sau không sửa; cần sửa thì dừng và hỏi.
- Chỉ tham chiếu project `Domain`. Dùng `System.Text.Json` có sẵn, không thêm gói NuGet.
- `IRepository<T>` có 5 hàm: `GetAll`, `GetById`, `NextId`, `Save` (thêm nếu Id mới, luôn ghi lại), `Delete`.
- `JsonRepository<TEntity, TRecord>` cài `IRepository<TEntity>`: nạp file một lần vào bộ nhớ, luôn trả về cùng một đối tượng cho cùng một Id, ghi lại cả file ở mỗi lần `Save`/`Delete` (ghi ra file tạm rồi thay thế). `GetAll` trả về bản chụp, nên duyệt danh sách trong lúc `Save` không lỗi.
- `InMemoryRepository<T>` cài cùng interface, dùng cho test.
- Thực thể không được serialize trực tiếp. Mỗi kiểu có một `*Record` (DTO phẳng, thuộc tính `get; set;`) trong `Records/` và một mapper trong `Mappers/` cài `IMapper<TEntity, TRecord>`.
- Mapper dựng lại thực thể bằng hàm `internal static Restore(...)` của lớp Domain, không gọi constructor public (constructor public chạy quy tắc nghiệp vụ).
- Tên trường của record trùng tên thuộc tính của thực thể (PascalCase). Enum ghi dạng chuỗi. Thời gian ghi ISO 8601.
- Lớp con phân biệt bằng trường chuỗi: `tickets.json` có `Type` và `State`; `employees.json` có `Role`; `customers.json` có `Tier`, `Points` và mảng `Interactions`; `departments.json` có `Code`. Dựng lại bằng `TicketState.FromName`, `MembershipTier.FromName`.
- Mapper cần đối tượng tham chiếu (ví dụ `Booking` cần `Room` và `Customer`) thì nhận repository tương ứng qua constructor.
- 8 file dữ liệu, nạp đúng thứ tự: `departments.json`, `employees.json`, `roomtypes.json`, `rooms.json`, `customers.json`, `bookings.json`, `tickets.json`, `surveys.json`.
- Repository cấp Id kế tiếp (lớn nhất hiện có + 1).
- `AppConfig` là Singleton (`AppConfig.Instance`), đọc `appsettings.json` cạnh file chạy. Đường dẫn tính từ `AppContext.BaseDirectory`; không gán cứng đường dẫn tuyệt đối. `AppConfig` chỉ được gọi trong `Program.cs`.
- Lỗi đọc/ghi file ném ngoại lệ có thông báo nêu tên file; không nuốt lỗi. Thiếu file: `FileNotFoundException`. JSON hỏng, tên trạng thái/hạng/loại lạ, tham chiếu tới Id không tồn tại: `InvalidDataException`.
- File dữ liệu mẫu nằm ở `src/HotelCRM.WinForms/Data/` và được chép ra cạnh file chạy; app đọc và ghi bản ở thư mục output, không ghi vào bản trong mã nguồn.
- Chuỗi băm mật khẩu trong `employees.json` phải sinh bằng `PasswordHasher` (PBKDF2-SHA256, 100.000 vòng, salt 16 byte, hash 32 byte, Base64), không gõ tay. Lệnh: `dotnet run tools/HashPassword.cs -- <mật khẩu>` từ thư mục gốc repo.
