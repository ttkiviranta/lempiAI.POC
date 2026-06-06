using Microsoft.AspNetCore.Mvc;
using LempiAI.POC.API.Services;
using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;

namespace LempiAI.POC.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessImprovementController : ControllerBase
{
    private readonly IProcessImprovementAgent _agent;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProcessImprovementController> _logger;

    public ProcessImprovementController(
        IProcessImprovementAgent agent,
        ApplicationDbContext dbContext,
        ILogger<ProcessImprovementController> logger)
    {
        _agent = agent;
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost("identify-improvements")]
    public async Task<IActionResult> IdentifyImprovements()
    {
        try
        {
            var result = await _agent.IdentifyImprovementsAsync();
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error identifying improvements");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpPost("generate-report")]
    public async Task<IActionResult> GenerateReport()
    {
        try
        {
            var result = await _agent.GenerateReportAsync();
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating report");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("all-improvements")]
    public IActionResult GetAllImprovements()
    {
        try
        {
            var improvements = _dbContext.ProcessImprovements.ToList();
            return Ok(new { success = true, data = improvements });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting improvements");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("reports")]
    public IActionResult GetAgentReports()
    {
        try
        {
            var reports = _dbContext.AgentReports.ToList();
            return Ok(new { success = true, data = reports });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reports");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}
