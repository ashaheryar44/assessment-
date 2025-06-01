using Microsoft.EntityFrameworkCore;
using TeamTrackPro.API.Data;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(AppDbContext context, ILogger<ProjectService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Manager)
            .Include(p => p.Tickets)
            .ToListAsync();
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Manager)
            .Include(p => p.Tickets)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        try
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return null;
        }
    }

    public async Task<bool> UpdateProjectAsync(Project project)
    {
        try
        {
            var existingProject = await _context.Projects.FindAsync(project.Id);
            if (existingProject == null)
                return false;

            _context.Entry(existingProject).CurrentValues.SetValues(project);
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
                return false;

            _context.Projects.Remove(project);
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
                return false;

            project.Status = status;
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
        return await _context.Tickets
            .Include(t => t.AssignedTo)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }
} 