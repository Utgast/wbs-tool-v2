using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Modules.Persons.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.RateCategories.Models;
using WbsTool.Api.Modules.Wbs.Models;
using WbsTaskStatus = WbsTool.Api.Modules.TaskStatuses.Models.TaskStatus;

namespace WbsTool.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WbsNode> WbsNodes => Set<WbsNode>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<RateCategory> RateCategories => Set<RateCategory>();
    public DbSet<WbsTaskStatus> TaskStatuses => Set<WbsTaskStatus>();
    public DbSet<ResourceAssignment> ResourceAssignments => Set<ResourceAssignment>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    private static readonly Guid StatusEmptyId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid StatusInCreationId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid StatusDeliveredId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    private static readonly Guid StatusBlockedId = Guid.Parse("10000000-0000-0000-0000-000000000004");
    private static readonly Guid StatusDoneId = Guid.Parse("10000000-0000-0000-0000-000000000005");

    private static readonly Guid RateAId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid RateBId = Guid.Parse("20000000-0000-0000-0000-000000000002");
    private static readonly Guid RateCId = Guid.Parse("20000000-0000-0000-0000-000000000003");

    private static readonly Guid PersonAlleId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    private static readonly Guid PersonIngenieurId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    private static readonly Guid PersonTobiasId = Guid.Parse("30000000-0000-0000-0000-000000000003");
    private static readonly Guid PersonIbrahimId = Guid.Parse("30000000-0000-0000-0000-000000000004");
    private static readonly Guid PersonDennisId = Guid.Parse("30000000-0000-0000-0000-000000000005");

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

        SeedTaskStatuses(modelBuilder);
        SeedRateCategories(modelBuilder);
        SeedPersons(modelBuilder);
    }

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
}