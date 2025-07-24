using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Models.Entities.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.AssignmentId);

            builder.HasMany(a => a.StudentsAssignment)
                .WithOne(sa => sa.Assignment)
                .HasForeignKey(sa => sa.AssignmentId);
        }
    }
}
