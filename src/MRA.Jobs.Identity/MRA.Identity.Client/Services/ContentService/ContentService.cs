using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Client.Resources;
using MRA.Identity.Client.Resources.Languages;

namespace MRA.Identity.Client.Services.ContentService;

public class ContentService(IStringLocalizer<English> english, IStringLocalizer<Russian> russian,
        IStringLocalizer<Tajik> tajik, ILocalStorageService localStorageService)
    : IContentService
{
    private static CultureInfo _applicationCulture = CultureInfo.CurrentCulture;

    public string this[string name]
    {
        get
        {
            return _applicationCulture.Name switch
            {
                ApplicationCulturesNames.En => english[name].Value,
                ApplicationCulturesNames.Ru => russian[name].Value,
                ApplicationCulturesNames.Tj => tajik[name].Value,
                _ => english[name]
            };
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
            _ => "en"
        };
    }

    public async Task InitializeCultureAsync()
    {
        var cultureName = await localStorageService.GetItemAsStringAsync(nameof(ApplicationCulturesNames));
        if (!cultureName.IsNullOrEmpty())
        {
            _applicationCulture = new CultureInfo(cultureName);
        }
    }
}