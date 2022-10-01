using WCore.Framework.Models;
using System.Collections.Generic;

namespace WCore.Web.Areas.Admin.Models.Roles
{
    public class RoutingModel : BaseWCoreEntityModel
    {
        public string Name { get; set; }
        public string Template { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DataToken { get; set; }
        public int? ParentId { get; set; }
    }
}
