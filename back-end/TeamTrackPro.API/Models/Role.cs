using System.ComponentModel.DataAnnotations;

namespace TeamTrackPro.API.Models;

public class Role
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Description { get; set; }
    
    public ICollection<User> Users { get; set; } = new List<User>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
} 