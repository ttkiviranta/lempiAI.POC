# lempiAI.POC - Proof of Concept for Cleaning Company AI Agents

A comprehensive proof of concept system demonstrating three AI agents for optimizing cleaning company operations using .NET 8, Azure SQL, Azure OpenAI, and Blazor Server.

## Overview

lempiAI.POC showcases how artificial intelligence can be integrated into a cleaning service company to:
- **Operatiivinen aikatauluagentti** - Optimize employee scheduling based on operational data
- **Asiakaspalvelu- ja palautteiden analyysiagentti** - Analyze customer feedback and sentiment
- **Prosessikehitys- ja raportointiagentti** - Identify process improvements and generate reports

## Technology Stack

- **Backend**: .NET 8 Web API with ASP.NET Core
- **Frontend**: Blazor Server with Finnish UI
- **Database**: Azure SQL with Entity Framework Core
- **AI/ML**: Azure OpenAI GPT-4o-mini
- **Architecture**: RESTful API with dependency injection
- **Deployment**: Azure App Service + Azure SQL Database

## Project Structure

```
lempiAI.POC/
├── LempiAI.POC.API/                   # Backend API project
│   ├── Models/                         # Database models
│   │   ├── Employee.cs
│   │   ├── Schedule.cs
│   │   ├── Feedback.cs
│   │   ├── ProcessImprovement.cs
│   │   └── AgentReport.cs
│   ├── Services/                       # Business logic and AI agents
│   │   ├── AzureOpenAIService.cs
│   │   ├── OperationsSchedulingAgent.cs
│   │   ├── CustomerServiceAgent.cs
│   │   ├── ProcessImprovementAgent.cs
│   │   └── SeedDataService.cs
│   ├── Controllers/                    # REST API endpoints
│   │   ├── EmployeeController.cs
│   │   ├── OperationsSchedulingController.cs
│   │   ├── CustomerServiceController.cs
│   │   └── ProcessImprovementController.cs
│   ├── Data/
│   │   ├── ApplicationDbContext.cs    # EF Core context
│   │   └── Migrations/
│   └── Program.cs                      # API configuration
├── LempiAI.POC.Blazor/                # Frontend project (Blazor Server)
│   ├── Components/
│   │   ├── Pages/
│   │   │   ├── Home.razor
│   │   │   ├── Aikataulut.razor      # Schedules page
│   │   │   ├── Palautteet.razor      # Feedback page
│   │   │   ├── Tyontekijat.razor     # Employees page
│   │   │   └── Raportit.razor        # Reports page
│   │   └── App.razor
│   ├── Services/
│   │   └── ApiService.cs             # HTTP client for API
│   └── Program.cs                      # Blazor configuration
├── database-migration.sql              # EF Core generated SQL script
├── azure-sql-setup.sql                 # Azure SQL setup instructions
└── README.md                           # This file
```

## Prerequisites

- .NET 8 SDK or later
- Azure subscription (for Azure SQL and Azure OpenAI)
- Visual Studio 2026 Community (or VS Code)
- SQL Server Management Studio (optional, for database management)

## Local Development Setup

### 1. Clone and Restore

```bash
cd lempiAI.POC
dotnet restore
```

### 2. Configure Local Database

Update `LempiAI.POC.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=LempiAI_POC;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "AzureOpenAI": {
    "Endpoint": "",
    "ApiKey": "",
    "DeploymentId": "gpt-4o-mini",
    "ApiVersion": "2024-08-01-preview"
  }
}
```

### 3. Create and Migrate Database

```bash
cd LempiAI.POC.API
dotnet ef database update
```

This will:
- Create the database schema
- Run migrations
- Seed initial data (employees, schedules, feedback, improvements)

### 4. Run API

```bash
cd LempiAI.POC.API
dotnet run
```

API runs on `https://localhost:5001` with Swagger documentation at `/swagger`

### 5. Run Blazor UI

In a new terminal:

```bash
cd LempiAI.POC.Blazor
dotnet run
```

Blazor UI runs on `https://localhost:7001`

## API Endpoints

### Employees
- `GET /api/employee` - Get all employees
- `GET /api/employee/{id}` - Get employee by ID
- `POST /api/employee` - Create employee
- `PUT /api/employee/{id}` - Update employee
- `DELETE /api/employee/{id}` - Delete employee

### Operations Scheduling Agent
- `GET /api/operationsscheduling/schedules` - Get all schedules
- `POST /api/operationsscheduling/create` - Create schedule
- `POST /api/operationsscheduling/optimize/{employeeId}` - Optimize schedule
- `GET /api/operationsscheduling/recommendations` - Get AI recommendations

### Customer Service & Feedback Analysis Agent
- `GET /api/customerservice/all-feedback` - Get all feedback
- `POST /api/customerservice/submit-feedback` - Submit feedback
- `POST /api/customerservice/analyze-feedback` - Analyze all feedback
- `GET /api/customerservice/sentiment-summary` - Get sentiment analysis

### Process Development & Reporting Agent
- `GET /api/processimprovement/all-improvements` - Get all improvements
- `POST /api/processimprovement/identify-improvements` - Identify improvements
- `POST /api/processimprovement/generate-report` - Generate report
- `GET /api/processimprovement/reports` - Get all reports

## Azure Deployment

### 1. Azure SQL Database Setup

Follow `azure-sql-setup.sql` to:
- Create Azure SQL Database
- Create application user
- Run migrations

### 2. Azure OpenAI Configuration

1. Create Azure OpenAI resource in Azure Portal
2. Deploy GPT-4o-mini model
3. Get Endpoint and API Key
4. Update configuration in Azure App Service

