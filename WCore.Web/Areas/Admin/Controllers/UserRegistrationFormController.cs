using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Users;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Users;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class UserRegistrationFormController : BaseAdminController
    {
        #region Fields
        private readonly IUserRegistrationFormService _userRegistrationFormService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public UserRegistrationFormController(IUserRegistrationFormService userRegistrationFormService,
            IUserService userService,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._userRegistrationFormService = userRegistrationFormService;
            this._userService = userService;
            this._localizationService = localizationService;
            this._workContext = workContext;
        }
        #endregion

        #region Utilities
        #endregion

        #region UserRegistrationForm View
        public IActionResult Index()
        {
            return Redirect("/Admin/UserRegistrationForm/List");
        }

        public IActionResult List()
        {
            var searchModel = new UserRegistrationFormSearchModel();
            return View(searchModel);
        }
        [HttpPost]
        public virtual ActionResult GetFilteredItems(UserRegistrationFormSearchModel searchModel)
        {
            var hotels = _userRegistrationFormService.GetAllByFilters(searchModel.Query, skip: searchModel.skip, take: searchModel.take);

            var model = new UserRegistrationFormListModel().PrepareToGrid(searchModel, hotels, () =>
            {
                return hotels.Select(hotel =>
                {
                    var m = hotel.ToModel<UserRegistrationFormModel>();
                    return m;
                });
            });
            return Json(model);
        }


        public IActionResult AddOrEdit(int Id)
        {
            var entity = _userRegistrationFormService.GetById(Id);

            if (entity == null)
                entity = new UserRegistrationForm();

            var model = entity.ToModel<UserRegistrationFormModel>();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult AddOrEdit(UserRegistrationFormModel model, bool continueEditing, bool deleteAll, bool delete)
        {
            var entity = model.ToEntity<UserRegistrationForm>();

            #region Delete
            if (delete)
            {
                _userRegistrationFormService.Delete(entity.Id);
                return Json("Deleted");
            }
            #endregion

            return Json(continueEditing);
        }
        #endregion
    }
}
