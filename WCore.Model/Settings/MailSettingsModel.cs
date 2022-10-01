using SkiTurkish.Core.Configuration;

namespace SkiTurkish.Model.Settings
{
    /// <summary>
    /// Security settings
    /// </summary>
    public partial class MailSettingsModel : BaseSkiTurkishModel, ISettingsModel
    {

        /// <summary>
        /// HostName mail.skiTurkish.com.tr
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Mail port number 587
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// Mail Sender E-Mail Address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Mail Sender E-Mail Password
        /// </summary>
        public string EmailPassword { get; set; }

        /// <summary>
        /// If host use ssl use true!
        /// </summary>
        public bool UseSSL { get; set; }
    }
}
