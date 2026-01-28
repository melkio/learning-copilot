using System.ComponentModel.DataAnnotations;

namespace LearningCopilot.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(250)]
    public string Email { get; set; } = string.Empty;
    
    public string FullName => $"{FirstName} {LastName}";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<Registration> Registrations { get; set; } = new();
}