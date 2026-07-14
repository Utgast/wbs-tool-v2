using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Modules.Capacity.Models;
using WbsTool.Api.Modules.Competencies.Models;
using WbsTool.Api.Modules.Deliverables.Models;
using WbsTool.Api.Modules.Governance.Models;
using WbsTool.Api.Modules.ProcessPhases.Models;
using WbsTool.Api.Modules.Persons.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.RateCategories.Models;
using WbsTool.Api.Modules.Risks.Models;
using WbsTool.Api.Modules.ResourceDemands.Models;
using WbsTool.Api.Modules.Wbs.Models;
using WbsTaskStatus = WbsTool.Api.Modules.TaskStatuses.Models.TaskStatus;

namespace WbsTool.Api.Data;

/// <summary>
/// Entity Framework Core DbContext für WBS Tool
/// 
/// Verantwortung:
/// 1. Entity Konfiguration (Tabellenmapping, Indizes, Constraints)
/// 2. Relationship Configuration (Foreign Keys, Cascading)
/// 3. Seed Data für Master Data (Kompetenzen, Tarifkategorien, Personen)
/// 
/// WICHTIGE REGELN:
/// - WbsNode Unique Key: ProjectId + VisibleWbsId
/// - ResourceAssignments: Multiple pro WbsNode erlaubt
/// - Dashboard totals verwenden NUR WbsNode.PlannedHours/ActualHours (NICHT ResourceAssignments!)
/// - CapacityAllocation.ProjectId nullable (externe/interne Arbeiten)
/// - Guid Ids für alle Entities
/// 
/// Architektur: Diese Klasse defin iert die Persistierungsschicht,
/// Services sollten diese nicht direkt manipulieren (verwende Services).
/// </summary>
public class AppDbContext : DbContext
{
    /// ===== ENTITY SETS (Tabellen) =====
    /// Jedes DbSet repräsentiert eine Datenbank-Tabelle
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WbsNode> WbsNodes => Set<WbsNode>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<RateCategory> RateCategories => Set<RateCategory>();
    public DbSet<WbsTaskStatus> TaskStatuses => Set<WbsTaskStatus>();
    public DbSet<ResourceAssignment> ResourceAssignments => Set<ResourceAssignment>();
    public DbSet<ProcessPhase> ProcessPhases => Set<ProcessPhase>();
    public DbSet<WbsPhaseMapping> WbsPhaseMappings => Set<WbsPhaseMapping>();
    public DbSet<Competency> Competencies => Set<Competency>();
    public DbSet<PersonCompetency> PersonCompetencies => Set<PersonCompetency>();
    public DbSet<WbsRequiredCompetency> WbsRequiredCompetencies => Set<WbsRequiredCompetency>();
    public DbSet<CapacityAllocation> CapacityAllocations => Set<CapacityAllocation>();
    public DbSet<Deliverable> Deliverables => Set<Deliverable>();
    public DbSet<ResourceDemand> ResourceDemands => Set<ResourceDemand>();
    public DbSet<Risk> Risks => Set<Risk>();
    public DbSet<RoleAssignment> RoleAssignments => Set<RoleAssignment>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// ===== SEED DATA - Master Data Guids =====
    /// Diese Guids sind fest definiert und werden beim Seed verwendet
    /// Sie ermöglichen konsistente Referenzen in Tests und Development

    /// Task Status Ids (für WBS Knoten)
    private static readonly Guid StatusEmptyId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid StatusInCreationId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid StatusDeliveredId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    private static readonly Guid StatusBlockedId = Guid.Parse("10000000-0000-0000-0000-000000000004");
    private static readonly Guid StatusDoneId = Guid.Parse("10000000-0000-0000-0000-000000000005");

    /// Rate Category Ids (Tarifkategorien A, B, C)
    private static readonly Guid RateAId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid RateBId = Guid.Parse("20000000-0000-0000-0000-000000000002");
    private static readonly Guid RateCId = Guid.Parse("20000000-0000-0000-0000-000000000003");

    /// Person Ids (Platzhalter und Test-Personen)
    private static readonly Guid PersonAlleId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    private static readonly Guid PersonIngenieurId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    private static readonly Guid PersonTobiasId = Guid.Parse("30000000-0000-0000-0000-000000000003");
    private static readonly Guid PersonIbrahimId = Guid.Parse("30000000-0000-0000-0000-000000000004");
    private static readonly Guid PersonDennisId = Guid.Parse("30000000-0000-0000-0000-000000000005");

