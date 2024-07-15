using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApiNext.ApiTypes;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.Note;

namespace NotesApiNext.Services
{
    public class NoteRepository(
        INotesNextDbContext notesNextDbContext,
        IPasswordHashProvider passwordHashProvider,
        IMapper mapper,
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository) : INoteRepository
    {
        public async Task AddNoteAsync(Note note, CancellationToken cancellationToken)
        {
            await notesNextDbContext.Notes.AddAsync(note);
            await notesNextDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteNoteAsync(Guid userId, Guid noteId)
        {
            if (noteId == default) return;
            var note = notesNextDbContext.Notes.FirstOrDefault(note => note.Id == noteId && note.UserId == userId);
            if (note is not null)
            {
                notesNextDbContext.Notes.Remove(note);
            }
            await notesNextDbContext.SaveChangesAsync();
        }

        public async Task EditNoteAsync(Guid userId, Guid noteId, EditNoteDto editNoteDto)
        {
            var note = await notesNextDbContext.Notes.FirstOrDefaultAsync(note => note.Id == noteId && note.UserId == userId);
            if (note == null)
            {
                return;
            }
            var newNote = mapper.Map<Note>(editNoteDto);
            note.UpdatedDateTime = dateTimeProvider.UtcNow;
            note.Title = newNote.Title;
            note.Description = newNote.Description;
            note.Priority = newNote.Priority;

            notesNextDbContext.Notes.Update(note);
            await notesNextDbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ListNoteVm>> GetAllForUser(Guid userId)
        {
            //var user = userRepository.GetByUserIdAsync(userId);
            var userNotes = notesNextDbContext.Notes.AsNoTracking().Where(note => note.UserId == userId);

            await userNotes.ToListAsync();
            var listNoteVm = mapper.Map<List<ListNoteVm>>(userNotes);
            return listNoteVm.AsReadOnly();
        }

        public async Task<DetailedNoteVm> GetAsync(Guid userId, Guid noteId)
        {
            var note = await notesNextDbContext.Notes.FirstOrDefaultAsync(note => note.Id == noteId && note.UserId == userId);
            if (note is null)
            {
                throw new NotImplementedException();
            }
            var noteReturn = mapper.Map<DetailedNoteVm>(note);
            return noteReturn;
        }
    }
}
