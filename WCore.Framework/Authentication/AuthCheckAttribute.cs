using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using WCore.Core;
using WCore.Core.Infrastructure;
using WCore.Services.Logging;
using WCore.Services.Roles;
using System;
using System.Linq;
using System.Reflection;

namespace WCore.Framework.Authentication
{
    /// <summary>
    /// Represents middleware that enables authentication
    /// </summary>
    public class AuthCheckAttribute : Attribute, IAuthorizationFilter
    {
        bool _skip = false;
        public AuthCheckAttribute(bool skip)
        {
            this._skip = skip;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _workContext = EngineContext.Current.Resolve<IWorkContext>();
            var _logger = EngineContext.Current.Resolve<ILogger>();
            var _roleService = EngineContext.Current.Resolve<IRoleService>();

            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();

            var fullControllerName = controller + "Controller";


            var AllAssembly = Assembly.GetEntryAssembly().GetTypes().AsEnumerable();
            var WorkingController = AllAssembly.FirstOrDefault(type => type.Name == fullControllerName);
            var WorkingAction = WorkingController.GetMethods().FirstOrDefault(o => o.Name == action);


            if (!_skip)
            {

                if (_workContext.CurrentUser != null)
                {
                    var entity = _roleService.GetRoleByControllerAndActionName(_workContext.CurrentUser.RoleGroupId, controller, action);
                    if (entity == null)
                    {
                        var query = controller + "-Control Group Yetkilerinizde yok.";
                        _logger.Error("[ İzinsiz giriş : " + _workContext.CurrentUser.RoleGroupId + " ] - [ Controller : " + fullControllerName + " ] - [ Action : " + action + " ]", null, _workContext.CurrentUser);

                        context.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                            {"controller", "Auth"},
                            {"action", "Index"}
                            }
                        );
                    }
                }
                else
                {
                    var query = controller + "-Control Group Yetkilerinizde var.";
                    _logger.Error("[ İzinli giriş : KUllanıcı Yok ] - [ Controller : " + fullControllerName + " ] - [ Action : " + action + " ]");
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Auth"},
                            {"action", "Index"}
                        }
                    );
                }
            }

        }
    }
}
