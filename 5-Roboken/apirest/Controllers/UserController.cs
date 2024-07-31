using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [HttpPost("Role/Create")]
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
            return BadRequest(new { success = false, message = "Internal Server Error :c" });
        }
    }

    [HttpPost("Create")]
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
            return BadRequest(new { success = false, message = "Internal Server Error, verify users. :c" });
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

            var correctPass = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!correctPass)
                return Unauthorized(new { success = false, message = "Invalid username o password." });

            await _sigInManager.SignInAsync(user, false);
            return Ok(new { success = true, message = "Successful login. The cookie has been sent! :3" });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error en login: ${e.Message}");
            return Unauthorized(new { success = false, message = "Internal Server Error :c" });
        }
    }
}