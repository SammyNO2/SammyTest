using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace CovidPayTracking.Helper
{
    public class ApplicationExceptionLog
    {
        private HotLineLogEntities db = new HotLineLogEntities();
        public bool SaveException(Exception ex)
        {
            var exception = new ApplicationLog();
            exception.ShortDescription = $"Message: {ex.Message}";
            exception.LongDescription = $"InnerException: {ex.InnerException};; StackTrace: {ex.StackTrace}";
            exception.CreatedDt = DateTime.Now;
            db.ApplicationLogs.Add(exception);
            db.SaveChanges();
            return true;
        }

        public bool SaveEmployeeIDException(string strEmployeeID)
        {
            var exception = new ApplicationLog();
            exception.ShortDescription = $"Message: Error in EmployeeID";
            exception.LongDescription = $"EmployeeID : {strEmployeeID}";
            exception.CreatedDt = DateTime.Now;
            db.ApplicationLogs.Add(exception);
            db.SaveChanges();
            return true;
        }

        public bool SaveDBEntityValidationLog(DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                var exception = new ApplicationLog
                {
                    ShortDescription = $"Entity of type {eve.Entry.GetType().Name} in state {eve.Entry.State} has the following validation errors:",
                    CreatedDt = DateTime.Now
                };
                var entityExceptionDescription = string.Empty;
                foreach (var ve in eve.ValidationErrors)
                {
                    entityExceptionDescription += $"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}";
                }
                exception.LongDescription = entityExceptionDescription;
                db.ApplicationLogs.Add(exception);
                db.SaveChanges();
            }
            return true;
        }
    }
}