using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tickest.Domain.Helpers;

public class EncryptionHelper
{
    /// <summary>
    /// Cria uma chave de sal (salt) com o comprimento especificado.
    /// </summary>
    public static string CreateSaltKey(int length)
    {
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            var buffer = new byte[length];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer); // Convertendo o salt para Base64
        }
    }

    /// <summary>
    /// Cria um hash de senha utilizando o valor da senha e o salt fornecido.
    /// </summary>
    public static string CreatePasswordHashWithSalt(string password, string salt)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var combined = salt + password; // Combine o salt com a senha
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(hashBytes); // Retorna o hash como Base64
        }
    }

    public static bool CheckPasswod(string password, string salt, string passwordHashed)
    {
        var pass = EncryptionHelper.CreatePasswordHashWithSalt(password, salt);

        return pass.Equals(passwordHashed);
    }
}
