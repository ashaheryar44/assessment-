using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Users;
using TeamTrackPro.API.Helpers;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

/// <summary>
/// Controller for managing users, including CRUD operations and user-specific actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of the UsersController.
    /// </summary>
    /// <param name="userService">The user service for handling user operations.</param>
    /// <param name="logger">The logger for recording user-related events.</param>
    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <returns>
    /// - 200 OK with list of users
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to view users
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new { message = "An error occurred while retrieving users" });
        }
    }

    /// <summary>
    /// Retrieves a specific user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>
    /// - 200 OK with user details
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to view the user
    /// - 404 Not Found if user does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> GetUser(int id)
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
            _logger.LogError(ex, "Error retrieving user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the user" });
        }
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request containing user details.</param>
    /// <returns>
    /// - 201 Created with the created user details
    /// - 400 Bad Request if the request is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to create users
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, new { message = "An error occurred while creating the user" });
        }
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="request">The user update request containing updated user details.</param>
    /// <returns>
    /// - 200 OK with updated user details
    /// - 400 Bad Request if the request is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to update the user
    /// - 404 Not Found if user does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(id, request);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the user" });
        }
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>
    /// - 204 No Content if user is deleted successfully
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to delete users
    /// - 404 Not Found if user does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var success = await _userService.DeleteUserAsync(id);
            
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the user" });
        }
    }

    /// <summary>
    /// Updates a user's role.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="roleId">The ID of the new role.</param>
    /// <returns>
    /// - 200 OK if role is updated successfully
    /// - 400 Bad Request if the role is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 403 Forbidden if user does not have permission to update roles
    /// - 404 Not Found if user or role does not exist
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUserRole(int id, [FromBody] int roleId)
    {
        try
        {
            var success = await _userService.UpdateUserRoleAsync(id, roleId);
            
            if (!success)
            {
                return NotFound(new { message = "User or role not found" });
            }

            return Ok(new { message = "User role updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role for user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while updating user role" });
        }
    }

    /// <summary>
    /// Retrieves the current user's profile.
    /// </summary>
    /// <returns>
    /// - 200 OK with user profile details
    /// - 401 Unauthorized if user is not authenticated
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> GetCurrentUserProfile()
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current user profile");
            return StatusCode(500, new { message = "An error occurred while retrieving user profile" });
        }
    }

    /// <summary>
    /// Updates the current user's profile.
    /// </summary>
    /// <param name="request">The profile update request containing updated user details.</param>
    /// <returns>
    /// - 200 OK with updated user details
    /// - 400 Bad Request if the request is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> UpdateCurrentUserProfile([FromBody] UpdateUserProfileRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var user = await _userService.UpdateUserProfileAsync(userId, request);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating current user profile");
            return StatusCode(500, new { message = "An error occurred while updating user profile" });
        }
    }
} 