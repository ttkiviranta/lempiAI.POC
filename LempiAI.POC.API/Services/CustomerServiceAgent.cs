using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LempiAI.POC.API.Services;

public interface ICustomerServiceAgent
{
    Task<string> AnalyzeFeedbackAsync();
    Task<string> GetSentimentSummaryAsync();
}

public class CustomerServiceAgent : ICustomerServiceAgent
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAzureOpenAIService _openAIService;
    private readonly ILogger<CustomerServiceAgent> _logger;

    public CustomerServiceAgent(
        ApplicationDbContext dbContext,
        IAzureOpenAIService openAIService,
        ILogger<CustomerServiceAgent> logger)
    {
        _dbContext = dbContext;
        _openAIService = openAIService;
        _logger = logger;
    }

    public async Task<string> AnalyzeFeedbackAsync()
    {
        try
        {
            var allFeedback = await _dbContext.Feedback
                .Include(f => f.ProvidedByEmployee)
                .ToListAsync();

            var feedbackSummary = string.Join("; ", allFeedback.Select(f =>
                $"Rating: {f.Rating}/5, Category: {f.Category}, Comment: {f.FeedbackText.Substring(0, Math.Min(50, f.FeedbackText.Length))}"));

            var prompt = $"Analyze the following customer feedback and provide actionable insights: {feedbackSummary}";

            var analysis = await _openAIService.GetAnalysisAsync(prompt, "feedback");

            var report = new AgentReport
            {
                AgentName = "Customer Service & Feedback Analysis Agent",
                Title = "Feedback Analysis Report",
                Content = feedbackSummary,
                KeyFindings = analysis,
                ReportType = "Analysis",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.AgentReports.Add(report);
            await _dbContext.SaveChangesAsync();

            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing feedback");
            throw;
        }
    }

    public async Task<string> GetSentimentSummaryAsync()
    {
        try
        {
            var allFeedback = await _dbContext.Feedback.ToListAsync();

            var averageRating = allFeedback.Any() ? allFeedback.Average(f => f.Rating) : 0;
            var totalFeedback = allFeedback.Count;
            var byCategory = string.Join(", ", allFeedback
                .GroupBy(f => f.Category)
                .Select(g => $"{g.Key}: {g.Count()}"));

            var prompt = $"Summarize customer sentiment based on: Average Rating: {averageRating:F1}/5, Total: {totalFeedback}, Categories: {byCategory}";

            var summary = await _openAIService.GetCompletionAsync(prompt);

            return summary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sentiment summary");
            throw;
        }
    }
}
