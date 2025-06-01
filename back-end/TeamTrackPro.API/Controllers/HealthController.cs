using Microsoft.AspNetCore.Mvc;

namespace TeamTrackPro.API.Controllers;

/// <summary>
/// Controller for health check endpoints to monitor the application's status.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    /// <summary>
    /// Initializes a new instance of the HealthController.
    /// </summary>
    /// <param name="logger">The logger for recording health check events.</param>
    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Performs a basic health check to verify the API is running.
    /// </summary>
    /// <returns>
    /// - 200 OK with status information if the API is healthy
    /// - 500 Internal Server Error if the API is unhealthy
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthStatus), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CheckHealth()
    {
        try
        {
            var status = new HealthStatus
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = typeof(HealthController).Assembly.GetName().Version?.ToString() ?? "Unknown"
            };

            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(500, new { status = "Unhealthy", error = ex.Message });
        }
    }

    [HttpGet("detailed")]
    public IActionResult GetDetailed()
    {
        var healthInfo = new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            MachineName = Environment.MachineName,
            OSVersion = Environment.OSVersion.ToString()
        };

        return Ok(healthInfo);
    }
}

/// <summary>
/// Represents the health status of the API.
/// </summary>
public class HealthStatus
{
    /// <summary>
    /// The current status of the API (e.g., "Healthy" or "Unhealthy").
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// The UTC timestamp when the health check was performed.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The version of the API.
    /// </summary>
    public string Version { get; set; }
} 