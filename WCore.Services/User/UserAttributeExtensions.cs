using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Users;

namespace WCore.Services.Users
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class UserAttributeExtensions
    {
        /// <summary>
        /// A value indicating whether this user attribute should have values
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this UserAttribute userAttribute)
        {
            if (userAttribute == null)
                return false;

            if (userAttribute.AttributeControlTypeId == AttributeControlType.TextBox ||
                userAttribute.AttributeControlTypeId == AttributeControlType.MultilineTextbox ||
                userAttribute.AttributeControlTypeId == AttributeControlType.Datepicker ||
                userAttribute.AttributeControlTypeId == AttributeControlType.FileUpload)
                return false;

            //other attribute control types support values
            return true;
        }
    }
}
