using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTimeService _dateService;

        public AuditableEntityInterceptor(
            ICurrentUserService currentUser,
            IDateTimeService dateService)
        {
            _currentUser = currentUser;
            _dateService = dateService;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplyAuditing(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditing(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplyAuditing(DbContext? context)
        {
            if (context == null) return;

            var userId = _currentUser.UserId?.ToString();
            var now = _dateService.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;
                }
            }
        }
    }
}
