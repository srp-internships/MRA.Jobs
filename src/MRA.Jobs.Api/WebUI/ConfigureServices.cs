using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Web.Filters;

namespace MRA.Jobs.Web;

public static class ConfigureServices
{
    public static void AddWebUiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddHealthChecks();

        services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssembly(typeof(CreateInternshipVacancyCommand).Assembly);

        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddCors();
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "MraJobsApi",
                Description = "MraJobsApi"
            });
        });

        services.AddMvc(options =>
        {
            options.Filters.Add(typeof(BadRequestResponseFilter));
        });
        var corsAllowedHosts = configuration.GetSection("CORS").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy("CORS_POLICY", policyConfig =>
            {
                policyConfig.WithOrigins(corsAllowedHosts)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
            });
        });
    }
}