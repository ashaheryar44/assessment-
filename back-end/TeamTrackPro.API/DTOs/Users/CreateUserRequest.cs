using System.ComponentModel.DataAnnotations;

namespace TeamTrackPro.API.DTOs.Users;

public class CreateUserRequest
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }
    
    [Required]
    public int RoleId { get; set; }
} 