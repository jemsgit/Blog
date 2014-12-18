using EPAM.MyBlog.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login(string ReturnUrl)
        {
            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = "";
            }
            ViewData.Add("ReturnUrl", ReturnUrl);
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(UserModel model, string ReturnUrl)
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


        public ActionResult Reg(string ReturnUrl)
        {
            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = "";
            }
            ViewData.Add("ReturnUrl", ReturnUrl);
            return View();
        }


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
                    UserModel.LogOut();
                }
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [ChildActionOnly]
        public ActionResult State()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MenuState()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult TitleState()
        {
            return PartialView();
        }
    }
}
