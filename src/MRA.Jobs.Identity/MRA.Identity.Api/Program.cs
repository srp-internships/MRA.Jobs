using MRA.Identity.Application;
using MRA.Identity.Infrastructure;
using Mra.Shared.Initializer.Azure.Insight;
using Mra.Shared.Initializer.Azure.KeyVault;
using MRA.Identity.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
    builder.Configuration.ConfigureAzureKeyVault("Mra.Identity");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var applicationDbContextInitializer = app.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
await applicationDbContextInitializer.SeedAsync();

app.UseCors(config =>
{
    config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();