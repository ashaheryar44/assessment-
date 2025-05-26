using System.ComponentModel.DataAnnotations;

namespace TeamTrackPro.API.Models;

public class Role : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Description { get; set; }
    
    public ICollection<User> Users { get; set; } = new List<User>();
} 