using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotesApiNext.ApiTypes;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.Note;


namespace NotesApiNext.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController(
        IMapper mapper,
        INoteRepository noteRepository,
        IUserRepository userRepository) : Controller
    {
        [HttpPost("~/CreateNote")]
        public async Task<IActionResult> CreateNote(CreateNoteDto createNoteDto, CancellationToken cancellationToken)
        {
            var note = mapper.Map<Note>(createNoteDto);
            await noteRepository.AddNoteAsync(note, cancellationToken);
            return Created();
        }

        [HttpGet("~/GetAllNote")]
        public async Task<IActionResult> GetAllNote(Guid userId)
        {
            var notesReturn = await noteRepository.GetAllForUser(userId);
            return Ok(notesReturn);
        }

        [HttpDelete("~/DeleteNote")]
        public async Task<IActionResult> DeleteNote(Guid userId, Guid noteId)
        {
            await noteRepository.DeleteNoteAsync(userId, noteId);
            return Ok("Deleted");
        }

        [HttpPut("~/EditNote")]
        public async Task<IActionResult> EditNote(Guid noteId, Guid userId, EditNoteDto newNote)
        {
            await noteRepository.EditNoteAsync(noteId, userId, newNote);
            return Ok("Updated");
        }

        [HttpGet("~/{userId}/{noteId}")]
        public async Task<IActionResult> GetNote(Guid userId, Guid noteId)
        {
            var noteReturn = await noteRepository.GetAsync(userId, noteId);
            return Ok(noteReturn);
        }
    }
}
