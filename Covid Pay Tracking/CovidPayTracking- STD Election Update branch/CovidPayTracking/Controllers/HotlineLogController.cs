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
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects;

namespace CovidPayTracking.Controllers
{
    public class HotlineLogController : Controller
    {
        private HotLineLogEntities db = new HotLineLogEntities();

        #region Home
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public ActionResult Home()
        {
            return View();
        }
        #endregion


        #region Get Employees Called 
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public ActionResult EmployeesCalledIndex()
        {
            var hotlineLogVM = new HotLineLogViewModel()
            {
                SelectedFromDate = DateTime.Now.AddMonths(-1),
                SelectedToDate = DateTime.Now
            };
            return View(hotlineLogVM);

        }

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        public ActionResult GetDistinctEmployeesCalled([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dataTablesRequest)
        {
            try
            {
                //IQueryable<HotlineLog> query = db.HotlineLogs;
                var fromDt = Request["fromDt"];
            var toDt = Request["toDt"];
            DateTime? qFromDt = null;
            DateTime? qToDt = null;
            var fromEmplID = Request["fromEmplID"];
            var toEmplID = Request["toEmplID"];
            int? qFromtEmplID = 0;
            int? qToEmplID = 0;

            if (!string.IsNullOrEmpty(fromDt))
            {
                qFromDt = DateTime.Parse(fromDt);
            }
            if (!string.IsNullOrEmpty(toDt))
            {
                qToDt = DateTime.Parse(toDt);
            }

            qFromtEmplID = fromEmplID.ConvertStringToInt();

            qToEmplID = toEmplID.ConvertStringToInt();

            //query = query.Where(x => (qFromDt == null || x.DateOfCall >= qFromDt) && (qToDt == null || x.DateOfCall <= qToDt) && x.EmployeeID != null);
            
            var query = (from hl in db.HotlineLogs
                                .Where(x => (qFromDt == null || x.DateOfCall >= qFromDt) && (qToDt == null || x.DateOfCall <= qToDt) 
                                        && x.EmployeeID != null)
                                        ///Commented out following line as EmpID 212769 was not showing up on the list because STDElection_2020 was empty (not null)
                                        //&& x.STDElection_2020 !="" && x.STDElection_2020 !=null
                                .GroupBy(x => new { x.EmployeeID, x.EmployeeFirstName, x.EmployeeLastName, x.FTEStatus, x.FTEHours, x.STDElection_PrevYear, x.STDElection_ThisYear, x.PayReviewStatus })
                             join pdj in db.PayDecisions.Where(x => x.isActive == true && x.STDPayHours != null && x.STDPayHours != "" &&((x.STDHoursPaidByNSHGross != null && x.STDHoursPaidByNSHGross != "") || (x.COVIDPayTotalGross != null && x.COVIDPayTotalGross != ""))) on hl.Key.EmployeeID equals pdj.EmployeeID into sr
                             from pd in sr.DefaultIfEmpty()
                         select new
                         {
                             EmployeeID = hl.Key.EmployeeID,
                             EmployeeFirstName = hl.Key.EmployeeFirstName,
                             EmployeeLastName = hl.Key.EmployeeLastName,
                             FTEStatus = hl.Key.FTEStatus,
                             STDElection_PrevYear = hl.Key.STDElection_PrevYear,
                             STDElection_ThisYear = hl.Key.STDElection_ThisYear,
                             FTEHours = hl.Key.FTEHours,
                             STDPayHours = pd.STDPayHours,
                             STDHoursPaidByNSHGross = pd.STDHoursPaidByNSHGross,
                             PayReviewStatus = hl.Key.PayReviewStatus,
                             COVIDPayTotalGross = pd.COVIDPayTotalGross
                         }
                );

             var totalRecords = query.Count();
            #region Filtering
            var filterSearchString = string.Empty;
            foreach (var col in dataTablesRequest.Columns.GetFilteredColumns())
            {
                switch (col.Data)
                {
                    case "EmployeeID":
                        query = query.Where(x => x.EmployeeID.Contains(col.Search.Value));
                        filterSearchString = col.Search.Value;
                        break;
                     case "PayReviewStatus":
                            query = query.Where(x => x.PayReviewStatus.Contains(col.Search.Value));
                            filterSearchString = col.Search.Value;
                            break;
                    }
            }
            if (dataTablesRequest.Search.Value != string.Empty)
            {
                var value = dataTablesRequest.Search.Value.Trim();
                query = query.Where(x => x.EmployeeID.Contains(value)
                                        || x.PayReviewStatus.Contains(value)
                                        );
            }

                #endregion Filtering

                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^\d$");

                var result = query.AsEnumerable()
                .Where(x => (qFromtEmplID == null || x.EmployeeID.ValidateEmployeeID() >= qFromtEmplID) && (qToEmplID == null || x.EmployeeID.ValidateEmployeeID() <= qToEmplID))
                //.GroupBy(i => new { i.EmployeeID})
                .GroupBy(i => new { i.EmployeeID, i.EmployeeFirstName, i.EmployeeLastName, i.FTEStatus, i.STDElection_PrevYear, i.STDElection_ThisYear, i.FTEHours, i.PayReviewStatus })
                .Select(item =>
                    new HotLineLogViewModel
                    {
                        EmployeeID = item.Key.EmployeeID,
                        EmployeeFirstName = item.Key.EmployeeFirstName,
                        EmployeeLastName = item.Key.EmployeeLastName,
                        FTEStatus = item.Key.FTEStatus,
                        STDElection_PrevYear = item.Key.STDElection_PrevYear,
                        STDElection_ThisYear = item.Key.STDElection_ThisYear,
                        FTEHours = item.Key.FTEHours,
                        TotalSTDPayHours = item.Sum(x => x.STDPayHours.ValidateToDecimal()).ToString(),
                        TotalSTDHoursPaidByNSHGross = item.Sum(x => x.STDHoursPaidByNSHGross.ValidateToDecimal()).ToString(),
                        PayReviewStatus = item.Key.PayReviewStatus,
                        COVIDPayTotalGross = item.Sum(x => x.COVIDPayTotalGross.ValidateToDecimal()).ToString()
                    }
                );
                
                var filteredCount = result.Count();

            #region Sorting
            var sortedColumns = dataTablesRequest.Columns.GetSortedColumns();
            var orderByString = string.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != string.Empty ? "," : "";
                orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
            }
            if (orderByString == string.Empty)
                orderByString = "ID asc";
            result = result.OrderBy(orderByString).ToList();
                #endregion

                result = result.Skip(dataTablesRequest.Start).Take(dataTablesRequest.Length).ToList();
                return Json(new DataTablesResponse(dataTablesRequest.Draw, result, filteredCount, totalRecords), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveException(ex);
                return Json(new DataTablesResponse(dataTablesRequest.Draw, null, 0, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        
        
        #region Date Range Filter - Emplouees Called Index
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Search")]

        public ActionResult Search()
        {
            var vm = new HotLineLogViewModel();
            var fromDt = Request["SelectedFromDate"] ?? null;
            var toDt = Request["SelectedToDate"] ?? null;
            var fromEmplID = Request["EmplIDFrom"];
            var toEmplID = Request["EmplIDTo"];

            if (!string.IsNullOrEmpty(fromDt))
            {
                vm.SelectedFromDate = DateTime.Parse(fromDt);
            }
            if (!string.IsNullOrEmpty(toDt))
            {
                vm.SelectedToDate = DateTime.Parse(toDt);
            }

            if (!string.IsNullOrEmpty(fromEmplID))
            {
                vm.EmplIDFrom = fromEmplID;
            }
            if (!string.IsNullOrEmpty(fromEmplID))
            {
                vm.EmplIDTo = toEmplID;
            }

            return View("EmployeesCalledIndex", vm);
        }
        #endregion

        #region HotlineLog Index/ViewPayDecisions - NOT USED
        #region Index Page
        // GET: HotlineLog
        //[CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        //public ActionResult Index()
        //{
        //    var vm = new IndexViewModel();
        //    vm.SearchFromDate = DateTime.Now.AddDays (-60);
        //    vm.SearchToDate = DateTime.Now;

        //    return View(vm);
        //}
        #endregion

        #region Get HotLine Log Pay Decisions Each Employee Record
        //[CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        //public ActionResult ViewPayDecisions (int id)
        //{
        //    var payDecisionsDb =db.PayDecisions;

        //    var empID = (from r in payDecisionsDb
        //                 where r.HotlineLogID == id
        //                 select r.EmployeeID).FirstOrDefault();

        //    IQueryable<PayDecision> query = payDecisionsDb;

        //    query = query.Where(x => x.EmployeeID == empID);

        //    var totalRecords = query.Count();
        //    var filteredCount = totalRecords;

        //    var payDecision = query.ToList();
        //    var data = new List<PayDecisionViewModel>();
        //    foreach(var pd in payDecision)
        //    {
        //        data.Add(MapFromModel.MapPDViewModel(pd));
        //    }

        //   return View("ViewPayDecisions", data);

        //return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Date Range Filter - Report
        //[CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "Filter")]
        //public ActionResult Filter()
        //{
        //    var vm = new IndexViewModel();
        //    var fromDt = Request["SearchFromDate"] ?? null;
        //    var toDt = Request["SearchToDate"] ?? null;
        //    if (!string.IsNullOrEmpty(fromDt))
        //    {
        //        vm.SearchFromDate = DateTime.Parse(fromDt);
        //    }
        //    if (!string.IsNullOrEmpty(toDt))
        //    {
        //        vm.SearchToDate = DateTime.Parse(toDt);
        //    }

        //    return View("Index", vm);
        //}
        #endregion

        #region Get/Sort/Filter Data - Datatable (HotlineLog Records)
        //[CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        //[HttpPost]
        //public ActionResult Get([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest dataTablesRequest)
        //{
        //    IQueryable<HotlineLog> query = db.HotlineLogs;
        //    var fromDt = Request["fromDt"];
        //    var toDt = Request["toDt"];
        //    var viewAll = Request["viewAll"];
        //    DateTime? qFromDt = null;
        //    DateTime? qToDt = null;

        //    if (!string.IsNullOrEmpty(fromDt))
        //    {
        //        qFromDt = DateTime.Parse(fromDt);
        //    }
        //    if (!string.IsNullOrEmpty(toDt))
        //    {
        //        qToDt = DateTime.Parse(toDt);
        //    }
        //    query = query.Where(x => (qFromDt == null || x.DateOfCall >= qFromDt) && (qToDt == null || x.DateOfCall <= qToDt));
        //    var totalRecords = query.Count();

        //    #region Filtering
        //    var filterSearchString = string.Empty;
        //    foreach (var col in dataTablesRequest.Columns.GetFilteredColumns())
        //    {
        //        switch (col.Data)
        //        {
        //            //case "EmployeeDepartment":
        //            //    query = query.Where(x => x.EmployeeDepartment.Contains(col.Search.Value));
        //            //    filterSearchString = col.Search.Value;
        //            //    break;
        //            case "EmployeeID":
        //                query = query.Where(x => x.EmployeeID.Contains(col.Search.Value));
        //                filterSearchString = col.Search.Value;
        //                break;
        //            //case "EmployeeCategory":
        //            //    query = query.Where(x => x.EmployeeCategory.Contains(col.Search.Value));
        //            //    filterSearchString = col.Search.Value;
        //            //    break;
        //            //case "RequestStatus":
        //            //    query = query.Where(x => x.RequestStatus.Contains(col.Search.Value));
        //            //    filterSearchString = col.Search.Value;
        //            //    break;
        //            //case "EmployeeTitle":
        //            //    query = query.Where(x => x.EmployeeTitle.Contains(col.Search.Value));
        //            //    filterSearchString = col.Search.Value;
        //            //    break;
        //        }
        //    }

        //    if (dataTablesRequest.Search.Value != string.Empty)
        //    {
        //        var value = dataTablesRequest.Search.Value.Trim();
        //        query = query.Where(x => x.EmployeeID.Contains(value)
        //                                //|| x.EmployeeDepartment.Contains(value)
        //                                //|| x.EmployeeCategory.Contains(value)
        //                                //|| x.RequestStatus.Contains(value)
        //                                //|| x.EmployeeTitle.Contains(value)
        //                                );
        //    }

        //    var filteredCount = query.Count();
        //    #endregion Filtering

        //    var hllViewModels = new List<HotLineLogViewModel>();

        //    foreach (var a in query)
        //    {
        //        hllViewModels.Add(MapFromModel.MapHLLViewModel(a));
        //    }

        //    #region Sorting
        //    var sortedColumns = dataTablesRequest.Columns.GetSortedColumns();
        //    var orderByString = string.Empty;

        //    foreach (var column in sortedColumns)
        //    {
        //        orderByString += orderByString != string.Empty ? "," : "";
        //        orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
        //    }
        //    if (orderByString == string.Empty)
        //        orderByString = "EmployeeID asc";
        //    //query = query.OrderBy(orderByString);
        //    hllViewModels = hllViewModels.OrderBy(orderByString).ToList();
        //    #endregion Sorting

        //    hllViewModels = hllViewModels.Skip(dataTablesRequest.Start).Take(dataTablesRequest.Length).ToList();

        //    var data = hllViewModels.Select(dt => new
        //    {
        //        ID = dt.ID,
        //        EmployeeID = dt.EmployeeID,
        //        EmployeeFirstName = dt.EmployeeFirstName,
        //        EmployeeLastName = dt.EmployeeLastName,
        //        DateOfCall = dt.DateOfCall,
        //        Operator = dt.Operator,
        //        FTEStatus = dt.FTEStatus,
        //        ClearedToWork = dt.ClearedToWork,
        //        Notes = dt.Notes,
        //        LastDayWorked = dt.LastDayWorked,
        //        ReturnToWorkDt = dt.ReturnToWorkDt,
        //        STDElection_2020 = dt.STDElection_2020,
        //        STDElection_2021 = dt.STDElection_2021,
        //        RateOfPay = dt.RateOfPay,
        //        HoursPreviouslyPaid = dt.HoursPreviouslyPaid,
        //        PayPeriodPayDate = dt.PayPeriodPayDate,
        //        FTEHours = dt.FTEHours,
        //        DiffHoursToFTE = dt.DiffHoursToFTE,
        //        RawDataNo = dt.RawDataNo,
        //        NotesFromBenefits = dt.NotesFromBenefits,
        //        CreatedDt = dt.CreatedDt,
        //        CreatedBy = dt.CreatedBy,
        //        Modifiedby = dt.Modifiedby,
        //        ModifiedDt = dt.ModifiedDt

        //    }).ToList();

        //    return Json(new DataTablesResponse(dataTablesRequest.Draw, data, filteredCount, totalRecords),
        //        JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #endregion


    }
}