
namespace CovidPayTracking.Models.Mapper
{
    public static class MapFromModel
    {
        public static HotLineLogViewModel MapHLLViewModel(HotlineLog hotlineLog)
        {
            var hll = new HotLineLogViewModel();
            hll.ID = hotlineLog.ID;
            hll.EmployeeID = hotlineLog.EmployeeID;
            hll.EmployeeFirstName = hotlineLog.EmployeeFirstName;
            hll.EmployeeLastName = hotlineLog.EmployeeLastName;
            hll.DateOfCall = hotlineLog.DateOfCall;
            hll.Operator = hotlineLog.Operator;
            hll.FTEStatus = hotlineLog.FTEStatus;
            hll.ClearedToWork = hotlineLog.ClearedToWork;
            hll.Notes = hotlineLog.Notes;
            hll.LastDayWorked = hotlineLog.LastDayWorked;
            hll.ReturnToWorkDt = hotlineLog.ReturnToWorkDt;
            hll.STDElection_PrevYear = hotlineLog.STDElection_PrevYear;
            hll.STDElection_ThisYear = hotlineLog.STDElection_ThisYear;
            hll.RateOfPay = hotlineLog.RateOfPay;
            hll.HoursPreviouslyPaid = hotlineLog.HoursPreviouslyPaid;
            hll.PayPeriodPayDate = hotlineLog.PayPeriodPayDate;
            hll.FTEHours = hotlineLog.FTEHours;
            hll.DiffHoursToFTE = hotlineLog.DiffHoursToFTE;
            hll.RawDataNo = hotlineLog.RawDataNo;
            hll.NotesFromBenefits = hotlineLog.NotesFromBenefits;
            hll.CreatedDt = hotlineLog.CreatedDt;
            hll.CreatedBy = hotlineLog.CreatedBy;
            hll.Modifiedby = hotlineLog.Modifiedby;
            hll.ModifiedDt = hotlineLog.ModifiedDt;
            hll.isActive = hotlineLog.isActive;
            hll.RecordStatus = hotlineLog.RecordStatus;
            hll.PayReviewStatus = hotlineLog.PayReviewStatus;
            return hll;
        }

        public static PayDecisionViewModel MapPDViewModel(PayDecision payDecision)
        {
            var pd = new PayDecisionViewModel();
            pd.ID = payDecision.ID;
            pd.HotlineLogID = payDecision.HotlineLogID;
            pd.EmployeeID = payDecision.EmployeeID;
            pd.PayDecisionStatus = payDecision.PayDecisionStatus;
            pd.ClockedIn_ClearedToWork_Dt = payDecision.ClockedIn_ClearedToWork_Dt;
            pd.STDPayDtTo = payDecision.STDPayDtTo;
            pd.STDPayDtFrom = payDecision.STDPayDtFrom;
            pd.STDPayHours = payDecision.STDPayHours;
            pd.PayDecisionDt = payDecision.PayDecisionDt;
            pd.STDHoursPaidByNSHGross = payDecision.STDHoursPaidByNSHGross;
            pd.PTOAdjustment = payDecision.PTOAdjustment;
            pd.ReverseRegHours = payDecision.ReverseRegHours;
            pd.Notes = payDecision.Notes;
            pd.STDPayDates = payDecision.STDPayDates;
            pd.CreatedDt = payDecision.CreatedDt;
            pd.CreatedBy = payDecision.CreatedBy;
            pd.ModifiedDt = payDecision.ModifiedDt;
            pd.ModifiedBy = payDecision.ModifiedBy;
            pd.ReturnToWork = payDecision.ReturnToWork;
            pd.HoursInTimeCard = payDecision.HoursInTimeCard;
            pd.MadeWhole = payDecision.MadeWhole;
            pd.COVIDBankPayGross = payDecision.COVIDBankPayGross;
            pd.Week1Cigna = (bool)payDecision.Week1Cigna;
            pd.Week2Cigna = (bool)payDecision.Week2Cigna;
            pd.Week1TimeCardHours = payDecision.Week1TimeCardHours;
            pd.Week2TimeCardHours = payDecision.Week2TimeCardHours;
            pd.Week1COVIDPayHours = payDecision.Week1COVIDPayHours;
            pd.Week2COVIDPayHours = payDecision.Week2COVIDPayHours;
            pd.Week1COVIDPayGross = payDecision.Week1COVIDPayGross;
            pd.Week2COVIDPayGross = payDecision.Week2COVIDPayGross;
            pd.COVIDPayTotalGross = payDecision.COVIDPayTotalGross;
            pd.PTOHoursPaid = payDecision.PTOHoursPaid;
            pd.PTODollarsPaid = payDecision.PTODollarsPaid;
            pd.PTOHoursPaidWeek1 = payDecision.PTOHoursPaidWeek1;
            pd.PTODollarsPaidWeek1 = payDecision.PTODollarsPaidWeek1;
            pd.PTOHoursPaidWeek2 = payDecision.PTOHoursPaidWeek2;
            pd.PTODollarsPaidWeek2 = payDecision.PTODollarsPaidWeek2;
            pd.PTOPayHours = payDecision.PTOPayHours;
            pd.PTOAddedByTimekeeper = payDecision.PTOAddedByTimekeeper;
            pd.TotalPTOAdjustment = payDecision.TotalPTOAdjustment;
            return pd;
        }

        public static EmployeeMeritSTDElectionViewModel MapMEViewModel (EmployeeSTDElection ese, EmployeeMerit em)
        {
            var emse = new EmployeeMeritSTDElectionViewModel();
            emse.EmployeeID = ese.EmployeeID;
            emse.FirstName = em.EmployeeFirstName;
            emse.LastName = em.EmployeeLastName;
            emse.TotalSTDElectionPrevYear = ese.TotalSTDElectionPrevYear;
            emse.TotalSTDElectionThisYear = ese.TotalSTDElectionThisYear;
            emse.HourlyPayRate = em.HourlyPayRate;
            emse.FTEStatus = em.CombinedFTEStatus;
            return emse;
        }
        
    }
}