using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAppointer.Controllers
{
    public class ServicesController : Controller
    {
        //
        // GET: /Services/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Services/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Services/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Services/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

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

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Services/Delete/5

        [HttpPost]
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
