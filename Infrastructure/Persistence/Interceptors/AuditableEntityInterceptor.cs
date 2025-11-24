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

        private void ApplyAuditing(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = _dateService.UtcNow;
                    entry.Entity.CreatedBy = _currentUser.UserId.ToString();
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = _dateService.UtcNow;
                    entry.Entity.UpdatedBy = _currentUser.UserId.ToString();
                }
            }
        }
    }
}
