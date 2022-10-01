using WCore.Framework.Models;
using System.Collections.Generic;

namespace WCore.Web.Models
{
    public class ControllerAndMethodModel : BaseWCoreEntityModel
    {
        public ControllerAndMethodModel()
        {
            ControllerAndMethods = new List<ControllerAndMethodModel>();
        }
        public int? ParentId { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public string AreaName { get; set; }
        public List<ControllerAndMethodModel> ControllerAndMethods { get; set; }

    }
}
