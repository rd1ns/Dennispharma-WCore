using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a setting search model
    /// </summary>
    public partial class SettingSearchModel : BaseSearchModel
    {
        #region Ctor

        public SettingSearchModel()
        {
            AddSetting = new SettingModel();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Settings.AllSettings.SearchSettingName")]
        public string SearchSettingName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.AllSettings.SearchSettingValue")]
        public string SearchSettingValue { get; set; }

        public SettingModel AddSetting { get; set; }

        #endregion
    }
}