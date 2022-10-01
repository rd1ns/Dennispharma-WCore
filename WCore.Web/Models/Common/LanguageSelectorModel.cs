using WCore.Framework.Models;
using WCore.Web.Models.Localization;
using System.Collections.Generic;

namespace WCore.Web.Models.Common
{
    public partial class LanguageSelectorModel : BaseWCoreModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public LanguageModel CurrentLanguage { get; set; }

        public bool UseImages { get; set; }
    }
}
