using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface IFileService
{
    Task<string> UploadAsync(IFormFile file);
    Task<byte[]> Download(string key);
    bool FileExists(string key);
}
