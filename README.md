# Learning Copilot API

A .NET 8 Web API for managing course registrations with full support for user unregistration.

## Features

- **Course Management**: View available courses with real-time participant counts
- **User Registration**: Register users for courses with availability checking
- **Course Unregistration**: Cancel course registrations with optional reason tracking
- **Registration History**: View active and past registrations for users
- **Re-registration Support**: Allow users to re-register for courses after unregistration
- **Automatic Updates**: Participant counts update immediately when users register/unregister

## Running the Application

1. **Prerequisites**: .NET 8.0 SDK
2. **Build**: `dotnet build`
3. **Run**: `dotnet run`
4. **API Documentation**: Navigate to `https://localhost:5031/swagger` (or `http://localhost:5031/swagger`)

## API Endpoints

### Courses

- `GET /api/courses` - Get all available courses
- `GET /api/courses/{id}` - Get specific course details
- `GET /api/courses/{id}/registrations` - Get all registrations for a course

### Registrations

- `POST /api/registrations` - Register a user for a course
- `DELETE /api/registrations` - Unregister a user from a course
- `GET /api/registrations/user/{userId}` - Get user's registrations (active and past)

## Usage Examples

### View Available Courses
```bash
curl http://localhost:5031/api/courses
```

### Register for a Course
```bash
curl -X POST http://localhost:5031/api/registrations \
  -H "Content-Type: application/json" \
  -d '{"userId": 1, "courseId": 1}'
```

### Unregister from a Course
```bash
curl -X DELETE http://localhost:5031/api/registrations \
  -H "Content-Type: application/json" \
  -d '{"userId": 1, "courseId": 1, "reason": "Schedule conflict"}'
```

### View User Registrations
```bash
curl http://localhost:5031/api/registrations/user/1
```

## Sample Data

The application includes sample data:

**Users:**
- John Doe (ID: 1)
- Jane Smith (ID: 2)
- Mike Johnson (ID: 3)

**Courses:**
- Introduction to Programming (ID: 1) - 20 max participants
- Advanced Web Development (ID: 2) - 15 max participants
- Data Science Fundamentals (ID: 3) - 25 max participants

## Key Features Implemented

✅ **Course Unregistration**: Users can cancel their registration from any course  
✅ **Registration Viewing**: Users can view their current and past course registrations  
✅ **Confirmation Messages**: Clear success/error messages for all operations  
✅ **Participant Count Updates**: Course participant counts update immediately  
✅ **Unregistration Policies**: Support for optional unregistration reasons  
✅ **Re-registration Support**: Users can re-register for courses after unregistering  
✅ **Registration History**: Track both active and past registrations  

## Response Format

All API responses follow this format:
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": { /* response data */ }
}
```

## Error Handling

The API includes comprehensive error handling for:
- User not found
- Course not found
- Course capacity exceeded
- Duplicate registrations
- Invalid unregistration attempts