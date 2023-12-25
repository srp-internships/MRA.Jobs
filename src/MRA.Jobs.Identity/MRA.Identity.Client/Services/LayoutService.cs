using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Client.Enums;
using MRA.Identity.Client.Resources;
using MRA.Identity.Client.Services.ContentService;
using MRA.Identity.Client.Services.UserPreferences;
using MudBlazor;

namespace MRA.Identity.Client.Services;

public class LayoutService(IUserPreferencesService userPreferencesService, IContentService contentService)
{
    public UserProfileResponse User;

    private UserPreferences.UserPreferences _userPreferences;
    private bool _systemPreferences;

    public bool IsRtl { get; private set; } = false;
    public DarkLightMode DarkModeToggle = DarkLightMode.System;

    public bool IsDarkMode { get; private set; }

    public MudTheme CurrentTheme { get; private set; }

    public void SetDarkMode(bool value)
    {
        IsDarkMode = value;
    }

    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _systemPreferences = isDarkModeDefaultTheme;
        _userPreferences = await userPreferencesService.LoadUserPreferences();
        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.DarkLightTheme switch
            {
                DarkLightMode.Dark => true,
                DarkLightMode.Light => false,
                DarkLightMode.System => isDarkModeDefaultTheme,
                _ => IsDarkMode
            };
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences.UserPreferences { DarkLightTheme = DarkLightMode.System };
            await userPreferencesService.SaveUserPreferences(_userPreferences);
        }

        var lang = await contentService.GetCurrentCulture();
        _lang = lang.IsNullOrEmpty() ? "En" : lang;
    }

    public async Task OnSystemPreferenceChanged(bool newValue)
    {
        _systemPreferences = newValue;
        if (DarkModeToggle == DarkLightMode.System)
        {
            IsDarkMode = newValue;
            OnMajorUpdateOccured();
        }
    }

    public event EventHandler MajorUpdateOccured;

    private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

    public async Task ToggleDarkMode()
    {
        switch (DarkModeToggle)
        {
            case DarkLightMode.System:
                DarkModeToggle = DarkLightMode.Light;
                IsDarkMode = false;
                break;
            case DarkLightMode.Light:
                DarkModeToggle = DarkLightMode.Dark;
                IsDarkMode = true;
                break;
            case DarkLightMode.Dark:
                DarkModeToggle = DarkLightMode.System;
                IsDarkMode = _systemPreferences;
                break;
        }

        _userPreferences.DarkLightTheme = DarkModeToggle;
        await userPreferencesService.SaveUserPreferences(_userPreferences);
        OnMajorUpdateOccured();
    }

    public string _lang = "En";

    public async Task ChangeLanguage(string lang)
    {
        _lang = lang;

        if (lang == "Ru")
            await contentService.ChangeCulture(ApplicationCulturesNames.Ru);
        if (lang == "En")
            await contentService.ChangeCulture(ApplicationCulturesNames.En);
        if (lang == "Tj")
            await contentService.ChangeCulture(ApplicationCulturesNames.Tj);

        OnMajorUpdateOccured();
    }

    public void SetBaseTheme(MudTheme theme)
    {
        CurrentTheme = theme;
        OnMajorUpdateOccured();
    }

    public DocPages GetDocsBasePage(string uri)
    {
        if (uri.Contains("/jobs")) return DocPages.Jobs;
        if (uri.Contains("/internships")) return DocPages.Internships;
        if (uri.Contains("/trainings")) return DocPages.Trainings;
        if (uri.Contains("/contact")) return DocPages.Contact;
        if (uri.Contains("/profile")) return DocPages.Profile;
        if (uri.Contains("/applications")) return DocPages.Applications;
        if (uri.Contains("/upload-cv")) return DocPages.NoVacancyUploadCv;
        return DocPages.Home;
    }

}