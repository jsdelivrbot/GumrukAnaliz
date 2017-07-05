using Gumruk.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Gumruk.Web
{
    public class BaseController : Controller
    {
        public users GetCurrentUser()
        {
            if (Session["CurrentUser"] != null)
                return (users)Session["CurrentUser"];
            else
                RedirectToAction("Login","Home");

            return null;
        }

        public void SetCurrentUser(users user)
        {
            Session["CurrentUser"] = user;
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    Exception ex = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;

        //    var model = new HandleErrorInfo(filterContext.Exception, "Home", "Error");

        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error",
        //        ViewData = new ViewDataDictionary(model)
        //    };

        //}

    }
}