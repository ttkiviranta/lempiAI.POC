namespace LempiAI.POC.Blazor.Models;

public class Schedule
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string TaskDescription { get; set; } = null!;
    public DateTime ScheduledStart { get; set; }
    public DateTime ScheduledEnd { get; set; }
    public string Status { get; set; } = "Pending";
    public string Priority { get; set; } = "Medium";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
   // public Employee? Employee { get; set; }
}
