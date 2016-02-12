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
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login","User");
            }
            else if(Session["Role"].ToString() == "user")
            {
                return RedirectToAction("Index","Home");
            }
            string userSettings="";
            if (Request.Cookies["userInfo"] != null)
            {
                if (Request.Cookies["UserInfo"]["userName"] != null)
                {
                    userSettings = Request.Cookies["UserInfo"]["userName"];
                }
                
            }
            var v = db.Users.Where(model => model.Email.Equals(userSettings)).FirstOrDefault();
            var jo = db.JobOwners.Where(model => model.UserId.Equals(v.Id)).FirstOrDefault();
            var query =
            from Services in db.Services
            where Services.JobOwnerId == jo.Id

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
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login","User");
            }
            else if (Session["Role"].ToString() == "user")
            {
                return RedirectToAction("Index","Home");
            }
            return View();  
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Services Service)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login","User");
            }
            else if (Session["Role"].ToString() == "user")
            {
                return RedirectToAction("Index","Home");
            }
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
            Service.JobOwners = db.JobOwners.Where(model => model.UserId.Equals(v.Id)).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Services.Add(Service);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
