using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcapi.Models;

[Table("MyEntity")] // Especifica el nombre de la tabla
public class MyEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    /*
    [Colum(TypeName = "decimal(18, 2)")]
    public decimal Price {get;set;}

    [DataType(DataType.MultilineText)]
    [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
    public string Description {set;get;}
    */
}
