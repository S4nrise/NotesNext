using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApiNext.ApiTypes;
using NotesApiNext.Extensions;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.User;
using System.Security.Claims;

namespace NotesApiNext.Controllers;

public class UserController(
    IJwtTokenGenerator jwtTokenGenerator,
    IJwtTokensRepository jwtTokensRepository,
    IUserRepository userRepository,
    IPasswordHashProvider passwordHashProvider) : BaseController
{
    [AllowAnonymous]
    [HttpPost("Registration")]
    public async Task<string> Registration(UserDto userDto)
    {
        var user = await userRepository.AddUserAsync(userDto);
        var token = GenerateAndStoreToken(user);

        return token;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<string> Login(UserDto loginDto)
    {
        var user = await userRepository.GetByUserEmailAsync(loginDto.Email);
        var userPass = passwordHashProvider.Verify(loginDto.Password, user.Password);
        if (!userPass)
        {
            throw new ArgumentException(nameof(loginDto.Password));
        }
        var token = GenerateAndStoreToken(user);

        return token;
    }

    [HttpDelete("logout")]
    public IActionResult Logout(Guid userId)
    {
        jwtTokensRepository.Remove(userId);

        return Ok();
    }

    [HttpGet]
    public async Task<string> RefreshToken()
    {
        var userId = HttpContext.ExtractUserIdFromClaims();
        if (userId is null)
        {
            throw new InvalidOperationException();
        }

        var user = await userRepository.GetByUserIdAsync(userId.Value);
        var newToken = GenerateAndStoreToken(user);

        return newToken;
    }

    [HttpGet("allusers")]
    public async Task<List<string>> GetAllUsers()
    {
        return await userRepository.GetAllUsers();
    }

    private string GenerateAndStoreToken(User user)
    {
        var token = jwtTokenGenerator.GenerateToken(user);
        jwtTokensRepository.Update(user.UserId, token);
        return token;
    }
}
