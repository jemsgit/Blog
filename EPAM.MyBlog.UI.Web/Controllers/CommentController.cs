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

        public ActionResult AddComment(Guid Post_ID)
        {
            var list = CommentModel.GetAllComments(Post_ID);
            ViewData["List"] = list;
            return PartialView();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            comment.Author = User.Identity.Name.ToString();
            comment.Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (comment.AddComment())
                {
                    CommentModel.Comments.Add(comment);
                    ViewData["List"] = CommentModel.Comments;
                    ModelState.Clear();
                    return PartialView();
                }
                else
                {
                    ViewData["List"] = CommentModel.Comments;
                    return PartialView(comment);
                }
            }
            else
            {
                ViewData["List"] = CommentModel.Comments;
                return PartialView(comment);
            }
        }
    }
}
