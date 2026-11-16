# Tiến độ dự án Hotel CRM

Mỗi phiên đọc file này đầu tiên và cập nhật trước khi kết thúc. Ghi ngắn, đúng sự thật: việc chưa kiểm tra thì ghi là chưa kiểm tra. File này chỉ ghi trạng thái; thiết kế và lộ trình nằm ở `docs/PLAN.md`, cách làm từng giai đoạn nằm ở `docs/tasks/`.

## Trạng thái các giai đoạn

| GĐ | Hạn | Trạng thái | File task |
|---|---|---|---|
| 0 Chuẩn bị | 16/11 | Xong, đã commit và push trên `develop` | — |
| 1 Khung | 17/11 | Chưa bắt đầu | `docs/tasks/stage-1-skeleton.md` |
| 2 Suất A | 20/11 | Chưa bắt đầu | viết ở cuối GĐ 1 |
| 3 Suất B | 23/11 | Chưa bắt đầu | viết ở cuối GĐ 1 |
| 4 Ghép nối | 25/11 | Chưa bắt đầu | viết ở cuối GĐ 1 |
| 5 Tài liệu kỹ thuật | 27/11 | Chưa bắt đầu | viết ở cuối GĐ 4 |
| 6 Báo cáo & nộp | 29/11 | Chưa bắt đầu | viết ở cuối GĐ 5 |

## Việc tiếp theo
Mở phiên mới bằng model mạnh (Fable 5.2 hoặc Opus 5.5) với câu: "Đọc `CLAUDE.md`, `docs/PROGRESS.md` và `docs/tasks/stage-1-skeleton.md` rồi làm theo."

## Cách commit của suất B
**Nhận patch** (mặc định). Thành viên B chưa xác nhận; hạn 17/11. Nếu họ chọn tự code, sửa dòng này thành "Tự code".

## Trạng thái Git
- `main`: 1 commit ("first commit"), chỉ có `README.md`.
- `develop`: đã tạo từ `main` ở GĐ 0, có thêm 1 commit chứa `CLAUDE.md`, `.gitignore`, `.claude/`, `docs/`. Đã push lên `origin/develop` ngày 16/11.
- Chưa có nhánh `local/b-work`.
- `docs/TEAM-BRIEF.md` đã gửi cho thành viên B và nằm trong `.gitignore`; không commit, không cập nhật.

## Việc đang dở
(chưa có)

## Quyết định lệch khỏi plan
(chưa có)

## Câu hỏi và việc đang chờ người
| Việc | Cần trước | Kết quả |
|---|---|---|
| Chủ repo báo cho thành viên B: Strategy chuyển sang suất B, hạn chọn cách commit là 17/11 (khác bản tóm tắt đã gửi) | 17/11 | |
| Tên tài khoản GitHub của thành viên B | 17/11 | |
| Thành viên B chọn cách commit: nhận patch hay tự code | 17/11 | |
| Mẫu báo cáo LaTeX | 26/11 | |
| Máy có Java (xuất ảnh PlantUML) và bản TeX không | 26/11 | |

## Nhật ký
- 15/11/2026 — GĐ 0: tạo `CLAUDE.md`, `.gitignore`, `.claude/settings.json`, 5 file `.claude/rules/`, file này và `docs/tasks/stage-1-skeleton.md`. Các file rules viết khi chưa có code, phải rà lại ở cuối GĐ 1.
- 15/11/2026 — Rà soát và sửa plan + instruction theo 34 điểm. Các thay đổi chính: Strategy sang suất B, Repository + Singleton và toàn bộ `Data/` thành phần chung; quy tắc đặt theo "service tra cứu, Domain quyết định"; ticket không dùng `MembershipTier` mà nhận `CustomerSlaProfile`; GĐ 1 viết luôn `Program.cs` và vỏ 9 Form; ma trận phân quyền của quy tắc 11; commit không kèm `Co-Authored-By`. Chưa có dòng code nào; quy tắc `PowerShell(...)` trong `settings.json` chưa được chạy thử.
- 16/11/2026 — Tạo nhánh `develop` từ `main` và commit toàn bộ file GĐ 0 trên `develop` (commit số 1 trong danh sách đề xuất của `stage-1-skeleton.md`). GĐ 1 không cần tạo lại nhánh, chỉ cần chuyển sang `develop`.
