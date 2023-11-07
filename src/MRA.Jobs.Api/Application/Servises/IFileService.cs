using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Application.Servises;
public interface IFileService
{
    Task<string> UploadAsync(IFormFile file);
    byte[] Download(string key);
    bool FileExists(string key);
}
