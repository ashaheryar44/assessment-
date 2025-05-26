using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTrackPro.API.Models;

public class ActivityLog : BaseEntity
{
    [Key]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [Required]
    [StringLength(50)]
    public required string Action { get; set; }
    
    [Required]
    [StringLength(500)]
    public required string Details { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string EntityType { get; set; }
    
    public int? EntityId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string IpAddress { get; set; }
    
    [Required]
    [MaxLength(500)]
    public required string AdditionalData { get; set; }
} 