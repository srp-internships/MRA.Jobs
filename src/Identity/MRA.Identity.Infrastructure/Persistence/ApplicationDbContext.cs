using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces;
using MRA.Identity.Domain.Common;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext,IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
        Users = Set<User>();
    }

  

    #region override
    protected override void OnModelCreating(ModelBuilder builder)
    {
       // builder.Entity<BaseEntity>().HasQueryFilter(e => !e.IsDeleted);
        builder.Ignore<BaseEntity>();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
    
    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    #endregion
}