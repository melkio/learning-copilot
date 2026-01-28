using System.ComponentModel.DataAnnotations;

namespace LearningCopilot.DTOs;

public record CourseDto(
    int Id,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants,
    int CurrentParticipants,
    bool IsAvailable,
    string Instructor,
    decimal Price
);

public record RegistrationDto(
    int Id,
    int UserId,
    string UserName,
    string UserEmail,
    int CourseId,
    string CourseTitle,
    DateTime RegistrationDate,
    bool IsActive
);

public record UnregistrationRequest(
    [Required] int UserId,
    [Required] int CourseId,
    string? Reason = null
);

public record RegistrationRequest(
    [Required] int UserId,
    [Required] int CourseId
);

public record ApiResponse<T>(
    bool Success,
    string Message,
    T? Data = default
);

public record UserRegistrationsResponse(
    int UserId,
    string UserName,
    string UserEmail,
    List<RegistrationDto> ActiveRegistrations,
    List<RegistrationDto> PastRegistrations
);