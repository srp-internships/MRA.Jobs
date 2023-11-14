using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MRA.Jobs.Infrastructure.Services;

public class AzureFileService : IFileService
{
    private readonly IConfiguration _configuration;
    public AzureFileService(IConfiguration configurations)
    {
        _configuration = configurations;
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        string fileId = $"{Guid.NewGuid()}_{file.FileName}";
        var myBlob = file.OpenReadStream();
        var blobClient = new BlobContainerClient(_configuration["AzureBlob:AzureWebJobsStorage"], _configuration["AzureBlob:ContainerName"]);
        var blob = blobClient.GetBlobClient(fileId);
        await blob.UploadAsync(myBlob);//TODO check the status and handle exceptions 
        return fileId;
    }

    public async Task<byte[]> Download(string key)
    {
        var blobServiceClient = new BlobServiceClient(_configuration["AzureBlob:AzureWebJobsStorage"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(_configuration["AzureBlob:ContainerName"]);

        var blobClient = containerClient.GetBlobClient(key);//key is filename

        BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

        await using var ms = new MemoryStream();
        await blobDownloadInfo.Content.CopyToAsync(ms);
        return ms.ToArray();
    }

    public bool FileExists(string key)
    {
        throw new NotImplementedException();
    }
}