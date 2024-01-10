using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.ClientWebApp.Components;
using MRA.Identity.ClientWebApp.Services;
using MRA.Identity.ClientWebApp.Services.Auth;
using MRA.Identity.ClientWebApp.Services.ContentService;
using MRA.Identity.ClientWebApp.Services.Profile;
using MRA.Identity.ClientWebApp.Services.UserPreferences;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMudServices();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.Configuration["HttpClient:BaseAddress"]) });

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddLocalization();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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
