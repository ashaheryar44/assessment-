using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTrackPro.API.DTOs.Auth;
using TeamTrackPro.API.Services.Interfaces;

namespace TeamTrackPro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
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

    [Authorize]
    [HttpPost("change-password")]
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

    [HttpPost("reset-password")]
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

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}

public class ResetPasswordRequest
{
    public string Email { get; set; }
} 