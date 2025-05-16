using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidPayTracking.Models
{
    public class EmployeeMeritSTDElectionViewModel
    {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TotalSTDElectionPrevYear { get; set; }
        public string TotalSTDElectionThisYear { get; set; }
        public decimal? HourlyPayRate { get; set; }
        public decimal? FTEStatus { get; set; }
    }
}