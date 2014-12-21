using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class MyRoleProvider: RoleProvider
    {

        public override string[] GetAllRoles()
        {
            return GetDAL.dal.GetAllRoles();
        }

        public override string[] GetRolesForUser(string username)
        {
            int role = GetDAL.dal.GetRoleForUser(username);
            switch (role)
            {
                case 1:
                    return new[] { "Admin", "Moder", "User" };
                case 2:
                    return new[] { "Moder", "User" };
                case 3:
                    return new[] { "User" };
                default:
                    return new[] { "User" };
            }
        }

        #region Not Used

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }


        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

#endregion
    }
}