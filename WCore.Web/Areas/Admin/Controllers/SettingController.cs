using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Core.Domain;
using WCore.Core.Domain.Settings;
using WCore.Framework.Extensions;
using WCore.Services.Localization;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Models.Settings;
using System.IO;
using System.Linq;
using WCore.Framework.Themes;
using WCore.Core.Domain.Common;
using WCore.Services.Directory;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class SettingController : BaseAdminController
    {
        #region Fields
        private readonly ISettingService _settingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILanguageService _languageService;
        private readonly ICountryService _countryService;
        private readonly IThemeProvider _themeProvider;
        private readonly IThemeContext _themeContext;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public SettingController(ISettingService settingService,
            IWebHostEnvironment webHostEnvironment,
            ILanguageService languageService,
            ICountryService countryService,
            IThemeProvider themeProvider,
            IThemeContext themeContext,
            IWorkContext workContext)
        {
            this._settingService = settingService;
            this._webHostEnvironment = webHostEnvironment;
            this._languageService = languageService;
            this._countryService = countryService;
            this._themeProvider = themeProvider;
            this._themeContext = themeContext;
            this._workContext = workContext;
        }
        #endregion

        public virtual IActionResult Index()
        {
            return RedirectToAction("SeoSetting");
        }

        #region Methods

        #region Store Information Settings
        public virtual IActionResult StoreInformationSettings()
        {
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>();
            var model = new StoreInformationSettingsModel
            {
                LogoPicture = storeInformationSettings.LogoPicture,
                StickyPicture = storeInformationSettings.StickyPicture,
                BgImage = storeInformationSettings.BgImage,
                BgPattern = storeInformationSettings.BgPattern,
                PageTitleImage = storeInformationSettings.PageTitleImage,
                DefaultStoreTheme = storeInformationSettings.DefaultStoreTheme,
                DefaultLanguageId = storeInformationSettings.DefaultLanguageId,
                DefaultCountryId = storeInformationSettings.DefaultCountryId,
                DefaultStoreThemeColorScheme = storeInformationSettings.DefaultStoreThemeColorScheme,
                DefaultStoreThemeTemplateType = storeInformationSettings.DefaultStoreThemeTemplateType,
                DefaultStoreThemeTemplateFeature = storeInformationSettings.DefaultStoreThemeTemplateFeature,
                DefaultStoreThemeLayoutType = storeInformationSettings.DefaultStoreThemeLayoutType,
                DefaultStoreThemeGalleryType = storeInformationSettings.DefaultStoreThemeGalleryType,
                DefaultStoreThemeHeaderType = storeInformationSettings.DefaultStoreThemeHeaderType,
                DefaultStoreThemePageTitle = storeInformationSettings.DefaultStoreThemePageTitle,
                AllowUserToSelectTheme = storeInformationSettings.AllowUserToSelectTheme,
                DisplayEuCookieLawWarning = storeInformationSettings.DisplayEuCookieLawWarning,
                FacebookLink = storeInformationSettings.FacebookLink,
                TwitterLink = storeInformationSettings.TwitterLink,
                YoutubeLink = storeInformationSettings.YoutubeLink,
                LinkedinLink = storeInformationSettings.LinkedinLink,
                InstagramLink = storeInformationSettings.InstagramLink
            };

            var themes = _themeProvider.GetThemes();
            var theme = themes.FirstOrDefault(o => o.SystemName == _themeContext.WorkingThemeName);

            var templateType = string.IsNullOrEmpty(storeInformationSettings.DefaultStoreThemeTemplateType) ? theme.TemplateTypes.FirstOrDefault() : theme.TemplateTypes.FirstOrDefault(o => o.Type == storeInformationSettings.DefaultStoreThemeTemplateType);

            model.Themes = new SelectList(themes, "SystemName", "SystemName", _themeContext.WorkingThemeName).ToList();
            model.ThemeLayoutTypes = new SelectList(theme.LayoutTypes, "Type", "Type", storeInformationSettings.DefaultStoreThemeLayoutType).ToList();
            model.ThemeTemplateTypes = new SelectList(theme.TemplateTypes, "Type", "Type", storeInformationSettings.DefaultStoreThemeTemplateType).ToList();
            model.ThemeTemplateFeatures = new MultiSelectList(templateType.Features, "Name", "Name", storeInformationSettings.DefaultStoreThemeTemplateFeature).ToList();
            model.ThemeLayoutTypes = new SelectList(theme.LayoutTypes, "Type", "Type", storeInformationSettings.DefaultStoreThemeLayoutType).ToList();
            model.ThemeColorSchemes = new SelectList(theme.ColorSchemes, "Color", "Color", storeInformationSettings.DefaultStoreThemeColorScheme).ToList();
            model.ThemeGalleryTypes = new SelectList(theme.GalleryTypes, "Type", "Type", storeInformationSettings.DefaultStoreThemeGalleryType).ToList();
            model.ThemePageTitles = new SelectList(theme.PageTitles, "Type", "Type", storeInformationSettings.DefaultStoreThemePageTitle).ToList();
            model.ThemeHeaderTypes = new SelectList(theme.HeaderTypes, "Type", "Type", storeInformationSettings.DefaultStoreThemeGalleryType).ToList();
            model.Languages = new SelectList(_languageService.GetAllLanguages(), "Id", "Name", storeInformationSettings.DefaultLanguageId).ToList();
            model.Countries = new SelectList(_countryService.GetAllCountries(), "Id", "Name", storeInformationSettings.DefaultCountryId).ToList();
            return View(model);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> StoreInformationSettingsAsync(StoreInformationSettings model)
        {
            //store information settings
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>();

            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/setting");

            var logoPicture = Request.Form.Files["LogoPicture"];
            if (logoPicture != null && logoPicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, logoPicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await logoPicture.CopyToAsync(fileStream);
                storeInformationSettings.LogoPicture = "/uploads/setting/" + logoPicture.FileName;

            }

            var stickyPicture = Request.Form.Files["StickyPicture"];
            if (stickyPicture != null && stickyPicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, stickyPicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await stickyPicture.CopyToAsync(fileStream);
                storeInformationSettings.StickyPicture = "/uploads/setting/" + stickyPicture.FileName;

            }

            var bgPattern = Request.Form.Files["BgPattern"];
            if (bgPattern != null && bgPattern.Length > 0)
            {
                var filePath = Path.Combine(uploads, bgPattern.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await bgPattern.CopyToAsync(fileStream);
                storeInformationSettings.BgPattern = "/uploads/setting/" + bgPattern.FileName;

            }

            var bgImage = Request.Form.Files["BgImage"];
            if (bgImage != null && bgImage.Length > 0)
            {
                var filePath = Path.Combine(uploads, bgImage.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await bgImage.CopyToAsync(fileStream);
                storeInformationSettings.BgImage = "/uploads/setting/" + bgImage.FileName;

            }

            var pageTitleImage = Request.Form.Files["PageTitleImage"];
            if (pageTitleImage != null && pageTitleImage.Length > 0)
            {
                var filePath = Path.Combine(uploads, pageTitleImage.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await pageTitleImage.CopyToAsync(fileStream);
                storeInformationSettings.PageTitleImage = "/uploads/setting/" + pageTitleImage.FileName;

            }


            storeInformationSettings.DefaultStoreTheme = model.DefaultStoreTheme;
            storeInformationSettings.DefaultStoreThemeColorScheme = model.DefaultStoreThemeColorScheme;
            storeInformationSettings.DefaultStoreThemeTemplateType = model.DefaultStoreThemeTemplateType;
            storeInformationSettings.DefaultStoreThemeTemplateFeature = model.DefaultStoreThemeTemplateFeature;
            storeInformationSettings.DefaultStoreThemeLayoutType = model.DefaultStoreThemeLayoutType;
            storeInformationSettings.DefaultStoreThemeGalleryType = model.DefaultStoreThemeGalleryType;
            storeInformationSettings.DefaultStoreThemePageTitle = model.DefaultStoreThemePageTitle;
            storeInformationSettings.DefaultStoreThemeHeaderType = model.DefaultStoreThemeHeaderType;
            storeInformationSettings.AllowUserToSelectTheme = model.AllowUserToSelectTheme;
            storeInformationSettings.DisplayEuCookieLawWarning = model.DisplayEuCookieLawWarning;
            storeInformationSettings.FacebookLink = model.FacebookLink;
            storeInformationSettings.TwitterLink = model.TwitterLink;
            storeInformationSettings.YoutubeLink = model.YoutubeLink;
            storeInformationSettings.LinkedinLink = model.LinkedinLink;
            storeInformationSettings.InstagramLink = model.InstagramLink;
            storeInformationSettings.DefaultCountryId = model.DefaultCountryId;
            storeInformationSettings.DefaultLanguageId = model.DefaultLanguageId;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSetting(storeInformationSettings, x => x.LogoPicture);
            _settingService.SaveSetting(storeInformationSettings, x => x.StickyPicture);
            _settingService.SaveSetting(storeInformationSettings, x => x.BgImage);
            _settingService.SaveSetting(storeInformationSettings, x => x.BgPattern);
            _settingService.SaveSetting(storeInformationSettings, x => x.PageTitleImage);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreTheme);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreThemeLayoutType);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreThemeColorScheme);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreThemeTemplateType);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreThemeTemplateFeature);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreThemeGalleryType);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreThemePageTitle);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreThemeHeaderType);
            _settingService.SaveSetting(storeInformationSettings, x => x.AllowUserToSelectTheme);
            _settingService.SaveSetting(storeInformationSettings, x => x.DisplayEuCookieLawWarning);
            _settingService.SaveSetting(storeInformationSettings, x => x.FacebookLink);
            _settingService.SaveSetting(storeInformationSettings, x => x.TwitterLink);
            _settingService.SaveSetting(storeInformationSettings, x => x.YoutubeLink);
            _settingService.SaveSetting(storeInformationSettings, x => x.LinkedinLink);
            _settingService.SaveSetting(storeInformationSettings, x => x.InstagramLink);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultCountryId);
            _settingService.SaveSetting(storeInformationSettings, x => x.DefaultLanguageId);

            //now clear settings cache
            _settingService.ClearCache();

            return Redirect("/admin/setting/storeInformationSettings");
        }
        #endregion

        #region Seo Settings
        public virtual IActionResult SeoSettings()
        {
            var seoSettings = _settingService.LoadSetting<SeoSettings>();

            //fill in model values from the entity
            var model = new SeoSettingsModel
            {
                PageTitleSeparator = seoSettings.PageTitleSeparator,
                PageTitleSeoAdjustment = (int)seoSettings.PageTitleSeoAdjustment,
                PageTitleSeoAdjustmentValues = seoSettings.PageTitleSeoAdjustment.ToSelectList(),
                DefaultMetaKeywords = seoSettings.DefaultMetaKeywords,
                DefaultMetaDescription = seoSettings.DefaultMetaDescription,
                GenerateProductMetaDescription = seoSettings.GenerateProductMetaDescription,
                ConvertNonWesternChars = seoSettings.ConvertNonWesternChars,
                CanonicalUrlsEnabled = seoSettings.CanonicalUrlsEnabled,
                WwwRequirement = (int)seoSettings.WwwRequirement,
                WwwRequirementValues = seoSettings.WwwRequirement.ToSelectList(),
                TwitterMetaTags = seoSettings.TwitterMetaTags,
                OpenGraphMetaTags = seoSettings.OpenGraphMetaTags,
                CustomHeadTags = seoSettings.CustomHeadTags,
                MicrodataEnabled = seoSettings.MicrodataEnabled,
                DefaultTitle = seoSettings.DefaultTitle,
                ReservedUrlRecordSlugs = seoSettings.ReservedUrlRecordSlugs
            };
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult SeoSettings(SeoSettingsModel model)
        {
            //store information settings
            var seoSettings = _settingService.LoadSetting<SeoSettings>();

            seoSettings.PageTitleSeparator = model.PageTitleSeparator;
            seoSettings.PageTitleSeoAdjustment = (PageTitleSeoAdjustment)model.PageTitleSeoAdjustment;
            seoSettings.DefaultTitle = model.DefaultTitle;
            seoSettings.DefaultMetaKeywords = model.DefaultMetaKeywords;
            seoSettings.DefaultMetaDescription = model.DefaultMetaDescription;
            seoSettings.GenerateProductMetaDescription = model.GenerateProductMetaDescription;
            seoSettings.ConvertNonWesternChars = model.ConvertNonWesternChars;
            seoSettings.CanonicalUrlsEnabled = model.CanonicalUrlsEnabled;
            seoSettings.WwwRequirement = (WwwRequirement)model.WwwRequirement;
            seoSettings.TwitterMetaTags = model.TwitterMetaTags;
            seoSettings.OpenGraphMetaTags = model.OpenGraphMetaTags;
            seoSettings.CustomHeadTags = model.CustomHeadTags;
            seoSettings.MicrodataEnabled = model.MicrodataEnabled;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSetting(seoSettings, x => x.DefaultTitle);
            _settingService.SaveSetting(seoSettings, x => x.DefaultMetaKeywords);
            _settingService.SaveSetting(seoSettings, x => x.DefaultMetaDescription);
            _settingService.SaveSetting(seoSettings, x => x.PageTitleSeparator);
            _settingService.SaveSetting(seoSettings, x => x.PageTitleSeoAdjustment);
            _settingService.SaveSetting(seoSettings, x => x.GenerateProductMetaDescription);
            _settingService.SaveSetting(seoSettings, x => x.ConvertNonWesternChars);
            _settingService.SaveSetting(seoSettings, x => x.CanonicalUrlsEnabled);
            _settingService.SaveSetting(seoSettings, x => x.WwwRequirement);
            _settingService.SaveSetting(seoSettings, x => x.OpenGraphMetaTags);
            _settingService.SaveSetting(seoSettings, x => x.CustomHeadTags);
            _settingService.SaveSetting(seoSettings, x => x.MicrodataEnabled);

            //now clear settings cache
            _settingService.ClearCache();

            return Redirect("/admin/setting/seoSettings");
        }
        #endregion

        #region Common Settings
        public virtual IActionResult CommonSettings()
        {
            var settings = _settingService.LoadSetting<CommonSettings>();

            var model = new CommonSettingsModel();

            model.SubjectFieldOnContactUsForm = settings.SubjectFieldOnContactUsForm;
            model.UseSystemEmailForContactUsForm = settings.UseSystemEmailForContactUsForm;
            //model.UseStoredProcedureForLoadingCategories = settings.UseStoredProcedureForLoadingCategories;
            model.DisplayJavaScriptDisabledWarning = settings.DisplayJavaScriptDisabledWarning;
            model.UseFullTextSearch = settings.UseFullTextSearch;
            model.Log404Errors = settings.Log404Errors;
            model.BreadcrumbDelimiter = settings.BreadcrumbDelimiter;
            //model.RenderXuaCompatible = settings.RenderXuaCompatible;
            //model.XuaCompatibleValue = settings.XuaCompatibleValue;
            model.IgnoreLogWordlist = settings.IgnoreLogWordlist;
            model.BbcodeEditorOpenLinksInNewWindow = settings.BbcodeEditorOpenLinksInNewWindow;
            model.PopupForTermsOfServiceLinks = settings.PopupForTermsOfServiceLinks;
            model.JqueryMigrateScriptLoggingActive = settings.JqueryMigrateScriptLoggingActive;
            model.SupportPreviousWCorecommerceVersions = settings.SupportPreviousWCorecommerceVersions;
            model.UseResponseCompression = settings.UseResponseCompression;
            model.StaticFilesCacheControl = settings.StaticFilesCacheControl;
            model.FaviconAndAppIconsHeadCode = settings.FaviconAndAppIconsHeadCode;
            model.EnableHtmlMinification = settings.EnableHtmlMinification;
            model.EnableJsBundling = settings.EnableJsBundling;
            model.EnableCssBundling = settings.EnableCssBundling;
            model.ScheduleTaskRunTimeout = settings.ScheduleTaskRunTimeout;

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult CommonSettings(CommonSettingsModel model)
        {
            //store information settings
            var settings = _settingService.LoadSetting<CommonSettings>();

            _settingService.SaveSetting(settings, x => x.SubjectFieldOnContactUsForm);
            _settingService.SaveSetting(settings, x => x.UseSystemEmailForContactUsForm);
            //_settingService.SaveSetting(settings, x => x.UseStoredProcedureForLoadingCategories);
            _settingService.SaveSetting(settings, x => x.DisplayJavaScriptDisabledWarning);
            _settingService.SaveSetting(settings, x => x.UseFullTextSearch);
            _settingService.SaveSetting(settings, x => x.Log404Errors);
            _settingService.SaveSetting(settings, x => x.BreadcrumbDelimiter);
            //_settingService.SaveSetting(settings, x => x.RenderXuaCompatible);
            //_settingService.SaveSetting(settings, x => x.XuaCompatibleValue);
            _settingService.SaveSetting(settings, x => x.IgnoreLogWordlist);
            _settingService.SaveSetting(settings, x => x.BbcodeEditorOpenLinksInNewWindow);
            _settingService.SaveSetting(settings, x => x.JqueryMigrateScriptLoggingActive);
            _settingService.SaveSetting(settings, x => x.SupportPreviousWCorecommerceVersions);
            _settingService.SaveSetting(settings, x => x.UseResponseCompression);
            _settingService.SaveSetting(settings, x => x.StaticFilesCacheControl);
            _settingService.SaveSetting(settings, x => x.FaviconAndAppIconsHeadCode);
            _settingService.SaveSetting(settings, x => x.EnableHtmlMinification);
            _settingService.SaveSetting(settings, x => x.EnableJsBundling);
            _settingService.SaveSetting(settings, x => x.EnableCssBundling);
            _settingService.SaveSetting(settings, x => x.ScheduleTaskRunTimeout);

            //now clear settings cache
            _settingService.ClearCache();

            return Redirect("/admin/setting/CommonSettings");
        }
        #endregion

        #region Mail Settings
        public virtual IActionResult MailSettings()
        {
            var settings = _settingService.LoadSetting<MailSettings>();

            //fill in model values from the entity
            var model = new MailSettingsModel();
            model.HostName = settings.HostName;
            model.PortNumber = settings.PortNumber;
            model.EmailAddress = settings.EmailAddress;
            model.EmailPassword = settings.EmailPassword;
            model.UseSSL = settings.UseSSL;

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult MailSettings(MailSettingsModel model)
        {
            //store information settings
            var settings = _settingService.LoadSetting<MailSettings>();

            settings.HostName = model.HostName;
            settings.PortNumber = model.PortNumber;
            settings.EmailAddress = model.EmailAddress;
            settings.EmailPassword = model.EmailPassword;
            settings.UseSSL = model.UseSSL;

            _settingService.SaveSetting(settings, x => x.HostName);
            _settingService.SaveSetting(settings, x => x.PortNumber);
            _settingService.SaveSetting(settings, x => x.EmailAddress);
            _settingService.SaveSetting(settings, x => x.EmailPassword);
            _settingService.SaveSetting(settings, x => x.UseSSL);
            //now clear settings cache
            _settingService.ClearCache();

            return Redirect("/admin/setting/mailSettings");
        }
        #endregion

        #region Notification Settings
        public virtual IActionResult NotificationSettings()
        {
            var settings = _settingService.LoadSetting<NotificationSettings>();

            //fill in model values from the entity
            var model = new NotificationSettingsModel();


            model.SendRegisterNotification = settings.SendRegisterNotification;
            model.SendCreateCompanyNotification = settings.SendCreateCompanyNotification;
            model.CreateCompanyNotificationMailList = settings.CreateCompanyNotificationMailList;
            model.SendCreateOrderNotification = settings.SendCreateOrderNotification;
            model.CreateOrderNotificationMailList = settings.CreateOrderNotificationMailList;
            model.SendInadequateLimitNotification = settings.SendInadequateLimitNotification;
            model.InadequateLimitNotificationMailList = settings.InadequateLimitNotificationMailList;
            model.SendDeniedOrderNotification = settings.SendDeniedOrderNotification;
            model.DeniedOrderNotificationMailList = settings.DeniedOrderNotificationMailList;
            model.SendCreateActivityNotification = settings.SendCreateActivityNotification;
            model.CreateActivityNotificationMailList = settings.CreateActivityNotificationMailList;
            model.SendCreateVehicleOrderNotification = settings.SendCreateVehicleOrderNotification;
            model.CreateVehicleOrderNotificationMailList = settings.CreateVehicleOrderNotificationMailList;

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult NotificationSettings(NotificationSettingsModel model)
        {
            //store information settings
            var settings = _settingService.LoadSetting<NotificationSettings>();


            settings.SendRegisterNotification = model.SendRegisterNotification;
            settings.SendCreateCompanyNotification = model.SendCreateCompanyNotification;
            settings.CreateCompanyNotificationMailList = model.CreateCompanyNotificationMailList;
            settings.SendCreateOrderNotification = model.SendCreateOrderNotification;
            settings.CreateOrderNotificationMailList = model.CreateOrderNotificationMailList;
            settings.SendInadequateLimitNotification = model.SendInadequateLimitNotification;
            settings.InadequateLimitNotificationMailList = model.InadequateLimitNotificationMailList;
            settings.SendDeniedOrderNotification = model.SendDeniedOrderNotification;
            settings.DeniedOrderNotificationMailList = model.DeniedOrderNotificationMailList;
            settings.SendCreateActivityNotification = model.SendCreateActivityNotification;
            settings.CreateActivityNotificationMailList = model.CreateActivityNotificationMailList;
            settings.SendCreateVehicleOrderNotification = model.SendCreateVehicleOrderNotification;
            settings.CreateVehicleOrderNotificationMailList = model.CreateVehicleOrderNotificationMailList;

            _settingService.SaveSetting(settings, x => x.SendRegisterNotification);
            _settingService.SaveSetting(settings, x => x.SendCreateCompanyNotification);
            _settingService.SaveSetting(settings, x => x.CreateCompanyNotificationMailList);
            _settingService.SaveSetting(settings, x => x.SendCreateOrderNotification);
            _settingService.SaveSetting(settings, x => x.CreateOrderNotificationMailList);
            _settingService.SaveSetting(settings, x => x.SendInadequateLimitNotification);
            _settingService.SaveSetting(settings, x => x.InadequateLimitNotificationMailList);
            _settingService.SaveSetting(settings, x => x.SendDeniedOrderNotification);
            _settingService.SaveSetting(settings, x => x.DeniedOrderNotificationMailList);
            _settingService.SaveSetting(settings, x => x.SendCreateActivityNotification);
            _settingService.SaveSetting(settings, x => x.CreateActivityNotificationMailList);
            _settingService.SaveSetting(settings, x => x.SendCreateVehicleOrderNotification);
            _settingService.SaveSetting(settings, x => x.CreateVehicleOrderNotificationMailList);

            //now clear settings cache
            _settingService.ClearCache();

            return Redirect("/admin/setting/notificationSettings");
        }
        #endregion

        #region Localization Settings
        public virtual IActionResult LocalizationSettings()
        {
            var LocalizationSettings = _settingService.LoadSetting<LocalizationSettings>();

            //fill in model values from the entity
            var model = new LocalizationSettingsModel
            {
                DefaultAdminLanguageId = LocalizationSettings.DefaultAdminLanguageId,
                UseImagesForLanguageSelection = LocalizationSettings.UseImagesForLanguageSelection,
                SeoFriendlyUrlsForLanguagesEnabled = LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled,
                AutomaticallyDetectLanguage = LocalizationSettings.AutomaticallyDetectLanguage,
                LoadAllLocaleRecordsOnStartup = LocalizationSettings.LoadAllLocaleRecordsOnStartup,
                LoadAllLocalizedPropertiesOnStartup = LocalizationSettings.LoadAllLocalizedPropertiesOnStartup,
                LoadAllUrlRecordsOnStartup = LocalizationSettings.LoadAllUrlRecordsOnStartup,
                IgnoreRtlPropertyForAdminArea = LocalizationSettings.IgnoreRtlPropertyForAdminArea,
                Languages = _languageService.GetAllLanguages().Select(o =>
                {
                    var m = new SelectListItem();
                    m.Text = o.Name;
                    m.Value = o.Id.ToString();
                    m.Selected = o.Id == LocalizationSettings.DefaultAdminLanguageId;
                    return m;
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult LocalizationSettings(LocalizationSettingsModel model)
        {
            //store information settings
            var LocalizationSettings = _settingService.LoadSetting<LocalizationSettings>();

            LocalizationSettings.DefaultAdminLanguageId = model.DefaultAdminLanguageId;
            LocalizationSettings.UseImagesForLanguageSelection = model.UseImagesForLanguageSelection;
            LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled = model.SeoFriendlyUrlsForLanguagesEnabled;
            LocalizationSettings.AutomaticallyDetectLanguage = model.AutomaticallyDetectLanguage;
            LocalizationSettings.LoadAllLocaleRecordsOnStartup = model.LoadAllLocaleRecordsOnStartup;
            LocalizationSettings.LoadAllLocalizedPropertiesOnStartup = model.LoadAllLocalizedPropertiesOnStartup;
            LocalizationSettings.LoadAllUrlRecordsOnStartup = model.LoadAllUrlRecordsOnStartup;
            LocalizationSettings.IgnoreRtlPropertyForAdminArea = model.IgnoreRtlPropertyForAdminArea;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSetting(LocalizationSettings, x => x.DefaultAdminLanguageId);
            _settingService.SaveSetting(LocalizationSettings, x => x.UseImagesForLanguageSelection);
            _settingService.SaveSetting(LocalizationSettings, x => x.SeoFriendlyUrlsForLanguagesEnabled);
            _settingService.SaveSetting(LocalizationSettings, x => x.AutomaticallyDetectLanguage);
            _settingService.SaveSetting(LocalizationSettings, x => x.LoadAllLocaleRecordsOnStartup);
            _settingService.SaveSetting(LocalizationSettings, x => x.LoadAllLocalizedPropertiesOnStartup);
            _settingService.SaveSetting(LocalizationSettings, x => x.LoadAllUrlRecordsOnStartup);
            _settingService.SaveSetting(LocalizationSettings, x => x.IgnoreRtlPropertyForAdminArea);


            //now clear settings cache
            _settingService.ClearCache();

            return Redirect("/admin/setting/localizationSettings");
        }
        #endregion

        #endregion
    }
}
