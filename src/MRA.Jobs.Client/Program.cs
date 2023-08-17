global using System.Net.Http.Json;
global using MRA.Jobs.Client.Services.CategoryServices;
global using MRA.Jobs.Client.Services.VacancyServices;
using Blazored.LocalStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MRA.Jobs.Client;
using MRA.Jobs.Client.Services.ApplicationService;
using MRA.Jobs.Client.Services.InternshipsServices;
using MRA.Jobs.Client.Services.TrainingServices;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMatBlazor();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVacancyService, VacancyService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();