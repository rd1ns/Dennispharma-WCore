using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using SkiTurkish.Core.Configuration;

namespace SkiTurkish.Model.Settings
{
    /// <summary>
    /// Security settings
    /// </summary>
    public class NotificationSettingsModel : BaseSkiTurkishModel, ISettings
    {

        /// <summary>
        /// Kullanıcı oluşturulduğunda mail/bildirim gönder
        /// </summary>
        public bool SendRegisterNotification { get; set; }

        /// <summary>
        /// Firma oluşturulduğunda mail/bildirim gönder
        /// </summary>
        public bool SendCreateCompanyNotification { get; set; }
        public string CreateCompanyNotificationMailList { get; set; }

        /// <summary>
        /// Sipariş oluşturulduğunda mail/bildirim gönder
        /// </summary>
        public bool SendCreateOrderNotification { get; set; }
        public string CreateOrderNotificationMailList { get; set; }

        /// <summary>
        /// Limit yetersizliğinde mail/bildirim gönder
        /// </summary>
        public bool SendInadequateLimitNotification { get; set; }
        public string InadequateLimitNotificationMailList { get; set; }

        /// <summary>
        /// Sipariş Reddedildiğinde mail/bildirim gönder
        /// </summary>
        public bool SendDeniedOrderNotification { get; set; }
        public string DeniedOrderNotificationMailList { get; set; }

        /// <summary>
        /// Ajandaya toplantı girildiğinde mail/bildirim gönder
        /// </summary>
        public bool SendCreateActivityNotification { get; set; }
        public string CreateActivityNotificationMailList { get; set; }

        /// <summary>
        /// Taşıtmatik siparişi oluşturulduğunda mail/bildirim gönder
        /// </summary>
        public bool SendCreateVehicleOrderNotification { get; set; }
        public string CreateVehicleOrderNotificationMailList { get; set; }
    }
}
