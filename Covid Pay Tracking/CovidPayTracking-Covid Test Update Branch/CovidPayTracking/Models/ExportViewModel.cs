using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidPayTracking.Models
{
    public class ExportViewModel
    {
        public string EmployeeID { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime? DateOfCall { get; set; }
        public string Operator { get; set; }
        public string FTEStatus { get; set; }
        public string ClearedToWork { get; set; }
        public string HotlineLogNotes { get; set; }
        public DateTime? LastDayWorked { get; set; }
        public DateTime? ReturnToWorkDt { get; set; }
        public string STDElection_PrevYear { get; set; }
        public string STDElection_ThisYear { get; set; }
        public string RateOfPay { get; set; }
        public string HoursPreviouslyPaid { get; set; }
        public string FTEHours { get; set; }
        public string DiffHoursToFTE { get; set; }
        public string NotesFromBenefits { get; set; }
        public DateTime? HotlineLogCreatedDt { get; set; }
        public string HotlineLogCreatedBy { get; set; }
        public DateTime? HotlineLogModifiedDt { get; set; }
        public string HotlineLogModifiedby { get; set; }
        public string RecordStatus { get; set; }
        public string PayReviewStatus { get; set; }
        public DateTime? PayPeriodPayDate { get; set; }
        public bool? HotlineLogisActive { get; set; }
        public string PayDecisionStatus { get; set; }
        public string ClockedIn_ClearedToWork_Dt { get; set; }
        public DateTime? STDPayDtTo { get; set; }
        public DateTime? STDPayDtFrom { get; set; }
        public string HoursInTimeCard { get; set; }
        public string STDPayHours { get; set; }
        public DateTime? PayDecisionDt { get; set; }
        public string STDHoursPaidByNSHGross { get; set; }
        public string PTOHoursPaid { get; set; }
        public string PTODollarsPaid { get; set; }
        public string COVIDBankPayGross { get; set; }
        public string Week1Cigna { get; set; }
        public string Week1TimeCardHours { get; set; }
        public string Week1COVIDPayHours { get; set; }
        public string Week1COVIDPayGross { get; set; }
        public string Week1PTOHoursPaid { get; set; }
        public string Week1PTODollarsPaid { get; set; }
        public string Week2Cigna { get; set; }
        public string Week2TimeCardHours { get; set; }
        public string Week2COVIDPayHours { get; set; }
        public string Week2COVIDPayGross { get; set; }
        public string Week2PTOHoursPaid { get; set; }
        public string Week2PTODollarsPaid { get; set; }
        public string COVIDPayTotalGross { get; set; }
        public string PTOAdjustment { get; set; }
        public string ReverseRegHours { get; set; }
        public string PayDecisionNotes { get; set; }
        public string SentToCigna { get; set; }
        public string STDPayDates { get; set; }
        public DateTime? PayDecisionCreatedDt { get; set; }
        public string PayDecisionCreatedBy { get; set; }
        public DateTime? PayDecisionModifiedDt { get; set; }
        public string PayDecisionModifiedBy { get; set; }
        public bool? PayDecisionisActive { get; set; }
        public string PTOPayHours { get; set; }
        public string PTOAddedByTimekeeper { get; set; }
        public string TotalPTOAdjustment { get; set; }
        public int countPD { get; set; }
        public DateTime? latestPD { get; set; }
        public DateTime? oldestPD { get; set; }
    }
}