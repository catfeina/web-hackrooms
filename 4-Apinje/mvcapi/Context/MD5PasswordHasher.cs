using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace mvcapi.Context;
public class MD5PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    public string HashPassword(TUser user, string password)
    {
        using var md5 = MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = md5.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public PasswordVerificationResult VerifyHashedPassword(
        TUser user,
        string hashedPassword,
        string providedPassword
    )
    {
        using var md5 = MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(providedPassword);
        var hash = md5.ComputeHash(bytes);
        var providedHash = Convert.ToBase64String(hash);

        if (hashedPassword == providedHash)
            return PasswordVerificationResult.Success;

        return PasswordVerificationResult.Failed;
    }
}
