using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Configurations
{
    public class GovernmentEntityConfiguration : IEntityTypeConfiguration<GovernmentEntity>
    {
        public void Configure(EntityTypeBuilder<GovernmentEntity> builder)
        {
            builder.ToTable("GovernmentEntities");

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.LogoUrl)
                   .HasMaxLength(500);

            builder.HasMany(e => e.Complaints)
                   .WithOne(c => c.GovernmentEntity)
                   .HasForeignKey(c => c.GovernmentEntityId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
