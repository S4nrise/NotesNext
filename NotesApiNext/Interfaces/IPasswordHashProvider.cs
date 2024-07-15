namespace NotesApiNext.Interfaces
{
    public interface IPasswordHashProvider
    {
        byte[] GetHash(string password);
        bool Verify(string password, byte[] expected);
    }
}
