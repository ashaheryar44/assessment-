using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Services.Interfaces;

public interface IAuthService
{
    Task<(bool success, string token)> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(User user, string password);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(string email);
    Task<bool> ValidateTokenAsync(string token);
    Task<User> GetUserByUsernameAsync(string username);
} 