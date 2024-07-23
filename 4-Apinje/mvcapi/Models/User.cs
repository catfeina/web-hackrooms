using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcapi.Models;

[Table("User")]
public class Users
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50, ErrorMessage = "El campo {0} no debe exceder los {1} carácteres.")]
    public required string User { get; set; }
    [MaxLength(50, ErrorMessage = "El campo {0} no debe exceder los {1} carácteres.")]
    public required string Pass { get; set; }
}