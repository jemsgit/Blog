using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class CommentModel
    {

        public string Author { get { return author; } set { author = value; } }

        [Display(Name = "Ваш комментарий")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        public string Text { get { return text; } set { text = value; } }

        public DateTime Time { get { return time; } set { time = value; } }

        private string author;
        private string text;
        private DateTime time;


        public static explicit operator CommentModel(Entities.Comment Comment)
        {
            return new CommentModel() { Author = Comment.Author, Text = Comment.Text, Time = Comment.Time };
        }

        public static explicit operator Entities.Comment(CommentModel Comment)
        {
            return new Entities.Comment() { Author = Comment.Author, Text = Comment.Text, Time = Comment.Time, ID = Guid.NewGuid() };
        }

        

    }


}