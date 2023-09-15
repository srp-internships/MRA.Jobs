using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Domain.Entities;
using Mra.Shared.Common.Constants;

namespace MRA.Identity.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public ApplicationDbContextInitializer(RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }


    public async Task SeedAsync()
    {
        await CreateSuperAdminAsync();

        await CreateApplicationAdmin("MraJobs", "mrajobs12@@34,.$3#A");
        await CreateApplicationAdmin("MraOnlinePlatform", "mraonline2@f@34,/.$3#A");
    }

    private async Task CreateApplicationAdmin(string applicationName, string adminPassword)
    {
        //create role
        var adminRole = await _context.Roles.FirstOrDefaultAsync(s => s.NormalizedName == "APPLICATIONADMIN");

        if (adminRole == null)
        {
            adminRole = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = "ApplicationAdmin",
                NormalizedName = "APPLICATIONADMIN",
                Slug = "ApplicationAdmin"
            };

            var createRoleResult = await _roleManager.CreateAsync(adminRole);
            if (!createRoleResult.Succeeded)
            {
                throw new Exception(createRoleResult.Errors.First().Description);
            }
        }

        var roleClaim = new IdentityRoleClaim<Guid>
        {
            Id = 0, RoleId = adminRole.Id, ClaimType = "Application", ClaimValue = applicationName
        };
        await _context.RoleClaims.AddAsync(roleClaim);
        await _context.SaveChangesAsync();


        //create role

        //create user
        var mraJobsAdminUser =
            await _userManager.Users.SingleOrDefaultAsync(u =>
                u.NormalizedUserName == $"{applicationName}ADMIN".ToUpper());

        if (mraJobsAdminUser == null)
        {
            mraJobsAdminUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = $"{applicationName}Admin",
                NormalizedUserName = $"{applicationName}ADMIN".ToUpper(),
                Email = "mrajobsadmin@silkroadprofessionals.com",
            };

            var createMraJobsAdminResult = await _userManager.CreateAsync(mraJobsAdminUser, adminPassword);
            if (!createMraJobsAdminResult.Succeeded)
            {
                throw new Exception(createMraJobsAdminResult.Errors.First().Description);
            }
        }
        //create user

        //create userRole
        var userRole = new ApplicationUserRole
        {
            UserId = mraJobsAdminUser.Id, RoleId = adminRole.Id, Slug = $"{applicationName}Admin-applicationAdmin"
        };

        if (!await _context.UserRoles.AnyAsync(s => s.RoleId == userRole.RoleId && s.UserId == userRole.UserId))
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
        //create userRole
    }


    private async Task CreateSuperAdminAsync()
    {
        var superAdminRole = await _roleManager.Roles.SingleOrDefaultAsync(s => s.NormalizedName == "SUPERADMIN");
        if (superAdminRole == null)
        {
            superAdminRole = new ApplicationRole
            {
                Id = Guid.NewGuid(), Name = "SuperAdmin", NormalizedName = "SUPERADMIN", Slug = "SuperAdmin",
            };
            var createRoleResult = await _roleManager.CreateAsync(superAdminRole);
            if (!createRoleResult.Succeeded)
            {
                throw new Exception(createRoleResult.Errors.First().Description);
            }
        }

        var superAdmin = await _userManager.Users.SingleOrDefaultAsync(s => s.NormalizedUserName == "SUPERADMIN");
        if (superAdmin == null)
        {
            superAdmin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "SuperAdmin",
                NormalizedUserName = "SUPERADMIN",
                Email = "mraidentity@silkroadprofessionals.com",
                NormalizedEmail = "mraidentity@silkroadprofessionals.com",
                EmailConfirmed = false,
            };
            var superAdminResult = await _userManager.CreateAsync(superAdmin, "Mra123!!@#$AGfer4");
            if (!superAdminResult.Succeeded)
            {
                throw new Exception(superAdminResult.Errors.First().Description);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(superAdmin, superAdminRole.Name!);
            if (!addRoleResult.Succeeded)
            {
                throw new Exception(addRoleResult.Errors.First().Description);
            }

            var claim = new ApplicationUserClaim
            {
                ClaimType = ClaimTypes.Role,
                ClaimValue = superAdminRole.Name,
                Slug = "SuperAdmin-SuperAdmin",
                UserId = superAdmin.Id
            };
            await _context.UserClaims.AddAsync(claim);
            await _context.SaveChangesAsync();
        }
    }
}