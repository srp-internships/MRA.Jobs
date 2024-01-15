using Microsoft.AspNetCore.Components.Forms;

namespace MRA.Jobs.SSR.Client.Services.FileService;

public interface IFileService
{
    Task<string> UploadAsync(IBrowserFile file);
}