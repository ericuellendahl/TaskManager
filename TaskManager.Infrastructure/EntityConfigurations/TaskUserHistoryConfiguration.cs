using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.EntityConfigurations;

public class TaskUserHistoryConfiguration : IEntityTypeConfiguration<TaskUserHistory>
{
    public void Configure(EntityTypeBuilder<TaskUserHistory> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.PropertyName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ModifiedAt);
    }
}
