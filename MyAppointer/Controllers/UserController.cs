﻿using System;
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
using MyAppointer.Filters;
using MyAppointer.Models;
namespace MyAppointer.Controllers
{
    public class UserController : Controller
    {
        private MyAppointerEntities db = new MyAppointerEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.Users.ToList());
            //var users = db.Users.ToList();
            //return View(users);
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Users.Add(users);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(users);
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();

                if (users.Role == "jobowner")
                {
                    //@Url.Action("ViewStockNext", "Inventory", new { firstItem = 11 });

                    return RedirectToAction("Create", "Job", new { FirstJobOwner = users.Id });

                    //return RedirectToAction("Job/Index");
                }
                else
                {

                    return RedirectToAction("Index", "Home");

                    //return RedirectToAction("Home/Index");

                    //return View(users);
                }
            }

            //ViewBag.JobId = new SelectList(db.Jobs, "Id", "Title", users.JobId);


            return View(users);//jobs
        }

        //login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users u)
        {
        // this action is for handle post (login)
        //if (ModelState.IsValid) // this is check validity
        //{
          using (db)
               {
                   var v = db.Users.Where(model => model.Email.Equals(u.Email) && model.Password.Equals(u.Password)).FirstOrDefault();
                   if (v != null)
                   {
                       Session["LogedUserID"] = v.Id.ToString();
                       Session["LogedUserFullname"] = v.FullName.ToString();
                       return RedirectToAction("AfterLogin", "User");
                   }
                   else
                   {
                       ModelState.AddModelError("", "Email Or Password is Incorrect.");
                   }
               }
          return View();
        //}
         }
         public ActionResult AfterLogin()
         {
             if (Session["LogedUserID"] != null)
             {
                 return View();
             }
             else
             {
                 return RedirectToAction("Login");
              }
         }

        //LogOff
         
        [HttpGet]
         public ActionResult LogOff()
         {
            Session.Clear();
            return RedirectToAction("Index", "Home");//sths
         }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
        #endregion

        public ActionResult SelectCategory()
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "user", Value = "0", Selected = true });

            items.Add(new SelectListItem { Text = "jobowner", Value = "1" });


            ViewBag.Role = items;

            return View();

        }
        //
        // GET: /User/Edit/5

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Users user = db.Users.Find(id);
            if (user == null)
            {
               // return HttpNotFound();
            }
            return View(user);
        }
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AfterLogin", "User");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}