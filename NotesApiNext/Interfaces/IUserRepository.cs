using NotesApiNext.ApiTypes;
using NotesApiNext.Models.User;

namespace NotesApiNext.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUserExistsAsync(string userName);
        Task<User> AddUserAsync(UserDto userDto);
        Task<User> GetByUserIdAsync(Guid id);
        Task<User> GetByUserEmailAsync(string email);

        Task<List<string>> GetAllUsers();
    }
}
