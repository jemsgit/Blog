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
            return View(UserAdminModel.GetAllUsers());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            return View(UserAdminModel.GetAllUsers());
        }

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
                    logger.Info("Удаление поста " + id);
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                    {
                        return RedirectToAction("Users", "Admin");
                    }
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
                    logger.Info("Удаление комментария " + id);
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                    {
                        return RedirectToAction("Users", "Admin");
                    }
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
            return View();

        }

        //[HttpPost]
        //public ActionResult ChangeUser()
        //{
        //    string exist;

            
        //    for (int i = 0; i < length; i++)
        //    {
        //        exist = Request[i.ToString()];
        //        if (exist == "on")
        //        {
        //            UserModel.AddLink(idUser, ChooseAwardsModel.awards[i].AwardId);
        //        }
        //    }
        //}
    }
}
