using Microsoft.AspNetCore.Mvc.Rendering;
using SkiTurkish.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SkiTurkish.Model.Users
{
    public class UserModel : BaseSkiTurkishEntityModel
    {
        public UserModel()
        {
            UserTypes = new List<SelectListItem>();
        }

        [DisplayName("İsim")]
        public string FirstName { get; set; }
        [DisplayName("İkinic İsim")]
        public string MiddleName { get; set; }
        [DisplayName("Soyisim")]
        public string LastName { get; set; }
        [DisplayName("Avatar")]
        public string Avatar { get; set; }
        [DisplayName("E-Posta İmzası")]
        public string Signature { get; set; }
        [DisplayName("E-Posta")]
        public string EMail { get; set; }
        [DisplayName("Kullanıcı Tipi")]
        public UserType UserType { get; set; }
        [DisplayName("Telefon")]
        public string Phone { get; set; }
        [DisplayName("Şifre")]
        public string Password { get; set; }
        [DisplayName("Pozisyonu")]
        public string Position { get; set; }
        [DisplayName("Kullanıcı Adı")]
        public string Username { get; set; }


        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Güncellenme Tarihi")]
        public DateTime UpdatedOn { get; set; }
        [DisplayName("Silindi")]
        public bool Deleted { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }

        public UserPasswordModel UserPassword { get; set; }
        public UserSetupModel UserSetup { get; set; }
        public List<SelectListItem> UserTypes { get; set; }

        /// <summary>
        /// Ajax olarak ekleme/düzenleme işlemleri için true gönderin.
        /// </summary>
        public bool IsPopup { get; set; }
    }
    public class UserPasswordModel
    {
        [DisplayName("Şimdiki Şifre")]
        public string CurrentPassword { get; set; }
        [DisplayName("Yeni Şifre")]
        public string NewPassword { get; set; }
        [DisplayName("Yeni Şifre Doğrula")]
        public string VerifyPassword { get; set; }

        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
    }
    public class UserSetupModel
    {
        [DisplayName("E-Posta Bildirimleri")]
        public bool EmailNotification { get; set; }
        [DisplayName("Yeni Bildirimler")]
        public bool NewNotifications { get; set; }
        [DisplayName("Direk Mesaj")]
        public bool DirectMessage { get; set; }

        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
    }
}
