﻿using System;
using System.Collections.Generic;
using System.Text;
using WCore.Core.Configuration;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Common settings
    /// </summary>
    public class CommonSettingsModel : BaseWCoreModel, ISettingsModel
    {
        public CommonSettingsModel()
        {
            IgnoreLogWordlist = new List<string>();
        }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.SubjectFieldOnContactUsForm")]  
        /// <summary>
        /// Gets or sets a value indicating whether the contacts form should have "Subject"
        /// </summary>
        public bool SubjectFieldOnContactUsForm { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.UseSystemEmailForContactUsForm")]   
        /// <summary>
        /// Gets or sets a value indicating whether the contacts form should use system email
        /// </summary>
        public bool UseSystemEmailForContactUsForm { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.UseStoredProcedureForLoadingCategories")]
        /// <summary>
        /// Gets or sets a value indicating whether to use stored procedure (if supported) for loading categories (it's much faster in admin area with a large number of categories than the LINQ implementation)
        /// </summary>
        public bool UseStoredProcedureForLoadingCategories { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.DisplayJavaScriptDisabledWarning")]
        /// <summary>
        /// Gets or sets a value indicating whether to display a warning if java-script is disabled
        /// </summary>
        public bool DisplayJavaScriptDisabledWarning { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.UseFullTextSearch")]
        /// <summary>
        /// Gets or sets a value indicating whether full-text search is supported
        /// </summary>
        public bool UseFullTextSearch { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.Log404Errors")]
        /// <summary>
        /// Gets or sets a value indicating whether 404 errors (page or file not found) should be logged
        /// </summary>
        public bool Log404Errors { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.BreadcrumbDelimiter")]
        /// <summary>
        /// Gets or sets a breadcrumb delimiter used on the site
        /// </summary>
        public string BreadcrumbDelimiter { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.RenderXuaCompatible")]
        /// <summary>
        /// Gets or sets a value indicating whether we should render <meta http-equiv="X-UA-Compatible" content="IE=edge"/> tag
        /// </summary>
        public bool RenderXuaCompatible { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.XuaCompatibleValue")]
        /// <summary>
        /// Gets or sets a value of "X-UA-Compatible" META tag
        /// </summary>
        public string XuaCompatibleValue { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.IgnoreLogWordlist")]
        /// <summary>
        /// Gets or sets ignore words (phrases) to be ignored when logging errors/messages
        /// </summary>
        public List<string> IgnoreLogWordlist { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.BbcodeEditorOpenLinksInNewWindow")]
        /// <summary>
        /// Gets or sets a value indicating whether links generated by BBCode Editor should be opened in a new window
        /// </summary>
        public bool BbcodeEditorOpenLinksInNewWindow { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.PopupForTermsOfServiceLinks")]
        /// <summary>
        /// Gets or sets a value indicating whether "accept terms of service" links should be open in popup window. If disabled, then they'll be open on a new page.
        /// </summary>
        public bool PopupForTermsOfServiceLinks { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.JqueryMigrateScriptLoggingActive")]
        /// <summary>
        /// Gets or sets a value indicating whether jQuery migrate script logging is active
        /// </summary>
        public bool JqueryMigrateScriptLoggingActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.SupportPreviousWCorecommerceVersions")]
        /// <summary>
        /// Gets or sets a value indicating whether we should support previous WCoreCommerce versions (it can slightly improve performance)
        /// </summary>
        public bool SupportPreviousWCorecommerceVersions { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.UseResponseCompression")]     
        /// <summary>
        /// Gets or sets a value indicating whether to compress response (gzip by default). 
        /// You may want to disable it, for example, If you have an active IIS Dynamic Compression Module configured at the server level
        /// </summary>
        public bool UseResponseCompression { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.StaticFilesCacheControl")]
        /// <summary>
        /// Gets or sets a value of "Cache-Control" header value for static content
        /// </summary>
        public string StaticFilesCacheControl { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.FaviconAndAppIconsHeadCode")]
        /// <summary>
        /// Gets or sets a value of favicon and app icons <head/> code
        /// </summary>
        public string FaviconAndAppIconsHeadCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.EnableHtmlMinification")]
        /// <summary>
        /// Gets or sets a value indicating whether to enable markup minification
        /// </summary>
        public bool EnableHtmlMinification { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.EnableJsBundling")]
        /// <summary>
        /// A value indicating whether JS file bundling and minification is enabled
        /// </summary>
        public bool EnableJsBundling { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.EnableCssBundling")]
        /// <summary>
        /// A value indicating whether CSS file bundling and minification is enabled
        /// </summary>
        public bool EnableCssBundling { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Common.ScheduleTaskRunTimeout")]
        /// <summary>
        /// The length of time, in milliseconds, before the running schedule task times out. Set null to use default value
        /// </summary>
        public int? ScheduleTaskRunTimeout { get; set; }
    }
}
