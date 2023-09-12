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
using ClaimTypes = Mra.Shared.Common.Constants.ClaimTypes;

namespace MRA.Jobs.Application.IntegrationTests;

public class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Guid _currentUserId;
    private static IJwtTokenService _tokenService;
    protected HttpClient _httpClient;
    protected string jwtToken;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();
        _tokenService = new JwtTokenService(_configuration);
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

    public void RunAsDefaultUserAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Application, ApplicationClaimValues.ApplicationName),
            new(ClaimTypes.Id,Guid.NewGuid().ToString())
        };
        jwtToken = _tokenService.CreateTokenByClaims(claims);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }
    public void RunAsReviewerAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, ApplicationClaimValues.Reviewer),
            new(ClaimTypes.Application, ApplicationClaimValues.ApplicationName),
            new(ClaimTypes.Id,Guid.NewGuid().ToString())
        };
        jwtToken = _tokenService.CreateTokenByClaims(claims);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    public void RunAsAdministratorAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, ApplicationClaimValues.Administrator),
            new(ClaimTypes.Application, ApplicationClaimValues.ApplicationName),
            new(ClaimTypes.Id,Guid.NewGuid().ToString())
        };
        jwtToken = _tokenService.CreateTokenByClaims(claims);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    

    public static async Task ResetState()
    {
        try
        {
            //await _checkpoint.ResetAsync(_configuration.GetConnectionString("DefaultConnection"));
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

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public static async Task<TEntity> FindFirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.Set<TEntity>().FirstOrDefaultAsync(criteria);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}