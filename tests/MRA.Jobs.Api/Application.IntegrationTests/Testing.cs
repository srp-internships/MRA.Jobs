using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Jobs.Application.IntegrationTests.Common.Interfaces;
using MRA.Jobs.Application.IntegrationTests.Common.Services;
using MRA.Jobs.Infrastructure.Identity;
using MRA.Jobs.Infrastructure.Persistence;
using NUnit.Framework;
using ClaimTypes = MRA.Configurations.Common.Constants.ClaimTypes;

namespace MRA.Jobs.Application.IntegrationTests;

public enum UserRoles
{
    DefaultUser, 
    Reviewer,
    Admin,
}
public class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Guid _currentUserId;
    private static IJwtTokenService _tokenService;
    protected HttpClient _httpClient;
    protected string jwtToken;
    private static bool _initialized;
    private static readonly Random Random = new();

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        if (_initialized)
            return;

        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();
        _tokenService = new JwtTokenService(_configuration);
        _initialized = true;
    }

    [SetUp]
    public void SetupTest()
    {
        _httpClient = _factory.CreateClient();
    }

    public static string CreateJwtToken(IList<Claim> claims)
    {
        return _tokenService.CreateTokenByClaims(claims);
    }

    public static Guid GetCurrentUserId()
    {
        return _currentUserId;
    }

    protected void RunAsRoleAsync(UserRoles role)
    {
        switch (role)
        {
            case UserRoles.Reviewer:
                RunAsReviewerAsync();
                break;
            case UserRoles.Admin:
                RunAsAdministratorAsync();
                break;
            case UserRoles.DefaultUser:
                RunAsDefaultUserAsync("applicant");
                break;
        }
    }
    
    public void RunAsDefaultUserAsync(string username)
    {
        Guid userId = Guid.NewGuid();
        _currentUserId = userId;
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, "fakeEmail@asd.com"),
            new(ClaimTypes.Id, userId.ToString()),
            new(ClaimTypes.Username, username)
        };
        jwtToken = _tokenService.CreateTokenByClaims(claims);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    public void RunAsReviewerAsync()
    {
        Guid userId = Guid.NewGuid();
        _currentUserId = userId;
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, "fakeEmail@asd.com"),
            new(ClaimTypes.Role, ApplicationClaimValues.Reviewer),
            new(ClaimTypes.Application, ApplicationClaimValues.ApplicationName),
            new(ClaimTypes.Id, userId.ToString()),
            new(ClaimTypes.Username, "username")
        };
        jwtToken = _tokenService.CreateTokenByClaims(claims);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    public void RunAsAdministratorAsync()
    {
        Guid userId = Guid.NewGuid();
        _currentUserId = userId;
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, "fakeEmail@asd.com"),
            new(ClaimTypes.Role, ApplicationClaimValues.Administrator),
            new(ClaimTypes.Application, ApplicationClaimValues.ApplicationName),
            new(ClaimTypes.Id, userId.ToString()),
            new(ClaimTypes.Username, "username")
        };
        jwtToken = _tokenService.CreateTokenByClaims(claims);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    public static void ResetState()
    {
        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        catch (Exception) { }

        _currentUserId = Guid.Empty;
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task RemoveAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Remove(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public static async Task<TEntity> FindFirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> criteria)
        where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.Set<TEntity>().FirstOrDefaultAsync(criteria);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
    
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}


