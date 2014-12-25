using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Comment
    {
        public string Author { get { return author; } set { author = value; } }
        public string Text { get { return text; } set { text = value; } }
        public DateTime Time { get { return time; } set { time = value; } }
        public Guid ID { get { return id; } set { id = value; } }

        private string author;
        private string text;
        private DateTime time;
        private Guid id;


    }
}
