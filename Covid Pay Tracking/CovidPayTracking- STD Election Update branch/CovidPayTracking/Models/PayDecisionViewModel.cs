using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CovidPayTracking.Models
{
    public class PayDecisionViewModel
    {
        [Key]
        [Display(Name = "Pay Decision ID")]
        public int ID { get; set; }
        public int HotlineLogID { get; set; }

        [Display(Name = "Employee ID")]
        public string EmployeeID { get; set; }

        [Display(Name = "Pay Decision Status")]
        [Required(ErrorMessage = "Status is Required")]
        public string PayDecisionStatus { get; set; }

        [Display(Name = "Clocked In/Cleared to Work Date?")]
        public string ClockedIn_ClearedToWork_Dt { get; set; }

        [Display(Name = "STD Pay Date To")]
        public DateTime? STDPayDtTo { get; set; }

        [Display(Name = "STD Pay Date From")]
        public DateTime? STDPayDtFrom { get; set; }

        [Display(Name = "COVID Bank Pay Hours")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter whole Number")]
       // [RegularExpression(@"^-?[0-9]\d{0,2}(\.\d{0,1})?$", ErrorMessage = "Please enter valid Number")]
        public string STDPayHours { get; set; }


        [Required(ErrorMessage = "Please enter Pay Decision Date")]
        [Display(Name = "Pay Decision Date")]
        public DateTime? PayDecisionDt { get; set; }

        [Display(Name = "Total STD Hours Paid- Gross (Prior to 10/1/21)")]
        [RegularExpression("^[+-]?[0-9]{1,9}(?:\\.[0-9]{0,2})?$")]
        public string STDHoursPaidByNSHGross { get; set; }

        [Display(Name = "PTO Adjustment")]
       // [RegularExpression(@"^-?[0-9]\d{0,2}(\.\d{0,1})?$", ErrorMessage = "Please enter valid Number")]
        public string PTOAdjustment { get; set; }

        [Display(Name = "Reverse Regular Hours")]
        public string ReverseRegHours { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "STD Pay Dates")]
        [Required(ErrorMessage = "STD Pay Date(From-To) is Required")]
        public string STDPayDates { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDt { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        public bool isActive { get; set; }

        [Display(Name = "Return To Work?")]
        public bool ReturnToWork { get; set; }

        [Display(Name = "Hours in Time Card")]
        public string HoursInTimeCard { get; set; }

        [Display(Name = "Made Whole?")]
        public bool MadeWhole { get; set; }

        [Display(Name = "This Year STD Election")]
        public string STDElection_ThisYear { get; set; }

        public string RateOfPay { get; set; }
        public string FTEHours { get; set; }
        public string FTEStatus { get; set; }
        public string TotalSTDPayHours { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LastDayWorked { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ReturnToWorkDt { get; set; }

        public IEnumerable<SelectListItem> PayDates { get; set; }

        [Display(Name = "COVID Bank Pay - Gross")]
        public string COVIDBankPayGross { get; set; }

        [Display(Name = "Cigna Paying STD?")]
        public bool Week1Cigna { get; set; }

        [Display(Name = "Hours in Timecard")]
        public string Week1TimeCardHours { get; set; }

        [Display(Name = "COVID Pay Hours")]
        public string Week1COVIDPayHours { get; set; }

        [Display(Name = "COVID Pay Gross")]
        public string Week1COVIDPayGross { get; set; }

        [Display(Name = "Cigna Paying STD?")]
        public bool Week2Cigna { get; set; }

        [Display(Name = "Hours in Timecard")]
        public string Week2TimeCardHours { get; set; }

        [Display(Name = "COVID Pay Hours")]
        public string Week2COVIDPayHours { get; set; }

        [Display(Name = "COVID Pay Gross")]
        public string Week2COVIDPayGross { get; set; }

        [Display(Name = "COVID Pay Total- Gross")]
        public string COVIDPayTotalGross { get; set; }

        [Display(Name = "PTO Pay Hours")]
        public string PTOHoursPaid { get; set; }

        [Display(Name = "PTO Pay Hours")]
        public string PTOHoursPaidWeek1 { get; set; }

        [Display(Name = "PTO Pay Hours")]
        public string PTOHoursPaidWeek2 { get; set; }

        [Display(Name = "PTO Pay Dollars")]
        public string PTODollarsPaid { get; set; }

        [Display(Name = "PTO Pay Dollars")]
        public string PTODollarsPaidWeek1 { get; set; }

        [Display(Name = "PTO Pay Dollars")]
        public string PTODollarsPaidWeek2 { get; set; }

        [Display(Name = "PTO Pay Hours")]
        public string PTOPayHours { get; set; }

        [Display(Name = "PTO Added By Timekeeper")]
        public string PTOAddedByTimekeeper { get; set; }

        [Display(Name = "Total PTO Adjustment")]
        public string TotalPTOAdjustment { get; set; }


        public void PayDecisionMapToModel(PayDecision pd)
        {
            pd.ID = ID;
            pd.HotlineLogID = HotlineLogID;
            pd.EmployeeID = EmployeeID;
            pd.PayDecisionStatus = PayDecisionStatus;
            pd.ClockedIn_ClearedToWork_Dt = ClockedIn_ClearedToWork_Dt;
            pd.STDPayDtTo = STDPayDtTo;
            pd.STDPayDtFrom = STDPayDtFrom;
            pd.STDPayHours = STDPayHours;
            pd.PayDecisionDt = PayDecisionDt;
            pd.STDHoursPaidByNSHGross = STDHoursPaidByNSHGross;
            pd.PTOAdjustment = PTOAdjustment;
            pd.ReverseRegHours = ReverseRegHours;
            pd.Notes = Notes;
            pd.STDPayDates = STDPayDates;
            pd.CreatedDt = CreatedDt;
            pd.CreatedBy = CreatedBy;
            pd.ModifiedDt = ModifiedDt;
            pd.ModifiedBy = ModifiedBy;
            pd.isActive = isActive;
            pd.ReturnToWork = ReturnToWork;
            pd.HoursInTimeCard = HoursInTimeCard;
            pd.MadeWhole = MadeWhole;
            pd.COVIDBankPayGross = COVIDBankPayGross;
            pd.Week1Cigna = Week1Cigna;
            pd.Week2Cigna = Week2Cigna;
            pd.Week1TimeCardHours = Week1TimeCardHours;
            pd.Week2TimeCardHours = Week2TimeCardHours;
            pd.Week1COVIDPayHours = Week1COVIDPayHours;
            pd.Week2COVIDPayHours = Week2COVIDPayHours;
            pd.Week1COVIDPayGross = Week1COVIDPayGross;
            pd.Week2COVIDPayGross = Week2COVIDPayGross;
            pd.COVIDPayTotalGross = COVIDPayTotalGross;
            pd.PTOHoursPaid = PTOHoursPaid;
            pd.PTODollarsPaid = PTODollarsPaid;
            pd.PTOHoursPaidWeek1 = PTOHoursPaidWeek1;
            pd.PTODollarsPaidWeek1 = PTODollarsPaidWeek1;
            pd.PTOHoursPaidWeek2 = PTOHoursPaidWeek2;
            pd.PTODollarsPaidWeek2 = PTODollarsPaidWeek2;
            pd.PTOPayHours = PTOPayHours;
            pd.PTOAddedByTimekeeper = PTOAddedByTimekeeper;
            pd.TotalPTOAdjustment = TotalPTOAdjustment;
        }
    }
}