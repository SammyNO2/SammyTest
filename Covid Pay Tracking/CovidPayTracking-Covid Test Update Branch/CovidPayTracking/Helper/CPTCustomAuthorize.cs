using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CovidPayTracking.Helper
{
    public class CPTCustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //If authorized, handle accourdingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                //Otherwise redirect to unauthorized view
                filterContext.Result = new RedirectResult("~/Error/Unathorized");
            }

        }
    }
}