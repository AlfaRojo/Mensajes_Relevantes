using System;
using Mensajes_Relevantes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using SDES;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using static MRDB.ChatHub;

namespace MRDB.Controllers
{
    public class UserController : Controller
    {
        //Completo
        #region Login

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

        #endregion

        //Completo
        #region Create
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

        #endregion

        //Completo
        #region Contact
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

        #endregion

        //Corregir
        #region Hub
        private readonly IHubContext<ChatHub> _chatHubContext;
        public UserController(IHubContext<ChatHub> hubContext)
        {
            this._chatHubContext = hubContext;
        }

        #endregion

        #region Menu
        public ActionResult Menu()
        {
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");

            return View();
        }

        #endregion

        //Corregir
        #region Message

        public ActionResult Message(User receptor)
        {
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
            if (receptor.Nick_Name == null)
            {
                ViewBag.emisor = ViewBag.sessionV;
                return View();
            }
            else
            {
                ViewBag.emisor = ViewBag.sessionV;
                ViewBag.receptor = receptor.Nick_Name;
                var message = new Message();
                message.emisor = (string)ViewBag.emisor;
                message.receptor = (string)(ViewBag).receptor;
                return View(message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Message(Message message, IFormFile file)
        {
            ViewBag.sessionv = message.emisor = HttpContext.Session.GetString("Nick_Name");
            var information = new UserInformation();
            var operation = new Operation();
            var user_Info = operation.Get_User_Info(message.emisor);
            if (user_Info.Friends.Count().Equals(0) || user_Info == null)
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
                operation.Insert_Chat(text, message.SendDate, message.emisor, message.receptor , file_Cont, file.FileName);
                information.SetHistoryCollection(message.emisor, message.receptor, message, text);
            }
            if (message.Text != null)
            {
                var DH_Group = operation.Get_DH_Group(message.emisor, message.receptor);
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                text = encryptDecrypt.Encrypt(message.Text, Convert.ToString(DH_Group, 2));
                operation.Insert_Chat(text, message.SendDate, message.emisor, message.receptor, file_Cont, "");
                information.SetHistoryCollection(message.emisor, message.receptor, message, text);

            }
            await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", message.emisor, message.Text);

            return RedirectToAction("Get_History", "User", message);
        }

        public ActionResult Get_Msg()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Get_Msg(string message)
        {
            Operation operation = new Operation();
            string current_user = ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
            var all_msg = operation.Get_Messages(message, current_user);
            return View(all_msg);
        }

        [HttpGet]
        public ActionResult ToSend(User emisor)
        {
             ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
            if (emisor.Nick_Name != null)
            {
                return RedirectToAction("Message", "User", emisor);
            }
            if (ViewBag.sessionv != null)
            {
                Operation operation = new Operation();
                var friends = operation.Get_Friends(ViewBag.sessionv);
                return View(friends);
            }
            return RedirectToAction("LoginAsync", "User");
        }

        [HttpPost]
        public ActionResult ToSend()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Get_History(User receptor, Message other)
        {
            var Information = new UserInformation();
            var emisor = ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
            if (other != null)
            {
                var history = Information.GetHistoryCollection(emisor, other.receptor);
                return View(history);
            }
            var List = Information.GetHistoryCollection(emisor, receptor.Nick_Name);
            return View(List);
        }

        #endregion
    }
}
