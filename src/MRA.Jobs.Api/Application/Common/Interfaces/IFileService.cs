using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface IFileService
{
    Task<string> UploadAsync(IFormFile file);
    Task<byte[]> Download(string key);
    Task<bool> FileExistsAsync(string key);
}
