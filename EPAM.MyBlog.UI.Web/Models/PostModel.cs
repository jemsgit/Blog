using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class PostModel
    {
        /// <summary>
        /// Поля для ID, Заголовка поста и текста поста.
        /// </summary>

        public static List<PostModel> Posts;

        public Guid Id { get { return id; } set { id = value; } }

        [Display(Name = "Текст записи")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        public string Text { get { return text; } set { text = value; } }

        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [StringLength(50,MinimumLength=1,ErrorMessage="Поле {0} должно быть от {2} до {1} символов")]
        public string Title { get { return title; } set { title = value; } }

        private string text;
        private string title;
        private Guid id;


        public static explicit operator Entities.PostText(PostModel Post)
        {
            return new Entities.PostText() {Id = Guid.NewGuid(), Text = Post.Text, Title = Post.Title};
        }

        public static explicit operator PostModel(Entities.PostText Post)
        {
            return new PostModel() { Id = Post.Id, Text = Post.Text, Title = Post.Title };
        }



        internal bool AddPost(string login)
        {
            var post = (Entities.PostText)this;
            if (GetDAL.dal.AddPost(post, login))
            {
                return true;
            }
            else 
            {
                return false;
            }
        }


    }
}