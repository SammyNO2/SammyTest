using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CovidPayTracking.Models
{
    public class PayPeriodViewModel
    {
        [Key]
        public int PayPeriodNum { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Pay Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? PayDate { get; set; }


        public void PayPeriodMapToModel(PayPeriod pp)
        {
            pp.PayPeriodNum = PayPeriodNum;
            pp.StartDate = StartDate;
            pp.EndDate = EndDate;
            pp.PayDate = PayDate;
        }

    }
}