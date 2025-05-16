using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using System.Data;
using CovidPayTracking.Models;

namespace CovidPayTracking.Helper
{
    public class ExportReport
    {
        public static List<ExportViewModel> GetExports (HotLineLogEntities db, DateTime? fromDt = null, DateTime? toDt = null)
        {
            var queryRecords = (from hl in db.HotlineLogs.Where(e => e.EmployeeID !=null)
                                join pd in db.PayDecisions.Where(o => o.isActive == true)
                                on hl.EmployeeID equals pd.EmployeeID into g
                                //where pd.isActive == true && hl.EmployeeID !=null 
                                from pd in g.DefaultIfEmpty()
                                select new ExportViewModel()
                                {
                                    EmployeeID = hl.EmployeeID
                                    , EmployeeFirstName = hl.EmployeeFirstName
                                    , EmployeeLastName = hl.EmployeeLastName
                                    , DateOfCall = hl.DateOfCall
                                    , Operator = hl.Operator
                                    , FTEStatus = hl.FTEStatus
                                    , ClearedToWork = hl.ClearedToWork
                                    , HotlineLogNotes = hl.Notes
                                    , LastDayWorked = hl.LastDayWorked
                                    ,ReturnToWorkDt = hl.ReturnToWorkDt
                                    ,STDElection_PrevYear = hl.STDElection_PrevYear
                                    ,STDElection_ThisYear = hl.STDElection_ThisYear
                                    ,RateOfPay = hl.RateOfPay
                                    ,HoursPreviouslyPaid = hl.HoursPreviouslyPaid
                                    ,FTEHours = hl.FTEHours
                                    ,DiffHoursToFTE = hl.DiffHoursToFTE 
                                    ,NotesFromBenefits = hl.NotesFromBenefits
                                    ,HotlineLogCreatedDt = hl.CreatedDt
                                    ,HotlineLogCreatedBy = hl.CreatedBy
                                    ,HotlineLogModifiedDt = hl.ModifiedDt
                                    ,HotlineLogModifiedby = hl.Modifiedby
                                    ,HotlineLogisActive = hl.isActive
                                    ,PayDecisionStatus = pd.PayDecisionStatus
                                    ,ClockedIn_ClearedToWork_Dt = pd.ClockedIn_ClearedToWork_Dt
                                    ,STDPayHours = pd.STDPayHours
                                    ,PayDecisionDt = pd.PayDecisionDt
                                    ,STDHoursPaidByNSHGross = pd.STDHoursPaidByNSHGross
                                    ,COVIDBankPayGross = pd.COVIDBankPayGross
                                    ,COVIDPayTotalGross = pd.COVIDPayTotalGross
                                    ,Week1Cigna = pd.Week1Cigna.ToString()
                                    ,Week1TimeCardHours = pd.Week1TimeCardHours
                                    ,Week1COVIDPayHours = pd.Week1COVIDPayHours
                                    ,Week1COVIDPayGross = pd.Week1COVIDPayGross
                                    ,Week2Cigna = pd.Week2Cigna.ToString()
                                    ,Week2TimeCardHours = pd.Week2TimeCardHours
                                    ,Week2COVIDPayHours = pd.Week2COVIDPayHours
                                    ,Week2COVIDPayGross = pd.Week2COVIDPayGross
                                    ,PTOAdjustment = pd.PTOAdjustment
                                    ,ReverseRegHours  = pd.ReverseRegHours
                                    ,PayDecisionNotes  = pd.Notes
                                    ,SentToCigna = pd.SentToCigna
                                    ,STDPayDates = pd.STDPayDates
                                    ,PayDecisionCreatedDt = pd.CreatedDt
                                    ,PayDecisionCreatedBy = pd.CreatedBy
                                    ,PayDecisionModifiedDt = pd.ModifiedDt
                                    ,PayDecisionModifiedBy = pd.ModifiedBy
                                    ,PayDecisionisActive = pd.isActive
                                    ,PTOHoursPaid = pd.PTOHoursPaid
                                    ,PTODollarsPaid = pd.PTODollarsPaid
                                    ,Week1PTOHoursPaid = pd.PTOHoursPaidWeek1
                                    ,Week1PTODollarsPaid = pd.PTODollarsPaidWeek1
                                    ,Week2PTOHoursPaid = pd.PTOHoursPaidWeek2
                                    ,Week2PTODollarsPaid = pd.PTODollarsPaidWeek2
                                    ,PTOPayHours = pd.PTOPayHours
                                    ,PTOAddedByTimekeeper = pd.PTOAddedByTimekeeper
                                    ,TotalPTOAdjustment = pd.TotalPTOAdjustment
                                }).Distinct().ToList();
            queryRecords = queryRecords.Where(x => (fromDt == null || x.DateOfCall >= fromDt) && (toDt == null || x.DateOfCall <= toDt)).ToList();
            return queryRecords;
        }

