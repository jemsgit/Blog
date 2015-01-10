using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Search
    {
        public string SearchText { get { return search; } set { search = value; } }

        private string search;
    }
}
