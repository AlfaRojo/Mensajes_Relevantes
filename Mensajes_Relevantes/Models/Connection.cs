namespace Mensajes_Relevantes.Models
{
    public class Connection : Message
    {
        public Message DataMessage { get; set; }
        public string nickName { get; set; }

    }
}
