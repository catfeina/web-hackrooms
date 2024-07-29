using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace mvcapi.Context;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

public class MD5PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    public string HashPassword(TUser user, string password)
    {
        var inputBytes = Encoding.ASCII.GetBytes(password);
        var hashBytes = MD5.HashData(inputBytes);

        // Convert the byte array to hexadecimal string
        var sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }

        return sb.ToString();
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        var hashOfProvidedPassword = HashPassword(user, providedPassword);

        if (hashedPassword.Equals(hashOfProvidedPassword, StringComparison.OrdinalIgnoreCase))
            return PasswordVerificationResult.Success;

        return PasswordVerificationResult.Failed;
    }
}
