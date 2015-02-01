using EPAM.MyBlog.UI.Web.App_Start;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EPAM.MyBlog.UI.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog logger = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            GetDAL.DAL();
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;
            

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        Response.Redirect("~/Home/HttpError404/");
                        break;
                    case 403:
                        // page not found
                        Response.Redirect("~/Home/HttpError403/");
                        break;
                    case 500:
                        // page not found
                        Response.Redirect("~/Home/HttpError500/");
                        break;
                    default:
                        Response.Redirect("~/Home/HttpError/");
                        break;
                }
                // clear error on server
                Server.ClearError();

            }
            else
            {
                int index = exception.StackTrace.IndexOf("System");
                logger.Error(exception.StackTrace.Remove(index - 3));
                logger.Error(exception.Message);
                Response.Redirect("~/Home/HttpError/");
            }
            
        }

    }
}