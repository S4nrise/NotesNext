using NotesApiNext.Interfaces;

namespace NotesApiNext.Services
{
    public class PasswordHashProvider : IPasswordHashProvider
    {
        public byte[] GetHash(string password)
        {
            throw new NotImplementedException();
        }

        public bool Verify(string password, byte[] expected)
        {
            throw new NotImplementedException();
        }
    }
}
