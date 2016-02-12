using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAppointer.Models;

namespace MyAppointer.Controllers
{
    public class JobTypesController : Controller
    {
        private MyAppointerEntities db = new MyAppointerEntities();

        //
        // GET: /JobTypes/

        public ActionResult Index()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }else if (Session["Role"].ToString() == "admin")
            {
                return View(db.JobTypes.ToList());
            }else{
                return RedirectToAction("Login","Home");
            }
            
        }

        //
        // GET: /JobTypes/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }else if (Session["Role"].ToString() != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            JobTypes jobtypes = db.JobTypes.Find(id);
            if (jobtypes == null)
            {
                return HttpNotFound();
            }
            return View(jobtypes);
        }

        //
        // GET: /JobTypes/Create

        public ActionResult Create()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            } else if (Session["Role"].ToString() != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        //
        // POST: /JobTypes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobTypes jobtypes)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }else if (Session["Role"].ToString() != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.JobTypes.Add(jobtypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobtypes);
        }

        //
        // GET: /JobTypes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }else if (Session["Role"].ToString() != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            JobTypes jobtypes = db.JobTypes.Find(id);
            if (jobtypes == null)
            {
                return HttpNotFound();
            }
            return View(jobtypes);
        }

        //
        // POST: /JobTypes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobTypes jobtypes)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }else if (Session["Role"].ToString() != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(jobtypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobtypes);
        }

        //
        // GET: /JobTypes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            } else if (Session["Role"].ToString() != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            JobTypes jobtypes = db.JobTypes.Find(id);
            if (jobtypes == null)
            {
                return HttpNotFound();
            }
            return View(jobtypes);
        }

        //
        // POST: /JobTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login");
            }else if (Session["Role"].ToString() != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            JobTypes jobtypes = db.JobTypes.Find(id);
            db.JobTypes.Remove(jobtypes);
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