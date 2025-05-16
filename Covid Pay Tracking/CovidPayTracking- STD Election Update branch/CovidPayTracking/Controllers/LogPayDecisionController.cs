using CovidPayTracking.Helper;
using CovidPayTracking.Models;
using CovidPayTracking.Models.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web.Mvc;

namespace CovidPayTracking.Controllers
{
    public class LogPayDecisionController : Controller
    {
        private HotLineLogEntities db = new HotLineLogEntities();

        #region Display Log and Pay Decision History for an Employee
        // GET: LogPayDecision

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public ActionResult GetLogPayDecision(string id)
        {
            HotlineLogPayDecisionViewModel hlPD = new HotlineLogPayDecisionViewModel();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var empInfo = db.HotlineLogs.Where(x => x.EmployeeID == id && x.isActive == true).OrderByDescending(y => y.DateOfCall).FirstOrDefault();
                //var payDecisionDB = db.PayDecisions;

                hlPD.EmployeeID = empInfo.EmployeeID;
                hlPD.EmployeeFirstName = empInfo.EmployeeFirstName;
                hlPD.EmployeeLastName = empInfo.EmployeeLastName;
                hlPD.FTEStatus = empInfo.FTEStatus;
                hlPD.STDElection_PrevYear = empInfo.STDElection_PrevYear;
                hlPD.STDElection_ThisYear = empInfo.STDElection_ThisYear.ValidateToDecimal().ToString("p2");
                hlPD.STDElection_Detail = empInfo.STDElection_ThisYear;
                if (empInfo.STDElection_ThisYear != null && empInfo.STDElection_ThisYear.ToUpper() == "RESOURCE" && empInfo.FTEStatus.ValidateToDouble() > 0 && empInfo.FTEStatus.ValidateToDouble() < 0.50)
                {
                    hlPD.STDElection_ThisYear = "50.00%";
                }               
                hlPD.RateOfPay = empInfo.RateOfPay;
                hlPD.FTEHours = empInfo.FTEHours;
                hlPD.RecordStatus = empInfo.RecordStatus;
                hlPD.LastDayWorked = empInfo.LastDayWorked;
                hlPD.ReturnToWorkDt = empInfo.ReturnToWorkDt;
                hlPD.PayReviewStatus = empInfo.PayReviewStatus;
                // hlPD.DiffHoursToFTE = empInfo.DiffHoursToFTE;

                var empCase = db.CovidCases.Where(x => x.EmployeeID == hlPD.EmployeeID && x.EmployeeID != null && x.isActive == true).ToList();

                if (empCase.Count > 0)
                {
                    hlPD.MadeWhole = empCase.FirstOrDefault().MadeWhole;
                    hlPD.CaseNote = empCase.FirstOrDefault().CaseNote;
                }

                var allRecforpdEmp = db.PayDecisions.Where(x => x.EmployeeID == hlPD.EmployeeID && x.isActive == true && (x.STDPayHours != null && x.STDPayHours != "") && ((x.STDHoursPaidByNSHGross != null && x.STDHoursPaidByNSHGross != "") || (x.COVIDPayTotalGross != null && x.COVIDPayTotalGross != ""))).ToList();
                var sumofPayHoursPaid = allRecforpdEmp.Sum(x => x.STDPayHours.ValidateToDecimal());
                var sumofNSHGrossPaid = allRecforpdEmp.Sum(x => x.STDHoursPaidByNSHGross.ValidateToDecimal());
                var sumofCOVIDPayTotal = allRecforpdEmp.Sum(x => x.COVIDPayTotalGross.ValidateToDecimal());
                var sumofCOVIDBankPaid = allRecforpdEmp.Sum(x => x.COVIDBankPayGross.ValidateToDecimal());
                hlPD.TotalSTDPayHours = sumofPayHoursPaid.ToString();
                hlPD.TotalSTDHoursPaidByNSHGross = sumofNSHGrossPaid.ToString();
                hlPD.DiffHoursToFTE = CalculateHourPay.CalculateDiffHours(sumofPayHoursPaid.ToString(), empInfo.FTEHours);
                if (hlPD.DiffHoursToFTE.ValidateToDecimal() == 0m)
                {
                    hlPD.RecordStatus = "Max Paid";
                }
                else if (hlPD.DiffHoursToFTE.ValidateToDecimal() > 0m)
                {
                    hlPD.RecordStatus = "Bank Exhausted";
                }
                else
                {
                    hlPD.RecordStatus = "Open";
                }
                hlPD.COVIDBankPayGross = (sumofCOVIDBankPaid + sumofNSHGrossPaid).ToString();
                hlPD.COVIDPayTotalGross = (sumofCOVIDPayTotal + sumofNSHGrossPaid).ToString();
                hlPD.HotlineLogsByEmployee = GetHotLineLogModel(empInfo.EmployeeID);
                hlPD.PayDecisionsByEmployee = GetPayDecisionModel(empInfo.EmployeeID);
            }
            catch (Exception ex)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveException(ex);
                throw;
            }
            return View(hlPD);
        }
        #endregion

        #region Get Hotline Log List

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public List<HotLineLogViewModel> GetHotLineLogModel(string empID)
        {
            var hotLineLogDb = db.HotlineLogs;
            IQueryable<HotlineLog> queryLogs = hotLineLogDb;
            queryLogs = queryLogs.Where(x => x.EmployeeID == empID && x.isActive == true);
            var totalRecords = queryLogs.Count();
            var filteredCount = totalRecords;
            var logs = queryLogs.ToList();
            var qLogData = new List<HotLineLogViewModel>();
            foreach (var l in logs)
            {
                qLogData.Add(MapFromModel.MapHLLViewModel(l));
            }
            return qLogData;
        }
        #endregion

        #region Get Pay Decision List

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public List<PayDecisionViewModel> GetPayDecisionModel(string empID)
        {
            var payDecisionDB = db.PayDecisions;
            IQueryable<PayDecision> queryPays = payDecisionDB;
            queryPays = queryPays.Where(x => x.EmployeeID == empID && x.isActive == true);
            var totalRecords = queryPays.Count();
            var filteredCount = totalRecords;
            var pays = queryPays.ToList();
            var qPayData = new List<PayDecisionViewModel>();
            foreach (var pd in pays)
            {
                qPayData.Add(MapFromModel.MapPDViewModel(pd));
            }
            return qPayData;
        }
        #endregion

        #region Create New Pay Decision

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public PartialViewResult CreateNew(string empID, string fteHour, string rateofPay, string stdElection, string fteStatus, DateTime? lastDayWorked, DateTime? returnToWorkDt, string totalSTDPayHours)
        {
            try
            {
                var payDecisionDates = db.PayDecisionDates.OrderByDescending(x => x.PayDate).Select(c => c.PayDate).ToList(); 
                PayDecisionViewModel pdVM = new PayDecisionViewModel()
                {
                    EmployeeID = empID,
                    FTEHours = fteHour,
                    RateOfPay = rateofPay,
                    STDElection_ThisYear = stdElection,
                    FTEStatus = fteStatus,
                    TotalSTDPayHours = totalSTDPayHours,
                    LastDayWorked = lastDayWorked,
                    ReturnToWorkDt = returnToWorkDt,
                    PayDates = payDecisionDates.Select(c => new SelectListItem
                    {
                        Value = c.ToString("MM/dd/yyyy"),
                        Text = c.ToString("MM/dd/yyyy")
                    })
                };
                return PartialView("_createNewModal", pdVM);
            }
            catch (Exception ex)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveException(ex);
                throw;
            }

        }

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNew([Bind(Include = "EmployeeID,PayDecisionStatus,ClockedIn_ClearedToWork_Dt,STDPayDtTo,STDPayDtFrom,STDPayHours,PayDecisionDt,STDHoursPaidByNSHGross," +
            "PTOAdjustment,ReverseRegHours,Notes,STDPayDates,HotLineLogPayDecisionTempID,isActive,ReturnToWork,HoursInTimeCard,MadeWhole,COVIDBankPayGross,Week1Cigna,Week1TimeCardHours," +
            "Week1COVIDPayHours,Week1COVIDPayGross,Week2Cigna,Week2TimeCardHours,Week2COVIDPayHours,Week2COVIDPayGross,COVIDPayTotalGross,PTOHoursPaid," +
            "PTODollarsPaid,PTOHoursPaidWeek1,PTODollarsPaidWeek1,PTOHoursPaidWeek2,PTODollarsPaidWeek2,PTOPayHours,PTOAddedByTimekeeper,TotalPTOAdjustment")] PayDecisionViewModel payDecisionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // throw new Exception("Error Force");

                    if (payDecisionViewModel.EmployeeID != null && payDecisionViewModel.EmployeeID.Length == 6)
                    {
                        var payDecision = new PayDecision();
                        payDecisionViewModel.CreatedBy = User.Identity.Name.ToString().Split('\\').Last();
                        payDecisionViewModel.CreatedDt = System.DateTime.Now;
                        payDecisionViewModel.isActive = true;
                        payDecisionViewModel.PayDecisionMapToModel(payDecision);

                        db.PayDecisions.Add(payDecision);
                        db.SaveChanges();

                        var pdEmp = payDecisionViewModel.EmployeeID;
                        //var allRecforpdEmp = db.PayDecisions.Where(x => x.EmployeeID == pdEmp && x.isActive == true && (x.STDPayDates != null && x.STDPayDates != "" && x.STDPayDates != "N/A")).ToList();

                        var allRecforpdEmp = db.PayDecisions.Where(x => x.EmployeeID == pdEmp && x.isActive == true && (x.STDPayHours != null && x.STDPayHours != "") && ((x.STDHoursPaidByNSHGross != null && x.STDHoursPaidByNSHGross != "") || (x.COVIDPayTotalGross != null && x.COVIDPayTotalGross != ""))).ToList();
                        var sumofPayHours = allRecforpdEmp.Sum(x => x.STDPayHours.ValidateToDecimal());
                        var hLRecord = db.HotlineLogs.Where(x => x.EmployeeID == payDecisionViewModel.EmployeeID).ToList();
                        //var oldHours = hLRecord.FirstOrDefault().HoursPreviouslyPaid;
                        var oldHours = hLRecord.OrderByDescending(y => y.DateOfCall).FirstOrDefault().HoursPreviouslyPaid;
                        var newFTEHours = hLRecord.OrderByDescending(y => y.DateOfCall).FirstOrDefault().FTEHours;
                        var newHours = sumofPayHours.ToString();


                        foreach (var item in hLRecord)
                        {
                            db.Entry(item).State = EntityState.Modified;
                            item.Modifiedby = User.Identity.Name.ToString().Split('\\').Last();
                            item.ModifiedDt = System.DateTime.Now;
                            item.PayReviewStatus = "Completed";
                            item.FTEHours = newFTEHours.ToString();
                            if (newHours != oldHours)
                            {
                                item.HoursPreviouslyPaid = newHours;
                            }
                            item.DiffHoursToFTE = CalculateHourPay.CalculateDiffHours(newHours, item.FTEHours);
                            if (item.DiffHoursToFTE.ValidateToDecimal() == 0m)
                            {
                                item.RecordStatus = "Max Paid";
                            }
                            else if (item.DiffHoursToFTE.ValidateToDecimal() > 0m)
                            {
                                item.RecordStatus = "Bank Exhausted";
                            }
                            else
                            {
                                item.RecordStatus = "Open";
                            }
                            db.SaveChanges();
                        }

                        return Json(new { success = true, message = "New Record Added Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var applog = new ApplicationExceptionLog();
                        applog.SaveEmployeeIDException($"Employee ID is null. LogPayDecisionController -- ActionResult CreateNew([Bind(Include = 'EmployeeID....')]");
                        return Json(new { success = false, message = "Error Occurred. Please contact system Administrator." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return View(payDecisionViewModel);
            }
            catch (DbEntityValidationException e)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveDBEntityValidationLog(e);
                throw;
            }
            catch (Exception ex)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveException(ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { success = false, message = "Error Occurred. Please contact system Administrator." }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Edit Pay Decision
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public PartialViewResult Edit(int id, string empID, string fteHour, string fteStatus, string rateofPay, string stdElection)
        {
            try
            {
                PayDecision pD = db.PayDecisions.Find(id);
                var payDecisionDates = db.PayDecisionDates.OrderByDescending(x => x.PayDate).Select(c => c.PayDate).ToList();
                PayDecisionViewModel pdVM = new PayDecisionViewModel()
                {
                    ID = pD.ID,
                    EmployeeID = empID,
                    PayDecisionStatus = pD.PayDecisionStatus,
                    ClockedIn_ClearedToWork_Dt = pD.ClockedIn_ClearedToWork_Dt,
                    STDPayHours = pD.STDPayHours,
                    PayDecisionDt = pD.PayDecisionDt,
                    STDHoursPaidByNSHGross = pD.STDHoursPaidByNSHGross,
                    COVIDBankPayGross = pD.COVIDBankPayGross,
                    PTOAdjustment = pD.PTOAdjustment,
                    ReverseRegHours = pD.ReverseRegHours,
                    Notes = pD.Notes,
                    STDPayDates = pD.STDPayDates,
                    CreatedBy = pD.CreatedBy,
                    CreatedDt = pD.CreatedDt,
                    HotlineLogID = pD.HotlineLogID,
                    ReturnToWork = pD.ReturnToWork,
                    MadeWhole = pD.MadeWhole,
                    HoursInTimeCard = pD.HoursInTimeCard,
                    Week1Cigna = (bool)pD.Week1Cigna,
                    Week2Cigna = (bool)pD.Week2Cigna,
                    Week1TimeCardHours = pD.Week1TimeCardHours,
                    Week2TimeCardHours = pD.Week2TimeCardHours,
                    Week1COVIDPayHours = pD.Week1COVIDPayHours,
                    Week2COVIDPayHours = pD.Week2COVIDPayHours,
                    Week1COVIDPayGross = pD.Week1COVIDPayGross,
                    Week2COVIDPayGross = pD.Week2COVIDPayGross,
                    COVIDPayTotalGross = pD.COVIDPayTotalGross,
                    PTOHoursPaid = pD.PTOHoursPaid,
                    PTODollarsPaid = pD.PTODollarsPaid,
                    PTODollarsPaidWeek1 = pD.PTODollarsPaidWeek1,
                    PTOHoursPaidWeek1 = pD.PTOHoursPaidWeek1,
                    PTODollarsPaidWeek2 = pD.PTODollarsPaidWeek2,
                    PTOHoursPaidWeek2 = pD.PTOHoursPaidWeek2,
                    PTOPayHours = pD.PTOPayHours,
                    PTOAddedByTimekeeper = pD.PTOAddedByTimekeeper,
                    TotalPTOAdjustment = pD.TotalPTOAdjustment,
                    FTEHours = fteHour,
                    FTEStatus = fteStatus,
                    RateOfPay = rateofPay,
                    STDElection_ThisYear = stdElection,
                    PayDates = payDecisionDates.Select(c => new SelectListItem
                    {
                        Value = c.ToString("MM/dd/yyyy"),
                        Text = c.ToString("MM/dd/yyyy")
                    })
                };

                return PartialView("_editModal", pdVM);
            }
            catch (Exception ex)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveException(ex);
                throw;
            }

        }

        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,HotlineLogID,EmployeeID,PayDecisionStatus,ClockedIn_ClearedToWork_Dt,STDPayDtTo,STDPayDtFrom,STDPayHours,PayDecisionDt,STDHoursPaidByNSHGross,PTOAdjustment," +
            "ReverseRegHours,Notes,STDPayDates,HotLineLogPayDecisionTempID,CreatedBy,CreatedDt,isActive,ReturnToWork,HoursInTimeCard,MadeWhole,COVIDBankPayGross,Week1Cigna,Week1TimeCardHours,Week1COVIDPayHours," +
            "Week1COVIDPayGross,Week2Cigna,Week2TimeCardHours,Week2COVIDPayHours,Week2COVIDPayGross,COVIDPayTotalGross,PTOHoursPaid,PTODollarsPaid," +
            "PTOHoursPaidWeek1,PTODollarsPaidWeek1,PTOHoursPaidWeek2,PTODollarsPaidWeek2,PTOPayHours,PTOAddedByTimekeeper,TotalPTOAdjustment")] PayDecisionViewModel payDecisionVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (payDecisionVM.EmployeeID != null && payDecisionVM.EmployeeID.Length == 6)
                    {
                        payDecisionVM.isActive = true;
                        PayDecision pD = db.PayDecisions.Find(payDecisionVM.ID);

                        payDecisionVM.PayDecisionMapToModel(pD);

                        db.Entry(pD).State = EntityState.Modified;
                        pD.ModifiedBy = User.Identity.Name.ToString().Split('\\').Last();
                        pD.ModifiedDt = System.DateTime.Now;
                        db.SaveChanges();


                        var pdEmp = pD.EmployeeID;

                        //var allRecforpdEmp = db.PayDecisions.Where(x => x.EmployeeID == pdEmp && x.isActive == true && (x.STDPayDates != null && x.STDPayDates != "" && x.STDPayDates != "N/A")).ToList();

                        var allRecforpdEmp = db.PayDecisions.Where(x => x.EmployeeID == pdEmp && x.isActive == true && (x.STDPayHours != null && x.STDPayHours != "") && ((x.STDHoursPaidByNSHGross != null && x.STDHoursPaidByNSHGross != "") || (x.COVIDPayTotalGross != null && x.COVIDPayTotalGross != ""))).ToList();


                        //var sumofPayHours = (from e in allRecforpdEmp
                        //                                      group e by e.EmployeeID into g
                        //                                      select g.Sum(c => Convert.ToDecimal(c.STDPayHours)).ToString());

                        var sumofPayHours = allRecforpdEmp.Sum(x => x.STDPayHours.ValidateToDecimal());

                        var hLRecord = db.HotlineLogs.Where(x => x.EmployeeID == payDecisionVM.EmployeeID).ToList();
                        //var oldHours = hLRecord.FirstOrDefault().HoursPreviouslyPaid;
                        //var newHours = sumofPayHours.ToString();
                        var oldHours = hLRecord.OrderByDescending(y => y.DateOfCall).FirstOrDefault().HoursPreviouslyPaid;
                        var newFTEHours = hLRecord.OrderByDescending(y => y.DateOfCall).FirstOrDefault().FTEHours;
                        var newHours = sumofPayHours.ToString();


                        foreach (var item in hLRecord)
                        {
                            db.Entry(item).State = EntityState.Modified;
                            item.PayReviewStatus = "Completed";
                            item.Modifiedby = User.Identity.Name.ToString().Split('\\').Last();
                            item.ModifiedDt = System.DateTime.Now;
                            item.FTEHours = newFTEHours.ToString();
                            if (newHours != oldHours)
                            {
                                item.HoursPreviouslyPaid = newHours;

                            }
                            item.DiffHoursToFTE = CalculateHourPay.CalculateDiffHours(newHours, item.FTEHours);
                            if (item.DiffHoursToFTE.ValidateToDecimal() == 0m)
                            {
                                item.RecordStatus = "Max Paid";
                            }
                            else if (item.DiffHoursToFTE.ValidateToDecimal() > 0m)
                            {
                                item.RecordStatus = "Bank Exhausted";
                            }
                            else
                            {
                                item.RecordStatus = "Open";
                            }

                            db.SaveChanges();
                        }
                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var applog = new ApplicationExceptionLog();
                        applog.SaveEmployeeIDException($"Employee ID is null. LogPayDecisionController -- ActionResult CreateNew([Bind(Include = 'EmployeeID....')]");
                        return Json(new { success = false, message = "Error Occurred. Please contact system Administrator." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return PartialView("_editModal", payDecisionVM);
            }
            catch (DbEntityValidationException e)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveDBEntityValidationLog(e);
                throw;
            }
            catch (Exception ex)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveException(ex);
                //throw;
                return Json(new { success = false, message = "Error updating record. Please contact System Administrator" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Pay Decision Edit/View/Delete



        #region Delete/Inactivate the record
        // GET: PayDecisions/Delete/5
        [CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        public ActionResult Inactivate(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var payDecisionRecord = db.PayDecisions.Find(id);
                var empID = payDecisionRecord.EmployeeID;
                payDecisionRecord.ModifiedBy = User.Identity.Name.ToString().Split('\\').Last();
                payDecisionRecord.ModifiedDt = System.DateTime.Now;
                payDecisionRecord.isActive = false;
                db.SaveChanges();

                var pdEmp = payDecisionRecord.EmployeeID;
                //var allRecforpdEmp = db.PayDecisions.Where(x => x.EmployeeID == pdEmp && x.isActive == true && (x.STDPayDates != null && x.STDPayDates != "" && x.STDPayDates != "N/A")).ToList();

                var allRecforpdEmp = db.PayDecisions.Where(x => x.EmployeeID == pdEmp && x.isActive == true && (x.STDPayHours != null && x.STDPayHours != "") && ((x.STDHoursPaidByNSHGross != null && x.STDHoursPaidByNSHGross != "") || (x.COVIDPayTotalGross != null && x.COVIDPayTotalGross != ""))).ToList();

                var sumofPayHours = allRecforpdEmp.Sum(x => x.STDPayHours.ValidateToDecimal());

                var hLRecord = db.HotlineLogs.Where(x => x.EmployeeID == payDecisionRecord.EmployeeID).ToList();

                var oldHours = hLRecord.OrderByDescending(y => y.DateOfCall).FirstOrDefault().HoursPreviouslyPaid;
                var newFTEHours = hLRecord.OrderByDescending(y => y.DateOfCall).FirstOrDefault().FTEHours;
                var newHours = sumofPayHours.ToString();


                foreach (var item in hLRecord)
                {
                    db.Entry(item).State = EntityState.Modified;
                    item.Modifiedby = User.Identity.Name.ToString().Split('\\').Last();
                    item.ModifiedDt = System.DateTime.Now;
                    item.FTEHours = newFTEHours.ToString();
                    if (newHours != oldHours)
                    {
                        item.HoursPreviouslyPaid = newHours;

                    }
                    item.DiffHoursToFTE = CalculateHourPay.CalculateDiffHours(newHours, item.FTEHours);
                    if (item.DiffHoursToFTE.ValidateToDecimal() == 0m)
                    {
                        item.RecordStatus = "Max Paid";
                    }
                    else if (item.DiffHoursToFTE.ValidateToDecimal() > 0m)
                    {
                        item.RecordStatus = "Bank Exhausted";
                    }
                    else
                    {
                        item.RecordStatus = "Open";
                    }
                    db.SaveChanges();
                }
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveDBEntityValidationLog(e);
                throw;
            }
            catch (Exception ex)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveException(ex);
                //throw;
                return Json(new { success = false, message = "Error Inactivating record. Please contact System Administrator" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Edit/View Details NOT USED
        //[CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PayDecision payDecision = db.PayDecisions.Find(id);
        //    if (payDecision == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(payDecision);
        //}
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PayDecision payDecision = db.PayDecisions.Find(id);
        //    if (payDecision == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(payDecision);
        //}

        // POST: PayDecisions/Delete/5
        //[CPTCustomAuthorize(Roles = @"NS\CovidPayHRAppMembers")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PayDecision payDecision = db.PayDecisions.Find(id);
        //    db.PayDecisions.Remove(payDecision);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        #endregion


        #endregion

    }

}