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
        
        public ActionResult AddComment(Guid Post_Id)
        {
            var list = CommentModel.GetAllComments(Post_Id);
            ViewData["List"] = list;
            ViewData["ID"] = Post_Id; 
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
                    return RedirectToAction("Posts", "Post", new { Id = comment.Post_ID });
                }
                else
                {
                    ViewData["List"] = CommentModel.Comments;
                    return RedirectToAction("Posts", "Post", new { Id = comment.Post_ID });
                }
            }
            else
            {
                ViewData["List"] = CommentModel.Comments;
                return RedirectToAction("Posts", "Post", new { Id = comment.Post_ID });
            }
        }
    }
}
