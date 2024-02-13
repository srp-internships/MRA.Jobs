global using System.Net.Http.Json;
global using MRA.Jobs.Client.Services.CategoryServices;
global using MRA.Jobs.Client.Services.VacancyServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MRA.Jobs.Client;
using MRA.Jobs.Client.Services.ApplicationService;
using MRA.Jobs.Client.Services.InternshipsServices;
using MRA.Jobs.Client.Services.TrainingServices;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using MRA.Jobs.Client.Services.Profile;
using System.Reflection;
using AltairCA.Blazor.WebAssembly.Cookie.Framework;
using Blazored.LocalStorage;
using Microsoft.FeatureManagement;
using MRA.BlazorComponents;
using MRA.BlazorComponents.DynamicPages;
using MRA.BlazorComponents.HttpClient;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.Client.Services.ContentService;
using MRA.Jobs.Client.Services.ConverterService;
using MRA.Jobs.Client.Services;
using MRA.Jobs.Client.Services.FileService;
using MRA.Jobs.Client.Services.NoVacancies;
using MRA.Jobs.Client.Services.UserPreferences;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();

builder.Services.AddAltairCACookieService(options => { options.DefaultExpire = TimeSpan.Zero; });
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(typeof(RemoveUserSkillCommand).Assembly);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IJobsService, JobsService>();
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

//Mra.BlazorComponents
builder.Services.AddHttpClientService();
builder.Services.AddMraPages();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
//Mra.BlazorComponents

builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<LayoutService>();

builder.Services.AddScoped<INoVacancyService, NoVacancyService>();
builder.Services.AddFeatureManagement();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddLocalization();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IDateTimeConvertToStringService, DateTimeConverterToStringService>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();