using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Converters;


namespace Infrastructure.Persistence.Configurations
{
    public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.ToTable("Complaints");

            builder.HasMany(c => c.AgencyNotes)
                   .WithOne(n => n.Complaint)
                   .HasForeignKey(n => n.ComplaintId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.AdditionalInfoRequests)
                   .WithOne(r => r.Complaint)
                   .HasForeignKey(r => r.ComplaintId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Attachments)
                .HasConversion(new AttachmentListConverter())
                .HasColumnType("nvarchar(max)");



            // configure actions navigation
            builder.HasMany(c => c.Actions)
                   .WithOne(a => a.Complaint)
                   .HasForeignKey(a => a.ComplaintId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.ReferenceNumber);
        }
    }
}
