﻿using EPAM.MyBlog.UI.Web.Models;
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
            if (Request.IsAjaxRequest())
                return PartialView("Index",null);
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
                    return RedirectToAction("Result", new { search = " " });
                }

        }

        
        public ActionResult Result(string search)
        {
            SearchModel model = new SearchModel();
            model.SearchText = search;
            var result = model.GetResult();
            if (Request.IsAjaxRequest())
                return PartialView("Result", result);
            return View(result);
        }

        public ActionResult Tags()
        {
            return PartialView(SearchModel.GetTopTags());
        }
    }
}
