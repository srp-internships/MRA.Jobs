using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace AuthController.IntegrationTest;

[TestFixture]
public class RegistrationTests
{
    private HttpClient _client = null!;
    private ApplicationDbContext _context = null!;
 
    /// <summary>
    /// Initializing of factory and _context. And Creating a InMemoryDB
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    [SetUp]
    public void Setup()
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
                        options.UseInMemoryDatabase("InMemoryDB");
                    });
                });
            });
        
        _context = factory.Services.GetService<IServiceScopeFactory>()!.CreateScope().ServiceProvider.GetService<ApplicationDbContext>()!;
        
        _client = factory.CreateClient();
    }

    [Test]
    public async Task Register_ValidRequestWithCorrectRegisterData_ReturnsOk()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex22",
            LastName = "Makedonskiy",
            PhoneNumber = "123456789"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/Auth/register", content);
        string path = "api/Auth/register";

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task Register_ValidRequestWithCorrectRegisterData_SavesUserIntoDb()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex22",
            LastName = "Makedonskiy",
            PhoneNumber = "123456789"
        };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        await _client.PostAsync("api/Auth/register", content);
        var result = _context.Users.Count();

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task Register_InvalidRequestWithWrongRegisterData_ReturnsBadRequest()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password", // incorrect password
            FirstName = "Alex",
            Username = "@Alex22",
            LastName = "Makedonskiy", 
            PhoneNumber = "123456789"
        };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
    
        // Act
        var response = await _client.PostAsync("api/Auth/register", content);
    
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task Register_InvalidRequestWithEmptyRegisterData_ReturnsBadRequest()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
           // Empty Register Data
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/register", JsonConvert.SerializeObject(request));
    
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}