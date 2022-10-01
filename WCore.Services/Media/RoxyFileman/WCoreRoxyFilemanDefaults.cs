using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Services.RoxyFileman
{
    /// <summary>
    /// Represents default values related to roxyFileman
    /// </summary>
    public static partial class WCoreRoxyFilemanDefaults
    {
        /// <summary>
        /// Default path to root directory of uploaded files (if appropriate settings are not specified)
        /// </summary>
        public static string DefaultRootDirectory { get; } = "/images/uploaded";

        /// <summary>
        /// Path to configuration file
        /// </summary>
        public static string ConfigurationFile { get; } = "/lib/Roxy_Fileman/conf.json";

        /// <summary>
        /// Path to directory of language files
        /// </summary>
        public static string LanguageDirectory { get; } = "/lib/Roxy_Fileman/lang";
    }
}
