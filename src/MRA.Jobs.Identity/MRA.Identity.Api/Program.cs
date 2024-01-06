using MRA.Identity.Application;
using MRA.Identity.Infrastructure;
using MRA.Identity.Infrastructure.Persistence;
using MRA.Identity.Api.Filters;
using FluentValidation;
using MRA.Identity.Application.Contract.Skills.Command;
using FluentValidation.AspNetCore;
using MRA.Configurations.Initializer.Azure.Insight;
using MRA.Configurations.Initializer.Azure.KeyVault;
using MRA.Configurations.Initializer.Azure.AppConfig;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Configuration.ConfigureAzureKeyVault("MRAIdentity");
    string appConfigConnectionString = builder.Configuration["AppConfigConnectionString"];
    builder.Configuration.AddAzureAppConfig(appConfigConnectionString);
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
}
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(RemoveUserSkillCommand).Assembly);


WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbMigration>();
    await initializer.InitialiseAsync();
}

var applicationDbContextInitializer = app.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
await applicationDbContextInitializer.SeedAsync();

app.UseCors("CORS_POLICY");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();