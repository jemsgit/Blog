using EPAM.MyBlog.UI.Web.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EPAM.MyBlog.UI.Web.Controllers
{


    [Authorize(Roles = "User")]
    public class AccountController : Controller
    {

        private static ILog logger = LogManager.GetLogger(typeof(AccountController));

        public ActionResult Index()
        {
            return View();
        }


        //
        // GET: /Account/
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {

            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = "";
            }
            ViewData.Add("ReturnUrl", ReturnUrl);
            if (Request.IsAjaxRequest())
                return PartialView("Login");
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginModel model, string ReturnUrl)
        {
            var checkbox = Request.Form["remember1"];
            if (checkbox == "on")
            {
                model.Remember = true;
            }
            else
            {
                model.Remember = false;
            }
            if (ModelState.IsValid)
            {
                string Result;
                if (model.Login(out Result))
                {
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewData["Result"] = Result;
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("Login");
            return View();
        }

        [AllowAnonymous]
        public ActionResult Reg(string ReturnUrl)
        {
            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = "";
            }
            ViewData.Add("ReturnUrl", ReturnUrl);
            if (Request.IsAjaxRequest())
                return PartialView("Reg");
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Reg(RegModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                string Result;
                if (model.Reg(out Result))
                {

                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewData["Result"] = Result;
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("Reg");
            return View();

        }

        [Authorize(Roles = "User")]
        public ActionResult LogOut()
        {
            if (Request.IsAjaxRequest())
                return PartialView("LogOut");
            return View();
        }

        [HttpPost]
        public ActionResult LogOut(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Confirm)
                {
                    LoginModel.LogOut();
                    logger.Info("Пользователь вышел из системы: " + User.Identity.Name);

                }
                return RedirectToAction("Index", "Home");
            }
            if (Request.IsAjaxRequest())
                return PartialView("LogOut");
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult State()
        {
            return PartialView();
        }
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult MenuState()
        {
            return PartialView();
        }



        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult TitleState()
        {
            return PartialView();
        }

        [Authorize(Roles = "User")]
        public ActionResult DeleteAc()
        {
            if (Request.IsAjaxRequest())
                return PartialView("DeleteAc");
            return View();
        }

        [HttpPost]
        public ActionResult DeleteAc(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Confirm)
                {
                    LoginModel.LogOut();
                    LoginModel.SaveReason(model.Reason);
                    LoginModel.DeleteUser(User.Identity.Name);
                    logger.Info("Пользователь удалил свой аккаунт с логином: " + User.Identity.Name);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("DeleteAc");
            return View();
        }

        public ActionResult Avatar(string name) 
        {
            var info = AvatarModel.GetInfo(name);
            return PartialView(info);
        }

        public ActionResult UserAvatar(string name)
        {
            var info = AvatarModel.GetInfo(name);
            return PartialView(info);
        }

        public ActionResult AboutMe() 
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
            if (Request.IsAjaxRequest())
                return PartialView("AboutMe", info);
            return View(info);
        }

        public ActionResult UserInfo(string name) 
        {
            var info = UserAboutModel.GetInfo(name);
            if (Request.IsAjaxRequest())
                return PartialView("UserInfo", info);
            return View(info);
        }
        

        public ActionResult EditSex()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
            if (Request.IsAjaxRequest())
                return PartialView("EditSex", info);
            return View(info);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSex(UserAboutModel model)
        {
            if(string.IsNullOrEmpty(model.Sex))
            {
                model.Sex = " ";
            }
            model.Login = User.Identity.Name;
            if (ModelState.IsValid)
            {
                if (model.SetSex())
                {
                    logger.Info("Пользователь: " + User.Identity.Name + " изменил информацию о поле");
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    logger.Error("Ошибка при изменении информации о поле пользователя: " + User.Identity.Name);
                    if (Request.IsAjaxRequest())
                        return PartialView("EditSex", model);
                    return View(model);
                }
            }
            else
            {
                logger.Debug("Невалидная модель при изменении данных о поле пользователя: " + User.Identity.Name);
                if (Request.IsAjaxRequest())
                    return PartialView("EditSex", model);
                return View(model);
            }
        }



        public ActionResult EditDate()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
            if (Request.IsAjaxRequest())
                return PartialView("EditDate", info);
            return View(info);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDate(UserAboutModel model)
        {
            model.Login = User.Identity.Name;
            if (ModelState.IsValid)
            {
                if (model.SetDate())
                {
                    logger.Info("Пользователь: " + User.Identity.Name + " изменил дату рождения");
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    logger.Error("Ошибка при изменении даты рождения пользователя: " + User.Identity.Name);
                    return View(model);
                }
            }
            else
            {
                logger.Debug("Невалидная модель при изменении даты рождения пользователе: " + User.Identity.Name);
                return View(model);
            }
        }

        public ActionResult EditName()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
            if (Request.IsAjaxRequest())
                return PartialView("EditName", info);
            return View(info);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditName(UserAboutModel model)
        {
            model.Login = User.Identity.Name;
            if (string.IsNullOrEmpty(model.Name))
            {
                model.Name = " ";
            }
            if (ModelState.IsValid)
            {
                if (model.SetName())
                {
                    logger.Info("Пользователь: " + User.Identity.Name + " изменил имя");
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    logger.Error("Ошибка при изменении имени пользователя: " + User.Identity.Name);
                    return View(model);
                }
            }
            else
            {
                logger.Debug("Невалидная модель при изменении имени пользователя: " + User.Identity.Name);
                return View(model);
            }
        }


        public ActionResult EditAbout()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
            if (Request.IsAjaxRequest())
                return PartialView("EditAbout", info);
            return View(info);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAbout(UserAboutModel model)
        {
            if (string.IsNullOrEmpty(model.AboutMe))
            {
                model.AboutMe = " ";
            }
            model.Login = User.Identity.Name;
            if (ModelState.IsValid)
            {
                if (model.SetAbout())
                {
                    logger.Info("Пользователь: " + User.Identity.Name + " изменил информцияю о себе");
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    logger.Error("Ошибка при изменении информации о себе пользователя: " + User.Identity.Name);
                    return View(model);
                }
            }
            else
            {
                logger.Debug("Невалидная модель при изменении информации о себе пользователя: " + User.Identity.Name);
                return View(model);
            }
        }
    }
}
