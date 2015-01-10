using EPAM.MyBlog.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            if (!String.IsNullOrEmpty(model.SearchText))
            {
                return RedirectToAction("Result", new { search = model.SearchText });
            }
            else
            {
                return PartialView();
            }

        }

        [HttpPost]
        public ActionResult Result(string search)
        {
            SearchModel model = new SearchModel();
            var result = model.GetResult();
            return View(result);
        }
    }
}
