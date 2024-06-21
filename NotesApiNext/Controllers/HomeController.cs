using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApiNext.ApiTypes;
using NotesApiNext.Database;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.Note;
using NotesApiNext.Models.User;
using System.Text;


namespace NotesApiNext.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController(NotesNextDbContext notesNextDbContext, IDateTimeProvider dateTimeProvider, IMapper mapper) : Controller
    {
        [HttpPost("~/CreateNote")]
        public async Task<IActionResult> CreateNote(CreateNoteDto createNoteDto)
        {
            var note = mapper.Map<Note>(createNoteDto);
            notesNextDbContext.Notes.Add(note);
            await notesNextDbContext.SaveChangesAsync();
            return Created();
        }

        [HttpGet("~/GetAllNote")]
        public async Task<IActionResult> GetAllNote()
        {
            var notes = await notesNextDbContext.Notes.ToListAsync();
            return Ok(notes);
        }

        [HttpDelete("~/DeleteNote")]
        public async Task<IActionResult> DeleteNote(Guid noteId)
        {
            var note = await notesNextDbContext.Notes.FirstOrDefaultAsync(note => note.Id == noteId);

            if (note == null) 
            {
                return BadRequest();
            }
            notesNextDbContext.Notes.Remove(note);
            await notesNextDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("~/EditNote")]
        public async Task<IActionResult> EditNote(Note newNote)
        {
            var note = await notesNextDbContext.Notes.FirstOrDefaultAsync(note =>note.Id == newNote.Id);
            if (note == null)
            {
                return BadRequest();
            }
            note.UpdatedDateTime = dateTimeProvider.UtcNow;
            note.Title = newNote.Title;
            note.Description = newNote.Description;
            note.Priority = newNote.Priority;

            notesNextDbContext.Notes.Update(note);
            await notesNextDbContext.SaveChangesAsync();
            return Ok("Updated");
        }
    }
}
