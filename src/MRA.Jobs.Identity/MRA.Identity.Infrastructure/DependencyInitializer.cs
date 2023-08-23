using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mra.Shared.Initializer.Azure.EmailService;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Persistence;

namespace MRA.Identity.Infrastructure;

public static class DependencyInitializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurations)
    {
        string? dbConnectionString = configurations.GetConnectionString("SqlServer");
        services.AddDbContext<ApplicationDbContext>(s => s
            .UseSqlServer(dbConnectionString));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAzureEmailService();
    }
}