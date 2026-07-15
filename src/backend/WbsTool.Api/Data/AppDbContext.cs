using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Data;

public class AppDbContext : DbContext
{
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

        modelBuilder.Entity<WbsNode>(entity =>
        {
            entity.ToTable("WbsNodes");

            entity.HasKey(w => w.Id);

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