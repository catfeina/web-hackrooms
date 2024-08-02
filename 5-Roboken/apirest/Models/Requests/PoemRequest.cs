using System.ComponentModel.DataAnnotations;

namespace apirest.Models.Requests;

public class PoemRequest
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Verse { get; set; }
}