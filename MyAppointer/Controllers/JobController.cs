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
    public class JobController : Controller
    {
        private MyAppointerEntities db = new MyAppointerEntities();

        //
        // GET: /Job/

        public ActionResult Index()
        {
            
            var jobs = db.Jobs.Include(j => j.JobTypes).Include(j => j.Users);
            return View(jobs.ToList());
        }

        //
        // GET: /Job/Details/5

        public ActionResult Details(int id = 0)
        {
            //id = db.JobOwners.Where(model => model.UserId.Equals(id)).FirstOrDefault().Id;

            Jobs jobs = db.Jobs.Where(model => model.Id.Equals(id)).FirstOrDefault() ;
            if (jobs == null)
            {
                return HttpNotFound();
            }
            return View(jobs);
        }
        //get
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "Title", "Select Job Type");
            ViewBag.FirstJobOwner = new SelectList(db.Users, "Id", "Email");          

            return View();
        }

        //
        // POST: /Job/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BigViewModel bv)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else if (Session["LogedUserID"].ToString() == "user")
            {
                return RedirectToAction("Index", "Home");
            }

            bv.Jobs.JobTypes = db.JobTypes.Where(model => model.Id.Equals(bv.Jobs.JobTypeId)).FirstOrDefault();
            
                bv.Jobs.FirstJobOwner = Int32.Parse(Session["LogedUserID"].ToString());
                bv.Jobs.Users = db.Users.Where(model => model.Id.Equals(bv.Jobs.FirstJobOwner)).FirstOrDefault();
                db.Jobs.Add(bv.Jobs);
                db.SaveChanges();

                bv.JobOwners = new JobOwners();
                bv.JobOwners.JobId = bv.Jobs.Id;
                bv.JobOwners.UserId = bv.Jobs.FirstJobOwner;
                db.JobOwners.Add(bv.JobOwners);
                db.SaveChanges();

                WorkingTimes WT = new WorkingTimes();

                WT.JobOwnerId = bv.JobOwners.Id;

                db.WorkingTimes.Add(WT);
                db.SaveChanges();
                
                foreach (WeeklyWorkingDays wwd in bv.WeeklyWorkingDays)
                {
                    wwd.WorkingTimes = WT;
                    db.WeeklyWorkingDays.Add(wwd);
                    db.SaveChanges();

                }

                foreach (WeeklyWorkingTimes wwt in bv.WeeklyWorkingTimes)
                {
                    wwt.WorkingTimes = WT;
                    db.WeeklyWorkingTimes.Add(wwt);
                    db.SaveChanges();

                }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Job/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else if (Session["Role"].ToString() == "user")
            {
                return RedirectToAction("Index", "Home");
            }

            Jobs jobs = db.Jobs.Find(id);
            if (jobs == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "Title", jobs.JobTypeId);
            ViewBag.FirstJobOwner = new SelectList(db.Users, "Id", "Email", jobs.FirstJobOwner);
            return View(jobs);
        }

        //
        // POST: /Job/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Jobs jobs)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else if (Session["LogedUserID"].ToString() == "user")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(jobs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "Title", jobs.JobTypeId);
            ViewBag.FirstJobOwner = new SelectList(db.Users, "Id", "Email", jobs.FirstJobOwner);
            return View(jobs);
        }

        //
        // GET: /Job/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else if (Session["Role"].ToString() == "user")
            {
                return RedirectToAction("Index", "Home");
            }

            Jobs jobs = db.Jobs.Find(id);
            if (jobs == null)
            {
                return HttpNotFound();
            }
            return View(jobs);
        }

        //
        // POST: /Job/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else if (Session["Role"].ToString() == "user")
            {
                return RedirectToAction("Index", "Home");
            }

            Jobs jobs = db.Jobs.Find(id);
            db.Jobs.Remove(jobs);
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