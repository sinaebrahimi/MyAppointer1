using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MyAppointer.Models;
using System.Web.Services.Description;

namespace MyAppointer.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Services/
        private MyAppointerEntities db = new MyAppointerEntities();


        public ActionResult Index()
        {
            string userSettings="";
            if (Request.Cookies["userInfo"] != null)
            {

                if (Request.Cookies["UserInfo"]["userName"] != null)
                {
                    userSettings = Request.Cookies["UserInfo"]["userName"];
                }
                
            }
            var v = db.Users.Where(model => model.Email.Equals(userSettings)).FirstOrDefault();
            var query =
            from Services in db.Services
            where Services.JobOwnerId == v.Id

            select Services;

            ViewBag.cook = userSettings;

            return View(query);
        }

        //
        // GET: /Services/Details/5

        public ActionResult Details(int id)
        {

            Services Service = db.Services.Find(id);
            if (Service == null)
            {
                return HttpNotFound();
            }
            return View(Service);
        }

        //
        // GET: /Services/Create

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Services Service)
        {
            string userSettings = "";
            if (Request.Cookies["userInfo"] != null)
            {

                if (Request.Cookies["UserInfo"]["userName"] != null)
                {
                    userSettings = Request.Cookies["UserInfo"]["userName"];
                }

            }
            var v = db.Users.Where(model => model.Email.Equals(userSettings)).FirstOrDefault();

            Service.JobOwnerId = v.Id;
            if (ModelState.IsValid)
            {
                db.Services.Add(Service);
                db.SaveChanges();

            }
            return View(Service);
            }

        //
        // POST: /Services/Create

      

        //
        // GET: /Services/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Services/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Services/Delete/5
         [HttpPost]
        public ActionResult Delete(int[] ID)
        {
            foreach (int item in ID)
            {
                Services Service = db.Services.Where(model => model.Id.Equals(item)).FirstOrDefault();
                db.Services.Remove(Service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Content("OK");
        }

        //
        // POST: /Services/Delete/5

        
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
