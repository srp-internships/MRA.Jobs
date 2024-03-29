using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace MRA.Jobs.Infrastructure.Services;

public class AzureFileService(IConfiguration configurations,ICurrentUserService currentUserService) : IFileService
{
    public async Task<string> UploadAsync(byte[] fileBytes, string fileName)
    {
        var fileId = $"{DateTime.Now:yyyyMMddhhmmss}_{currentUserService.GetUserName()}.{fileName.Split('.').Last()}";
        var blobClient = new BlobContainerClient(configurations["AzureBlob:AzureWebJobsStorage"],
            configurations["AzureBlob:ContainerName"]);
        var blob = blobClient.GetBlobClient(fileId);
        var ms = new MemoryStream(fileBytes);
        await blob.UploadAsync(ms); //TODO check the status and handle exceptions 
        return fileId;
    }

    public async Task<byte[]> Download(string key)
    {
        var blobServiceClient = new BlobServiceClient(configurations["AzureBlob:AzureWebJobsStorage"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(configurations["AzureBlob:ContainerName"]);

        var blobClient = containerClient.GetBlobClient(key); //key is filename

        BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

        await using var ms = new MemoryStream();
        await blobDownloadInfo.Content.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<bool> FileExistsAsync(string key)
    {
        var blobServiceClient = new BlobServiceClient(configurations["AzureBlob:AzureWebJobsStorage"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(configurations["AzureBlob:ContainerName"]);
        return await containerClient.GetBlobClient(key).ExistsAsync();
    }
}