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
using MyAppointer.Filters;
using MyAppointer.Models;
namespace MyAppointer.Controllers
{
    public class UserController : Controller
    {
        private MyAppointerEntities db = new MyAppointerEntities();

       

        public ActionResult Index()
        {
            return View(db.Users.ToList());
           
        }

       

        public ActionResult Details(int id = 0)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

       

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            
            if (ModelState.IsValid)
            {
                var v = db.Users.Where(model => model.Email.Equals(users.Email)).FirstOrDefault();
                
                if(v==null)
                { db.Users.Add(users);
                db.SaveChanges();


                if (users.Role == "jobowner")
                {

                    return RedirectToAction("Create", "Job", new { FirstJobOwner = users.Id });

                }
                else
                {

                    return RedirectToAction("Index", "Home");
                }
            }
                else
                {
                    ViewBag.error = "error:please enter another Email";
                    return View(users);//jobs
                }


            }
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
        
          using (db)
               {
                   var v = db.Users.Where(model => model.Email.Equals(u.Email) && model.Password.Equals(u.Password)).FirstOrDefault();
                   if (v != null)
                   {
                       Session["LogedUserID"] = v.Id.ToString();
                       Session["LogedUserFullname"] = v.FullName.ToString();
                       HttpCookie aCookie = new HttpCookie("userInfo");
                       aCookie.Values["userName"] = u.Email;
                       aCookie.Values["lastVisit"] = DateTime.Now.ToString();
                       aCookie.Expires = DateTime.Now.AddDays(1);
                       Response.Cookies.Add(aCookie);
                       return RedirectToAction("AfterLogin", "User");
                   }
                   else
                   {
                       return RedirectToAction("Create", "User");
                   }
               }
        
         }
         public ActionResult AfterLogin()
         {
             if (Session["LogedUserID"] != null)
             {
                 return View();
             }
             else
             {
                 return RedirectToAction("Index");
              }
         }

         
        [HttpGet]
         public ActionResult LogOff(){

             Session["LogedUserID"] = null;
             Session["LogedUserFullname"] = null;
             if (Request.Cookies["UserInfo"]["userName"] != null)
             {
                 Request.Cookies["UserInfo"]["userName"]=null;
             }
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


        public ActionResult Edit(int id = 0)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
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