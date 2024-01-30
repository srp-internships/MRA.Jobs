using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Services;
using MudBlazor;

namespace MRA.Jobs.Client.Services.FileService;

public class FileService(HttpClientService httpClientService, ISnackbar snackbar, IConfiguration configuration)
    : IFileService
{
    public async Task<string> UploadAsync(IBrowserFile file)
    {
        var fileContent = new StreamContent(file.OpenReadStream()); //todo check file size;
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        var content = new MultipartFormDataContent();
        content.Add(fileContent, "\"file\"", file.Name);

        var fileUploadResponse =
            await httpClientService.PostAsJsonAsync<FileUploadResponse>(configuration.GetJobsUrl("api/File/upload"),
                content);
        switch (fileUploadResponse.HttpStatusCode)
        {
            case HttpStatusCode.OK:
                snackbar.Add("CV upload success", Severity.Success);
                return fileUploadResponse.Result.Key;
            default:
                snackbar.Add("CV upload error", Severity.Error);
                return "";
        }
    }
}

internal class FileUploadResponse
{
    [JsonPropertyName("key")] public string Key { get; set; }
}