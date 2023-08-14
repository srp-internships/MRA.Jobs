using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new ApplicationRole("Administrator");
        var applicantRole = new ApplicationRole("Applicant");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }
        if (_roleManager.Roles.All(r => r.Name != applicantRole.Name))
        {
            await _roleManager.CreateAsync(applicantRole);
        }

        // Default users

        if (!_userManager.Users.Any(u => u.UserName == "Admin"))
        {
            var administrator = new ApplicationUser
            {
                UserName = "Admin",
                Email = "administrator@localhost.com",
                PhoneNumber = "1234567890",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            await _userManager.CreateAsync(administrator, "Test.12324");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }

            _context.Reviewers.Add(new Reviewer
            {
                Id = administrator.Id,
                DateOfBirth = DateTime.Now.AddYears(-20),
                Email = administrator.Email,
                PhoneNumber = administrator.PhoneNumber,
                Avatar = "https://i.pravatar.cc/300",
                Gender = Domain.Enums.Gender.Male,
                FirstName = "Admin",
                LastName = "Admin",
                JobTitle = "Administrator"
            });
            await _context.SaveChangesAsync();
        }

    }
}
