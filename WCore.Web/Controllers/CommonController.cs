using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using WCore.Core;
using WCore.Core.Domain;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Framework.Extensions;
using WCore.Services.Common;
using WCore.Services.Directory;
using WCore.Services.DynamicForms;
using WCore.Services.Localization;
using WCore.Services.Settings;
using WCore.Services.Users;
using WCore.Web.Controllers;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models;
using WCore.Web.Models.Users;

namespace WCore.Controllers
{
    public class CommonController : BasePublicController
    {
        #region Fields
        private readonly IDynamicFormRecordService _dynamicFormRecordService;
        private readonly IDynamicFormService _dynamicFormService;

        private readonly IDynamicFormModelFactory _dynamicFormModelFactory;
        private readonly IUserRegistrationFormService _userRegistrationFormService;

        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;
        private readonly IWebHelper _webHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        #endregion

        #region Ctor
        public CommonController(IDynamicFormRecordService dynamicFormRecordService,
            IDynamicFormService dynamicFormService,
            IDynamicFormModelFactory dynamicFormModelFactory,
            IUserRegistrationFormService userRegistrationFormService,
            IWorkContext workContext,
            ISettingService settingService,
            IUserService userService,
            IWebHelper webHelper,
            IWebHostEnvironment webHostEnvironment,
            ILanguageService languageService,
            ICurrencyService currencyService,
            ICountryService countryService,
            ILocalizationService localizationService,
            LocalizationSettings localizationSettings,
            StoreInformationSettings storeInformationSettings)
        {
            this._dynamicFormRecordService = dynamicFormRecordService;
            this._dynamicFormService = dynamicFormService;
            this._dynamicFormModelFactory = dynamicFormModelFactory;
            this._userRegistrationFormService = userRegistrationFormService;
            this._workContext = workContext;
            this._settingService = settingService;
            this._userService = userService;
            this._webHelper = webHelper;
            this._webHostEnvironment = webHostEnvironment;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._countryService = countryService;
            this._localizationService = localizationService;
            this._localizationSettings = localizationSettings;
            this._storeInformationSettings = storeInformationSettings;
        }
        #endregion

        #region Methods
        public IActionResult PageNotFound()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DynamicForm()
        {
            var dynamicFormId = 0;
            var Keys = "";
            var DynamicFormTitle = "";
            SmtpException exception = new SmtpException() { Source = "" };
            var result = false;

            if (Request.Form != null)
            {
                foreach (var key in Request.Form.Keys)
                {
                    var value = key;

                    var val = Request.Form[key][0];


                    if (key == "DynamicFormId")
                        dynamicFormId = val.ToInt();

                    if (key == "DynamicFormTitle")
                    {
                        DynamicFormTitle = val.ToString();
                        Keys += "<b>Form Başlık : </b>" + val + "<br/>";
                    }

                    var SplittedKey = key.Split("##");
                    if (key.Contains("##"))
                    {
                        Keys += "<b>" + SplittedKey[0] + " : </b>" + val + "<br/>";
                    }
                }
                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/formuploads");
                foreach (var file in Request.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, file.FileName);
                        using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                        await file.CopyToAsync(fileStream);

                        var fileAddressPath = Path.Combine(Request.Scheme + ":", Request.Host.Value, "uploads/formuploads/" + file.FileName + "").Replace("\\", "/");

                        Keys += "<img style='max-height:300px;' src='" + fileAddressPath + "'><b><a href='" + fileAddressPath + "'>İndir</a></b>";
                    }
                }
                Keys += "</br></br><b> Gönerim Zamanı : " + DateTime.Now.ToString() + "</b><br/>";
                var Message = "<div style='padding:5px;'>" + Keys + "</div>";


                _dynamicFormRecordService.Insert(new Core.Domain.DynamicForms.DynamicFormRecord()
                {
                    Body = Message,
                    CreatedOn = DateTime.Now,
                    DynamicFormId = dynamicFormId
                });
                var dynamicForm = _dynamicFormModelFactory.PrepareDynamicFormModel(dynamicFormId);

                exception = _webHelper.MailSender(Message, DynamicFormTitle, dynamicForm.ToAddresses);
                result = string.IsNullOrEmpty(exception.Source) ? true : false;
                if (!result)
                {
                    //_logger.Error("Mail gönderme hatası", MailResult.GetBaseException());
                }
                return new JsonResult(new
                {
                    result,
                    message = !result ? exception.Source : dynamicForm.Result
                });

            }

            return new JsonResult(new
            {
                result,
                message = !result ? exception.Source : _localizationService.GetLocaleStringResourceByName("common.dynamicform.message", _workContext.WorkingLanguage.Id)?.ResourceValue
            });
        }

        [HttpPost]
        public IActionResult UserRegistration(UserRegistrationFormModel userRegistrationFormModel)
        {
            var entity = userRegistrationFormModel.ToEntity<UserRegistrationForm>();
            entity.CreatedOn = DateTime.Now;
            _userRegistrationFormService.Insert(entity);
            return Json("OK");
        }

        [HttpPost]
        public IActionResult AddNewsletter(string eMail)
        {
            var user = _userService.GetUserByEmail(eMail);
            if (user == null)
            {
                _userService.InsertNewsletterUser(eMail);
            }
            return Json(user == null);
        }

        public virtual IActionResult InternalRedirect(string url, bool permanentRedirect)
        {
            //ensure it's invoked from our GenericPathRoute class
            if (HttpContext.Items["WCore.RedirectFromGenericPathRoute"] == null ||
                !Convert.ToBoolean(HttpContext.Items["WCore.RedirectFromGenericPathRoute"]))
            {
                url = Url.RouteUrl("Homepage");
                permanentRedirect = false;
            }

            //home page
            if (string.IsNullOrEmpty(url))
            {
                url = Url.RouteUrl("Homepage");
                permanentRedirect = false;
            }

            //prevent open redirection attack
            if (!Url.IsLocalUrl(url))
            {
                url = Url.RouteUrl("Homepage");
                permanentRedirect = false;
            }

            if (permanentRedirect)
                return RedirectPermanent(url);

            return Redirect(url);
        }

        public virtual IActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _languageService.GetById(langid);
            if (!language?.Published ?? false)
                language = _workContext.WorkingLanguage;

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("Homepage");

            //language part in URL
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                //remove current language code if it's already localized URL
                if (returnUrl.IsLocalizedUrl(Request.PathBase, true, out var _))
                    returnUrl = returnUrl.RemoveLanguageSeoCodeFromUrl(Request.PathBase, true);

                //and add code of passed language
                returnUrl = returnUrl.AddLanguageSeoCodeToUrl(Request.PathBase, true, language);
            }

            _workContext.WorkingLanguage = language;

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("Homepage");

            return Redirect(returnUrl);
        }

        public virtual IActionResult SetCurrency(int userCurrency, string returnUrl = "")
        {
            var currency = _currencyService.GetById(userCurrency);
            if (currency != null)
                _workContext.WorkingCurrency = currency;

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("Homepage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("Homepage");

            return Redirect(returnUrl);
        }

        public virtual IActionResult SetCountry(int userCountry, string returnUrl = "")
        {
            var country = _countryService.GetCountryById(userCountry);
            if (country != null)
                _workContext.WorkingCountry = country;

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("Homepage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("Homepage");

            return Redirect(returnUrl);
        }

        #endregion

    }
}
