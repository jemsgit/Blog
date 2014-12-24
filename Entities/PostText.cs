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
        public string Author { get { return author; } set { author = value; } }
        public DateTime Time { get { return time; } set { time = value; } }

        private string text;
        private string title;
        private string author;
        private Guid id;
        private DateTime time;

        public PostText()
        {
 
        }
    }
}
