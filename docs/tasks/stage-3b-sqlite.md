# GĐ 3b — Chuyển lưu trữ từ JSON sang SQLite

Hạn: 22/11/2026 (Mốc 2). Model: Sonnet 5.5. Kế hoạch chốt ngày 17–18/11/2026 qua ba vòng hỏi đáp với chủ repo; file này ghi lại kết quả. Lúc viết file này chưa có dòng code nào.

Làm theo `CLAUDE.md`: dừng sau mỗi module (A1, A2, B1, B2, A3), tóm tắt thiết kế và điểm cần nhớ khi vấn đáp, chờ duyệt, không tự commit.

## Phê duyệt đã có
Chủ repo đồng ý ngày 18/11/2026, chỉ trong phạm vi việc chuyển này:
- Thêm gói NuGet `Microsoft.Data.Sqlite` vào `HotelCRM.Data` (bản ổn định mới nhất của nhánh 10.0; ghi số phiên bản vào `docs/PROGRESS.md`). Không thêm gói nào khác: không Dapper, không EF Core.
- Sửa phần chung sau GĐ 1: `src/HotelCRM.Data/**`, `Program.cs`, `appsettings.json`, `HotelCRM.WinForms.csproj`, `HotelCRM.Data.csproj`, `tools/HashPassword.cs`, `.gitignore`, comment trong `Domain/Interfaces/IRepository.cs`, `CLAUDE.md`, `docs/PLAN.md`, `.claude/rules/data.md`, `.claude/rules/tests.md`.
- Mỗi suất sửa dòng repository của mình trong `Program.cs` (bảng ở mục "Chia việc").
- Bỏ JSON làm kho lưu ở Module A3: xóa `JsonRepository` và `Data/*.json`.

`CLAUDE.md` và `.claude/rules/data.md` hiện vẫn cấm những việc trên. **Bước đầu tiên của Module A1 là sửa hai file đó** cho khớp phê duyệt này. Ngoài phạm vi trên, mọi luật của `CLAUDE.md` giữ nguyên.

## Quyết định đã chốt

| # | Quyết định |
|---|---|
| 1 | Lý do: yêu cầu/điểm của môn học và để học. Không cần nhiều máy dùng chung một CSDL |
| 2 | SQLite, một file `hotel.db` |
| 3 | Repository giữ cách hoạt động cũ: nạp hết một lần lúc khởi động, cùng Id luôn trả cùng một đối tượng; `Save`/`Delete` chỉ ghi một dòng |
| 4 | ADO.NET thuần qua `Microsoft.Data.Sqlite`, SQL viết tay có tham số |
| 5 | Bỏ hẳn JSON làm kho lưu, không có công tắc chọn kho. `appsettings.json` vẫn là JSON vì là file cấu hình |
| 6 | Chốt GĐ 3 trên JSON trước, chuyển xong trước GĐ 4. Chia việc theo phân hệ; phần của B đi qua patch |
| 7 | Lịch sử tương tác là bảng con. Ticket và nhân viên mỗi loại một bảng, có cột `Type`/`Role` phân biệt lớp con. Không đặt khóa ngoại. Id = Id lớn nhất + 1, do repository cấp |
| 8 | Dữ liệu mẫu là 8 file `.sql` theo bảng, đặt trong `Data/` cạnh `.exe`. Câu `CREATE TABLE` nằm trong lớp bảng C#. Không nộp sẵn `hotel.db`; muốn làm mới thì xóa `hotel.db`. `HashPassword` in thêm dạng dùng cho SQL |
| 9 | Mỗi lần `Save` là một transaction riêng; không gom chung với observer |
| 10 | Thêm 4 test ghi rồi nạp lại (A 2, B 2). Tổng 26 test |
| 11 | Sửa tài liệu ngay trong từng module |
| 12 | Hai mốc 20/11 và 22/11; không đạt Mốc 2 thì quay về JSON; B chưa commit kịp thì chờ hết 23/11 rồi tính như không đạt |

## Thiết kế

