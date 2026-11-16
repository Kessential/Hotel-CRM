---
paths:
  - "tests/**"
---

# Quy ước kiểm thử

- xUnit. Test chỉ phủ `Domain` và `Business`; không test Form.
- Danh sách 22 test nằm ở mục "Kiểm thử" của `docs/PLAN.md`.
- Thư mục `Tickets/` thuộc suất A. Thư mục `Customers/` và `Assignment/` thuộc suất B và chỉ có trên nhánh `local/b-work` cho tới khi thành viên B push.
- Lớp giả viết tay đặt trong `Fakes/`: `FakeCustomerDirectory` (`Add(customerId, profile)`), `FakeStayLookup` (`AddActiveStay(customerId, roomId)`) thuộc suất A; `FakeTicketQuery` (`AddOpenTicketFor(customerId)`, `MarkResolvedOrClosed(ticketId)`) thuộc suất B. Không thêm thư viện mock.
- Giá trị cố định và hàm dựng đối tượng dùng chung: `Tickets/TicketTestData` (suất A), `Customers/CustomerTestData` (suất B). Hai suất không dùng lớp trợ giúp của nhau.
- `TicketServiceTests` dùng một strategy giả lồng trong lớp test, không dùng strategy thật của suất B.
- Test của suất A không dùng lớp nào của `Domain/Customers` hay `Domain/Hotel`.
- **Không sửa, xóa hay bỏ qua (`Skip`) test có sẵn để làm cho nó xanh.** Test được viết ở GĐ 1 là đặc tả. Nếu tin rằng test sai, dừng lại, ghi vào `docs/PROGRESS.md` và hỏi người dùng.
- Ở cuối GĐ 1: 2 test định dạng của `CustomerTests` xanh, 20 test còn lại đỏ vì `NotImplementedException`. Test đỏ vì lý do khác là lỗi cần sửa. Khi lời gọi nằm trong `Assert.Throws`, xUnit báo "Exception type was not an exact match" và ghi `Actual: NotImplementedException`: vẫn là cùng lý do.
- Lộ trình xanh: 11 test `Tickets/` ở GĐ 2; 9 test suất B ở GĐ 3; 2 test `SurveyServiceTests` ở GĐ 4.
- Tên hàm test: `Method_Condition_Expected`. Tên lớp test: `<Chủ đề>Tests`.
- Mỗi test theo bố cục Arrange / Act / Assert, kiểm tra một hành vi.
- Dùng `InMemoryRepository<T>`; không đọc/ghi file thật.
- Thời điểm `now` là giá trị cố định khai báo trong test, không dùng `DateTime.Now`.
- Kiểm tra ngoại lệ bằng `Assert.Throws<BusinessRuleException>` hoặc `Assert.Throws<ValidationException>` đúng loại mà quy tắc quy định.
- Test về đa hình khai báo biến bằng kiểu cha (`SupportTicket`, `MembershipTier`, `Employee`) và gán đối tượng lớp con.
