using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcapi.Context;
using mvcapi.Models.Response;
namespace mvcapi.Controllers;

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
}