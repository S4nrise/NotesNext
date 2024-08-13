namespace NotesApiNext.Interfaces
{
    public interface IJwtTokensRepository
    {
        void Update(Guid userId , string jwtToken);
        bool Verify(Guid userId , string jwtToken);
        void Remove(Guid userId);
    }
}
