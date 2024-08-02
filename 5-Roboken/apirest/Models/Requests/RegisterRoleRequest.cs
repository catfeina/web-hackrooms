using System.ComponentModel.DataAnnotations;

namespace apirest.Models.Requests;

public class RegisterRoleRequest
{
    [Required]
    public required string Rolename { get; set; }
}