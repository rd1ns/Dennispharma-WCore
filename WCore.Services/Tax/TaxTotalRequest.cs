using System.Collections.Generic;
using WCore.Core.Domain.Users;
using WCore.Core.Domain.Orders;

namespace WCore.Services.Tax
{
    /// <summary>
    /// Represents a request to get tax total
    /// </summary>
    public partial class TaxTotalRequest
    {
        #region Ctor

        public TaxTotalRequest()
        {
            ShoppingCart = new List<ShoppingCartItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a shopping cart
        /// </summary>
        public IList<ShoppingCartItem> ShoppingCart { get; set; }

        /// <summary>
        /// Gets or sets a user
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use payment method additional fee
        /// </summary>
        public bool UsePaymentMethodAdditionalFee { get; set; }

        /// <summary>
        /// Gets or sets a store identifier
        /// </summary>
        public int StoreId { get; set; }

        #endregion
    }
}