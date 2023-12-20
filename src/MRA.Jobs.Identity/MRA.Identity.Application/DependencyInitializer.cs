using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Common.Behaviours;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Features.Users;
using MRA.Identity.Application.Services;

namespace MRA.Identity.Application;

public static class DependencyInitializer
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddScoped<IGoogleTokenService, TokenService>();

        services.AddAutoMapper(typeof(UsersProfile).Assembly);

        Assembly assem = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assem);

        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }
}