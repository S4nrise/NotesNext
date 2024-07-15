using NotesApiNext.ApiTypes;
using NotesApiNext.Models.Note;

namespace NotesApiNext.Interfaces
{
    public interface INoteRepository
    {
        Task<DetailedNoteVm> GetAsync(Guid userId, Guid id);
        Task<IReadOnlyList<ListNoteVm>> GetAllForUser(Guid userId);
        Task AddNoteAsync(Note note, CancellationToken cancellationToken);
        Task EditNoteAsync(Guid userId, Guid noteId, EditNoteDto editNoteDto);
        Task DeleteNoteAsync(Guid userId, Guid noteId);
    }
}
