using System.ComponentModel.DataAnnotations;

namespace apirest.Models.Requests;

public class CreateTaskRequest
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }
}

public class CommentTaskRequest
{
    [Required]
    public required int TaskCode { get; set; }

    [Required]
    public required string Comment { get; set; }
    public string? Status { get; set; }
}