### Cách chạy
- **Khởi động:** `Program.cs` → `AppConfig` → `SqliteDatabase.EnsureCreated(...)`:
  - Chưa có `hotel.db`: tạo file, chạy `CREATE TABLE` của các lớp bảng, chạy mọi file `*.sql` trong `DataFolder` theo thứ tự tên, tất cả trong một transaction.
  - Đã có `hotel.db`: chỉ mở, không chạy seed.
- **Nạp:** mỗi `SqliteRepository` đọc cả bảng một lần trong constructor, đi qua Record → Mapper → thực thể, như `JsonRepository` đọc file.
- **Ghi:** `Save` là UPSERT một dòng, `Delete` xóa một dòng, mỗi lần một transaction. Danh sách trong bộ nhớ được cập nhật giống `JsonRepository`.
- Record, Mapper và `IRepository<T>` giữ nguyên chữ ký.

### Lớp mới (đề xuất; Module A1 chốt chữ ký, lệch thì ghi vào bảng lệch plan)
Thư mục `src/HotelCRM.Data/Sqlite/`:
- `ISqlTable` (không generic): `string TableName { get; }`, `void CreateTable(SqliteTransaction transaction)`. `SqliteDatabase` dùng nó để tạo mọi bảng.
- `ISqlTable<TRecord> : ISqlTable`: `IReadOnlyList<TRecord> ReadAll(SqliteConnection connection)`, `void Upsert(SqliteTransaction transaction, TRecord record)`, `void Delete(SqliteTransaction transaction, int id)`.
- `SqliteDatabase`: constructor `(string databasePath, string seedFolder)`; `SqliteConnection Open()`; `void EnsureCreated(IReadOnlyList<ISqlTable> tables)`.
- `SqliteRepository<TEntity, TRecord> : IRepository<TEntity>`: constructor `(SqliteDatabase database, ISqlTable<TRecord> table, IMapper<TEntity, TRecord> mapper)`.
- `Tables/`: `DepartmentTable`, `EmployeeTable`, `TicketTable` (A); `RoomTypeTable`, `RoomTable`, `BookingTable`, `CustomerTable`, `SurveyTable` (B). Mỗi lớp chứa câu `CREATE TABLE` và SQL đọc/ghi của bảng mình. Giá trị luôn đi qua tham số, không ghép chuỗi.

`AppConfig`: thêm khóa `DatabaseFile` (mặc định `hotel.db`) và thuộc tính `DatabasePath` = `DataFolder` + `DatabaseFile`. `DataFolder` giữ nguyên, giờ là thư mục chứa file seed và `hotel.db`.

### Bảng
Tên bảng theo tên file JSON cũ; tên cột trùng tên thuộc tính của Record. `Id INTEGER PRIMARY KEY`, không AUTOINCREMENT, không khóa ngoại.

| Bảng | Cột | Ai |
|---|---|---|
| `departments` | Id, Code, Name | A |
| `employees` | Id, Role, FullName, Phone, Email, Username, PasswordHash, Salt, DepartmentId | A |
| `tickets` | Id, Type, CustomerId, Title, Description, Priority, CreatedAt, DueAt, UpdatedAt, State, AssignedAgentId, ResolvedAt, ResolutionNote, ClosedAt, RoomId | A |
| `roomtypes` | Id, Name, PricePerNight, Capacity | B |
| `rooms` | Id, Number, Floor, RoomTypeId | B |
| `bookings` | Id, CustomerId, RoomId, CheckInDate, CheckOutDate, Status | B |
| `customers` | Id, FullName, Phone, Email, IdentityNumber, Nationality, Notes, Tier, Points | B |
| `interactions` | CustomerId, OccurredAt, Channel, Summary, TicketId. Không có Id riêng (đúng PLAN: `Interaction` không có Id); đọc theo `ORDER BY CustomerId, rowid` để giữ thứ tự ghi | B |
| `surveys` | Id, TicketId, Score, Comment, SubmittedAt | B |

Kiểu cột:
- Số nguyên: `INTEGER`.
- Chuỗi, enum, tên trạng thái/hạng/loại: `TEXT`, lưu theo tên.
- Thời gian: `TEXT` ISO 8601. Định dạng chốt ở A1, ghi vào `rules/data.md`; seed viết đúng định dạng đó.
- `PricePerNight` (`decimal`): `TEXT`, để không mất phần lẻ.
- Trường nullable của Record: cột cho phép `NULL`.

