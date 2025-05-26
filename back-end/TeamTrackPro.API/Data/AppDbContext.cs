using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TeamTrackPro.API.Models;
using TeamTrackPro.API.Data.Configurations;
using TeamTrackPro.API.Data.Seed;

namespace TeamTrackPro.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityLogConfiguration());

        // Apply seeders
        new RoleSeeder(modelBuilder).Seed();
        new UserSeeder(modelBuilder).Seed();

        // Configure User-Role relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        // Configure Project-Ticket relationship
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.ProjectId);

        // Configure Ticket-User relationship (AssignedTo)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.AssignedTo)
            .WithMany()
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure TicketComment relationships
        modelBuilder.Entity<TicketComment>()
            .HasOne(tc => tc.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(tc => tc.TicketId);

        modelBuilder.Entity<TicketComment>()
            .HasOne(tc => tc.CreatedBy)
            .WithMany()
            .HasForeignKey(tc => tc.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure ActivityLog-User relationship
        modelBuilder.Entity<ActivityLog>()
            .HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 