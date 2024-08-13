using NotesApiNext.ApiTypes;

namespace NotesApiNext.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(string email, string password);
        
    }
}
