using Microsoft.AspNetCore.Mvc;
using LempiAI.POC.API.Services;
using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;

namespace LempiAI.POC.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerServiceController : ControllerBase
{
    private readonly ICustomerServiceAgent _agent;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CustomerServiceController> _logger;

    public CustomerServiceController(
        ICustomerServiceAgent agent,
        ApplicationDbContext dbContext,
        ILogger<CustomerServiceController> logger)
    {
        _agent = agent;
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost("analyze-feedback")]
    public async Task<IActionResult> AnalyzeFeedback()
    {
        try
        {
            var result = await _agent.AnalyzeFeedbackAsync();
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing feedback");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("sentiment-summary")]
    public async Task<IActionResult> GetSentimentSummary()
    {
        try
        {
            var result = await _agent.GetSentimentSummaryAsync();
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sentiment summary");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("all-feedback")]
    public IActionResult GetAllFeedback()
    {
        try
        {
            var feedback = _dbContext.Feedback.ToList();
            return Ok(new { success = true, data = feedback });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting feedback");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpPost("submit-feedback")]
    public async Task<IActionResult> SubmitFeedback([FromBody] Feedback feedback)
    {
        try
        {
            feedback.CreatedAt = DateTime.UtcNow;
            feedback.UpdatedAt = DateTime.UtcNow;
            _dbContext.Feedback.Add(feedback);
            await _dbContext.SaveChangesAsync();
            return Ok(new { success = true, data = feedback });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting feedback");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}
