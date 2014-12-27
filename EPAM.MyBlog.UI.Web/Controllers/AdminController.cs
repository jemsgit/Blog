using EPAM.MyBlog.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        [Authorize(Roles = "Moder")]
        [ChildActionOnly]
        public ActionResult ModerMenu()
        {
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [ChildActionOnly]
        public ActionResult AdminMenu()
        {
            return PartialView();
            
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            return View(UserAdminModel.GetAllUsers());
        }

    }
}
