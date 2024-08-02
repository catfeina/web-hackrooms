using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apirest.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace apirest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    RoleManager<IdentityRole> roleManager
    ) : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _sigInManager = signInManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    [Authorize(Roles = "Mishi")]
    [HttpPost("Role")]
    public async Task<IActionResult> RegisterRole(
        [FromBody] Models.Requests.RegisterRoleRequest request
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var rolExist = await _roleManager.RoleExistsAsync(request.Rolename);
            if (rolExist)
                return BadRequest(new { success = false, message = "Role already exists! :0" });

            var result = await _roleManager.CreateAsync(new IdentityRole(request.Rolename));
            if (!result.Succeeded)
                return BadRequest(new { success = false, message = result.Errors });

            return Ok(new { success = true, message = "Role has been created! :3" });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al crear rol: {e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
        }
    }

    [Authorize(Roles = "Mishi")]
    [HttpGet("Role")]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var roles = await _roleManager.Roles.Select(r => new RoleResponse
            {
                Id = r.Id,
                Rolename = r.Name
            }).ToListAsync();

            return Ok(roles);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al devolver roles: ${e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
        }
    }

    [Authorize(Roles = "Mishi")]
    [HttpPost]
    public async Task<ActionResult> RegisterUser(
        [FromBody] Models.Requests.RegisterUserRequest request
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(request.Email))
            request.Email = "fake@fake.com";

        var user = new IdentityUser
        {
            UserName = request.Username,
            Email = request.Email
        };

        try
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(new { success = false, message = result.Errors });

            var existsRole = await _roleManager.RoleExistsAsync(request.Role);
            if (!existsRole)
            {
                await _userManager.DeleteAsync(user);
                Console.WriteLine("[+] Error al crear usuario: El rol no existe. :c");
                return BadRequest(new { success = false, message = "Internal Server Error :c" });
            }

            result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest(new { success = false, message = result.Errors });
            }

            return Ok(new { success = true, message = "User has been created! :3" });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al crear usuario: {e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error, verify users. :c" });
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = _userManager.Users.ToList();
            var usersWithRole = new List<UserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                usersWithRole.Add(new UserResponse
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = [.. roles]
                });
            }

            return Ok(usersWithRole);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al consultar los usuarios: ${e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Server :c" });
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(
        [FromBody] Models.Requests.LoginUserRequest request
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = ModelState });

        try
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return Unauthorized(new { success = false, message = "Invalid username o password." });

            var correctPass = _userManager.CheckPasswordAsync(user, request.Password);
            var role = _userManager.GetRolesAsync(user);
            var login = _sigInManager.SignInAsync(user, false);

            await Task.WhenAll(correctPass, role, login);

            if (!correctPass.Result)
                return Unauthorized(new { success = false, message = "Invalid username o password." });

            if (login == null)
                return StatusCode(500, new { success = false, message = "Internal Server Error :c" });

            if (role.Result == null)
                return StatusCode(500, new { success = false, message = "Internal Server Error :c" });

            return Ok(new
            {
                success = true,
                message = "Successful login. The cookie has been sent! :3",
                roles = role.Result
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error en login: ${e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
        }
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _sigInManager.SignOutAsync();
            return Ok(new { sucess = true, message = "Sucessful logout!" });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al cerrar sesión: ${e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
        }
    }

    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied()
    {
        return Unauthorized(new { sucess = false, message = "No, papu, usted no tiene autorización para esta acción. :c" });
    }
}