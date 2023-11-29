using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MRA.Jobs.Application;
using MRA.Jobs.Infrastructure;
using MRA.Jobs.Infrastructure.Identity;
using MRA.Jobs.Web;
using MRA.Configurations.Initializer.Azure.Insight;
using MRA.Configurations.Initializer.Azure.KeyVault;
using Newtonsoft.Json;
using Sieve.Models;
using FluentValidation.AspNetCore;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Configuration.ConfigureAzureKeyVault(ApplicationClaimValues.ApplicationName);
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
}

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddWebUiServices(builder.Configuration);

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));



WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<DbMigration>();
    await initialiser.InitialiseAsync();
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/api";
        settings.DocumentPath = "/api/specification.json";
        settings.EnableTryItOut = true;
        settings.PersistAuthorization = true;
    });
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
                Name = entry.Key,
                Status = entry.Value.Status.ToString(),
                entry.Value.Description
            })
        };

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CORS_POLICY");


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/api"));

app.Run();