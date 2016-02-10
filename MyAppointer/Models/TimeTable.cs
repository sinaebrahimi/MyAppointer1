using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAppointer.Models
{
    public class TimeTable
    {
        public List<int> days { get; set; }
        public List<WeeklyWorkingTimes> times { get; set; }
        public JobOwners jobOwner { get; set; }
        public string userId { get; set; }
        public List<Appointments> appointments { get; set; }
    }
}