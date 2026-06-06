using Microsoft.AspNetCore.Mvc;
using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;

namespace LempiAI.POC.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(ApplicationDbContext dbContext, ILogger<EmployeeController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllEmployees()
    {
        try
        {
            var employees = _dbContext.Employees.ToList();
            return Ok(new { success = true, data = employees });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employees");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        try
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound(new { success = false, error = "Employee not found" });
            return Ok(new { success = true, data = employee });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        try
        {
            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return Ok(new { success = true, data = employee });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employeeUpdate)
    {
        try
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound(new { success = false, error = "Employee not found" });
            
            employee.Name = employeeUpdate.Name;
            employee.Email = employeeUpdate.Email;
            employee.Role = employeeUpdate.Role;
            employee.PhoneNumber = employeeUpdate.PhoneNumber;
            employee.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
            return Ok(new { success = true, data = employee });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound(new { success = false, error = "Employee not found" });
            
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return Ok(new { success = true, message = "Employee deleted" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}
