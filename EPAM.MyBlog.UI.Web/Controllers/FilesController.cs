using EPAM.MyBlog.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class FilesController : Controller
    {
        //
        // GET: /Files/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditInfoAvatar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfoAvatar(HttpPostedFileBase File)
        {
            var model = new AvatarModel();
            model.Login = User.Identity.Name;
            
            if (ModelState.IsValid)
            {
                if (File != null)
                {
                    model.MimeType = File.ContentType;
                    model.Avatar = new byte[File.ContentLength];
                    File.InputStream.Read(model.Avatar, 0, File.ContentLength);
                }

                model.AddPhoto();
                return RedirectToAction("AboutMe", "Account");
            }
            return View();
            
        }
    }
}
