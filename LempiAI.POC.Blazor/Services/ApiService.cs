using System.Net.Http.Json;
using System.Text.Json;
using LempiAI.POC.Blazor.Models;
using Microsoft.Extensions.Logging;

namespace LempiAI.POC.Blazor.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _logger.LogInformation($"[ApiService] BaseAddress = {_httpClient.BaseAddress}");
    }

    // -----------------------------
    // Generic GET with ApiResponse<T>
    // -----------------------------
    private async Task<T> GetWrappedAsync<T>(string url)
    {
        _logger.LogInformation($"GET {url}");
        var response = await _httpClient.GetAsync(url);
        var raw = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"RAW RESPONSE: {raw}");

        var parsed = JsonSerializer.Deserialize<ApiResponse<T>>(raw, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (parsed == null)
            throw new Exception("API returned invalid JSON");

        if (!parsed.Success)
            throw new Exception(parsed.Error ?? "Unknown API error");

        if (parsed.Data == null)
            throw new Exception(parsed.Error ?? "Unknown API error");

        return parsed.Data;
    }

    // -----------------------------
    // Generic POST with ApiResponse<T>
    // -----------------------------
    private async Task<T> PostWrappedAsync<T>(string url, object payload)
    {
        _logger.LogInformation($"POST {url}");
        var response = await _httpClient.PostAsJsonAsync(url, payload);
        var raw = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"RAW RESPONSE: {raw}");

        var parsed = JsonSerializer.Deserialize<ApiResponse<T>>(raw, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (parsed == null)
            throw new Exception("API returned invalid JSON");

        if (!parsed.Success)
            throw new Exception(parsed.Error ?? "Unknown API error");

        if (parsed.Data == null)
            throw new Exception(parsed.Error ?? "Unknown API error");

        return parsed.Data;
    }

    // -----------------------------
    // Employee endpoints
    // -----------------------------
    public Task<List<Employee>> GetEmployeesAsync()
        => GetWrappedAsync<List<Employee>>("api/employee");

    public Task<Employee> CreateEmployeeAsync(Employee employee)
        => PostWrappedAsync<Employee>("api/employee", employee);

    // -----------------------------
    // Schedule endpoints
    // -----------------------------
    public Task<List<Schedule>> GetSchedulesAsync()
        => GetWrappedAsync<List<Schedule>>("api/operationsscheduling/schedules");

    public Task<Schedule> CreateScheduleAsync(Schedule schedule)
        => PostWrappedAsync<Schedule>("api/operationsscheduling/create", schedule);

    // -----------------------------
    // Feedback endpoints
    // -----------------------------
    public Task<List<Feedback>> GetFeedbackAsync()
        => GetWrappedAsync<List<Feedback>>("api/customerservice/all-feedback");

    public Task<Feedback> SubmitFeedbackAsync(Feedback feedback)
        => PostWrappedAsync<Feedback>("api/customerservice/submit-feedback", feedback);

    // -----------------------------
    // Process improvement endpoints
    // -----------------------------
    public Task<List<ProcessImprovement>> GetImprovementsAsync()
        => GetWrappedAsync<List<ProcessImprovement>>("api/processimprovement/all-improvements");

    public Task<ProcessImprovement> CreateImprovementAsync(ProcessImprovement improvement)
        => PostWrappedAsync<ProcessImprovement>("api/processimprovement/create", improvement);

    // -----------------------------
    // Agent report endpoints
    // -----------------------------
    public Task<List<AgentReport>> GetAgentReportsAsync()
        => GetWrappedAsync<List<AgentReport>>("api/processimprovement/reports");
}
