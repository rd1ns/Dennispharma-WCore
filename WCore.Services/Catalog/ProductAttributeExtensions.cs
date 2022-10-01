using WCore.Core.Domain.Catalog;

namespace WCore.Services.Catalog
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class ProductAttributeExtensions
    {
        /// <summary>
        /// A value indicating whether this product attribute should have values
        /// </summary>
        /// <param name="productProductAttribute">Product attribute mapping</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this ProductProductAttribute productProductAttribute)
        {
            if (productProductAttribute == null)
                return false;

            if (productProductAttribute.AttributeControlType == AttributeControlType.TextBox ||
                productProductAttribute.AttributeControlType == AttributeControlType.MultilineTextbox ||
                productProductAttribute.AttributeControlType == AttributeControlType.Datepicker ||
                productProductAttribute.AttributeControlType == AttributeControlType.FileUpload)
                return false;

            //other attribute control types support values
            return true;
        }

        /// <summary>
        /// A value indicating whether this product attribute can be used as condition for some other attribute
        /// </summary>
        /// <param name="productProductAttribute">Product attribute mapping</param>
        /// <returns>Result</returns>
        public static bool CanBeUsedAsCondition(this ProductProductAttribute productProductAttribute)
        {
            if (productProductAttribute == null)
                return false;

            if (productProductAttribute.AttributeControlType == AttributeControlType.ReadonlyCheckboxes || 
                productProductAttribute.AttributeControlType == AttributeControlType.TextBox ||
                productProductAttribute.AttributeControlType == AttributeControlType.MultilineTextbox ||
                productProductAttribute.AttributeControlType == AttributeControlType.Datepicker ||
                productProductAttribute.AttributeControlType == AttributeControlType.FileUpload)
                return false;

            //other attribute control types support it
            return true;
        }

        /// <summary>
        /// A value indicating whether this product attribute should can have some validation rules
        /// </summary>
        /// <param name="productProductAttribute">Product attribute mapping</param>
        /// <returns>Result</returns>
        public static bool ValidationRulesAllowed(this ProductProductAttribute productProductAttribute)
        {
            if (productProductAttribute == null)
                return false;

            if (productProductAttribute.AttributeControlType == AttributeControlType.TextBox ||
                productProductAttribute.AttributeControlType == AttributeControlType.MultilineTextbox ||
                productProductAttribute.AttributeControlType == AttributeControlType.FileUpload)
                return true;

            //other attribute control types does not have validation
            return false;
        }

        /// <summary>
        /// A value indicating whether this product attribute is non-combinable
        /// </summary>
        /// <param name="productProductAttribute">Product attribute mapping</param>
        /// <returns>Result</returns>
        public static bool IsNonCombinable(this ProductProductAttribute productProductAttribute)
        {
            //When you have a product with several attributes it may well be that some are combinable,
            //whose combination may form a new SKU with its own inventory,
            //and some non-combinable are more used to add accessories

            if (productProductAttribute == null)
                return false;

            //we can add a new property to "ProductProductAttribute" entity indicating whether it's combinable/non-combinable
            //but we assume that attributes
            //which cannot have values (any value can be entered by a user)
            //are non-combinable
            var result = !ShouldHaveValues(productProductAttribute);
            return result;
        }
    }
}
