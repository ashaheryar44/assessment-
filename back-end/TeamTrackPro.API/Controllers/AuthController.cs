using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Auth;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

/// <summary>
/// Controller for handling authentication-related operations such as login, password change, and password reset.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the AuthController.
    /// </summary>
    /// <param name="authService">The authentication service for handling user authentication.</param>
    /// <param name="logger">The logger for recording authentication events.</param>
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token upon successful login.
    /// </summary>
    /// <param name="request">The login request containing username and password.</param>
    /// <returns>
    /// - 200 OK with JWT token and user details if login is successful
    /// - 401 Unauthorized if credentials are invalid
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Login request received for username: {Username}", request.Username);

            var (success, token) = await _authService.LoginAsync(request.Username, request.Password);
            if (!success)
            {
                _logger.LogWarning("Login failed for username: {Username}", request.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Get user details from database
            var user = await _authService.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                _logger.LogError("User not found after successful login: {Username}", request.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

            _logger.LogInformation("Login successful for user: {Username}", request.Username);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                RoleName = user.Role?.Name ?? "User",
                ExpirationDate = DateTime.UtcNow.AddMinutes(60)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Username}", request.Username);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    /// <summary>
    /// Changes the password for the currently authenticated user.
    /// </summary>
    /// <param name="request">The password change request containing current and new passwords.</param>
    /// <returns>
    /// - 200 OK if password is changed successfully
    /// - 400 Bad Request if current password is invalid
    /// - 401 Unauthorized if user is not authenticated
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("nameid")?.Value);
            var success = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            
            if (!success)
            {
                return BadRequest(new { message = "Invalid current password" });
            }

            return Ok(new { message = "Password changed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user");
            return StatusCode(500, new { message = "An error occurred while changing password" });
        }
    }

    /// <summary>
    /// Initiates the password reset process for a user by sending reset instructions to their email.
    /// </summary>
    /// <param name="request">The password reset request containing the user's email address.</param>
    /// <returns>
    /// - 200 OK if reset instructions are sent successfully
    /// - 400 Bad Request if email is not found
    /// - 500 Internal Server Error if an unexpected error occurs
    /// </returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            var success = await _authService.ResetPasswordAsync(request.Email);
            if (!success)
            {
                return BadRequest(new { message = "Email not found" });
            }

            return Ok(new { message = "Password reset instructions sent to email" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for email {Email}", request.Email);
            return StatusCode(500, new { message = "An error occurred while resetting password" });
        }
    }
}

/// <summary>
/// Request model for changing a user's password.
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// The user's current password.
    /// </summary>
    public string CurrentPassword { get; set; }

    /// <summary>
    /// The new password to set.
    /// </summary>
    public string NewPassword { get; set; }
}

/// <summary>
/// Request model for resetting a user's password.
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// The email address of the user requesting a password reset.
    /// </summary>
    public string Email { get; set; }
} 