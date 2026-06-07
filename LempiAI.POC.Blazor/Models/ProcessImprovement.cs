namespace LempiAI.POC.Blazor.Models;

public class ProcessImprovement
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ProcessArea { get; set; } = null!;
    public string ImpactLevel { get; set; } = "Medium";
    public string Status { get; set; } = "Proposed";
    public string SuggestedActions { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
