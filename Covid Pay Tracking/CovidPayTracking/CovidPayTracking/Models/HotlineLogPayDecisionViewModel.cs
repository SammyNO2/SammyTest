using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CovidPayTracking.Models
{
    public class HotlineLogPayDecisionViewModel
    {
        public string EmployeeID { get; set; }

        [Display(Name = "Employee First Name")]
        public string EmployeeFirstName { get; set; }

        [Display(Name = "Employee Last Name")]
        public string EmployeeLastName { get; set; }

        [Display(Name = "FTE Status")]
        public string FTEStatus { get; set; }

        [Display(Name = "Previous Year STD Election")]
        public string STDElection_PrevYear { get; set; }

        [Display(Name = "This Year STD Election")]
        public string STDElection_ThisYear { get; set; }

        public string STDElection_Detail { get; set; }

        [Display(Name = "Rate of Pay")]
        public string RateOfPay { get; set; }

        [Display(Name = "FTE Hours")]
        public string FTEHours { get; set; }

        [Display(Name = "Total STD Plus COVID Bank Hours")]
        public string TotalSTDPayHours { get; set; }

        [Display(Name = "Total STD Hours Paid- Gross (Prior to 10/1/21)")]
        public string TotalSTDHoursPaidByNSHGross { get; set; }

        public string RecordStatus { get; set; }
        public string PayReviewStatus { get; set; }

        public DateTime? LastDayWorked { get; set; }
        public DateTime? ReturnToWorkDt { get; set; }

        [Display(Name = "Made Whole?")]
        public bool MadeWhole { get; set; }

        [Display(Name = "Case Notes")]
        public string CaseNote { get; set; }

        [Display(Name = "Hours Left in COVID Bank")]
        public string DiffHoursToFTE { get; set; }

        public List<HotLineLogViewModel> HotlineLogsByEmployee { get; set; }

        public List<PayDecisionViewModel> PayDecisionsByEmployee { get; set; }

        public List<CovidTestViewModel> CovidTestsByEmployee { get; set; }

        public PayPeriodViewModel PayPeriodInfo { get; set; }

        [Display(Name = "Overall COVID Test Status")]
        public string OverallCovidTestStatus { get; set; }

        [Display(Name = "COVID Bank Pay- Gross (After 10/1/21)")]
        public string COVIDBankPayGross { get; set; }

        [Display(Name = "STD Plus COVID Bank Pay Total- Gross")]
        public string COVIDPayTotalGross { get; set; }

        public void HotlineLogPayDecisionViewModelMapToModel(HotlineLogPayDecisionViewModel hotlineLogPayDecisionView)
        {
            hotlineLogPayDecisionView.EmployeeID = EmployeeID;
            hotlineLogPayDecisionView.EmployeeFirstName = EmployeeFirstName;
            hotlineLogPayDecisionView.EmployeeLastName = EmployeeLastName;
            hotlineLogPayDecisionView.FTEStatus = FTEStatus;
            hotlineLogPayDecisionView.STDElection_PrevYear = STDElection_PrevYear;
            hotlineLogPayDecisionView.STDElection_ThisYear = STDElection_ThisYear;
            hotlineLogPayDecisionView.RateOfPay = RateOfPay;
            hotlineLogPayDecisionView.FTEHours = FTEHours;
            hotlineLogPayDecisionView.COVIDBankPayGross = COVIDBankPayGross;
            hotlineLogPayDecisionView.COVIDPayTotalGross = COVIDPayTotalGross;
            hotlineLogPayDecisionView.OverallCovidTestStatus = OverallCovidTestStatus;
            hotlineLogPayDecisionView.HotlineLogsByEmployee = new List<HotLineLogViewModel>();
            hotlineLogPayDecisionView.PayDecisionsByEmployee = new List<PayDecisionViewModel>();
            hotlineLogPayDecisionView.CovidTestsByEmployee = new List<CovidTestViewModel>();
            hotlineLogPayDecisionView.PayPeriodInfo = new PayPeriodViewModel();
        }
    }
}