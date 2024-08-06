using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using apirest.Models;
using apirest.Models.Requests;
using System.Security.Claims;
using apirest.Context;
using Microsoft.EntityFrameworkCore;
using apirest.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace apirest.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController(
    RestContext tables,
    UserManager<IdentityUser> users
    ) : ControllerBase
{
    private readonly RestContext _table = tables;
    private readonly UserManager<IdentityUser> _user = users;

    [HttpPost("Create")]
    public async Task<IActionResult> CreateTask(
        [FromBody] CreateTaskRequest request
    )
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Console.WriteLine("[+] No logró determinar el Id del usuario al crear tarea.");
                return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
            }

            var user = await _user.FindByIdAsync(userId);
            if (user == null || user.UserName == null)
            {
                Console.WriteLine("[+] No se encontró el nombre del usuario al crear la tarea. :c");
                return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
            }

            var task = new TaskModel
            {
                Title = request.Title,
                Description = request.Description,
                Status = "Open"
            };

            _table.Tasks.Add(task);
            await _table.SaveChangesAsync();

            if (task.Id == null)
            {
                Console.WriteLine("[+] No se logró determinar el Id de la nueva tarea creada. :c");
                return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
            }

            _table.Comments.Add(new CommentModel
            {
                Comment = "Tarea inicializada",
                UserName = user.UserName,
                TaskId = task.Id.Value
            });

            await _table.SaveChangesAsync();

            return Ok(new { success = true, message = "Tarea creada con éxito. :3" });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al crear tarea: {e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        try
        {
            var tasks = await _table.Tasks.Include(c => c.Commets).ToListAsync();

            if (tasks == null)
            {
                Console.WriteLine("[+] No se lograron obtener las tareas y sus comentarios. :c");
                return NotFound(new { success = false, message = "Tareas no encontradas :c" });
            }

            var response = tasks.Select(t => new TaskResponse
            {
                TaskCode = t.Id.Value,
                Title = t.Title,
                Status = t.Status,
                Description = t.Description,
                Comments = t.Commets.Select(c => new CommentResponse
                {
                    Comment = c.Comment,
                    UserName = c.UserName
                }).ToList()
            }).ToList();

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al obtener tareas y sus comentarios: {e.Message}");
            return StatusCode(500, new { sucess = false, message = "Internal Server Error :c" });
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetTaskById(int Id)
    {
        try
        {
            var task = await _table.Tasks.Include(c => c.Commets).FirstOrDefaultAsync(t => t.Id == Id);

            if (task == null)
            {
                Console.WriteLine($"[+] No se logró obtener la tarea {Id} :c");
                return BadRequest(new { success = false, message = "Tarea no encontrada :c" });
            }

            var response = new TaskResponse
            {
                TaskCode = task.Id.Value,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Comments = task.Commets.Select(c => new CommentResponse
                {
                    Comment = c.Comment,
                    UserName = c.UserName
                }).ToList()
            };

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al consultar tarea por Id: {e.Message}");
            return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
        }
    }

    [HttpPost("Comment")]
    public async Task<IActionResult> CommentTask(
        [FromBody] CommentTaskRequest request
    )
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Console.WriteLine("[+] No se logró obtener el Id del usuario al comentar una tarea. :c");
                return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
            }

            var user = await _user.FindByIdAsync(userId);
            if (user == null || user.UserName == null)
            {
                Console.WriteLine("[+] No se logró encontrar el nombre del usuario al comentar la tarea. :c");
                return StatusCode(500, new { success = false, message = "Internal Server Error :c" });
            }

            var task = await _table.Tasks.FindAsync(request.TaskCode);
            if (task == null)
            {
                Console.WriteLine($"[+] No existe la tarea {request.TaskCode} para agregar el comentario. :c");
                return BadRequest(new { sucess = false, message = "Invalid Task Code :c" });
            }

            _table.Comments.Add(
                new CommentModel
                {
                    Comment = request.Comment,
                    UserName = user.UserName,
                    TaskId = request.TaskCode
                }
            );

            if (!string.IsNullOrEmpty(request.Status))
            {
                task.Status = request.Status;
                _table.Tasks.Update(task);
            }

            await _table.SaveChangesAsync();

            return Ok(new { success = true, message = "The task has been commented on! :0" });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[+] Error al comentar tarea {request.TaskCode}: {e.Message}");
            return StatusCode(500, new { sucess = false, message = "Internal Server Error :c" });
        }
    }
}