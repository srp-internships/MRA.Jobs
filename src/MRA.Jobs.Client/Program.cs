global using System.Net.Http.Json;
global using MRA.Jobs.Client.Services.CategoryServices;
global using MRA.Jobs.Client.Services.VacancyServices;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using MatBlazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MRA.Jobs.Client;
using MRA.Jobs.Client.Services.ApplicationService;
using MRA.Jobs.Client.Services.InternshipsServices;
using MRA.Jobs.Client.Services.TrainingServices;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Jobs.Client.Services.Auth;
using MudBlazor.Services;
using MRA.Jobs.Client.Services.Profile;
using System.Reflection;
using Microsoft.FeatureManagement;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.Client.Pages.Applicant;
using MRA.Jobs.Client.Services.FileService;
using MRA.Jobs.Client.Services.HiddenVacancies;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();


builder.Services.AddMudServices();
builder.Services.AddMatBlazor();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(typeof(RemoveUserSkillCommand).Assembly);

builder.Services.AddScoped(_ =>
    new IdentityHttpClient { BaseAddress = new Uri(builder.Configuration["IdentityHttpClient:BaseAddress"]!) });
builder.Services.AddScoped(_ =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration["HttpClient:BaseAddress"]!) });


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVacancyService, VacancyService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(s =>
{
    s.AddPolicy(ApplicationPolicies.Applicant, ac => ac
        .RequireRole(ApplicationClaimValues.Applicant, ApplicationClaimValues.Reviewer,
            ApplicationClaimValues.Administrator, ApplicationClaimValues.SuperAdmin)
        .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName,
            ApplicationClaimValues.AllApplications)
        .RequireClaim(ClaimTypes.Id).RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Username));

    s.AddPolicy(ApplicationPolicies.Reviewer, ac => ac
        .RequireRole(ApplicationClaimValues.Reviewer, ApplicationClaimValues.Administrator,
            ApplicationClaimValues.SuperAdmin)
        .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName,
            ApplicationClaimValues.AllApplications)
        .RequireClaim(ClaimTypes.Id).RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Username));

    s.AddPolicy(ApplicationPolicies.Administrator, ac => ac
        .RequireRole(ApplicationClaimValues.Administrator, ApplicationClaimValues.SuperAdmin)
        .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName,
            ApplicationClaimValues.AllApplications)
        .RequireClaim(ClaimTypes.Id).RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Username));
});
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IHiddenVacancyService, HiddenVacancyService>();
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"));
await builder.Build().RunAsync();