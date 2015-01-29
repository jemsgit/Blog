using EPAM.MyBlog.UI.Web.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class FilesController : Controller
    {

        private static ILog logger = LogManager.GetLogger(typeof(FilesController));
        //
        // GET: /Files/

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return PartialView("Index");
            return View();
        }

        public ActionResult EditInfoAvatar()
        {
            if (Request.IsAjaxRequest())
                return PartialView("EditInfoAvatar");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfoAvatar(HttpPostedFileBase File)
        {
            var model = new AvatarModel();
            model.Login = User.Identity.Name;

            if (File != null) 
            {
                if (!File.ContentType.Contains("image"))
                {
                    logger.Error("Попытка загрузки не image в качетсве аватара пользователем: " + User.Identity.Name);
                    return View();
                }

                if (ModelState.IsValid)
                {
                    model.MimeType = File.ContentType;
                    model.Avatar = new byte[File.ContentLength];

                    File.InputStream.Read(model.Avatar, 0, File.ContentLength);
                    model.AddPhoto();

                    logger.Info("Загрузка нового аватара пользователем: " + User.Identity.Name);
                    return RedirectToAction("AboutMe", "Account");
                }

                else
                {
                    logger.Debug("Модель аватара невалидна у пользователя: " + User.Identity.Name);
                    return View();
                }
            }

            return View();

            
            
            
        }

        public FileContentResult GetImage(string name)
        {
            if (name != null)
            {
                var info = AvatarModel.GetInfo(name);
                logger.Info("Загрузка аватара из базы для пользователя: " + User.Identity.Name);
                return File(info.Avatar, info.MimeType);
            }
            else
            {
                return null;
            }
        }
    }
}
