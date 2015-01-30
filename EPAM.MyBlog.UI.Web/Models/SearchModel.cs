using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class SearchModel
    {
        /// <summary>
        /// Модель для поиска по тегам и тексту
        /// </summary>
        /// 
        [Required(ErrorMessage = "Поле не заполнено")]
        public string SearchText { get { return search; } set { search = value; } }

        private string search;

        public static explicit operator SearchModel(Entities.Search search)
        {
            return new SearchModel() { SearchText = search.SearchText };
        }

        public static explicit operator Entities.Search(SearchModel search)
        {
            return new Entities.Search() { SearchText = search.SearchText };
        }

        internal IEnumerable<PresentPostModel> GetResult()
        {
            string query = SearchText.Trim();
            if (!String.IsNullOrEmpty(query))
            {
                if (query.Substring(0, 1) == "#")
                {
                    var result = GetDAL.dal.GetResultOfSearchTag(this.SearchText.Substring(1, SearchText.Length - 1)).ToList();
                    List<PresentPostModel> TitleList = new List<PresentPostModel>();
                    foreach (var item in result)
                    {
                        TitleList.Add((PresentPostModel)item);
                    }
                    return TitleList;
                }
                else
                {
                    var result = GetDAL.dal.GetResultOfSearch(this.SearchText).ToList();
                    List<PresentPostModel> TitleList = new List<PresentPostModel>();
                    foreach (var item in result)
                    {
                        TitleList.Add((PresentPostModel)item);
                    }
                    return TitleList;
                }
            }
            else
            {
                return new List<PresentPostModel>();
            }
        }

        public static IEnumerable<string> GetTopTags()
        {
            return GetDAL.dal.GetTopTags();
        }

    }
}