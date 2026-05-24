using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class CommentCreateDto
{
    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;
}
