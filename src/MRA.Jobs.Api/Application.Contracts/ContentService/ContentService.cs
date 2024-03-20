using Microsoft.Extensions.Localization;
using MRA.Jobs.Application.Contracts.Resources;

namespace MRA.Jobs.Application.Contracts.ContentService;

public class ContentService(
    IStringLocalizer<ValidatorMessageRussian> russian,
    IStringLocalizer<ValidatorMessageEnglish> english) : IContentService
{
    public string this[string name]
    {
        get
        {
            if (ValidatorOptions.Global.LanguageManager.Culture?.Name.Contains("ru",
                    StringComparison.OrdinalIgnoreCase) ?? false)
                return russian[name].Value;

            return english[name].Value;
        }
    }
}