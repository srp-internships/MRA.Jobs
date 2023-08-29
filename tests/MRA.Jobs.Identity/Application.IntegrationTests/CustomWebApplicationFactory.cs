using MediatR.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mra.Shared.Common.Interfaces.Services;
using MRA.Identity.Infrastructure.Persistence;
using MRA.Jobs.Application.IntegrationTests.Email;

namespace MRA.Jobs.Application.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var inMemoryConfiguration = GetInMemoryConfiguration();
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .AddEnvironmentVariables()
                .Build();
            configurationBuilder.AddConfiguration(integrationConfig);
        });
        builder.ConfigureTestServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var description =
               new ServiceDescriptor(
                   typeof(IEmailService),
                   typeof(TestEmailSenderService),
                   ServiceLifetime.Transient);
            services.Replace(description);
        });
    }

    IDictionary<string, string> GetInMemoryConfiguration()
    {
        Dictionary<string, string> inMemoryConfiguraton = new Dictionary<string, string>
        {
            { "UseInMemoryDatabase", "true" },
        };
        return inMemoryConfiguraton;
    }
}