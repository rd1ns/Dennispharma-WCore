namespace WCore.Core.Domain.Shipping
{
    /// <summary>
    /// Represents a shipping method-country mapping class
    /// </summary>
    public partial class ShippingMethodCountry: BaseEntity
    {
        /// <summary>
        /// Gets or sets the shipping method identifier
        /// </summary>
        public int ShippingMethodId { get; set; }

        /// <summary>
        /// Gets or sets the country identifier
        /// </summary>
        public int CountryId { get; set; }
    }
}