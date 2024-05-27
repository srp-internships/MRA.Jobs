using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MRA.Jobs.Application.Contracts.ContentService;

namespace MRA.Jobs.Application.Contracts;

public static class Configuration
{
    public static void AddMraFluentValidatorCustomMessages(this IServiceCollection services)
    {
        services.AddScoped<IContentService, ContentService.ContentService>();
    }
}