using NotesApiNext.Models.Note;

namespace NotesApiNext.Interfaces
{
    public interface INoteRepository
    {
        Note Get(Guid userId, Guid id);
        IReadOnlyList<Note> GetAllForUser(Guid userId);
        void Add(Note note);
        bool Edit(Note note);
        bool Delete(Note note);
    }
}
