using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAppointer.Models
{
    public class PersianDatePicker
    {


        [UIHint("DatePicker")]
        public DateTime DatePicker { get; set; }
    }
}