using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.MraPagesServices;

public class PagesService(PagesApiHttpClientService httpClient)
{
    public async Task<string> GetHtmlContentAsync(string pageName)
    {
        var lang = ContentService.ContentService.ApplicationCulture.Name;
        var pagesResponse =
            await httpClient.GetAsJsonAsync<ContentResponse>($"pages/getContent?lang={lang}&pageName={pageName}");
        return pagesResponse.Success ? pagesResponse.Result.HtmlContent : "";
    }

    public async Task<List<PageResponse>> GetPagesAsync()
    {
        var lang = ContentService.ContentService.ApplicationCulture.Name;
        var pagesResponse = await httpClient.GetAsJsonAsync<List<PageResponse>>($"pages?lang={lang}");
        return pagesResponse.Success ? pagesResponse.Result : [];
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class PageResponse
{
    public string Title { get; set; }
    public string Name { get; set; }
    public bool ShowInMenu { get; set; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ContentResponse
{
    public string HtmlContent { get; set; }
}