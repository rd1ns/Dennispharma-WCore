using WCore.Framework.Models;
using WCore.Web.Models.Localization;
using System.Collections.Generic;

namespace WCore.Web.Models.Common
{
    public partial class CountrySelectorModel : BaseWCoreModel
    {
        public CountrySelectorModel()
        {
            AvailableCountries = new List<CountryModel>();
        }

        public IList<CountryModel> AvailableCountries { get; set; }

        public CountryModel CurrentCountry{ get; set; }
    }
}
