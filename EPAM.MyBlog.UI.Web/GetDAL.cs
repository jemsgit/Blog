using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web
{
    public class GetDAL
    {
        public static DAL.DB.DAL dal;

        public static void DAL()
        {
            log4net.Config.XmlConfigurator.Configure();
            dal = new DAL.DB.DAL();
        }
    }
}