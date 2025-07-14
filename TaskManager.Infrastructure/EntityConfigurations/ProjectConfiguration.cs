using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.EntityConfigurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).HasMaxLength(1000);
            builder.Property(e => e.CreatedAt);
            builder.Property(e => e.UpdatedAt);

            builder.HasMany(e => e.Tasks)
                   .WithOne(e => e.Project)
                   .HasForeignKey(e => e.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
