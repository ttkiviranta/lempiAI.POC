namespace LempiAI.POC.API.Services;

public interface IAzureOpenAIService
{
    Task<string> GetCompletionAsync(string prompt, int maxTokens = 1000);
    Task<string> GetAnalysisAsync(string content, string analysisType);
}

public class AzureOpenAIService : IAzureOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;
    private readonly string _apiKey;
    private readonly string _deploymentId;
    private readonly ILogger<AzureOpenAIService> _logger;

    public AzureOpenAIService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<AzureOpenAIService> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        
        _endpoint = configuration["AzureOpenAI:Endpoint"] ?? "";
        _apiKey = configuration["AzureOpenAI:ApiKey"] ?? "";
        _deploymentId = configuration["AzureOpenAI:DeploymentId"] ?? "gpt-4o-mini";
        
        if (string.IsNullOrEmpty(_endpoint) || string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogWarning("Azure OpenAI configuration is missing. Using mock responses.");
        }
    }

    public async Task<string> GetCompletionAsync(string prompt, int maxTokens = 1000)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            return $"Mock completion for: {prompt}";
        }
        
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_endpoint}/openai/deployments/{_deploymentId}/chat/completions?api-version=2024-08-01-preview");
            request.Headers.Add("api-key", _apiKey);
            request.Content = new StringContent(
                $$"""
                {
                    "messages": [{"role": "user", "content": "{{prompt}}"}],
                    "max_tokens": {{maxTokens}},
                    "temperature": 0.7
                }
                """, 
                System.Text.Encoding.UTF8, 
                "application/json");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return ExtractContent(content);
            }
            
            _logger.LogWarning($"Azure OpenAI API error: {content}");
            return "Error calling Azure OpenAI";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Azure OpenAI");
            return $"Error: {ex.Message}";
        }
    }

    public async Task<string> GetAnalysisAsync(string content, string analysisType)
    {
        var systemPrompt = analysisType switch
        {
            "scheduling" => "You are an operations scheduling expert for a cleaning company. Analyze the provided data and suggest optimal scheduling.",
            "feedback" => "You are a customer service specialist. Analyze the provided feedback and extract key insights and recommendations.",
            "process" => "You are a process improvement consultant. Analyze the provided information and suggest process improvements.",
            _ => "You are a business analyst. Analyze the provided data and provide insights."
        };

        if (string.IsNullOrEmpty(_apiKey))
        {
            return $"Mock analysis ({analysisType}): {content.Substring(0, Math.Min(100, content.Length))}...";
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_endpoint}/openai/deployments/{_deploymentId}/chat/completions?api-version=2024-08-01-preview");
            request.Headers.Add("api-key", _apiKey);
            request.Content = new StringContent(
                $$"""
                {
                    "messages": [
                        {"role": "system", "content": "{{systemPrompt}}"},
                        {"role": "user", "content": "{{content}}"}
                    ],
                    "max_tokens": 2000,
                    "temperature": 0.7
                }
                """, 
                System.Text.Encoding.UTF8, 
                "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return ExtractContent(responseContent);
            }
            
            _logger.LogWarning($"Azure OpenAI API error: {responseContent}");
            return "Error calling Azure OpenAI";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing analysis");
            return $"Error: {ex.Message}";
        }
    }

    private string ExtractContent(string jsonResponse)
    {
        try
        {
            var startIndex = jsonResponse.IndexOf("\"content\":\"") + 11;
            var endIndex = jsonResponse.IndexOf("\"", startIndex);
            return jsonResponse.Substring(startIndex, endIndex - startIndex);
        }
        catch
        {
            return jsonResponse;
        }
    }
}
