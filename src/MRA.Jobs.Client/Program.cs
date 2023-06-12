global using System.Net.Http.Json;
global using MRA.Jobs.Client.Services.CategoryServices;
using MatBlazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MRA.Jobs.Client;
using MRA.Jobs.Client.Services.ApplicationService;
using MRA.Jobs.Client.Services.InternshipsServices;
using MRA.Jobs.Client.Services.JobServices;
using MRA.Jobs.Client.Services.TrainingServices;
using MRA.Jobs.Client.Services.VacancyServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMatBlazor();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") });

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IJobVacancyService, JobVacancyService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IVacancyService, VacancyService>();

await builder.Build().RunAsync();
