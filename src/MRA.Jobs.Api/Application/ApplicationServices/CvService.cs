using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MRA.Configurations.Common.Constants;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Application.ApplicationServices;

public class CvService(IFileService fileService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
        IHttpClientFactory factory)
    : ICvService
{
    public Task<string> GetCvByCommandAsync(ref CreateApplicationCommand command)
    {
        if (command.Cv.IsUploadCvMode)
        {
            return fileService.UploadAsync(command.Cv.CvBytes, command.Cv.FileName);
        }

        return DownloadFromIdentityServerAsync();
    }

    public Task<string> GetCvByCommandNoVacancyAsync(ref CreateApplicationNoVacancyCommand command)
    {
        if (command.Cv.IsUploadCvMode)
        {
            return fileService.UploadAsync(command.Cv.CvBytes, command.Cv.FileName);
        }

        return DownloadFromIdentityServerAsync();
    }

    private async Task<string> DownloadFromIdentityServerAsync()
    {
        using var identityHttpClient = factory.CreateClient("IdentityHttpClient");
        identityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext?.Request.Headers.Authorization[0]!.Split(' ')[1]);
        await using var stream = await identityHttpClient.GetStreamAsync(configuration["IdentityApi:DownloadCvEndPoint"]);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return await fileService.UploadAsync(ms.ToArray(), $"{Guid.NewGuid()}_{httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Username)?.Value}.pdf");
    }
}