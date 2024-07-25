using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcapi.Context;
using mvcapi.Models.Response;

namespace mvcapi.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class ParagraphController(MyDbContext context) : ControllerBase
{
    private readonly MyDbContext _poem = context;

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(string Id)
    {
        var param = Id.Equals("0") ? "" : $"where Id = {Id}";
        var query = $"select * from Paragraph {param}";
        var poem = await _poem.Poem.FromSqlRaw(query)
            .Select(v => new ParagraphResponse { Title = v.Title, Verse = v.Verse })
            .ToListAsync();

        return Ok(poem);
    }
}