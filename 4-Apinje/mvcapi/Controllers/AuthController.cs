using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace mvcapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager
    ) : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Models.Request.RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new IdentityUser
        {
            UserName = registerRequest.Username,
            Email = registerRequest.Email
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (result.Succeeded)
            return Ok(new { success = true });

        return BadRequest(new { success = false, errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Models.Request.LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(loginRequest.Username);

        if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { success = true });
        }

        return Unauthorized(new { success = false, message = "Invalid username or password" });
    }
}
