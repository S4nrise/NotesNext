using Microsoft.EntityFrameworkCore;
using NotesApiNext.Database.Configurations;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.Note;
using NotesApiNext.Models.User;

namespace NotesApiNext.Database
{
    public class NotesNextDbContext : DbContext, INotesNextDbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public NotesNextDbContext(DbContextOptions<NotesNextDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NoteConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
