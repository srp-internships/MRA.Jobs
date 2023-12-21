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
using MRA.Jobs.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Configuration.ConfigureAzureKeyVault(ApplicationClaimValues.ApplicationName);
    builder.Configuration.AddAzureAppConfiguration(options => options.Connect("AppConfigConnectionString"));
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
}

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddWebUiServices(builder.Configuration);

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("MraJobs-Sieve"));



WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
}
else
{
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbMigration>();
    await initializer.InitialiseAsync();
    
    var dbInitializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await dbInitializer.SeedAsync();
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