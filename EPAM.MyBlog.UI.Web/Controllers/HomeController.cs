using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return PartialView("Index");
            return View();
        }

        public ActionResult Help()
        {
            if (Request.IsAjaxRequest())
                return PartialView("Help");
            return View();
        }

    }
}
