using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Services.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllTicketsAsync();
    Task<Ticket> GetTicketByIdAsync(int id);
    Task<Ticket> CreateTicketAsync(Ticket ticket);
    Task<bool> UpdateTicketAsync(Ticket ticket);
    Task<bool> DeleteTicketAsync(int id);
    Task<bool> UpdateTicketStatusAsync(int id, TicketStatus status, double? timeSpent, string comment);
    Task<bool> AssignTicketAsync(int id, int assignedToId);
} 