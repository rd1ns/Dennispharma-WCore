using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCore.Framework.Mvc.Routing;
using WCore.Core.Infrastructure;
using WCore.Services.Roles;
using WCore.Services.Menus;
using System.Reflection;
using WCore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using WCore.Core.Domain.Roles;
using WCore.Core.Caching;

namespace WCore.Web.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : BaseRouteProvider, IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var pattern = GetRouterPattern(endpointRouteBuilder);

            //areas
            endpointRouteBuilder.MapControllerRoute(name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //areas
            endpointRouteBuilder.MapControllerRoute(name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //auth page
            endpointRouteBuilder.MapControllerRoute("Auth", $"{pattern}auth",
                new { controller = "Auth", action = "Index" });

            //auth page
            endpointRouteBuilder.MapControllerRoute("Auth", $"{pattern}logout",
                new { controller = "Auth", action = "Logout" });

            //login
            endpointRouteBuilder.MapControllerRoute("UserRegistration", $"{pattern}UserRegistration/",
                new { controller = "Common", action = "UserRegistration" });


            //change currency (AJAX link)
            endpointRouteBuilder.MapControllerRoute("ChangeCurrency", pattern + "changecurrency/{usercurrency:min(0)}",
                new { controller = "Common", action = "SetCurrency" });

            //change language (AJAX link)
            endpointRouteBuilder.MapControllerRoute("ChangeLanguage", pattern + "changelanguage/{langid:min(0)}",
                new { controller = "Common", action = "SetLanguage" });

            //change language (AJAX link)
            endpointRouteBuilder.MapControllerRoute("ChangeCountry", pattern + "changecountry/{usercountry:min(0)}",
                new { controller = "Common", action = "SetCountry" });

            //home page
            endpointRouteBuilder.MapControllerRoute(name: "Homepage",
                pattern: $"{pattern}",
                defaults: new { controller = "Home", action = "Index" });

            //page not found
            endpointRouteBuilder.MapControllerRoute("PageNotFound", $"{pattern}page-not-found",
                new { controller = "Common", action = "PageNotFound" });

            //error page
            endpointRouteBuilder.MapControllerRoute("Error", $"{pattern}error",
                new { controller = "Common", action = "Error" });

            endpointRouteBuilder.MapControllerRoute("ShoppingCart", $"{pattern}shoppingcart",
                new { controller = "ShoppingCart", action = "Index" });

            //skiresorts
            endpointRouteBuilder.MapControllerRoute("ShoppingCart_Mail", $"{pattern}ShoppingCart/Mail",
                new { controller = "ShoppingCart", action = "Mail" });
            //skiresorts
            endpointRouteBuilder.MapControllerRoute("ShoppingCart_Completed", $"{pattern}ShoppingCart/Completed",
                new { controller = "ShoppingCart", action = "Completed" });

            //skiresorts
            endpointRouteBuilder.MapControllerRoute("TourCategoryList", $"{pattern}tour-categories",
                new { controller = "TourCategory", action = "List" });

            //checkout pages
            endpointRouteBuilder.MapControllerRoute("Checkout", $"{pattern}checkout/",
                new { controller = "Checkout", action = "Index" });

            endpointRouteBuilder.MapControllerRoute("CheckoutOnePage", $"{pattern}onepagecheckout/",
                new { controller = "Checkout", action = "OnePageCheckout" });

            endpointRouteBuilder.MapControllerRoute("CheckoutShippingAddress", $"{pattern}checkout/shippingaddress",
                new { controller = "Checkout", action = "ShippingAddress" });

            endpointRouteBuilder.MapControllerRoute("CheckoutSelectShippingAddress", $"{pattern}checkout/selectshippingaddress",
                new { controller = "Checkout", action = "SelectShippingAddress" });

            endpointRouteBuilder.MapControllerRoute("CheckoutBillingAddress", $"{pattern}checkout/billingaddress",
                new { controller = "Checkout", action = "BillingAddress" });

            endpointRouteBuilder.MapControllerRoute("CheckoutSelectBillingAddress", $"{pattern}checkout/selectbillingaddress",
                new { controller = "Checkout", action = "SelectBillingAddress" });

            endpointRouteBuilder.MapControllerRoute("CheckoutShippingMethod", $"{pattern}checkout/shippingmethod",
                new { controller = "Checkout", action = "ShippingMethod" });

            endpointRouteBuilder.MapControllerRoute("CheckoutPaymentMethod", $"{pattern}checkout/paymentmethod",
                new { controller = "Checkout", action = "PaymentMethod" });

            endpointRouteBuilder.MapControllerRoute("CheckoutPaymentInfo", $"{pattern}checkout/paymentinfo",
                new { controller = "Checkout", action = "PaymentInfo" });

            endpointRouteBuilder.MapControllerRoute("CheckoutConfirm", $"{pattern}checkout/confirm",
                new { controller = "Checkout", action = "Confirm" });

            endpointRouteBuilder.MapControllerRoute("CheckoutCompleted",
                pattern + "checkout/completed/{orderId:int}",
                new { controller = "Checkout", action = "Completed" });

            var useRoute = true;

            if (useRoute)
            {
                #region Find Controller

                var _routingService = EngineContext.Current.Resolve<IRoutingService>();
                var _menuService = EngineContext.Current.Resolve<IMenuService>();

                int i = 1;
                var controllerAndMethods = new List<ControllerAndMethodModel>();
                Assembly asm = Assembly.GetExecutingAssembly();

                asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).SelectMany(type => type.GetMethods());
                var ins = asm.ExportedTypes.Where(o => o.Name.Contains("Controller"));

                foreach (var controllers in ins)
                {
                    var areaName = controllers.FullName.Contains("Areas") ? "Admin" : "";
                    var trole = new ControllerAndMethodModel();
                    trole.Id = i;
                    var controller = controllers.GetMethods().Where(o => o.DeclaringType.FullName == controllers.FullName).Where(o => !o.IsSpecialName).ToList();
                    var mm = controllers.GetMembers();
                    trole.ControllerName = controllers.Name;
                    trole.AreaName = areaName;
                    trole.ControllerAndMethods.AddRange(controller.Select(o =>
                    {
                        var areaName = controllers.FullName.Contains("Areas") ? "Admin" : "";
                        var met = new ControllerAndMethodModel();
                        met.MethodName = o.Name;
                        met.ParentId = i;
                        met.AreaName = areaName;
                        return met;
                    }));
                    i++;
                    controllerAndMethods.Add(trole);
                }
                #endregion

                var Methods = new List<Menu>();

                #region Insert Routing
                foreach (var controller in controllerAndMethods)
                {
                    var Cache = EngineContext.Current.Resolve<IStaticCacheManager>();
                    int? parentId = null;
                    var dbMenu = _menuService.GetMenuById(ControllerName: controller.ControllerName.Replace("Controller", ""), AreaName: controller.AreaName);
                    var areaName = string.IsNullOrEmpty(controller.AreaName) ? "" : "." + controller.AreaName;
                    Methods.Add(new Menu()
                    {
                        Action = "Index",
                        Controller = controller.ControllerName.Replace("Controller", ""),
                        Name = "routing." + controller.ControllerName.ToLowerInvariant() + "." + "title",
                        Slug = "/" + controller.ControllerName.Replace("Controller", "").ToLowerInvariant() + "/" + "Index",
                        DataToken = controller.ControllerName,
                        AreaName = controller.AreaName
                    });
                    if (dbMenu == null)
                    {
                        var tempMenu = new Menu()
                        {
                            Action = "Index",
                            Controller = controller.ControllerName.Replace("Controller", ""),
                            Name = "routing" + areaName + "." + controller.ControllerName.ToLowerInvariant() + "." + "title",
                            Slug = "/" + controller.ControllerName.Replace("Controller", "").ToLowerInvariant() + "/" + "Index",
                            DataToken = controller.ControllerName,
                            AreaName = controller.AreaName
                        };
                        var insertedId = _menuService.Insert(tempMenu);
                        parentId = tempMenu.Id;
                    }
                    foreach (var method in controller.ControllerAndMethods)
                    {
                        var dbRoute = _menuService.GetMenuById(ControllerName: controller.ControllerName.Replace("Controller", ""), ActionName: method.MethodName, AreaName: controller.AreaName);
                        Methods.Add(new Menu()
                        {
                            Action = method.MethodName,
                            Controller = controller.ControllerName.Replace("Controller", ""),
                            Name = "routing" + areaName + "." + controller.ControllerName.ToLowerInvariant() + "." + method.MethodName.ToLowerInvariant(),
                            Slug = "/" + controller.ControllerName.Replace("Controller", "").ToLowerInvariant() + "/" + method.MethodName.ToLowerInvariant(),
                            DataToken = controller.ControllerName,
                            ParentId = parentId == null ? dbMenu.Id : parentId,
                            AreaName = controller.AreaName
                        });
                        if (dbRoute == null)
                        {
                            var tempMethod = new Menu()
                            {
                                Action = method.MethodName,
                                Controller = controller.ControllerName.Replace("Controller", ""),
                                Name = "routing" + areaName + "." + controller.ControllerName.ToLowerInvariant() + "." + method.MethodName.ToLowerInvariant(),
                                Slug = "/" + controller.ControllerName.Replace("Controller", "").ToLowerInvariant() + "/" + method.MethodName.ToLowerInvariant(),
                                DataToken = controller.ControllerName,
                                ParentId = parentId == null ? dbMenu.Id : parentId,
                                AreaName = controller.AreaName
                            };
                            _menuService.Insert(tempMethod);
                        }
                    }
                }


                #endregion

                #region GetRouting
                var getallRouting = _menuService.GetAll();

                foreach (var item in getallRouting)
                {
                    if (Methods.Where(o => o.Name == item.Name).Any())
                    {

                    }
                    else
                    {
                        if (item.PluginName == "")
                        {
                            _menuService.Delete(item.Id);
                        }
                    }

                }
                var _Cache = EngineContext.Current.Resolve<IStaticCacheManager>();
                var _getallRouting = _menuService.GetAll();
                var _fitered = _getallRouting.Where(o => o.PluginName == null).ToList();
                foreach (var route in _fitered)
                {
                    endpointRouteBuilder.MapControllerRoute(
                        name: route.Name,
                        pattern: route.Slug,
                        defaults: new { controller = route.Controller, action = route.Action },
                        constraints: null,
                        dataTokens: new { route.DataToken });
                }
                #endregion
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 0;

        #endregion
    }
}
