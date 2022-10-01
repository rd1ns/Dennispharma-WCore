using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a recurring payment model
    /// </summary>
    public partial class RecurringPaymentModel : BaseWCoreEntityModel
    {
        #region Ctor

        public RecurringPaymentModel()
        {
            RecurringPaymentHistorySearchModel = new RecurringPaymentHistorySearchModel();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.ID")]
        public override int Id { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.CycleLength")]
        public int CycleLength { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.CyclePeriod")]
        public int CyclePeriodId { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.CyclePeriod")]
        public string CyclePeriodStr { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.TotalCycles")]
        public int TotalCycles { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.StartDate")]
        public string StartDate { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.IsActive")]
        public bool IsActive { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.NextPaymentDate")]
        public string NextPaymentDate { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.CyclesRemaining")]
        public int CyclesRemaining { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.InitialOrder")]
        public int InitialOrderId { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.User")]
        public int UserId { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.User")]
        public string UserEmail { get; set; }

        [WCoreResourceDisplayName("Admin.RecurringPayments.Fields.PaymentType")]
        public string PaymentType { get; set; }

        public bool CanCancelRecurringPayment { get; set; }

        public bool LastPaymentFailed { get; set; }

        public RecurringPaymentHistorySearchModel RecurringPaymentHistorySearchModel { get; set; }

        #endregion
    }
}