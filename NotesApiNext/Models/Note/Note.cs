using System.ComponentModel.DataAnnotations;

namespace NotesApiNext.Models.Note
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreationDateTime { get; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; }
        public bool IsCompleted { get; set; }
        public Guid UserId{ get; set; }
        public Priority Priority { get; set; }
    }
}
