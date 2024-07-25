using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcapi.Models.Response;

namespace mvcapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlagController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        var flag = new FlagResponse()
        {
            Message = "Toma, pa' que dejes de llorar aweonao. Flag: ",
            Flag = "10ec91b80d9a29e28107dc92befb7090"
        };

        return Ok(flag);
    }
}
