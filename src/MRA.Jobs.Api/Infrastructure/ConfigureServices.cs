using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Infrastructure.Identity;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Persistence.Interceptors;
using MRA.Jobs.Infrastructure.Services;
using Mra.Shared.Common.Constants;

namespace MRA.Jobs.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAppIdentity(configuration);
        services.AddMediatR(typeof(ConfigureServices).Assembly);


        //services.AddAzureEmailService();//uncomment this if u wont use email service from Azure from namespace Mra.Shared.Initializer.Azure.EmailService

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        string dbConnectionString = configuration.GetConnectionString("SqlServer");

        bool useInMemoryDatabase = Environment.GetEnvironmentVariable("USE_IN_MEMORY_DATABASE") == "true";

        if (useInMemoryDatabase)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabase");
            });
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(dbConnectionString, builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<ITestHttpClientService, TestHttpClientService>();

        services.AddScoped<ISieveConfigurationsAssemblyMarker, InfrastructureSieveConfigurationsAssemblyMarker>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ISmsService, GenericSmsService>();

        return services;
    }

    public static IServiceCollection AddAppIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddScoped<ICurrentUserService, CurrentUserService>();


        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(op =>
        {
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings")["SecurityKey"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddAuthorization(auth =>
        {
            auth.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            auth.AddPolicy(ApplicationPolicies.Administrator, op => op
                .RequireClaim(ClaimTypes.Role, ApplicationClaimValues.Administrator)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName));

            auth.AddPolicy(ApplicationPolicies.Reviewer, op => op
                .RequireClaim(ClaimTypes.Role, ApplicationClaimValues.Reviewer, ApplicationClaimValues.Administrator)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName));

            auth.AddPolicy(ApplicationPolicies.Applicant, op => op
                .RequireClaim(ClaimTypes.Role, ApplicationClaimValues.Applicant, ApplicationClaimValues.Reviewer,
                    ApplicationClaimValues.Administrator)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName));
        });
        //todo add requirement for id
        return services;
    }
}

public class InfrastructureSieveConfigurationsAssemblyMarker : ISieveConfigurationsAssemblyMarker
{
}