using System.ComponentModel.DataAnnotations;

namespace TeamTrackPro.API.Models;

public class Project
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
    
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public enum ProjectStatus
{
    NotStarted,
    InProgress,
    OnHold,
    Completed,
    Cancelled
} 