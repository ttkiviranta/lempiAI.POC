using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LempiAI.POC.API.Services;

public interface IProcessImprovementAgent
{
    Task<string> IdentifyImprovementsAsync();
    Task<string> GenerateReportAsync();
}

public class ProcessImprovementAgent : IProcessImprovementAgent
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAzureOpenAIService _openAIService;
    private readonly ILogger<ProcessImprovementAgent> _logger;

    public ProcessImprovementAgent(
        ApplicationDbContext dbContext,
        IAzureOpenAIService openAIService,
        ILogger<ProcessImprovementAgent> logger)
    {
        _dbContext = dbContext;
        _openAIService = openAIService;
        _logger = logger;
    }

    public async Task<string> IdentifyImprovementsAsync()
    {
        try
        {
            var employees = await _dbContext.Employees.CountAsync();
            var schedules = await _dbContext.Schedules.CountAsync();
            var feedback = await _dbContext.Feedback.ToListAsync();
            var highPriorityTasks = await _dbContext.Schedules
                .Where(s => s.Priority == "High" || s.Priority == "Critical")
                .CountAsync();

            var avgRating = feedback.Any() ? feedback.Average(f => f.Rating).ToString("F1") : "N/A";
            var dataOverview = $"Company: {employees} employees, {schedules} tasks, {feedback.Count} feedback items, {highPriorityTasks} high-priority tasks, avg rating: {avgRating}";

            var prompt = $"Identify key process improvements based on: {dataOverview}. Provide actionable recommendations.";

            var improvements = await _openAIService.GetAnalysisAsync(prompt, "process");

            var improvementEntity = new ProcessImprovement
            {
                Title = "AI-Generated Process Improvement Recommendations",
                Description = dataOverview,
                ProcessArea = "General Operations",
                ImpactLevel = "High",
                Status = "Proposed",
                SuggestedActions = improvements,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.ProcessImprovements.Add(improvementEntity);
            await _dbContext.SaveChangesAsync();

            return improvements;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error identifying improvements");
            throw;
        }
    }

    public async Task<string> GenerateReportAsync()
    {
        try
        {
            var employees = await _dbContext.Employees.CountAsync();
            var completedSchedules = await _dbContext.Schedules
                .Where(s => s.Status == "Completed")
                .CountAsync();
            var totalSchedules = await _dbContext.Schedules.CountAsync();
            var recentFeedback = await _dbContext.Feedback
                .Where(f => f.CreatedAt > DateTime.UtcNow.AddDays(-30))
                .CountAsync();

            var reportData = $"Active Employees: {employees}, Completed Tasks: {completedSchedules}/{totalSchedules}, Recent Feedback (30 days): {recentFeedback}";

            var prompt = $"Generate a comprehensive operational report based on: {reportData}";

            var report = await _openAIService.GetCompletionAsync(prompt, 2000);

            var agentReport = new AgentReport
            {
                AgentName = "Process Development & Reporting Agent",
                Title = "Monthly Operational Report",
                Content = reportData,
                KeyFindings = report,
                ReportType = "Summary",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.AgentReports.Add(agentReport);
            await _dbContext.SaveChangesAsync();

            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating report");
            throw;
        }
    }
}
