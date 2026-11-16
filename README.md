# Hotel-CRM

Customer care management for a hotel: support tickets, customers, bookings, satisfaction surveys and reports.
C# / .NET 10, WinForms, JSON storage, xUnit tests.

## Run

```
dotnet build HotelCRM.sln
dotnet test HotelCRM.sln
dotnet run --project src/HotelCRM.WinForms
```

## Sample accounts

These accounts exist only in the sample data (`src/HotelCRM.WinForms/Data/employees.json`).

| Username | Password | Role | Department |
|---|---|---|---|
| `manager1` | `Manager@123` | Manager | Front Office |
| `agent1` | `Agent@123` | Agent | Guest Relations |
| `agent2` | `Agent@123` | Agent | Engineering |

Passwords are stored as salted PBKDF2-SHA256 hashes. To create the hash for a new account, run from the repository root:

```
dotnet run tools/HashPassword.cs -- <password>
```
