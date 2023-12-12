using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Identity;
using MRA.Configurations.Common.Constants;

namespace MRA.Identity.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    private ApplicationRole _superAdminRole = null!;
    private ApplicationRole _applicationRole = null!;

    public ApplicationDbContextInitializer(RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    public async Task SeedAsync()
    {
        await CreateRolesAsync();

        await CreateSuperAdminAsync();  

        await CreateApplicationAdmin("MraJobs", "mrajobs12@@34,.$3#A");
        await CreateApplicationAdmin("MraOnlinePlatform", "mraonline2@f@34,/.$3#A");
    }

    private async Task CreateApplicationAdmin(string applicationName, string adminPassword)
    {
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
                Email = $"{applicationName.ToLower()}admin@silkroadprofessionals.com",
            };

            var createMraJobsAdminResult = await _userManager.CreateAsync(mraJobsAdminUser, adminPassword);
            ThrowExceptionFromIdentityResult(createMraJobsAdminResult);
        }
        //create user

        //create userRole
        var userRole = new ApplicationUserRole
        {
            UserId = mraJobsAdminUser.Id, RoleId = _applicationRole.Id, Slug = $"{mraJobsAdminUser.UserName}-role"
        };

        if (!await _context.UserRoles.AnyAsync(s => s.RoleId == userRole.RoleId && s.UserId == userRole.UserId))
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
        //create userRole

        //create role claim
        if (!await _context.UserClaims.AnyAsync(s =>
                s.UserId == mraJobsAdminUser.Id &&
                s.ClaimType == ClaimTypes.Role))
        {
            var userRoleClaim = new ApplicationUserClaim
            {
                UserId = mraJobsAdminUser.Id,
                ClaimType = ClaimTypes.Role,
                ClaimValue = ApplicationClaimValues.Administrator,
                Slug = $"{mraJobsAdminUser.UserName}-role"
            };
            await _context.UserClaims.AddAsync(userRoleClaim);
        }
        //create role claim
        
        //create email claim
        if (!await _context.UserClaims.AnyAsync(s =>
                s.UserId == mraJobsAdminUser.Id &&
                s.ClaimType == ClaimTypes.Role))
        {
            var userRoleClaim = new ApplicationUserClaim
            {
                UserId = mraJobsAdminUser.Id,
                ClaimType = ClaimTypes.Email,
                ClaimValue = mraJobsAdminUser.Email,
                Slug = $"{mraJobsAdminUser.UserName}-email"
            };
            await _context.UserClaims.AddAsync(userRoleClaim);
        }
        //create email claim


        //create application claim
        if (!await _context.UserClaims.AnyAsync(s =>
                s.UserId == mraJobsAdminUser.Id &&
                s.ClaimType == ClaimTypes.Application))
        {
            var userApplicationClaim = new ApplicationUserClaim
            {
                UserId = mraJobsAdminUser.Id,
                ClaimType = ClaimTypes.Application,
                ClaimValue = applicationName,
                Slug = $"{mraJobsAdminUser.UserName}-application"
            };
            await _context.UserClaims.AddAsync(userApplicationClaim);
        }

        //create application claim

        //create username claim
        if (!await _context.UserClaims.AnyAsync(s =>
                s.UserId == mraJobsAdminUser.Id &&
                s.ClaimType == ClaimTypes.Username))
        {
            var userApplicationClaim = new ApplicationUserClaim
            {
                UserId = mraJobsAdminUser.Id,
                ClaimType = ClaimTypes.Username,
                ClaimValue = mraJobsAdminUser.UserName,
                Slug = $"{mraJobsAdminUser.UserName}-username"
            };
            await _context.UserClaims.AddAsync(userApplicationClaim);
        }
        //create username claim

        //create id claim
        if (!await _context.UserClaims.AnyAsync(s =>
                s.UserId == mraJobsAdminUser.Id &&
                s.ClaimType == ClaimTypes.Id))
        {
            var userApplicationClaim = new ApplicationUserClaim
            {
                UserId = mraJobsAdminUser.Id,
                ClaimType = ClaimTypes.Id,
                ClaimValue = mraJobsAdminUser.Id.ToString(),
                Slug = $"{mraJobsAdminUser.UserName}-id"
            };
            await _context.UserClaims.AddAsync(userApplicationClaim);
        }
        //create id claim
        
        await _context.SaveChangesAsync();
    }


    private async Task CreateSuperAdminAsync()
    {
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
            ThrowExceptionFromIdentityResult(superAdminResult);
            
            var userRole = new ApplicationUserRole
            {
                UserId = superAdmin.Id, RoleId = _superAdminRole.Id, Slug = $"role-{superAdmin.UserName}"
            };

            if (!await _context.UserRoles.AnyAsync(s => s.RoleId == userRole.RoleId && s.UserId == userRole.UserId))
            {
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }
            
            //create role claim
            var claim = new ApplicationUserClaim
            {
                ClaimType = ClaimTypes.Role,
                ClaimValue = _superAdminRole.Name,
                Slug = $"{superAdmin.UserName}-role",
                UserId = superAdmin.Id
            };
            await _context.UserClaims.AddAsync(claim);
            //create role claim

            //create application claim
            claim = new ApplicationUserClaim
            {
                ClaimType = ClaimTypes.Application,
                ClaimValue = ApplicationClaimValues.AllApplications,
                Slug = $"{superAdmin.UserName}-application",
                UserId = superAdmin.Id
            };
            await _context.UserClaims.AddAsync(claim);   
            //create application claim
            
            //create username claim
            claim = new ApplicationUserClaim
            {
                ClaimType = ClaimTypes.Username,
                ClaimValue = superAdmin.UserName,
                Slug = $"{superAdmin.UserName}-username",
                UserId = superAdmin.Id
            };
            await _context.UserClaims.AddAsync(claim);   
            //create username claim
            
            //create email claim
            claim = new ApplicationUserClaim
            {
                ClaimType = ClaimTypes.Email,
                ClaimValue = superAdmin.Email,
                Slug = $"{superAdmin.UserName}-email",
                UserId = superAdmin.Id
            };
            await _context.UserClaims.AddAsync(claim);   
            //create email claim
            
            //create id claim
            claim = new ApplicationUserClaim
            {
                ClaimType = ClaimTypes.Id,
                ClaimValue = superAdmin.Id.ToString(),
                Slug = $"{superAdmin.UserName}-id",
                UserId = superAdmin.Id
            };
            await _context.UserClaims.AddAsync(claim);   
            //create id claim

            await _context.SaveChangesAsync();
        }
    }

    private async Task CreateRolesAsync()
    {
        _context.Roles.RemoveRange(await _context.Roles.ToListAsync());
        await _context.SaveChangesAsync();
        
        //superAdmin
        var roleSuper = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = ApplicationClaimValues.SuperAdministrator,
            NormalizedName = ApplicationClaimValues.SuperAdministrator.ToUpper(),
            Slug = ApplicationClaimValues.SuperAdministrator
        };
        var createRoleResult = await _roleManager.CreateAsync(roleSuper);
        ThrowExceptionFromIdentityResult(createRoleResult);
        _superAdminRole = roleSuper;
        //superAdmin

        //applicationAdmin
        var role = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = ApplicationClaimValues.Administrator,
            NormalizedName = ApplicationClaimValues.Administrator.ToUpper(),
            Slug = ApplicationClaimValues.Administrator
        };
        createRoleResult = await _roleManager.CreateAsync(role);
        ThrowExceptionFromIdentityResult(createRoleResult);
        _applicationRole = role;
        //applicationAdmin

        //reviewer
        var roleString = "Reviewer";
        role = new ApplicationRole
        {
            Id = Guid.NewGuid(), Name = roleString, NormalizedName = roleString.ToUpper(), Slug = roleString
        };

        createRoleResult = await _roleManager.CreateAsync(role);
        ThrowExceptionFromIdentityResult(createRoleResult);
        //reviewer

        //applicant
        roleString = "Applicant";
        role = new ApplicationRole
        {
            Id = Guid.NewGuid(), Name = roleString, NormalizedName = roleString.ToUpper(), Slug = roleString
        };

        createRoleResult = await _roleManager.CreateAsync(role);
        ThrowExceptionFromIdentityResult(createRoleResult);
        //applicant
        
        //teacher
        roleString = "Teacher";
        role = new ApplicationRole
        {
            Id = Guid.NewGuid(), Name = roleString, NormalizedName = roleString.ToUpper(), Slug = roleString
        };

        createRoleResult = await _roleManager.CreateAsync(role);
        ThrowExceptionFromIdentityResult(createRoleResult);
        //teacher
        
        //student
        roleString = "Student";
        role = new ApplicationRole
        {
            Id = Guid.NewGuid(), Name = roleString, NormalizedName = roleString.ToUpper(), Slug = roleString
        };

        createRoleResult = await _roleManager.CreateAsync(role);
        ThrowExceptionFromIdentityResult(createRoleResult);
        //student
    }

    private static void ThrowExceptionFromIdentityResult(IdentityResult result,[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!result.Succeeded)
        {
            throw new Exception($"{result.Errors.First().Description}\n exception was thrown at line {sourceLineNumber}");
        }
    }
}