using System.Collections.Generic;
using WCore.Core.Configuration;

namespace WCore.Core.Domain.Settings
{

    /// <summary>
    /// SEO settings
    /// </summary>
    public class SeoSettings : ISettings
    {
        /// <summary>
        /// Page title separator
        /// </summary>
        public string PageTitleSeparator { get; set; }

        /// <summary>
        /// Page title SEO adjustment
        /// </summary>
        public PageTitleSeoAdjustment PageTitleSeoAdjustment { get; set; }

        /// <summary>
        /// Default title
        /// </summary>
        public string DefaultTitle { get; set; }

        /// <summary>
        /// Default META keywords
        /// </summary>
        public string DefaultMetaKeywords { get; set; }

        /// <summary>
        /// Default META description
        /// </summary>
        public string DefaultMetaDescription { get; set; }

        /// <summary>
        /// A value indicating whether product META descriptions will be generated automatically (if not entered)
        /// </summary>
        public bool GenerateProductMetaDescription { get; set; }

        /// <summary>
        /// A value indicating whether we should convert non-western chars to western ones
        /// </summary>
        public bool ConvertNonWesternChars { get; set; }

        /// <summary>
        /// A value indicating whether unicode chars are allowed
        /// </summary>
        public bool AllowUnicodeCharsInUrls { get; set; }

        /// <summary>
        /// A value indicating whether canonical URL tags should be used
        /// </summary>
        public bool CanonicalUrlsEnabled { get; set; }

        /// <summary>
        /// A value indicating whether to use canonical URLs with query string parameters
        /// </summary>
        public bool QueryStringInCanonicalUrlsEnabled { get; set; }

        /// <summary>
        /// WWW requires (with or without WWW)
        /// </summary>
        public WwwRequirement WwwRequirement { get; set; }

        /// <summary>
        /// A value indicating whether Twitter META tags should be generated
        /// </summary>
        public bool TwitterMetaTags { get; set; }

        /// <summary>
        /// A value indicating whether Open Graph META tags should be generated
        /// </summary>
        public bool OpenGraphMetaTags { get; set; }

        /// <summary>
        /// Slugs (sename) reserved for some other needs
        /// </summary>
        public string ReservedUrlRecordSlugs { get; set; }

        /// <summary>
        /// Custom tags in the <![CDATA[<head></head>]]> section
        /// </summary>
        public string CustomHeadTags { get; set; }

        /// <summary>
        /// A value indicating whether Microdata tags should be generated
        /// </summary>
        public bool MicrodataEnabled { get; set; }
    }

    /// <summary>
    /// Represents a page title SEO adjustment
    /// </summary>
    public enum PageTitleSeoAdjustment
    {
        /// <summary>
        /// Pagename comes after storename
        /// </summary>
        PagenameAfterStorename = 0,

        /// <summary>
        /// Storename comes after pagename
        /// </summary>
        StorenameAfterPagename = 10
    }

    /// <summary>
    /// Represents WWW requirement
    /// </summary>
    public enum WwwRequirement
    {
        /// <summary>
        /// Doesn't matter (do nothing)
        /// </summary>
        NoMatter = 0,

        /// <summary>
        /// Pages should have WWW prefix
        /// </summary>
        WithWww = 10,

        /// <summary>
        /// Pages should not have WWW prefix
        /// </summary>
        WithoutWww = 20
    }
}
