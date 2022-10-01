using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Framework.Extensions;
using WCore.Services.Localization;
using WCore.Services.Users;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Users;
using WCore.Web.Models.ViewModels;
using System.Linq;
using WCore.Web.Models.Pages;
using System.Collections.Generic;
using WCore.Web.Models.Newses;
using WCore.Web.Models.Teams;
using WCore.Web.Models.Congresses;
using WCore.Web.Models.Academies;

namespace WCore.Web.ViewComponents
{
    public class UserRegistrationViewComponent : ViewComponent
    {
        public UserRegistrationViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new UserRegistrationFormModel();
            return View(model);
        }
    }
}
