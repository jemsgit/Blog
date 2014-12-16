using EPAM.MyBlog.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult NewPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPost(PostModel model)
        {
            if (ModelState.IsValid)
            {
                //string Result;
                if (model.AddPost(User.Identity.Name.ToString()))
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

        [ChildActionOnly]
        public ActionResult MyPosts()
        {
            return PartialView(PresentPostModel.GetAllPostsTitle(User.Identity.Name));
        }

      
    }
}
