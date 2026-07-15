using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Capacity.Services;
using WbsTool.Api.Modules.Competencies.Services;
using WbsTool.Api.Modules.Deliverables.Services;
using WbsTool.Api.Modules.Persons.Services;
using WbsTool.Api.Modules.ProcessPhases.Services;
using WbsTool.Api.Modules.Projects.Services;
using WbsTool.Api.Modules.RateCategories.Services;
using WbsTool.Api.Modules.Risks.Services;
using WbsTool.Api.Modules.ResourceDemands.Services;
using WbsTool.Api.Modules.Seed.Services;
using WbsTool.Api.Modules.TaskStatuses.Services;
using WbsTool.Api.Modules.Wbs.Services;

/// <summary>
/// WBS Tool - Webbasiertes datenbankgestütztes WBS-Management System
/// 
/// Hauptmodule:
/// - Projects: Projektverwaltung
/// - Wbs: Work Breakdown Structure (Baumstruktur mit konsolidierten Metriken)
/// - ResourceAssignments: Ressourcen zu WBS-Knoten Zuordnung
/// - Competencies: Kompetenzkatalog und Zuordnungen
/// - ProcessPhases: Leistungsphasen (LPH)
/// - Persons: Ressourcen-Stammdaten
/// - Capacity: Kapazitätsplanung
/// - Risks, Deliverables, ResourceDemands: Management-Aufmerksamkeit Features
/// 
/// Wichtig: API ist Single Source of Business Logic - kein Business Logic im Frontend!
/// </summary>

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

/// CORS-Konfiguration für Frontend-Entwicklung
/// Erlaubt Zugriff von lokalen React Dev Server (Ports 5173-5175)
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

/// Datenbankkontext-Konfiguration
/// SQLite in Development, Azure SQL später möglich
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

/// ===== DEPENDENCY INJECTION - Service Registrierung =====
/// Registriert alle Business Logic Services als Scoped (neue Instanz pro Request)

/// Projects & Dashboard Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IManagementAttentionService, ManagementAttentionService>();
builder.Services.AddScoped<IProjectDashboardService, ProjectDashboardService>();

/// WBS & Ressourcen-Zuordnung Services (Kernmodule)
builder.Services.AddScoped<IWbsService, WbsService>();
builder.Services.AddScoped<IResourceAssignmentService, ResourceAssignmentService>();

/// Stammdaten Services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IRateCategoryService, RateCategoryService>();
builder.Services.AddScoped<ITaskStatusService, TaskStatusService>();

/// Spezielle Module
builder.Services.AddScoped<IResourceDemandService, ResourceDemandService>();
builder.Services.AddScoped<ICapacityAllocationService, CapacityAllocationService>();
builder.Services.AddScoped<IRiskService, RiskService>();
builder.Services.AddScoped<IDeliverableService, DeliverableService>();

/// Kompetenzen & Prozesse
builder.Services.AddScoped<ICompetencyService, CompetencyService>();
builder.Services.AddScoped<IProcessPhaseService, ProcessPhaseService>();

/// Seed Service für Referenzdaten (nur Development)
builder.Services.AddScoped<IAmprionPqSeedService, AmprionPqSeedService>();

var app = builder.Build();

/// ===== MIDDLEWARE PIPELINE =====

/// OpenAPI/Swagger Documentation (nur in Development)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

/// CORS Middleware aktivieren
app.UseCors("Frontend");

/// Map alle Controller Routes
app.MapControllers();

/// Start der Anwendung auf konfiguriertem Port (default: 5046)
app.Run();