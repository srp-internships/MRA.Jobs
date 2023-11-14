using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _uploadFolderPath;
    private readonly ConcurrentDictionary<string, string> _fileMap;


    public FileService()
    {
        _uploadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MraFiles");
        
        if (!Directory.Exists(_uploadFolderPath)) Directory.CreateDirectory(_uploadFolderPath);
        
        _fileMap = new ConcurrentDictionary<string, string>();
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        var key = Guid.NewGuid().ToString();
        var filePath = Path.Combine(_uploadFolderPath, file.FileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _fileMap[key] = file.FileName;

        return key;
    }

    public async Task<byte[]> Download(string key)
    {
        if (_fileMap.TryGetValue(key, out string fileName))
        {
            var filePath = Path.Combine(_uploadFolderPath, fileName);
            return File.Exists(filePath) ? await File.ReadAllBytesAsync(filePath) : null;
        }

        return null;
    }

    public bool FileExists(string key)
    {
        return _fileMap.ContainsKey(key);
    }
}