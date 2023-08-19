using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Jobs.Infrastructure.Identity;
// using MRA.Jobs.Infrastructure.Identity.Entities;
using MRA.Jobs.Infrastructure.Persistence;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;

namespace MRA.Jobs.Application.IntegrationTests;

[SetUpFixture]
public class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Respawner _checkpoint = null!;
    private static Guid _currentUserId;


    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();

        _checkpoint = Respawner.CreateAsync(_configuration.GetConnectionString("DefaultConnection"),
            new RespawnerOptions { TablesToIgnore = new Table[] { "__EFMigrationsHistory" } }).GetAwaiter().GetResult();
    }


    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ISender mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static Guid GetCurrentUserId()
    {
        return _currentUserId;
    }

    public static async Task<Guid> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    }

    public static async Task<Guid> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Administrator" });
    }

    public static async Task<Guid> RunAsUserAsync(string userName, string password, string[] roles)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        // UserManager<ApplicationUser> userManager =
            // scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // ApplicationUser user = new ApplicationUser { UserName = userName, Email = userName };

        // IdentityResult result = await userManager.CreateAsync(user, password);

        if (roles.Any())
        {
            RoleManager<IdentityRole> roleManager =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (string role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            // await userManager.AddToRolesAsync(user, roles);
        }

        // if (result.Succeeded)
        // {
            // _currentUserId = user.Id;

            // return _currentUserId;
        // }

        // string errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{1}");
        
    }

    public static async Task ResetState()
    {
        try
        {
            await _checkpoint.ResetAsync(_configuration.GetConnectionString("DefaultConnection"));
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

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}