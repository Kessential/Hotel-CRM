# 06. Sơ đồ thực thể liên kết và CSDL

Module M5 của P1 (`docs/tasks/v2-p1-analysis.md` mục 3.4). Sơ đồ: `diagrams/erd.puml` (TV1) `!include` `erd-tv1.iuml` và `erd-tv2.iuml`. Khóa ngoại xuất phát từ bảng của ai thì người đó vẽ.

## 1. ERD tổng (ảnh) [Chung – TV1]
_[Chung – TV1] chưa điền._

## 2. Bảng phân hệ CSKH (7 bảng) [TV1]
_[TV1] chưa điền._

## 3. Bảng phân hệ quan hệ khách hàng (10 bảng) [TV2]
Sơ đồ: `diagrams/erd-tv2.iuml`.

| Bảng | Loại | Lớp Domain | Ghi chú |
|---|---|---|---|
| `customers` | thực thể | `Customer` | |
| `interactions` | giao dịch | `Interaction` | nối sang TV1 (`TicketId`) |
| `room_types` | **danh mục** | `RoomType` | |
| `rooms` | thực thể | `Room` | TV1 trỏ tới (`tickets.RoomId`) |
| `bookings` | giao dịch | `Booking` | TV1 trỏ tới (`surveys.BookingId`) |
| `point_transactions` | giao dịch | `PointTransaction` | nối sang TV1 (`TicketId`) |
| `rewards` | **danh mục** | `Reward` | |
| `reward_redemptions` | **n–n** khách–quà, có thuộc tính | `RewardRedemption` | |
| `campaigns` | giao dịch | `Campaign` | |
| `campaign_targets` | **n–n** chiến dịch–khách, có thuộc tính | `CampaignTarget` | khóa ghép |

