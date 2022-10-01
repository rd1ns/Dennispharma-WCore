using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Domain;
using WCore.Core.Domain.Medias;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.Localization;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Web.Areas.Admin.Models
{
    public partial class UploadImageModel
    {
        #region Properties
        public string Original { get; set; }
        public string Big { get; set; }
        public string Medium { get; set; }
        public string Small { get; set; }
        #endregion
    }
}
