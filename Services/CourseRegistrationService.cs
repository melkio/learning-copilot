using LearningCopilot.Models;
using LearningCopilot.DTOs;
using System.Collections.Concurrent;

namespace LearningCopilot.Services;

public class CourseRegistrationService : ICourseRegistrationService
{
    private readonly ConcurrentDictionary<int, Course> _courses = new();
    private readonly ConcurrentDictionary<int, User> _users = new();
    private readonly ConcurrentDictionary<int, Registration> _registrations = new();
    private int _nextCourseId = 1;
    private int _nextUserId = 1;
    private int _nextRegistrationId = 1;

    public CourseRegistrationService()
    {
        SeedData();
    }

    private void SeedData()
    {
        // Create sample users
        var users = new[]
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" },
            new User { Id = 3, FirstName = "Mike", LastName = "Johnson", Email = "mike.johnson@example.com" }
        };

        foreach (var user in users)
        {
            _users.TryAdd(user.Id, user);
        }
        _nextUserId = 4;

        // Create sample courses
        var courses = new[]
        {
            new Course 
            { 
                Id = 1, 
                Title = "Introduction to Programming", 
                Description = "Learn the basics of programming with practical examples.",
                StartDate = DateTime.UtcNow.AddDays(30),
                EndDate = DateTime.UtcNow.AddDays(60),
                MaxParticipants = 20,
                Instructor = "Dr. Sarah Wilson",
                Price = 299.99m
            },
            new Course 
            { 
                Id = 2, 
                Title = "Advanced Web Development", 
                Description = "Master modern web development frameworks and tools.",
                StartDate = DateTime.UtcNow.AddDays(45),
                EndDate = DateTime.UtcNow.AddDays(90),
                MaxParticipants = 15,
                Instructor = "Prof. David Brown",
                Price = 499.99m
            },
            new Course 
            { 
                Id = 3, 
                Title = "Data Science Fundamentals", 
                Description = "Explore data analysis, visualization, and machine learning basics.",
                StartDate = DateTime.UtcNow.AddDays(60),
                EndDate = DateTime.UtcNow.AddDays(120),
                MaxParticipants = 25,
                Instructor = "Dr. Emily Chen",
                Price = 399.99m
            }
        };

        foreach (var course in courses)
        {
            _courses.TryAdd(course.Id, course);
        }
        _nextCourseId = 4;

        // Create some sample registrations
        var registrations = new[]
        {
            new Registration { Id = 1, UserId = 1, CourseId = 1, RegistrationDate = DateTime.UtcNow.AddDays(-5) },
            new Registration { Id = 2, UserId = 2, CourseId = 1, RegistrationDate = DateTime.UtcNow.AddDays(-3) },
            new Registration { Id = 3, UserId = 1, CourseId = 2, RegistrationDate = DateTime.UtcNow.AddDays(-2) }
        };

