using System.ComponentModel.DataAnnotations;

namespace TeamTrackPro.API.DTOs.Auth;

public class LoginRequest
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Password { get; set; }
} 