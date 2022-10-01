using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WCore.Web.Areas.Admin.Controllers;
using WCore.Framework;
using WCore.Framework.Controllers;
using WCore.Framework.Authentication;
using WCore.Core;

namespace WCore.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthCheck(false)]
    public class BaseAdminController : BaseController
    {
        public int? CreatedUserId = null;

        public int? FindCreatedUserId(IWorkContext _workContext)
        {
            try
            {

                if (_workContext.CurrentUser.UserType == Core.Domain.Users.UserType.SystemAdministrator)
                    return _workContext.CurrentUser.Id;
                else
                {
                    return null;
                }
            }
            catch (System.Exception)
            {
                return null;
            }
        }

    }
}
