using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Framework.Themes
{
    /// <summary>
    /// Represents a theme context
    /// </summary>
    public interface IThemeContext
    {
        /// <summary>
        /// Get or set current theme system name
        /// </summary>
        string WorkingThemeName { get; set; }
    }
}
