//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAppointer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WeeklyWorkingTimes
    {
        public int Id { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int WorkingTimesId { get; set; }
    
        public virtual WorkingTimes WorkingTimes { get; set; }
    }
}
