using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class UserAboutModel
    {
        public string Login { get { return login; } set { login = value; } }
        public byte[] Avatar { get { return avatar; } set { avatar = value; } }
        public string MimeType { get { return type; } set { type = value; } }
        [Display(Name = "Пол")]
        public string Sex { get { return sex; } set { sex = value; } }
        [Display(Name = "День Рождения")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Birthday { get { return birthday; } set { birthday = value; } }
        [Display(Name = "Имя")]
        public string Name { get { return name; } set { name = value; } }
        [Display(Name = "О себе")]
        public string AboutMe { get { return info; } set { info = value; } }

        private string login;
        private byte[] avatar;
        private string type;
        private string sex;
        private Nullable<DateTime> birthday;
        private string name;
        private string info;

        public static explicit operator Entities.UserInfo(UserAboutModel Info)
        {
            return new Entities.UserInfo() { Login = Info.Login, AboutMe = Info.AboutMe, Avatar = Info.Avatar, Birthday = Info.Birthday, MimeType = Info.MimeType, Name = Info.Name, Sex = Info.Sex };
        }

        public static explicit operator UserAboutModel(Entities.UserInfo Info)
        {
            return new UserAboutModel() { Login = Info.Login, AboutMe = Info.AboutMe, Avatar = Info.Avatar, Birthday = Info.Birthday, MimeType = Info.MimeType, Name = Info.Name, Sex = Info.Sex };
        }


        //public static Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    returnImage.Save("");
        //    return returnImage;
        //}

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


        public static UserAboutModel GetInfo(string name) 
        {
            var userinfo = (UserAboutModel)GetDAL.dal.GetUserInfo(name);
            return userinfo;
        }

        //MimeType = image.ContentType;
        //Data = new byte[image.ContentLength];

        internal bool SetSex()
        {
            var info = (Entities.UserInfo)this;
            return GetDAL.dal.SaveSex(info);
        }

        internal bool SetDate()
        {
            var info = (Entities.UserInfo)this;
            return GetDAL.dal.SaveDate(info);
        }

        internal bool SetName()
        {
            var info = (Entities.UserInfo)this;
            return GetDAL.dal.SaveName(info);
        }

        internal bool SetAbout()
        {
            var info = (Entities.UserInfo)this;
            return GetDAL.dal.SaveAbout(info);
        }
    }
}