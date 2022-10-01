using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.Settings;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Common;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Directory;
using System.Linq;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class CurrencyController : BaseAdminController
    {
        #region Fields
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor
        public CurrencyController(ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._webHostEnvironment = webHostEnvironment;
            this._settingService = settingService;
            this._webHelper = webHelper;

        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/Currency/List");
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetFilteredItems()
        {
            var _skip = int.Parse(Request.Form["skip"]);
            var _take = int.Parse(Request.Form["take"]);
            var _query = Request.Form["filter[filters][0][value]"].ToString();

            var currencies = _currencyService.GetAllByFilters(searchValue: _query, skip: _skip, take: _take);
            var searchModel = new CurrencySearchModel();

            //prepare list model
            var model = new CurrencyListModel().PrepareToGrid(searchModel, currencies, () =>
            {
                return currencies.Select(currency =>
                {
                    //fill in model values from the entity
                    var currencyModel = currency.ToModel<CurrencyModel>();

                    //fill in additional values (not existing in the entity)
                    currencyModel.IsPrimaryExchangeRateCurrency = currency.Id == _currencySettings.PrimaryExchangeRateCurrencyId;
                    currencyModel.IsPrimaryStoreCurrency = currency.Id == _currencySettings.PrimaryStoreCurrencyId;

                    return currencyModel;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _currencyService.GetById(Id).ToModel<CurrencyModel>();

            if (entity == null)
                entity = new CurrencyModel();

            return View(entity);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public IActionResult AddOrEdit(CurrencyModel currency, bool continueEditing, bool sendInfo, bool delete)
        {
            var entity = currency.ToEntity<Currency>();

            if (delete)
            {
                var _entity = _currencyService.GetById(currency.Id);
                _entity.Published = true;
                _currencyService.Update(_entity);
                return Json("Deleted");
            }


            if (currency.Id == 0)
            {
                _currencyService.Insert(entity);
            }
            else
            {
                _currencyService.Update(entity);
            }

            return Json(continueEditing);
        }
        #endregion
    }
}
