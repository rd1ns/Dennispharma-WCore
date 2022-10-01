using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Web.Areas.Admin.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Common
{
    #region Country
    public class CountryModel : BaseWCoreEntityModel
    {
        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.AllowsBilling")]
        public bool AllowsBilling { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.AllowsShipping")]
        public bool AllowsShipping { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.TwoLetterIsoCode")]
        public string TwoLetterIsoCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ThreeLetterIsoCode")]
        public string ThreeLetterIsoCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.NumericIsoCode")]
        public int NumericIsoCode { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.ShortCode")]
        public string ShortCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.LanguageCode")]
        public string LanguageCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.PhoneCode")]
        public string PhoneCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Flag")]
        public string Flag { get; set; }


        [WCoreResourceDisplayName("Admin.Configuration.User")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.CreatedUser")]
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SubjectToVat")]
        public bool SubjectToVat { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Published")]
        public bool Published { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
    }
    public partial class CountrySearchModel : BaseSearchModel
    {
        #region Ctor

        public CountrySearchModel()
        {
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Published")]
        public bool? Published { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        #endregion
    }
    public partial class CountryListModel : BasePagedListModel<CountryModel>
    {
    } 
    #endregion

    #region City
    public class CityModel : BaseWCoreEntityModel
    {
        public CityModel()
        {
            Countries = new List<SelectListItem>();
        }
        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.PlaqueCode")]
        public string PlaqueCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.PhoneCode")]
        public string PhoneCode { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Country")]
        public int CountryId { get; set; }
        public virtual CountryModel Country { get; set; }


        [WCoreResourceDisplayName("Admin.Configuration.User")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.CreatedUser")]
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        public virtual List<SelectListItem> Countries { get; set; }
    }
    public partial class CitySearchModel : BaseSearchModel
    {
        #region Ctor

        public CitySearchModel()
        {
            Countries = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Country")]
        public int CountryId { get; set; }
        public virtual List<SelectListItem> Countries { get; set; }
        #endregion
    }
    public partial class CityListModel : BasePagedListModel<CityModel>
    {
    } 
    #endregion

    #region District
    public class DistrictModel : BaseWCoreEntityModel
    {
        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.City")]
        public int CityId { get; set; }
        public virtual CityModel City { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Country")]
        public int CountryId { get; set; }
        public virtual CountryModel Country { get; set; }


        [WCoreResourceDisplayName("Admin.Configuration.User")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.CreatedUser")]
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        public virtual List<SelectListItem> Countries { get; set; }
        public virtual List<SelectListItem> Cities { get; set; }
    }
    public partial class DistrictSearchModel : BaseSearchModel
    {
        #region Ctor

        public DistrictSearchModel()
        {
            Countries = new List<SelectListItem>();
            Cities = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.City")]
        public int? CityId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Country")]
        public int? CountryId { get; set; }

        public virtual List<SelectListItem> Countries { get; set; }
        public virtual List<SelectListItem> Cities { get; set; }
        #endregion
    }
    public partial class DistrictListModel : BasePagedListModel<DistrictModel>
    {
    } 
    #endregion

}