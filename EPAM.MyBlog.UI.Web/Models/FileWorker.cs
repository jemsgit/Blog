using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class FileWorker
    {
        public void Upload(HttpPostedFile file, string name)
        {
            //string path = @"~\" + name + @"\";
            //if(!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //path += @"myAvatar";
            //if(File.Exists(path))
            //{
            //    File.Delete(path);
            //}
            //file.SaveAs(path);
        }
    }
}