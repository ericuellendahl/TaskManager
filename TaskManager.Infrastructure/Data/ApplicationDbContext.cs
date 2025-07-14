using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskUser> Tasks { get; set; }
    public DbSet<TaskUserHistory> TaskHistories { get; set; }
    public DbSet<TaskUserComment> TaskComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}