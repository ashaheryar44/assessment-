using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Projects;
using TeamTrackPro.API.Helpers;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

/// <summary>
/// Controller for managing projects, including CRUD operations and project-specific actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    /// <summary>
    /// Initializes a new instance of the ProjectsController.
    /// </summary>
    /// <param name="projectService">The project service for handling project operations.</param>
    /// <param name="logger">The logger for recording project-related events.</param>
    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of all projects accessible to the current user.
    /// </summary>
    /// <returns>
    /// - 200 OK with list of projects
    /// - 401 Unauthorized if user is not authenticated
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var projects = await _projectService.GetProjectsAsync(userId);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving projects");
            return StatusCode(500, new { message = "An error occurred while retrieving projects" });
        }
    }

    /// <summary>
    /// Retrieves a specific project by its ID.
    /// </summary>
    /// <param name="id">The ID of the project to retrieve.</param>
    /// <returns>
    /// - 200 OK with project details
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have access to the project
    /// - 404 Not Found if project does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var project = await _projectService.GetProjectByIdAsync(id, userId);
            
            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(project);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving project {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the project" });
        }
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="request">The project creation request containing project details.</param>
    /// <returns>
    /// - 201 Created with the created project details
    /// - 400 Bad Request if the request is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var project = await _projectService.CreateProjectAsync(request, userId);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return StatusCode(500, new { message = "An error occurred while creating the project" });
        }
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="id">The ID of the project to update.</param>
    /// <param name="request">The project update request containing updated project details.</param>
    /// <returns>
    /// - 200 OK with updated project details
    /// - 400 Bad Request if the request is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to update the project
    /// - 404 Not Found if project does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectDto>> UpdateProject(int id, [FromBody] UpdateProjectRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var project = await _projectService.UpdateProjectAsync(id, request, userId);
            
            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(project);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the project" });
        }
    }

    /// <summary>
    /// Deletes a project.
    /// </summary>
    /// <param name="id">The ID of the project to delete.</param>
    /// <returns>
    /// - 204 No Content if project is deleted successfully
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to delete the project
    /// - 404 Not Found if project does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProject(int id)
    {
        try
        {
            var success = await _projectService.DeleteProjectAsync(id);
            
            if (!success)
            {
                return NotFound(new { message = "Project not found" });
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the project" });
        }
    }

    /// <summary>
    /// Assigns a user to a project.
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="userId">The ID of the user to assign.</param>
    /// <returns>
    /// - 200 OK if user is assigned successfully
    /// - 400 Bad Request if the assignment is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to assign users
    /// - 404 Not Found if project or user does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPost("{projectId}/assign/{userId}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignUserToProject(int projectId, int userId)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst("nameid")?.Value);
            var success = await _projectService.AssignUserToProjectAsync(projectId, userId, currentUserId);
            
            if (!success)
            {
                return NotFound(new { message = "Project or user not found" });
            }

            return Ok(new { message = "User assigned to project successfully" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning user {UserId} to project {ProjectId}", userId, projectId);
            return StatusCode(500, new { message = "An error occurred while assigning user to project" });
        }
    }

    /// <summary>
    /// Removes a user from a project.
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="userId">The ID of the user to remove.</param>
    /// <returns>
    /// - 200 OK if user is removed successfully
    /// - 400 Bad Request if the removal is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to remove users
    /// - 404 Not Found if project or user does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpDelete("{projectId}/assign/{userId}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveUserFromProject(int projectId, int userId)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst("nameid")?.Value);
            var success = await _projectService.RemoveUserFromProjectAsync(projectId, userId, currentUserId);
            
            if (!success)
            {
                return NotFound(new { message = "Project or user not found" });
            }

            return Ok(new { message = "User removed from project successfully" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing user {UserId} from project {ProjectId}", userId, projectId);
            return StatusCode(500, new { message = "An error occurred while removing user from project" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateProjectStatus(int id, [FromBody] ProjectStatus status)
    {
        try
        {
            var success = await _projectService.UpdateProjectStatusAsync(id, status);
            if (!success)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(new { message = "Project status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project status {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the project status" });
        }
    }

    [HttpGet("{id}/tickets")]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetProjectTickets(int id)
    {
        try
        {
            var tickets = await _projectService.GetProjectTicketsAsync(id);
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tickets for project {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving project tickets" });
        }
    }
} 