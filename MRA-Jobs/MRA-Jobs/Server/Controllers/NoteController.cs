using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA_Jobs.Application.Common.Models.Dtos.NoteDtos;

namespace MRA_Jobs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        [HttpGet] public IActionResult Get(GetNoteDto getNoteDto)
        {

            return Ok();
        }
    }
}
