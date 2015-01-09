using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Avatar
    {
        public string Login { get { return login; } set { login = value; } }
        public byte[] Pic { get { return avatar; } set { avatar = value; } }
        public string MimeType { get { return type; } set { type = value; } }

        private string login;
        private byte[] avatar;
        private string type;

    }
}
