using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class PresentPostModel
    {
        /// <summary>
        /// Создана модель для того чтобы не тащить большой объем данных с текстом поста. Только заголовки и Id.
        /// </summary>

        public static List<PresentPostModel> PostsTitle;

        public Guid Id { get { return id; } set { id = value; } }
        public string Title { get { return title; } set { title = value; } }

        private string title;
        private Guid id;

        public PresentPostModel()
        { }

        public static explicit operator PresentPostModel(Entities.PresentPost Post)
        {
            return new PresentPostModel() { Id = Post.Id, Title = Post.Title };
        }

        internal static IEnumerable<PresentPostModel> GetAllPostsTitle(string user)
        {
            var list = GetDAL.dal.GetAllPostsTitle(user).ToList();
            List<PresentPostModel> TitleList = new List<PresentPostModel>();
            foreach (var item in list)
            {
                TitleList.Add((PresentPostModel)item);
            }
            return TitleList;
        }

        internal static IEnumerable<PresentPostModel> GetAllPostsTitle()
        {
            var list = GetDAL.dal.GetAllPostsTitle().ToList();
            List<PresentPostModel> TitleList = new List<PresentPostModel>();
            foreach (var item in list)
            {
                TitleList.Add((PresentPostModel)item);
            }
            return TitleList;
        }


        internal static IEnumerable<PresentPostModel> GetAllFavorite(string name)
        {
            var list = GetDAL.dal.GetAllFavorite(name).ToList();
            List<PresentPostModel> TitleList = new List<PresentPostModel>();
            foreach (var item in list)
            {
                TitleList.Add((PresentPostModel)item);
            }
            return TitleList;
        }
    }
}