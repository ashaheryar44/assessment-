using System.ComponentModel.DataAnnotations;
using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.DTOs.Tickets;

public class UpdateTicketStatusRequest
{
    [Required]
    public TicketStatus Status { get; set; }
    
    [Range(0, double.MaxValue)]
    public double? TimeSpent { get; set; }
    
    [MaxLength(500)]
    public string? Comment { get; set; }
} 