        foreach (var registration in registrations)
        {
            _registrations.TryAdd(registration.Id, registration);
        }
        _nextRegistrationId = 4;
    }

    public Task<ApiResponse<RegistrationDto>> RegisterUserAsync(RegistrationRequest request)
    {
        try
        {
            if (!_users.TryGetValue(request.UserId, out var user))
            {
                return Task.FromResult(new ApiResponse<RegistrationDto>(
                    false, "User not found", null));
            }

            if (!_courses.TryGetValue(request.CourseId, out var course))
            {
                return Task.FromResult(new ApiResponse<RegistrationDto>(
                    false, "Course not found", null));
            }

            // Check if user is already registered for this course
            var existingRegistration = _registrations.Values
                .FirstOrDefault(r => r.UserId == request.UserId && r.CourseId == request.CourseId && r.IsActive);
            
            if (existingRegistration != null)
            {
                return Task.FromResult(new ApiResponse<RegistrationDto>(
                    false, "User is already registered for this course", null));
            }

            // Check if course has available spots
            var currentRegistrations = _registrations.Values
                .Count(r => r.CourseId == request.CourseId && r.IsActive);
            
            if (currentRegistrations >= course.MaxParticipants)
            {
                return Task.FromResult(new ApiResponse<RegistrationDto>(
                    false, "Course is full", null));
            }

            // Create new registration
            var registration = new Registration
            {
                Id = Interlocked.Increment(ref _nextRegistrationId),
                UserId = request.UserId,
                CourseId = request.CourseId,
                RegistrationDate = DateTime.UtcNow
            };

            _registrations.TryAdd(registration.Id, registration);

            var registrationDto = new RegistrationDto(
                registration.Id,
                user.Id,
                user.FullName,
                user.Email,
                course.Id,
                course.Title,
                registration.RegistrationDate,
                registration.IsActive
            );

            return Task.FromResult(new ApiResponse<RegistrationDto>(
                true, $"Successfully registered for {course.Title}", registrationDto));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ApiResponse<RegistrationDto>(
                false, $"Registration failed: {ex.Message}", null));
        }
    }

    public Task<ApiResponse<string>> UnregisterUserAsync(UnregistrationRequest request)
    {
        try
        {
            if (!_users.TryGetValue(request.UserId, out var user))
            {
                return Task.FromResult(new ApiResponse<string>(
                    false, "User not found", null));
            }

            if (!_courses.TryGetValue(request.CourseId, out var course))
            {
                return Task.FromResult(new ApiResponse<string>(
                    false, "Course not found", null));
            }

            // Find active registration
            var registration = _registrations.Values
                .FirstOrDefault(r => r.UserId == request.UserId && r.CourseId == request.CourseId && r.IsActive);

            if (registration == null)
            {
                return Task.FromResult(new ApiResponse<string>(
                    false, "No active registration found for this user and course", null));
            }

            // Mark registration as inactive
            registration.UnregistrationDate = DateTime.UtcNow;
            registration.UnregistrationReason = request.Reason;

            var message = $"Successfully unregistered {user.FullName} from {course.Title}";
            if (!string.IsNullOrWhiteSpace(request.Reason))
            {
                message += $". Reason: {request.Reason}";
            }

            return Task.FromResult(new ApiResponse<string>(
                true, message, message));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ApiResponse<string>(
                false, $"Unregistration failed: {ex.Message}", null));
        }
    }

    public Task<ApiResponse<UserRegistrationsResponse>> GetUserRegistrationsAsync(int userId)
    {
        try
        {
            if (!_users.TryGetValue(userId, out var user))
            {
                return Task.FromResult(new ApiResponse<UserRegistrationsResponse>(
                    false, "User not found", null));
            }

            var userRegistrations = _registrations.Values
                .Where(r => r.UserId == userId)
                .ToList();

            var activeRegistrations = userRegistrations
                .Where(r => r.IsActive)
                .Select(r => CreateRegistrationDto(r))
                .Where(dto => dto != null)
                .Cast<RegistrationDto>()
                .ToList();

            var pastRegistrations = userRegistrations
                .Where(r => !r.IsActive)
                .Select(r => CreateRegistrationDto(r))
                .Where(dto => dto != null)
                .Cast<RegistrationDto>()
                .ToList();

            var response = new UserRegistrationsResponse(
                user.Id,
                user.FullName,
                user.Email,
                activeRegistrations,
                pastRegistrations
            );

            return Task.FromResult(new ApiResponse<UserRegistrationsResponse>(
                true, "User registrations retrieved successfully", response));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ApiResponse<UserRegistrationsResponse>(
                false, $"Failed to retrieve user registrations: {ex.Message}", null));
        }
    }

    public Task<ApiResponse<List<CourseDto>>> GetAvailableCoursesAsync()
    {
        try
        {
            var courses = _courses.Values
                .Select(CreateCourseDto)
                .ToList();

            return Task.FromResult(new ApiResponse<List<CourseDto>>(
                true, "Available courses retrieved successfully", courses));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ApiResponse<List<CourseDto>>(
                false, $"Failed to retrieve courses: {ex.Message}", null));
        }
    }

    public Task<ApiResponse<CourseDto>> GetCourseAsync(int courseId)
    {
        try
        {
            if (!_courses.TryGetValue(courseId, out var course))
            {
                return Task.FromResult(new ApiResponse<CourseDto>(
                    false, "Course not found", null));
            }

            var courseDto = CreateCourseDto(course);
            return Task.FromResult(new ApiResponse<CourseDto>(
                true, "Course retrieved successfully", courseDto));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ApiResponse<CourseDto>(
                false, $"Failed to retrieve course: {ex.Message}", null));
        }
    }

    public Task<ApiResponse<List<RegistrationDto>>> GetCourseRegistrationsAsync(int courseId)
    {
        try
        {
            if (!_courses.TryGetValue(courseId, out var course))
            {
                return Task.FromResult(new ApiResponse<List<RegistrationDto>>(
                    false, "Course not found", null));
            }

            var registrations = _registrations.Values
                .Where(r => r.CourseId == courseId && r.IsActive)
                .Select(r => CreateRegistrationDto(r))
                .Where(dto => dto != null)
                .Cast<RegistrationDto>()
                .ToList();

            return Task.FromResult(new ApiResponse<List<RegistrationDto>>(
                true, "Course registrations retrieved successfully", registrations));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ApiResponse<List<RegistrationDto>>(
                false, $"Failed to retrieve course registrations: {ex.Message}", null));
        }
    }

    private CourseDto CreateCourseDto(Course course)
    {
        var currentParticipants = _registrations.Values
            .Count(r => r.CourseId == course.Id && r.IsActive);

        return new CourseDto(
            course.Id,
            course.Title,
            course.Description,
            course.StartDate,
            course.EndDate,
            course.MaxParticipants,
            currentParticipants,
            currentParticipants < course.MaxParticipants,
            course.Instructor,
            course.Price
        );
    }

    private RegistrationDto? CreateRegistrationDto(Registration registration)
    {
        if (!_users.TryGetValue(registration.UserId, out var user) ||
            !_courses.TryGetValue(registration.CourseId, out var course))
        {
            return null;
        }

        return new RegistrationDto(
            registration.Id,
            user.Id,
            user.FullName,
            user.Email,
            course.Id,
            course.Title,
            registration.RegistrationDate,
            registration.IsActive
        );
    }
}