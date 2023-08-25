
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.DbContexts;

public interface IApplicationDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<ApplicationUserClaim> UserClaims { get; set; }
}