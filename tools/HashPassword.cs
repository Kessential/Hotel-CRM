#:project ../src/HotelCRM.Business/HotelCRM.Business.csproj

// Prints a PasswordHash and Salt pair for every password given on the command line, ready to paste
// into employees.json. It calls the application's own PasswordHasher, so the values always match
// what the login screen verifies. This file is a .NET file-based app: it is not part of the solution.
//
// Usage (from the repository root):
//   dotnet run tools/HashPassword.cs -- <password> [<password> ...]

using HotelCRM.Business.Security;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: dotnet run tools/HashPassword.cs -- <password> [<password> ...]");
    return 1;
}

foreach (string password in args)
{
    string salt = PasswordHasher.GenerateSalt();
    string hash = PasswordHasher.Hash(password, salt);

    Console.WriteLine($"// {password}");
    Console.WriteLine($"\"PasswordHash\": \"{hash}\",");
    Console.WriteLine($"\"Salt\": \"{salt}\",");
    Console.WriteLine();
}

return 0;
