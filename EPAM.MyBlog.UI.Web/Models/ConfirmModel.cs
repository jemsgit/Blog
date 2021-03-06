﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPAM.MyBlog.UI.Web.Models
{
    public class ConfirmModel
    {
        /// <summary>
        /// Модель подтверждения. Свойство Reason используется только при удалении - для записи причины удаления аккаунта
        /// </summary>

        public bool Confirm { get{return confirm;} set{confirm = value;} }

        [Display(Name = "Причина удаления (необязательно для заполнения)")]
        public string Reason { get { return reason; } set { reason = value; } }
        private bool confirm;
        private string reason;


    }
}