### 3. Publish to Azure

#### Create App Service and SQL Database

```bash
az group create --name lempiAI-rg --location eastus

az appservice plan create \
  --name lempiAI-plan \
  --resource-group lempiAI-rg \
  --sku B1 --is-linux

az webapp create \
  --resource-group lempiAI-rg \
  --plan lempiAI-plan \
  --name lempiai-api \
  --runtime "DOTNETCORE|8.0"

az sql server create \
  --name lempiaisqlserver \
  --resource-group lempiAI-rg \
  --admin-user sqladmin \
  --admin-password YourSecurePassword123!

az sql db create \
  --server lempiaisqlserver \
  --name LempiAI_POC \
  --resource-group lempiAI-rg \
  --tier Basic
```

#### Publish API

```bash
cd LempiAI.POC.API
dotnet publish -c Release
az webapp up \
  --resource-group lempiAI-rg \
  --name lempiai-api \
  --plan lempiAI-plan
```

#### Configure Connection String and Secrets

```bash
az webapp config appsettings set \
  --resource-group lempiAI-rg \
  --name lempiai-api \
  --settings ConnectionStrings__DefaultConnection="<your-connection-string>" \
            AzureOpenAI__Endpoint="<your-endpoint>" \
            AzureOpenAI__ApiKey="<your-api-key>"
```

#### Deploy Blazor Frontend

Similar process for the Blazor application.

## Testing

### Test with cURL

```bash
# Get all employees
curl -X GET https://localhost:5001/api/employee

# Get AI recommendations
curl -X GET https://localhost:5001/api/operationsscheduling/recommendations

# Submit feedback
curl -X POST https://localhost:5001/api/customerservice/submit-feedback \
  -H "Content-Type: application/json" \
  -d '{
    "providedByEmployeeId": 1,
    "rating": 5,
    "feedbackText": "Great service!",
    "category": "Service Quality",
    "status": "Received"
  }'

# Analyze all feedback
curl -X POST https://localhost:5001/api/customerservice/analyze-feedback

# Generate operational report
curl -X POST https://localhost:5001/api/processimprovement/generate-report
```

### Swagger Documentation

Open Swagger UI at `https://localhost:5001/swagger` to test all endpoints interactively.

## Database Models

### Employee
- ID, Name, Email, Role, PhoneNumber
- Relationships: Schedules, ProvidedFeedback

### Schedule
- ID, EmployeeId, TaskDescription, ScheduledStart, ScheduledEnd
- Status (Pending, InProgress, Completed, Cancelled)
- Priority (Low, Medium, High, Critical)

### Feedback
- ID, ProvidedByEmployeeId, Rating (1-5)
- Category (Service Quality, Professionalism, Punctuality, Communication, Other)
- Status (Received, Under Review, Addressed, Archived)

### ProcessImprovement
- ID, Title, Description, ProcessArea
- ImpactLevel (Low, Medium, High, Critical)
- Status (Proposed, In Progress, Implemented, Rejected)

### AgentReport
- ID, AgentName, Title, Content, KeyFindings
- ReportType (Analysis, Recommendation, Summary, Alert)

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "connection-string-here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AzureOpenAI": {
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key",
    "DeploymentId": "gpt-4o-mini",
    "ApiVersion": "2024-08-01-preview"
  },
  "AllowedHosts": "*"
}
```

## AI Agent Descriptions

### 1. Operations Scheduling Agent
Optimizes employee work schedules based on:
- Current schedule data
- Employee preferences
- Geographic location
- Workload distribution

Provides recommendations for:
- Optimal task assignments
- Travel time optimization
- Schedule conflict resolution
- Resource allocation

### 2. Customer Service & Feedback Analysis Agent
Analyzes customer feedback to:
- Determine customer satisfaction
- Identify common issues
- Extract actionable insights
- Generate sentiment scores

Outputs:
- Feedback summaries
- Trend analysis
- Improvement suggestions
- Quality metrics

### 3. Process Development & Reporting Agent
Reviews operational metrics to:
- Identify bottlenecks
- Suggest efficiency improvements
- Generate operational reports
- Monitor KPIs

Generates:
- Monthly reports
- Process improvement recommendations
- Performance analytics
- Strategic insights

## Troubleshooting

### Database Connection Issues
- Verify connection string in appsettings.json
- Ensure SQL Server is running
- Check firewall rules (for Azure SQL)

### Azure OpenAI Errors
- Verify API endpoint format (include trailing slash)
- Confirm API key is correct
- Check model deployment exists (gpt-4o-mini)
- Monitor API quota and rate limits

### CORS Issues
- Verify CORS policy in Program.cs
- Check allowed origins match your Blazor URL

## Development Guidelines

- **Code Comments**: All in English
- **Git Commits**: All messages in English
- **UI/UX**: Finnish language for end users
- **Logging**: Use ILogger<T> throughout
- **Error Handling**: Return appropriate HTTP status codes
- **Security**: Store sensitive data in configuration, never hardcode

## Future Enhancements

- [ ] Authentication and authorization (Azure Entra ID)
- [ ] Advanced reporting with charts (Chart.js integration)
- [ ] Real-time notifications (SignalR)
- [ ] Mobile app support (React Native or Flutter)
- [ ] Additional AI model support (GPT-4, Claude)
- [ ] Machine learning model training pipeline
- [ ] Data visualization dashboards
- [ ] Email notifications and alerts
- [ ] Role-based access control
- [ ] Audit logging

## License

This project is a Proof of Concept and is provided as-is for educational and demonstration purposes.

## Contact

For questions or support regarding this POC, please contact the development team.

## References

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/ai-services/openai/overview)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-server)
- [Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/)
