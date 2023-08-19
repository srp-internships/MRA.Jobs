namespace MRA.Identity.Api.ApplicationInsights;

public static class ApplicationInsightsConfig
{
    public static void AddApiApplicationInsights(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration["ApplicationInsights:ConnectionString"];

        builder.Logging.AddApplicationInsights(
            configureTelemetryConfiguration: (config) => config.ConnectionString = connectionString,
            configureApplicationInsightsLoggerOptions: (options) => { }
        );
    }
}
