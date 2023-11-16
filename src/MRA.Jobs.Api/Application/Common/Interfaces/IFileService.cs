
namespace MRA.Jobs.Application.Common.Interfaces;
public interface IFileService
{
    /// <summary>
    /// Method for upload file
    /// </summary>
    /// <param name="fileBytes">file bytes</param>
    /// <param name="fileName">name of file(not unique)</param>
    /// <returns>unique name of uploaded file</returns>
    Task<string> UploadAsync(byte[] fileBytes, string fileName);
    Task<byte[]> Download(string key);
    Task<bool> FileExistsAsync(string key);
}
