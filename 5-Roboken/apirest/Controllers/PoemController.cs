using apirest.Context;
using apirest.Models;
using apirest.Models.Requests;
using apirest.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apirest.Controllers;

[Route("api/[controller]")]
[Authorize]
public class PoemController(
    RestContext tables
    ) : ControllerBase
{
    private readonly RestContext _table = tables;

    [Authorize(Roles = "Mishi")]
    [HttpPost]
    public async Task<IActionResult> CreatePoem(
        [FromBody] PoemRequest request
    )
    {
        try
        {
            _table.Poem.Add(new PoemModel
            {
                Title = request.Title,
                Verse = request.Verse
            });

            await _table.SaveChangesAsync();
            return Ok(new { success = true, message = "Verse has been added! :3" });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al creat verso: {e.Message}");
            return StatusCode(500, new { succes = false, message = "Internal Server Error :c" });
        }
    }

    [HttpGet("{inTitle}")]
    public async Task<IActionResult> GetVerses(
        string inTitle
    )
    {
        try
        {
            if (string.IsNullOrEmpty(inTitle))
                return BadRequest(new { success = false, message = "Debe ingresar un texto para buscar :c" });

            var query = $"select Title, Verse from Poem where Title like '%{inTitle}%'";
            var poem = await _table.Poem.FromSqlRaw(query)
                .Select(v => new PoemResponse { Title = v.Title, Verse = v.Verse })
                .ToListAsync();

            return Ok(poem);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al consultar verso: {e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
        }
    }
}