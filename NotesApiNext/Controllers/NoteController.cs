using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApiNext.ApiTypes;
using NotesApiNext.Extensions;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.Note;


namespace NotesApiNext.Controllers;

public class NoteController(
    IMapper mapper,
    INoteRepository noteRepository) : BaseController
{
    [HttpPost("CreateNote")]
    public async Task<IActionResult> CreateNote(CreateNoteDto createNoteDto, CancellationToken cancellationToken)
    {
        var note = mapper.Map<Note>(createNoteDto);
        var userId = HttpContext.ExtractUserIdFromClaims()!.Value;
        note.UserId = userId;
        await noteRepository.AddNoteAsync(note, cancellationToken);
        return Created();
    }

    [HttpGet("GetAllNote")]
    public async Task<IActionResult> GetAllNote()
    {
        var userId = HttpContext.ExtractUserIdFromClaims()!.Value;
        var notesReturn = await noteRepository.GetAllForUser(userId);
        return Ok(notesReturn);
    }

    [HttpDelete("DeleteNote")]
    [Authorize(Policy = "NotesOwner")]
    public async Task<IActionResult> DeleteNote(Guid noteId)
    {
        var userId = HttpContext.ExtractUserIdFromClaims()!.Value;
        await noteRepository.DeleteNoteAsync(userId, noteId);
        return Ok("Deleted");
    }

    [HttpPut("EditNote")]
    public async Task<IActionResult> EditNote(Guid noteId, EditNoteDto newNote)
    {
        var userId = HttpContext.ExtractUserIdFromClaims()!.Value;
        await noteRepository.EditNoteAsync(noteId, userId, newNote);
        return Ok("Updated");
    }

    [HttpGet("{userId}/{noteId}")]
    public async Task<IActionResult> GetNote(Guid noteId)
    {
        var userId = HttpContext.ExtractUserIdFromClaims()!.Value;
        var noteReturn = await noteRepository.GetAsync(userId, noteId);
        return Ok(noteReturn);
    }
}
