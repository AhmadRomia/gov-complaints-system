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

            builder.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Attachments)
                .HasConversion(new AttachmentListConverter())
                .HasColumnType("nvarchar(max)");

        }
    }
}
