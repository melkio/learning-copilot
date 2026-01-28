using System.ComponentModel.DataAnnotations;

namespace LearningCopilot.Models;

public class Course
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int MaxParticipants { get; set; }
    
    public int CurrentParticipants => Registrations?.Count ?? 0;
    
    public bool IsAvailable => CurrentParticipants < MaxParticipants;
    
    public string Instructor { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public List<Registration> Registrations { get; set; } = new();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}