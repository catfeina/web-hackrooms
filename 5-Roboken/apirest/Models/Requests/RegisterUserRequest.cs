using System.ComponentModel.DataAnnotations;

namespace apirest.Models.Requests;

public class RegisterUserRequest
{
    [Required]
    public required string Username { set; get; }

    [Required]
    public required string Password { set; get; }

    [Required]
    public required string Role { set; get; }
    public string? Email { set; get; }
}