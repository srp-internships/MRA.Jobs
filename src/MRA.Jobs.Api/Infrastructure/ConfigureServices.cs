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
using MRA.Configurations.Common.Constants;
using MRA.Configurations.Initializer.Azure.EmailService;
using MRA.Configurations.Initializer.Services;

namespace MRA.Jobs.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAppIdentity(configuration);
        services.AddMediatR(typeof(ConfigureServices).Assembly);

        if (configuration["UseFileEmailService"] == "true")
        {
            services.AddFileEmailService();
        }
        else
        {
            services.AddAzureEmailService();
        }

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

       
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string dbConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (configuration["UseInMemoryDatabase"] == "true")
                options.UseInMemoryDatabase("testDb");
            else
                options.UseSqlServer(dbConnectionString);
        });
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<DbMigration>();
        services.AddScoped<ISieveConfigurationsAssemblyMarker, InfrastructureSieveConfigurationsAssemblyMarker>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ISmsService, GenericSmsService>();
        services.AddScoped<IHtmlService, HtmlService>();

        if (configuration["UseAzureBlobStorage"] == "true")
        {
            services.AddScoped<IFileService, AzureFileService>();
        }
        else
        {
            services.AddScoped<IFileService, FileService>();
        }
        return services;
    }

    private static IServiceCollection AddAppIdentity(this IServiceCollection services, IConfiguration configuration)
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
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddAuthorization(auth =>
        {
            auth.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationClaimValues.Applicant, ApplicationClaimValues.Reviewer,
                    ApplicationClaimValues.SuperAdmin,
                    ApplicationClaimValues.Administrator)
                .RequireClaim(ClaimTypes.Id)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName,
                    ApplicationClaimValues.AllApplications)
                .Build();

            auth.AddPolicy(ApplicationPolicies.Administrator, op => op
                .RequireRole(ApplicationClaimValues.Administrator, ApplicationClaimValues.SuperAdmin)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName,
                    ApplicationClaimValues.AllApplications));

            auth.AddPolicy(ApplicationPolicies.Reviewer, op => op
                .RequireRole(ApplicationClaimValues.Reviewer, ApplicationClaimValues.Administrator,
                    ApplicationClaimValues.SuperAdmin)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName,
                    ApplicationClaimValues.AllApplications));
        });
        return services;
    }
}

public class InfrastructureSieveConfigurationsAssemblyMarker : ISieveConfigurationsAssemblyMarker
{
}