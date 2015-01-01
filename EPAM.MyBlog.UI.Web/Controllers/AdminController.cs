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
        public ActionResult UserPosts(string name)
        {
            return View(PresentPostModel.GetAllPostsTitle(name));
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Posts(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            return View(post);
        }
        


        [Authorize(Roles = "Admin")]
        public ActionResult UserComments(string name)
        {
            return View(CommentModel.GetAllComments(name));
        }


        public ActionResult DeletePost(Guid Id, string ReturnUrl)
        {
            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = "";
            }
            var post = PostModel.GetPostById(Id);
            ViewData["Title"] = post.Title;
            ViewData.Add("ReturnUrl", ReturnUrl);
            return View();
        }


        [HttpPost]
        public ActionResult DeletePost(Guid id, ConfirmModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (PostModel.Delete(id))
                {
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                    {
                        return RedirectToAction("Users", "Admin");
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        /// <summary>
        /// Доделать
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        public ActionResult DeleteComment(Guid Id, string ReturnUrl)
        {
            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = "";
            }
            ViewData.Add("ReturnUrl", ReturnUrl);
            return View();
        }


        [HttpPost]
        public ActionResult DeleteComment(Guid id, ConfirmModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (CommentModel.Delete(id))
                {
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                    {
                        return RedirectToAction("Users", "Admin");
                    }
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
