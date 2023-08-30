
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.DbContexts;

public interface IApplicationDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public DbSet<IdentityUserRole<Guid>> UserRoles { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; }
}