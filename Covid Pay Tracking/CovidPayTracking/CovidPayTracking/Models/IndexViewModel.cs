using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CovidPayTracking.Models
{
    public class IndexViewModel
    {
        public DateTime? SearchFromDate { get; set; }

        public DateTime? SearchToDate { get; set; }

        public string SelectedDateCategoryValue { get; set; }

        public List<HotLineLogViewModel> HotLineLogs { get; set; }

        public List<PayDecisionViewModel> PayDecisions { get; set; }
    }
}