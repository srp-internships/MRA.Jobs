using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace MRA.Jobs.Infrastructure.Services;

public class AzureFileService : IFileService
{
    private readonly IConfiguration _configuration;

    public AzureFileService(IConfiguration configurations)
    {
        _configuration = configurations;
    }

    public async Task<string> UploadAsync(byte[] fileBytes, string fileName)
    {
        string fileId = $"{Guid.NewGuid()}_{fileName}";
        var blobClient = new BlobContainerClient(_configuration["AzureBlob:AzureWebJobsStorage"],
            _configuration["AzureBlob:ContainerName"]);
        var blob = blobClient.GetBlobClient(fileId);
        var ms = new MemoryStream(fileBytes);
        await blob.UploadAsync(ms); //TODO check the status and handle exceptions 
        return fileId;
    }

    public async Task<byte[]> Download(string key)
    {
        var blobServiceClient = new BlobServiceClient(_configuration["AzureBlob:AzureWebJobsStorage"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(_configuration["AzureBlob:ContainerName"]);

        var blobClient = containerClient.GetBlobClient(key); //key is filename

        BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

        await using var ms = new MemoryStream();
        await blobDownloadInfo.Content.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<bool> FileExistsAsync(string key)
    {
        var blobServiceClient = new BlobServiceClient(_configuration["AzureBlob:AzureWebJobsStorage"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(_configuration["AzureBlob:ContainerName"]);
        return await containerClient.GetBlobClient(key).ExistsAsync();
    }
}