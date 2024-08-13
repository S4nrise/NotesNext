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
        public async Task<User> AddUserAsync(UserDto userDto)
        {
            if (await IsUserExistsAsync(userDto.Email))
            {
                throw new Exception("User already exists");
            }

            var user = mapper.Map<User>(userDto);
            user.Password = passwordHashProvider.GetHash(userDto.Password);

            await notesNextDbContext.Users.AddAsync(user);
            await notesNextDbContext.SaveChangesAsync();

            return user;
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

        public async Task<User> GetByUserEmailAsync(string email)
        {
            var user = await notesNextDbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
            if (user == null)
            {
                throw new ArgumentException(nameof(email));
            }
            return user;
        }

        public Task<bool> IsUserExistsAsync(string email)
        {
            return notesNextDbContext.Users.AsNoTracking().AnyAsync(user => user.Email == email);
        }

        public Task<List<string>> GetAllUsers()
        {
            return notesNextDbContext.Users.Select(user=> user.UserName).ToListAsync();
        }
    }
}
