using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Net;
using System.DirectoryServices.AccountManagement;
using CovidPayTracking.Helper;
using CovidPayTracking.Models;
using CovidPayTracking.Models.Mapper;
using DataTables.Mvc;
using System.Linq.Dynamic;
using ClosedXML.Excel;
using static CovidPayTracking.Helper.DateTimeExtensions;


namespace CovidPayTracking.Controllers
{
    public class ReportController : Controller
    {
        private HotLineLogEntities db = new HotLineLogEntities();

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        // GET: Report
        public ActionResult Index()
        {
            var vm = new IndexViewModel();

            return View();
        }
       
        #region Export Master Report
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportReport")]
        public ActionResult ExportReport (DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[51]
                { new DataColumn("EmployeeID")
                , new DataColumn("EmployeeFirstName")
                , new DataColumn("EmployeeLastName")
                , new DataColumn("DateOfCall")
                , new DataColumn("Operator")
                , new DataColumn("FTEStatus")
                , new DataColumn("ClearedToWork")
                ,new DataColumn("HotlineLogNotes")
                , new DataColumn("LastDayWorked")
                , new DataColumn("ReturnToWorkDt")
                , new DataColumn("PreviousYearSTDElection")
                 , new DataColumn("ThisYearSTDElection")
                , new DataColumn("RateOfPay")
                , new DataColumn("HoursPreviouslyPaid")
                , new DataColumn("FTEHours")
                , new DataColumn("DiffHoursToFTE")
                , new DataColumn("NotesFromBenefits")
                , new DataColumn("PayDecisionDt")
                , new DataColumn("PayDecisionStatus")
                , new DataColumn("ClockedIn_ClearedToWork_Dt")
                , new DataColumn("STDPayHours")
                , new DataColumn("PTOAdjustment")
                , new DataColumn("ReverseRegHours")
                , new DataColumn("PayDecisionNotes")
                , new DataColumn("SentToCigna")
                , new DataColumn("STDPayDates")
                , new DataColumn("PTOHoursPaid")
                , new DataColumn("PTODollarsPaid")
                , new DataColumn("STDHoursPaidByNSHGross (Prior to 10/1/21)")
                , new DataColumn("COVIDBankPayGross (After to 10/1/21)")
                , new DataColumn("COVIDPayTotalGross (After to 10/1/21)")
                , new DataColumn("Week1CignaPayingSTD?")
                , new DataColumn("Week1TimeCardHours")
                , new DataColumn("Week1COVIDPayHours")
                , new DataColumn("Week1COVIDPayGross")
                , new DataColumn("Week1PTOHoursPaid")
                , new DataColumn("Week1PTODollarsPaid")
                , new DataColumn("Week2CignaPayingSTD?")
                , new DataColumn("Week2TimeCardHours")
                , new DataColumn("Week2COVIDPayHours")
                , new DataColumn("Week2COVIDPayGross")
                , new DataColumn("Week2PTOHoursPaid")
                , new DataColumn("Week2PTODollarsPaid")
                , new DataColumn("PTOPayHours")
                , new DataColumn("PTOAddedByTimekeeper")
                , new DataColumn("TotalPTOAdjustment")
                , new DataColumn("PayDecisionCreatedDt")
                , new DataColumn("PayDecisionCreatedBy")
                , new DataColumn("PayDecisionModifiedDt")
                , new DataColumn("PayDecisionModifiedBy")
                 , new DataColumn("HotlineLogCreatedDt")
                });

            var queryRecords = Helper.ExportReport.GetExports(db, SearchFromDate, SearchToDate);
            
