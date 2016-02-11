using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAppointer.Models
{
    public class BigViewModel
    {
        public Jobs Jobs { get; set; }
        public JobTypes JobTypes { get; set; }
        public JobOwners JobOwners { get; set; }
        public WorkingTimes WorkingTimes { get; set; }
        public OffDays OffDays { get; set; }
        public WeeklyWorkingDays[] WeeklyWorkingDays { get; set; }
        public WeeklyWorkingTimes[] WeeklyWorkingTimes { get; set; }
    }
}