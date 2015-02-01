using EPAM.MyBlog.UI.Web.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EPAM.MyBlog.UI.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class PostController : Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(AdminController));

        //
        // GET: /Post/

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Favorite() 
        {
            if (Request.IsAjaxRequest())
                return PartialView("Favorite", PresentPostModel.GetAllFavorite(User.Identity.Name));
            return View(PresentPostModel.GetAllFavorite(User.Identity.Name));
        }

        public ActionResult NewPost()
        {
            if (Request.IsAjaxRequest())
                return PartialView("NewPost");
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult NewPost(PostModel model)
        {
            model.Id = Guid.NewGuid();
            model.Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (model.Text.ToLower().IndexOf("<script>") != -1 || model.Text.ToLower().IndexOf("style") != -1)
                {
                    if (Request.IsAjaxRequest())
                        return PartialView("NewPost");
                }
                //string Result;
                if (model.AddPost(User.Identity.Name.ToString()))
                {

                    return RedirectToAction("MyPosts", "Post");
                }
                else
                {
                    if (Request.IsAjaxRequest())
                        return PartialView("NewPost");
                    return View();
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("NewPost");
            return View();
        }

        
        public ActionResult MyPosts()
        {
            if (Request.IsAjaxRequest())
                return PartialView("MyPosts", PresentPostModel.GetAllPostsTitle(User.Identity.Name));
            return View(PresentPostModel.GetAllPostsTitle(User.Identity.Name));
        }

        [AllowAnonymous]
        public ActionResult Posts(Guid Id)
        {
            ViewData["Check"] = PostModel.CheckFavorite(User.Identity.Name, Id).ToString();
            var post = PostModel.GetPostById(Id);
            if (Request.IsAjaxRequest())
                return PartialView("Posts", post);
            return View(post);
        }

        [HttpPost]
        public ActionResult Posts(PostModel model, string action)
        {
            if(action == "AddFav")
            {
               PostModel.AddFavorite(User.Identity.Name, model.Id);
               logger.Info("Добавлен пост с id: " + model.Id + "в избранное пользователя: " + User.Identity.Name);
               return RedirectToAction("Posts", new {Id = model.Id});
            }
            else
            {
                return RedirectToAction("Posts", new {Id = model.Id});
            }
        }

        public ActionResult Edit(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            if (Request.IsAjaxRequest())
                return PartialView("Edit", post);
            return View(post);
        }

        [HttpPost]
        public ActionResult Edit(PostModel post)
        {
            post.Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (post.Text.ToLower().IndexOf("<script>") != -1 || post.Text.ToLower().IndexOf("style") != -1)
                {
                    if (Request.IsAjaxRequest())
                        return PartialView("Edit");
                }
                if (post.EditPost())
                {
                    logger.Info("Изменен пост id: " + post.Id + "у пользователя: " + User.Identity.Name);
                    return RedirectToAction("MyPosts", "Post");
                }
                else
                {
                    logger.Error("Ошибка при изменении поста id: " + post.Id + "у пользователя: " + User.Identity.Name);
                    if (Request.IsAjaxRequest())
                        return PartialView("Edit");
                    return View();
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("Edit");
            return View();

        }


        public ActionResult Delete(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            ViewData["name"] = post.Title;
            if (Request.IsAjaxRequest())
                return PartialView("Delete");
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid id, ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Confirm)
                {

                    if (PostModel.Delete(id))
                    {
                        logger.Info("Удален пост id: " + id + "у пользователя: " + User.Identity.Name);
                        return RedirectToAction("MyPosts", "Post");
                    }
                    else
                    {
                        logger.Error("Ошибка при удалении поста id: " + id + "у пользователя: " + User.Identity.Name);
                        if (Request.IsAjaxRequest())
                            return PartialView("Delete", new { Id = id });
                        return View();
                    }
                }
                else 
                {
                    return RedirectToAction("MyPosts", "Post");
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("Delete", new { Id = id });
            return View();
        }

        public ActionResult DeleteFavorite(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            ViewData["name"] = post.Title;
            if (Request.IsAjaxRequest())
                return PartialView("DeleteFavorite");
            return View();
        }

        [HttpPost]
        public ActionResult DeleteFavorite(Guid id, ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Confirm)
                {
                    if (PostModel.DeleteFavorite(User.Identity.Name, id))
                    {
                        logger.Info("Удален пост id: " + id + "из Избранного у пользователя: " + User.Identity.Name);
                        return RedirectToAction("Favorite", "Post");
                    }
                    else
                    {
                        logger.Error("Ошибка при удалении поста id: " + id + "из Избранного у пользователя: " + User.Identity.Name);
                        if (Request.IsAjaxRequest())
                            return PartialView("DeleteFavorite", new { Id = id });
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Favorite", "Post");
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("DeleteFavorite");
            return View();
        }

        [AllowAnonymous]
        public ActionResult Comments(Guid id)
        {
            return View();
        }

        public ActionResult News()
        {
            var posts = PresentPostModel.GetAllPostsTitle();
            if (Request.IsAjaxRequest())
                return PartialView("News", posts);
            return View(posts);
        }

        public ActionResult UserPosts(string name)
        {
            if (Request.IsAjaxRequest())
                return PartialView("UserPosts", PresentPostModel.GetAllPostsTitle(name));
            return View(PresentPostModel.GetAllPostsTitle(name));
        }
    }
}
