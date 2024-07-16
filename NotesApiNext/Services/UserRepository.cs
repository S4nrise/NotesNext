using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApiNext.ApiTypes;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.User;

namespace NotesApiNext.Services
{
    public class UserRepository(
        INotesNextDbContext notesNextDbContext,
        IPasswordHashProvider passwordHashProvider,
        IMapper mapper) : IUserRepository
    {
        public async Task AddUserAsync(UserDto userDto)
        {
            if (await IsUserExistsAsync(userDto.UserName))
            {
                return;
            }

            var user = mapper.Map<User>(userDto);
            user.Password = passwordHashProvider.GetHash(userDto.Password);

            await notesNextDbContext.Users.AddAsync(user);
            await notesNextDbContext.SaveChangesAsync();
        }

        public async Task<User> GetByUserIdAsync(Guid id)
        {
            var user = await notesNextDbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.UserId == id);
            if (user == null)
            {
                throw new ArgumentException(nameof(id));
            }
            return user;
        }

        public Task<bool> IsUserExistsAsync(string userName)
        {
            return notesNextDbContext.Users.AsNoTracking().AnyAsync(user => user.UserName == userName);
        }
    }
}
