using CovidPayTracking.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CovidPayTracking.Models;

namespace CovidPayTracking.Controllers
{
    public class SendEmailController : Controller
    {
        // GET: SendEmail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PayDecisionEmail()
        {
            var emailViewModel = new EmailViewModel();

            var sendEmailController = new SendEmailController();
            sendEmailController.ControllerContext = new ControllerContext(Request.RequestContext, sendEmailController);
            var emailBody = ConvertViewEmailTemplate.RenderViewToString(sendEmailController.ControllerContext, "~/Views/SendEmail/PayDecisionEmail.cshtml", emailViewModel);
            SendSMTPEmail.SendEmail(emailBody, "", "");

            return View();
        }
    }
}