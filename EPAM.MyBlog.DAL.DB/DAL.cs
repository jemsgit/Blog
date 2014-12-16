﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EPAM.MyBlog.DAL.DB
{
    public class DAL
    {
        public static string ConnectionString;

        static DAL()
        {

            ConnectionString = ConfigurationManager.ConnectionStrings["Blog"].ConnectionString;
        }

        public Entities.User Login(string Name)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT [Login],[Password],[E_mail],[Role_Id] FROM [dbo].[Users] WHERE [Login] = @Name", con);
                command.Parameters.Add(new SqlParameter("@Name", Name));
                con.Open();
                var reader = command.ExecuteReader();
                        return new Entities.User()
                        {
                            Name = (string)reader["Login"],
                            Password = (string)reader["Password"],
                            Email = (string)reader["E_mail"],
                            Role_Id = (int)reader["Role_Id"]
                        };
            }

        }

        public bool Registration(Entities.User user, int role = 3)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [dbo].[Users] ([Login],[Password],[E_mail],[Role_Id]) VALUES (@Name, CAST( @Password AS BINARY(64)), @Email, @Role_Id)", con);
                command.Parameters.Add(new SqlParameter("@Name", user.Name));
                command.Parameters.Add(new SqlParameter("@Password", user.Password));
                command.Parameters.Add(new SqlParameter("@Email", user.Email));
                command.Parameters.Add(new SqlParameter("@Role_Id", role));
                con.Open();
                int num = command.ExecuteNonQuery();
                if (num == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public byte[] CheckLogPas(string Name) 
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT [Password] FROM [dbo].[Users] WHERE [Login] = @Name", con);
                command.Parameters.Add(new SqlParameter("@Name", Name));
                con.Open();
                var reader = command.ExecuteReader();
                byte[] Pass = null;
                int count = 0;
                while (reader.Read())
                {
                    count++;
                    Pass = (byte[])reader["Password"];
                }
                if(count<1)
                {
                    return Pass = null;
                }
                else
                {
                    return Pass;
                }

            }
        }

        public bool CheckMail(string Email)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) AS COUNT FROM [dbo].[Users] WHERE [E_mail] = @Email", con);
                command.Parameters.Add(new SqlParameter("@Email", Email));
                con.Open();
                int count = 0;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = (int)reader["COUNT"];
                }
                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AddPost(Entities.PostText post, string login)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO dbo.Posts (User_Name, Post_Id, Post_Title, Post_Text) VALUES (@Login,CAST(@ID AS NVARCHAR(36)), @Title, @Text)", con);
                command.Parameters.Add(new SqlParameter("@Login", login));
                command.Parameters.Add(new SqlParameter("@ID", post.Id));
                command.Parameters.Add(new SqlParameter("@Title", post.Title));
                command.Parameters.Add(new SqlParameter("@Text", post.Text));
                con.Open();
                int num = command.ExecuteNonQuery();

                if (num == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    }
}
