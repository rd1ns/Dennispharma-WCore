using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WCore.Core.Configuration;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Security settings
    /// </summary>
    public class NotificationSettingsModel : BaseWCoreModel, ISettings
    {

        /// <summary>
        /// Kullanıcı oluşturulduğunda mail/bildirim gönder
        /// </summary> 

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SendRegisterNotification")]
        public bool SendRegisterNotification { get; set; }

        /// <summary>
        /// Firma oluşturulduğunda mail/bildirim gönder
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SendCreateCompanyNotification")]
        public bool SendCreateCompanyNotification { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SubjectCreateCompanyNotificationMailListFieldOnContactUsForm")]
        public string CreateCompanyNotificationMailList { get; set; }

        /// <summary>
        /// Sipariş oluşturulduğunda mail/bildirim gönder
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SendCreateOrderNotification")]
        public bool SendCreateOrderNotification { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.CreateOrderNotificationMailList")]
        public string CreateOrderNotificationMailList { get; set; }

        /// <summary>
        /// Limit yetersizliğinde mail/bildirim gönder
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SendInadequateLimitNotification")]
        public bool SendInadequateLimitNotification { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.InadequateLimitNotificationMailList")]
        public string InadequateLimitNotificationMailList { get; set; }

        /// <summary>
        /// Sipariş Reddedildiğinde mail/bildirim gönder
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SendDeniedOrderNotification")]
        public bool SendDeniedOrderNotification { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.DeniedOrderNotificationMailList")]
        public string DeniedOrderNotificationMailList { get; set; }

        /// <summary>
        /// Ajandaya toplantı girildiğinde mail/bildirim gönder
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SendCreateActivityNotification")]
        public bool SendCreateActivityNotification { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.CreateActivityNotificationMailList")]
        public string CreateActivityNotificationMailList { get; set; }

        /// <summary>
        /// Taşıtmatik siparişi oluşturulduğunda mail/bildirim gönder
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.SendCreateVehicleOrderNotification")]
        public bool SendCreateVehicleOrderNotification { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Notification.CreateVehicleOrderNotificationMailList")]
        public string CreateVehicleOrderNotificationMailList { get; set; }
    }
}
