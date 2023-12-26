using Microsoft.Extensions.Configuration;

namespace MRA.Jobs.WebUI.AcceptanceTests;

public static class ConfigurationHelper
{
    private readonly static IConfiguration Configuration;

    static ConfigurationHelper()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
    }

    private static string? _baseUrl;

    public static string? GetBaseUrl()
    {
        if (_baseUrl == null)
        {
            _baseUrl = Configuration["BaseUrl"];
            _baseUrl = _baseUrl?.TrimEnd('/');
        }

        return _baseUrl;
    }
}
