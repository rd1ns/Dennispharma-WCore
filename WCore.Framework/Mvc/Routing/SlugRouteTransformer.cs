using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using WCore.Core.Domain.Settings;
using WCore.Services.Localization;
using WCore.Services.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCore.Framework.Mvc.Routing
{
    /// <summary>
    /// Represents slug route transformer
    /// </summary>
    public class SlugRouteTransformer : DynamicRouteValueTransformer
    {
        #region Fields
        private readonly ILanguageService _languageService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public SlugRouteTransformer(ILanguageService languageService,
            IUrlRecordService urlRecordService,
            LocalizationSettings localizationSettings)
        {
            _languageService = languageService;
            _urlRecordService = urlRecordService;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            if (values == null)
                return new ValueTask<RouteValueDictionary>(values);

            if (!values.TryGetValue("SeName", out var slugValue) || string.IsNullOrEmpty(slugValue as string))
                return new ValueTask<RouteValueDictionary>(values);

            var slug = slugValue as string;
            var urlRecord = _urlRecordService.GetBySlug(slug);

            //no URL record found
            if (urlRecord == null)
                return new ValueTask<RouteValueDictionary>(values);

            //virtual directory path
            var pathBase = httpContext.Request.PathBase;

            //if URL record is not active let's find the latest one
            if (!urlRecord.IsActive)
            {
                var activeSlug = _urlRecordService.GetActiveSlug(urlRecord.EntityId, urlRecord.EntityName, urlRecord.LanguageId);
                if (string.IsNullOrEmpty(activeSlug))
                    return new ValueTask<RouteValueDictionary>(values);

                //redirect to active slug if found
                values[WCorePathRouteDefaults.ControllerFieldKey] = "Common";
                values[WCorePathRouteDefaults.ActionFieldKey] = "InternalRedirect";
                values[WCorePathRouteDefaults.UrlFieldKey] = $"{pathBase}/{activeSlug}{httpContext.Request.QueryString}";
                values[WCorePathRouteDefaults.PermanentRedirectFieldKey] = true;
                httpContext.Items["WCore.RedirectFromGenericPathRoute"] = true;

                return new ValueTask<RouteValueDictionary>(values);
            }

            //Ensure that the slug is the same for the current language, 
            //otherwise it can cause some issues when customers choose a new language but a slug stays the same
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                var urllanguage = values["language"];
                if (urllanguage != null && !string.IsNullOrEmpty(urllanguage.ToString()))
                {
                    var language = _languageService.GetAllLanguages().FirstOrDefault(x => x.UniqueSeoCode.ToLowerInvariant() == urllanguage.ToString().ToLowerInvariant());
                    if (language == null)
                        language = _languageService.GetAllLanguages().FirstOrDefault();

                    var slugForCurrentLanguage = _urlRecordService.GetActiveSlug(urlRecord.EntityId, urlRecord.EntityName, language.Id);
                    if (!string.IsNullOrEmpty(slugForCurrentLanguage) && !slugForCurrentLanguage.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //we should make validation above because some entities does not have SeName for standard (Id = 0) language (e.g. news, blog posts)

                        //redirect to the page for current language
                        values[WCorePathRouteDefaults.ControllerFieldKey] = "Common";
                        values[WCorePathRouteDefaults.ActionFieldKey] = "InternalRedirect";
                        values[WCorePathRouteDefaults.UrlFieldKey] = $"{pathBase}/{language.UniqueSeoCode}/{slugForCurrentLanguage}{httpContext.Request.QueryString}";
                        values[WCorePathRouteDefaults.PermanentRedirectFieldKey] = false;
                        httpContext.Items["WCore.RedirectFromGenericPathRoute"] = true;

                        return new ValueTask<RouteValueDictionary>(values);
                    }
                }
            }

            //since we are here, all is ok with the slug, so process URL
            switch (urlRecord.EntityName.ToLowerInvariant())
            {
                case "page":
                    values[WCorePathRouteDefaults.ControllerFieldKey] = "Page";
                    values[WCorePathRouteDefaults.ActionFieldKey] = "Details";
                    values[WCorePathRouteDefaults.PageIdFieldKey] = urlRecord.EntityId;
                    values[WCorePathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                case "news":
                    values[WCorePathRouteDefaults.ControllerFieldKey] = "News";
                    values[WCorePathRouteDefaults.ActionFieldKey] = "Details";
                    values[WCorePathRouteDefaults.NewsdFieldKey] = urlRecord.EntityId;
                    values[WCorePathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                case "newscategory":
                    values[WCorePathRouteDefaults.ControllerFieldKey] = "NewsCategory";
                    values[WCorePathRouteDefaults.ActionFieldKey] = "Details";
                    values[WCorePathRouteDefaults.NewsCategoryIdFieldKey] = urlRecord.EntityId;
                    values[WCorePathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                case "teamcategory":
                    values[WCorePathRouteDefaults.ControllerFieldKey] = "TeamCategory";
                    values[WCorePathRouteDefaults.ActionFieldKey] = "Details";
                    values[WCorePathRouteDefaults.TeamCategoryIdFieldKey] = urlRecord.EntityId;
                    values[WCorePathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                case "academycategory":
                    values[WCorePathRouteDefaults.ControllerFieldKey] = "AcademyCategory";
                    values[WCorePathRouteDefaults.ActionFieldKey] = "Details";
                    values[WCorePathRouteDefaults.AcademyCategoryIdFieldKey] = urlRecord.EntityId;
                    values[WCorePathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                case "academy":
                    values[WCorePathRouteDefaults.ControllerFieldKey] = "Academy";
                    values[WCorePathRouteDefaults.ActionFieldKey] = "Details";
                    values[WCorePathRouteDefaults.AcademyIdFieldKey] = urlRecord.EntityId;
                    values[WCorePathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                case "congress":
                    values[WCorePathRouteDefaults.ControllerFieldKey] = "Congress";
                    values[WCorePathRouteDefaults.ActionFieldKey] = "Details";
                    values[WCorePathRouteDefaults.CongressIdFieldKey] = urlRecord.EntityId;
                    values[WCorePathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                default:
                    //no record found, thus generate an event this way developers could insert their own types
                    break;
            }

            return new ValueTask<RouteValueDictionary>(values);
        }

        #endregion
    }
}
