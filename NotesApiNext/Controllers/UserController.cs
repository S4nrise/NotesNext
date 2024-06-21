using Microsoft.AspNetCore.Mvc;
using NotesApiNext.ApiTypes;
using NotesApiNext.Database;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.User;
using NotesApiNext.Services;
using System.Text;

namespace NotesApiNext.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(NotesNextDbContext notesNextDbContext, IDateTimeProvider dateTimeProvider) : Controller
    {
        [HttpPost("~/Registration")]
        public async Task<IActionResult> UserRegistration(UserDto userDto)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = userDto.Email,
                UserName = userDto.UserName,
                Password = Encoding.UTF8.GetBytes(userDto.Password),
                RegistrDateTime = dateTimeProvider.UtcNow,
            };
            notesNextDbContext.Users.Add(user);
            await notesNextDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