### Dữ liệu mẫu (seed)
- 8 file trong `src/HotelCRM.WinForms/Data/`: `01-departments.sql`, `02-employees.sql`, `03-roomtypes.sql`, `04-rooms.sql`, `05-customers.sql` (gồm cả `interactions`), `06-bookings.sql`, `07-tickets.sql`, `08-surveys.sql`.
- Mỗi file chỉ `INSERT` vào bảng của mình. Comment trong file viết tiếng Anh. File chưa có dữ liệu thì chỉ có một dòng comment.
- Được chép ra thư mục output cạnh `.exe` và nằm ngoài file `.exe` khi publish single-file, như `Data\*.json` bây giờ.
- App ghi `hotel.db` ở thư mục output, không bao giờ ở `src/`. Không commit, không nộp kèm; `.gitignore` thêm `*.db`.
- Đổi bảng hoặc seed thì xóa `hotel.db` trong thư mục output (ví dụ `src/HotelCRM.WinForms/bin/Debug/net10.0-windows/Data/hotel.db`) để app tạo lại. Không có nút Reset.
- `tools/HashPassword.cs` in thêm một dòng dạng `'hash', 'salt'` để dán vào `02-employees.sql`.

### Lỗi
- **Seed:** file lỗi thì rollback, xóa `hotel.db` dở, ném `InvalidDataException` nêu tên file seed. Thiếu thư mục seed thì báo lỗi có tên thư mục.
- **Nạp bảng:** thiếu bảng, tên trạng thái/hạng/loại lạ, hoặc tham chiếu tới Id không có thì ném `InvalidDataException` nêu tên bảng. Riêng thiếu bảng, thông báo gợi ý xóa `hotel.db` để tạo lại. `ErrorHandler.ReportStartupFailure` hiện lý do lúc khởi động.
- **Ghi lúc đang chạy:** không bọc lỗi. `ErrorHandler` coi là lỗi ngoài dự kiến và ghi `logs/error.log`, giống lỗi ghi file hiện nay.
- **Kiểm tra ở A1:** connection pooling của `Microsoft.Data.Sqlite` có giữ khóa file `hotel.db` trên Windows sau khi đóng kết nối không. Nếu có thì tắt pooling hoặc xóa pool trước khi xóa file dở, và trước khi test dọn file tạm.

## Chia việc

| Module | Ai | Nhánh / nơi làm | Lớp bảng | Seed | Repository trong `Program.cs` |
|---|---|---|---|---|---|
| A1 Hạ tầng | A (phần chung) | `feature/sql-storage`, thư mục chính | khung 8 lớp | — | — |
| A2 Nhân sự + ticket | A | `feature/sql-storage` | `DepartmentTable`, `EmployeeTable`, `TicketTable` | 01, 02, 07 | `departments`, `employees`, `tickets` |
| B1 Phòng + booking | B (patch) | `local/b-work`, worktree `.claude/worktrees/b-work` | `RoomTypeTable`, `RoomTable`, `BookingTable` | 03, 04, 06 | `roomTypes`, `rooms`, `bookings` |
| B2 Khách + khảo sát | B (patch) | `local/b-work`, worktree | `CustomerTable`, `SurveyTable` | 05, 08 | `customers`, `surveys` |
| A3 Bỏ JSON | A | `develop` | — | — | — |

Số commit: A 3, B 2. Phần của B có bảng con `interactions`, là nội dung tốt để B trình bày khi vấn đáp.

## Nhánh và thứ tự
Làm tuần tự **A1 → A2 → B1 → B2 → A3**. B1 chỉ bắt đầu khi A2 đã vào `local/b-work`, nên `Program.cs` không bao giờ bị hai bên sửa cùng lúc.

1. **A1, A2:** nhánh `feature/sql-storage` tạo từ `develop`, làm ở thư mục chính.
2. **Sau A2 và Mốc 1:**
   - merge `feature/sql-storage` vào `develop` bằng `--no-ff`;
   - push `develop` (cần chủ repo đồng ý);
   - trong worktree, merge `develop` vào `local/b-work`.
