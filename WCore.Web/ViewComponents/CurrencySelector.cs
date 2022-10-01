using Microsoft.AspNetCore.Mvc;
using WCore.Web.Factories;

namespace WCore.Web.ViewComponents
{
    public class CurrencySelectorViewComponent : ViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public CurrencySelectorViewComponent(ICommonModelFactory commonModelFactory)
        {
            _commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareCurrencySelectorModel();
            if (model.AvailableCurrencies.Count == 1)
                return Content("");

            return View(model);
        }
    }
}
