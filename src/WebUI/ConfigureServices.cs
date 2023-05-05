using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using MRA.Jobs.Web.Services;
using MRA.Jobs.Web.Filters;

namespace MRA.Jobs.Web;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddRazorPages();

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "MRA.Jobs API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });
            
            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        return services;
    }
}
