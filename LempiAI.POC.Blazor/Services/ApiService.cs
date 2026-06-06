namespace LempiAI.POC.Blazor.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Employee endpoints
    public async Task<T> GetEmployeesAsync<T>()
    {
        return await _httpClient.GetFromJsonAsync<T>("api/employee") ?? default!;
    }

    public async Task<T> GetEmployeeByIdAsync<T>(int id)
    {
        return await _httpClient.GetFromJsonAsync<T>($"api/employee/{id}") ?? default!;
    }

    public async Task<T> CreateEmployeeAsync<T>(object employee)
    {
        var response = await _httpClient.PostAsJsonAsync("api/employee", employee);
        return await response.Content.ReadFromJsonAsync<T>() ?? default!;
    }

    // Schedule endpoints
    public async Task<T> GetSchedulesAsync<T>()
    {
        return await _httpClient.GetFromJsonAsync<T>("api/operationsscheduling/schedules") ?? default!;
    }

    public async Task<T> CreateScheduleAsync<T>(object schedule)
    {
        var response = await _httpClient.PostAsJsonAsync("api/operationsscheduling/create", schedule);
        return await response.Content.ReadFromJsonAsync<T>() ?? default!;
    }

    public async Task<T> OptimizeScheduleAsync<T>(int employeeId)
    {
        return await _httpClient.GetFromJsonAsync<T>($"api/operationsscheduling/optimize/{employeeId}") ?? default!;
    }

    public async Task<T> GetScheduleRecommendationsAsync<T>()
    {
        return await _httpClient.GetFromJsonAsync<T>("api/operationsscheduling/recommendations") ?? default!;
    }

    // Feedback endpoints
    public async Task<T> GetFeedbackAsync<T>()
    {
        return await _httpClient.GetFromJsonAsync<T>("api/customerservice/all-feedback") ?? default!;
    }

    public async Task<T> SubmitFeedbackAsync<T>(object feedback)
    {
        var response = await _httpClient.PostAsJsonAsync("api/customerservice/submit-feedback", feedback);
        return await response.Content.ReadFromJsonAsync<T>() ?? default!;
    }

    public async Task<T> AnalyzeFeedbackAsync<T>()
    {
        var response = await _httpClient.PostAsync("api/customerservice/analyze-feedback", null);
        return await response.Content.ReadFromJsonAsync<T>() ?? default!;
    }

    public async Task<T> GetSentimentSummaryAsync<T>()
    {
        return await _httpClient.GetFromJsonAsync<T>("api/customerservice/sentiment-summary") ?? default!;
    }

    // Process improvement endpoints
    public async Task<T> GetImprovementsAsync<T>()
    {
        return await _httpClient.GetFromJsonAsync<T>("api/processimprovement/all-improvements") ?? default!;
    }

    public async Task<T> IdentifyImprovementsAsync<T>()
    {
        var response = await _httpClient.PostAsync("api/processimprovement/identify-improvements", null);
        return await response.Content.ReadFromJsonAsync<T>() ?? default!;
    }

    public async Task<T> GenerateReportAsync<T>()
    {
        var response = await _httpClient.PostAsync("api/processimprovement/generate-report", null);
        return await response.Content.ReadFromJsonAsync<T>() ?? default!;
    }

    public async Task<T> GetAgentReportsAsync<T>()
    {
        return await _httpClient.GetFromJsonAsync<T>("api/processimprovement/reports") ?? default!;
    }
}
