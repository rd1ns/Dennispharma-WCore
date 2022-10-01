using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Core.Domain.Roles
{
    public class Routing : BaseEntity
    {
        public Routing()
        {
        }
        public string Name { get; set; }
        public string Template { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DataToken { get; set; }
        public int? ParentId { get; set; }

    }
}
