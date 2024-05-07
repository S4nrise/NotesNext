using Microsoft.EntityFrameworkCore;
using NotesApiNext.Models.Note;
using NotesApiNext.Models.User;

namespace NotesApiNext.Interfaces
{
    public interface INotesNextDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Note> Notes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
