using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MRA.Jobs.Infrastructure.Persistence;

namespace MRA.Jobs.Application.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            IConfigurationRoot integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            services.RemoveAll(typeof(ApplicationDbContext));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var builderConf = builder.Configuration;

                //options.UseInMemoryDatabase("InMemoryDatabase");
            });
        });        
    }
}