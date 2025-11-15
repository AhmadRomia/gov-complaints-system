

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
