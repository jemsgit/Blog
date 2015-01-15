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
        /// <summary>
        /// Модель для информации о пользователе.
        /// </summary>
        /// 

        public string Login { get { return login; } set { login = value; } }

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

        private string sex;
        private Nullable<DateTime> birthday;
        private string name;
        private string info;

        public static explicit operator Entities.UserInfo(UserAboutModel Info)
        {
            return new Entities.UserInfo() { Login = Info.Login, AboutMe = Info.AboutMe, Birthday = Info.Birthday, Name = Info.Name, Sex = Info.Sex };
        }

        public static explicit operator UserAboutModel(Entities.UserInfo Info)
        {
            return new UserAboutModel() { Login = Info.Login, AboutMe = Info.AboutMe, Birthday = Info.Birthday, Name = Info.Name, Sex = Info.Sex };
        }

        public static UserAboutModel GetInfo(string name) 
        {
            var userinfo = (UserAboutModel)GetDAL.dal.GetUserInfo(name);
            return userinfo;
        }


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