using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Domain.Roles;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace WCore.Web.Areas.Admin.Models.Roles
{
    public class RoleModel : BaseWCoreEntityModel
    {
        [WCoreResourceDisplayName("Admin.Configuration.RoleGroup")]
        public int RoleGroupId { get; set; }
        public virtual RoleGroupModel RoleGroup { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Menu")]
        public int MenuId { get; set; }
        public virtual MenuModel Menu { get; set; }
    }
    public class RoleGroupModel : BaseWCoreEntityModel
    {
        public RoleGroupModel()
        {
            RoleGroupTypes = new List<SelectListItem>();
        }
        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.RoleGroupType")]
        public RoleGroupType RoleGroupType { get; set; }
        public string RoleGroupTypeName { get; set; }
        public List<SelectListItem> RoleGroupTypes { get; set; }
    }
}
