using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace MRA.Jobs.Client.Services.FileService;

public class FileService : IFileService
{
    private readonly HttpClient _client;
    private readonly ISnackbar _snackbar;

    public FileService(HttpClient client, ISnackbar snackbar)
    {
        _client = client;
        _snackbar = snackbar;
    }

    public async Task<string> UploadAsync(IBrowserFile file)
    {
        var fileContent = new StreamContent(file.OpenReadStream()); //todo check file size;
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        var content = new MultipartFormDataContent();
        content.Add(fileContent, "\"file\"", file.Name);

        var fileUploadResponse = await _client.PostAsync("/api/File/upload", content);
        switch (fileUploadResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                _snackbar.Add("CV upload success", Severity.Success);
                return (await fileUploadResponse.Content.ReadFromJsonAsync<FileUploadResponse>()).Key;
            default:
                _snackbar.Add("CV upload error", Severity.Error);
                return "";
        }
    }
}

internal class FileUploadResponse
{
    [JsonPropertyName("key")] public string Key { get; set; }
}