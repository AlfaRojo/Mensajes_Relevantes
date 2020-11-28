using System;
using Mensajes_Relevantes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using SDES;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MRDB.Controllers
{
    public class UserController : Controller
    {
        private readonly IHubContext<ChatHub> chatHub;
        public UserController(IHubContext<ChatHub> hubContext)
        {
            this.chatHub = hubContext;
        }
        public ActionResult Message()
        {
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Message(Message message, IFormFile file)
        {
            byte[] file_Cont = { };
            string text = message.Text;
            if (text.Equals(null) && file_Cont != null)
            {
                ViewBag.sessionv = message.emisor;
                return View();
            }
            Operation operation = new Operation();
            var date = DateTime.Today;
            message.SendDate = date.ToShortDateString();
            message.emisor = HttpContext.Session.GetString("Nick_Name");
            if (file != null)
            {
                Import import = new Import();
                file_Cont = await import.Upload_FileAsync(file);
                operation.Insert_Chat(text, message.SendDate, message.emisor, file_Cont, file.FileName);
            }
            if (message.Text != null)
            {
                var DH_Group = operation.Get_DH_Group(message.emisor, message.receptor);
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                text = encryptDecrypt.Encrypt(message.Text, Convert.ToString(DH_Group, 2));
                operation.Insert_Chat(text, message.SendDate, message.emisor, file_Cont, "");
            }
            ChatHub chatHub = new ChatHub();
            await chatHub.SendMessage(message.emisor, message.Text);

            ViewBag.sessionv = message.emisor;
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                var Operation = new Operation();
                Operation.CreateUser(collection["name"], collection["Nick_Name"], collection["password"]);
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController
        public ActionResult LoginAsync()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAsync(IFormCollection collection)
        {
            try
            {
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                var Operation = new Operation();

                var result = Operation.SearchUser(collection["Nick_Name"], encryptDecrypt.Encrypt(collection["password"], Convert.ToString(Operation.Find_DH(collection["Nick_Name"]), 2)));
                if (result)
                {
                    Connection connection = new Connection();
                    connection.nickName = Request.Form["Nick_Name"];
                    HttpContext.Session.SetString("Nick_Name", connection.nickName);
                    return RedirectToAction("Menu", "User");
                }
                else { return View(); }
            }
            catch
            {
                return View();
            }

        }
        public ActionResult Menu()
        {
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");

            return View();
        }

        [HttpGet]
        public ActionResult Contact(User user)
        {
            UserInformation userInformation = new UserInformation();

            string current_User = HttpContext.Session.GetString("Nick_Name");
            ViewBag.sessionv = current_User;
            if (user.Nick_Name != null && user.Name != null)
            {
                Contact contact = new Contact
                {
                    id_Contact = Guid.NewGuid().ToString(),
                    Nick_Name = user.Nick_Name
                };
                userInformation.Add_Contact(contact, current_User);
                return RedirectToAction("Menu", "User");
            }
            else
            {
                Operation operation = new Operation();
                return View(operation.Get_Contacts(current_User));
            }
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            return RedirectToAction("Menu", "User");
        }
    }
}
