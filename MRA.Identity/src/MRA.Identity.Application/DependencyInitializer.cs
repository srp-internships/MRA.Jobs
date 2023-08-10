using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Features.Applicants;
using MRA.Identity.Application.Features.Applicants.Command.LoginUser;
using MRA.Identity.Application.Features.Applicants.Command.RegisterUser;
using MRA.Identity.Application.Services;

namespace MRA.Identity.Application;

public static class DependencyInitializer
{
    public static void AddApplication(this IServiceCollection services,IConfiguration configurations)
    {
        services.AddOptions();
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // services.AddMediatR(typeof(RegisterUserCommandHandler).GetTypeInfo().Assembly);
        // services.AddMediatR(typeof(LoginUserCommandHandler).GetTypeInfo().Assembly);
        services.AddAutoMapper(typeof(UserProfile));
        JwtTokenService.SecretKey = configurations.GetSection("JwtSettings")["SecurityKey"] ??
                                    throw new NullReferenceException(
                                        "Fill security key in configurations file where section=JwtSettings and key=SecurityKey");
        services.AddScoped<IPasswordService, PasswordService>();

        var assem = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assem);



        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }
}