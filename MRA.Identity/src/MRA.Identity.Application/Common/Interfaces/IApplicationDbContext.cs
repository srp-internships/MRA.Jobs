using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MRA.Identity.Application.Common.Interfaces;


public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
