using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gumruk.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WebixTest()
        {
            return View();
        }

        public ActionResult DragableText()
        {
            return View();
        }
    }
}