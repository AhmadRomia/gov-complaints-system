using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ComplaintVersionConfiguration : IEntityTypeConfiguration<ComplaintVersion>
    {
        public void Configure(EntityTypeBuilder<ComplaintVersion> builder)
        {
            builder.ToTable("ComplaintVersions");

            builder.HasOne(x => x.Complaint)
                .WithMany()
                .HasForeignKey(x => x.ComplaintId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
