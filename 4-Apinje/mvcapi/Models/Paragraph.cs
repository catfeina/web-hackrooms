using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcapi.Models;

[Table("Paragraph")]
public class Paragraph
{
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Verse { get; set; }
}