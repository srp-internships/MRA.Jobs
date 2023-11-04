using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MRA.Configurations.Common.Interfaces.Services;

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

            var descriptor = new ServiceDescriptor(typeof(ISmsService), typeof(TestSmsSenderService), ServiceLifetime.Scoped);
            services.Replace(descriptor);
        });
    }

    IDictionary<string, string> GetInMemoryConfiguration()
    {
        Dictionary<string, string> inMemoryConfiguration = new()
        {
            { "UseInMemoryDatabase", "true" }, { "UseFileEmailService", "true" }, { "UseFileSmsService", "true" }
        };
        return inMemoryConfiguration;
    }
}