using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence
{
    public class ApplicationDbContext :  IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        private readonly AuditableEntityInterceptor _auditableInterceptor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            AuditableEntityInterceptor auditableInterceptor
        ) : base(options)
        {
            _auditableInterceptor = auditableInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableInterceptor);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
