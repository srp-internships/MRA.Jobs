using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MRA.Configurations.Common.Constants;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Application.ApplicationServices;

public class CvService : ICvService
{
    private readonly IFileService _fileService;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CvService(IFileService fileService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _fileService = fileService;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<string> GetCvByCommandAsync(ref CreateApplicationCommand command)
    {
        if (command.Cv.IsUploadCvMode)
        {
            return _fileService.UploadAsync(command.Cv.CvBytes, command.Cv.FileName);
        }

        return DownloadFromIdentityServerAsync();
    }

    private async Task<string> DownloadFromIdentityServerAsync()
    {
        var identityHttpClient = new HttpClient();
        identityHttpClient.BaseAddress = new Uri(_configuration["IdentityApi:BaseAddress"]!);
        identityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",_httpContextAccessor.HttpContext?.Request.Headers.Authorization[0]!.Split(' ')[1]);
        var stream = await identityHttpClient.GetStreamAsync(_configuration["IdentityApi:DownloadCvEndPoint"]);
        var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return await _fileService.UploadAsync(ms.ToArray(),$"{Guid.NewGuid()}_{_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Username)}.pdf");
    }
}