using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(int id);
    Task<Project> CreateProjectAsync(Project project);
    Task<bool> UpdateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(int id);
    Task<bool> UpdateProjectStatusAsync(int id, ProjectStatus status);
    Task<IEnumerable<Ticket>> GetProjectTicketsAsync(int projectId);
} 