using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Servises;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not selected.");

        var key = await _fileService.UploadAsync(file);

        return Ok(new { key });
    }

    [HttpGet("download/{key}")]
    public IActionResult Download(string key)
    {
        if (!_fileService.FileExists(key))
            return NotFound("File not found.");

        var fileBytes = _fileService.Download(key);
        return File(fileBytes, "application/octet-stream", key);
    }
}
