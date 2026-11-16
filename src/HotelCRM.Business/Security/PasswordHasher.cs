using System.Security.Cryptography;

namespace HotelCRM.Business.Security;

/// <summary>
/// Hashes and verifies passwords with PBKDF2-SHA256: 100,000 iterations, a random 16-byte salt per
/// account and a 32-byte hash, both stored as Base64. Passwords are never stored or logged.
/// </summary>
public static class PasswordHasher
{
    /// <summary>Number of PBKDF2 iterations.</summary>
    public const int Iterations = 100_000;

    /// <summary>Salt length in bytes.</summary>
    public const int SaltSize = 16;

    /// <summary>Hash length in bytes.</summary>
    public const int HashSize = 32;

    /// <summary>Creates a new random salt, Base64 encoded. Every account gets its own.</summary>
    public static string GenerateSalt()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(SaltSize));
    }

    /// <summary>Computes the Base64 hash of a password with the given Base64 salt.</summary>
    /// <exception cref="FormatException">The salt is not valid Base64.</exception>
    public static string Hash(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA256, HashSize);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Checks a password against a stored hash and salt. The comparison takes the same time whether
    /// or not the hashes match. Returns false when the stored values are not valid Base64.
    /// </summary>
    public static bool Verify(string password, string passwordHash, string salt)
    {
        byte[] expected;
        byte[] actual;
        try
        {
            expected = Convert.FromBase64String(passwordHash);
            actual = Convert.FromBase64String(Hash(password, salt));
        }
        catch (FormatException)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
