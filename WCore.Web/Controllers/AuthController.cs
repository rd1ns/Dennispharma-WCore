using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Core.Domain.Users;
using WCore.Framework.Authentication;
using WCore.Services.Authentication;
using WCore.Services.Localization;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Users;
using System;
using WCore.Services.Common;
using WCore.Services.Logging;
using WCore.Services.Orders;
using WCore.Web.Factories;
using WCore.Services.Messages;
using WCore.Services.Events;
using WCore.Services.Tax;
using WCore.Services.Settings;
using WCore.Core.Domain.Security;
using WCore.Core.Domain;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Tax;
using WCore.Framework.Mvc.Filters;
using WCore.Core.Http;
using WCore.Framework.Controllers;
using WCore.Framework;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WCore.Core.Domain.Catalog;
using Microsoft.Extensions.Primitives;
using WCore.Core.Domain.Messages;
using WCore.Core.Domain.Common;
using WCore.Framework.Validators;

namespace WCore.Web.Controllers
{
    [AuthCheck(true)]
    public class AuthController : BasePublicController
    {
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

        #region Ctor
        public AuthController(IUserService userService,
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
            this._addressService = addressService;
            this._userAttributeService = userAttributeService;
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
            this._webHelper = webHelper;
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

        public IActionResult Index(bool? checkoutAsGuest)
        {
            var model = new LoginOrRegisterModel();

            model.Login = _userModelFactory.PrepareLoginModel(checkoutAsGuest);

            var registerModel = new RegisterModel();
            model.Register = _userModelFactory.PrepareRegisterModel(registerModel, false, setDefaultValues: true);


            //check whether registration is allowed
            if (_userSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });


            var user = _workContext.CurrentUser;
            if (user.UserType != Core.Domain.Users.UserType.Guest)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);

        }

        #region Login / logout

        [HttpsRequirement]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Login(bool? checkoutAsGuest)
        {
            var model = _userModelFactory.PrepareLoginModel(checkoutAsGuest);
            return View(model);
        }

        [HttpPost]
        [ValidateCaptcha]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Login(LoginModel model, string returnUrl, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptchaMessage"));
            }

