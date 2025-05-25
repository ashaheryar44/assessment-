using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Projects;
using TeamTrackPro.API.Helpers;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
    {
        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all projects");
            return StatusCode(500, new { message = "An error occurred while retrieving projects" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProjectById(int id)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the project" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectRequest request)
    {
        try
        {
            if (request.EndDate.HasValue && request.EndDate.Value < request.StartDate)
            {
                return BadRequest(new { message = "End date cannot be earlier than start date" });
            }

            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = ProjectStatus.NotStarted
            };

            var createdProject = await _projectService.CreateProjectAsync(project);
            if (createdProject == null)
            {
                return BadRequest(new { message = "Failed to create project" });
            }

            return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project {ProjectName}", request.Name);
            return StatusCode(500, new { message = "An error occurred while creating the project" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
    {
        try
        {
            if (id != project.Id)
            {
                return BadRequest(new { message = "Project ID mismatch" });
            }

            if (project.EndDate.HasValue && project.EndDate.Value < project.StartDate)
            {
                return BadRequest(new { message = "End date cannot be earlier than start date" });
            }

            var success = await _projectService.UpdateProjectAsync(project);
            if (!success)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(new { message = "Project updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the project" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        try
        {
            var success = await _projectService.DeleteProjectAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(new { message = "Project deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {ProjectId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the project" });
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