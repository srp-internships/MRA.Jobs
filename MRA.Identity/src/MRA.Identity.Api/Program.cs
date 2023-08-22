using MRA.Identity.Application;
using MRA.Identity.Infrastructure;
using Mra.Shared.Initializer.Azure.Insight;
using Mra.Shared.Initializer.Azure.KeyVault;

public class Program
{

    private static void Main(string[] args)
    {
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

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}