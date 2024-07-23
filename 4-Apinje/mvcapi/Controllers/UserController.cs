using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using mvcapi.Context;
using mvcapi.Models.Response;
using mvcapi.Models.Request;
//using Microsoft.AspNetCore.Cors;

namespace mvcapi.Controllers;

//[EnableCors("AllowAllOrigins")]
[Route("api/[controller]")]
[ApiController]
public class UserController(MyDbContext context) : ControllerBase
{
    private readonly MyDbContext _user = context;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _user.User
                .Select(u => new UserResponse { Name = u.User })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var query = $"select * from User where Id = {id}";
            var user = await _user.User.FromSqlRaw(query)
                .Select(u => new UserResponse { Name = u.User })
                .ToListAsync();

            if (user == null || user.Count == 0)
                return NotFound();

            return Ok(user);
        }
        catch (Exception)
        {
            return NotFound();
        }

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _user.User
            .Where(u => u.User == loginRequest.Username)
            .FirstOrDefaultAsync();

        if (user == null)
            return Unauthorized("Invalid username or password");

        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(loginRequest.Password);
        var hashBytes = md5.ComputeHash(inputBytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        if (hash != user.Pass)
            return Unauthorized("Invalid username or password");

        return Ok("Login successful");
    }
}