        public static List<ExportViewModel> GetExportPayDate(HotLineLogEntities db, DateTime? fromDt = null, DateTime? toDt = null)
        {
            var queryRecords = (from pd in db.PayDecisions.Where(o => o.isActive == true && o.PayDecisionStatus.Contains("Yes"))
                                join hl in db.HotlineLogs on pd.EmployeeID equals hl.EmployeeID
                                group pd by new { pd.EmployeeID, pd.PayDecisionStatus, pd.PayDecisionDt,pd.CreatedBy, pd.STDHoursPaidByNSHGross, pd.PTOAdjustment, pd.ReverseRegHours, pd.STDPayDates, pd.COVIDPayTotalGross, pd.STDPayHours, pd.Week1Cigna, pd.Week2Cigna, pd.Week1COVIDPayHours, pd.Week2COVIDPayHours }
                                    into g
                                //where pd.isActive == true && pd.PayDecisionStatus.Contains("Yes") 
                                //&& hl.STDElection_2020 != "" && hl.HoursPreviouslyPaid != "0" && hl.EmployeeID != null
                                select new ExportViewModel()
                                {
                                    EmployeeID = g.Key.EmployeeID
                                    //,
                                    //EmployeeFirstName = g.Key.EmployeeFirstName
                                    //,
                                    //EmployeeLastName = g.Key.EmployeeLastName
                                    //,STDElection_2021 = hl.STDElection_2021
                                    //,RateOfPay = hl.RateOfPay
                                    //,FTEHours = hl.FTEHours
                                    ,
                                    PayDecisionStatus = g.Key.PayDecisionStatus
                                    ,STDPayHours = g.Key.STDPayHours
                                    ,Week1Cigna = (bool)(g.Key.Week1Cigna) ? "true" : "false"
                                    ,Week2Cigna = (bool)(g.Key.Week2Cigna) ? "true" : "false"
                                    ,Week1COVIDPayHours = g.Key.Week1COVIDPayHours
                                    ,Week2COVIDPayHours = g.Key.Week2COVIDPayHours
                                    ,
                                    PayDecisionDt = g.Key.PayDecisionDt
                                    ,
                                    STDHoursPaidByNSHGross = g.Key.STDHoursPaidByNSHGross
                                    ,
                                    PTOAdjustment = g.Key.PTOAdjustment
                                    ,
                                    ReverseRegHours = g.Key.ReverseRegHours
                                    ,
                                    STDPayDates = g.Key.STDPayDates
                                    //,PayDecisionCreatedDt = pd.CreatedDt
                                    ,PayDecisionCreatedBy = g.Key.CreatedBy
                                    //,PayDecisionModifiedDt = pd.ModifiedDt
                                    //,PayDecisionModifiedBy = pd.ModifiedBy
                                    //,PayDecisionisActive = pd.isActive
                                    ,
                                    COVIDPayTotalGross = g.Key.COVIDPayTotalGross 
                                }).ToList();
            
            queryRecords = queryRecords.Where(x => (fromDt == null || x.PayDecisionDt >= fromDt) && (toDt == null || x.PayDecisionDt <= toDt)).ToList();
            return queryRecords;
        }

        public static List<ExportViewModel> GetExportAllPayDate(HotLineLogEntities db, DateTime? fromDt = null, DateTime? toDt = null)
        {
            var queryRecords = (from pd in db.PayDecisions.Where(o => o.isActive == true)
                                join hl in db.HotlineLogs on pd.EmployeeID equals hl.EmployeeID
                                group pd by new { pd.EmployeeID, pd.PayDecisionStatus, pd.PayDecisionDt, pd.CreatedBy, pd.STDHoursPaidByNSHGross, pd.PTOAdjustment, pd.ReverseRegHours, pd.STDPayDates, pd.COVIDPayTotalGross, pd.STDPayHours, pd.Week1Cigna, pd.Week2Cigna, pd.Week1COVIDPayHours, pd.Week2COVIDPayHours }
                                    into g
                                select new ExportViewModel()
                                {
                                    EmployeeID = g.Key.EmployeeID
                                    ,
                                    PayDecisionStatus = g.Key.PayDecisionStatus
                                    ,
                                    STDPayHours = g.Key.STDPayHours
                                    ,
                                    Week1Cigna = (bool)(g.Key.Week1Cigna) ? "true" : "false"
                                    ,
                                    Week2Cigna = (bool)(g.Key.Week2Cigna) ? "true" : "false"
                                    ,
                                    Week1COVIDPayHours = g.Key.Week1COVIDPayHours
                                    ,
                                    Week2COVIDPayHours = g.Key.Week2COVIDPayHours
                                    ,
                                    PayDecisionDt = g.Key.PayDecisionDt
                                    ,
                                    STDHoursPaidByNSHGross = g.Key.STDHoursPaidByNSHGross
                                    ,
                                    PTOAdjustment = g.Key.PTOAdjustment
                                    ,
                                    ReverseRegHours = g.Key.ReverseRegHours
                                    ,
                                    STDPayDates = g.Key.STDPayDates
                                    ,
                                    PayDecisionCreatedBy = g.Key.CreatedBy

                                    ,
                                    COVIDPayTotalGross = g.Key.COVIDPayTotalGross
                                }).ToList();

            queryRecords = queryRecords.Where(x => (fromDt == null || x.PayDecisionDt >= fromDt) && (toDt == null || x.PayDecisionDt <= toDt)).ToList();
            return queryRecords;
        }

