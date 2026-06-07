using LempiAI.POC.Blazor.Components;
using LempiAI.POC.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Load configuration files
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Add services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


// Add HttpClient with debug logging
builder.Services.AddScoped(sp =>
{
    var apiUrl = builder.Configuration["ApiUrl"] ?? "https://localhost:7279/";
    Console.WriteLine($"[DEBUG] ApiUrl from config = {builder.Configuration["ApiUrl"]}");
    Console.WriteLine($"[DEBUG] Using BaseAddress = {apiUrl}");

    return new HttpClient
    {
        BaseAddress = new Uri(apiUrl)
    };
});

// Register ApiService
builder.Services.AddScoped<ApiService>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
