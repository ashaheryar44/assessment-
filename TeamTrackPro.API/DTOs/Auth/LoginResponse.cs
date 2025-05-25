namespace TeamTrackPro.API.DTOs.Auth;

public class LoginResponse
{
    public required string Token { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateTime ExpirationDate { get; set; }
} 