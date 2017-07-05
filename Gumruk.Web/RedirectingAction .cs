using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gumruk.Web
{
    public class AuthenticationAction : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            HttpContext context = HttpContext.Current;

            if (context.Session["CurrentUser"] == null)
            {
                if (!HttpContext.Current.Request.Path.EndsWith("Home/Index", StringComparison.InvariantCultureIgnoreCase))
                    context.Response.Redirect("~/Home/Login");
            }
        }
    }
}