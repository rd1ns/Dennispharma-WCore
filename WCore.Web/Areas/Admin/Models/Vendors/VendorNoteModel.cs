using System;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor note model
    /// </summary>
    public partial class VendorNoteModel : BaseWCoreEntityModel
    {
        #region Ctor
        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Vendors.Fields.Name")]
        public int VendorId { get; set; }
        public virtual VendorModel Vendor { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
        public string Note { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorNotes.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
    public partial class VendorNoteSearchModel : BaseSearchModel
    {
        #region Ctor

        public VendorNoteSearchModel()
        {
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Vendors.Fields.Name")]
        public int VendorId { get; set; }
        #endregion
    }
    public partial class VendorNoteListModel : BasePagedListModel<VendorNoteModel>
    {
    }
}
