using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Modules.Capacity.Models;
using WbsTool.Api.Modules.Competencies.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Competency> Competencies => Set<Competency>();
    public DbSet<PersonCompetency> PersonCompetencies => Set<PersonCompetency>();
    public DbSet<PersonCapacity> PersonCapacities => Set<PersonCapacity>();
    public DbSet<PersonAllocation> PersonAllocations => Set<PersonAllocation>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WbsNode> WbsNodes => Set<WbsNode>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

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

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Persons");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<Competency>(entity =>
        {
            entity.ToTable("Competencies");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<PersonCompetency>(entity =>
        {
            entity.ToTable("PersonCompetencies");

            entity.HasKey(pc => pc.Id);

            entity.HasOne(pc => pc.Person)
                .WithMany(p => p.PersonCompetencies)
                .HasForeignKey(pc => pc.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pc => pc.Competency)
                .WithMany(c => c.PersonCompetencies)
                .HasForeignKey(pc => pc.CompetencyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(pc => new { pc.PersonId, pc.CompetencyId })
                .IsUnique();
        });

        modelBuilder.Entity<PersonCapacity>(entity =>
        {
            entity.ToTable("PersonCapacities");

            entity.HasKey(pc => pc.Id);

            entity.HasOne(pc => pc.Person)
                .WithMany(p => p.Capacities)
                .HasForeignKey(pc => pc.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(pc => new { pc.PersonId, pc.WeekStartDate })
                .IsUnique();
        });

        modelBuilder.Entity<PersonAllocation>(entity =>
        {
            entity.ToTable("PersonAllocations");

            entity.HasKey(pa => pa.Id);

            entity.HasOne(pa => pa.Person)
                .WithMany(p => p.Allocations)
                .HasForeignKey(pa => pa.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pa => pa.Project)
                .WithMany()
                .HasForeignKey(pa => pa.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pa => pa.WbsNode)
                .WithMany()
                .HasForeignKey(pa => pa.WbsNodeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<WbsNode>(entity =>
        {
            entity.ToTable("WbsNodes");

            entity.HasKey(w => w.Id);

            entity.Property(w => w.VisibleWbsId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Code)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.Description)
                .HasMaxLength(2000);

            entity.Property(w => w.ResponsiblePersonName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.Comment)
                .HasMaxLength(2000);

            entity.HasOne(w => w.Project)
                .WithMany(p => p.WbsNodes)
                .HasForeignKey(w => w.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(w => w.Parent)
                .WithMany(w => w.Children)
                .HasForeignKey(w => w.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}