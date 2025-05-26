using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTrackPro.API.Models;

public class Project : BaseEntity
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    
    [Required]
    [StringLength(500)]
    public required string Description { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public int ManagerId { get; set; }
    
    [ForeignKey("ManagerId")]
    public User Manager { get; set; }
    
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
    
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

public enum ProjectStatus
{
    NotStarted,
    InProgress,
    OnHold,
    Completed,
    Cancelled
} 