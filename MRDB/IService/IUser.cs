using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mensajes_Relevantes.Models;

namespace MRDB.IService
{
    public interface IUser
    {
        public void SetContactCollection();
        public User GetUser();
        public IEnumerable<Contact> GetAllContacts();
    }
}
