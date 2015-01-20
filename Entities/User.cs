using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {
        //public Guid User_Id { get { return user_id; } set { user_id = value; } }


        public string Name { get { return name; } set { name = value; } }
        public string Password { get { return pass; } set { pass = value; } }
        public string Email { get { return email; } set { email = value; } }
        public int Role_Id { get { return role_id; } set { role_id = value; } }
        public string Role { get { return role; } set { role = value; } }

        //public Guid user_id;
        private string name;
        private string pass;
        private string email;
        private int role_id;
        private string role;

    }
}
