using Microsoft.AspNetCore.Mvc;
using LearningCopilot.Services;
using LearningCopilot.DTOs;

namespace LearningCopilot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseRegistrationService _registrationService;

    public CoursesController(ICourseRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    /// <summary>
    /// Get all available courses
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CourseDto>>>> GetCourses()
    {
        var result = await _registrationService.GetAvailableCoursesAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get a specific course by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> GetCourse(int id)
    {
        var result = await _registrationService.GetCourseAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get all registrations for a specific course
    /// </summary>
    [HttpGet("{id:int}/registrations")]
    public async Task<ActionResult<ApiResponse<List<RegistrationDto>>>> GetCourseRegistrations(int id)
    {
        var result = await _registrationService.GetCourseRegistrationsAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}