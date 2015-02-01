using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Text { get { return text; } set { text = value; } }

        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [StringLength(50,MinimumLength=1,ErrorMessage="Поле {0} должно быть от {2} до {1} символов")]
        public string Title { get { return title; } set { title = value; } }

        public string Author { get { return author; } set { author = value; } }
        public DateTime Time { get { return time; } set { time = value; } }

        [Display(Name = "Теги")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Поле {0} должно быть от {2} до {1} символов")]
        public string Tags { get { return tags; } set { tags = value; } }

        private string text;
        private string title;
        private string author;
        private Guid id;
        private DateTime time;
        private string tags;

        public static explicit operator Entities.PostText(PostModel Post)
        {
            return new Entities.PostText() { Id = Post.Id, Text = Post.Text, Title = Post.Title, Author = Post.Author, Time = Post.Time, Tags = Post.Tags };
        }

        public static explicit operator PostModel(Entities.PostText Post)
        {
            return new PostModel() { Id = Post.Id, Text = Post.Text, Title = Post.Title, Author = Post.Author, Time = Post.Time, Tags = Post.Tags };
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



        internal static PostModel GetPostById(Guid Id)
        {
            var post = GetDAL.dal.GetPostById(Id);
            return (PostModel)post;
        }

        internal bool EditPost()
        {
            var post = (Entities.PostText)this;
            if (GetDAL.dal.EditPostById(post))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool Delete(Guid id)
        {
            return GetDAL.dal.DeletePostById(id);
        }

        internal static bool CheckFavorite(string name, Guid Id)
        {
            return GetDAL.dal.CheckFavorite(name, Id);
        }

        internal static bool AddFavorite(string name, Guid Id)
        {
            return GetDAL.dal.AddFavorite(name, Id);
        }

        internal static bool DeleteFavorite(string name, Guid id)
        {
            return GetDAL.dal.DeletePostFromFav(name, id);
        }
    }
}