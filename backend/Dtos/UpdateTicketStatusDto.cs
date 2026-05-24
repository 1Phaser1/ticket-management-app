using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class UpdateTicketStatusDto
{
    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = string.Empty;
}
