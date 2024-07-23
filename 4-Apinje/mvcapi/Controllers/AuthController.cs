using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using mvcapi.Models.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using mvcapi.Context;

[ApiController]
[Route("api/[controller]")]
public class AuthController(MyDbContext context) : ControllerBase
{
    private readonly MyDbContext _user = context;
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _user.User
            .Where(u => u.User == loginRequest.Username)
            .FirstOrDefaultAsync();

        if (user == null)
            return Unauthorized(new { success = false, message = "Invalid username or password" });

        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(loginRequest.Password);
        var hashBytes = md5.ComputeHash(inputBytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        if (hash != user.Pass)
            return Unauthorized(new { success = false, message = "Invalid username or password" });

        // Generar el token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.User)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Success = true, Token = tokenString });
    }
}
