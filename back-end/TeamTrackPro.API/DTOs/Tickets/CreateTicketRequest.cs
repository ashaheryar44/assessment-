using System.ComponentModel.DataAnnotations;
using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.DTOs.Tickets;

public class CreateTicketRequest
{
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public required string Description { get; set; }
    
    [Required]
    public int ProjectId { get; set; }
    
    public int? AssignedToId { get; set; }
    
    [Required]
    public TicketPriority Priority { get; set; }
    
    [Required]
    public TicketType Type { get; set; }
    
    public DateTime? DueDate { get; set; }
} 