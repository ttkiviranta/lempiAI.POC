namespace LempiAI.POC.API.Models;

/// <summary>
/// Represents an employee in the cleaning company.
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<Feedback> ProvidedFeedback { get; set; } = new List<Feedback>();
}
