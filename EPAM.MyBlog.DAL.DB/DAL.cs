﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using log4net;

namespace EPAM.MyBlog.DAL.DB
{
    public class DAL
    {

        private static ILog logger = LogManager.GetLogger(typeof(DAL));

        public static string ConnectionString;

        static DAL()
        {

            ConnectionString = ConfigurationManager.ConnectionStrings["Blog"].ConnectionString;
        }

        #region Account


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
                if (num == 1 & AddUserInfo(user.Name))
                {
                    logger.Info("DB: Добавлен новый пользователь: " + user.Name);
                    return true;
                }
                else {
                    logger.Error("DB: Ошибка добавления пользователя: " + user.Name);
                    return false;
                }
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
                if (count < 1)
                {
                    logger.Info("DB: Отсутствует пароль для пользователя: " + Name);
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
                    logger.Info("DB: Существует пользователь с данным e-mail адресом: " + Email);
                    return true;
                }
                else
                {
                    logger.Info("DB: Не существует пользвателя с данным e-mail адресом: " + Email);
                    return false;
                }
            }
        }

        public bool DeleteUser(string name)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("Delete FROM dbo.Users WHERE Login = @Name", con);
                command.Parameters.Add(new SqlParameter("@Name", name));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Пользователь удален: " + name);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка удаления пользователя: " + name);
                    return false;
                }
            }
        }

        public void SaveReason(string reason)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO dbo.Reasons VALUES (@Reason)", con);
                command.Parameters.Add(new SqlParameter("@Reason", reason));
                con.Open();
                command.ExecuteNonQuery();
            }
        }


        #endregion

        #region Posts


        public bool AddPost(Entities.PostText post, string login)
        {
            string[] split = post.Tags.Trim().Split(' ');
            for (int i = 0; i < split.Length; i++)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("INSERT INTO dbo.Tags (Post_Id, Tag) VALUES (CAST(@ID AS NVARCHAR(36)), @Tag)", con);

                    command.Parameters.Add(new SqlParameter("@ID", post.Id));
                    command.Parameters.Add(new SqlParameter("@Tag", split[i]));
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }



            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO dbo.Posts (User_Name, Post_Id, Post_Title, Post_Text, Time) VALUES (@Login,CAST(@ID AS NVARCHAR(36)), @Title, @Text, @Time)", con);
                command.Parameters.Add(new SqlParameter("@Login", login));
                command.Parameters.Add(new SqlParameter("@ID", post.Id));
                command.Parameters.Add(new SqlParameter("@Title", post.Title));
                command.Parameters.Add(new SqlParameter("@Text", post.Text));
                command.Parameters.Add(new SqlParameter("@Time", post.Time));
                con.Open();
                int num = command.ExecuteNonQuery();

                if (num == 1)
                {
                    logger.Info("DB: Добавлен пост: " + post.Id + " пользователя: " + post.Author);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка добавления поста: " + post.Id + " пользователя: " + post.Author);
                    return false;
                }
            }

        }

        public IEnumerable<Entities.PresentPost> GetAllPostsTitle()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Post_Title, Post_Id FROM dbo.Posts ORDER BY Time DESC", con);
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Entities.PresentPost()
                    {
                        Id = new Guid((string)reader["Post_Id"]),
                        Title = (string)reader["Post_Title"]
                    };

                }
            }

        }

        public IEnumerable<Entities.PresentPost> GetAllPostsTitle(string user)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Post_Title, Post_Id FROM dbo.Posts WHERE User_Name = @Name ORDER BY Time DESC", con);
                command.Parameters.Add(new SqlParameter("@Name", user));
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Entities.PresentPost()
                    {
                        Id = new Guid((string)reader["Post_Id"]),
                        Title = (string)reader["Post_Title"]
                    };

                }
            }

        }

        public Entities.PostText GetPostById(Guid Id)
        {
            StringBuilder s = new StringBuilder();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Post_Id, Post_Title, Post_Text, User_Name, Time FROM dbo.Posts WHERE Post_Id = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", Id));
                con.Open();
                Entities.PostText post = new Entities.PostText() { Id = Id };
                int count = 0;
                var reader = command.ExecuteReader();
                string post_id = "";

                while (reader.Read())
                {
                    post_id = (string)reader["Post_Id"];
                    post.Title = (string)reader["Post_Title"];
                    post.Text = (string)reader["Post_Text"];
                    post.Author = (string)reader["User_Name"];
                    post.Time = (DateTime)reader["Time"];
                    count++;
                }
                if (count < 0)
                {
                    return post = null;
                }
                else
                {
                    reader.Close();
                    using (SqlConnection con2 = new SqlConnection(ConnectionString))
                    {
                        SqlCommand command2 = new SqlCommand("SELECT Tag FROM dbo.Tags WHERE Post_Id = @Id", con);
                        command2.Parameters.Add(new SqlParameter("@Id", post_id));
                        con2.Open();
                        var reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            s.Append((string)reader2["Tag"]);
                            s.Append(" ");
                        }
                    }
                    post.Tags = s.ToString();
                    return post;
                }

            }
        }

        public bool EditPostById(Entities.PostText post)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE dbo.Posts SET Post_Title = @Title, Post_Text = @Text, Time = @Time WHERE Post_Id = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", post.Id));
                command.Parameters.Add(new SqlParameter("@Text", post.Text));
                command.Parameters.Add(new SqlParameter("@Title", post.Title));
                command.Parameters.Add(new SqlParameter("@Time", post.Time));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    using (SqlConnection con2 = new SqlConnection(ConnectionString))
                    {
                        SqlCommand command2 = new SqlCommand("DELETE FROM dbo.Tags WHERE Post_Id = @ID", con);

                        command2.Parameters.Add(new SqlParameter("@ID", post.Id));
                        con2.Open();
                        command2.ExecuteNonQuery();
                        con2.Close();
                    }

                    string[] split = post.Tags.Trim().Split(' ');
                    for (int i = 0; i < split.Length; i++)
                    {
                        using (SqlConnection con2 = new SqlConnection(ConnectionString))
                        {
                            SqlCommand command2 = new SqlCommand("INSERT INTO dbo.Tags (Post_Id, Tag) VALUES (CAST(@ID AS NVARCHAR(36)), @Tag)", con);

                            command2.Parameters.Add(new SqlParameter("@ID", post.Id));
                            command2.Parameters.Add(new SqlParameter("@Tag", split[i]));
                            con2.Open();
                            command2.ExecuteNonQuery();
                            con2.Close();
                        }
                    }

                    logger.Info("DB: Изменен пост: " + post.Id + " пользователя: " + post.Author);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка изменения поста: " + post.Id + " пользователя: " + post.Author);
                    return false;
                }
            }
        }

        public bool DeletePostById(Guid Id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("Delete FROM dbo.Posts WHERE Post_Id = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", Id));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    using (SqlConnection con2 = new SqlConnection(ConnectionString))
                    {
                        SqlCommand command2 = new SqlCommand("DELETE FROM dbo.Tags WHERE Post_Id = @ID", con);

                        command2.Parameters.Add(new SqlParameter("@ID", Id));
                        con2.Open();
                        command2.ExecuteNonQuery();
                        con2.Close();
                    }

                    logger.Info("DB: Удален пост: " + Id);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка удаления поста: " + Id);
                    return false;
                }
            }

        }

        public bool CheckFavorite(string name, Guid Id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) AS C FROM Blog.dbo.Favorite WHERE Post_Id = @Id AND Login=@Login", con);
                command.Parameters.Add(new SqlParameter("@Id", Id));
                command.Parameters.Add(new SqlParameter("@Login", name));
                con.Open();
                var reader = command.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count = (int)reader["C"];
                }
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AddFavorite(string name, Guid Id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Blog.dbo.Favorite (Blog.dbo.Favorite.Login, Blog.dbo.Favorite.Post_Id) VALUES (@Login, @Id)", con);
                command.Parameters.Add(new SqlParameter("@Id", Id));
                command.Parameters.Add(new SqlParameter("@Login", name));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Пост добавлен в Избранное: " + Id + " пользователя: " + name);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка добавления поста в избранного: " + Id + " пользователя: " + name);
                    return false;
                }
            }
        }

        public IEnumerable<Entities.PresentPost> GetAllFavorite(string name)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Posts.Post_Title, Posts.Post_Id FROM dbo.Posts INNER JOIN dbo.Favorite ON Favorite.Post_Id = Posts.Post_Id WHERE Login = @Login ORDER BY Time DESC", con);
                command.Parameters.Add(new SqlParameter("@Login", name));
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Entities.PresentPost()
                    {
                        Id = new Guid((string)reader["Post_Id"]),
                        Title = (string)reader["Post_Title"]
                    };

                }
            }
        }

        public bool DeletePostFromFav(string name, Guid id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM dbo.Favorite WHERE Post_Id = @Id AND Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Id", id));
                command.Parameters.Add(new SqlParameter("@Login", name));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Удален пост из избранного: " + id + " пользователя: " + name);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка удаления поста из избранного: " + id + " пользователя: " + name);
                    return false;
                }
            }
        }


        #endregion

        #region Roles

        public int GetRoleForUser(string username)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Role_Id FROM dbo.Users WHERE Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Login", username));
                con.Open();
                int role = 0;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    role = (int)reader["Role_Id"];
                }
                return role;
            }
        }

        #endregion

        #region Comments


        public bool AddComment(Entities.Comment comment)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO dbo.Comments (User_Name, Post_ID, Comment_ID, Text, Time) VALUES (@Author,CAST(@Post_ID AS NVARCHAR(36)), CAST(@Comment_ID AS NVARCHAR(36)), @Text, @Time)", con);
                command.Parameters.Add(new SqlParameter("@Author", comment.Author));
                command.Parameters.Add(new SqlParameter("@Post_ID", comment.Post_ID));
                command.Parameters.Add(new SqlParameter("@Comment_ID", comment.ID));
                command.Parameters.Add(new SqlParameter("@Text", comment.Text));
                command.Parameters.Add(new SqlParameter("@Time", comment.Time));
                con.Open();
                int num = command.ExecuteNonQuery();
                if (num == 1)
                {
                    logger.Info("DB: Добавлен комментарий: " + comment.ID + " к посту: " + comment.Post_ID);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка добавления комментария: " + comment.ID + " к посту: " + comment.Post_ID);
                    return false;
                }
            }
        }

        public IEnumerable<Entities.Comment> GetAllComments(Guid Post_ID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Post_ID, User_Name, Comment_ID, Text, Time FROM dbo.Comments  WHERE Post_ID = CAST(@Post_ID AS NVARCHAR(36)) ORDER BY Time DESC", con);
                command.Parameters.Add(new SqlParameter("@Post_ID", Post_ID));
                con.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Entities.Comment()
                    {
                        Post_ID = new Guid((string)reader["Post_Id"]),
                        Author = (string)reader["User_Name"],
                        ID = new Guid((string)reader["Comment_ID"]),
                        Text = (string)reader["Text"],
                        Time = (DateTime)reader["Time"]

                    };

                }
            }

        }

        public IEnumerable<Entities.Comment> GetAllCommentsOfUser(string name)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT  Post_Id, Comment_ID, Text, Time FROM dbo.Comments  WHERE User_Name = @Name ORDER BY Time DESC", con);
                command.Parameters.Add(new SqlParameter("@Name", name));
                con.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Entities.Comment()
                    {
                        Post_ID = new Guid((string)reader["Post_Id"]),
                        ID = new Guid((string)reader["Comment_ID"]),
                        Text = (string)reader["Text"],
                        Time = (DateTime)reader["Time"]
                    };

                }
            }
        }

        public bool DeleteCommentById(Guid id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string text = "НЛО прилетело и оставило эту запись";
                SqlCommand command = new SqlCommand("UPDATE dbo.Comments SET Text = @Text WHERE dbo.Comments.Comment_ID = @Id", con);
                command.Parameters.Add(new SqlParameter("@Text", text));
                command.Parameters.Add(new SqlParameter("@Id", id));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Удален комментарий: " + id);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка удаления комментария: " + id);
                    return false;
                }
            }
        }


        #endregion

        #region Users


        public IEnumerable<Entities.User> GetAllUsers()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Login, E_mail, Title FROM dbo.Users, dbo.Roles WHERE Users.Role_Id = Roles.Id ORDER BY Login", con);
                con.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Entities.User()
                    {
                        Name = (string)reader["Login"],
                        Email = (string)reader["E_mail"],
                        Role = (string)reader["Title"],
                    };
                }
            }
        }

        public bool AddUserInfo(string login) 
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO dbo.UserAbout (Login) VALUES (@Login)", con);
                command.Parameters.Add(new SqlParameter("@Login", login));
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


        public Entities.UserInfo GetUserInfo(string name)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Sex, Birthday, True_Name, About FROM dbo.UserAbout RIGHT JOIN Users ON Users.Login = UserAbout.Login Where Users.Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Login", name));
                con.Open();
                Entities.UserInfo info = new Entities.UserInfo() { Login = name };
                int count = 0;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (DBNull.Value.Equals(reader["Sex"]))
                    {
                        info.Sex = null;
                    }
                    else
                    {
                        info.Sex = (string)reader["Sex"];
                    }
                    if (DBNull.Value.Equals(reader["Birthday"]))
                    {
                        info.Birthday = null;
                    }
                    else
                    {
                        info.Birthday = (DateTime)reader["Birthday"];
                    }
                    if (DBNull.Value.Equals(reader["True_Name"]))
                    {
                        info.Name = null;
                    }
                    else
                    {
                        info.Name = (string)reader["True_Name"];
                    }
                    if (DBNull.Value.Equals(reader["About"]))
                    {
                        info.AboutMe = null;
                    }
                    else
                    {
                        info.AboutMe = (string)reader["About"];
                    }
                    count++;
                }
                if (count < 0)
                {
                    return info = null;
                }
                else
                {
                    return info;
                }
            }
        }

        public bool SaveSex(Entities.UserInfo info)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE dbo.UserAbout SET Sex = @Sex WHERE Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Sex", info.Sex));
                command.Parameters.Add(new SqlParameter("@Login", info.Login));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Данные о поле пользователя сохранены: " + info.Login);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка записи данных о поле пользователя: " + info.Login);
                    return false;
                }
            }
        }

        public bool SaveDate(Entities.UserInfo info)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand command = new SqlCommand("UPDATE dbo.UserAbout SET Birthday = @Birthday WHERE Login = @Login", con);
                if (info.Birthday == null)
                {
                    command.Parameters.Add(new SqlParameter("@Birthday", DBNull.Value));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@Birthday", info.Birthday));
                }
                command.Parameters.Add(new SqlParameter("@Login", info.Login));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Данные о дате рождения пользователя сохранены: " + info.Login);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка записи данных о дате рождения пользователя: " + info.Login);
                    return false;
                }
            }
        }

        public bool SaveName(Entities.UserInfo info)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE dbo.UserAbout SET True_Name = @Name WHERE Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Name", info.Name));
                command.Parameters.Add(new SqlParameter("@Login", info.Login));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Данные об имени пользователя сохранены: " + info.Login);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка записи данных об имени пользователя: " + info.Login);
                    return false;
                }
            }
        }

        public bool SaveAbout(Entities.UserInfo info)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE dbo.UserAbout SET About = @About WHERE Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@About", info.AboutMe));
                command.Parameters.Add(new SqlParameter("@Login", info.Login));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Данные о пользователе сохранены: " + info.Login);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка записи данных о пользователе: " + info.Login);
                    return false;
                }
            }
        }

        public Entities.Avatar GetAvatarInfo(string name)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Avatar, Type FROM dbo.UserAbout RIGHT JOIN Users ON Users.Login = UserAbout.Login Where Users.Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Login", name));
                con.Open();
                Entities.Avatar info = new Entities.Avatar() { Login = name };
                int count = 0;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (DBNull.Value.Equals(reader["Avatar"]))
                    {
                        info.Pic = null;
                    }
                    else
                    {
                        info.Pic = (byte[])reader["Avatar"];
                    }

                    if (DBNull.Value.Equals(reader["Type"]))
                    {
                        info.MimeType = null;
                    }
                    else
                    {
                        info.MimeType = (string)reader["Type"];
                    }
                    count++;
                }
                if (count < 0)
                {
                    return info = null;
                }
                else
                {
                    return info;
                }
            }
        }

        public bool AddAvatar(Entities.Avatar avatar)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE dbo.UserAbout SET Avatar = @Pic, Type = @Type WHERE Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Pic", avatar.Pic));
                command.Parameters.Add(new SqlParameter("@Type", avatar.MimeType));
                command.Parameters.Add(new SqlParameter("@Login", avatar.Login));
                con.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    logger.Info("DB: Аватр добавлен: " + avatar.Login);
                    return true;
                }
                else
                {
                    logger.Error("DB: Ошибка добавления аватара: " + avatar.Login);
                    return false;
                }
            }
        }


        #endregion

        #region Tags


        public IEnumerable<Entities.PresentPost> GetResultOfSearchTag(string p)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT [Blog].dbo.Posts.Post_Id,[Blog].dbo.Posts.Post_Title FROM [Blog].dbo.Posts INNER JOIN [Blog].dbo.Tags ON [Blog].dbo.Tags.Post_Id = [Blog].dbo.Posts.Post_Id WHERE Tag LIKE @Tag ORDER BY [Blog].dbo.Posts.Time DESC", con);
                command.Parameters.Add(new SqlParameter("@Tag", p));
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Entities.PresentPost()
                    {
                        Id = new Guid((string)reader["Post_Id"]),
                        Title = (string)reader["Post_Title"]
                    };

                }
            }
        }

        public IEnumerable<Entities.PresentPost> GetResultOfSearch(string p)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT [Post_Id], [Post_Title] FROM [Blog].dbo.Posts WHERE Post_Text LIKE @Text OR Post_Title LIKE @Text ORDER BY [Blog].dbo.Posts.Time DESC", con);
                command.Parameters.Add(new SqlParameter("@Text", p));
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Entities.PresentPost()
                    {
                        Id = new Guid((string)reader["Post_Id"]),
                        Title = (string)reader["Post_Title"]
                    };

                }
            }
        }

        public IEnumerable<string> GetTopTags()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT TOP 10 [Blog].dbo.Tags.Tag, COUNT([Blog].dbo.Tags.Tag) as count_tag FROM [Blog].dbo.Tags GROUP BY [Blog].dbo.Tags.Tag ORDER BY count_tag DESC", con);
                con.Open();
                var reader = command.ExecuteReader();
                List<string> Tags = new List<string>();
                while (reader.Read())
                {
                    Tags.Add((string)reader["Tag"]);
                }
                return Tags;
            }
        }


        #endregion

        #region Admin


        public void AddUser(List<string> names)
        {
            foreach (var item in names)
            {

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("UPDATE dbo.Users SET Role_Id = '3' WHERE Login = @Login", con);
                    command.Parameters.Add(new SqlParameter("@Login", item));
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

            }
        }

        public void AddModer(List<string> names)
        {
            foreach (var item in names)
            {

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("UPDATE dbo.Users SET Role_Id = '2' WHERE Login = @Login", con);
                    command.Parameters.Add(new SqlParameter("@Login", item));
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

            }
        }

        public void AddAdmin(List<string> names)
        {
            foreach (var item in names)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("UPDATE dbo.Users SET Role_Id = '1' WHERE Login = @Login", con);
                    command.Parameters.Add(new SqlParameter("@Login", item));
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void DeleteUser(List<string> names)
        {
            foreach (var item in names)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("Delete FROM dbo.Users WHERE Login = @Login", con);
                    command.Parameters.Add(new SqlParameter("@Login", item));
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


        #endregion


    } 
}
