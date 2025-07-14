using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.EntityConfigurations;

public class TaskUserCommentConfiguration : IEntityTypeConfiguration<TaskUserComment>
{
    public void Configure(EntityTypeBuilder<TaskUserComment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Content).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.CreatedAt);
    }
}
