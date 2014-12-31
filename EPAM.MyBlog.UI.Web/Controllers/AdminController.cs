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

        [Authorize(Roles = "Admin")]
        public ActionResult Posts(string name)
        {
            return View(PresentPostModel.GetAllPostsTitle(name));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Comments()
        {
            return PartialView();

        }


        public ActionResult Delete(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            ViewData["Title"] = post.Title;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid id, ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                if (PostModel.Delete(id))
                {
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
    }
}
