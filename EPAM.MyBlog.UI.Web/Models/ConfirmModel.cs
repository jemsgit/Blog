using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class ConfirmModel
    {
        public bool Confirm { get{return confirm;} set{confirm = value;} }

        private bool confirm;
    }
}