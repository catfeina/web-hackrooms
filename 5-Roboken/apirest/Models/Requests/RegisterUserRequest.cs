namespace apirest.Models.Requests;

public class RegisterUserRequest
{
    public required string Username { set; get; }
    public required string Password { set; get; }
    public required string Role { set; get; }
    public string? Email { set; get; }
}