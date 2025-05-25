using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> DeactivateUserAsync(int id);
    Task<bool> ReactivateUserAsync(int id);
} 