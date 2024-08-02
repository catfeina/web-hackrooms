using System.ComponentModel.DataAnnotations;

namespace apirest.Models.Requests;

public class LoginUserRequest
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}