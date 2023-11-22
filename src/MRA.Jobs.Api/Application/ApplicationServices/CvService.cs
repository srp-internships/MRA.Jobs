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
        return _fileService.UploadAsync(command.Cv.CvBytes, command.Cv.FileName);
    }
}