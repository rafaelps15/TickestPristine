namespace Tickest.Application.Abstractions.Authentication
{
    /// <summary>
    /// Define métodos para hashing e verificação de senhas.
    /// Implementações desta interface devem fornecer
    /// uma forma segura de gerar hash de senhas e validar senhas fornecidas
    /// comparando-as com o hash armazenado.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Gera um hash seguro a partir de uma senha em texto plano.
        /// </summary>
        /// <param name="password">A senha em texto plano.</param>
        /// <returns>Uma string representando o hash seguro da senha.</returns>
        string Hash(string password);

        /// <summary>
        /// Verifica se a senha fornecida corresponde ao hash armazenado.
        /// </summary>
        /// <param name="password">A senha em texto plano a ser verificada.</param>
        /// <param name="passwordHash">O hash da senha para comparação.</param>
        /// <returns>True se a senha corresponder ao hash; caso contrário, false.</returns>
        bool Verify(string password, string passwordHash);
    }
}
