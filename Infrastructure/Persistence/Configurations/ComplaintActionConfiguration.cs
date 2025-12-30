using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ComplaintActionConfiguration : IEntityTypeConfiguration<ComplaintAction>
    {
        public void Configure(EntityTypeBuilder<ComplaintAction> builder)
        {
            builder.ToTable("ComplaintActions");

            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(x => x.Complaint)
                .WithMany(c => c.Actions)
                .HasForeignKey(x => x.ComplaintId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