        public static List<ExportViewModel> GetExportPayDateDetail(HotLineLogEntities db, DateTime? fromDt = null, DateTime? toDt = null)
        {
            var queryRecords = (from pd in db.PayDecisions.Where(o => o.isActive == true && o.PayDecisionStatus.Contains("Yes"))
                                join hl in db.HotlineLogs on pd.EmployeeID equals hl.EmployeeID
                                group pd by new
                                {
                                    pd.EmployeeID,
                                    pd.PayDecisionStatus,
                                    pd.PayDecisionDt,
                                    pd.CreatedBy,
                                    pd.HoursInTimeCard,
                                    pd.STDPayHours,
                                    pd.STDHoursPaidByNSHGross,
                                    pd.PTOAdjustment,
                                    pd.ReverseRegHours,
                                    pd.STDPayDates,
                                    pd.COVIDBankPayGross,
                                    pd.COVIDPayTotalGross,
                                    pd.Week1Cigna,
                                    pd.Week1COVIDPayHours,
                                    pd.Week1TimeCardHours,
                                    pd.Week1COVIDPayGross,
                                    pd.Week2Cigna,
                                    pd.Week2COVIDPayHours,
                                    pd.Week2TimeCardHours,
                                    pd.Week2COVIDPayGross,
                                    pd.PTOHoursPaid,
                                    pd.PTODollarsPaid,
                                    pd.PTOHoursPaidWeek1,
                                    pd.PTODollarsPaidWeek1,
                                    pd.PTOHoursPaidWeek2,
                                    pd.PTODollarsPaidWeek2,
                                    pd.PTOPayHours,
                                    pd.PTOAddedByTimekeeper,
                                    pd.TotalPTOAdjustment
                                }
                                    into g
                                //where pd.isActive == true && pd.PayDecisionStatus.Contains("Yes") 
                                //&& hl.STDElection_2020 != "" && hl.HoursPreviouslyPaid != "0" && hl.EmployeeID != null
                                select new ExportViewModel()
                                {
                                    EmployeeID = g.Key.EmployeeID
                                    //,
                                    //EmployeeFirstName = g.Key.EmployeeFirstName
                                    //,
                                    //EmployeeLastName = g.Key.EmployeeLastName
                                    //,STDElection_2021 = hl.STDElection_2021
                                    //,RateOfPay = hl.RateOfPay
                                    //,FTEHours = hl.FTEHours
                                    ,
                                    PayDecisionStatus = g.Key.PayDecisionStatus
                                    //,STDPayHours = pd.STDPayHours
                                    ,
                                    PayDecisionDt = g.Key.PayDecisionDt
                                    ,
                                    HoursInTimeCard = g.Key.HoursInTimeCard
                                    ,
                                    STDPayHours = g.Key.STDPayHours
                                    ,
                                    STDHoursPaidByNSHGross = g.Key.STDHoursPaidByNSHGross
                                    ,
                                    PTOAdjustment = g.Key.PTOAdjustment
                                    ,
                                    ReverseRegHours = g.Key.ReverseRegHours
                                    ,
                                    STDPayDates = g.Key.STDPayDates
                                    //,PayDecisionCreatedDt = pd.CreatedDt
                                    ,
                                    PayDecisionCreatedBy = g.Key.CreatedBy
                                    //,PayDecisionModifiedDt = pd.ModifiedDt
                                    //,PayDecisionModifiedBy = pd.ModifiedBy
                                    //,PayDecisionisActive = pd.isActive
                                    ,
                                    COVIDPayTotalGross = g.Key.COVIDPayTotalGross
                                    ,
                                    COVIDBankPayGross = g.Key.COVIDBankPayGross
                                    ,
                                    Week1Cigna = g.Key.Week1Cigna.ToString()
                                    ,
                                    Week1TimeCardHours = g.Key.Week1TimeCardHours
                                    ,
                                    Week1COVIDPayHours = g.Key.Week1COVIDPayHours
                                    ,
                                    Week1COVIDPayGross = g.Key.Week1COVIDPayGross
                                    ,
                                    Week2Cigna = g.Key.Week2Cigna.ToString()
                                    ,
                                    Week2TimeCardHours = g.Key.Week2TimeCardHours
                                    ,
                                    Week2COVIDPayHours = g.Key.Week2COVIDPayHours
                                    ,
                                    Week2COVIDPayGross = g.Key.Week2COVIDPayGross
                                    ,
                                    PTOHoursPaid = g.Key.PTOHoursPaid
                                    ,
                                    PTODollarsPaid = g.Key.PTODollarsPaid
                                    ,
                                    Week1PTODollarsPaid = g.Key.PTODollarsPaidWeek1
                                    ,
                                    Week1PTOHoursPaid = g.Key.PTOHoursPaidWeek1
                                    ,
                                    Week2PTODollarsPaid = g.Key.PTODollarsPaidWeek2
                                    ,
                                    Week2PTOHoursPaid = g.Key.PTOHoursPaidWeek2
                                    ,
                                    PTOPayHours = g.Key.PTOPayHours
                                    ,
                                    PTOAddedByTimekeeper = g.Key.PTOAddedByTimekeeper
                                    ,
                                    TotalPTOAdjustment = g.Key.TotalPTOAdjustment
                                }).ToList();

            queryRecords = queryRecords.Where(x => (fromDt == null || x.PayDecisionDt >= fromDt) && (toDt == null || x.PayDecisionDt <= toDt)).ToList();
            return queryRecords;
        }


