using NotesApiNext.Models.User;

namespace NotesApiNext.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
