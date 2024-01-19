using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.DbContexts;

public interface IApplicationDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public DbSet<ApplicationUserRole> UserRoles { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; }
    public DbSet<IdentityRoleClaim<Guid>> RoleClaims { get; set; }
    public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<EducationDetail> Educations { get; set; }
    public DbSet<ExperienceDetail> Experiences { get; set; }
    public DbSet<Message> Messages { get; set; }
}