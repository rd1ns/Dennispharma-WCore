using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Web.Areas.Admin.Models
{

    public class GridMetaModel
    {
        public string Field { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Perpage { get; set; }
        public int Total { get; set; }
        public string Sort { get; set; }
    }
}
