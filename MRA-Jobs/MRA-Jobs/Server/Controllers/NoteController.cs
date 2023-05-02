using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MRA_Jobs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notes = await _noteService.GetAll<GetNoteDto>();
            return Ok(notes);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _noteService.GetById<GetNoteDto>(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(AddNoteDto addNoteDto)
        {
            var result = await _noteService.Add<AddNoteDto, GetNoteDto>(addNoteDto);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNote(UpdateNoteDto updateNoteDto)
        {
            var result = await _noteService.Update<UpdateNoteDto, GetNoteDto>(updateNoteDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _noteService.Delete(id);
            return Ok(result);
        }
    }
}
