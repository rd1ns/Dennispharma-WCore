﻿using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents an associated product search model
    /// </summary>
    public partial class AssociatedProductSearchModel : BaseSearchModel
    {
        #region Properties

        public int ProductId { get; set; }

        #endregion
    }
}