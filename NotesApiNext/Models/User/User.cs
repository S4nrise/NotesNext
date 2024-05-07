using System.ComponentModel.DataAnnotations;

namespace NotesApiNext.Models.User
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte Password { get; set; }
        public DateTime RegistrDateTime { get; set; } = DateTime.UtcNow;
    }
}
