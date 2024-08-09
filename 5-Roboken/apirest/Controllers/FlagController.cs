using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apirest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlagController : ControllerBase
{
    [Authorize(Roles = "Mishi,Lvl1")]
    [HttpGet("v1/{distance}")]
    public IActionResult Get1(
        string distance
    )
    {
        if (string.IsNullOrEmpty(distance))
            return BadRequest(new { success = false, message = "Distancia no encontrada :c" });

        distance = distance.Trim().ToLower();
        if (!distance.Equals("150 metros"))
            return BadRequest(new { success = false, message = "Distancia incorrecta o formato incorrecto. Try again!" });

        return Ok(new
        {
            success = true,
            message = "Zona marcada en un mapa de un océano..."
        });
    }

    [Authorize(Roles = "Mishi,Lvl2")]
    [HttpGet("v2/{zone}")]
    public IActionResult Get2(string zone)
    {
        if (string.IsNullOrEmpty(zone))
            return BadRequest(new { success = false, message = "Debe ingreasr una zona" });

        zone = zone.Trim().ToLower();
        if (!zone.Equals("media noche"))
            return BadRequest(new { success = false, message = "Lugar incorrecto :c" });

        return Ok(new
        {
            success = true,
            message = "Sustancia para permanecer cuerdo. c;"
        });
    }

    [Authorize(Roles = "Mishi,Lvl3")]
    [HttpGet("{sustance}")]
    public IActionResult GetFlag(string sustance)
    {
        if (string.IsNullOrEmpty(sustance))
            return BadRequest(new { success = false, message = "Debe ingresar una sustancia" });

        sustance = sustance.Trim().ToLower();
        if (!sustance.Equals("agua de almendras"))
            return BadRequest(new { success = false, message = "Sustancia incorrecta o mal escrita :c" });

        return Ok(new { success = true, message = "El fin se acerca... Ten, toma una flag: 1758edfb2ab3a9c7aa435e16c3deaf58" });
    }
}