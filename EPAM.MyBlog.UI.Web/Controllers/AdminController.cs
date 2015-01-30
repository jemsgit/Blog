using EPAM.MyBlog.UI.Web.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class AdminController : Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(AdminController));
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


        [Authorize(Roles = "Moder")]
        public ActionResult PostComment()
        {
            if (Request.IsAjaxRequest())
                return PartialView("PostComment", UserAdminModel.GetAllUsers());
            return View(UserAdminModel.GetAllUsers());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            if (Request.IsAjaxRequest())
                return PartialView("Users", UserAdminModel.GetAllUsers());
            return View(UserAdminModel.GetAllUsers());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Users(string action)
        {
            string exist;
            var list = new List<UserAdminModel>(UserAdminModel.GetAllUsers());
            List<string> names = new List<string>();

            foreach (var item in list)
	        {
                exist = Request.Form[item.Name];
                if (exist == "on")
                {
                    names.Add(item.Name);
                }
	        }

            switch (action)
            {
                case "addUser":
                    UserAdminModel.AddUser(names);
                    logger.Info("Изменены роли на -Пользователь- у пользователей: " + string.Join(",", names.ToArray()));
                    break;
                case "addModer":
                    UserAdminModel.AddModer(names);
                    logger.Info("Изменены роли на -Модератор- у пользователей: " + string.Join(",", names.ToArray()));
                    break;
                case "addAdmin":
                    UserAdminModel.AddAdmin(names);
                    logger.Info("Изменены роли на -Администратор- у пользователей: " + string.Join(",", names.ToArray()));
                    break;
                case "Delete":
                    UserAdminModel.DeleteUsers(names);
                    logger.Info("Удалены учетные записи пользователей: " + string.Join(",", names.ToArray()));
                    break;
                default:
                    break;
            }
            if (Request.IsAjaxRequest())
                return PartialView("Users", UserAdminModel.GetAllUsers());
            return View(UserAdminModel.GetAllUsers());
        }

        [Authorize(Roles = "Admin, Moder")]
        public ActionResult UserPosts(string name)
        {
            ViewData["name"] = name;
            if (Request.IsAjaxRequest())
                return PartialView("UserPosts", PresentPostModel.GetAllPostsTitle(name));
            return View(PresentPostModel.GetAllPostsTitle(name));
        }


        [Authorize(Roles = "Admin, Moder")]
        public ActionResult UserComments(string name)
        {
            ViewData["name"] = name;
            if (Request.IsAjaxRequest())
                return PartialView("UserComments", CommentModel.GetAllComments(name));
            return View(CommentModel.GetAllComments(name));
        }

        [Authorize(Roles = "Admin, Moder")]
        public ActionResult DeletePost(Guid Id, string name)
        {
            var post = PostModel.GetPostById(Id);
            ViewData["Title"] = post.Title;
            ViewData["name"] = name;
            if (Request.IsAjaxRequest())
                return PartialView("DeletePost");
            return View();
        }

        [Authorize(Roles = "Admin, Moder")]
        [HttpPost]
        public ActionResult DeletePost(Guid id, ConfirmModel model, string name)
        {
            if (ModelState.IsValid)
            {
                if (PostModel.Delete(id))
                {
                    logger.Info("Удаление поста " + id);
                    return RedirectToAction("UserPosts", "Admin", new {name = name});
                }
                else
                {
                    logger.Error("Ошибка удаления поста " + id);
                    return View();
                }
            }
            else
            {
                logger.Debug("Невалидная модель удаления поста " + id);
                return View();
            }
        }

        /// <summary>
        /// Доделать
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin, Moder")]
        public ActionResult DeleteComment(Guid Id, string name)
        {
            ViewData["name"] = name;
            if (Request.IsAjaxRequest())
                return PartialView("DeleteComment");
            return View();
        }

        [Authorize(Roles = "Admin, Moder")]
        [HttpPost]
        public ActionResult DeleteComment(Guid id, ConfirmModel model, string name)
        {
            if (ModelState.IsValid)
            {
                if (CommentModel.Delete(id))
                {
                    logger.Info("Удаление комментария " + id);
                    return RedirectToAction("UserComments", "Admin", new {name = name});
                }
                else
                {
                    logger.Error("Ошибка удаления комментария " + id);
                    return View();
                }
            }
            return View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView("Create");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(RegModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                string Result;
                if (model.RegByAdmin(out Result))
                {
                    logger.Info("Регистрация Администратором пользователя с логином: " + model.Name);
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Users", "Admin");
                    }
                }
                else
                {
                    logger.Error("Ошибка регистрации Администратором пользователя с логином: " + model.Name);
                    ViewData["Result"] = Result;
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("Create");
            return View();

        }

    }
}
