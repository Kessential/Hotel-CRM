# 07. Thiết kế lớp

Module M6 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.5). Sơ đồ: `diagrams/class.puml` (TV1) `!include` `class-common.iuml`, `class-tv1.iuml`, `class-tv2.iuml`; `diagrams/package.puml` (TV1).

## 1. Sơ đồ lớp tổng (ảnh) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. Lớp phần chung [Chung – TV1]
_[Chung – TV1] chưa điền._

## 3. Lớp phân hệ CSKH [TV1]
_[TV1] chưa điền._

## 4. Lớp phân hệ quan hệ khách hàng [TV2]
Sơ đồ: `diagrams/class-tv2.iuml`. Cột "Trạng thái": «đã có» là lớp đã có code (PR #2); các lớp còn lại là thiết kế cho đợt 2 (P2) và P3.

### 4.1 Lớp Domain
| Lớp | Vai trò | Hành vi chính (quyết định quy tắc) | Trạng thái |
|---|---|---|---|
| `Customer` : `Person` | hồ sơ khách | `UpdateProfile`, `EnsureUnique` (Q7), `EnsureCanBeDeleted(hasStays, hasTickets, inCampaigns)` (Q13), `EarnPoints` (Q9: cộng điểm đã tích, nâng hạng bằng `Next()`, trả `true` nếu lên hạng) | đợt 2 |
| `MembershipTier` (abstract) + `StandardTier`, `SilverTier`, `GoldTier`, `VipTier` | hạng, mỗi hạng tự trả số liệu của mình | `PointsFor`, `IsAtLeast`, `Next` | đã có |
| `Interaction` | một lần liên hệ với khách | kiểm tra nội dung không rỗng | đợt 2 |
| `RoomType`, `Room` | danh mục phòng (kết tập ◇) | `Room.EnsureAvailable` (Q8) | có (v1), sửa đợt 2 |
| `Booking` | một lần đặt/ở | constructor chốt `RatePerNight` (Q8); `CheckIn(now)`, `CheckOut(now)`, `Cancel()`; `Nights`, `TotalAmount` | sửa đợt 2 |
| `PointTransaction` | một dòng sổ điểm | 4 hàm `For…` bảo đảm đúng liên kết theo lý do | đã có |
| `Reward`, `RewardRedemption` | quà, lượt đổi | `EnsureCanRedeem`, `Redeem`, `CancelRedemption` (Q10) | đã có |
| `Campaign`, `CampaignTarget` (hợp thành ◆) | chương trình chăm sóc, khách nhận | `IsEligible`, `AddTarget`, `EnsureHasTargets`, `RecordResponse` (Q11) | đã có |
| `Receptionist` : `Employee` | vai trò Lễ tân | override các hàm quyền mà TV1 khai báo ở `Employee` | đợt 2, sau khi TV1 viết lại `Employee` |
| enum `ContactChannel`, `BookingStatus`, `PointReason`, `RedemptionStatus`, `CampaignResponse` | | | |

Một chỗ lệch so với PLAN 5.3 (ghi ở `docs/PROGRESS.md` mục 8, TV1 + TV2 duyệt trong PR #3) và một ghi chú thiết kế:
- **Không có `Customer.SpendPoints`.** Quy tắc đổi quà (Q10) đã do `Reward.EnsureCanRedeem(tier, balance)` quyết định. Số dư là tổng sổ điểm (không lưu, mục 3.4 của `06-erd.md`), nên `Customer` không giữ số dư để trừ.
- **`Customer.LifetimePoints`** (điểm đã tích) không phải cột CSDL. Repository tính bằng tổng các dòng Lưu trú và Thiện chí khi nạp khách. `EarnPoints` cộng vào con số này rồi xét hạng.

### 4.2 Lớp BLL

| Lớp | Vai trò | Phụ thuộc (qua constructor) |
|---|---|---|
| `ISegmentStrategy` + `VipSegment`, `LapsedGuestSegment`, `BirthdayMonthSegment`, `SegmentCandidate` | Strategy chọn khách (đã có) | – |
| `CustomerService` | UC07 | repository khách, booking, chương trình (Q13); `ITicketQuery` (L5; danh sách phiếu cho hồ sơ 360 đang đề xuất, mục 6.2) |
| `StayService` | UC08 | repository phòng, booking, khách, sổ điểm, tương tác; `ISurveyScheduler` (L3); `IUnitOfWork`; `IClock` |
| `LoyaltyService` | UC09 | repository quà, lượt đổi, sổ điểm, khách; `IUnitOfWork`; `IClock` |
| `CampaignService` | UC10, Context của Strategy | danh sách `ISegmentStrategy`; repository chương trình, khách; `ITicketQuery` (L4); `IUnitOfWork`; `IClock` |
| `CustomerReportService` | R3, R4 | truy vấn báo cáo (DAL) |
| `DashboardService` | R5 | truy vấn báo cáo; `ICustomerCareStats` (L6) |
| `CustomerDirectory`, `StayLookup` | cài `ICustomerDirectory`, `IStayLookup` cho TV1 | repository khách, booking |
| `GoodwillPointsObserver`, `InteractionLogger` | cài `ITicketObserver` cho TV1 (Q12) | repository sổ điểm, khách, tương tác |
| `ExcelReportExporter` | cài `IReportExporter` (ClosedXML), dùng chung | – |

Mọi hàm ghi của service nhận `Employee actor` và gọi kiểm quyền trước (Q6). Service không tự ném lỗi nghiệp vụ.

### 4.3 Câu hỏi còn mở
1. Tên hàm quyền ở `Employee` v2 (TV1 quyết) để `Receptionist` override.

Đã chốt với TV1 (PR #3, 26/11): Q13 mở rộng sang khách đã nằm trong chương trình chăm sóc (`06-erd.md` mục 3.5), nên hàm là `Customer.EnsureCanBeDeleted(hasStays, hasTickets, inCampaigns)`. `CustomerService` tra cứu ba giá trị, `Customer` quyết định và ném `BusinessRuleException`.

## 5. Package diagram và chiều phụ thuộc [Chung – TV1]
_[Chung – TV1] chưa điền._

## 6. Hợp đồng interface giữa hai phân hệ

### 6.1 Interface TV1 khai báo [TV1]
_[TV1] chưa điền._

### 6.2 Interface TV2 khai báo [TV2]
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

    /// <summary>Proposed 28/11, waiting for TV1: the tickets of one customer for the 360 profile, newest first.</summary>
    IReadOnlyList<CustomerTicket> GetCustomerTickets(int customerId);
}

/// <summary>One ticket as the customer profile shows it. TypeName and StateName are Vietnamese display names.</summary>
public sealed record CustomerTicket(
    int TicketId, string TypeName, string Title, string StateName, DateTime CreatedAt);

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

`ISegmentStrategy` không phải interface ranh giới (cả Strategy lẫn các lớp cài đều của TV2), nhưng là chỗ mở rộng của phân hệ (bài Live OCP, mục 7.2). Chữ ký như code PR #2; namespace đổi từ `HotelCRM.Business.Segments` sang `HotelCRM.BLL.Segments` khi TV1 đổi tên project ở P2.

```csharp
namespace HotelCRM.BLL.Segments;

/// <summary>One way of choosing the customers of a care campaign (Strategy). Context: CampaignService.</summary>
public interface ISegmentStrategy
{
    /// <summary>Short code stored in campaigns.SegmentCode. Unique among the registered strategies.</summary>
    string Code { get; }

    /// <summary>Name shown in the segment list of the campaign form.</summary>
    string DisplayName { get; }

    /// <summary>Whether the customer belongs to this segment on <paramref name="referenceDate"/> (campaign start date).</summary>
    bool Includes(SegmentCandidate customer, DateTime referenceDate);
}

/// <summary>What a strategy may look at for one customer; filled by CampaignService in one query.</summary>
public sealed record SegmentCandidate(
    int CustomerId,
    MembershipTier Tier,
    DateTime? DateOfBirth,
    IReadOnlyList<DateTime> StayCheckOutDates);
```

#### Đề xuất bảng hợp đồng (để chốt ở 6.3)
Trạng thái: **TV1 đã đồng ý ba điểm (a), (b), (c) dưới đây khi duyệt PR #3** (26/11); chữ ký chính thức ghi ở biên bản 6.3. Sau khi chốt, đổi chữ ký phải được cả hai duyệt trong PR.

| Interface | Khai báo | Cài đặt | Hàm | Luồng | Test dùng Fake |
|---|---|---|---|---|---|
| `ITicketQuery` | TV2 | TV1 `TicketQuery` | `HasAnyTicket`, `GetCustomersWithOpenComplaints`; đề xuất `GetCustomerTickets` | L4, L5; hồ sơ 360 | TV2: `FakeTicketQuery` (TC2-09, TC2-10) |
| `ISurveyScheduler` | TV2 | TV1 `SurveyScheduler` | `ScheduleFor` | L3 | TV2: `FakeSurveyScheduler` (TC2-12) |
| `ICustomerCareStats` | TV2 | TV1 `CustomerCareStats` | `GetTicketCounts`, `GetSurveyAverages` | L6 | TV2: `FakeCustomerCareStats` (TC2-13) |
| `ICustomerDirectory` | TV1 | TV2 `CustomerDirectory` | TV1 điền (xem yêu cầu bên dưới) | L1, R2 | TV1: `FakeCustomerDirectory` |
| `IStayLookup` | TV1 | TV2 `StayLookup` | TV1 điền (xem yêu cầu bên dưới) | L1 (Q4) | TV1: `FakeStayLookup` |
| `ITicketObserver` | TV1 | TV2 `GoodwillPointsObserver`, `InteractionLogger` | TV1 điền (xem yêu cầu bên dưới) | L2 | – (integration TV1) |
| `IReportExporter` | TV2 | TV2 `ExcelReportExporter` | `Export` | R1–R5 | – |
| `IUnitOfWork`, `IClock` | TV1 | TV1 | TV1 điền | L2, L3 | – |

#### Yêu cầu của TV2 với 3 interface phía TV1
TV1 viết chữ ký; TV2 cần những thông tin sau để cài đặt:
- **`ICustomerDirectory`:**
  - Tìm khách theo từ khóa: trả Id, tên, SĐT, tên hạng.
  - Hồ sơ SLA của một khách: `SlaFactor`, `PriorityBoost`.
  - Tên hạng **theo lô** cho R2. TV2 cài bằng một truy vấn.
- **`IStayLookup`:**
  - Khách có đang ở phòng này không (Q4).
  - Các phòng khách đang ở: Id → số phòng, để TV1 nạp danh sách chọn phòng của phiếu sửa chữa.
- **`ITicketObserver`** đề xuất nhận một record thay cho cả đối tượng `SupportTicket`, để observer của TV2 không phụ thuộc `Domain/Tickets`. Record gồm:
  - `TicketId`, `CustomerId`, trạng thái cũ và mới (tên), thời điểm, `EmployeeId` (người thao tác, có thể null);
  - `GoodwillPoints` (TV1 tính theo loại phiếu);
  - tiêu đề phiếu (ghi vào tương tác).
- **Giao dịch:** observer chạy bên trong `IUnitOfWork.Execute` của TV1. Observer ném lỗi thì TV1 rollback cả việc đóng phiếu (Q12).

Ba điểm TV1 đã đồng ý (PR #3): (a) bỏ việc trả `SupportTicket` qua `ITicketQuery`; (b) `ITicketObserver` nhận record thay cho `SupportTicket`; (c) `ICustomerCareStats` hỏi điểm khảo sát theo booking.

**Đề xuất bổ sung (28/11, chờ TV1 duyệt):** `ITicketQuery.GetCustomerTickets` cho tab Phiếu yêu cầu ở `frm201_dm_customer` (PLAN 5.1 bản web).
- TV2 không được đọc bảng `tickets`, nên phải hỏi qua interface.
- Khi duyệt PR #3, điểm (a) được hiểu là `ITicketQuery` chỉ trả Id và bool. Hàm này trả thêm một record nhỏ, vẫn không truyền lớp Domain của phân hệ kia.
- Code viết sau khi TV1 đồng ý.

### 6.3 Biên bản chốt (27/11) [Chung – TV1 + TV2]
_[Chung – TV1 + TV2] chưa điền (họp chốt 27/11)._

## 7. Mẫu GoF

### 7.1 State, Factory Method [TV1]
_[TV1] chưa điền._

### 7.2 Strategy [TV2]

| Vai trò trong mẫu | Lớp |
|---|---|
| Strategy | `ISegmentStrategy` (`Code`, `DisplayName`, `Includes`) |
| ConcreteStrategy | `VipSegment` (hạng ≥ VIP, so bằng `MembershipTier.IsAtLeast`), `LapsedGuestSegment` (lần trả phòng gần nhất cách ngày bắt đầu ≥ N ngày, N = 180 đọc từ `appsettings.json`), `BirthdayMonthSegment` (tháng sinh = tháng bắt đầu chương trình) |
| Context | `CampaignService`: nhận danh sách `ISegmentStrategy` qua constructor; `GetSegments` trả danh sách phân khúc cho ô chọn trên trang `frm206_nv_campaign`; `PreviewTargets` tìm Strategy theo `Code` người dùng chọn rồi gọi `Includes` cho từng `SegmentCandidate` |
| Client | `Program.cs` tạo 3 Strategy và đưa vào `CampaignService`; `frm206` cho Quản lý chọn phân khúc lúc chạy |

**Vấn đề giải quyết:** mỗi đợt ưu đãi chọn khách theo một cách khác. Không có Strategy thì `CampaignService` phải `switch (segmentCode)` và mỗi phân khúc mới là một lần sửa service đang chạy đúng. Có Strategy thì service chỉ biết interface, còn mỗi cách chọn khách là một lớp nhỏ test riêng được (`SegmentStrategyTests`, 11 trường hợp, không cần CSDL).

**Live OCP (bài tập khi tắt AI):** thêm `FrequentGuestSegment` (khách có ≥ 3 lần trả phòng trong 365 ngày trước ngày bắt đầu): một lớp mới cài `ISegmentStrategy`, dùng `SegmentCandidate.StayCheckOutDates`, và một dòng đăng ký trong `Program.cs`. Không sửa `CampaignService`, `frm206` hay lớp phân khúc nào đã có; ô chọn phân khúc tự có thêm dòng mới.

**Nhược điểm:**
1. **`SegmentCandidate` phải đủ dữ liệu cho mọi Strategy.** Phân khúc mới cần dữ liệu chưa có (ví dụ tổng chi tiêu) thì phải sửa cả `SegmentCandidate` lẫn truy vấn nạp nó trong `CampaignService`. Tính "mở để mở rộng" chỉ giữ được khi dữ liệu đã đủ.
2. **Lọc trong bộ nhớ.** Service nạp mọi khách rồi gọi `Includes` cho từng khách. Với vài trăm khách của bài thì nhanh; với hàng trăm nghìn khách thì nên đẩy điều kiện xuống SQL, khi đó mỗi Strategy phải sinh điều kiện `WHERE` và mất tính đơn giản.
3. **`Code` phải ổn định.** `campaigns.SegmentCode` lưu `Code` của Strategy; đổi `Code` hoặc gỡ một Strategy thì chương trình cũ không còn tên phân khúc để hiển thị (R4 chỉ còn mã).

### 7.3 Observer (cầu nối) [Chung – TV1 phát, TV2 nhận]

#### Phía phát sự kiện [TV1]
_[TV1] chưa điền._

#### Phía nhận sự kiện [TV2]
- **Observer:** `GoodwillPointsObserver` và `InteractionLogger` cài `ITicketObserver` (TV1 khai báo). `Program.cs` đăng ký hai observer vào phía phiếu yêu cầu của TV1; thêm observer mới chỉ cần một lớp và một dòng đăng ký.
- **`GoodwillPointsObserver`:** chỉ làm khi trạng thái mới là Đã đóng; ghi `PointTransaction.ForGoodwill` (điểm theo loại phiếu do TV1 tính) và gọi `Customer.EarnPoints` (có thể lên hạng; Q9, Q12).
- **`InteractionLogger`:** ghi một `Interaction` gắn `TicketId`, nội dung là tiêu đề phiếu và trạng thái mới.
- **Giao dịch:** cả hai chạy bên trong `IUnitOfWork.Execute` của `TicketService`; observer ném ngoại lệ thì TV1 rollback cả việc đóng phiếu (L2).
- **Không phụ thuộc ngược:** observer nhận một record (`TicketId`, `CustomerId`, trạng thái, `GoodwillPoints`, tiêu đề) chứ không nhận `SupportTicket`, nên `Domain/Customers` và `Domain/Loyalty` không tham chiếu `Domain/Tickets` (đề xuất ở 6.2, chốt ở 6.3).
- **Nhược điểm (nhìn từ phía nhận):**
  1. Thứ tự gọi observer phụ thuộc thứ tự đăng ký. Hai observer của TV2 độc lập nhau nên không sao, nhưng nếu sau này có observer cần kết quả của observer trước thì phải sửa cách đăng ký.
  2. Lỗi ở observer làm hỏng việc chính (đóng phiếu bị rollback). Ở L2 đó là chủ đích (điểm thiện chí phải đi cùng việc đóng), nhưng với một observer chỉ ghi log thì cách này quá chặt.
  3. Khó lần theo luồng khi đọc code: `TicketService` không gọi tên `GoodwillPointsObserver` ở đâu cả; phải xem `Program.cs` hoặc đặt breakpoint mới thấy ai nhận sự kiện.

## 8. Cây kế thừa và LSP

### 8.1 Cây `Employee`, `SupportTicket`, `TicketState` [TV1]
_[TV1] chưa điền._

### 8.2 Cây `MembershipTier` [TV2]
- `MembershipTier` (abstract) có 4 lớp con `StandardTier`, `SilverTier`, `GoldTier`, `VipTier`. Mỗi lớp con chỉ trả số liệu của mình (`Name`, `MinPoints`, `EarnRate`, `SlaFactor`, `PriorityBoost`) và hạng kế tiếp (`Next()`); phép tính dùng chung nằm ở lớp cha (`PointsFor`, `IsAtLeast`).
- **LSP:** mọi nơi dùng kiểu cha và không ép kiểu: `Customer.Tier`, `Reward.MinTier`, `SegmentCandidate.Tier`, `VipSegment` (gọi `IsAtLeast`, không `is VipTier`), `CustomerDirectory` trả `SlaFactor`/`PriorityBoost` cho TV1. Thay một hạng bằng hạng khác thì mọi hàm vẫn giữ hợp đồng: `PointsFor` không âm, `Next()` của `VipTier` trả `null` (hạng cao nhất).
- `ISegmentStrategy` có 3 lớp cài đặt; `CampaignService` chỉ biết interface.
- **LSP xuyên phân hệ:** `Receptionist` (TV2) đi qua `TicketService` (TV1) dưới kiểu `Employee`, không bị ép kiểu.