    /// <summary>
    /// Konfiguriert alle Entity Mappings, Indizes und Relationships
    /// Dies wird beim DbContext Init aufgerufen
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.ProjectNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Description)
                .HasMaxLength(1000);
        });

        modelBuilder.Entity<WbsNode>(entity =>
        {
            entity.ToTable("WbsNodes");

            entity.HasKey(w => w.Id);

            entity.HasIndex(w => new { w.ProjectId, w.VisibleWbsId })
                .IsUnique();

            entity.Property(w => w.VisibleWbsId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.Description)
                .HasMaxLength(2000);

            entity.Property(w => w.Comment)
                .HasMaxLength(2000);

            entity.Property(w => w.PlannedHours)
                .HasColumnType("decimal(18,2)");

            entity.Property(w => w.ActualHours)
                .HasColumnType("decimal(18,2)");

            entity.Property(w => w.ImportedPlannedCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(w => w.ImportedActualCost)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(w => w.Project)
                .WithMany(p => p.WbsNodes)
                .HasForeignKey(w => w.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(w => w.Parent)
                .WithMany(w => w.Children)
                .HasForeignKey(w => w.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(w => w.Status)
                .WithMany(s => s.WbsNodes)
                .HasForeignKey(w => w.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(w => w.ResourceAssignments)
                .WithOne(a => a.WbsNode)
                .HasForeignKey(a => a.WbsNodeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Persons");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.ShortName)
                .HasMaxLength(100);

            entity.Property(p => p.Email)
                .HasMaxLength(200);

            entity.Property(p => p.PlaceholderType)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<RateCategory>(entity =>
        {
            entity.ToTable("RateCategories");

            entity.HasKey(r => r.Id);

            entity.HasIndex(r => r.Code)
                .IsUnique();

            entity.Property(r => r.Code)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(r => r.Currency)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(r => r.HourlyRate)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<WbsTaskStatus>(entity =>
        {
            entity.ToTable("TaskStatuses");

            entity.HasKey(s => s.Id);

            entity.HasIndex(s => s.Code)
                .IsUnique();

            entity.Property(s => s.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(s => s.Label)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.Color)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<ResourceAssignment>(entity =>
        {
            entity.ToTable("ResourceAssignments");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.AssignmentRole)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(a => a.Comment)
                .HasMaxLength(2000);

            entity.Property(a => a.PlannedHours)
                .HasColumnType("decimal(18,2)");

            entity.Property(a => a.ActualHours)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(a => a.Person)
                .WithMany(p => p.ResourceAssignments)
                .HasForeignKey(a => a.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.PlannedRateCategory)
                .WithMany(r => r.PlannedAssignments)
                .HasForeignKey(a => a.PlannedRateCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.ActualRateCategory)
                .WithMany(r => r.ActualAssignments)
                .HasForeignKey(a => a.ActualRateCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => new { a.WbsNodeId, a.PersonId, a.AssignmentRole, a.IsActive });
        });

        modelBuilder.Entity<ProcessPhase>(entity =>
        {
            entity.ToTable("ProcessPhases");

            entity.HasKey(p => p.Id);

            entity.HasIndex(p => p.Code)
                .IsUnique();

            entity.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Goal)
                .HasMaxLength(1000);

            entity.Property(p => p.Description)
                .HasMaxLength(2000);

            entity.Property(p => p.DefaultResponsibility)
                .HasMaxLength(200);
        });

        modelBuilder.Entity<WbsPhaseMapping>(entity =>
        {
            entity.ToTable("WbsPhaseMappings");

            entity.HasKey(m => m.Id);

            entity.HasIndex(m => new { m.ProjectId, m.WbsNodeId, m.ProcessPhaseId })
                .IsUnique();

            entity.Property(m => m.Comment)
                .HasMaxLength(2000);

            entity.HasOne(m => m.WbsNode)
                .WithMany()
                .HasForeignKey(m => m.WbsNodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.ProcessPhase)
                .WithMany()
                .HasForeignKey(m => m.ProcessPhaseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Competency>(entity =>
        {
            entity.ToTable("Competencies");

            entity.HasKey(c => c.Id);

            entity.HasIndex(c => c.Code)
                .IsUnique();

            entity.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.Description)
                .HasMaxLength(2000);
        });

        modelBuilder.Entity<PersonCompetency>(entity =>
        {
            entity.ToTable("PersonCompetencies");

            entity.HasKey(pc => pc.Id);

            entity.HasIndex(pc => new { pc.PersonId, pc.CompetencyId })
                .IsUnique();

            entity.Property(pc => pc.Comment)
                .HasMaxLength(2000);

            entity.HasOne(pc => pc.Person)
                .WithMany()
                .HasForeignKey(pc => pc.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pc => pc.Competency)
                .WithMany()
                .HasForeignKey(pc => pc.CompetencyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WbsRequiredCompetency>(entity =>
        {
            entity.ToTable("WbsRequiredCompetencies");

            entity.HasKey(wc => wc.Id);

            entity.HasIndex(wc => new { wc.ProjectId, wc.WbsNodeId, wc.CompetencyId })
                .IsUnique();

            entity.Property(wc => wc.Comment)
                .HasMaxLength(2000);

            entity.HasOne(wc => wc.WbsNode)
                .WithMany()
                .HasForeignKey(wc => wc.WbsNodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(wc => wc.Competency)
                .WithMany()
                .HasForeignKey(wc => wc.CompetencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CapacityAllocation>(entity =>
        {
            entity.ToTable("CapacityAllocations");

            entity.HasKey(ca => ca.Id);

            entity.Property(ca => ca.Source)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(ca => ca.Comment)
                .HasMaxLength(2000);

            entity.Property(ca => ca.PlannedHours)
                .HasColumnType("decimal(18,2)");

            entity.Property(ca => ca.ActualHours)
                .HasColumnType("decimal(18,2)");

            entity.Property(ca => ca.AllocationPercent)
                .HasColumnType("decimal(5,2)");

            entity.HasOne(ca => ca.Person)
                .WithMany()
                .HasForeignKey(ca => ca.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ca => ca.Project)
                .WithMany()
                .HasForeignKey(ca => ca.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(ca => ca.WbsNode)
                .WithMany()
                .HasForeignKey(ca => ca.WbsNodeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ResourceDemand>(entity =>
        {
            entity.ToTable("ResourceDemands");

            entity.HasKey(rd => rd.Id);

            entity.Property(rd => rd.Title)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(rd => rd.Description)
                .HasMaxLength(4000);

            entity.Property(rd => rd.CreatedBy)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(rd => rd.UpdatedBy)
                .HasMaxLength(200);

            entity.Property(rd => rd.StatusChangedBy)
                .HasMaxLength(200);

            entity.Property(rd => rd.DecisionComment)
                .HasMaxLength(2000);

            entity.Property(rd => rd.PlannedHours)
                .HasColumnType("decimal(18,2)");

            entity.Property(rd => rd.Status)
                .HasConversion<int>();

            entity.HasOne(rd => rd.Project)
                .WithMany()
                .HasForeignKey(rd => rd.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rd => rd.WbsNode)
                .WithMany()
                .HasForeignKey(rd => rd.WbsNodeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(rd => rd.RequiredCompetency)
                .WithMany()
                .HasForeignKey(rd => rd.RequiredCompetencyId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Risk>(entity =>
        {
            entity.ToTable("Risks");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(r => r.Description)
                .HasMaxLength(4000);

            entity.Property(r => r.Category)
                .HasConversion<int>();

            entity.Property(r => r.Severity)
                .HasConversion<int>();

            entity.Property(r => r.Status)
                .HasConversion<int>();

            entity.HasOne(r => r.Project)
                .WithMany()
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.OwnerPerson)
                .WithMany()
                .HasForeignKey(r => r.OwnerPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.WbsNode)
                .WithMany()
                .HasForeignKey(r => r.WbsNodeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Deliverable>(entity =>
        {
            entity.ToTable("Deliverables");

            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(d => d.Description)
                .HasMaxLength(4000);

            entity.Property(d => d.Type)
                .HasConversion<int>();

            entity.Property(d => d.Status)
                .HasConversion<int>();

            entity.HasOne(d => d.Project)
                .WithMany()
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.OwnerPerson)
                .WithMany()
                .HasForeignKey(d => d.OwnerPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.ProcessPhase)
                .WithMany()
                .HasForeignKey(d => d.ProcessPhaseId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.WbsNode)
                .WithMany()
                .HasForeignKey(d => d.WbsNodeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<RoleAssignment>(entity =>
        {
            entity.ToTable("RoleAssignments");

            entity.HasKey(ra => ra.Id);

            entity.Property(ra => ra.Role)
                .HasConversion<int>();

            entity.Property(ra => ra.ScopeType)
                .HasConversion<int>();

            entity.Property(ra => ra.AssignedBy)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(ra => ra.Comment)
                .HasMaxLength(2000);

            entity.Property(ra => ra.RevokedBy)
                .HasMaxLength(200);

            entity.HasIndex(ra => new { ra.PersonId, ra.Role, ra.ScopeType, ra.ProjectId, ra.IsActive });

            entity.HasOne(ra => ra.Person)
                .WithMany()
                .HasForeignKey(ra => ra.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        /// ===== SEED DATEN LADEN =====
        /// Diese Methoden laden Master Data in die Datenbank beim Initialisieren
        /// Idempotent: Können mehrfach ausgeführt werden ohne Duplikate zu erzeugen
        SeedTaskStatuses(modelBuilder);
        SeedRateCategories(modelBuilder);
        SeedPersons(modelBuilder);
        SeedPersonCompetencies(modelBuilder);
        SeedCompetencies(modelBuilder);
    }

    /// <summary>
    /// Seed: Task Status Katalog für WBS Knoten
    /// Status: leer, in Erstellung, geliefert, blockiert, abgeschlossen
    /// </summary>
    private static void SeedTaskStatuses(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WbsTaskStatus>().HasData(
            new WbsTaskStatus
            {
                Id = StatusEmptyId,
                Code = "Empty",
                Label = "leer",
                Color = "#94A3B8",
                SortOrder = 10,
                IsActive = true,
                IsTerminal = false
            },
            new WbsTaskStatus
            {
                Id = StatusInCreationId,
                Code = "InCreation",
                Label = "in Erstellung",
                Color = "#F59E0B",
                SortOrder = 20,
                IsActive = true,
                IsTerminal = false
            },
            new WbsTaskStatus
            {
                Id = StatusDeliveredId,
                Code = "Delivered",
                Label = "geliefert",
                Color = "#3B82F6",
                SortOrder = 30,
                IsActive = true,
                IsTerminal = false
            },
            new WbsTaskStatus
            {
                Id = StatusBlockedId,
                Code = "Blocked",
                Label = "blockiert",
                Color = "#DC2626",
                SortOrder = 40,
                IsActive = true,
                IsTerminal = false
            },
            new WbsTaskStatus
            {
                Id = StatusDoneId,
                Code = "Done",
                Label = "abgeschlossen",
                Color = "#16A34A",
                SortOrder = 50,
                IsActive = true,
                IsTerminal = true
            }
        );
    }

    /// <summary>
    /// Seed: Tarifkategorien (A, B, C) für Ressourcen-Kostenverwaltung
    /// Werden für Ressourcen-Zuordnungen und Capacity Planning verwendet
    /// </summary>
    private static void SeedRateCategories(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<RateCategory>().HasData(
            new RateCategory
            {
                Id = RateAId,
                Code = "A",
                Name = "Kategorie A",
                HourlyRate = 120m,
                Currency = "EUR",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new RateCategory
            {
                Id = RateBId,
                Code = "B",
                Name = "Kategorie B",
                HourlyRate = 95m,
                Currency = "EUR",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new RateCategory
            {
                Id = RateCId,
                Code = "C",
                Name = "Kategorie C",
                HourlyRate = 75m,
                Currency = "EUR",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            }
        );
    }

    /// <summary>
    /// Seed: Kompetenzkatalog für Amprion PQ Freileitung
    /// Enthält alle technischen Kompetenzen für Freileitung-Projekte
    /// Beispiele: FM-Profil, CAD, GIS, Statik, Seilberechnung, Projektmanagement
    /// </summary>
    private static void SeedCompetencies(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2026, 7, 9, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Competency>().HasData(
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000001"),
                Code = "FM_PROFIL",
                Name = "FM-Profil",
                Description = "Planung und Bearbeitung von Freileitungsprofilen in FM-Profil.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000002"),
                Code = "CAD",
                Name = "CAD",
                Description = "CAD-Bearbeitung fuer Uebersichtsplaene, Lageplaene und technische Planunterlagen.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000003"),
                Code = "GIS",
                Name = "GIS",
                Description = "GIS-Datenverarbeitung, Datenerfassung und Aktualisierung raeumlicher Projektdaten.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000004"),
                Code = "STATIK",
                Name = "Statik",
                Description = "Statische Bemessung und Nachweisfuehrung fuer Freileitungsmaste und Bestandsmaste.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000005"),
                Code = "EMF",
                Name = "EMF-Berechnung",
                Description = "Berechnung elektromagnetischer Felder und Bewertung elektrischer Beeinflussung.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000006"),
                Code = "SEILBERECHNUNG",
                Name = "Seilberechnung",
                Description = "Berechnung von Seilspannungen, Durchhaengen, Passlaengen und zugehoerigen Mengenermittlungen.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000007"),
                Code = "PROJEKTMANAGEMENT",
                Name = "Projektmanagement",
                Description = "Projektsteuerung, Terminplanung, Ressourcenmanagement und Koordination.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000008"),
                Code = "AUTOMATISIERUNG",
                Name = "Automatisierung / KI",
                Description = "Automatisierung, KI-Unterstuetzung, Prozessentwicklung und digitale Werkzeugunterstuetzung.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000009"),
                Code = "TRASSIERUNG",
                Name = "Trassierung",
                Description = "Grob- und Feintrassierung von Freileitungen einschliesslich technischer und raeumlicher Randbedingungen.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Competency
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000010"),
                Code = "MASTKONZEPTE",
                Name = "Mastkonzepte / Mastkatalog",
                Description = "Erstellung und Bewertung von Mastkonzepten, Mastkatalogen und technischen Uebersichten.",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            }
        );
    }

    /// <summary>
    /// Seed: Personen (Platzhalter und Test-Personen)
    /// Platzhalter: "alle" (Allgemein), "Ingenieur" (Rolleneinsatz)
    /// Test-Personen: Tobias, Ibrahim, Dennis (für Entwicklung)
    /// </summary>
    private static void SeedPersons(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Person>().HasData(
            new Person
            {
                Id = PersonAlleId,
                DisplayName = "alle",
                ShortName = "alle",
                IsPlaceholder = true,
                PlaceholderType = "All",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Person
            {
                Id = PersonIngenieurId,
                DisplayName = "Ingenieur",
                ShortName = "Ingenieur",
                IsPlaceholder = true,
                PlaceholderType = "RolePlaceholder",
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Person
            {
                Id = PersonTobiasId,
                DisplayName = "Tobias",
                ShortName = "Tobias",
                Email = "tobias@example.local",
                IsPlaceholder = false,
                PlaceholderType = null,
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Person
            {
                Id = PersonIbrahimId,
                DisplayName = "Ibrahim",
                ShortName = "Ibrahim",
                Email = "ibrahim@example.local",
                IsPlaceholder = false,
                PlaceholderType = null,
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new Person
            {
                Id = PersonDennisId,
                DisplayName = "Dennis",
                ShortName = "Dennis",
                Email = "dennis@example.local",
                IsPlaceholder = false,
                PlaceholderType = null,
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            }
        );
    }

    /// <summary>
    /// Seed: Person-Kompetenz Zuordnungen (Test-Daten)
    /// Mappt Test-Personen (Tobias, Ibrahim, Dennis) zu ihren Kompetenzen
    /// </summary>
    private static void SeedPersonCompetencies(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2026, 7, 9, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<PersonCompetency>().HasData(
            new PersonCompetency
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000001"),
                PersonId = PersonDennisId,
                CompetencyId = Guid.Parse("40000000-0000-0000-0000-000000000008"),                Comment = "Automatisierung und KI",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new PersonCompetency
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000002"),
                PersonId = PersonTobiasId,
                CompetencyId = Guid.Parse("40000000-0000-0000-0000-000000000001"),                Comment = "FM-Profil",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new PersonCompetency
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000003"),
                PersonId = PersonTobiasId,
                CompetencyId = Guid.Parse("40000000-0000-0000-0000-000000000002"),                Comment = "CAD",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new PersonCompetency
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000004"),
                PersonId = PersonIbrahimId,
                CompetencyId = Guid.Parse("40000000-0000-0000-0000-000000000002"),                Comment = "CAD",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            },
            new PersonCompetency
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000005"),
                PersonId = PersonIbrahimId,
                CompetencyId = Guid.Parse("40000000-0000-0000-0000-000000000003"),                Comment = "GIS",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            }
        );
    }
}


