using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using SkiTurkish.Core.Configuration;

namespace SkiTurkish.Model.Settings
{
    /// <summary>
    /// Security settings
    /// </summary>
    public class SeoSettingsModel : BaseSkiTurkishModel, ISettings
    {

        #region Properties

        public string PageTitleSeparator { get; set; }

        public int PageTitleSeoAdjustment { get; set; }
        public SelectList PageTitleSeoAdjustmentValues { get; set; }

        public string DefaultTitle { get; set; }
        #endregion
    }
}
