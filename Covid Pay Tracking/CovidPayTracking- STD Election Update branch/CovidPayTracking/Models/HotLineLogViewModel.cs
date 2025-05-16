using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CovidPayTracking.Models
{
    public class HotLineLogViewModel
    {

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter Employee Number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter 6 digit Employee ID"), StringLength(8, ErrorMessage = "Please enter 6 digit Employee ID")]
        [Display(Name = "Employee ID")]
        public string EmployeeID { get; set; }

        [Display(Name = "Employee First Name")]
        public string EmployeeFirstName { get; set; }

        [Display(Name = "Employee Last Name")]
        public string EmployeeLastName { get; set; }

        [Display(Name = "Date of Call")]
        public DateTime? DateOfCall { get; set; }

        [Display(Name = "Operator")]
        public string Operator { get; set; }

        [Display(Name = "FTE Status")]
        public string FTEStatus { get; set; }

        [Display(Name = "Cleared to Work?")]
        public string ClearedToWork { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Last Day Worked")]
        public DateTime? LastDayWorked { get; set; }

        [Display(Name = "Return To Work Date")]
        public DateTime? ReturnToWorkDt { get; set; }

        [Display(Name = "Previous Year STD Election")]
        public string STDElection_PrevYear { get; set; }

        [Display(Name = "This Year STD Election")]
        public string STDElection_ThisYear { get; set; }

        [Display(Name = "Rate of Pay")]
        public string RateOfPay { get; set; }

        [Display(Name = "Hours Previously Paid")]
        public string HoursPreviouslyPaid { get; set; }

        [Display(Name = "FTE Hours")]
        public string FTEHours { get; set; }

        [Display(Name = "Hours Left in COVID Bank")]
        public string DiffHoursToFTE { get; set; }

        [Display(Name = "Notes from Benefits")]
        public string NotesFromBenefits { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDt { get; set; }

        [Display(Name = "Modified By")]
        public string Modifiedby { get; set; }

        [Display(Name = "Pay Period Pay Date")]
        public DateTime? PayPeriodPayDate { get; set; }
        public int? RawDataNo { get; set; }

        public DateTime? SelectedFromDate { get; set; }

        public DateTime? SelectedToDate { get; set; }
         
        public bool isActive { get; set; }

        [Display(Name = "Overall Review Status")]
        public string RecordStatus { get; set; }

        [Display(Name = "Pay Review Status")]
        public string PayReviewStatus { get; set; }

        [Display(Name = "Total STD Plus COVID Bank Hours")]
        public string TotalSTDPayHours { get; set; }

        [Display(Name = "Total STD Hours Paid- Gross (Prior to 10/1/21)")]
        public string TotalSTDHoursPaidByNSHGross { get; set; }

        public string EmplIDFrom { get; set; }
        public string EmplIDTo { get; set; }

        [Display(Name = "COVID Bank Pay Total - Gross (After 10/1/21)")]
        public string COVIDPayTotalGross { get; set; }

        public void HotlineLogMapToModel(HotlineLog hll)
        {
            hll.ID = ID;
            hll.EmployeeID = EmployeeID;
            hll.EmployeeFirstName = EmployeeFirstName;
            hll.EmployeeLastName = EmployeeLastName;
            hll.DateOfCall = DateOfCall;
            hll.Operator = Operator;
            hll.FTEStatus = FTEStatus;
            hll.ClearedToWork = ClearedToWork;
            hll.Notes = Notes;
            hll.LastDayWorked = LastDayWorked;
            hll.ReturnToWorkDt = ReturnToWorkDt;
            hll.STDElection_PrevYear = STDElection_PrevYear;
            hll.RateOfPay = RateOfPay;
            hll.HoursPreviouslyPaid = HoursPreviouslyPaid;
            hll.PayPeriodPayDate = PayPeriodPayDate;
            hll.FTEHours = FTEHours;
            hll.DiffHoursToFTE = DiffHoursToFTE;
            hll.RawDataNo = RawDataNo;
            hll.NotesFromBenefits = NotesFromBenefits;
            hll.CreatedDt = CreatedDt;
            hll.CreatedBy = CreatedBy;
            hll.Modifiedby = Modifiedby;
            hll.ModifiedDt = ModifiedDt;
            hll.isActive = isActive;
            hll.STDElection_ThisYear = STDElection_ThisYear;
            hll.RecordStatus = RecordStatus;
            hll.PayReviewStatus = PayReviewStatus;

        }
    }

}