        public static List<ExportViewModel> GetCallLogWithPayStatusAndOverallStatus(HotLineLogEntities db, DateTime? fromDt = null, DateTime? toDt = null)
        {
            var queryRecords = (from hl in db.HotlineLogs
                                .Where(e => e.EmployeeID != null)
                                .GroupBy(e => e.EmployeeID)
                                .Select(e => e.OrderByDescending(y => y.DateOfCall).FirstOrDefault())
                                select new ExportViewModel()
                                {
                                    EmployeeID = hl.EmployeeID,
                                    STDElection_ThisYear = hl.STDElection_ThisYear,
                                    HoursPreviouslyPaid = hl.HoursPreviouslyPaid,
                                    FTEHours = hl.FTEHours,
                                    DiffHoursToFTE = hl.DiffHoursToFTE,
                                    RecordStatus = hl.RecordStatus
                                }).ToList();
           // queryRecords = queryRecords.Where(x => (fromDt == null || x.DateOfCall >= fromDt) && (toDt == null || x.DateOfCall <= toDt)).ToList();
            return queryRecords;
        }

        public static List<ExportViewModel> GetCallLogWithPayReviewStatusNew(HotLineLogEntities db, DateTime? fromDt = null, DateTime? toDt = null)
        {
            var queryRecords = (from hl in db.HotlineLogs
                                .Where(e => e.EmployeeID != null && e.PayReviewStatus=="New")
                                .GroupBy(e => e.EmployeeID)
                                .Select(e => e.OrderByDescending(y => y.DateOfCall).FirstOrDefault())
                                select new ExportViewModel()
                                {
                                    EmployeeID = hl.EmployeeID,
                                    EmployeeFirstName = hl.EmployeeFirstName,
                                    EmployeeLastName = hl.EmployeeLastName,
                                    PayReviewStatus =hl.PayReviewStatus,
                                    DateOfCall = hl.DateOfCall
                                }).ToList();
             queryRecords = queryRecords.Where(x => (fromDt == null || x.DateOfCall >= fromDt) && (toDt == null || x.DateOfCall <= toDt)).ToList();
            return queryRecords;

        }

        public static List<ExportViewModel> GetFrequentFlyers(HotLineLogEntities db)
        {
            var queryRecords = db.PayDecisions
                                .Where(e => e.EmployeeID != null && e.PayDecisionStatus.ToUpper().StartsWith("YES") && e.isActive)
                                .GroupBy(e => e.EmployeeID)
                                .Select(g => new ExportViewModel { EmployeeID = g.Key, countPD = g.Count(), latestPD = g.Max(x => x.PayDecisionDt), oldestPD = g.Min(x => x.PayDecisionDt) });
            queryRecords = queryRecords.Where(x => x.countPD > 3);
            return queryRecords.ToList();
        }
    }
}