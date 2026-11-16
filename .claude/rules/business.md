---
paths:
  - "src/HotelCRM.Business/**"
---

# Quy ước tầng Business

- Chỉ tham chiếu project `Domain`. Không tham chiếu `Data` hay `WinForms`, không dùng kiểu của WinForms.
- Service nhận mọi phụ thuộc qua constructor dưới dạng interface: `IRepository<T>`, `ICustomerDirectory`, `IStayLookup`, `ITicketQuery`, `IAssignmentStrategy`, `ITicketObserver`. Không tự `new` repository, không gọi `AppConfig`.
- Mỗi service phụ trách một nhóm Use Case; tên hàm public trùng cột "Hàm điều khiển" trong bảng Use Case của `docs/PLAN.md`.
- **Service tra cứu, lớp Domain quyết định.** Service lấy thực thể và dữ liệu cần thiết từ repository hoặc interface ranh giới, truyền kết quả vào phương thức của lớp Domain, rồi lưu lại. Service không tự `throw` ngoại lệ nghiệp vụ; ngoại lệ đến từ lớp Domain. Tìm theo Id dùng `repository.GetRequired(id)` (ném `BusinessRuleException` khi không có), không viết `?? throw` trong service.
- Chú thích XML của từng hàm service liệt kê các bước phải làm theo thứ tự; thân hàm làm đúng các bước đó.
- Service của phân hệ Ticket không gọi service của phân hệ Khách hàng/Lưu trú và ngược lại. Cần dữ liệu của phân hệ kia thì đi qua interface ranh giới; lớp cài đặt nằm trong `Queries/` và chỉ dựa trên repository.
- Phân quyền (quy tắc 11): thao tác phân công, xử lý, đóng, mở lại ticket nhận người thực hiện qua tham số `Employee actor` và gọi `actor.EnsureCanAssign()`, `actor.EnsureCanClose()`, `actor.EnsureCanReopen()` hoặc `ticket.EnsureAssignedTo(actor)` trước khi làm. Tạo và tìm ticket không cần `actor`. Service không đọc `UserSession`; chỉ `AuthService` ghi `UserSession`.
- `BusinessSettings` và thời điểm `now` được truyền từ ngoài vào; service không gọi `DateTime.Now`.
- Strategy: `TicketService.AutoAssign()` chỉ gọi `IAssignmentStrategy.Pick(departmentCode, candidates)`, không biết lớp strategy cụ thể. `candidates` là danh sách `AgentWorkload(Agent, ActiveCount)` chỉ gồm Agent còn nhận được ticket (`Agent.CanTake`); strategy trả `null` khi danh sách rỗng và không tự ném lỗi.
- Factory Method: `TicketService.CreateTicket()` lấy factory từ bảng tra `tên loại → TicketFactory` nhận qua constructor; không `new ComplaintTicket(...)` trực tiếp và không `switch` theo loại.
- Observer: `TicketService` gắn observer cho mọi ticket lúc khởi tạo và cho ticket mới trong `CreateTicket()`. Observer chạy sau khi trạng thái đã đổi, tự lưu thực thể mà nó sửa, và không nuốt lỗi.
- Sau mỗi thay đổi, service gọi `repository.Save(entity)` (thêm mới hoặc ghi lại). Thực thể không hợp lệ thì không được lưu: tạo và kiểm tra xong mới `Save`. Không giữ bản sao dữ liệu riêng trong service.
- Kiểu trả về cho giao diện: thực thể của chính phân hệ, hoặc record nhỏ (`CustomerSummary`, `AgentPerformanceRow`, `DepartmentReportRow`). Form của phân hệ Ticket không nhận `Customer`, `Room`, `Booking`.
