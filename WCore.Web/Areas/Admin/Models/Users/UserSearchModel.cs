using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    ///// <summary>
    ///// Represents a user search model
    ///// </summary>
    //public partial class UserSearchModel : BaseSearchModel
    //{
    //    #region Ctor

    //    public UserSearchModel()
    //    {
    //        SelectedUserRoleIds = new List<int>();
    //        AvailableUserRoles = new List<SelectListItem>();
    //    }

    //    #endregion

    //    #region Properties

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.UserRoles")]
    //    public IList<int> SelectedUserRoleIds { get; set; }

    //    public IList<SelectListItem> AvailableUserRoles { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchEmail")]
    //    public string SearchEmail { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchUsername")]
    //    public string SearchUsername { get; set; }

    //    public bool UsernamesEnabled { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchFirstName")]
    //    public string SearchFirstName { get; set; }
    //    public bool FirstNameEnabled { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchLastName")]
    //    public string SearchLastName { get; set; }
    //    public bool LastNameEnabled { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchDateOfBirth")]
    //    public string SearchDayOfBirth { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchDateOfBirth")]
    //    public string SearchMonthOfBirth { get; set; }

    //    public bool DateOfBirthEnabled { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchCompany")]
    //    public string SearchCompany { get; set; }

    //    public bool CompanyEnabled { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchPhone")]
    //    public string SearchPhone { get; set; }

    //    public bool PhoneEnabled { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchZipCode")]
    //    public string SearchZipPostalCode { get; set; }

    //    public bool ZipPostalCodeEnabled { get; set; }

    //    [WCoreResourceDisplayName("Admin.Users.Users.List.SearchIpAddress")]
    //    public string SearchIpAddress { get; set; }

    //    public bool AvatarEnabled { get; internal set; }

    //    #endregion
    //}
}