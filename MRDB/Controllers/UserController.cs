using System;
using Mensajes_Relevantes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using DiffieHelman;
using SDES;
using System.Threading.Tasks;

namespace MRDB.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Message()
        {
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> MessageAsync(Message message, IFormFile file)
        {
            string id_file = string.Empty;
            string text = string.Empty;
            if (file != null)
            {
                Import import = new Import();
                id_file = await import.Upload_FileAsync(file);
            }
            if (message.Text != null)
            {
                //Hay que obtener el código DH entre usuarios para ser el código que se envie para encriptar
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                text = encryptDecrypt.Encrypt(message.Text, "0110101001");
            }
            if (text == null && id_file == null)
            {
                return View();
            }
            var date = DateTime.Today;
            message.SendDate = date.ToShortDateString();
            message.emisor = HttpContext.Session.GetString("Nick_Name");
            Operation operation = new Operation();
            operation.Insert_Chat(text, message.SendDate, id_file, message.emisor);
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
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
                var Operation = new Operation();
                Operation.CreateUser(collection["name"], collection["Nick_Name"], encryptDecrypt.Encrypt(collection["password"], "0110100101"));
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
                var result = Operation.SearchUser(collection["Nick_Name"], encryptDecrypt.Encrypt(collection["password"], "0110100101"));
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
        public ActionResult Contact(Contact id)
        {
            UserInformation userInformation = new UserInformation();

            string user_name = HttpContext.Session.GetString("Nick_Name");
            ViewBag.sessionv = user_name;
            if ((id.id_Contact != null) && (id.Nick_Name != ViewBag.sessionv))
            {
                userInformation.GetAllUser();
                userInformation.SetContactUser(id, (string)ViewBag.sessionv);
                return RedirectToAction("Menu", "User");
            }
            else
            {
                //userInformation.SetContactCollection();
                //var GetContacts = userInformation.GetAllContacts();
                Operation operation = new Operation();
                return View(operation.Get_Contacts(user_name));
            }
        }

        [HttpPost]
        public ActionResult ContactI(Contact contact)
        {
            return RedirectToAction("Menu", "User");
        }
    }
}
