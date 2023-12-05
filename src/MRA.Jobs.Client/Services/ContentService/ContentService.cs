using System.Globalization;
using Blazored.LocalStorage;
using Blazorise.Extensions;
using Microsoft.Extensions.Localization;
using MRA.Jobs.Client.wwwroot.resources;
using MRA.Jobs.Client.wwwroot.resources.languages;

namespace MRA.Jobs.Client.Services.ContentService;

public class ContentService(IStringLocalizer<English> english, IStringLocalizer<Russian> russian,
        IStringLocalizer<Tajik> tajik, ILocalStorageService localStorageService)
    : IContentService
{
    private static CultureInfo _applicationCulture = CultureInfo.CurrentCulture;

    public string this[string name]
    {
        get
        {
            Console.WriteLine(name + @" is key");
            Console.WriteLine(_applicationCulture.Name + @" is culture for now");
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

    public async Task InitializeCultureAsync()
    {
        Console.WriteLine(@"Initializing");
        var cultureName = await localStorageService.GetItemAsStringAsync(nameof(ApplicationCulturesNames));
        Console.WriteLine(cultureName + @"i s cultureName");
        if (!cultureName.IsNullOrEmpty())
        {
            _applicationCulture = new CultureInfo(cultureName);
        }
    }
}