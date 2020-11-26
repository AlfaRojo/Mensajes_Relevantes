using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajes_Relevantes.Models
{
    public class Connection : Message
    {
        public Message DataMessage { get; set; }
        public Chat DataChat { get; set; }
        public string nickName { get; set; }

        public string Path { get; set; }

        public Connection()
        {
            Path = "~/Images/coete.png";
        }
    }
}