3. **B1, B2:** worktree `.claude/worktrees/b-work`, nhánh `local/b-work`, không bao giờ push.
   - Mỗi module một commit, xuất patch theo mục "Xuất patch" của `docs/tasks/stage-3-customer.md`.
   - Số patch chạy tiếp số đang có: dự kiến 10 và 11, nếu chốt GĐ 3 không sinh thêm patch sửa lỗi.
4. **Thành viên B, trên `feature/b-customer`:**
   - áp patch 02–09 (GĐ 3);
   - `git merge origin/develop` để lấy A1, A2;
   - áp patch của B1, B2;
   - build;
   - push.

   Mục của patch B1 trong `handoff/README.txt` phải ghi rõ bước merge này.
5. **A3:** trên `develop`, sau khi đã merge `feature/b-customer` (có B1, B2) và đạt Mốc 2.

## Đọc trước
- `.claude/rules/data.md`, `tests.md`.
- `src/HotelCRM.Data/**`, `src/HotelCRM.WinForms/Program.cs`, `Domain/Interfaces/IRepository.cs`.
- Module B1, B2: thêm mục "Xuất patch" của `docs/tasks/stage-3-customer.md` và `handoff/README.txt`.

## File cấm đụng (mọi module)
- Logic trong Domain và Business: chỉ được sửa những comment mà module nêu tên.
- Mọi Form, `ErrorHandler`, `MainForm`, `LoginForm`.
- Chữ ký của `IRepository<T>` và `IMapper`.
- Mapper, `InMemoryRepository`.
- Record: A3 chỉ sửa comment.
- Mọi test và lớp giả có sẵn.

Thấy cần đổi thì dừng, ghi lý do vào `docs/PROGRESS.md` và hỏi.

## Các module

### Module A1 — Hạ tầng SQLite (A, phần chung)
1. **Sửa luật trước:**
   - `CLAUDE.md`: dòng giới thiệu "lưu JSON" → "lưu SQLite"; ngoại lệ NuGet cho `Microsoft.Data.Sqlite`; thêm file mới vào danh sách của từng suất theo bảng "Chia việc"; ngoại lệ phần chung cho GĐ 3b; lệnh `HashPassword` dùng cho `02-employees.sql`.
   - `.claude/rules/data.md`: viết lại theo SQLite.
   - `.claude/rules/tests.md`: cách viết test bảng.
2. **Sửa `docs/PLAN.md`:**
   - câu mở đầu và quyết định "Lưu JSON" trong "Quyết định đã chốt";
   - minh chứng tiêu chí 18 (`Data/*.sql`, 26 test) và dòng "Kiến trúc đóng gói";
   - cây thư mục, đoạn "Lưu JSON", dòng Repository + Singleton trong bảng Design Pattern;
   - mục 6 "Lưu trữ", mục "Kiểm thử" (26 test), mục "Dữ liệu mẫu liên thông";
   - lộ trình: thêm GĐ 3b;
   - lệnh publish (nếu đổi).
3. **Gói:** thêm `Microsoft.Data.Sqlite` vào `HotelCRM.Data.csproj`. `Tests` và `WinForms` nhận gói qua tham chiếu project; build để xác nhận.
4. **Lớp mới:** `ISqlTable`, `ISqlTable<TRecord>`, `SqliteDatabase`, `SqliteRepository`. Khung 8 lớp bảng: chữ ký và chú thích XML đầy đủ, thân hàm `throw new NotImplementedException();`.
5. **Cấu hình và file dự án:**
   - `AppConfig` (`DatabaseFile`, `DatabasePath`; comment bỏ chữ "JSON data files"), `appsettings.json`;
   - `HotelCRM.WinForms.csproj`: chép `Data\*.sql`, `ExcludeFromSingleFile`;
   - `.gitignore`: `*.db`;
   - comment của `IRepository.cs`.
6. **Kiểm tra connection pooling** (mục "Lỗi").

Không sửa `Program.cs`: sau A1 app vẫn chạy hoàn toàn trên JSON.

