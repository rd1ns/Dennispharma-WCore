using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WCore.Core;
using WCore.Core.Domain;
using WCore.Framework;
using WCore.Framework.Authentication;
using WCore.Services;
using WCore.Framework.Controllers;
using WCore.Framework.Mvc.Filters;
using WCore.Core.Infrastructure;

namespace WCore.Web.Controllers
{
    [WwwRequirement]
    [CheckLanguageSeoCode]
    [AuthCheck(true)]
    public class BasePublicController : BaseController
    {
        public int? CreatedUserId = null;
        public BasePublicController()
        {
        }

        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}
