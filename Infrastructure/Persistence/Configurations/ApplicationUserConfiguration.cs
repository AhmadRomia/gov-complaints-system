using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.OtpCode)
                .HasMaxLength(10);

            builder.Property(u => u.IsVerified)
                .HasDefaultValue(false);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(u => u.GovernmentEntity)
                .WithMany(g => g.Employees)
                .HasForeignKey(u => u.GovernmentEntityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