**Xong khi:** build 0 lỗi 0 warning; số test xanh/đỏ giữ nguyên như trước A1; app khởi động và đăng nhập được như cũ.

### Module A2 — Bảng nhân sự và ticket (A)
1. **Lớp bảng:** điền `DepartmentTable`, `EmployeeTable`, `TicketTable`.
2. **Seed:** `01-departments.sql`, `02-employees.sql` chuyển từ `departments.json`, `employees.json` hiện có, giữ nguyên chuỗi băm đang có. `07-tickets.sql`: `tickets.json` đang rỗng nên file chỉ có comment.
3. **`Program.cs`:**
   - tạo `SqliteDatabase` từ `config.DatabasePath` và `config.DataFolder`;
   - gọi `EnsureCreated` với 3 bảng;
   - `departments`, `employees`, `tickets` dùng `SqliteRepository`; 5 repository còn lại vẫn là `JsonRepository`;
   - giữ đúng thứ tự nạp hiện có.
4. **`tools/HashPassword.cs`:** in thêm dòng dạng SQL.
5. **Comment:** sửa chỗ nhắc `tickets.json` trong `TicketState.cs` và `SupportTicket.cs`.
6. **Test:** `tests/HotelCRM.Tests/Tickets/TicketTableTests.cs`, 2 test. Mỗi test dùng một file CSDL tạm riêng và xóa khi xong; không dùng seed của app.
   - `Save_MaintenanceTicketWithAgent_ReloadsSameValues`:
     - ticket bảo trì ở trạng thái Resolved, có Agent và `RoomId`;
     - `ResolvedAt`, `ResolutionNote` có giá trị, `ClosedAt` là `null`;
     - nạp lại bằng repository mới trên cùng file thì mọi trường bằng nhau, `State.Name` là `Resolved`, `Priority` đúng, `AssignedAgent` là cùng đối tượng trong repository nhân viên.
   - `Delete_SavedTicket_NotLoadedAgain`: lưu 2 ticket, xóa 1, nạp lại thì còn 1 và `NextId()` đúng.
7. **Mốc 1** (mục "Mốc và phương án lui").

**Xong khi:**
- build sạch;
- 2 test mới xanh, test cũ giữ nguyên trạng thái;
- `EnsureCreated` chạy được file seed chỉ có comment (như `07-tickets.sql`) mà không báo lỗi;
- xóa `hotel.db` rồi chạy app: `hotel.db` được tạo, đăng nhập được cả Manager và Agent;
- Mốc 1 đạt.

### Module B1 — Bảng phòng và booking (B, patch)
**Bắt đầu khi:** `local/b-work` đã merge `develop` có A1, A2; build sạch; đã xóa `hotel.db` cũ trong thư mục output của worktree.

1. **Lớp bảng:** điền `RoomTypeTable`, `RoomTable`, `BookingTable`.
2. **Seed:**
   - `03-roomtypes.sql`, `04-rooms.sql` chuyển từ `roomtypes.json`, `rooms.json` đang có trên `local/b-work` (2 loại phòng, 4 phòng);
   - `06-bookings.sql` chỉ có comment.
3. **`Program.cs`:** thêm 3 bảng vào `EnsureCreated`; `roomTypes`, `rooms`, `bookings` dùng `SqliteRepository`.
4. **Test:** `tests/HotelCRM.Tests/Customers/BookingTableTests.cs`, 1 test, cùng cách dùng file tạm như A2.
   - `Save_BookingWithRoom_ReloadsSameValues`: loại phòng có giá lẻ (ví dụ `120.50`), một phòng, một booking `CheckedIn`; khách lấy từ `InMemoryRepository<Customer>`.
   - Nạp lại thì giá đúng tới phần lẻ, ngày nhận/trả và trạng thái đúng, `Booking.Room` là cùng đối tượng trong repository phòng.

**Xong khi:**
- build sạch, test mới xanh;
- chạy app: đặt phòng, check-in, thoát, mở lại thì booking còn nguyên;
- đã xuất patch kèm `.msg.txt` và cập nhật `handoff/README.txt` (ghi bước `git merge origin/develop`).

