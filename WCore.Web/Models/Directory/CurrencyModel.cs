using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.Localization;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.UI.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Web.Models.Directory
{
    public partial class CurrencyModel : BaseWCoreEntityModel
    {
        #region Ctor

        #endregion

        #region Properties
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public string DisplayLocale { get; set; }
        public decimal Rate { get; set; }
        public string CustomFormatting { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPrimaryExchangeRateCurrency { get; set; }
        public bool IsPrimaryStoreCurrency { get; set; }
        #endregion
    }
    public partial class CurrencyListModel : BaseWCoreModel
    {
        public CurrencyListModel()
        {
            PagingFilteringContext = new CurrencyPagingFilteringModel();
            Currencies = new List<CurrencyModel>();
        }

        public int WorkingLanguageId { get; set; }
        public CurrencyPagingFilteringModel PagingFilteringContext { get; set; }
        public List<CurrencyModel> Currencies { get; set; }
    }
    public partial class CurrencyPagingFilteringModel : BasePageableModel
    {
        #region Properties
        #endregion
    }
}
