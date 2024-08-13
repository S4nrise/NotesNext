using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.User;
using NotesApiNext.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotesApiNext.Services
{
    public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, IDateTimeProvider dateTimeProvider) : IJwtTokenGenerator
    {
        public string GenerateToken(User user)
        {
            var options = jwtSettings.Value;

            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var expiration = dateTimeProvider.UtcNow.AddMinutes(5);

            JwtSecurityToken securityToken = new(
                issuer: options.Issuer,
                audience: options.Audience,
                expires: expiration,
                claims:  claims,
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
