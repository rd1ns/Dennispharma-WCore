﻿using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a gift card usage history search model
    /// </summary>
    public partial class GiftCardUsageHistorySearchModel : BaseSearchModel
    {
        #region Properties

        public int GiftCardId { get; set; }

        #endregion
    }
}