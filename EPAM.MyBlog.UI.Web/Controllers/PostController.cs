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
            model.Id = Guid.NewGuid();
            model.Time = DateTime.Now;
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

        public ActionResult Posts(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            return View(post);
        }


        public ActionResult Edit(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            return View(post);
        }

        [HttpPost]
        public ActionResult Edit(PostModel post)
        {
            post.Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (post.EditPost())
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


        public ActionResult Delete(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            //ViewData["Post_Id"] = Id;
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

        public ActionResult Comments(Guid id)
        {
            //
            return View();
        }

        public ActionResult News()
        {
            var posts = PresentPostModel.GetAllPostsTitle();
            return View(posts);
        }
    }
}
