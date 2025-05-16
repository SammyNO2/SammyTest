using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CovidPayTracking.Models
{
    public class EmailViewModel
    {
        public string EmployeeID { get; set; }
        public string STDElection_ThisYear { get; set; }
        public string TotalSTDPayHours { get; set; }
        public string STDPayHours { get; set; }
        public DateTime? PayDecisionDt { get; set; }
        public string STDHoursPaidByNSHGross { get; set; }
        public string DiffHoursToFTE { get; set; }
        public string FTEHours { get; set; }

    }
}