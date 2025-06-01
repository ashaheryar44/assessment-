using Microsoft.EntityFrameworkCore;
using TeamTrackPro.API.Data;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(AppDbContext context, ILogger<ProjectService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        try
        {
            return await _context.Projects
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project by ID {ProjectId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        try
        {
            return await _context.Projects
                .Where(p => p.IsActive)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all projects");
            return new List<Project>();
        }
    }

    public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status)
    {
        try
        {
            return await _context.Projects
                .Where(p => p.Status == status && p.IsActive)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting projects by status {Status}", status);
            return new List<Project>();
        }
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        try
        {
            _logger.LogInformation("Creating project: {@Project}", project);
            
            project.CreatedAt = DateTime.UtcNow;
            project.IsActive = true;
            project.Status = ProjectStatus.NotStarted;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Project created successfully with ID: {ProjectId}", project.Id);
            return project;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project {ProjectName}. Details: {Message}", project.Name, ex.Message);
            if (ex.InnerException != null)
            {
                _logger.LogError("Inner exception: {Message}", ex.InnerException.Message);
            }
            return null;
        }
    }

    public async Task<bool> UpdateProjectAsync(Project project)
    {
        try
        {
            var existingProject = await _context.Projects.FindAsync(project.Id);
            if (existingProject == null)
            {
                return false;
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {ProjectId}", project.Id);
            return false;
        }
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        try
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }

            project.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {ProjectId}", id);
            return false;
        }
    }

    public async Task<bool> UpdateProjectStatusAsync(int id, ProjectStatus status)
    {
        try
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }

            project.Status = status;
            project.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project status {ProjectId}", id);
            return false;
        }
    }

    public async Task<IEnumerable<Ticket>> GetProjectTicketsAsync(int projectId)
    {
        try
        {
            return await _context.Tickets
                .Where(t => t.ProjectId == projectId && t.IsActive)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tickets for project {ProjectId}", projectId);
            return new List<Ticket>();
        }
    }
} 