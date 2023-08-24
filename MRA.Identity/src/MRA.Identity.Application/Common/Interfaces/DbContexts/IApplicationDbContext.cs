using Microsoft.EntityFrameworkCore;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.DbContexts;

public interface IApplicationDbContext
{
    public DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public Task<int> SaveChangesAsync();
}