using MRA.Jobs.Application.Common.Interfaces;

namespace MRA.Jobs.Web.Services;
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public bool FileExists(string key)
    {
        string path = Path.Combine(_environment.WebRootPath, "Images", key ?? "");
        return File.Exists(path);
    }
}
