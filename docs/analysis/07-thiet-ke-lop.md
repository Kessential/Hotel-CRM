# 07. Thiết kế lớp, package và hợp đồng giữa hai phân hệ

Module M6 của P1. [TV1] viết sơ đồ lớp phần chung + TV1, sơ đồ package, khai báo `ICustomerDirectory`, `IStayLookup`, `ITicketObserver`. [TV2] viết sơ đồ lớp TV2, khai báo `ITicketQuery`, `ISurveyScheduler`, `ICustomerCareStats`, `IReportExporter`. **Cả hai chốt bảng hợp đồng ở mục 4.**

## 1. Sơ đồ package và lớp phần chung + TV1 [TV1]
> TV1 viết: `diagrams/package.puml`, `diagrams/class.puml` (khung, khai báo `Person`, `Employee` và 3 interface phía TV1, rồi `!include class-tv1.iuml`, `!include class-tv2.iuml`).

## 2. Sơ đồ lớp TV2 [TV2]
Sơ đồ: `diagrams/class-tv2.iuml`. Đánh dấu «đã có» là lớp đã có code (PR #2); các lớp còn lại là thiết kế cho đợt 2 (P2) và P3.

### 2.1 Lớp Domain

| Lớp | Vai trò | Hành vi chính (quyết định quy tắc) | Trạng thái |
|---|---|---|---|
| `Customer` : `Person` | hồ sơ khách | `UpdateProfile`, `EnsureUnique` (Q7), `EnsureCanBeDeleted` (Q13), `EarnPoints` (Q9: cộng điểm đã tích, nâng hạng bằng `Next()`, trả `true` nếu lên hạng) | đợt 2 |
| `MembershipTier` (abstract) + `StandardTier`, `SilverTier`, `GoldTier`, `VipTier` | hạng, mỗi hạng tự trả số liệu của mình | `PointsFor`, `IsAtLeast`, `Next` | đã có |
| `Interaction` | một lần liên hệ với khách | kiểm tra nội dung không rỗng | đợt 2 |
| `RoomType`, `Room` | danh mục phòng (kết tập ◇) | `Room.EnsureAvailable` (Q8) | có (v1), sửa đợt 2 |
| `Booking` | một lần đặt/ở | constructor chốt `RatePerNight` (Q8); `CheckIn(now)`, `CheckOut(now)`, `Cancel()`; `Nights`, `TotalAmount` | sửa đợt 2 |
| `PointTransaction` | một dòng sổ điểm | 4 hàm `For…` bảo đảm đúng liên kết theo lý do | đã có |
| `Reward`, `RewardRedemption` | quà, lượt đổi | `EnsureCanRedeem`, `Redeem`, `CancelRedemption` (Q10) | đã có |
| `Campaign`, `CampaignTarget` (hợp thành ◆) | chiến dịch, khách nhận | `IsEligible`, `AddTarget`, `EnsureHasTargets`, `RecordResponse` (Q11) | đã có |
| `Receptionist` : `Employee` | vai trò Lễ tân | override các hàm quyền mà TV1 khai báo ở `Employee` | đợt 2, sau khi TV1 viết lại `Employee` |
| enum `ContactChannel`, `BookingStatus`, `PointReason`, `RedemptionStatus`, `CampaignResponse` | | | |

Hai thay đổi so với plan:
- **Không có `Customer.SpendPoints`.** Quy tắc đổi quà (Q10) đã do `Reward.EnsureCanRedeem(tier, balance)` quyết định. Số dư là tổng sổ điểm (không lưu, mục 3.4 của `06-erd.md`), nên `Customer` không giữ số dư để trừ.
- **`Customer.LifetimePoints`** (điểm đã tích) không phải cột CSDL. Repository tính bằng tổng các dòng Lưu trú và Thiện chí khi nạp khách. `EarnPoints` cộng vào con số này rồi xét hạng.

### 2.2 Lớp BLL

| Lớp | Vai trò | Phụ thuộc (qua constructor) |
|---|---|---|
| `ISegmentStrategy` + `VipSegment`, `LapsedGuestSegment`, `BirthdayMonthSegment`, `SegmentCandidate` | Strategy chọn khách (đã có) | – |
| `CustomerService` | UC07 | repository khách, `ITicketQuery` (L5) |
| `StayService` | UC08 | repository phòng, booking, khách, sổ điểm, tương tác; `ISurveyScheduler` (L3); `IUnitOfWork`; `IClock` |
| `LoyaltyService` | UC09 | repository quà, lượt đổi, sổ điểm, khách; `IUnitOfWork`; `IClock` |
| `CampaignService` | UC10, Context của Strategy | danh sách `ISegmentStrategy`; repository chiến dịch, khách; `ITicketQuery` (L4); `IUnitOfWork`; `IClock` |
| `CustomerReportService` | R3, R4 | truy vấn báo cáo (DAL) |
| `DashboardService` | R5 | truy vấn báo cáo; `ICustomerCareStats` (L6) |
| `CustomerDirectory`, `StayLookup` | cài `ICustomerDirectory`, `IStayLookup` cho TV1 | repository khách, booking |
| `GoodwillPointsObserver`, `InteractionLogger` | cài `ITicketObserver` cho TV1 (Q12) | repository sổ điểm, khách, tương tác |
| `ExcelReportExporter` | cài `IReportExporter` (ClosedXML), dùng chung | – |

Mọi hàm ghi của service nhận `Employee actor` và gọi kiểm quyền trước (Q6). Service không tự ném lỗi nghiệp vụ.

### 2.3 Cây kế thừa và đa hình (cho LSP)
- `MembershipTier` có 4 lớp con. Mọi nơi dùng kiểu cha: `Reward.MinTier`, `Customer.Tier`, `VipSegment` (gọi `IsAtLeast`, không `is VipTier`).
- `ISegmentStrategy` có 3 lớp cài đặt. `CampaignService` chỉ biết interface.
- `Receptionist` (TV2) đi qua `TicketService` (TV1) dưới kiểu `Employee`, không bị ép kiểu: LSP xuyên phân hệ.

## 3. Interface do TV2 khai báo [TV2]
Bên **dùng** khai báo, bên kia cài. Hợp đồng chỉ truyền Id, số và record nhỏ, **không truyền lớp Domain của phân hệ kia**. v1 đang trả `SupportTicket` qua `ITicketQuery`; v2 bỏ cách đó để `Domain/Customers` không phụ thuộc `Domain/Tickets`.

```csharp
namespace HotelCRM.Domain.Interfaces;

/// <summary>Questions the customer subsystem asks about tickets. Implemented by TV1 (TicketQuery).</summary>
public interface ITicketQuery
{
    /// <summary>L5, Q13: whether the customer has at least one ticket, in any state.</summary>
    bool HasAnyTicket(int customerId);

    /// <summary>
    /// L4, Q11: the customers among <paramref name="customerIds"/> who have a complaint ticket that is not closed.
    /// One query for the whole list.
    /// </summary>
    IReadOnlySet<int> GetCustomersWithOpenComplaints(IReadOnlyCollection<int> customerIds);
}

/// <summary>Creates pending satisfaction surveys. Implemented by TV1 (SurveyScheduler).</summary>
public interface ISurveyScheduler
{
    /// <summary>
    /// L3: creates the pending survey of a booking that has just been checked out. Runs inside the caller's
    /// transaction; an exception makes the caller roll back the check-out.
    /// </summary>
    void ScheduleFor(int bookingId, DateTime checkedOutAt);
}

/// <summary>Customer care figures for the 360 dashboard. Implemented by TV1 (CustomerCareStats).</summary>
public interface ICustomerCareStats
{
    /// <summary>L6: number of tickets and of complaint tickets per customer, for the given customers only.</summary>
    IReadOnlyDictionary<int, TicketCounts> GetTicketCounts(IReadOnlyCollection<int> customerIds);

    /// <summary>
    /// L6: average score (1-5) of each completed survey, for the given bookings only.
    /// Bookings without a completed survey are left out.
    /// </summary>
    IReadOnlyDictionary<int, decimal> GetSurveyAverages(IReadOnlyCollection<int> bookingIds);
}

/// <summary>Ticket figures of one customer.</summary>
public sealed record TicketCounts(int Total, int Complaints);
```

Vì sao `ICustomerCareStats` hỏi điểm khảo sát **theo booking** mà không theo khách:
- Bảng `surveys` chỉ có `BookingId`, không có `CustomerId` (giữ 3NF).
- Muốn biết khách thì phải JOIN sang `bookings`, mà đó là bảng của TV2. TV1 không được đọc bảng của TV2.
- Vì vậy TV2 đưa danh sách booking của khách, TV1 trả điểm của từng phiếu, rồi TV2 tự tính trung bình theo khách.
- **Định nghĩa (dùng cho bảng tính tay R5):** điểm hài lòng TB của khách = trung bình cộng điểm TB của các phiếu đã khảo sát của khách đó; khách chưa có phiếu nào thì để trống.

```csharp
namespace HotelCRM.BLL.Reporting;

/// <summary>Writes a report table to a file. Implemented by TV2 (ExcelReportExporter, ClosedXML); used by both.</summary>
public interface IReportExporter
{
    /// <summary>Writes the title, export time, column headers, rows and the total row to <paramref name="filePath"/>.</summary>
    void Export(ReportTable report, string filePath);
}

/// <summary>A report ready to export. Column kinds decide the cell format (VND #,##0 right aligned, dd/MM/yyyy).</summary>
public sealed record ReportTable(
    string Title,
    DateTime ExportedAt,
    IReadOnlyList<ReportColumn> Columns,
    IReadOnlyList<IReadOnlyList<object?>> Rows,
    IReadOnlyList<object?>? TotalRow);

/// <summary>One column of a report.</summary>
public sealed record ReportColumn(string Header, ReportColumnKind Kind);

/// <summary>How a column is formatted.</summary>
public enum ReportColumnKind { Text, Number, Money, Date, Percent }
```

## 4. Bảng hợp đồng giữa hai phân hệ [TV1][TV2]
Trạng thái: **TV2 đề xuất, chờ TV1 duyệt** (hạn 27/11). Sau khi chốt, đổi chữ ký phải được cả hai duyệt trong PR.

| Interface | Khai báo | Cài đặt | Hàm | Luồng | Test dùng Fake |
|---|---|---|---|---|---|
| `ITicketQuery` | TV2 | TV1 `TicketQuery` | `HasAnyTicket`, `GetCustomersWithOpenComplaints` | L4, L5 | TV2: `FakeTicketQuery` (TC2-09, TC2-10) |
| `ISurveyScheduler` | TV2 | TV1 `SurveyScheduler` | `ScheduleFor` | L3 | TV2: `FakeSurveyScheduler` (integration L3 rollback) |
| `ICustomerCareStats` | TV2 | TV1 `CustomerCareStats` | `GetTicketCounts`, `GetSurveyAverages` | L6 | TV2: `FakeCustomerCareStats` |
| `ICustomerDirectory` | TV1 | TV2 `CustomerDirectory` | TV1 điền (xem 4.1) | L1, R2 | TV1: `FakeCustomerDirectory` |
| `IStayLookup` | TV1 | TV2 `StayLookup` | TV1 điền (xem 4.1) | L1 (Q4) | TV1: `FakeStayLookup` |
| `ITicketObserver` | TV1 | TV2 `GoodwillPointsObserver`, `InteractionLogger` | TV1 điền (xem 4.1) | L2 | – (integration TV1) |
| `IReportExporter` | TV2 | TV2 `ExcelReportExporter` | `Export` | R1–R5 | – |
| `IUnitOfWork`, `IClock` | TV1 | TV1 | TV1 điền | L2, L3 | – |

### 4.1 Yêu cầu của TV2 với 3 interface phía TV1
TV1 viết chữ ký; TV2 cần những thông tin sau để cài đặt:
- **`ICustomerDirectory`:**
  - Tìm khách theo từ khóa: trả Id, tên, SĐT, tên hạng.
  - Hồ sơ SLA của một khách: `SlaFactor`, `PriorityBoost`.
  - Tên hạng **theo lô** cho R2. TV2 cài bằng một truy vấn.
- **`IStayLookup`:**
  - Khách có đang ở phòng này không (Q4).
  - Các phòng khách đang ở: Id → số phòng, để TV1 nạp combo phòng của ticket sửa chữa.
- **`ITicketObserver`** đề xuất nhận một record thay cho cả đối tượng `SupportTicket`, để observer của TV2 không phụ thuộc `Domain/Tickets`. Record gồm:
  - `TicketId`, `CustomerId`, trạng thái cũ và mới (tên), thời điểm, `EmployeeId` (người thao tác, có thể null);
  - `GoodwillPoints` (TV1 tính theo loại ticket);
  - tiêu đề ticket (ghi vào tương tác).
- **Giao dịch:** observer chạy bên trong `IUnitOfWork.Execute` của TV1. Observer ném lỗi thì TV1 rollback cả việc đóng ticket (Q12).

## 5. Câu hỏi còn mở [TV2]
1. TV1 duyệt bảng hợp đồng mục 4, nhất là: (a) bỏ việc trả `SupportTicket` qua `ITicketQuery`; (b) `ITicketObserver` nhận record thay cho `SupportTicket`; (c) `ICustomerCareStats` hỏi điểm khảo sát theo booking.
2. Q13 mở rộng sang khách đã nằm trong chiến dịch (câu hỏi ở `06-erd.md` mục 4). Nếu đồng ý thì `EnsureCanBeDeleted(hasStays, hasTickets, inCampaigns)`.
3. Tên hàm quyền ở `Employee` v2 (TV1 quyết) để `Receptionist` override.
