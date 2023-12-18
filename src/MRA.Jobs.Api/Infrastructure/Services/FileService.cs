namespace MRA.Jobs.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _uploadFolderPath;
    private readonly ICurrentUserService _currentUserService;


    public FileService(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
        _uploadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MraFiles");

        if (!Directory.Exists(_uploadFolderPath)) Directory.CreateDirectory(_uploadFolderPath);
    }

    public async Task<string> UploadAsync(byte[] fileBytes, string fileName)
    {
        var fileId = $"{DateTime.Now:yyyyMMddhhmmss}_{_currentUserService.GetUserName()}_{fileName.Split('.').Last()}";
        var filePath = Path.Combine(_uploadFolderPath, fileId);

        using var ms = new MemoryStream(fileBytes);
        await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await ms.CopyToAsync(fs);
        return fileId;
    }

    public async Task<byte[]> Download(string key)
    {
        var filePath = Path.Combine(_uploadFolderPath, key);
        return File.Exists(filePath) ? await File.ReadAllBytesAsync(filePath) : null;
    }

    public Task<bool> FileExistsAsync(string key)
    {
        return Task.FromResult(File.Exists(Path.Combine(_uploadFolderPath, key)));
    }
}