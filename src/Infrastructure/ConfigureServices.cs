using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Persistence.Interceptors;
using MRA.Jobs.Infrastructure.Services;

namespace MRA.Jobs.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAppIdentity(configuration);
        services.AddMediatR(typeof(ConfigureServices).Assembly);

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        string dbConnectionString = configuration.GetConnectionString("SqlServer");

        services.AddDbContext<ApplicationDbContext>(options =>

            options.UseSqlServer(dbConectionString, builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))

        );
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
       
        services.AddScoped<ITestHttpClientService, TestHttpClientService>();

        services.AddScoped<ISieveConfigurationsAssemblyMarker, InfrastructureSieveConfigurationsAssemblyMarker>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ISmsService, GenericSmsService>();

        return services;
    }

    public static IServiceCollection AddAppIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        // services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddOptions();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        // services.AddScoped<IClaimsTransformation, ClaimsTransformation>();
        // services.AddScoped<IAuthorizationHandler, CheckCurrentUserAuthHandler>();


        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddAuthorization(auth =>
        {
            auth.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                // .AddRequirements(new CheckCurrentUserRequirement())
                .Build();
        });

        return services;
    }
}

public class InfrastructureSieveConfigurationsAssemblyMarker : ISieveConfigurationsAssemblyMarker
{
}