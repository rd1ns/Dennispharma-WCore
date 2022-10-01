using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace WCore.Web.Models.Users
{
    public class UserRegistrationFormModel : BaseWCoreEntityModel
    {
        #region Personel Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.IdNo")]
        public string IdNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.BirthDate")]
        public string BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.BirthPlace")]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.MotherName")]
        public string MotherName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.FatherName")]
        public string FatherName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.Phone")]
        public string Phone { get; set; }
        #endregion

        #region Contact Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.CompanyDepartment")]
        public string CompanyDepartment { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.HomeAddress")]
        public string HomeAddress { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.CompanyAddress")]
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.City")]
        public string City { get; set; }
        #endregion

        #region Extra Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.Profession")]
        public string Profession { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.HistoryOfExpertise")]
        public string HistoryOfExpertise { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.InstitutionExpert")]
        public string InstitutionExpert { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.ProfessionDate")]
        public string ProfessionDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.ProfessionDocumentNo")]
        public string ProfessionDocumentNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.SideBranch")]
        public string SideBranch { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.SideBranchDocumentDate")]
        public string SideBranchDocumentDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.SideBranchDocumentNo")]
        public string SideBranchDocumentNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.ReferenceName")]
        public string ReferenceName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.ReferenceEmail")]
        public string ReferenceEmail { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [WCoreResourceDisplayName("Common.UserRegistrationForm.ShareInfo")]
        public bool ShareInfo { get; set; }
        #endregion


        [WCoreResourceDisplayName("Common.UserRegistrationForm.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}
