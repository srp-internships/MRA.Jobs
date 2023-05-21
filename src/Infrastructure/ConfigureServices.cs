using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Jobs.Application.Common.Seive;
using MRA.Jobs.Infrastructure.Identity.Authorization;
using MRA.Jobs.Infrastructure.Identity.Entities;
using MRA.Jobs.Infrastructure.Identity.Services;
using MRA.Jobs.Infrastructure.Identity.Settings;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Persistence.Interceptors;
using MRA.Jobs.Infrastructure.Services;

namespace MRA.Jobs.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        var dbConectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(dbConectionString, builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<ISieveConfigurationsAssemblyMarker, InfrastructureSieveConfigurationsAssemblyMarker>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ISmsService, GenericSmsService>();

        return services;
    }

    public static IServiceCollection AddAppIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CurrentUserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<Microsoft.AspNetCore.Authentication.IClaimsTransformation, ClaimsTransformation>();
        services.AddScoped<IAuthorizationHandler, CheckCurrentUserAuthHandler>();

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = true;
            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;

        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddOptions();

        return services;
    }
}

public class InfrastructureSieveConfigurationsAssemblyMarker : ISieveConfigurationsAssemblyMarker { };