### Module B2 — Bảng khách hàng và khảo sát (B, patch)
1. **`CustomerTable`:**
   - tạo 2 bảng `customers` và `interactions`;
   - `Upsert` ghi khách, rồi xóa và ghi lại toàn bộ dòng tương tác của khách đó, trong cùng transaction;
   - `Delete` xóa khách và các dòng tương tác của khách;
   - `ReadAll` gom tương tác theo `CustomerId` và giữ thứ tự ghi.
2. **`SurveyTable`.**
3. **Seed:** `05-customers.sql`, `08-surveys.sql`; dữ liệu JSON đang rỗng nên chỉ có comment.
4. **`Program.cs`:** thêm 2 bảng vào `EnsureCreated`; `customers`, `surveys` dùng `SqliteRepository`. Sau bước này cả 8 repository chạy trên SQLite.
5. **Comment:** sửa chỗ nhắc `customers.json` trong `MembershipTier.cs`.
6. **Test:** `tests/HotelCRM.Tests/Customers/CustomerTableTests.cs`, 1 test.
   - `Save_CustomerTwice_KeepsInteractionsInOrder`: khách hạng Gold có điểm, 2 tương tác (một có `TicketId`, một `null`); lưu; thêm tương tác thứ 3; lưu lần nữa.
   - Nạp lại thì có đúng 3 tương tác, đúng thứ tự, `Channel` và `TicketId` đúng, hạng và điểm đúng.

**Xong khi:**
- build sạch, test mới xanh;
- chạy app: đăng ký khách, đặt phòng, check-in, check-out (điểm được cộng, lịch sử có dòng mới); thoát, mở lại thì điểm và lịch sử còn nguyên;
- đã xuất patch.

### Module A3 — Bỏ JSON (A, sau Mốc 2)
1. **Xóa:** `JsonRepository.cs`, `src/HotelCRM.WinForms/Data/*.json`, mục `Data\*.json` trong `HotelCRM.WinForms.csproj`.
2. **Comment:** sửa comment của các Record (từ "một mục của x.json" thành "một dòng của bảng x") và của `IMapper`.
3. **`README.md`:**
   - dòng giới thiệu ("SQLite storage");
   - chỗ ghi tài khoản mẫu: thành `02-employees.sql`;
   - cách làm mới dữ liệu: xóa `Data/hotel.db` cạnh `.exe`.
4. **`docs/tasks/stage-4-integration.md`:**
   - Module 7 dùng `02-employees.sql`;
   - bảng mẫu dữ liệu đổi thành mẫu câu `INSERT`;
   - bước kiểm tra publish: `Data/` chứa `*.sql`, `hotel.db` tự sinh;
   - danh sách commit: mỗi file seed một commit.
5. **`docs/PROGRESS.md`.**

**Xong khi:** đạt "Điều kiện xong" ở cuối file.

## Mốc và phương án lui

| Ngày | Việc |
|---|---|
| 18/11 | Chốt GĐ 3 trên JSON (kiểm tra tay, Designer, `/code-review`). Publish thử bản JSON hiện tại làm mốc so sánh. Gửi patch 02–09 cho B. Cả ba việc phải xong trước A1 |
| 19/11 | A1 |
| 20/11 | A2 + **Mốc 1**; merge vào `develop`, push (cần đồng ý), merge sang `local/b-work` |
| 20–21/11 | B1, B2; B áp patch và push |
| 22/11 | **Mốc 2** trong worktree `local/b-work`; đạt và B đã push thì merge `feature/b-customer` vào `develop` rồi làm A3 |
| 23/11 | GĐ 4 bắt đầu trên `local/b-work` (đã có SQLite). Hết 23/11 mà B vẫn chưa push B1, B2 thì quay về JSON (quyết định 12), kể cả trên `local/b-work`. Sau đó không quay lại JSON nữa; lỗi SQL sửa tại chỗ |

**Mốc 2 chạy ở đâu:** sau B2, `local/b-work` đã có đủ A1, A2, B1, B2. Vì vậy phần kiểm tra kỹ thuật (build, test, kiểm tra tay, publish) chạy trong worktree ngay ngày 22/11, không phải chờ B push. Chỉ có hai việc chờ B: merge `feature/b-customer` vào `develop` và Module A3.