### 3.1 Quy ước tên và kiểu (TV2 đề xuất, TV1 chốt khi viết khung `schema.sql`)
- **Tên:** bảng là danh từ số nhiều, `snake_case` (`room_types`); cột là `PascalCase` trùng tên thuộc tính C# (`RatePerNight`). Khóa chính `Id INTEGER PRIMARY KEY` (tự tăng), trừ bảng n–n thuần dùng khóa ghép.
- **Kiểu SQLite:**
  - Tiền VND: `INTEGER` (đồng, không có phần lẻ; C# đọc thành `decimal`).
  - Ngày: `TEXT 'yyyy-MM-dd'`. Thời điểm: `TEXT 'yyyy-MM-dd HH:mm:ss'`.
  - Đúng/sai: `INTEGER` 0/1 có `CHECK`.
  - Trạng thái, hạng, lý do: `TEXT` có `CHECK (… IN (…))`, giá trị là tên hằng trong code (`GoldTier.TierName`, tên enum).
- **Khóa ngoại:** bật `PRAGMA foreign_keys = ON` mỗi kết nối; mỗi cột khóa ngoại có index. `ON DELETE` khớp UML: hợp thành (◆) → `CASCADE`; còn lại → `RESTRICT`.

### 3.2 Chi tiết cột và ràng buộc

**`customers`**

| Cột | Kiểu | Ràng buộc |
|---|---|---|
| Id | INTEGER | PK |
| FullName | TEXT | NOT NULL |
| Phone | TEXT | NOT NULL, UNIQUE, `CHECK (length(Phone) = 10)` |
| Email | TEXT | NOT NULL, UNIQUE |
| IdentityNumber | TEXT | NOT NULL, UNIQUE |
| Nationality | TEXT | NOT NULL |
| DateOfBirth | TEXT | NULL |
| MarketingConsent | INTEGER | NOT NULL DEFAULT 0, `CHECK (MarketingConsent IN (0, 1))` |
| Tier | TEXT | NOT NULL DEFAULT 'Standard', `CHECK (Tier IN ('Standard', 'Silver', 'Gold', 'VIP'))` |
| Notes | TEXT | NOT NULL DEFAULT '' |
| CreatedAt | TEXT | NOT NULL |

Định dạng chi tiết của Q7 (CCCD theo quốc tịch, email) kiểm ở Domain; CSDL chỉ giữ ràng buộc đơn giản làm lớp chặn thứ hai.

**`interactions`**

| Cột | Kiểu | Ràng buộc |
|---|---|---|
| Id | INTEGER | PK |
| CustomerId | INTEGER | NOT NULL, FK → `customers` RESTRICT |
| OccurredAt | TEXT | NOT NULL |
| Channel | TEXT | NOT NULL, `CHECK (Channel IN ('FrontDesk', 'Phone', 'Email', 'System'))` |
| Summary | TEXT | NOT NULL |
| EmployeeId | INTEGER | NULL, FK → `employees` RESTRICT; NULL = hệ thống tự ghi (observer) |
| TicketId | INTEGER | NULL, FK → `tickets` RESTRICT; có khi tương tác do đóng ticket (Q12) |

**`room_types`**: Id PK; Name TEXT NOT NULL UNIQUE; PricePerNight INTEGER NOT NULL `CHECK (> 0)`; Capacity INTEGER NOT NULL `CHECK (> 0)`.

**`rooms`**: Id PK; Number TEXT NOT NULL UNIQUE; Floor INTEGER NOT NULL `CHECK (> 0)`; RoomTypeId INTEGER NOT NULL FK → `room_types` RESTRICT.

**`bookings`**

| Cột | Kiểu | Ràng buộc |
|---|---|---|
| Id | INTEGER | PK |
| CustomerId | INTEGER | NOT NULL, FK → `customers` RESTRICT |
| RoomId | INTEGER | NOT NULL, FK → `rooms` RESTRICT |
| CheckInDate | TEXT | NOT NULL |
| CheckOutDate | TEXT | NOT NULL, `CHECK (CheckOutDate > CheckInDate)` |
| RatePerNight | INTEGER | NOT NULL, `CHECK (RatePerNight > 0)`; **thuộc tính lịch sử** |
| Status | TEXT | NOT NULL, `CHECK (Status IN ('Reserved', 'CheckedIn', 'CheckedOut', 'Cancelled'))` |
| CreatedById | INTEGER | NOT NULL, FK → `employees` RESTRICT |
| CreatedAt | TEXT | NOT NULL |
| CheckedInAt | TEXT | NULL; `CHECK ((Status IN ('CheckedIn', 'CheckedOut')) = (CheckedInAt IS NOT NULL))` |
| CheckedOutAt | TEXT | NULL; `CHECK ((Status = 'CheckedOut') = (CheckedOutAt IS NOT NULL))` |

So sánh ngày dạng `TEXT 'yyyy-MM-dd'` đúng theo thứ tự thời gian. Quy tắc "không trùng lịch" (Q8) liên quan nhiều dòng nên không viết được bằng `CHECK`; Domain kiểm tra trong cùng giao dịch với lệnh ghi.

**`point_transactions`**

| Cột | Kiểu | Ràng buộc |
|---|---|---|
| Id | INTEGER | PK |
| CustomerId | INTEGER | NOT NULL, FK → `customers` RESTRICT |
| Points | INTEGER | NOT NULL, `CHECK (Points <> 0)` |
| Reason | TEXT | NOT NULL, `CHECK (Reason IN ('Stay', 'Goodwill', 'Redemption'))` |
| BookingId | INTEGER | NULL, FK → `bookings` RESTRICT |
| TicketId | INTEGER | NULL, FK → `tickets` RESTRICT |
| RedemptionId | INTEGER | NULL, FK → `reward_redemptions` RESTRICT |
| CreatedAt | TEXT | NOT NULL |

Ràng buộc thêm, khớp với các hàm `For…` của `PointTransaction`:
```sql
CHECK (
     (Reason = 'Stay'       AND Points > 0 AND BookingId IS NOT NULL AND TicketId IS NULL     AND RedemptionId IS NULL)
  OR (Reason = 'Goodwill'   AND Points > 0 AND TicketId IS NOT NULL  AND BookingId IS NULL    AND RedemptionId IS NULL)
  OR (Reason = 'Redemption'                AND RedemptionId IS NOT NULL AND BookingId IS NULL AND TicketId IS NULL)
)
CREATE UNIQUE INDEX ux_points_stay     ON point_transactions(BookingId) WHERE Reason = 'Stay';      -- mỗi lần ở cộng điểm một lần (Q9)
CREATE UNIQUE INDEX ux_points_goodwill ON point_transactions(TicketId)  WHERE Reason = 'Goodwill';  -- mỗi ticket một lần (Q12)
```

**`rewards`**: Id PK; Name TEXT NOT NULL UNIQUE; PointsCost INTEGER NOT NULL `CHECK (> 0)`; MinTier TEXT NOT NULL `CHECK (IN ('Standard', 'Silver', 'Gold', 'VIP'))`; Quantity INTEGER NOT NULL `CHECK (Quantity >= 0)` (Q10, chặn hai người cùng đổi món cuối); IsActive INTEGER NOT NULL `CHECK (IN (0, 1))`.

**`reward_redemptions`**

| Cột | Kiểu | Ràng buộc |
|---|---|---|
| Id | INTEGER | PK (khách đổi một món nhiều lần nên không dùng khóa ghép) |
| CustomerId | INTEGER | NOT NULL, FK → `customers` RESTRICT |
| RewardId | INTEGER | NOT NULL, FK → `rewards` RESTRICT |
| PointsSpent | INTEGER | NOT NULL, `CHECK (PointsSpent > 0)`; **thuộc tính lịch sử** |
| RedeemedAt | TEXT | NOT NULL |
| Status | TEXT | NOT NULL, `CHECK (Status IN ('Completed', 'Cancelled'))` |
| ProcessedById | INTEGER | NOT NULL, FK → `employees` RESTRICT |

**`campaigns`**: Id PK; Name TEXT NOT NULL; SegmentCode TEXT NOT NULL; Offer TEXT NOT NULL; StartDate TEXT NOT NULL; EndDate TEXT NOT NULL `CHECK (EndDate >= StartDate)`; CreatedById INTEGER NOT NULL FK → `employees` RESTRICT; CreatedAt TEXT NOT NULL.

`SegmentCode` không có khóa ngoại: phân khúc là lớp Strategy trong code (`ISegmentStrategy.Code`), không phải dữ liệu, nên không có bảng `segments`. Thêm phân khúc là thêm lớp, không thêm dòng.

**`campaign_targets`**

| Cột | Kiểu | Ràng buộc |
|---|---|---|
| CampaignId | INTEGER | PK (ghép), FK → `campaigns` **CASCADE** (hợp thành) |
| CustomerId | INTEGER | PK (ghép), FK → `customers` RESTRICT |
| Channel | TEXT | NOT NULL, `CHECK (Channel IN ('FrontDesk', 'Phone', 'Email', 'System'))` |
| SentAt | TEXT | NOT NULL |
| Response | TEXT | NULL, `CHECK (Response IN ('Interested', 'Booked', 'Declined'))` |
| RespondedAt | TEXT | NULL; `CHECK ((Response IS NULL) = (RespondedAt IS NULL))` |
| RecordedById | INTEGER | NULL, FK → `employees` RESTRICT; `CHECK ((Response IS NULL) = (RecordedById IS NULL))` |

Khóa ghép `(CampaignId, CustomerId)` chính là quy tắc "mỗi khách một lần trong một chiến dịch" (Q11), lớp chặn thứ hai sau `Campaign.AddTarget`.

### 3.3 Quan hệ trong phân hệ

| Quan hệ | Bản số | UML | ON DELETE |
|---|---|---|---|
| `customers` – `bookings`, `interactions`, `point_transactions`, `reward_redemptions`, `campaign_targets` | 1 – 0..n | kết hợp | RESTRICT (Q13) |
| `room_types` – `rooms` | 1 – 0..n | kết tập ◇ | RESTRICT |
| `rooms` – `bookings` | 1 – 0..n | kết hợp | RESTRICT |
| `rewards` – `reward_redemptions` | 1 – 0..n | kết hợp | RESTRICT |
| `campaigns` – `campaign_targets` | 1 – 1..n | **hợp thành ◆** | **CASCADE** |
| `bookings` – `point_transactions` (Lưu trú) | 1 – 0..1 | kết hợp | RESTRICT |
| `reward_redemptions` – `point_transactions` | 1 – 1..2 (trừ, hoàn) | kết hợp | RESTRICT |

### 3.4 Chuẩn hóa các bảng TV2
- **1NF:** mọi cột là giá trị đơn. Lịch sử tương tác, các lần ở, các giao dịch điểm, danh sách khách của chiến dịch đều tách thành bảng riêng, không gộp vào một cột của `customers`.
- **2NF:** chỉ `campaign_targets` có khóa ghép. `Channel`, `SentAt`, `Response`, `RespondedAt`, `RecordedById` phụ thuộc vào cả cặp (chiến dịch, khách), không phụ thuộc vào một nửa khóa.
- **3NF:**
  - Số đêm, thành tiền: không lưu, tính từ ngày và `RatePerNight`.
  - Số dư điểm, điểm đã tích: không lưu, tính bằng tổng `point_transactions`.
  - Tỷ lệ phản hồi chiến dịch: không lưu, tính từ `campaign_targets`.
  - Loại phòng của booking: lấy qua `rooms`, không chép vào `bookings`.
- **Dữ liệu dẫn xuất được lưu có chủ đích: `customers.Tier`.**
  - Hạng tính được từ điểm đã tích, nhưng được lưu vì quy tắc "hạng không bao giờ hạ" (Q9). Nếu sau này đổi ngưỡng hạng, khách đã lên hạng vẫn giữ hạng; tức hạng là một sự kiện đã xảy ra, không chỉ là kết quả tính.
  - Ngoài ra, phân hệ TV1 hỏi hạng theo lô để tính SLA (`ICustomerDirectory`). Có cột `Tier` thì không phải tính tổng sổ điểm cho từng khách.
  - Cột chỉ được ghi ở một chỗ: `Customer.EarnPoints`, sau khi cộng điểm.

### 3.5 Câu hỏi còn mở
1. **Xóa khách chỉ có trong chiến dịch.** Q13 chỉ chặn khách có lưu trú hoặc ticket. Khách chưa ở lần nào vẫn có thể nằm trong chiến dịch "Sinh nhật trong tháng". Khi xóa khách này, khóa ngoại `campaign_targets.CustomerId` (RESTRICT) sẽ chặn ở CSDL với một lỗi khó hiểu.
   - **Đề xuất:** mở rộng Q13 thành "không xóa khách đã có lưu trú, ticket hoặc đã nằm trong chiến dịch", để Domain báo lỗi rõ ràng.
   - **Cách khác:** dùng `CASCADE` cho cột này, nhưng như vậy làm mất số liệu của báo cáo R4.

   Cần TV1 đồng ý.
2. **Quy ước kiểu dữ liệu ở mục 3.1** (tiền là `INTEGER`, ngày là `TEXT`) do TV2 đề xuất; TV1 chốt khi viết khung `schema.sql`.

## 4. Khóa ngoại nối hai phân hệ [Chung – TV1 + TV2]

### Khóa đi từ bảng TV2 (TV2 vẽ trong `erd-tv2.iuml`)

| Khóa ngoại | Trỏ tới | NULL? | ON DELETE | Ý nghĩa |
|---|---|---|---|---|
| `interactions.TicketId` | `tickets` | có | RESTRICT | tương tác sinh ra khi đóng ticket (Q12, L2) |
| `point_transactions.TicketId` | `tickets` | có | RESTRICT | dòng điểm thiện chí của ticket (Q12, L2) |
| `interactions.EmployeeId`, `bookings.CreatedById`, `reward_redemptions.ProcessedById`, `campaigns.CreatedById`, `campaign_targets.RecordedById` | `employees` | theo bảng ở mục 3.2 | RESTRICT | nhân viên đã làm (ghi người làm) |

Các khóa này chỉ tồn tại ở CSDL. Trong code, hai phân hệ vẫn chỉ giữ Id và nói chuyện qua interface; repository của TV2 không đọc bảng `tickets`, `employees`.

### Khóa đi từ bảng TV1
_[TV1] chưa điền (`tickets.CustomerId`, `tickets.RoomId`, `surveys.BookingId`)._

## 5. Chuẩn hóa 3NF [Chung – TV1]
_[Chung – TV1] chưa điền._

## 6. Thuộc tính lịch sử [TV2]
Thuộc tính lịch sử là giá trị được **chép** từ bảng khác vào lúc giao dịch xảy ra và giữ nguyên về sau, dù giá trị gốc thay đổi.

| Cột | Chép từ | Lúc | Vì sao không đọc thẳng giá trị gốc |
|---|---|---|---|
| `bookings.RatePerNight` | `room_types.PricePerNight` | đặt phòng | Giá phòng đổi theo mùa. Đọc giá hiện tại thì thành tiền, điểm đã cộng (Q9) và báo cáo chi tiêu (R3, R5) của các lần ở cũ đều sai. Biểu mẫu BM5 ghi đơn giá trên phiếu. |
| `reward_redemptions.PointsSpent` | `rewards.PointsCost` | đổi quà | Quà có thể tăng hoặc giảm giá điểm. Đọc giá hiện tại thì không giải thích được số dư của khách, và khi hủy lượt đổi sẽ hoàn sai số điểm (Q10). Code chốt trong `Reward.Redeem`. |

**Đây không phải vi phạm 3NF.** 3NF cấm thuộc tính không khóa phụ thuộc bắc cầu vào khóa chính. `RatePerNight` phụ thuộc vào chính booking ("giá đã thỏa thuận cho lần đặt này"), không phụ thuộc vào `RoomId` → `RoomTypeId` → `PricePerNight`. Bằng chứng: cùng một phòng, hai booking ở hai thời điểm có thể có hai giá khác nhau. Phụ thuộc bắc cầu thì không thể như vậy.

## 7. Đối chiếu định mức (PLAN 5.10) [Chung – TV1]
_[Chung – TV1] chưa điền._
