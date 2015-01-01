using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class CommentModel
    {

        public static List<CommentModel> Comments;

        public string Author { get { return author; } set { author = value; } }

        [Display(Name = "Ваш комментарий")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        public string Text { get { return text; } set { text = value; } }

        public DateTime Time { get { return time; } set { time = value; } }

        public Guid ID { get { return id; } set { id = value; } }
        public Guid Post_ID { get { return post_id; } set { post_id = value; } }

        private string author;
        private string text;
        private DateTime time;
        private Guid id;
        private Guid post_id;

        public static explicit operator CommentModel(Entities.Comment Comment)
        {
            return new CommentModel() { Author = Comment.Author, Text = Comment.Text, Time = Comment.Time, ID = Comment.ID, Post_ID = Comment.Post_ID };
        }

        public static explicit operator Entities.Comment(CommentModel Comment)
        {
            return new Entities.Comment() { Author = Comment.Author, Text = Comment.Text, Time = Comment.Time, ID = Guid.NewGuid(), Post_ID = Comment.Post_ID};
        }

        public static IEnumerable<CommentModel> GetAllComments(Guid Post_ID)
        {
            var comments = new List<Entities.Comment>(GetDAL.dal.GetAllComments(Post_ID).ToList());
            Comments = new List<CommentModel>();
            foreach (var item in comments)
            {
                Comments.Add((CommentModel)item);
            }
            return Comments;
        }

        internal bool AddComment()
        {
            return GetDAL.dal.AddComment((Entities.Comment)this);
        }

        internal static object GetAllComments(string name)
        {
            var comments = new List<Entities.Comment>(GetDAL.dal.GetAllCommentsOfUser(name).ToList());
            Comments = new List<CommentModel>();
            foreach (var item in comments)
            {
                Comments.Add((CommentModel)item);
            }
            return Comments;
            
        }

        internal static bool Delete(Guid id)
        {
            return GetDAL.dal.DeleteCommentById(id);
        }
    }


}