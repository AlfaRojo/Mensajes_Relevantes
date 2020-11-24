using System;
using Mensajes_Relevantes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using DiffieHelman;

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

        public ActionResult Menu()
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
                var Operation = new Operation();
                Operation.CreateUser(collection["name"], collection["Nick_Name"],collection["password"]);
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
        public ActionResult Login(IFormCollection collection)
        {
            try
            {
                var Operation = new Operation();
                var result = Operation.SearchUser(collection["Nick_Name"], collection["password"]);
                if (result == true) { return RedirectToAction("Message", "User"); }
                else { return View(); }
            }
            catch
            {
                return View();
            }

        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
