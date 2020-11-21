using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mensajes_Relevantes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;

namespace MRDB.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Details(Message message)
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
                MongoHelper.ConnectToMongoService();
                MongoHelper.User_Collection = MongoHelper.Database.GetCollection<UserProperty>("User");

                Operation operation = new Operation();
                var id = operation.GenerateRandomId(24);
                MongoHelper.User_Collection.InsertOneAsync(new UserProperty
                {
                    Id = id,
                    Name = collection["Name"],
                    User = collection["User"],
                    Password = collection["Password"]
                });
                return View();
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
