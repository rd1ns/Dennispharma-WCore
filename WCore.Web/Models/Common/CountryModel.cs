using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Web.Areas.Admin.Models.Users;
using System;
using System.Collections.Generic;

namespace WCore.Web.Models.Common
{
    public class CountryModel : BaseWCoreEntityModel
    {
        public string Name { get; set; }

        public string ShortCode { get; set; }
        public string LanguageCode { get; set; }
        public string PhoneCode { get; set; }
        public string Flag { get; set; }


        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
    }
    public class CityModel : BaseWCoreEntityModel
    {
        public string Name { get; set; }
        public string PlaqueCode { get; set; }
        public string PhoneCode { get; set; }

        public int CountryId { get; set; }
        public virtual CountryModel Country { get; set; }


        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        public virtual List<SelectListItem> Countries { get; set; }
    }
    public class DistrictModel : BaseWCoreEntityModel
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public virtual CityModel City { get; set; }
        public int CountryId { get; set; }
        public virtual CountryModel Country { get; set; }


        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        public virtual List<SelectListItem> Countries { get; set; }
        public virtual List<SelectListItem> Cities { get; set; }
    }

}