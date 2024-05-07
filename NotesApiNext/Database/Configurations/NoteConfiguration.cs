using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesApiNext.Models.Note;

namespace NotesApiNext.Database.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(note => note.Id);
            builder.Property(note => note.Title).HasMaxLength(100);
            builder.Property(note => note.Description).HasMaxLength(500);
        }
    }
}
