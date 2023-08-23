using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Api.ApplicationInsights;
using MRA.Identity.Api.AzureKeyVault;
using MRA.Identity.Application;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure;
using MRA.Identity.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.AddApiApplicationInsights();
    builder.ConfigureAzureKeyVault();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
//builder.Services.AddScoped<ApplicationDbContextInitializer>();


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var initializer = app.Services.CreateScope().ServiceProvider.GetService<ApplicationDbContextInitializer>();

await initializer.SeedAsync();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


    

