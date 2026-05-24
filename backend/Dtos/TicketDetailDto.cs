namespace backend.Dtos;

public class TicketDetailDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public int? AssignedToId { get; set; }

    public UserDto? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<TicketCommentDto> Comments { get; set; } = [];
}

public class TicketCommentDto
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
