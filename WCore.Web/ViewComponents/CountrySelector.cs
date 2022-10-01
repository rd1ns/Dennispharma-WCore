using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Framework.Extensions;
using WCore.Services.Localization;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.ViewModels;
using System.Linq;

namespace WCore.Web.ViewComponents
{
    public class CountrySelectorViewComponent : ViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public CountrySelectorViewComponent(ICommonModelFactory commonModelFactory)
        {
            _commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareCountrySelectorModel();

            if (model.AvailableCountries.Count == 1)
                return Content("");

            return View(model);
        }
    }
}
