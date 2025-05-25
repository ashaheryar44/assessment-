using System.ComponentModel.DataAnnotations;

namespace TeamTrackPro.API.DTOs.Projects;

public class CreateProjectRequest
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
} 