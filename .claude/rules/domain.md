---
paths:
  - "src/HotelCRM.Domain/**"
---

# Quy ước tầng Domain

- Project này không tham chiếu project nào và không dùng gói NuGet nào.
- Mỗi file một lớp/interface/enum; namespace khớp thư mục (`HotelCRM.Domain.Tickets`, ...).
- Trạng thái của thực thể chỉ đổi qua phương thức của chính nó: thuộc tính dùng `private set` hoặc field `private`; tập hợp lộ ra ngoài dưới dạng `IReadOnlyList<T>`.
- Constructor public và phương thức public kiểm tra dữ liệu. Không để thực thể tồn tại ở trạng thái sai.
- Khôi phục từ file dùng hàm `internal static Restore(...)`: dựng lại đối tượng với trạng thái đã lưu, không chạy lại quy tắc nghiệp vụ. Chỉ project `Data` gọi được (qua `InternalsVisibleTo`). Mẫu đang dùng: constructor `private` chỉ gán field; constructor public gọi nó rồi gọi một hàm kiểm tra `private`; `Restore` gọi thẳng constructor `private`. Ticket gói giá trị đã lưu trong `TicketSnapshot` (internal). Riêng định dạng tên/SĐT/email trong `Person` luôn được kiểm tra, kể cả khi `Restore`.
- Quy tắc cần dữ liệu của nhiều đối tượng được viết thành phương thức nhận kết quả tra cứu làm tham số, ví dụ `Agent.EnsureCanTake(activeCount, maxActive)`, `Customer.EnsureCanBeDeleted(hasOpenTickets)`. Lớp Domain quyết định và ném ngoại lệ; service chỉ tra cứu.
- Hàm tên `EnsureXxx` ném ngoại lệ khi điều kiện sai; hàm tên `CanXxx`/`IsXxx` chỉ trả `bool`. Quyền (quy tắc 11): `Employee.CanAssign()/CanClose()/CanReopen()` do `Agent`/`Manager` override, service gọi `EnsureCanAssign()/EnsureCanClose()/EnsureCanReopen()`.
- "Không tìm thấy theo Id" do `RepositoryExtensions.GetRequired` (trong `Domain/Interfaces`) ném `BusinessRuleException`.
- Loại ngoại lệ:
  - `ValidationException`: dữ liệu của một đối tượng không hợp lệ (sai định dạng, rỗng, ngoài khoảng).
  - `BusinessRuleException`: vi phạm quy tắc giữa các đối tượng, sai trạng thái, sai quyền.
  - Cả hai kế thừa `DomainException`. Không ném `Exception`, `ArgumentException`, `InvalidOperationException` cho lỗi nghiệp vụ. Ngoại lệ duy nhất: `TicketState.FromName` và `MembershipTier.FromName` ném `ArgumentException` khi tên trong file dữ liệu không hợp lệ (lỗi dữ liệu, không phải lỗi nghiệp vụ).
  - Thông báo lỗi bằng tiếng Anh, đủ rõ để hiện thẳng lên giao diện.
- Không gọi `DateTime.Now`/`DateTime.UtcNow`: thời điểm đi vào qua tham số `now`.
- Không đọc cấu hình: giới hạn như `MaxActiveTickets`, `ReopenDays` đi vào qua tham số hoặc `BusinessSettings`.
- Không gắn attribute JSON, không có code đọc/ghi file. Việc lưu trữ thuộc tầng Data.
- Con số và định nghĩa nghiệp vụ lấy từ mục "Quyết định thiết kế bổ sung" của `docs/PLAN.md`; đặt thành hằng số hoặc thuộc tính `override` trong chính lớp liên quan.
- Đa hình thay cho `switch` theo kiểu: hành vi khác nhau giữa các lớp con đặt trong thành viên `abstract`/`virtual` được `override`.
- State pattern: `SupportTicket` ủy quyền `Start(now)`, `Resolve(note, now)`, `Close(now)`, `Reopen(now, reopenDays)` cho `TicketState` hiện tại; chỉ lớp State quyết định chuyển sang trạng thái nào, qua `ticket.MoveTo(next, now)`. Lớp cha `TicketState` từ chối mọi hành động; lớp con chỉ override hành động hợp lệ. Nơi khác hỏi trạng thái bằng `State.IsActive`/`State.IsClosed`/`State.Name`, không dùng `is OpenState`.
- Tên dùng để lưu và tra bảng là hằng số của chính lớp: `OpenState.StateName`, `GoldTier.TierName`, `ComplaintTicket.TypeKey`.
- Kết tập hai chiều: `Agent` tự đăng ký vào `Department.Agents`, `Room` tự đăng ký vào `RoomType.Rooms` trong constructor.
- Ranh giới hai phân hệ: `Domain/Tickets` không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`, và ngược lại. Chỉ `ITicketObserver` và `ITicketQuery` trong `Domain/Interfaces` đưa `SupportTicket` sang cho tầng Business của suất B. Tham chiếu sang phân hệ kia chỉ giữ Id (`CustomerId`, `RoomId`, `TicketId`); thông tin hạng khách đi vào ticket dưới dạng số qua `CustomerSlaProfile`.
- Mọi thành viên public có chú thích XML `<summary>`; phương thức ném ngoại lệ có thêm `<exception>`.
