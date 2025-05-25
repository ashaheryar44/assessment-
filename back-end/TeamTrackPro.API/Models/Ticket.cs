using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTrackPro.API.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public required string Description { get; set; }
    
    [Required]
    public int ProjectId { get; set; }
    
    [ForeignKey("ProjectId")]
    public required Project Project { get; set; }
    
    public int? AssignedToId { get; set; }
    
    [ForeignKey("AssignedToId")]
    public required User AssignedTo { get; set; }
    
    [Required]
    public TicketStatus Status { get; set; } = TicketStatus.New;
    
    [Required]
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    
    [Required]
    public TicketType Type { get; set; } = TicketType.Task;
    
    public double? TimeSpent { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
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