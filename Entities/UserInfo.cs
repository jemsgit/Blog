using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserInfo
    {
        public string Login { get { return login; } set { login = value; } }
        public byte[] Avatar { get { return avatar; } set { avatar = value; } }
        public string MimeType { get { return type; } set { type = value; } }
        public string Sex { get { return sex; } set { sex = value; } }
        public Nullable<DateTime> Birthday { get { return birthday; } set { birthday = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string AboutMe { get { return info; } set { info = value; } }

        private string login;
        private byte[] avatar;
        private string type;
        private string sex;
        private Nullable<DateTime> birthday;
        private string name;
        private string info;

    }
}
