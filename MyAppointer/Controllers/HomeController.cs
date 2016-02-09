using MyAppointer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAppointer.Controllers
{
    public class HomeController : Controller
    {
        private MyAppointerEntities db = new MyAppointerEntities();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult info(int JobOwnerId)
        {
            TimeTable timeTable = new TimeTable();
            Users user = db.Users.Find(JobOwnerId);
            Jobs job = db.Jobs.Where(model => model.FirstJobOwner.Equals(JobOwnerId)).FirstOrDefault();
            JobOwners jobowner = db.JobOwners.Where(model => model.JobId.Equals(job.Id)).FirstOrDefault();
            var services = db.Services.Where(model => model.JobOwnerId.Equals(jobowner.Id));

            foreach (Services service in services)
            {
                //ViewBag.Message += "  " + service.Title;
            }



            WorkingTimes workingTime = db.WorkingTimes.Where(model => model.JobOwnerId.Equals(jobowner.Id)).FirstOrDefault();
            var weeklyworkingdays = db.WeeklyWorkingDays.Where(model => model.WorkingTimesId.Equals(workingTime.Id));

            List<WeeklyWorkingDays> days = new List<WeeklyWorkingDays>();

            foreach (WeeklyWorkingDays wwd in weeklyworkingdays)
            {
                ViewBag.day += (wwd.Day+1).ToString();
                days.Add(wwd);
                //ViewBag.Message += "  " + wwd.Day; 
            }
            timeTable.days = days;

            ViewBag.days = days;

            var weeklyworkingTimes = db.WeeklyWorkingTimes.Where(model => model.WorkingTimesId.Equals(workingTime.Id));
            List<WeeklyWorkingTimes> times = new List<WeeklyWorkingTimes>();
            foreach (WeeklyWorkingTimes wwt in weeklyworkingTimes)
            {
                times.Add(wwt);
                ViewBag.Times += wwt.StartTime+"-" + wwt.EndTime + ",";
            }
            timeTable.times = times;

            ViewBag.timeTable = timeTable;

            var offdays = db.OffDays.Where(model => model.WorkingTimesId.Equals(workingTime.Id));
            foreach (OffDays od in offdays)
            {
                ViewBag.Message = od.OffDay;
            }



            //Services service = services[1];
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}




            return View();
        }
    }
}
