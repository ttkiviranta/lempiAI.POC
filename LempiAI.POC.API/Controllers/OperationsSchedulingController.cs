using Microsoft.AspNetCore.Mvc;
using LempiAI.POC.API.Services;
using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;

namespace LempiAI.POC.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationsSchedulingController : ControllerBase
{
    private readonly IOperationsSchedulingAgent _agent;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<OperationsSchedulingController> _logger;

    public OperationsSchedulingController(
        IOperationsSchedulingAgent agent,
        ApplicationDbContext dbContext,
        ILogger<OperationsSchedulingController> logger)
    {
        _agent = agent;
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost("optimize/{employeeId}")]
    public async Task<IActionResult> OptimizeSchedule(int employeeId)
    {
        try
        {
            var result = await _agent.OptimizeScheduleAsync(employeeId);
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error optimizing schedule");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations()
    {
        try
        {
            var result = await _agent.GetScheduleRecommendationsAsync();
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recommendations");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("schedules")]
    public IActionResult GetAllSchedules()
    {
        try
        {
            var schedules = _dbContext.Schedules.ToList();
            return Ok(new { success = true, data = schedules });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedules");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSchedule([FromBody] Schedule schedule)
    {
        try
        {
            _dbContext.Schedules.Add(schedule);
            await _dbContext.SaveChangesAsync();
            return Ok(new { success = true, data = schedule });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schedule");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}
