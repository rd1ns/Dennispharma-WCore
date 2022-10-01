namespace WCore.Core.Domain.Discounts
{
    /// <summary>
    /// Represents a discount-category mapping class
    /// </summary>
    public partial class DiscountAppliedToCategory : DiscountMapping
    {
        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public override int EntityId { get; set; }
    }
}