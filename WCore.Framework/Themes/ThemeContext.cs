using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Settings;

namespace WCore.Framework.Themes
{
    /// <summary>
    /// Represents the theme context implementation
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        #region Fields
        private readonly IThemeProvider _themeProvider;
        private readonly IWorkContext _workContext;
        private readonly StoreInformationSettings _storeInformationSettings;

        private string _cachedThemeName;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="themeProvider">Theme provider</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeInformationSettings">Store information settings</param>
        public ThemeContext(IThemeProvider themeProvider,
            IWorkContext workContext,
            StoreInformationSettings storeInformationSettings)
        {
            _themeProvider = themeProvider;
            _workContext = workContext;
            _storeInformationSettings = storeInformationSettings;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set current theme system name
        /// </summary>
        public string WorkingThemeName
        {
            get
            {
                if (!string.IsNullOrEmpty(_cachedThemeName))
                    return _cachedThemeName;

                var themeName = string.Empty;


                //if not, try to get default store theme
                if (string.IsNullOrEmpty(themeName))
                    themeName = _storeInformationSettings.DefaultStoreTheme;

                //ensure that this theme exists
                if (!_themeProvider.ThemeExists(themeName))
                {
                    //if it does not exist, try to get the first one
                    themeName = _themeProvider.GetThemes().FirstOrDefault()?.SystemName
                        ?? throw new Exception("No theme could be loaded");
                }

                //cache theme system name
                _cachedThemeName = themeName;

                return themeName;
            }
            set
            {
                //whether customers are allowed to select a theme
                if (!_storeInformationSettings.AllowUserToSelectTheme || _workContext.CurrentUser == null)
                    return;

                //clear cache
                _cachedThemeName = null;
            }
        }

        #endregion
    }
}
