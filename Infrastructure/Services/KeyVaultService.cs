using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;
using Tickest.Domain.Interfaces;

namespace Tickest.Infrastructure.Services;

public class KeyVaultService : IKeyVaultService
{
    private readonly KeyClient _keyClient;

    public KeyVaultService(IConfiguration configuration)
    {
        _keyClient = new KeyClient(new Uri(configuration["AzureKeyVaultURI"]!), new DefaultAzureCredential());
    }

    public async Task<string> EncryptAsync(string key, string content)
    {
        try
        {
            var keyResponse = await _keyClient.GetKeyAsync(key);
            var cryptoClient = new CryptographyClient(keyResponse.Value.Id, new DefaultAzureCredential());
            var encryptResult = await cryptoClient.EncryptAsync(EncryptionAlgorithm.RsaOaep, Encoding.UTF8.GetBytes(content));
            return Convert.ToBase64String(encryptResult.Ciphertext);
        }
        catch (RequestFailedException ex)
        {
            throw new RequestFailedException($"Erro ao criptografar dados no Key Vault: {ex.Message}", ex);
        }

    }

    public async Task<string> DecryptAsync(string key, string content)
    {
        var keyResponse = await _keyClient.GetKeyAsync(key);
        var cryptoClient = new CryptographyClient(keyResponse.Value.Id, new DefaultAzureCredential());
        var decryptResult = await cryptoClient.DecryptAsync(EncryptionAlgorithm.RsaOaep, Convert.FromBase64String(content));
        return Encoding.UTF8.GetString(decryptResult.Plaintext);
    }
}
