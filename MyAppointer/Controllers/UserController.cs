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
using System.Security.Cryptography;

namespace MyAppointer.Controllers
{
    public class UserController : Controller
    {
        private MyAppointerEntities db = new MyAppointerEntities();

        public ActionResult Index()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "admin")
                {
                    return View(db.Users.ToList());
                }
                else if (Session["Role"].ToString() == "jobowner" || (Session["Role"].ToString() == "user"))
                {
                    return RedirectToAction("ListAppointment");
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult Details(int id = 0)
        {
           
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }
            else if (Session["Role"].ToString() == "jobowner" || (Session["Role"].ToString() == "user"))
            {
                id = Int32.Parse(Session["LogedUserID"].ToString());
            }

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
                db.Users.Add(users);
                db.SaveChanges();
                if (users.Role == "jobowner")
                {
                    Session["LogedUserID"] = users.Id.ToString();
                    Session["LogedUserFullname"] = users.FullName;
                    Session["Role"] = users.Role;
                    return RedirectToAction("Create", "Job", new { FirstJobOwner = users.Id });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(users);
        }

        [HttpPost]
        public JsonResult doesUserNameExist(Users u)
        {
            var v = db.Users.Where(model => model.Email.Equals(u.Email)).FirstOrDefault();
            return Json(v == null);
        }

        [HttpPost]
        public JsonResult doesPhoneNumberExist(Users u)
        {
            var v = db.Users.Where(model => model.Phone.Equals(u.Phone)).FirstOrDefault();
            return Json(v == null);

        }

        //login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users l)
        {

            using (db)
            {
                var v = db.Users.Where(model => model.Email.Equals(l.Email) && model.Password.Equals(l.Password)).FirstOrDefault();
                if (v != null)
                {
                    Session["LogedUserID"] = v.Id.ToString();
                    Session["LogedUserFullname"] = v.FullName;
                    Session["Role"] = v.Role;

                    HttpCookie aCookie = new HttpCookie("userInfo");
                    aCookie.Values["userName"] = l.Email;
                    aCookie.Values["Role"] = l.Role;
                    aCookie.Values["lastVisit"] = DateTime.Now.ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);
                    if(v.Role == "user")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("AfterLogin", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email Or Password is Incorrect.");
                }
            }
            return View();
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
            
            Session["LogedUserID"] = null;
            Session["LogedUserFullname"] = null;
            Session["Role"] = null;
            if (Request.Cookies["UserInfo"]["userName"] != null)
            {
                Request.Cookies["UserInfo"]["userName"] = null;
            }
            return RedirectToAction("Index", "Home");//sths
        }

        //
        // GET: /User/Edit/5

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == null)
                {
                    return RedirectToAction("Login");
                }
                else if (Session["Role"].ToString() == "jobowner" || (Session["Role"].ToString() == "user"))
                {
                    id = Int32.Parse(Session["LogedUserID"].ToString());
                }

                Users user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Email = user.Email;
                ViewBag.Phone = user.Phone;
                return View(user);
            }
            return RedirectToAction("Login");
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users editu ,string NewEmail,string NewPhone)
        {
            editu.Email = NewEmail;
            editu.Phone = NewPhone;
            if (ModelState.IsValid)
            {
                db.Entry(editu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AfterLogin", "User");
            }
            return View(editu);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }
            else if (Session["Role"].ToString() == "jobowner" || (Session["Role"].ToString() == "user"))
            {
                id = Int32.Parse(Session["LogedUserID"].ToString());
            }

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

        public ActionResult ListAppointment()
        {
            IEnumerable<MyAppointer.Models.Appointments> appointments;
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }
            else if (Session["Role"].ToString() == "jobowner") {
                var str = Session["LogedUserID"].ToString();
                appointments = db.Appointments.Where(model => model.JobOwners.UserId.Equals(Int32.Parse(str)));
            }
            else if(Session["Role"].ToString() == "user")
            {
                int id = Int32.Parse(Session["LogedUserID"].ToString());
                appointments = db.Appointments.Where(model => model.JobOwnerId.Equals(id));
            }
            else
            {
                appointments = db.Appointments;
            }
            return View(appointments);
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        
    }
}