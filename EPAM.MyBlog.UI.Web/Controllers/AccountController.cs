﻿using EPAM.MyBlog.UI.Web.Models;
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
            return View();

        }

        [Authorize(Roles = "User")]
        public ActionResult LogOut()
        {
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
                }
                return RedirectToAction("Index", "Home");
            }

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
                }
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult Avatar() 
        {

            var info = AvatarModel.GetInfo(User.Identity.Name);
            ViewData["path"] = "~/Content/img/" + User.Identity.Name + ".jpg";
            var file = File(info.Avatar, info.MimeType, User.Identity.Name + ".jpg");
            
            return PartialView(info);
        }

        public ActionResult AboutMe() 
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
            return View(info);
        }

        public ActionResult EditSex()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
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
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }



        public ActionResult EditDate()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
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
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult EditName()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
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
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }


        public ActionResult EditAbout()
        {
            var info = UserAboutModel.GetInfo(User.Identity.Name);
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
                    return RedirectToAction("AboutMe", "Account");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
