using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CovidPayTracking.Models
{
    public class CovidTestViewModel
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Employee ID")]
        public string EmployeeNumber { get; set; }

        [Display(Name = "Employee First Name")]
        public string EmployeeFirstName { get; set; }

        [Display(Name = "Employee Last Name")]
        public string EmployeeLastName { get; set; }

        [Display(Name = "Resulted")]
        public DateTime? Resulted { get; set; }

        [Display(Name = "Result")]
        public string Result { get; set; }

        [Display(Name = "Source")]
        public string Source { get; set; }

        public void CovidTestMapToModel(CovidTest ct)
        {
            ct.ID = ID;
            ct.EmployeeNumber = EmployeeNumber;
            ct.EmployeeFirstName = EmployeeFirstName;
            ct.EmployeeLastName = EmployeeLastName;
            ct.RESULTED = Resulted;
            ct.RESULT = Result;
            ct.Source = Source;
        }

    }
}