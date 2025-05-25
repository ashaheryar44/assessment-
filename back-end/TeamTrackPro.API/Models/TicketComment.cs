using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTrackPro.API.Models;

public class TicketComment
{
    [Key]
    public int Id { get; set; }

    public int TicketId { get; set; }
    [Required]
    public required Ticket Ticket { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? CreatedById { get; set; }
    [Required]
    public required User CreatedBy { get; set; }
} 