using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MRA.Jobs.Application;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Infrastructure;
using MRA.Jobs.Infrastructure.Identity;
using MRA.Jobs.Infrastructure.Services;
using MRA.Jobs.Web;
using Mra.Shared.Initializer.Azure.Insight;
using Mra.Shared.Initializer.Azure.KeyVault;
using Newtonsoft.Json;
using Sieve.Models;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
    builder.Configuration.ConfigureAzureKeyVault(ApplicationClaimValues.ApplicationName);
}

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddWebUiServices(builder.Configuration);

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, SmtpEmailService>();
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/status", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = new
        {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(entry => new
            {
                Name = entry.Key, Status = entry.Value.Status.ToString(), entry.Value.Description
            })
        };

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(config =>
{
    config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});
app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
    settings.EnableTryItOut = true;
    settings.PersistAuthorization = true;
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/api"));

app.Run();