using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PostText
    {
        public Guid Id { get { return id; } set { id = value; } }
        public string Text { get { return text; } set { text = value; } }
        public string Title { get { return title; } set { title = value; } }

        private string text;
        private string title;
        private Guid id;

        public PostText()
        {
 
        }
    }
}
