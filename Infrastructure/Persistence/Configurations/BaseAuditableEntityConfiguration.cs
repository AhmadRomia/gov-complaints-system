using Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Configurations
{
    public class BaseAuditableEntityConfiguration : IEntityTypeConfiguration<BaseAuditableEntity>
    {
        public void Configure(EntityTypeBuilder<BaseAuditableEntity> builder)
        {
            builder.Property(e => e.CreatedBy).HasMaxLength(200);
            builder.Property(e => e.UpdatedBy).HasMaxLength(200);

            builder.Property(e => e.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken();
        }
    }
}
