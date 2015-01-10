using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class SearchModel
    {
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
            if(query.Substring(0,1) == "#")
            {
                var result = GetDAL.dal.GetResultOfSearchTag(this.SearchText).ToList();
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
    }
}