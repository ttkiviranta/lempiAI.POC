namespace LempiAI.POC.API.Models;

/// <summary>
/// Represents customer feedback or internal feedback about services.
/// </summary>
public class Feedback
{
    public int Id { get; set; }
    public int ProvidedByEmployeeId { get; set; }
    public int Rating { get; set; }
    public string FeedbackText { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Status { get; set; } = "Received";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Employee ProvidedByEmployee { get; set; } = null!;
}
