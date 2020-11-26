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
        public async Task<ActionResult> MessageAsync(Message message)
        {
            if (message.FileName != null)
            {
                Import import = new Import();
                var id_file = await import.Upload_FileAsync(message.FileName);
            }
            var date = DateTime.Today;
            message.SendDate = date.ToShortDateString();
            ViewBag.sessionv = HttpContext.Session.GetString("Nick_Name");
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
                if (result == true)
                {
                    Connection connection = new Connection();
                    connection.nickName = Request.Form["Nick_Name"];
                    HttpContext.Session.SetString("Nick_Name", connection.nickName);
                    return RedirectToAction("Message", "User");
                }
                else { return View(); }
            }
            catch
            {
                return View();
            }

        }
    }
}
