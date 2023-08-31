using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Persistence;
using Mra.Shared.Initializer.Azure.EmailService;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Infrastructure.Account.Services;
using Mra.Shared.Initializer.Services;

namespace MRA.Identity.Infrastructure;

public static class DependencyInitializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurations)
    {
        string? dbConnectionString = configurations.GetConnectionString("SqlServer");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (configurations["UseInMemoryDatabase"] == "true")
                options.UseInMemoryDatabase("testDb");
            else
                options.UseSqlServer(dbConnectionString);
        });

        services.AddScoped<IEmailVerification, EmailVerification>();

        services.AddScoped<IUserHttpContextAccessor, UserHttpContextAccessor>();


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

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        if (configurations["UseFileEmailService"] == "true")
        {
            services.AddFileEmailService();
        }
        else
        {
            services.AddAzureEmailService(); //uncomment this if u wont use email service from Azure from namespace Mra.Shared.Initializer.Azure.EmailService
        }
    }
}