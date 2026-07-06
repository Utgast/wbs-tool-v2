using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Competencies.Services;
using WbsTool.Api.Modules.Persons.Services;
using WbsTool.Api.Modules.ProcessPhases.Services;
using WbsTool.Api.Modules.Projects.Services;
using WbsTool.Api.Modules.RateCategories.Services;
using WbsTool.Api.Modules.ResourceDemands.Services;
using WbsTool.Api.Modules.Seed.Services;
using WbsTool.Api.Modules.TaskStatuses.Services;
using WbsTool.Api.Modules.Wbs.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:5175"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectDashboardService, ProjectDashboardService>();
builder.Services.AddScoped<IWbsService, WbsService>();
builder.Services.AddScoped<IResourceAssignmentService, ResourceAssignmentService>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IRateCategoryService, RateCategoryService>();
builder.Services.AddScoped<ITaskStatusService, TaskStatusService>();
builder.Services.AddScoped<IResourceDemandService, ResourceDemandService>();
builder.Services.AddScoped<ICompetencyService, CompetencyService>();
builder.Services.AddScoped<IProcessPhaseService, ProcessPhaseService>();

builder.Services.AddScoped<IAmprionPqSeedService, AmprionPqSeedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");

app.MapControllers();

app.Run();