using System.Security.Cryptography;
using System.Text;
using HeadLessBlog.Application.Common.Interfaces;

namespace HeadLessBlog.Infrastructure.Services.Security;

public class PasswordHasherService : IPasswordHasherService
{
    private const int SaltSize = 16; 
    private const int KeySize = 32; 
    private const int Iterations = 10000; 

    public string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(KeySize);

        var saltBase64 = Convert.ToBase64String(salt);
        var keyBase64 = Convert.ToBase64String(key);

        return $"{saltBase64}.{keyBase64}";
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2)
            return false;

        var salt = Convert.FromBase64String(parts[0]);
        var key = Convert.FromBase64String(parts[1]);

        using var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, Iterations, HashAlgorithmName.SHA256);
        var keyToCheck = pbkdf2.GetBytes(KeySize);

        return keyToCheck.SequenceEqual(key);
    }
}
