using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Localization;
using WCore.Services.Users;
using WCore.Web.Factories;

namespace WCore.Web.ViewComponents
{
    public class DynamicFormViewComponent : ViewComponent
    {
        private readonly IDynamicFormModelFactory _dynamicFormModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public DynamicFormViewComponent(IDynamicFormModelFactory dynamicFormModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._dynamicFormModelFactory = dynamicFormModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke(int dynamicFormId)
        {
            var model = _dynamicFormModelFactory.PrepareDynamicFormModel(dynamicFormId);
            return View(model);
        }
    }
}
