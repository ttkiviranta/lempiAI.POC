namespace LempiAI.POC.Blazor.Models;

public class AgentReport
{
    public int Id { get; set; }
    public string AgentName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string KeyFindings { get; set; } = null!;
    public string ReportType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
