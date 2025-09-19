using System.ComponentModel.DataAnnotations;

namespace LearningCopilot.Models;

public class Registration
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? UnregistrationDate { get; set; }
    
    public bool IsActive => !UnregistrationDate.HasValue;
    
    public string? UnregistrationReason { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
}