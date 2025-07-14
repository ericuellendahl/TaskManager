using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.EntityConfigurations;

public class TaskUserConfiguration : IEntityTypeConfiguration<TaskUser>
{
    public void Configure(EntityTypeBuilder<TaskUser> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.Status).HasConversion<int>();
        builder.Property(e => e.Priority).HasConversion<int>();
        builder.Property(e => e.CreatedAt);
        builder.Property(e => e.UpdatedAt);

        builder.HasMany(e => e.History)
               .WithOne(e => e.Task)
               .HasForeignKey(e => e.TaskId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Comments)
               .WithOne(e => e.Task)
               .HasForeignKey(e => e.TaskId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
