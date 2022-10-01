using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using WCore.Core.Domain.Templates;
using WCore.Web.Areas.Admin.Models.Users;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Templates
{
    public class TemplateModel : BaseWCoreEntityModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public TemplateType TemplateType { get; set; }

        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemTemplate { get; set; }
        public List<SelectListItem> TemplateTypes { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
