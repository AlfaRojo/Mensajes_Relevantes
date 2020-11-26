using System;
using Mensajes_Relevantes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using DiffieHelman;
using SDES;

namespace MRDB.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Message()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Message(Message message)
        {
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
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController
        public ActionResult Login()
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
                if (result) {
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

    }
}
