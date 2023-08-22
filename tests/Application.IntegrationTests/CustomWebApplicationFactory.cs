using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
    }

    IDictionary<string, string> GetInMemoryConfiguration()
    {
        Dictionary<string, string> inMemoryConfiguraton = new Dictionary<string, string>
            {
                { "UseInMemoryDatabase", "true" }
            };
        return inMemoryConfiguraton;
    }
}