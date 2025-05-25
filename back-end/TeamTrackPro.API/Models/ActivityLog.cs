using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTrackPro.API.Models;

public class ActivityLog
{
    [Key]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    [Required]
    public required User User { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Action { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string EntityType { get; set; }
    
    public int? EntityId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    [MaxLength(50)]
    public required string IpAddress { get; set; }
    
    [Required]
    [MaxLength(500)]
    public required string AdditionalData { get; set; }
} 