            foreach (var qR in queryRecords)
            {
                dt.Rows.Add(
                    qR.EmployeeID
                    , qR.EmployeeFirstName
                    , qR.EmployeeLastName
                    , qR.DateOfCall?.ToShortDateString()
                    , qR.Operator
                    , qR.FTEStatus
                    , qR.ClearedToWork
                    , qR.HotlineLogNotes
                    , qR.LastDayWorked?.ToShortDateString()
                    , qR.ReturnToWorkDt?.ToShortDateString()
                    , qR.STDElection_PrevYear
                    , qR.STDElection_ThisYear
                    , qR.RateOfPay
                    , qR.HoursPreviouslyPaid
                    , qR.FTEHours
                    , qR.DiffHoursToFTE
                    , qR.NotesFromBenefits
                    , qR.PayDecisionDt?.ToShortDateString()
                    , qR.PayDecisionStatus
                    , qR.ClockedIn_ClearedToWork_Dt
                    , qR.STDPayHours
                    , qR.PTOAdjustment
                    , qR.ReverseRegHours
                    , qR.PayDecisionNotes
                    , qR.SentToCigna
                    , qR.STDPayDates
                    , qR.PTOHoursPaid
                    , qR.PTODollarsPaid
                    , qR.STDHoursPaidByNSHGross
                    , qR.COVIDBankPayGross
                    , qR.COVIDPayTotalGross
                    , qR.Week1Cigna
                    , qR.Week1TimeCardHours
                    , qR.Week1COVIDPayHours
                    , qR.Week1COVIDPayGross
                    , qR.Week1PTOHoursPaid
                    , qR.Week1PTODollarsPaid
                    , qR.Week2Cigna
                    , qR.Week2TimeCardHours
                    , qR.Week2COVIDPayHours
                    , qR.Week2COVIDPayGross
                    , qR.Week2PTOHoursPaid
                    , qR.Week2PTODollarsPaid
                    , qR.PTOPayHours
                    , qR.PTOAddedByTimekeeper
                    , qR.TotalPTOAdjustment
                    , qR.PayDecisionCreatedDt?.ToShortDateString()
                    , qR.PayDecisionCreatedBy
                    , qR.PayDecisionModifiedDt?.ToShortDateString()
                    , qR.PayDecisionModifiedBy
                    , qR.HotlineLogCreatedDt?.ToShortDateString()
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"Report_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion


        #region Export Report by Pay Date
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportReportByPayDate")]
        public ActionResult ExportReportByPayDate(DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9]
                {
                new DataColumn("PayDecisionDate")
                ,new DataColumn("EmployeeID")
                , new DataColumn("STDPayDates")
                , new DataColumn("PTOAdjustment")
                , new DataColumn("ReverseRegHours")
                , new DataColumn("COVIDPayTotalGross (After 10/1/21)")
                , new DataColumn("COVIDPayHours")
                , new DataColumn("PayDecisionStatus")
                //, new DataColumn("PayDecisionCreatedDt")
                , new DataColumn("PayDecisionCreatedBy")
                //, new DataColumn("PayDecisionModifiedDt")
                //, new DataColumn("PayDecisionModifiedBy")
                });

            var queryRecords = Helper.ExportReport.GetExportPayDate(db, SearchFromDate, SearchToDate);

            foreach (var qR in queryRecords)
            {
                decimal totalCOVIDPayHours = qR.STDPayHours.ValidateToDecimal();

                if (qR.Week1Cigna == "false")
                {
                    totalCOVIDPayHours += qR.Week1COVIDPayHours.ValidateToDecimal();
                }
                if (qR.Week2Cigna == "false")
                {
                    totalCOVIDPayHours += qR.Week2COVIDPayHours.ValidateToDecimal();
                }
                dt.Rows.Add(
                    qR.PayDecisionDt?.ToShortDateString(),
                    qR.EmployeeID
                    , qR.STDPayDates
                    , qR.PTOAdjustment
                    , qR.ReverseRegHours
                    , qR.COVIDPayTotalGross
                    , totalCOVIDPayHours.ToString()
                    , qR.PayDecisionStatus
                    // , qR.PayDecisionCreatedDt?.ToShortDateString()
                    , qR.PayDecisionCreatedBy
                    //, qR.PayDecisionModifiedDt?.ToShortDateString()
                    //, qR.PayDecisionModifiedBy
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"Report_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region Export All Pay Decision Report by Pay Date
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportAllPayDecisionReport")]
        public ActionResult ExportAllPayDecisionReport(DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9]
                {
                new DataColumn("PayDecisionDate")
                ,new DataColumn("EmployeeID")
                , new DataColumn("STDPayDates")
                , new DataColumn("PTOAdjustment")
                , new DataColumn("ReverseRegHours")
                , new DataColumn("COVIDPayTotalGross (After 10/1/21)")
                , new DataColumn("COVIDPayHours")
                , new DataColumn("PayDecisionStatus")
                //, new DataColumn("PayDecisionCreatedDt")
                , new DataColumn("PayDecisionCreatedBy")
                //, new DataColumn("PayDecisionModifiedDt")
                //, new DataColumn("PayDecisionModifiedBy")
                });

            var queryRecords = Helper.ExportReport.GetExportAllPayDate(db, SearchFromDate, SearchToDate);

            foreach (var qR in queryRecords)
            {
                decimal totalCOVIDPayHours = qR.STDPayHours.ValidateToDecimal();

                if (qR.Week1Cigna == "false")
                {
                    totalCOVIDPayHours += qR.Week1COVIDPayHours.ValidateToDecimal();
                }
                if (qR.Week2Cigna == "false")
                {
                    totalCOVIDPayHours += qR.Week2COVIDPayHours.ValidateToDecimal();
                }
                dt.Rows.Add(
                    qR.PayDecisionDt?.ToShortDateString(),
                    qR.EmployeeID
                    , qR.STDPayDates
                    , qR.PTOAdjustment
                    , qR.ReverseRegHours
                    , qR.COVIDPayTotalGross
                    , totalCOVIDPayHours.ToString()
                    , qR.PayDecisionStatus
                    // , qR.PayDecisionCreatedDt?.ToShortDateString()
                    , qR.PayDecisionCreatedBy
               //, qR.PayDecisionModifiedDt?.ToShortDateString()
               //, qR.PayDecisionModifiedBy
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"Report_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region Export Detailed Report by Pay Date
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportReportByPayDateDetail")]
        public ActionResult ExportReportByPayDateDetail(DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[29]
                {
                 new DataColumn("PayDecisionDate")
                ,new DataColumn("EmployeeID")
                , new DataColumn("STDPayDates")
                , new DataColumn("PTOAdjustment")
                , new DataColumn("ReverseRegHours")
                , new DataColumn("STDHoursPaidByNSHGross (Prior to 10/1/21)")
                , new DataColumn("COVIDPayTotalGross (After 10/1/21)")
                , new DataColumn("HoursInTimeCard")
                , new DataColumn("STDPayHours")
                , new DataColumn("PTOHoursPaid")
                , new DataColumn("PTODollarsPaid")
                , new DataColumn("COVIDBankPayGross (After to 10/1/21)")
                , new DataColumn("Week1CignaPayingSTD?")
                , new DataColumn("Week1TimeCardHours")
                , new DataColumn("Week1COVIDPayHours")
                , new DataColumn("Week1COVIDPayGross")
                , new DataColumn("Week1PTOHoursPaid")
                , new DataColumn("Week1PTODollarsPaid")
                , new DataColumn("Week2CignaPayingSTD?")
                , new DataColumn("Week2TimeCardHours")
                , new DataColumn("Week2COVIDPayHours")
                , new DataColumn("Week2COVIDPayGross")
                , new DataColumn("Week2PTOHoursPaid")
                , new DataColumn("Week2PTODollarsPaid")
                , new DataColumn("PTOPayHours")
                , new DataColumn("PTOAddedByTimekeeper")
                , new DataColumn("TotalPTOAdjustment")
                , new DataColumn("PayDecisionStatus")
                //, new DataColumn("PayDecisionCreatedDt")
                , new DataColumn("PayDecisionCreatedBy")
                //, new DataColumn("PayDecisionModifiedDt")
                //, new DataColumn("PayDecisionModifiedBy")
                });

            var queryRecords = Helper.ExportReport.GetExportPayDateDetail(db, SearchFromDate, SearchToDate);

            foreach (var qR in queryRecords)
            {
                dt.Rows.Add(
                    qR.PayDecisionDt?.ToShortDateString()
                    , qR.EmployeeID
                    , qR.STDPayDates
                    , qR.PTOAdjustment
                    , qR.ReverseRegHours
                    , qR.STDHoursPaidByNSHGross
                    , qR.COVIDPayTotalGross
                    , qR.HoursInTimeCard
                    , qR.STDPayHours
                    , qR.PTOHoursPaid
                    , qR.PTODollarsPaid
                    , qR.COVIDBankPayGross
                    , qR.Week1Cigna
                    , qR.Week1TimeCardHours
                    , qR.Week1COVIDPayHours
                    , qR.Week1COVIDPayGross
                    , qR.Week1PTOHoursPaid
                    , qR.Week1PTODollarsPaid
                    , qR.Week2Cigna
                    , qR.Week2TimeCardHours
                    , qR.Week2COVIDPayHours
                    , qR.Week2COVIDPayGross
                    , qR.Week2PTOHoursPaid
                    , qR.Week2PTODollarsPaid
                    , qR.PTOPayHours
                    , qR.PTOAddedByTimekeeper
                    , qR.TotalPTOAdjustment
                    , qR.PayDecisionStatus
                    // , qR.PayDecisionCreatedDt?.ToShortDateString()
                    , qR.PayDecisionCreatedBy
               //, qR.PayDecisionModifiedDt?.ToShortDateString()
               //, qR.PayDecisionModifiedBy
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"Report_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region Export Hotline Log Report With Pay and Overall Status
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportReportWithPayAndOverallStatus")]
        public ActionResult ExportReportWithPayAndOverallStatus(DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6]
                { new DataColumn("EmployeeID")
                , new DataColumn("ThisYearSTDElection")
                , new DataColumn("HoursPreviouslyPaid")
                , new DataColumn("FTEHours")
                , new DataColumn("DiffHoursToFTE")
                , new DataColumn("OverallStatus")
                });

            var queryRecords = Helper.ExportReport.GetCallLogWithPayStatusAndOverallStatus(db, SearchFromDate, SearchToDate);

            foreach (var qR in queryRecords)
            {
                dt.Rows.Add(
                    qR.EmployeeID
                    , qR.STDElection_ThisYear
                    , qR.HoursPreviouslyPaid
                    , qR.FTEHours
                    , qR.DiffHoursToFTE
                    , qR.RecordStatus
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"Report_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region Export List of Employees with "New" Pay Status Review
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportReportPayReviewStatusNew")]
        public ActionResult ExportReportPayReviewStatusNew(DateTime? SearchFromDate = null, DateTime? SearchToDate = null)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5]
                { new DataColumn("EmployeeID")
                , new DataColumn("EmployeeFirstName")
                , new DataColumn("EmployeeLastName")
                , new DataColumn("DateOfCall")
                , new DataColumn("PayReviewStatus")
                });

            var queryRecords = Helper.ExportReport.GetCallLogWithPayReviewStatusNew(db, SearchFromDate, SearchToDate);

            foreach (var qR in queryRecords)
            {
                dt.Rows.Add(
                    qR.EmployeeID
                    , qR.EmployeeFirstName
                    , qR.EmployeeLastName
                    , qR.DateOfCall?.ToShortDateString()
                    , qR.PayReviewStatus
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"ReportPayReviewStatusNew_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion



        #region Employee STD Election and Merit
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public ActionResult GetEmployeeMeritSTDElection()
        {
            var vm = new EmployeeMeritSTDElectionViewModel();
            return View();
        }

        #region Employee Get STD Election and Merit
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        public ActionResult GetEmployeeSTDElectionPayData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dataTablesRequest)
        {
            var qdata = (from a in db.EmployeeSTDElections
                         join b in db.EmployeeMerits on a.EmployeeID equals b.EmployeeID
                         select new EmployeeMeritSTDElectionViewModel
                         {
                             EmployeeID = a.EmployeeID,
                             FirstName = b.EmployeeFirstName,
                             LastName = b.EmployeeLastName,
                             TotalSTDElectionPrevYear = a.TotalSTDElectionPrevYear,
                             TotalSTDElectionThisYear = a.TotalSTDElectionThisYear,
                             HourlyPayRate = b.HourlyPayRate,
                             FTEStatus = b.CombinedFTEStatus
                         });


            var totalRecords = qdata.Count();

            #region Filtering
            var filterSearchString = string.Empty;
            foreach (var col in dataTablesRequest.Columns.GetFilteredColumns())
            {
                switch (col.Data)
                {
                    case "EmployeeID":
                        qdata = qdata.Where(x => x.EmployeeID.Contains(col.Search.Value));
                        filterSearchString = col.Search.Value;
                        break;
                }
            }

            if (dataTablesRequest.Search.Value != string.Empty)
            {
                var value = dataTablesRequest.Search.Value.Trim();
                qdata = qdata.Where(x => x.EmployeeID.Contains(value)
                                        //|| x.EmployeeDepartment.Contains(value)
                                        //|| x.EmployeeCategory.Contains(value)
                                        //|| x.RequestStatus.Contains(value)
                                        //|| x.EmployeeTitle.Contains(value)
                                        );
            }

            //var filteredCount = query.Count();
            #endregion Filtering

            var data = qdata.ToList();
            var filteredCount = data.Count();

            #region Sorting
            var sortedColumns = dataTablesRequest.Columns.GetSortedColumns();
            var orderByString = string.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != string.Empty ? "," : "";
                orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
            }
            if (orderByString == string.Empty)
                orderByString = "EmployeeID asc";

            data = data.OrderBy(orderByString).ToList();
            #endregion Sorting

            data = data.Skip(dataTablesRequest.Start).Take(dataTablesRequest.Length).ToList();
            return Json(new DataTablesResponse(dataTablesRequest.Draw, data, filteredCount, totalRecords),
                JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Export STD Election and Pay Report
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportSTDElectionPay")]
        public ActionResult ExportSTDElectionPay()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7]
                { new DataColumn("EmployeeID")
                , new DataColumn("EmployeeFirstName")
                , new DataColumn("EmployeeLastName")
                , new DataColumn("PreviousYearSTDElection")
                , new DataColumn("ThisYearSTDElection")
                , new DataColumn("HourlyPayRate")
                ,new DataColumn("FTEStatus")
                });

            var queryRecords = (from a in db.EmployeeSTDElections
                                join b in db.EmployeeMerits on a.EmployeeID equals b.EmployeeID
                                select new EmployeeMeritSTDElectionViewModel
                                {
                                    EmployeeID = a.EmployeeID,
                                    FirstName = b.EmployeeFirstName,
                                    LastName = b.EmployeeLastName,
                                    HourlyPayRate = b.HourlyPayRate,
                                    TotalSTDElectionPrevYear = a.TotalSTDElectionPrevYear,
                                    TotalSTDElectionThisYear = a.TotalSTDElectionThisYear,
                                    FTEStatus = b.CombinedFTEStatus
                                });

            foreach (var qR in queryRecords)
            {
                dt.Rows.Add(
                    qR.EmployeeID
                    , qR.FirstName
                    , qR.LastName
                    , qR.TotalSTDElectionPrevYear
                    , qR.TotalSTDElectionThisYear
                    , qR.HourlyPayRate
                    , qR.FTEStatus
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"Report_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion
        #endregion

        #region Export Frequent Flyer Report
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ExportFrequents")]
        public ActionResult ExportFrequentFlyer()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4]
                { new DataColumn("EmployeeID")
                , new DataColumn("Num 'Yes' Pay Decisions")
                , new DataColumn("Latest Pay Decision")
                , new DataColumn("Oldest Pay Decision")
                });

            var queryRecords = Helper.ExportReport.GetFrequentFlyers(db);

            foreach (var qR in queryRecords)
            {
                dt.Rows.Add(
                    qR.EmployeeID
                    , qR.countPD
                    , qR.latestPD
                    , qR.oldestPD
               );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"ReportFrequent_{DateTime.Now.ToString()}.xls";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion
    }

}