using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Common.Seive;
using MRA.Jobs.Infrastructure.Identity.Models;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Persistence.Interceptors;
using MRA.Jobs.Infrastructure.Services;

namespace MRA.Jobs.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("data source=localhost; initial catalog=MRA_Jobs; integrated security=true; persist security info=true;TrustServerCertificate=True",
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<ISieveConfigurationsAssemblyMarker, InfrastructureSieveConfigurationsAssemblyMarker>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        //services.AddAuthentication()
        //    .AddIdentityServerJwt();

        //services.AddAuthorization(options =>
        //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}

public class InfrastructureSieveConfigurationsAssemblyMarker : ISieveConfigurationsAssemblyMarker { };

