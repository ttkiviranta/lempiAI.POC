using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LempiAI.POC.API.Services;

public interface IOperationsSchedulingAgent
{
    Task<string> OptimizeScheduleAsync(int employeeId);
    Task<string> GetScheduleRecommendationsAsync();
}

public class OperationsSchedulingAgent : IOperationsSchedulingAgent
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAzureOpenAIService _openAIService;
    private readonly ILogger<OperationsSchedulingAgent> _logger;

    public OperationsSchedulingAgent(
        ApplicationDbContext dbContext,
        IAzureOpenAIService openAIService,
        ILogger<OperationsSchedulingAgent> logger)
    {
        _dbContext = dbContext;
        _openAIService = openAIService;
        _logger = logger;
    }

    public async Task<string> OptimizeScheduleAsync(int employeeId)
    {
        try
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Schedules)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
            {
                return "Employee not found";
            }

            var scheduleData = string.Join("; ", employee.Schedules
                .Select(s => $"{s.TaskDescription} ({s.ScheduledStart:yyyy-MM-dd HH:mm} - {s.ScheduledEnd:yyyy-MM-dd HH:mm})"));

            var prompt = $"Analyze and optimize the following schedule for employee {employee.Name}: {scheduleData}. Provide specific recommendations.";

            var analysis = await _openAIService.GetAnalysisAsync(prompt, "scheduling");
            
            var report = new AgentReport
            {
                AgentName = "Operations Scheduling Agent",
                Title = $"Schedule Optimization for {employee.Name}",
                Content = scheduleData,
                KeyFindings = analysis,
                ReportType = "Recommendation",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            _dbContext.AgentReports.Add(report);
            await _dbContext.SaveChangesAsync();

            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error optimizing schedule");
            throw;
        }
    }

    public async Task<string> GetScheduleRecommendationsAsync()
    {
        try
        {
            var allSchedules = await _dbContext.Schedules
                .Include(s => s.Employee)
                .ToListAsync();

            var scheduleOverview = $"Total schedules: {allSchedules.Count}. Pending: {allSchedules.Count(s => s.Status == "Pending")}. High priority: {allSchedules.Count(s => s.Priority == "High" || s.Priority == "Critical")}";

            var prompt = $"Analyze this schedule overview and provide workflow management recommendations: {scheduleOverview}";

            var recommendations = await _openAIService.GetAnalysisAsync(prompt, "scheduling");
            
            return recommendations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule recommendations");
            throw;
        }
    }
}
