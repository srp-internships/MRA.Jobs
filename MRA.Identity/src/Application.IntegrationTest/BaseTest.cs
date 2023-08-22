using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Infrastructure.Persistence;

namespace Application.IntegrationTest;

[TestFixture]
public abstract class BaseTest
{
    protected HttpClient _client { get; private set; } = null!;
    protected ApplicationDbContext _context  { get; private set; }= null!;

    
    /// <summary>
    /// Initializing of factory and _context. And Creating a InMemoryDB
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    [OneTimeSetUp]
    public Task Setup()
    {
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(host =>
            {
                host.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<ApplicationDbContext>));
    
                    services.Remove(descriptor ?? throw new NullReferenceException());
    
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(GetType().ToString());
                    });
                });
            });
        
        _context = factory.Services.GetService<IServiceScopeFactory>()!.CreateScope().ServiceProvider.GetService<ApplicationDbContext>()!;
        
        _client = factory.CreateClient();
        return Task.CompletedTask;
    }
}