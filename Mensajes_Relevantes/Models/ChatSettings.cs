namespace Mensajes_Relevantes.Models
{
    public class ChatSettings : IChatDatabaseSettings
    {
        public string MessagesCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IChatDatabaseSettings
    {
        string MessagesCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
