using System.Security.Cryptography;
using System.Text;

namespace Tickest.Domain.Helpers;

public class EncryptionHelper
{
    public static string CreateSaltKey(int length)
    {
        var buffer = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(buffer);
    }

    public static string CreatePasswordHashWithSalt(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = salt + password;
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(hashBytes);
    }
}
