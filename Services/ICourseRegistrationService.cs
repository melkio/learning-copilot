using LearningCopilot.Models;
using LearningCopilot.DTOs;

namespace LearningCopilot.Services;

public interface ICourseRegistrationService
{
    Task<ApiResponse<RegistrationDto>> RegisterUserAsync(RegistrationRequest request);
    Task<ApiResponse<string>> UnregisterUserAsync(UnregistrationRequest request);
    Task<ApiResponse<UserRegistrationsResponse>> GetUserRegistrationsAsync(int userId);
    Task<ApiResponse<List<CourseDto>>> GetAvailableCoursesAsync();
    Task<ApiResponse<CourseDto>> GetCourseAsync(int courseId);
    Task<ApiResponse<List<RegistrationDto>>> GetCourseRegistrationsAsync(int courseId);
}