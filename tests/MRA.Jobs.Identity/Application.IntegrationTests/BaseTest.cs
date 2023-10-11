using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Persistence;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public abstract class BaseTest
{
    protected HttpClient _client { get; private set; } = null!;
    private CustomWebApplicationFactory _factory { get; set; }
    protected Guid FirstUserId { get; private set; }
    protected ApplicationUser Applicant { get; private set; }
    protected ApplicationUser Reviewer { get; private set; }
    /// <summary>
    /// Initializing of factory and _context. And Creating a InMemoryDB
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    [OneTimeSetUp]
    public virtual async Task OneTimeSetup()
    {
        _factory = new CustomWebApplicationFactory();

        // Register a new user and add it to the database to check whether he has logged in
        var request1 = new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            UserName = "@Alex33",
            NormalizedUserName = "@alex33",
            PhoneNumber = "123456789",
        };

        await AddUser(request1, "password@#12P");
        FirstUserId = Guid.NewGuid();

        var request2 = new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            Email = "reviewer@example.com",
            UserName = "@Reviewer",
            NormalizedUserName = "@reviewer",
            PhoneNumber = "123456789",
        };

        await AddUser(request2, "password@#12P");
    }
    protected async Task<T> GetEntity<T>(Expression<Func<T, bool>> query) where T : class
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await dbContext.Set<T>().FirstOrDefaultAsync(query);
    }

    protected async Task AddEntity<T>(T entity) where T : class
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    protected Task<List<T>> GetAll<T>() where T : class
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return dbContext.Set<T>().ToListAsync();
    }
    protected Task<List<T>> GetWhere<T>(Expression<Func<T, bool>> query) where T : class
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return dbContext.Set<T>().Where(query).ToListAsync();
    }

    protected Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return dbContext.Set<TEntity>().CountAsync();
    }
    protected async Task AddUser(ApplicationUser user, string password)
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await dbContext.CreateAsync(user, password);
    }

    protected async Task AddAuthorizationAsync()
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var superAdmin = await userManager.Users.FirstOrDefaultAsync(s => s.NormalizedUserName == "SUPERADMIN");
        var claims = await userManager.GetClaimsAsync(superAdmin);
        var token = tokenService.CreateTokenByClaims(claims, out _);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [SetUp]
    public void Setup()
    {
        _client = _factory.CreateClient();
    }

    protected async Task AddApplicantAuthorizationAsync()
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByNameAsync("@Alex33");
        Applicant = user;
        var prefics = "http://schemas.microsoft.com/ws/2008/06/identity/claims/";

        var claims = new List<Claim>
        {
            new Claim($"{prefics}role", "Applicant"),
            new Claim($"{prefics}Id", user.Id.ToString()),
            new Claim($"{prefics}username", user.UserName),
            new Claim($"{prefics}email", user.Email),
            new Claim($"{prefics}application", "MraJobs")
        };
        var token = tokenService.CreateTokenByClaims(claims);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected async Task AddReviewerAuthorizationAsync()
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var reviewer = await userManager.Users.FirstOrDefaultAsync(s => s.UserName == "@Reviewer");
        var prefics = "http://schemas.microsoft.com/ws/2008/06/identity/claims/";
        Reviewer = reviewer;
        var claims = new List<Claim>
        {
            new Claim($"{prefics}role", "Reviewer"),
            new Claim($"{prefics}Id", reviewer.Id.ToString()),
            new Claim($"{prefics}username", reviewer.UserName),
            new Claim($"{prefics}email", reviewer.Email),
            new Claim($"{prefics}application", "MraJobs")
        };
        var token = tokenService.CreateTokenByClaims(claims);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}