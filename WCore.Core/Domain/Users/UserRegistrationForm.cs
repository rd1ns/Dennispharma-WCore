using System;

namespace WCore.Core.Domain.Users
{
    public class UserRegistrationForm : BaseEntity
    {
        #region Personel Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string MotherName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string FatherName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Phone { get; set; }
        #endregion

        #region Contact Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string CompanyDepartment { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string City { get; set; }
        #endregion

        #region Extra Information

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string HistoryOfExpertise { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string InstitutionExpert { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string ProfessionDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string ProfessionDocumentNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string SideBranch { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string SideBranchDocumentDate { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string SideBranchDocumentNo { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string ReferenceName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string ReferenceEmail { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public bool ShareInfo { get; set; }
        #endregion


        public DateTime CreatedOn { get;set;}
    }
}
