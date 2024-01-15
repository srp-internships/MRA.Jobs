global using System.Net.Http.Json;
global using MRA.Jobs.SSR.Client.Services.CategoryServices;
global using MRA.Jobs.SSR.Client.Services.VacancyServices;
using System.Reflection;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FeatureManagement;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Jobs.Client;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.SSR.Client;
using MRA.Jobs.SSR.Client.Components;
using MRA.Jobs.SSR.Client.Services;
using MRA.Jobs.SSR.Client.Services.ApplicationService;
using MRA.Jobs.SSR.Client.Services.Auth;
using MRA.Jobs.SSR.Client.Services.ContentService;
using MRA.Jobs.SSR.Client.Services.ConverterService;
using MRA.Jobs.SSR.Client.Services.FileService;
using MRA.Jobs.SSR.Client.Services.HttpClients;
using MRA.Jobs.SSR.Client.Services.InternshipsServices;
using MRA.Jobs.SSR.Client.Services.NoVacancies;
using MRA.Jobs.SSR.Client.Services.Profile;
using MRA.Jobs.SSR.Client.Services.TrainingServices;
using MRA.Jobs.SSR.Client.Services.UserPreferences;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
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

builder.Services.AddHttpClient();
builder.Services.AddScoped<JobsApiHttpClientService>();
builder.Services.AddScoped<IdentityApiHttpClientService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
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

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();