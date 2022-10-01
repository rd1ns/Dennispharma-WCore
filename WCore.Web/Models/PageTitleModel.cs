using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Web.Areas.Admin.Models.Users;
using System;
using System.Collections.Generic;

namespace WCore.Web.Models
{
    public class PageTitleModel
    {
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<BreadcrumbModel> Breadcrumb { get; set; }
    }
    public class BreadcrumbModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int ParentId { get; set; }
    }

}