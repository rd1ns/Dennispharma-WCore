using WCore.Core.Configuration;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Security settings
    /// </summary>
    public partial class MailSettingsModel : BaseWCoreModel, ISettingsModel
    {

        /// <summary>
        /// HostName mail.WCore.com.tr
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Mail.HostName")]
        public string HostName { get; set; }

        /// <summary>
        /// Mail port number 587
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Mail.PortNumber")]
        public int PortNumber { get; set; }

        /// <summary>
        /// Mail Sender E-Mail Address
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Mail.EmailAddress")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Mail Sender E-Mail Password
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Mail.EmailPassword")]
        public string EmailPassword { get; set; }

        /// <summary>
        /// If host use ssl use true!
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Mail.UseSSL")]
        public bool UseSSL { get; set; }
        /// <summary>
        /// If host use status use true!
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Mail.Status")]
        public bool Status { get; set; }
    }
}
