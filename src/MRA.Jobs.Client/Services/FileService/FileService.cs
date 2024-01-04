using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Client.Services.HttpClients;
using MudBlazor;

namespace MRA.Jobs.Client.Services.FileService;

public class FileService : IFileService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ISnackbar _snackbar;
    private readonly IConfiguration _configuration;

    public FileService(IHttpClientService httpClientService, ISnackbar snackbar, IConfiguration configuration)
    {
        _httpClientService = httpClientService;
        _snackbar = snackbar;
        _configuration = configuration;
    }

    public async Task<string> UploadAsync(IBrowserFile file)
    {
        var fileContent = new StreamContent(file.OpenReadStream()); //todo check file size;
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        var content = new MultipartFormDataContent();
        content.Add(fileContent, "\"file\"", file.Name);

        var fileUploadResponse = await _httpClientService.PostAsJsonAsync<FileUploadResponse>($"{_configuration["HttpClient:BaseAddress"]}api/File/upload", content);
        switch (fileUploadResponse.HttpStatusCode)
        {
            case HttpStatusCode.OK:
                _snackbar.Add("CV upload success", Severity.Success);
                return fileUploadResponse.Result.Key;
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