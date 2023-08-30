
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.DbContexts;

public interface IApplicationDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }

    public EntityEntry Update(object entity);
}