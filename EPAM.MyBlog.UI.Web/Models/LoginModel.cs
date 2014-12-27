using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class LoginModel
    {
       /// <summary>
       /// Модель имеет поля Логин,Пароль и Запомнить_меня и такие же переменные
       /// </summary>

        [Display(Name= "Логин")]
        [Required(ErrorMessage="Поле {0} не заполнено")]
        [RegularExpression(@"[\w]{6,20}", ErrorMessage = "{0} должен состоять из анлйских букв и/или цифр")]
        public string Name { get { return name; } set { name = value; } }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [RegularExpression(@"[\w]{6,20}", ErrorMessage = "{0} должен состоять из анлйских букв и/или цифр")]
        public string Password { get { return pass; } set { pass = value; } }

        [Required]
        public bool Remember { get {return remember;} set{remember = value;} }

        private string name;
        private string pass;
        private bool remember;

        /// <summary>
        /// Преобразование из UserModel в User
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>

        public static explicit operator Entities.User(LoginModel User)
        {
            return new Entities.User() { Name = User.Name, Password = GetString(EncodePassword(User.pass))};
        }


        /// <summary>
        /// Проверяем введенные данные. Существует ли юзер с таким логином. Если при выборке пароля из БД ничего не выдало, значит такого юзера нет. Если результат есть,
        /// то сравниваем пароль с введенным и авторизируем пользователя
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>

        internal bool Login(out string Result)
        {
            
            byte[] Pass = GetDAL.dal.CheckLogPas(this.Name);
            if (Pass == null)
            {
                Result = "Пользователя с таким логином не существует!";
                return false;
            }
            else
            {
                if (GetString(Pass) == GetString(EncodePassword(this.Password)))
                {
                    Result = "Welcome";
                    FormsAuthentication.RedirectFromLoginPage(this.Name, this.Remember);
                    return true;
                }
                else
                {
                    Result = "Неверный пароль";
                    return false;
                }
            }
        }

        /// <summary>
        /// LogOut пользователя
        /// </summary>
        
        internal static void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Шфируем пароль в SHA1
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>


        public static byte[] EncodePassword(string pass)
        {
            var s = SHA1.Create();
            return s.ComputeHash(GetBytes(pass));
        }

        /// <summary>
        /// Преобразование строки пароля в набор байтов
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Обратное преобразование. Пароль естественно занимает не все 64 байта в БД, поэтому обратно приходит строка с символами "\0". Копируем все до этих 
        /// символов в новую строку.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>


        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            string s = new string(chars);
            while (s.Contains("\0"))
            {
                string s2 = s.Substring(0, s.IndexOf("\0", 0));
                return s2;
            }
            return s;
        }


    }
}