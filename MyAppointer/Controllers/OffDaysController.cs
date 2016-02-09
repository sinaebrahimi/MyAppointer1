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
    public class OffDaysController : Controller
    {
        private MyAppointerEntities db = new MyAppointerEntities();

        //
        // GET: /OffDays/

        public ActionResult Index()
        {
            var offdays = db.OffDays.Include(o => o.WorkingTimes);
            return View(offdays.ToList());
        }

        //
        // GET: /OffDays/Details/5

        public ActionResult Details(int id = 0)
        {
            OffDays offdays = db.OffDays.Find(id);
            if (offdays == null)
            {
                return HttpNotFound();
            }
            return View(offdays);
        }

        //
        // GET: /OffDays/Create

        public ActionResult Create()
        {
            ViewBag.WorkingTimesId = new SelectList(db.WorkingTimes, "Id", "Id");
            return View();
        }

        //
        // POST: /OffDays/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OffDays offdays)
        {


            //offdays.OffDay = Int32.Parse(offdays.OffDay);
            if (ModelState.IsValid)
            {
                db.OffDays.Add(offdays);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkingTimesId = new SelectList(db.WorkingTimes, "Id", "Id", offdays.WorkingTimesId);
            return View(offdays);
        }

        //
        // GET: /OffDays/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OffDays offdays = db.OffDays.Find(id);
            if (offdays == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkingTimesId = new SelectList(db.WorkingTimes, "Id", "Id", offdays.WorkingTimesId);
            return View(offdays);
        }

        //
        // POST: /OffDays/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OffDays offdays)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offdays).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkingTimesId = new SelectList(db.WorkingTimes, "Id", "Id", offdays.WorkingTimesId);
            return View(offdays);
        }

        //
        // GET: /OffDays/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OffDays offdays = db.OffDays.Find(id);
            if (offdays == null)
            {
                return HttpNotFound();
            }
            return View(offdays);
        }

        //
        // POST: /OffDays/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OffDays offdays = db.OffDays.Find(id);
            db.OffDays.Remove(offdays);
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