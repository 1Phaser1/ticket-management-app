using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketListDto>>> GetTickets()
    {
        var tickets = await _context.Tickets
            .Include(ticket => ticket.AssignedTo)
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ToListAsync();

        return Ok(tickets.Select(MapToListDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketDetailDto>> GetTicket(int id)
    {
        var ticket = await _context.Tickets
            .Include(ticket => ticket.AssignedTo)
            .Include(ticket => ticket.Comments)
            .FirstOrDefaultAsync(ticket => ticket.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        return Ok(MapToDetailDto(ticket));
    }

    [HttpPost]
    public async Task<ActionResult<TicketDetailDto>> CreateTicket(TicketCreateDto dto)
    {
        if (!await UserExists(dto.AssignedToId))
        {
            return BadRequest("Assigned user was not found.");
        }

        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            AssignedToId = dto.AssignedToId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        await _context.Entry(ticket).Reference(item => item.AssignedTo).LoadAsync();

        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, MapToDetailDto(ticket));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TicketDetailDto>> UpdateTicket(int id, TicketUpdateDto dto)
    {
        var ticket = await _context.Tickets
            .Include(item => item.Comments)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        if (!await UserExists(dto.AssignedToId))
        {
            return BadRequest("Assigned user was not found.");
        }

        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.Status = dto.Status;
        ticket.Priority = dto.Priority;
        ticket.AssignedToId = dto.AssignedToId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _context.Entry(ticket).Reference(item => item.AssignedTo).LoadAsync();

        return Ok(MapToDetailDto(ticket));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateTicketStatus(int id, UpdateTicketStatusDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket is null)
        {
            return NotFound();
        }

        ticket.Status = dto.Status;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id:int}/assign")]
    public async Task<ActionResult<TicketListDto>> AssignTicket(int id, AssignTicketDto dto)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(item => item.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        if (!await UserExists(dto.AssignedToId))
        {
            return BadRequest("Assigned user was not found.");
        }

        ticket.AssignedToId = dto.AssignedToId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _context.Entry(ticket).Reference(item => item.AssignedTo).LoadAsync();

        return Ok(MapToListDto(ticket));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket is null)
        {
            return NotFound();
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{ticketId:int}/comments")]
    public async Task<ActionResult<TicketCommentDto>> AddComment(int ticketId, CommentCreateDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);

        if (ticket is null)
        {
            return NotFound();
        }

        var comment = new Comment
        {
            TicketId = ticketId,
            Text = dto.Text,
            CreatedAt = DateTime.UtcNow
        };

        ticket.UpdatedAt = DateTime.UtcNow;
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var result = new TicketCommentDto
        {
            Id = comment.Id,
            TicketId = comment.TicketId,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt
        };

        return CreatedAtAction(nameof(GetTicket), new { id = ticketId }, result);
    }

    private async Task<bool> UserExists(int? userId)
    {
        return !userId.HasValue || await _context.Users.AnyAsync(user => user.Id == userId.Value);
    }

    private static TicketListDto MapToListDto(Ticket ticket)
    {
        return new TicketListDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Status = ticket.Status,
            Priority = ticket.Priority,
            AssignedToId = ticket.AssignedToId,
            AssignedToFullName = ticket.AssignedTo?.FullName,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
    }

    private static TicketDetailDto MapToDetailDto(Ticket ticket)
    {
        return new TicketDetailDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            AssignedToId = ticket.AssignedToId,
            AssignedTo = ticket.AssignedTo is null
                ? null
                : new UserDto
                {
                    Id = ticket.AssignedTo.Id,
                    FullName = ticket.AssignedTo.FullName,
                    Email = ticket.AssignedTo.Email
                },
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            Comments = ticket.Comments
                .OrderBy(comment => comment.CreatedAt)
                .Select(comment => new TicketCommentDto
                {
                    Id = comment.Id,
                    TicketId = comment.TicketId,
                    Text = comment.Text,
                    CreatedAt = comment.CreatedAt
                })
                .ToList()
        };
    }
}
