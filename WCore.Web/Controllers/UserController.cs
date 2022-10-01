using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Framework.Authentication;
using WCore.Services.Settings;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Users;
using WCore.Web.Models.ViewModels;
using WCore.Framework.Mvc.Filters;
using WCore.Core.Domain.Security;
using WCore.Services.Localization;
using WCore.Core.Domain.Users;
using WCore.Services.Orders;
using WCore.Services.Authentication;
using WCore.Services.Events;
using WCore.Services.Logging;
using WCore.Services.Common;
using WCore.Framework;
using WCore.Core.Http;
using WCore.Core.Domain;
using WCore.Framework.Controllers;
using System;
using WCore.Services.Messages;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WCore.Services.Tax;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Tax;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Messages;
using WCore.Framework.Validators;
using WCore.Core.Domain.Catalog;
using Microsoft.Extensions.Primitives;

namespace WCore.Web.Controllers
{
    [AuthCheck(false)]
    public class UserController : BasePublicController
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private readonly IUserAttributeService _userAttributeService;
        private readonly IUserAttributeParser _userAttributeParser;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ILocalizationService _localizationService;
        private readonly IUserModelFactory _userModelFactory;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;

        private readonly IAuthenticationService _authenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        private readonly ITaxService _taxService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly CaptchaSettings _captchaSettings;
        private readonly UserSettings _userSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly TaxSettings _taxSettings;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Utilities

        protected virtual string ParseCustomUserAttributes(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var attributesXml = "";
            var attributes = _userAttributeService.GetAllUserAttributes();
            foreach (var attribute in attributes)
            {
                var controlId = $"{WCoreUserServicesDefaults.UserAttributePrefix}{attribute.Id}";
                switch (attribute.AttributeControlTypeId)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.Checkboxes:
                        {
                            var cblAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(cblAttributes))
                            {
                                foreach (var item in cblAttributes.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    var selectedAttributeId = int.Parse(item);
                                    if (selectedAttributeId > 0)
                                        attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                            attribute, selectedAttributeId.ToString());
                                }
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //load read-only (already server-side selected) values
                            var attributeValues = _userAttributeService.GetUserAttributeValues(attribute.Id);
                            foreach (var selectedAttributeId in attributeValues
                                .Where(v => v.IsPreSelected)
                                .Select(v => v.Id)
                                .ToList())
                            {
                                attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var enteredText = ctrlAttributes.ToString().Trim();
                                attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                    attribute, enteredText);
                            }
                        }
                        break;
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                    case AttributeControlType.FileUpload:
                    //not supported user attributes
                    default:
                        break;
                }
            }

            return attributesXml;
        }

        #endregion

        #region Ctor
        public UserController(IUserService userService,
            IAddressService addressService,
            IUserAttributeService userAttributeService,
            IUserAttributeParser userAttributeParser,
            IUserActivityService userActivityService,
            IUserRegistrationService userRegistrationService,
            IShoppingCartService shoppingCartService,
            ILocalizationService localizationService,
            IUserModelFactory userModelFactory,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IAuthenticationService authenticationService,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            IWorkflowMessageService workflowMessageService,
            IWorkContext workContext,
            IWebHelper webHelper,
            ITaxService taxService,
            IStoreContext storeContext,
            ISettingService settingService,
            CaptchaSettings captchaSettings,
            UserSettings userSettings,
            StoreInformationSettings storeInformationSettings,
            DateTimeSettings dateTimeSettings,
            TaxSettings taxSettings,
            LocalizationSettings localizationSettings)
        {
            this._userService = userService;
            this._addressService= addressService;
            this._userAttributeService= userAttributeService;
            this._userAttributeParser = userAttributeParser;
            this._userActivityService = userActivityService;
            this._userRegistrationService = userRegistrationService;
            this._shoppingCartService = shoppingCartService;
            this._localizationService = localizationService;
            this._userModelFactory = userModelFactory;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;

            this._authenticationService = authenticationService;
            this._eventPublisher = eventPublisher;
            this._genericAttributeService = genericAttributeService;
            this._workflowMessageService = workflowMessageService;
            this._workContext = workContext;
            this._webHelper= webHelper;
            this._taxService = taxService;
            this._storeContext = storeContext;
            this._settingService = settingService;
            this._captchaSettings = captchaSettings;
            this._userSettings = userSettings;
            this._storeInformationSettings = storeInformationSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._taxSettings = taxSettings;
            this._localizationSettings = localizationSettings;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            var model = _workContext.CurrentUser.ToModel<UserModel>();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(UserModel model)
        {
            return View(model);
        }
        public IActionResult Orders()
        {
            return View();
        }

        #endregion
    }
}
