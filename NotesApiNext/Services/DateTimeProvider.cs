using NotesApiNext.Interfaces;

namespace NotesApiNext.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
