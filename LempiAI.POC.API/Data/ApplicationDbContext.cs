using Microsoft.EntityFrameworkCore;
using LempiAI.POC.API.Models;

namespace LempiAI.POC.API.Data;

/// <summary>
/// Database context for the lempiAI.POC application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<Feedback> Feedback { get; set; } = null!;
    public DbSet<ProcessImprovement> ProcessImprovements { get; set; } = null!;
    public DbSet<AgentReport> AgentReports { get; set; } = null!;
    
    /// <summary>
    /// Configures the database model.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasMany(e => e.Schedules)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.ProvidedFeedback)
                .WithOne(f => f.ProvidedByEmployee)
                .HasForeignKey(f => f.ProvidedByEmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TaskDescription).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Priority).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.EmployeeId);
        });
        
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FeedbackText).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Rating).IsRequired();
            entity.HasIndex(e => e.ProvidedByEmployeeId);
        });
        
        modelBuilder.Entity<ProcessImprovement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.ProcessArea).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ImpactLevel).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SuggestedActions).IsRequired();
        });
        
        modelBuilder.Entity<AgentReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AgentName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.KeyFindings).IsRequired();
            entity.Property(e => e.ReportType).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.AgentName);
        });
    }
}
