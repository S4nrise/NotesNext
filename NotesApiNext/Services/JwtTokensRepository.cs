using NotesApiNext.Interfaces;
using NuGet.Common;
using System.Collections.Concurrent;

namespace NotesApiNext.Services
{
    public class JwtTokensRepository : IJwtTokensRepository
    {
        private readonly ConcurrentDictionary<Guid, string> _tokens = new();
        public void Update(Guid userId, string jwtToken) => _tokens[userId] = jwtToken;
        public bool Verify(Guid userId, string jwtToken) => 
            _tokens.ContainsKey(userId) && _tokens[userId] == jwtToken;
        public void Remove(Guid userId) => _tokens.Remove(userId, out _);
    }
}
