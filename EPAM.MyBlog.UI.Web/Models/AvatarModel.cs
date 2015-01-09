using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class AvatarModel
    {
        public string Login { get { return login; } set { login = value; } }
        public byte[] Avatar { get { return avatar; } set { avatar = value; } }
        public string MimeType { get { return type; } set { type = value; } }

        private string login;
        private byte[] avatar;
        private string type;

        public static explicit operator Entities.Avatar(AvatarModel Info)
        {
            return new Entities.Avatar() { Login = Info.Login, Pic = Info.Avatar, MimeType = Info.MimeType };
        }

        public static explicit operator AvatarModel(Entities.Avatar Info)
        {
            return new AvatarModel() { Login = Info.Login, Avatar = Info.Pic, MimeType = Info.MimeType };
        }


        internal static AvatarModel GetInfo(string name)
        {
            var avatarinfo = (AvatarModel)GetDAL.dal.GetAvatarInfo(name);
            return avatarinfo;
        }

        internal void AddPhoto()
        {
            var avatar = (Entities.Avatar)this;
            GetDAL.dal.AddAvatar(avatar);
        }
    }
}