using Microsoft.EntityFrameworkCore;
using TeamTrackPro.API.Data;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TicketService> _logger;

    public TicketService(AppDbContext context, ILogger<TicketService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _context.Tickets
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<Ticket> GetTicketByIdAsync(int id)
    {
        return await _context.Tickets
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        try
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ticket");
            return null;
        }
    }

    public async Task<bool> UpdateTicketAsync(Ticket ticket)
    {
        try
        {
            var existingTicket = await _context.Tickets.FindAsync(ticket.Id);
            if (existingTicket == null)
                return false;

            _context.Entry(existingTicket).CurrentValues.SetValues(ticket);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ticket {TicketId}", ticket.Id);
            return false;
        }
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        try
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ticket {TicketId}", id);
            return false;
        }
    }

    public async Task<bool> UpdateTicketStatusAsync(int id, TicketStatus status, double? timeSpent, string comment)
    {
        try
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return false;

            ticket.Status = status;
            if (timeSpent.HasValue)
                ticket.TimeSpent = (ticket.TimeSpent ?? 0) + timeSpent.Value;

            if (!string.IsNullOrEmpty(comment))
            {
                var ticketComment = new TicketComment
                {
                    TicketId = id,
                    Comment = comment,
                    CreatedAt = DateTime.UtcNow,
                    Ticket = ticket,
                    CreatedBy = await _context.Users.FindAsync(ticket.AssignedToId)
                };
                _context.TicketComments.Add(ticketComment);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ticket status {TicketId}", id);
            return false;
        }
    }

    public async Task<bool> AssignTicketAsync(int id, int assignedToId)
    {
        try
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return false;

            ticket.AssignedToId = assignedToId;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning ticket {TicketId}", id);
            return false;
        }
    }
} 