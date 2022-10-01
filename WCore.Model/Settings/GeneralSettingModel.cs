using System.ComponentModel;

namespace SkiTurkish.Model.Settings
{
    public class NotificationSettingModel : ISettingsModel
    {
        //Kullanıcı oluşturulduğunda
        [DisplayName("Kullanıcı oluşturulduğunda mail/bildirim gönder")]
        public bool SendRegisterNotification { get; set; }

        //Firma oluşturulduğunda
        [DisplayName("Firma oluşturulduğunda mail/bildirim gönder")]
        public bool SendCreateCompanyNotification { get; set; }
        public string CreateCompanyNotificationMailList { get; set; }

        //Sipariş oluşturulduğunda
        [DisplayName("Sipariş oluşturulduğunda mail/bildirim gönder")]
        public bool SendCreateOrderNotification { get; set; }
        public string CreateOrderNotificationMailList { get; set; }

        //Sipariş oluşturulduğunda
        [DisplayName("Limit yetersizliğinde mail/bildirim gönder")]
        public bool SendInadequateLimitNotification { get; set; }
        public string InadequateLimitNotificationMailList { get; set; }

        //Sipariş Reddedildiğinde
        [DisplayName("Sipariş Reddedildiğinde mail/bildirim gönder")]
        public bool SendDeniedOrderNotification { get; set; }
        public string DeniedOrderNotificationMailList { get; set; }

        //Ajandaya toplantı girildiğinde
        [DisplayName("Ajandaya toplantı girildiğinde mail/bildirim gönder")]
        public bool SendCreateActivityNotification { get; set; }
        public string CreateActivityNotificationMailList { get; set; }

        //Taşıtmatik Siparişi Girildiğinde
        [DisplayName("Taşıtmatik siparişi oluşturulduğunda mail/bildirim gönder")]
        public bool SendCreateVehicleOrderNotification { get; set; }
        public string CreateVehicleOrderNotificationMailList { get; set; }


    }
}
