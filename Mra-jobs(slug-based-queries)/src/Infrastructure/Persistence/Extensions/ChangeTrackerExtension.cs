using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MRA.Jobs.Infrastructure.Persistence.Extensions;
public static class ChangeTrackerExtensions
{
    public static void SetAuditProperties(this ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();
        IEnumerable<EntityEntry> entities =
            changeTracker
                .Entries()
                .Where(t => t.Entity is ISoftDelete && t.State == EntityState.Deleted);

        if (entities.Any())
        {
            foreach (EntityEntry entry in entities)
            {
                ISoftDelete entity = (ISoftDelete)entry.Entity;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}