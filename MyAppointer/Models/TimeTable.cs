﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAppointer.Models
{
    public class TimeTable
    {
        public List<int> days { get; set; }
        public List<WeeklyWorkingTimes> times { get; set; }
    }
}