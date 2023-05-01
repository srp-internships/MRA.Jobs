using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRA_Jobs.Application.Abstractions;
using MRA_Jobs.Infrastructure;
using MRA_Jobs.Infrastructure.Persistence;
using MRA_Jobs.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(),
                                AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped(typeof(IEntityService<>),typeof(EntityService<>));
        services.AddScoped<ICategoryService, CategoryService>();

        


        //services
        //    .AddDefaultIdentity<ApplicationUser>()
        //    .AddRoles<IdentityRole>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>();

        //services.AddIdentityServer()
        //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        //services.AddTransient<IDateTime, DateTimeService>();
        //services.AddTransient<IIdentityService, IdentityService>();
        //services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        //services.AddAuthentication()
        //    .AddIdentityServerJwt();

        //services.AddAuthorization(options =>
        //        options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}