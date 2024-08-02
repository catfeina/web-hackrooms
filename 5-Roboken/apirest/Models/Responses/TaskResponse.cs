namespace apirest.Models.Responses;

public class TaskResponse
{
    public required int TaskCode { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; }
    public required ICollection<CommentResponse> Comments { get; set; }
}

public class CommentResponse
{
    public required string Comment { get; set; }
    public required string UserName { get; set; }
}