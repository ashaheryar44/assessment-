using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Tickets;
using TeamTrackPro.API.Helpers;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

/// <summary>
/// Controller for managing tickets, including CRUD operations and ticket-specific actions.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ILogger<TicketsController> _logger;
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the TicketsController.
    /// </summary>
    /// <param name="ticketService">The ticket service for handling ticket operations.</param>
    /// <param name="logger">The logger for recording ticket-related events.</param>
    /// <param name="projectService">The project service for handling project operations.</param>
    /// <param name="userService">The user service for handling user operations.</param>
    public TicketsController(ITicketService ticketService, ILogger<TicketsController> logger, IProjectService projectService, IUserService userService)
    {
        _ticketService = ticketService;
        _logger = logger;
        _projectService = projectService;
        _userService = userService;
    }

    /// <summary>
    /// Retrieves a list of all tickets accessible to the current user.
    /// </summary>
    /// <returns>
    /// - 200 OK with list of tickets
    /// - 401 Unauthorized if user is not authenticated
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TicketDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var tickets = await _ticketService.GetTicketsAsync(userId);
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tickets");
            return StatusCode(500, new { message = "An error occurred while retrieving tickets" });
        }
    }

    /// <summary>
    /// Retrieves a specific ticket by its ID.
    /// </summary>
    /// <param name="id">The ID of the ticket to retrieve.</param>
    /// <returns>
    /// - 200 OK with ticket details
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have access to the ticket
    /// - 404 Not Found if ticket does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TicketDto>> GetTicket(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var ticket = await _ticketService.GetTicketByIdAsync(id, userId);
            
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(ticket);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ticket {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the ticket" });
        }
    }

    /// <summary>
    /// Creates a new ticket.
    /// </summary>
    /// <param name="request">The ticket creation request containing ticket details.</param>
    /// <returns>
    /// - 201 Created with the created ticket details
    /// - 400 Bad Request if the request is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketRequest request)
    {
        try
        {
            if (request.DueDate.HasValue && request.DueDate.Value < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Due date cannot be in the past" });
            }

            var userId = int.Parse(User.FindFirst("nameid")?.Value);
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

            var createdTicket = await _ticketService.CreateTicketAsync(ticket, userId);
            if (createdTicket == null)
            {
                return BadRequest(new { message = "Failed to create ticket" });
            }

            return CreatedAtAction(nameof(GetTicket), new { id = createdTicket.Id }, createdTicket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ticket {TicketTitle}", request.Title);
            return StatusCode(500, new { message = "An error occurred while creating the ticket" });
        }
    }

    /// <summary>
    /// Updates an existing ticket.
    /// </summary>
    /// <param name="id">The ID of the ticket to update.</param>
    /// <param name="request">The ticket update request containing updated ticket details.</param>
    /// <returns>
    /// - 200 OK with updated ticket details
    /// - 400 Bad Request if the request is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to update the ticket
    /// - 404 Not Found if ticket does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TicketDto>> UpdateTicket(int id, [FromBody] UpdateTicketRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            if (id != request.Id)
            {
                return BadRequest(new { message = "Ticket ID mismatch" });
            }

            if (request.DueDate.HasValue && request.DueDate.Value < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Due date cannot be in the past" });
            }

            var ticket = await _ticketService.UpdateTicketAsync(id, request, userId);
            
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(ticket);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ticket {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the ticket" });
        }
    }

    /// <summary>
    /// Deletes a ticket.
    /// </summary>
    /// <param name="id">The ID of the ticket to delete.</param>
    /// <returns>
    /// - 204 No Content if ticket is deleted successfully
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to delete the ticket
    /// - 404 Not Found if ticket does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        try
        {
            var success = await _ticketService.DeleteTicketAsync(id);
            
            if (!success)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ticket {TicketId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the ticket" });
        }
    }

    /// <summary>
    /// Updates the status of a ticket.
    /// </summary>
    /// <param name="ticketId">The ID of the ticket.</param>
    /// <param name="status">The new status to set.</param>
    /// <returns>
    /// - 200 OK if status is updated successfully
    /// - 400 Bad Request if the status is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to update the ticket
    /// - 404 Not Found if ticket does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPut("{ticketId}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTicketStatus(int ticketId, [FromBody] TicketStatus status)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var success = await _ticketService.UpdateTicketStatusAsync(ticketId, status, userId);
            
            if (!success)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(new { message = "Ticket status updated successfully" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating status for ticket {TicketId}", ticketId);
            return StatusCode(500, new { message = "An error occurred while updating ticket status" });
        }
    }

    /// <summary>
    /// Assigns a user to a ticket.
    /// </summary>
    /// <param name="ticketId">The ID of the ticket.</param>
    /// <param name="userId">The ID of the user to assign.</param>
    /// <returns>
    /// - 200 OK if user is assigned successfully
    /// - 400 Bad Request if the assignment is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to assign users
    /// - 404 Not Found if ticket or user does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPost("{ticketId}/assign/{userId}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignUserToTicket(int ticketId, int userId)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst("nameid")?.Value);
            var success = await _ticketService.AssignUserToTicketAsync(ticketId, userId, currentUserId);
            
            if (!success)
            {
                return NotFound(new { message = "Ticket or user not found" });
            }

            return Ok(new { message = "User assigned to ticket successfully" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning user {UserId} to ticket {TicketId}", userId, ticketId);
            return StatusCode(500, new { message = "An error occurred while assigning user to ticket" });
        }
    }

    /// <summary>
    /// Removes a user from a ticket.
    /// </summary>
    /// <param name="ticketId">The ID of the ticket.</param>
    /// <param name="userId">The ID of the user to remove.</param>
    /// <returns>
    /// - 200 OK if user is removed successfully
    /// - 400 Bad Request if the removal is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to remove users
    /// - 404 Not Found if ticket or user does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpDelete("{ticketId}/assign/{userId}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveUserFromTicket(int ticketId, int userId)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst("nameid")?.Value);
            var success = await _ticketService.RemoveUserFromTicketAsync(ticketId, userId, currentUserId);
            
            if (!success)
            {
                return NotFound(new { message = "Ticket or user not found" });
            }

            return Ok(new { message = "User removed from ticket successfully" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing user {UserId} from ticket {TicketId}", userId, ticketId);
            return StatusCode(500, new { message = "An error occurred while removing user from ticket" });
        }
    }
} 