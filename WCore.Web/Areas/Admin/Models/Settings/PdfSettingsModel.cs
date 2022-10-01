using System.ComponentModel.DataAnnotations;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a PDF settings model
    /// </summary>
    public partial class PdfSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLetterPageSizeEnabled")]
        public bool LetterPageSizeEnabled { get; set; }
        public bool LetterPageSizeEnabled_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLogo")]
        [UIHint("Picture")]
        public int LogoPictureId { get; set; }
        public bool LogoPictureId_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisablePdfInvoicesForPendingOrders")]
        public bool DisablePdfInvoicesForPendingOrders { get; set; }
        public bool DisablePdfInvoicesForPendingOrders_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.InvoiceFooterTextColumn1")]
        public string InvoiceFooterTextColumn1 { get; set; }
        public bool InvoiceFooterTextColumn1_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.InvoiceFooterTextColumn2")]
        public string InvoiceFooterTextColumn2 { get; set; }
        public bool InvoiceFooterTextColumn2_OverrideForStore { get; set; }

        #endregion
    }
}