using MyAppointer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAppointer.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        private MyAppointerEntities db = new MyAppointerEntities();
        [HttpGet]
        public ViewResult Index()
        {
            var user = new List<Users>() ;

            return View(user);
        }

        [HttpPost]
        public ViewResult Index(string city, string jtype)
        {
            if (jtype == null || jtype == "")
            {
                var user = (from u in db.Users
                            where u.City == city
                            select u).ToList();
                return View(user);
            }
            else
            {
                if (city == "" || city == null)
                {
                    var jt = db.JobTypes.Where(model => model.Title.Equals(jtype)).FirstOrDefault();


                    var user = (from j in db.Jobs
                                join u in db.Users on j.FirstJobOwner equals u.Id
                                where j.JobTypeId == jt.Id
                                select u).ToList();
                    return View(user);
                }
                else
                {
                    var jt = db.JobTypes.Where(model => model.Title.Equals(jtype)).FirstOrDefault();
                    


                    var user = (from j in db.Jobs
                                join u in db.Users on j.FirstJobOwner equals u.Id
                                where j.JobTypeId == jt.Id && u.City == city
                                select u).ToList();
                    return View(user);
                }

            }
        }
       
       


    }
}
