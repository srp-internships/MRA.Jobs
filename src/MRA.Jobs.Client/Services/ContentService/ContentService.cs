using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.Extensions.Localization;
using Microsoft.FeatureManagement;
using MRA.Jobs.Client.Resources;
using MRA.Jobs.Client.Resources.Languages;
using MRA.Jobs.Client.Shared;

namespace MRA.Jobs.Client.Services.ContentService;

public class ContentService(
    IStringLocalizer<English> english,
    IStringLocalizer<Russian> russian,
    IStringLocalizer<Tajik> tajik,
    ILocalStorageService localStorageService,
    IFeatureManager featureManager)
    : IContentService
{
    private bool _en;
    private bool _ru;
    private bool _tj;

    private static CultureInfo _applicationCulture = new(ApplicationCulturesNames.Ru);

    public string this[string name]
    {
        get
        {
            if (_applicationCulture.Name == ApplicationCulturesNames.En && _en)
                return english[name].Value;
            if (_applicationCulture.Name == ApplicationCulturesNames.Ru && _ru)
                return russian[name].Value;
            if (_applicationCulture.Name == ApplicationCulturesNames.Tj && _tj)
                return tajik[name].Value;
            return russian[name];
        }
    }

    public async Task ChangeCulture(string name)
    {
        _applicationCulture = new CultureInfo(name);
        await localStorageService.SetItemAsStringAsync(nameof(ApplicationCulturesNames), name);
    }

    public async Task<string> GetCurrentCulture()
    {
        var cultureName = await localStorageService.GetItemAsStringAsync(nameof(ApplicationCulturesNames));
        return cultureName switch
        {
            ApplicationCulturesNames.En => "En",
            ApplicationCulturesNames.Ru => "Ru",
            ApplicationCulturesNames.Tj => "Tj",
            _ => "ru"
        };
    }

    public async Task InitializeCultureAsync()
    {
        var cultureName = await localStorageService.GetItemAsStringAsync(nameof(ApplicationCulturesNames));
        if (!string.IsNullOrEmpty(cultureName))
        {
            _applicationCulture = new CultureInfo(cultureName);
        }
        _en = await featureManager.IsEnabledAsync(FeatureFlags.En);
        _ru = await featureManager.IsEnabledAsync(FeatureFlags.Ru);
        _tj = await featureManager.IsEnabledAsync(FeatureFlags.Tj);
    }
}