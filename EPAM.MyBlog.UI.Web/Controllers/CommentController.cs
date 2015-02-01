using EPAM.MyBlog.UI.Web.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPAM.MyBlog.UI.Web.Controllers
{
    
    public class CommentController : Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(CommentController));
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
                    logger.Info("Добавлен комментарий пользователя: " + comment.Author + " к посту: " + comment.Post_ID);
                    if (Request.IsAjaxRequest())
                        return RedirectToAction("AddComment", "Comment", new { Post_Id = comment.Post_ID });
                    return RedirectToAction("Posts", "Post", new { Id = comment.Post_ID });
                }
                else
                {
                    logger.Error("ошибка добавления комментария пользователя: " + comment.Author + " к посту: " + comment.Post_ID);
                    ViewData["List"] = CommentModel.Comments;
                    if (Request.IsAjaxRequest())
                        return RedirectToAction("AddComment", "Comment", new { Post_Id = comment.Post_ID });
                    return RedirectToAction("Posts", "Post", new { Id = comment.Post_ID });
                }
            }
            else
            {
                ViewData["List"] = CommentModel.Comments;
                if (Request.IsAjaxRequest())
                    return RedirectToAction("AddComment", "Comment", new { Post_Id = comment.Post_ID });
                return RedirectToAction("Posts", "Post", new { Id = comment.Post_ID });
            }
        }
    }
}
