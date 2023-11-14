using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MRA.Jobs.Infrastructure.Services;

public class AzureFileService : IFileService
{
    private readonly string _connection;
    private readonly string _containerName;

    public AzureFileService(IConfiguration configurations)
    {
        _connection = configurations["AzureWebJobsStorage"];
        _containerName = configurations["ContainerName"];
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        var myBlob = file.OpenReadStream();
        var blobClient = new BlobContainerClient(_connection, _containerName);
        var blob = blobClient.GetBlobClient(file.FileName);
        await blob.UploadAsync(myBlob);
        return ";";
    }

    public async Task<byte[]> Download(string key)
    {
        var blobServiceClient = new BlobServiceClient(_connection);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

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