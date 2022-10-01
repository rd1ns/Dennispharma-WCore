using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCore.Web.Models
{
    public enum ProcessType
    {
        Added = 0,
        Updated = 1,
        Deleted = 2,
        Cancel = 99
    }
}
