using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTrackPro.API.Models;

public class User : BaseEntity
{
    [Required]
    [StringLength(50)]
    public required string Username { get; set; }
    
    [Required]
    [StringLength(100)]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    public required string PasswordHash { get; set; }
    
    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }
    
    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }
    
    public int RoleId { get; set; }
    
    [ForeignKey("RoleId")]
    public Role? Role { get; set; }
    
    public DateTime? LastLoginAt { get; set; }

    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
} 