using System;
using Mensajes_Relevantes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using SDES;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace MRDB.Controllers
{
    public class UserController : Controller
    {
        private readonly IHubContext<ChatHub> chatHub;
        public UserController(IHubContext<ChatHub> hubContext)
        {
            this.chatHub = hubContext;
        }

        public ActionResult Message(User receptor)
        {
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
            ViewBag.receptor = receptor.Nick_Name;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Message(Message message, IFormFile file)
        {
            ViewBag.sessionv = message.emisor = HttpContext.Session.GetString("Nick_Name");
            Operation operation = new Operation();
            var user_Info = operation.Get_User_Info(message.emisor);
            if (user_Info.Friends.Count().Equals(0))
            {
                return View();
            }
            byte[] file_Cont = { };
            string text = message.Text;
            if (text == null && file_Cont != null)
            {
                ViewBag.sessionv = message.emisor;
                return View();
            }
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
            await chatHub.Clients.All.SendAsync(message.emisor, message.Text);

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
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
                var user = Operation.Get_User_Info(collection["Nick_Name"]);
                if (user == null)
                {
                    return View();
                }
                var pass = Convert.ToString(user.DH, 2);
                var result = Operation.SearchUser(collection["Nick_Name"], encryptDecrypt.Encrypt(collection["password"], pass));
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
        public ActionResult Contact()
        {
            return RedirectToAction("Menu", "User");
        }

        public ActionResult Get_Msg()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Get_Msg(string message)
        {
            Operation operation = new Operation();
            string current_User = HttpContext.Session.GetString("Nick_Name");
            var all_msg = operation.Get_Messages(message, current_User);
            return View(all_msg);
        }

        [HttpGet]
        public ActionResult ToSend(User emisor)
        {
            string current_user = HttpContext.Session.GetString("Nick_Name");
            if (emisor.Nick_Name != null)
            {
                return RedirectToAction("Message", "User", emisor);
            }
            if (current_user != null)
            {
                Operation operation = new Operation();
                var friends = operation.Get_Friends(current_user);
                return View(friends);
            }
            return RedirectToAction("LoginAsync", "User");
        }

        [HttpPost]
        public ActionResult ToSend()
        {
            return View();
        }

        #region Without_Post
        public ActionResult Menu()
        {
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");

            return View();
        }
        #endregion
    }
}
