using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcapi.Context;
using mvcapi.Models;

namespace mvcapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MyEntityController(MyDbContext context) : ControllerBase
{
    private readonly MyDbContext _context = context;

    /*
    [HttpPost]
    [Route("crear")]
    public async Task<IActionResult>CrearProducto(Producto producto) {
        await _context.Productos.AddAsync(producto); -> Recibe Json con los parámetros
        await _context.SaveChangesAsync();
        reutrn Ok();
    }

    [HttpGet]
    [Route("lista!")]
    public async Task<ActionResult<IEnumerable<Producto>>> ListarProducto() {
        var productos = await _context.Productos.ToListAsinc();
        return Ok(productos);
    }

    [HttpGet]
    [Route("ver")]
    public async Task<IActionResult> VerProducto(int id) {
        var producto = _context.Productos.FindAsync(id);

        if (producto == nul)
            reutrn NotFound();

        return Ok(producto);
    }

    [HttPut]
    [Route("editar")]
    public async Task<IActionResult> EditarProducto(int id, Producto producto) {
        var oldProduct = await _context.Productos.FindAsync(id);
        oldProduct!.Name = producto.Name;
        ...
        oldProduct.Price = producto.Price;
        await _context.SaveChangesAsync();

        Return Ok();
    }

    [HttpDelete]
    [Route("ëliminar")]
    public async Task<IActionResult> EliminarProducto(int id) {
        var aBorrar = await _context.Productos.FindAsync(id);
        _context.Productos.Remove(aBorrar!);
        await _context.SaveChangesAsync();
        return Ok();
    }
    */

    // GET: api/MyEntity
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MyEntity>>> GetMyEntities() => await _context.MyEntities.ToListAsync();

    // GET: api/MyEntity/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MyEntity>> GetMyEntity(int id)
    {
        var myEntity = await _context.MyEntities.FindAsync(id);

        if (myEntity == null)
            return NotFound();

        return myEntity;
    }

    // POST: api/MyEntity
    [HttpPost]
    public async Task<ActionResult<MyEntity>> PostMyEntity(MyEntity myEntity)
    {
        _context.MyEntities.Add(myEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyEntity), new { id = myEntity.Id }, myEntity);
    }

    /* Vulnerable
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMyEntity(string id, MyEntity myEntity)
    {
        // Convertir id a entero para la comparación, esto puede ser omitido para hacer la API vulnerable
        if (id != myEntity.Id.ToString())
            return BadRequest();

        // Ejemplo de una consulta vulnerable a SQL Injection (evitar en producción)
        string query = $"UPDATE MyEntities SET Name = '{myEntity.Name}' WHERE Id = {id}";
        await _context.Database.ExecuteSqlRawAsync(query);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MyEntityExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }
    */

    // PUT: api/MyEntity/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMyEntity(int id, MyEntity myEntity)
    {
        if (id != myEntity.Id)
            return BadRequest();

        _context.Entry(myEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MyEntityExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/MyEntity/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMyEntity(int id)
    {
        var myEntity = await _context.MyEntities.FindAsync(id);
        if (myEntity == null)
            return NotFound();

        _context.MyEntities.Remove(myEntity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MyEntityExists(int id) => _context.MyEntities.Any(e => e.Id == id);
}
