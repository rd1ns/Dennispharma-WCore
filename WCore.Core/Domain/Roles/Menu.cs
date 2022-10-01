using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Core.Domain.Roles
{
    public class Menu : BaseEntity
    {
        public Menu()
        {
        }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string AreaName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public string ControllerGroup { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DataToken { get; set; }
        public string PluginName { get; set; }

    }
}
