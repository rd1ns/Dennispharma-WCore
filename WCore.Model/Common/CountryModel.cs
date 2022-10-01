using Microsoft.AspNetCore.Mvc.Rendering;
using SkiTurkish.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SkiTurkish.Model.Common
{
    public class CountryModel : BaseSkiTurkishEntityModel
    {
        public string Name { get; set; }

        [DisplayName("Kısa Kod")]
        public string ShortCode { get; set; }
        [DisplayName("Dil Kodu")]
        public string LanguageCode { get; set; }
        [DisplayName("Ülke Kodu")]
        public string PhoneCode { get; set; }
        [DisplayName("Bayrak")]
        public string Flag { get; set; }


        [DisplayName("Son İşlem Yapan")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [DisplayName("İlk İşlem Yapan")]
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Güncellenme Tarihi")]
        public DateTime UpdatedOn { get; set; }
        [DisplayName("Silindi")]
        public bool Deleted { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }
    }
    public class CityModel : BaseSkiTurkishEntityModel
    {
        [DisplayName("Adı")]
        public string Name { get; set; }
        [DisplayName("Plaka Kodu")]
        public string PlaqueCode { get; set; }
        [DisplayName("Alan Kodu")]
        public string PhoneCode { get; set; }

        [DisplayName("Ülke")]
        public int CountryId { get; set; }
        public virtual CountryModel Country { get; set; }


        [DisplayName("Son İşlem Yapan")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [DisplayName("İlk İşlem Yapan")]
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Güncellenme Tarihi")]
        public DateTime UpdatedOn { get; set; }
        [DisplayName("Silindi")]
        public bool Deleted { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }
        public virtual List<SelectListItem> Countries { get; set; }
    }
    public class DistrictModel : BaseSkiTurkishEntityModel
    {
        [DisplayName("Adı")]
        public string Name { get; set; }

        [DisplayName("Şehir")]
        public int CityId { get; set; }
        public virtual CityModel City { get; set; }

        [DisplayName("Ülke")]
        public int CountryId { get; set; }
        public virtual CountryModel Country { get; set; }


        [DisplayName("Son İşlem Yapan")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [DisplayName("İlk İşlem Yapan")]
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Güncellenme Tarihi")]
        public DateTime UpdatedOn { get; set; }
        [DisplayName("Silindi")]
        public bool Deleted { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }
        public virtual List<SelectListItem> Countries { get; set; }
        public virtual List<SelectListItem> Cities { get; set; }
    }

}