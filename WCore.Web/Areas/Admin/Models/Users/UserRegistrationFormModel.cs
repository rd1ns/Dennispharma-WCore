using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace WCore.Web.Areas.Admin.Models.Users
{
    public class UserRegistrationFormModel : BaseWCoreEntityModel
    {
        #region Personel Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.IdNo")]
        public string IdNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.BirthDate")]
        public string BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.BirthPlace")]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.MotherName")]
        public string MotherName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.FatherName")]
        public string FatherName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.Phone")]
        public string Phone { get; set; }
        #endregion

        #region Contact Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.CompanyDepartment")]
        public string CompanyDepartment { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.HomeAddress")]
        public string HomeAddress { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.CompanyAddress")]
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.City")]
        public string City { get; set; }
        #endregion

        #region Extra Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.Profession")]
        public string Profession { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.HistoryOfExpertise")]
        public string HistoryOfExpertise { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.InstitutionExpert")]
        public string InstitutionExpert { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.ProfessionDate")]
        public string ProfessionDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.ProfessionDocumentNo")]
        public string ProfessionDocumentNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.SideBranch")]
        public string SideBranch { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.SideBranchDocumentDate")]
        public string SideBranchDocumentDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.SideBranchDocumentNo")]
        public string SideBranchDocumentNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.ReferenceName")]
        public string ReferenceName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.ReferenceEmail")]
        public string ReferenceEmail { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.ShareInfo")]
        public bool ShareInfo { get; set; }
        #endregion


        [WCoreResourceDisplayName("Admin.Configuration.UserRegistrationForm.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// Represents a UserRegistrationForm search model
    /// </summary>
    public partial class UserRegistrationFormSearchModel : BaseSearchModel
    {
        #region Ctor
        public UserRegistrationFormSearchModel()
        {
        }
        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Query")]
        public string Query { get; set; }

        #endregion
    }
}
