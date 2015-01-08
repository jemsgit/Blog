using System;
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region Posts
        public bool AddPost(Entities.PostText post, string login)
        {
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
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public IEnumerable<Entities.PresentPost> GetAllPostsTitle(string user)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Post_Title, Post_Id FROM dbo.Posts WHERE User_Name = @Name", con);
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
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Post_Title, Post_Text, User_Name, Time FROM dbo.Posts WHERE Post_Id = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", Id));
                con.Open();
                Entities.PostText post = new Entities.PostText() {Id = Id };
                int count = 0;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
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
                    return true;
                }
                else
                {
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
 
        }

        #endregion

        #region Roles
        public string[] GetAllRoles()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Title FROM dbo.Roles", con);
                con.Open();
                string[] roles = new string[command.ExecuteNonQuery()];
                int count = 0;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    roles[count] = (string)reader["Title"];
                    count++;
                }
                return roles;
            }
            
        }

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
                    return true;
                }
                else
                {
                    return false;
                }
            }  
        }

        public IEnumerable<Entities.Comment> GetAllComments(Guid Post_ID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Post_ID, User_Name, Comment_ID, Text, Time FROM dbo.Comments  WHERE Post_ID = CAST(@Post_ID AS NVARCHAR(36)) ORDER BY Time", con);
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
                SqlCommand command = new SqlCommand("SELECT  Post_Id, Comment_ID, Text, Time FROM dbo.Comments  WHERE User_Name = @Name ORDER BY Time", con);
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
                SqlCommand command = new SqlCommand("UPDATE dbo.Comments SET Text = @Text", con);
                command.Parameters.Add(new SqlParameter("@Text", text));
                con.Open();
                int count = command.ExecuteNonQuery();
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

        #endregion

        #region Users

        public IEnumerable<Entities.User> GetAllUsers()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Login, E_mail, Title FROM dbo.Users, dbo.Roles WHERE Users.Role_Id = Roles.Id", con);
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

        public Entities.UserInfo GetUserInfo(string name)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Avatar, Type, Sex, Birthday, True_Name, About FROM dbo.UserAbout RIGHT JOIN Users ON Users.Login = UserAbout.Login Where Users.Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Login", name));
                con.Open();
                Entities.UserInfo info = new Entities.UserInfo() { Login = name };
                int count = 0;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (DBNull.Value.Equals(reader["Avatar"]))
                    {
                        info.Avatar = null;
                    }
                    else
                    {
                        info.Avatar = (byte[])reader["Avatar"];
                    }

                    if (DBNull.Value.Equals(reader["Type"]))
                    {
                        info.MimeType = null;
                    }
                    else
                    {
                        info.MimeType = (string)reader["Type"];
                    }
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

        #endregion



        public void SaveReason(string reason)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO dbo.Reasons VALUES (@Reason)", con);
                command.Parameters.Add(new SqlParameter("@Reason", reason));
                con.Open();
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
                    return true;
                }
                else
                {
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
                    return true;
                }
                else
                {
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
                    return true;
                }
                else
                {
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
