using EPAM.MyBlog.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        public ActionResult AddComment()
        {
            return PartialView();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            comment.Author = User.Identity.Name.ToString();
            comment.Time = DateTime.Now;

            return PartialView();
        }
    }
}
