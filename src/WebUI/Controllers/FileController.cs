using Microsoft.AspNetCore.Mvc;

namespace MRA.Jobs.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public FileController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet]
    public ActionResult GetFile([FromQuery] string fileName)
    {
        string path = Path.Combine(_environment.WebRootPath, "Images", fileName ?? "");
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult AddFile(IFormFile file)
    {
        if (file == null)
            return BadRequest();

        string uploadsFolder = Path.Combine(_environment.WebRootPath, "Images");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "." + file.FileName.Split(".").Last();
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
            file.CopyTo(fileStream);

        return Ok(uniqueFileName);
    }
}
