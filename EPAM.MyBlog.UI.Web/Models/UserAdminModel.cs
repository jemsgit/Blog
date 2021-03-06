﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class UserAdminModel
    {
        /// <summary>
        /// Модель пользователей для админитстратора и модератора
        /// </summary>
        ///

        public string Name { get { return name; } set { name = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Role { get { return role; } set { role = value; } }

        private string name;
        private string email;
        private string role;

        public UserAdminModel()
        { }

        public static explicit operator Entities.User(UserAdminModel User)
        {
            return new Entities.User() { Name = User.Name, Email = User.Email, Role = User.Role };
        }

        public static explicit operator UserAdminModel(Entities.User User)
        {
            return new UserAdminModel() { Name = User.Name, Email = User.Email, Role = User.Role };
        }

        internal static IEnumerable<UserAdminModel> GetAllUsers()
        {
            var list = GetDAL.dal.GetAllUsers().ToList();
            List<UserAdminModel> Users = new List<UserAdminModel>();
            foreach (var item in list)
            {
                Users.Add((UserAdminModel)item);
            }
            return Users;
        }

        internal static void AddUser(List<string> names)
        {
            GetDAL.dal.AddUser(names);
        }

        internal static void AddModer(List<string> names)
        {
            GetDAL.dal.AddModer(names);
        }

        internal static void AddAdmin(List<string> names)
        {
            GetDAL.dal.AddAdmin(names);
        }

        internal static void DeleteUsers(List<string> names)
        {
            GetDAL.dal.DeleteUser(names);
        }
    }
}