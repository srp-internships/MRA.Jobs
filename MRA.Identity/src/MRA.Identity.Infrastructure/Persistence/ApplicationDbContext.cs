using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces;
using MRA.Identity.Domain.Common;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Infrastructure.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>,IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }
    

    #region override
    protected override void OnModelCreating(ModelBuilder builder)
    {
       // builder.Entity<BaseEntity>().HasQueryFilter(e => !e.IsDeleted);
        builder.Ignore<BaseEntity>();
        // builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
    

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    #endregion
}