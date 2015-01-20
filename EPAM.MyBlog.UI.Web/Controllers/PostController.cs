using EPAM.MyBlog.UI.Web.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class PostController : Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(AdminController));
        //
        // GET: /Post/


        
        public ActionResult Favorite() 
        {
            return View(PresentPostModel.GetAllFavorite(User.Identity.Name));
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

        
        public ActionResult MyPosts()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(PresentPostModel.GetAllPostsTitle(User.Identity.Name));
            }
            return View(PresentPostModel.GetAllPostsTitle(User.Identity.Name));
        }

        public ActionResult Posts(Guid Id)
        {
            ViewData["Check"] = PostModel.CheckFavorite(User.Identity.Name, Id).ToString();
            var post = PostModel.GetPostById(Id);
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
                    logger.Info("Изменен пост id: " + post.Id + "у пользователя: " + User.Identity.Name);
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    logger.Error("Ошибка при изменении поста id: " + post.Id + "у пользователя: " + User.Identity.Name);
                    return View();
                }
            }
            return View();

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
                    logger.Info("Удален пост id: " + id + "у пользователя: " + User.Identity.Name);
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    logger.Error("Ошибка при удалении поста id: " + id + "у пользователя: " + User.Identity.Name);
                    return View();
                }
            }
            return View();
        }

        public ActionResult DeleteFavorite(Guid Id)
        {
            var post = PostModel.GetPostById(Id);
            ViewData["Title"] = post.Title;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteFavorite(Guid id, ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                if (PostModel.DeleteFavorite(User.Identity.Name, id))
                {
                    logger.Info("Удален пост id: " + id + "из Избранного у пользователя: " + User.Identity.Name);
                    return RedirectToAction("Favorite", "Post");
                }
                else
                {
                    logger.Error("Ошибка при удалении поста id: " + id + "из Избранного у пользователя: " + User.Identity.Name);
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

        public ActionResult UserPosts(string name)
        {
            return View(PresentPostModel.GetAllPostsTitle(name));
        }
    }
}
