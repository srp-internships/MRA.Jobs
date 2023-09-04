using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Persistence;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public abstract class BaseTest
{
    protected HttpClient _client { get; private set; } = null!;
    private CustomWebApplicationFactory _factory { get;  set; }
    
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
            Email = "test@example.com",
            UserName = "@Alex33",
            NormalizedUserName = "@alex33",
            PhoneNumber = "123456789",
        };
        
        await AddUser(request1,"password@#12P");
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
       var res= await dbContext.SaveChangesAsync();
    }
    
    protected Task<List<T>> GetAll<T>() where T : class
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return dbContext.Set<T>().ToListAsync();
    }

    protected Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return dbContext.Set<TEntity>().CountAsync();
    }
    protected async Task AddUser(ApplicationUser user,string password)
    {
        using var scope = _factory.Services.GetService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await dbContext.CreateAsync(user,password);
    }

    [SetUp]
    public void Setup()
    {
        _client = _factory.CreateClient();
    }
}