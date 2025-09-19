using Microsoft.AspNetCore.Mvc;
using LearningCopilot.Services;
using LearningCopilot.DTOs;

namespace LearningCopilot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationsController : ControllerBase
{
    private readonly ICourseRegistrationService _registrationService;

    public RegistrationsController(ICourseRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    /// <summary>
    /// Register a user for a course
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RegistrationDto>>> RegisterUser([FromBody] RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<RegistrationDto>(
                false, "Invalid request data", null));
        }

        var result = await _registrationService.RegisterUserAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Unregister a user from a course
    /// </summary>
    [HttpDelete]
    public async Task<ActionResult<ApiResponse<string>>> UnregisterUser([FromBody] UnregistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<string>(
                false, "Invalid request data", null));
        }

        var result = await _registrationService.UnregisterUserAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get all registrations for a specific user
    /// </summary>
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<ApiResponse<UserRegistrationsResponse>>> GetUserRegistrations(int userId)
    {
        var result = await _registrationService.GetUserRegistrationsAsync(userId);
        return result.Success ? Ok(result) : NotFound(result);
    }
}