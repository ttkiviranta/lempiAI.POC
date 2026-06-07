namespace LempiAI.POC.Blazor.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Schedule> Schedules { get; set; } = new();
    public List<Feedback> ProvidedFeedback { get; set; } = new();
}
