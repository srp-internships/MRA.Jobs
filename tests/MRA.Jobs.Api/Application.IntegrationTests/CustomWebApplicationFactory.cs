using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Jobs.Application.IntegrationTests.FakeClasses;

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
        builder.ConfigureServices(services =>
        {
            var httpClientFactoryDescriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                                            typeof(IHttpClientFactory));
            services.Remove(httpClientFactoryDescriptor);

            services.AddScoped<IHttpClientFactory, FakeHttpClientFactory>();
        });
    }

    IDictionary<string, string> GetInMemoryConfiguration()
    {
        Dictionary<string, string> inMemoryConfiguration = new()
        {
            { "UseInMemoryDatabase", "true" }, { "UseFileEmailService", "true" }
        };
        return inMemoryConfiguration;
    }
}