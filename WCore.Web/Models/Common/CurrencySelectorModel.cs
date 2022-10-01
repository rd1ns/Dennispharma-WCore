using WCore.Framework.Models;
using WCore.Web.Models.Directory;
using WCore.Web.Models.Localization;
using System.Collections.Generic;

namespace WCore.Web.Models.Common
{
    public partial class CurrencySelectorModel : BaseWCoreModel
    {
        public CurrencySelectorModel()
        {
            AvailableCurrencies = new List<CurrencyModel>();
        }

        public IList<CurrencyModel> AvailableCurrencies { get; set; }

        public CurrencyModel CurrentCurrency { get; set; }
    }
}
