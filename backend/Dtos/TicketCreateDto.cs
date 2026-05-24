using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class TicketCreateDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Priority { get; set; } = string.Empty;

    public int? AssignedToId { get; set; }
}
