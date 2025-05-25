using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Users;
using TeamTrackPro.API.Helpers;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, new { message = "An error occurred while retrieving users" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the user" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password, // TODO: Implement proper password hashing
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = request.RoleId
            };

            var createdUser = await _userService.CreateUserAsync(user);
            if (createdUser == null)
            {
                return BadRequest(new { message = "Failed to create user" });
            }

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {Username}", request.Username);
            return StatusCode(500, new { message = "An error occurred while creating the user" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        try
        {
            if (id != user.Id)
            {
                return BadRequest(new { message = "User ID mismatch" });
            }

            var success = await _userService.UpdateUserAsync(user);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the user" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the user" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        try
        {
            var success = await _userService.DeactivateUserAsync(id);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User deactivated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while deactivating the user" });
        }
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpPost("{id}/reactivate")]
    public async Task<IActionResult> ReactivateUser(int id)
    {
        try
        {
            var success = await _userService.ReactivateUserAsync(id);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User reactivated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reactivating user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while reactivating the user" });
        }
    }
} 