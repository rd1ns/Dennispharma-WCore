using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Core.Domain.Logging;
using WCore.Framework.Extensions;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Logging;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Logs;
using WCore.Web.Areas.Admin.Models.Users;
using System.Linq;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class LogController : BaseAdminController
    {
        #region Fields
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public LogController(ILogger logger,
            IUserService userService,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._logger = logger;
            this._userService = userService;
            this._localizationService = localizationService;
            this._workContext = workContext;
        }
        #endregion

        #region Utilities
        #endregion

        #region Log View
        public IActionResult Index()
        {
            return Redirect("/Admin/Log/List");
        }

        public IActionResult List()
        {
            var searchModel = new LogSearchModel();
            var model = new LogModel();
            searchModel.LogLevels = model.LogLevel.ToSelectList().InsertEmptyFirst(_localizationService.GetResource("admin.configuration.all")).ToList();
            return View(searchModel);
        }
        [HttpPost]
        public virtual ActionResult GetFilteredItems(LogSearchModel searchModel)
        {
            var hotels = _logger.GetAllByFilters(logLevel: searchModel.LogLevel, skip: searchModel.skip, take: searchModel.take);

            var model = new LogListModel().PrepareToGrid(searchModel, hotels, () =>
            {
                return hotels.Select(hotel =>
                {
                    var m = hotel.ToModel<LogModel>();
                    m.LogLevelName = m.LogLevel.GetLocalizedEnum(_localizationService,_workContext);
                    return m;
                });
            });
            return Json(model);
        }


        public IActionResult AddOrEdit(int Id)
        {
            var entity = _logger.GetById(Id);

            if (entity == null)
                entity = new Log();

            var model = entity.ToModel<LogModel>();

            if (model.UserId.HasValue)
                model.User = _userService.GetById(model.UserId.Value).ToModel<UserModel>();

            model.LogLevelName = model.LogLevel.GetLocalizedEnum(_localizationService, _workContext);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult AddOrEdit(LogModel model, bool continueEditing, bool deleteAll, bool delete)
        {
            var entity = model.ToEntity<Log>();

            #region Delete
            if (delete)
            {
                _logger.Delete(entity.Id);
                return Json("Deleted");
            }
            if (deleteAll)
            {
                _logger.ClearLog();
                return Json("Deleted");
            }
            #endregion

            return Json(continueEditing);
        }
        #endregion
    }
}
