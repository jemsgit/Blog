using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class RegModel
    {
        /// <summary>
        /// Включаем логгер
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(RegModel));

        /// <summary>
        /// Поля и переменные: Логин, пароль,подтверждение пароля, электронный адрес. 
        /// </summary>

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [StringLength(20,MinimumLength=6,ErrorMessage="Поле {0} должно быть от {2} до {1} символов")]
        [RegularExpression(@"[\w]{6,20}", ErrorMessage = "{0} должен состоять из английских букв и/или цифр")]
        public string Name { get { return name; } set { name = value; } }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Поле {0} должно быть от {2} до {1} символов")]
        [RegularExpression(@"[\w]{6,20}", ErrorMessage = "{0} должен состоять из английских букв и/или цифр")]
        public string Password { get { return pass; } set { pass = value; } }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль еще раз")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [Compare("Password", ErrorMessage = "Поля должны совпадать")]
        public string Confirm { get { return confirm; } set { confirm = value; } }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Поле {0} не заполнено")]
        [RegularExpression(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$", ErrorMessage = "{0} должен состоять из английских букв и/или цифр")]
        public string Email { get { return email; } set { email = value; } }



        private string name;
        private string pass;
        private string email;
        private string confirm;


        /// <summary>
        /// Приведение RegUser в User
        /// </summary>
        /// <param name="RegUser"></param>
        /// <returns></returns>
        public static explicit operator Entities.User (RegModel RegUser)
        {
            return new Entities.User() { Name = RegUser.Name, Password = GetString(EncodePassword(RegUser.Password)), Email=RegUser.Email, Role_Id = 3 };
        }

        /// <summary>
        /// Шифрование пароля по SHA1
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>

        public static byte[] EncodePassword(string pass)
        {
            var s = SHA1.Create();
            return s.ComputeHash(GetBytes(pass));
        }

        /// <summary>
        /// Преобразование байтов в строку
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
        /// Обратное преобазование
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }


        /// <summary>
        /// Регистрация пользователя с проверками на существование такого же логина и адреса почты в БД. Сразу же авторизируем его.
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>

        internal bool Reg(out string Result)
        {
            var user = (Entities.User)this;
            byte[] Pass = GetDAL.dal.CheckLogPas(this.Name);
            if (Pass != null)
            {
                Result = "Пользователь с таким логином уже существует!";
                logger.Info("Попытка регистрации существующего логина " + this.Name);
                return false;
            }
            else
            {
                if (GetDAL.dal.CheckMail(this.Email))
                {
                    Result = "Данный E-mail уже зарегистрирован!";
                    logger.Info("Попытка регистрации существующего E-mail: " + this.Email);
                    return false;
                }
                else
                {

                    bool answer = GetDAL.dal.Registration(user);
                    if (answer)
                    {
                        FormsAuthentication.RedirectFromLoginPage(this.Name, false);
                        Result = "Вы зарегистрированы!";
                        logger.Info("Зарегистрирован новый пользователь: " + this.Name);
                        return true;
                    }
                    else
                    {
                        Result = "Упс, что-то пошло не так! Попробуйте, пажалуйста, еще раз.";
                        logger.Error("Ошибка регистрации пользователя c E-mail:" + this.Email + " и логином: " + this.Name);
                        return false;
                    }
                }
            }
        }


        internal bool RegByAdmin(out string Result)
        {
            var user = (Entities.User)this;
            byte[] Pass = GetDAL.dal.CheckLogPas(this.Name);
            if (Pass != null)
            {
                Result = "Пользователь с таким логином уже существует!";
                logger.Info("Попытка регистрации Админитсратором существующего логина: " + this.Name);
                return false;
            }
            else
            {
                if (GetDAL.dal.CheckMail(this.Email))
                {
                    Result = "Данный E-mail уже зарегистрирован!";
                    logger.Info("Попытка регистрации Администратором существующего E-mail: " + this.Email);
                    return false;
                }
                else
                {

                    bool answer = GetDAL.dal.Registration(user);
                    if (answer)
                    {
                        Result = "Вы зарегистрированы!";
                        logger.Info("Администратор Зарегистрировал нового пользователя: " + this.Name);
                        return true;
                    }
                    else
                    {
                        Result = "Упс, что-то пошло не так! Попробуйте, пажалуйста, еще раз.";
                        logger.Error("Ошибка регистрации Администратором пользователя c E-mail:" + this.Email + " и логином: " + this.Name);
                        return false;
                    }
                }
            }
        }


    }
}