﻿using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _uploadFolderPath;

    public FileService()
    {
        _uploadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MraFiles");

        if (!Directory.Exists(_uploadFolderPath)) Directory.CreateDirectory(_uploadFolderPath);
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        var key = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(_uploadFolderPath, key);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return key;
    }

    public async Task<string> UploadAsync(byte[] fileBytes, string fileName)
    {
        var key = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(_uploadFolderPath, key);

        var ms = new MemoryStream(fileBytes);
        var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await ms.CopyToAsync(fs);
        return key;
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