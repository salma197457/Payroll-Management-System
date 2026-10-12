using Hangfire;
using Hangfire.SqlServer;
using Payroll.Api.ExceptionHandling;
using Payroll.Api.Jobs;
using Payroll.Application.Interfaces.ServiceIntefaces;
using Payroll.Application.Services;
using Payroll.Infrastructure;
using Payroll.Application;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Exceptions handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);



//hangfire

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string is missing.");

builder.Services.AddHangfire(configuration =>
    configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<MonthlyPayrollJob>();

var app = builder.Build();

//run job
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<MonthlyPayrollJob>(
    "monthly-payroll-generation",
    job => job.ExecuteAsync(),
    "0 1 1 * *",
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Utc
    });

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire");

app.Run();
