namespace Tickest.Domain.Contracts.Services
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Gera um salt criptograficamente seguro com o tamanho especificado.
        /// </summary>
        /// <param name="tamanho">Tamanho do salt em bytes. O valor padrão é 16.</param>
        /// <returns>Salt codificado em Base64.</returns>
        string GenerateSalt(int tamanho = 16);

        /// <summary>
        /// Gera um hash criptografado da senha usando PBKDF2 com o salt fornecido.
        /// </summary>
        /// <param name="password">A senha em texto claro que será criptografada.</param>
        /// <param name="salt">Salt em Base64 usado para fortalecer o hash da senha.</param>
        /// <param name="iterations">Número de iterações do algoritmo PBKDF2. O valor padrão é 10.000.</param>
        /// <param name="hashSize">Tamanho desejado do hash em bytes. O valor padrão é 32.</param>
        /// <returns>Hash da senha codificado em Base64.</returns>
        string HashPassword(string password, string salt, int iterations = 10000, int hashSize = 32);
    }
}