**Mốc 1 (cuối A2):**
1. Chạy lệnh publish trong `CLAUDE.md`, chép thư mục publish sang chỗ khác, chạy `.exe`. Đạt khi `Data/hotel.db` được tạo và đăng nhập được.
2. Nếu thư viện native của SQLite (`e_sqlite3.dll`) không gói được vào `.exe`, thử thêm `IncludeNativeLibrariesForSelfExtract=true`. Được thì cập nhật lệnh publish trong `CLAUDE.md` và `PLAN.md`.
3. Vẫn không được thì dừng, không merge, báo chủ repo. Phương án mặc định là bỏ nhánh `feature/sql-storage` và giữ JSON.

**Mốc 2 (22/11): người dùng kiểm tra bằng tay**
1. Xóa `hotel.db`, chạy app: dữ liệu được tạo lại từ seed, đăng nhập được 2 vai trò.
2. Chạy lại các bước kiểm tra bằng tay của GĐ 2 (`stage-2-ticket.md`) và GĐ 3 (`stage-3-customer.md`) trên SQLite.
3. Thoát, mở lại: mọi dữ liệu vừa nhập còn nguyên.
4. Publish, chép thư mục publish sang chỗ khác, chạy được.

Kèm theo: build 0 lỗi 0 warning; test đúng như "Điều kiện xong".

**Không đạt Mốc 2, hoặc hết 23/11 mà B chưa push B1, B2: quay về JSON** (A3 chưa làm nên JSON vẫn còn):
1. Trên `develop`, revert theo thứ tự ngược:
   - nếu B1, B2 đã vào `develop`: `git revert --no-edit <commit B2> <commit B1>` (hai commit thường);
   - rồi `git revert -m 1 --no-edit <merge commit của feature/sql-storage>`.
2. Build, test, chạy app trên JSON. Merge `develop` vào `local/b-work`; báo B merge `origin/develop`. Ghi vào `docs/PROGRESS.md`.
3. Lưu ý: muốn đưa SQLite trở lại sau này thì phải revert chính các commit revert; merge lại nhánh cũ không có tác dụng.

## Đề xuất commit (chỉ commit khi người dùng đồng ý)
Mỗi module một commit, message tiếng Anh, không kèm `Co-Authored-By`:
- A1 — "Add the SQLite storage infrastructure"
- A2 — "Store departments, employees and tickets in SQLite"
- B1 (patch) — "Store room types, rooms and bookings in SQLite"
- B2 (patch) — "Store customers, interactions and surveys in SQLite"
- A3 — "Remove the JSON storage"

## Điều kiện xong
- `dotnet build` 0 lỗi 0 warning.
- 26 test: 24 xanh, 2 `SurveyServiceTests` đỏ vì `NotImplementedException` (để GĐ 4). Không test có sẵn nào bị sửa.
- Mốc 2 đạt; A3 xong. Trong `src/` không còn `JsonRepository` và không còn file dữ liệu `.json` (trừ `appsettings.json`).
- `CLAUDE.md`, `docs/PLAN.md`, `.claude/rules/`, `README.md`, `stage-4-integration.md`, `docs/PROGRESS.md` khớp với code.

## Rủi ro đã biết
- **Publish single-file:** chưa ai chạy thử lệnh publish, và SQLite cần thư viện native. Mốc 1 kiểm tra.
- **Khóa file trên Windows:** connection pooling có thể giữ khóa `hotel.db`. A1 kiểm tra.
- **`hotel.db` cũ:** sau mỗi lần đổi bảng hoặc seed mà quên xóa `hotel.db`, app sẽ báo thiếu bảng hoặc chạy trên dữ liệu cũ.
- **Hàng đợi patch của B** dài thêm (02 → 11). B phải áp đúng thứ tự và merge `origin/develop` trước patch B1.
- **GĐ 4** vẫn hạn 25/11; việc sinh dữ liệu mẫu đổi từ JSON sang `.sql`.