            if (ModelState.IsValid)
            {
                if (_userSettings.UsernamesEnabled && model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }
                var loginResult = _userRegistrationService.ValidateUser(_userSettings.UsernamesEnabled ? model.Username : model.Email, model.Password);
                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            var user = _userSettings.UsernamesEnabled
                                ? _userService.GetUserByUsername(model.Username)
                                : _userService.GetUserByEmail(model.Email);

                            //migrate shopping cart
                            //_shoppingCartService.MigrateShoppingCart(_workContext.CurrentUser, user, true);

                            //sign in new user
                            _authenticationService.SignIn(user, model.RememberMe);

                            //raise event       
                            _eventPublisher.Publish(new UserLoggedinEvent(user));

                            //activity log
                            _userActivityService.InsertActivity(user, "PublicStore.Login",
                                _localizationService.GetResource("ActivityLog.PublicStore.Login"), user);

                            return Json("OK");
                        }
                    case UserLoginResults.UserNotExist:
                        model.ErorrMessage= _localizationService.GetResource("Account.Login.WrongCredentials.UserNotExist");
                        break;
                    case UserLoginResults.Deleted:
                        model.ErorrMessage = _localizationService.GetResource("Account.Login.WrongCredentials.Deleted");
                        break;
                    case UserLoginResults.NotActive:
                        model.ErorrMessage = _localizationService.GetResource("Account.Login.WrongCredentials.NotActive");
                        break;
                    case UserLoginResults.NotRegistered:
                        model.ErorrMessage = _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered");
                        break;
                    case UserLoginResults.LockedOut:
                        model.ErorrMessage = _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut");
                        break;
                    case UserLoginResults.WrongPassword:
                    default:
                        model.ErorrMessage = _localizationService.GetResource("Account.Login.WrongCredentials");
                        break;
                }
            }
            return Json(model);
        }

        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Logout()
        {
            if (_workContext.OriginalUserIfImpersonated != null)
            {
                //activity log
                _userActivityService.InsertActivity(_workContext.OriginalUserIfImpersonated, "Impersonation.Finished",
                    string.Format(_localizationService.GetResource("ActivityLog.Impersonation.Finished.StoreOwner"),
                        _workContext.CurrentUser.Email, _workContext.CurrentUser.Id),
                    _workContext.CurrentUser);

                _userActivityService.InsertActivity("Impersonation.Finished",
                    string.Format(_localizationService.GetResource("ActivityLog.Impersonation.Finished.User"),
                        _workContext.OriginalUserIfImpersonated.Email, _workContext.OriginalUserIfImpersonated.Id),
                    _workContext.OriginalUserIfImpersonated);

                //logout impersonated user
                _genericAttributeService
                    .SaveAttribute<int?>(_workContext.OriginalUserIfImpersonated, WCoreUserDefaults.ImpersonatedUserIdAttribute, null);

                //redirect back to user details page (admin area)
                return RedirectToAction("Edit", "User", new { id = _workContext.CurrentUser.Id, area = AreaNames.Admin });
            }

            //activity log
            _userActivityService.InsertActivity(_workContext.CurrentUser, "PublicStore.Logout",
                _localizationService.GetResource("ActivityLog.PublicStore.Logout"), _workContext.CurrentUser);

            //standard logout 
            _authenticationService.SignOut();

            //raise logged out event       
            _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

            //EU Cookie
            if (_storeInformationSettings.DisplayEuCookieLawWarning)
            {
                //the cookie law message should not pop up immediately after logout.
                //otherwise, the user will have to click it again...
                //and thus next visitor will not click it... so violation for that cookie law..
                //the only good solution in this case is to store a temporary variable
                //indicating that the EU cookie popup window should not be displayed on the next page open (after logout redirection to homepage)
                //but it'll be displayed for further page loads
                TempData[$"{WCoreCookieDefaults.Prefix}{WCoreCookieDefaults.IgnoreEuCookieLawWarning}"] = true;
            }

            return RedirectToRoute("Homepage");
        }

        #endregion

        #region Password recovery

        [HttpsRequirement]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecovery()
        {
            var model = new PasswordRecoveryModel();
            model = _userModelFactory.PreparePasswordRecoveryModel(model);

            return View(model);
        }

        [ValidateCaptcha]
        [HttpPost, ActionName("PasswordRecovery")]
        [FormValueRequired("send-email")]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecoverySend(PasswordRecoveryModel model, bool captchaValid)
        {
            // validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnForgotPasswordPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptchaMessage"));
            }

            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByEmail(model.Email);
                if (user != null && user.Active && !user.Deleted)
                {
                    //save token and current date
                    var passwordRecoveryToken = Guid.NewGuid();
                    _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.PasswordRecoveryTokenAttribute,
                        passwordRecoveryToken.ToString());
                    DateTime? generatedDateTime = DateTime.Now;
                    _genericAttributeService.SaveAttribute(user,
                        WCoreUserDefaults.PasswordRecoveryTokenDateGeneratedAttribute, generatedDateTime);

                    //send email
                    _workflowMessageService.SendUserPasswordRecoveryMessage(user,
                        _workContext.WorkingLanguage.Id);

                    model.Result = _localizationService.GetResource("Account.PasswordRecovery.EmailHasBeenSent");
                }
                else
                {
                    model.Result = _localizationService.GetResource("Account.PasswordRecovery.EmailNotFound");
                }
            }

            model = _userModelFactory.PreparePasswordRecoveryModel(model);
            return View(model);
        }

        [HttpsRequirement]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecoveryConfirm(string token, string email, Guid guid)
        {
            //For backward compatibility with previous versions where email was used as a parameter in the URL
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                user = _userService.GetUserByGuid(guid);

            if (user == null)
                return RedirectToRoute("Homepage");

            if (string.IsNullOrEmpty(_genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.PasswordRecoveryTokenAttribute)))
            {
                return base.View(new PasswordRecoveryConfirmModel
                {
                    DisablePasswordChanging = true,
                    Result = _localizationService.GetResource("Account.PasswordRecovery.PasswordAlreadyHasBeenChanged")
                });
            }

            var model = _userModelFactory.PreparePasswordRecoveryConfirmModel();

            //validate token
            if (!_userService.IsPasswordRecoveryTokenValid(user, token))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.WrongToken");
            }

            //validate token expiration date
            if (_userService.IsPasswordRecoveryLinkExpired(user))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.LinkExpired");
            }

            return View(model);
        }

        [HttpPost, ActionName("PasswordRecoveryConfirm")]
        [FormValueRequired("set-password")]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecoveryConfirmPOST(string token, string email, Guid guid, PasswordRecoveryConfirmModel model)
        {
            //For backward compatibility with previous versions where email was used as a parameter in the URL
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                user = _userService.GetUserByGuid(guid);

            if (user == null)
                return RedirectToRoute("Homepage");

            //validate token
            if (!_userService.IsPasswordRecoveryTokenValid(user, token))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.WrongToken");
                return View(model);
            }

            //validate token expiration date
            if (_userService.IsPasswordRecoveryLinkExpired(user))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.LinkExpired");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var response = _userRegistrationService.ChangePassword(new ChangePasswordRequest(user.Email,
                    false, _userSettings.DefaultPasswordFormat, model.NewPassword));
                if (response.Success)
                {
                    _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.PasswordRecoveryTokenAttribute, "");

                    model.DisablePasswordChanging = true;
                    model.Result = _localizationService.GetResource("Account.PasswordRecovery.PasswordHasBeenChanged");
                }
                else
                {
                    model.Result = response.Errors.FirstOrDefault();
                }

                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion     

        #region Register

        [HttpsRequirement]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Register()
        {
            //check whether registration is allowed
            if (_userSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            var model = new RegisterModel();
            model = _userModelFactory.PrepareRegisterModel(model, false, setDefaultValues: true);

            return View(model);
        }

        [HttpPost]
        [ValidateCaptcha]
        [ValidateHoneypot]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Register(RegisterModel model, string returnUrl, bool captchaValid, IFormCollection form)
        {
            //check whether registration is allowed
            if (_userSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            if (_userService.IsRegistered(_workContext.CurrentUser))
            {
                //Already registered user. 
                _authenticationService.SignOut();

                //raise logged out event       
                _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

                //Save a new record
                _workContext.CurrentUser = _userService.InsertGuestUser();
            }
            var user = _workContext.CurrentUser;
            user.RegisteredInStoreId = _storeContext.CurrentStore.Id;

            //custom user attributes
            var userAttributesXml = ParseCustomUserAttributes(form);
            var userAttributeWarnings = _userAttributeParser.GetAttributeWarnings(userAttributesXml);
            foreach (var error in userAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptchaMessage"));
            }

            if (ModelState.IsValid)
            {
                if (_userSettings.UsernamesEnabled && model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }

                var isApproved = _userSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new UserRegistrationRequest(user,
                    model.Email,
                    _userSettings.UsernamesEnabled ? model.Username : model.Email,
                    model.Password,
                    _userSettings.DefaultPasswordFormat,
                    _storeContext.CurrentStore.Id,
                    isApproved);
                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest);
                if (registrationResult.Success)
                {
                    //properties
                    if (_dateTimeSettings.AllowUsersToSetTimeZone)
                    {
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.TimeZoneIdAttribute, model.TimeZoneId);
                    }
                    //VAT number
                    if (_taxSettings.EuVatEnabled)
                    {
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.VatNumberAttribute, model.VatNumber);

                        var vatNumberStatus = _taxService.GetVatNumberStatus(model.VatNumber, out _, out var vatAddress);
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.VatNumberStatusIdAttribute, (int)vatNumberStatus);
                        //send VAT number admin notification
                        if (!string.IsNullOrEmpty(model.VatNumber) && _taxSettings.EuVatEmailAdminWhenNewVatSubmitted)
                            _workflowMessageService.SendNewVatSubmittedStoreOwnerNotification(user, model.VatNumber, vatAddress, _localizationSettings.DefaultAdminLanguageId);
                    }

                    //form fields
                    if (_userSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.GenderAttribute, model.Gender);
                    if (_userSettings.FirstNameEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.FirstNameAttribute, model.FirstName);
                    if (_userSettings.LastNameEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.LastNameAttribute, model.LastName);
                    if (_userSettings.DateOfBirthEnabled)
                    {
                        var dateOfBirth = model.ParseDateOfBirth();
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.DateOfBirthAttribute, dateOfBirth);
                    }
                    if (_userSettings.CompanyEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.CompanyAttribute, model.Company);
                    if (_userSettings.StreetAddressEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.StreetAddressAttribute, model.StreetAddress);
                    if (_userSettings.StreetAddress2Enabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.StreetAddress2Attribute, model.StreetAddress2);
                    if (_userSettings.ZipPostalCodeEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.ZipPostalCodeAttribute, model.ZipPostalCode);
                    if (_userSettings.CityEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.CityAttribute, model.City);
                    if (_userSettings.CountyEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.CountyAttribute, model.County);
                    if (_userSettings.CountryEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.CountryIdAttribute, model.CountryId);
                    if (_userSettings.CountryEnabled && _userSettings.StateProvinceEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.StateProvinceIdAttribute,
                            model.StateProvinceId);
                    if (_userSettings.PhoneEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.PhoneAttribute, model.Phone);
                    if (_userSettings.FaxEnabled)
                        _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.FaxAttribute, model.Fax);

                    //newsletter
                    if (_userSettings.NewsletterEnabled)
                    {
                        //save newsletter value
                        var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(model.Email, _storeContext.CurrentStore.Id);
                        if (newsletter != null)
                        {
                            if (model.Newsletter)
                            {
                                newsletter.Active = true;
                                _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsletter);
                            }
                            //else
                            //{
                            //When registering, not checking the newsletter check box should not take an existing email address off of the subscription list.
                            //_newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletter);
                            //}
                        }
                        else
                        {
                            if (model.Newsletter)
                            {
                                _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription
                                {
                                    NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                    Email = model.Email,
                                    Active = true,
                                    StoreId = _storeContext.CurrentStore.Id,
                                    CreatedOn = DateTime.Now
                                });
                            }
                        }
                    }

                    if (_userSettings.AcceptPrivacyPolicyEnabled)
                    {
                    }

                    //save user attributes
                    _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.CustomUserAttributes, userAttributesXml);

                    //login user now
                    if (isApproved)
                        _authenticationService.SignIn(user, true);

                    //insert default address (if possible)
                    var defaultAddress = new Address
                    {
                        FirstName = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.FirstNameAttribute),
                        LastName = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.LastNameAttribute),
                        Email = user.Email,
                        Company = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.CompanyAttribute),
                        CountryId = _genericAttributeService.GetAttribute<int>(user, WCoreUserDefaults.CountryIdAttribute) > 0
                            ? (int?)_genericAttributeService.GetAttribute<int>(user, WCoreUserDefaults.CountryIdAttribute)
                            : null,
                        StateProvinceId = _genericAttributeService.GetAttribute<int>(user, WCoreUserDefaults.StateProvinceIdAttribute) > 0
                            ? (int?)_genericAttributeService.GetAttribute<int>(user, WCoreUserDefaults.StateProvinceIdAttribute)
                            : null,
                        County = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.CountyAttribute),
                        City = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.CityAttribute),
                        Address1 = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.StreetAddressAttribute),
                        Address2 = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.StreetAddress2Attribute),
                        ZipPostalCode = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.ZipPostalCodeAttribute),
                        PhoneNumber = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.PhoneAttribute),
                        FaxNumber = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.FaxAttribute),
                        CreatedOn = user.CreatedOn
                    };
                    if (_addressService.IsAddressValid(defaultAddress))
                    {
                        //some validation
                        if (defaultAddress.CountryId == 0)
                            defaultAddress.CountryId = null;
                        if (defaultAddress.StateProvinceId == 0)
                            defaultAddress.StateProvinceId = null;
                        //set default address
                        //user.Addresses.Add(defaultAddress);

                        _addressService.InsertAddress(defaultAddress);

                        _userService.InsertUserAddress(user, defaultAddress);

                        user.BillingAddressId = defaultAddress.Id;
                        user.ShippingAddressId = defaultAddress.Id;

                        _userService.Update(user);
                    }

                    //notifications
                    if (_userSettings.NotifyNewUserRegistration)
                        _workflowMessageService.SendUserRegisteredNotificationMessage(user,
                            _localizationSettings.DefaultAdminLanguageId);

                    //raise event       
                    _eventPublisher.Publish(new UserRegisteredEvent(user));

                    switch (_userSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.EmailValidation:
                            {
                                //email validation message
                                _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());
                                _workflowMessageService.SendUserEmailValidationMessage(user, _workContext.WorkingLanguage.Id);

                                //result
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.EmailValidation });
                            }
                        case UserRegistrationType.AdminApproval:
                            {
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.AdminApproval });
                            }
                        case UserRegistrationType.Standard:
                            {
                                //send user welcome message
                                _workflowMessageService.SendUserWelcomeMessage(user, _workContext.WorkingLanguage.Id);

                                //raise event       
                                _eventPublisher.Publish(new UserActivatedEvent(user));

                                var redirectUrl = Url.RouteUrl("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.Standard, returnUrl }, _webHelper.CurrentRequestProtocol);
                                return Json(user);
                            }
                        default:
                            {
                                return Json(user);
                            }
                    }
                }

                //errors
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareRegisterModel(model, true, userAttributesXml);
            return View(model);
        }

        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult RegisterResult(int resultId)
        {
            var model = _userModelFactory.PrepareRegisterResultModel(resultId);
            return View(model);
        }

        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public virtual IActionResult RegisterResult(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToRoute("Homepage");

            return Redirect(returnUrl);
        }

        [HttpPost]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult CheckUsernameAvailability(string username)
        {
            var usernameAvailable = false;
            var statusText = _localizationService.GetResource("Account.CheckUsernameAvailability.NotAvailable");

            if (!UsernamePropertyValidator.IsValid(username, _userSettings))
            {
                statusText = _localizationService.GetResource("Account.Fields.Username.NotValid");
            }
            else if (_userSettings.UsernamesEnabled && !string.IsNullOrWhiteSpace(username))
            {
                if (_workContext.CurrentUser != null &&
                    _workContext.CurrentUser.Username != null &&
                    _workContext.CurrentUser.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    statusText = _localizationService.GetResource("Account.CheckUsernameAvailability.CurrentUsername");
                }
                else
                {
                    var user = _userService.GetUserByUsername(username);
                    if (user == null)
                    {
                        statusText = _localizationService.GetResource("Account.CheckUsernameAvailability.Available");
                        usernameAvailable = true;
                    }
                }
            }

            return Json(new { Available = usernameAvailable, Text = statusText });
        }

        [HttpsRequirement]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult AccountActivation(string token, string email, Guid guid)
        {
            //For backward compatibility with previous versions where email was used as a parameter in the URL
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                user = _userService.GetUserByGuid(guid);

            if (user == null)
                return RedirectToRoute("Homepage");

            var cToken = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.AccountActivationTokenAttribute);
            if (string.IsNullOrEmpty(cToken))
                return
                    View(new AccountActivationModel
                    {
                        Result = _localizationService.GetResource("Account.AccountActivation.AlreadyActivated")
                    });

            if (!cToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return RedirectToRoute("Homepage");

            //activate user account
            user.Active = true;
            _userService.Update(user);
            _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.AccountActivationTokenAttribute, "");
            //send welcome message
            _workflowMessageService.SendUserWelcomeMessage(user, _workContext.WorkingLanguage.Id);

            //raise event       
            _eventPublisher.Publish(new UserActivatedEvent(user));

            var model = new AccountActivationModel
            {
                Result = _localizationService.GetResource("Account.AccountActivation.Activated")
            };
            return View(model);
        }

        #endregion

    }
}
