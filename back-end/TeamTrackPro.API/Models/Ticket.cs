using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTrackPro.API.Models;

public class Ticket : BaseEntity
{
    [Required]
    [StringLength(200)]
    public required string Title { get; set; }
    
    [Required]
    [StringLength(1000)]
    public required string Description { get; set; }
    
    [Required]
    public TicketStatus Status { get; set; }
    
    [Required]
    public TicketPriority Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public double? TimeSpent { get; set; }
    
    public int ProjectId { get; set; }
    
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }
    
    public int? AssignedToId { get; set; }
    
    [ForeignKey("AssignedToId")]
    public User? AssignedTo { get; set; }
    
    public int CreatedById { get; set; }
    
    [ForeignKey("CreatedById")]
    public User CreatedBy { get; set; }
    
    [Required]
    public TicketType Type { get; set; } = TicketType.Task;
    
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
}

public enum TicketPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum TicketStatus
{
    New,
    InProgress,
    OnHold,
    Resolved,
    Closed
}

public enum TicketType
{
    Bug,
    Feature,
    Task,
    Enhancement
} 