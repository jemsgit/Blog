using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PresentPost
    {
        public Guid Id { get { return id; } set { id = value; } }
        public string Title { get { return title; } set { title = value; } }

        private string title;
        private Guid id;



    }
}
