using System.ComponentModel.DataAnnotations;

namespace apirest.Models;

public class PoemModel
{
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Verse { get; set; }
}