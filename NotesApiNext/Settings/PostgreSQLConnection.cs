namespace NotesApiNext.Settings
{
    public class PostgreSQLConnection
    {
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
    public string ConnectionString => $"host={Host};port={Port};username={Username};password={Password};database={Database}";
    }
}