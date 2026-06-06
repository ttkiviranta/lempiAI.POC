using LempiAI.POC.API.Data;
using LempiAI.POC.API.Models;

namespace LempiAI.POC.API.Services;

public static class SeedDataService
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Employees.Any())
        {
            return; // Already seeded
        }

        var employees = new List<Employee>
        {
            new Employee
            {
                Name = "Matti Meikäläinen",
                Email = "matti.meikaalainen@example.com",
                Role = "Siivoja",
                PhoneNumber = "+358 10 123 4567",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Employee
            {
                Name = "Liisa Lintunen",
                Email = "liisa.lintunen@example.com",
                Role = "Esimies",
                PhoneNumber = "+358 10 765 4321",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Employee
            {
                Name = "Kari Karvonen",
                Email = "kari.karvonen@example.com",
                Role = "Siivoja",
                PhoneNumber = "+358 10 456 7890",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Employee
            {
                Name = "Anna Ahonen",
                Email = "anna.ahonen@example.com",
                Role = "Siivoja",
                PhoneNumber = "+358 10 321 6543",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();

        var schedules = new List<Schedule>
        {
            new Schedule
            {
                EmployeeId = 1,
                TaskDescription = "Toimiston siivoaminen",
                ScheduledStart = DateTime.UtcNow.AddDays(1),
                ScheduledEnd = DateTime.UtcNow.AddDays(1).AddHours(2),
                Status = "Pending",
                Priority = "High",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Schedule
            {
                EmployeeId = 2,
                TaskDescription = "Käytävöiden siivous ja lattian pesu",
                ScheduledStart = DateTime.UtcNow.AddDays(2),
                ScheduledEnd = DateTime.UtcNow.AddDays(2).AddHours(3),
                Status = "Pending",
                Priority = "Medium",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Schedule
            {
                EmployeeId = 3,
                TaskDescription = "WC-tilojen puhdistus",
                ScheduledStart = DateTime.UtcNow.AddDays(1),
                ScheduledEnd = DateTime.UtcNow.AddDays(1).AddHours(1),
                Status = "Completed",
                Priority = "Critical",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        context.Schedules.AddRange(schedules);
        await context.SaveChangesAsync();

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                ProvidedByEmployeeId = 1,
                Rating = 5,
                FeedbackText = "Hyvä siivouspalvelu, hyvin tehtävä. Ammattitaitoinen henkilökunta.",
                Category = "Service Quality",
                Status = "Received",
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = DateTime.UtcNow.AddDays(-7)
            },
            new Feedback
            {
                ProvidedByEmployeeId = 2,
                Rating = 4,
                FeedbackText = "Palvelu oli ajoissa ja yleisesti hyvin suoritettu. Voisi vielä kehittyä yksityiskohdissa.",
                Category = "Professionalism",
                Status = "Received",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Feedback
            {
                ProvidedByEmployeeId = 3,
                Rating = 5,
                FeedbackText = "Erittäin hyvä viestintä ja ammattitaito. Suosittelisin muille.",
                Category = "Communication",
                Status = "Received",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Feedback
            {
                ProvidedByEmployeeId = 4,
                Rating = 3,
                FeedbackText = "Palvelu olisi voinut olla paikallaan olevan ajan puitteissa nopeampaa.",
                Category = "Punctuality",
                Status = "Under Review",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            }
        };

        context.Feedback.AddRange(feedbacks);
        await context.SaveChangesAsync();

        var improvements = new List<ProcessImprovement>
        {
            new ProcessImprovement
            {
                Title = "Aikataulujen optimointi",
                Description = "Järjestelmä voisi paremmin optimoida työntekijöiden aikatauluja matkaajan aika-säästöjen perusteella.",
                ProcessArea = "Scheduling",
                ImpactLevel = "High",
                Status = "Proposed",
                SuggestedActions = "Toteuta algoritmi, joka optimoi reitit GPS-dataan perustuva. Vähennetään matka-aikaa 15-20%.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ProcessImprovement
            {
                Title = "Asiakaspalauteprosessin automatisointi",
                Description = "Automaattinen palauteprosessi voisi parantaa vasteaikoja.",
                ProcessArea = "Customer Service",
                ImpactLevel = "Medium",
                Status = "Proposed",
                SuggestedActions = "Ota käyttöön automaattinen sähköpostivastaus ja CRM-integraatio.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.ProcessImprovements.AddRange(improvements);
        await context.SaveChangesAsync();
    }
}
