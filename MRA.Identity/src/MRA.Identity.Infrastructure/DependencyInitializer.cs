using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Common.Interfaces;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Persistence;

namespace MRA.Identity.Infrastructure;

public static class DependencyInitializer
{
    public static void AddInfrastructure(this IServiceCollection services,IConfiguration configurations)
    {
        var dbConnectionString = configurations.GetConnectionString("SqlServer");
        services.AddDbContext<ApplicationDbContext>(s => s
            .UseSqlServer(dbConnectionString));
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // options.User.RequireUniqueEmail = true;
                // options.SignIn.RequireConfirmedPhoneNumber = false;
                // options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                // options.Password.RequireLowercase = true;
                // options.Password.RequireUppercase = true;
                // options.Password.RequireDigit = true;
                // options.Password.RequiredLength = 8;

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

    }
}