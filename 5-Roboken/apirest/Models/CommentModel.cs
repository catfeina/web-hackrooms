using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apirest.Models;

public class CommentModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [MaxLength(200)]
    public required string Comment { get; set; }

    [MaxLength(50)]
    public required string UserName { get; set; }

    [Required]
    public int TaskId { get; set; }

    [ForeignKey("TaskId")]
    public TaskModel? Task { get; set; }
}