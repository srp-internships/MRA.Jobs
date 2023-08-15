using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MRA.Jobs.Application;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Infrastructure;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Services;
using MRA.Jobs.Web;
using Newtonsoft.Json;
using Sieve.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("dbsettings.json", optional: true);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUiServices(builder.Configuration);
builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, SmtpEmailService>();
builder.Services.AddMediatR(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                Name = entry.Key,
                Status = entry.Value.Status.ToString(),
                Description = entry.Value.Description
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

app.MapGet("/", () =>
{
    return Results.Redirect("/api");
});

app.Run();