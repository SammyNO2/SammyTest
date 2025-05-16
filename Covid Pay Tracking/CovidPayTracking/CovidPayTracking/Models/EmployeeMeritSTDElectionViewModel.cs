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
        public string SSN { get; set; }
        public string Status { get; set; }
        public string StructureGroup { get; set; }
        public string STDElection { get; set; }
        public string STDElectionPercent { get; set; }
        public string STDReview { get; set; }
        public DateTime? STDEffectiveDt { get; set; }
        public string STD2Election { get; set; }
        public string STD2ElectionPercent { get; set; }
        public string TotalSTDElectionPercent { get; set; }
        public string BenefitStatus { get; set; }
        public string TotalSTDElectionPrevYear { get; set; }
        public string TotalSTDElectionThisYear { get; set; }
        public decimal? HourlyPayRate { get; set; }
        public decimal? FTEStatus { get; set; }
    }
}