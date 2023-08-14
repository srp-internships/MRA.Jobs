using System.Reflection;
using AutoMapper.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Jobs.Application.Common.Behaviours;
using MRA.Jobs.Application.Common.Seive;
using MRA.Jobs.Application.Common.SlugGeneratorService;


using Sieve.Services;

namespace MRA.Jobs.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(config =>
        {
            config.Internal().MethodMappingEnabled = false;
        }, typeof(IApplicationMarker).Assembly);

        services.AddSingleton<ISlugGeneratorService, SlugGeneratorService>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddScoped<ISieveConfigurationsAssemblyMarker, ApplicationSieveConfigurationsAssemblyMarker>();
      
        services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
        services.AddScoped<IApplicationSieveProcessor, ApplicationSieveProcessor>();
        return services;
    }
}

public interface IApplicationMarker
{
}