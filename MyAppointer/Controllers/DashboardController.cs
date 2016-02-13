using MyAppointer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAppointer.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        private MyAppointerEntities db = new MyAppointerEntities();

        public ActionResult Index()
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
            var q = db.JobOwners.Where(model => model.UserId.Equals(v.Id)).FirstOrDefault();

            var query =
            from Appointments in db.Appointments
            where Appointments.JobOwnerId == q.Id

            select Appointments;
            string[] ds = new string[10] ;
           

              var list = (from j in db.JobOwners
                          join app in db.Appointments on j.Id equals app.JobOwnerId
                          join u in db.Users on app.UserId equals u.Id
                          orderby app.BookDate,app.StartTime 

                          select new Dashboard
                          {
                             FullName= u.FullName,
                             BookDate=app.BookDate,
                             StartTime =app.StartTime,
                             EndTime=app.EndTime,
                             BookId=app.Id

                          }
                        ).ToList();

             

             

            return View(list);
            

        }

        //
        // GET: /Dashboard/Details/5
        [HttpGet]
        public ActionResult Confirm(int BookId)
        {
            var q = db.Appointments.Where(model => model.Id.Equals(BookId)).FirstOrDefault();
            q.isReserved=true;
            db.Entry(q).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }

        //
        // GET: /Dashboard/Create

        public ActionResult Cancle(int BookId)
        {
            var q = db.Appointments.Where(model => model.Id.Equals(BookId)).FirstOrDefault();
            q.isReserved = false;
            db.Entry(q).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }

        

        
    }
}
