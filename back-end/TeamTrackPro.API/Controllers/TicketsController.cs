using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Tickets;
using TeamTrackPro.API.Helpers;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ILogger<TicketsController> _logger;
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;

    public TicketsController(ITicketService ticketService, ILogger<TicketsController> logger, IProjectService projectService, IUserService userService)
    {
        _ticketService = ticketService;
        _logger = logger;
        _projectService = projectService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetAllTickets()
    {
        try
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all tickets");
            return StatusCode(500, new { message = "An error occurred while retrieving tickets" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> GetTicketById(int id)
    {
        try
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(ticket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting ticket {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the ticket" });
        }
    }

    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
    [HttpPost]
    public async Task<ActionResult<Ticket>> CreateTicket([FromBody] CreateTicketRequest request)
    {
        try
        {
            if (request.DueDate.HasValue && request.DueDate.Value < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Due date cannot be in the past" });
            }

            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectId,
                AssignedToId = request.AssignedToId,
                Priority = request.Priority,
                Type = request.Type,
                Status = TicketStatus.New,
                DueDate = request.DueDate,
                Project = await _projectService.GetProjectByIdAsync(request.ProjectId),
                AssignedTo = request.AssignedToId.HasValue ? await _userService.GetUserByIdAsync(request.AssignedToId.Value) : null
            };

            var createdTicket = await _ticketService.CreateTicketAsync(ticket);
            if (createdTicket == null)
            {
                return BadRequest(new { message = "Failed to create ticket" });
            }

            return CreatedAtAction(nameof(GetTicketById), new { id = createdTicket.Id }, createdTicket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ticket {TicketTitle}", request.Title);
            return StatusCode(500, new { message = "An error occurred while creating the ticket" });
        }
    }

    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket ticket)
    {
        try
        {
            if (id != ticket.Id)
            {
                return BadRequest(new { message = "Ticket ID mismatch" });
            }

            if (ticket.DueDate.HasValue && ticket.DueDate.Value < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Due date cannot be in the past" });
            }

            var success = await _ticketService.UpdateTicketAsync(ticket);
            if (!success)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(new { message = "Ticket updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ticket {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the ticket" });
        }
    }

    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        try
        {
            var success = await _ticketService.DeleteTicketAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(new { message = "Ticket deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ticket {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the ticket" });
        }
    }

    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager},{RoleConstants.User}")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] UpdateTicketStatusRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var userRole = User.FindFirst("role")?.Value;

            // Only allow status updates if user is assigned to the ticket or has admin/manager role
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            if (ticket.AssignedToId != userId && userRole != RoleConstants.Admin && userRole != RoleConstants.Manager)
            {
                return Forbid();
            }

            var success = await _ticketService.UpdateTicketStatusAsync(id, request.Status, request.TimeSpent, request.Comment);
            if (!success)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(new { message = "Ticket status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ticket status {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the ticket status" });
        }
    }

    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignTicket(int id, [FromBody] int assignedToId)
    {
        try
        {
            var success = await _ticketService.AssignTicketAsync(id, assignedToId);
            if (!success)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(new { message = "Ticket assigned successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning ticket {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while assigning the ticket" });
        }
    }
} 