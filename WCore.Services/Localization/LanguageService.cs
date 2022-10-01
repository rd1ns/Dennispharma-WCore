using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Settings;
using WCore.Services.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WCore.Services.Caching;
using WCore.Services.Stores;

namespace WCore.Services.Localization
{
    public class LanguageService : Repository<Language>, ILanguageService
    {

        private readonly ISettingService _settingService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        public LanguageService(WCoreContext context,
            ISettingService settingService,
            IStoreMappingService storeMappingService,
            IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService,
            LocalizationSettings localizationSettings,
            StoreInformationSettings storeInformationSettings) : base(context)
        {
            this._settingService = settingService;
            this._storeMappingService = storeMappingService;
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService = cacheKeyService;
            this._localizationSettings = localizationSettings;
            this._storeInformationSettings = storeInformationSettings;
        }
        public IPagedList<Language> GetAllByFilters(string searchValue = "", string sortColumnName = "", string sortColumnDirection = "", int skip = 0, int take = 10)
        {
            sortColumnName = Helper.FirstLetterToUpper(sortColumnName);

            IQueryable<Language> recordsFiltered = context.Set<Language>();

            if (!string.IsNullOrEmpty(searchValue))
                recordsFiltered = recordsFiltered.Where(o => o.Name.Contains(searchValue));


            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<Language>().Count();

            var data = recordsFiltered.OrderByDescending(o => o.DisplayOrder).Skip(skip).Take(take).ToList();

            return new PagedList<Language>(data, 0, 10, recordsFilteredCount);
        }

        public List<Language> GetAllLanguages()
        {
            //cacheable copy
            //var key = _staticCacheManager.PrepareKeyForDefaultCache(WCoreLocalizationDefaults.LanguagesAllWithoutParametersCacheKey);

            //var languages = _staticCacheManager.Get(key, () =>
            //{
            //}).ToList();

            var allLanguages = context.Set<Language>();
            allLanguages.Where(o => o.Published);

            return allLanguages.ToList();
        }
        public List<Language> GetAllLanguages(bool? AllowSelection = null, bool? Published = null)
        {

            IQueryable<Language> recordsFiltered = context.Set<Language>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocalizationDefaults.LanguagesAllCacheKey, AllowSelection, Published);

            if (AllowSelection.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.AllowSelection == AllowSelection.Value);

            if (Published.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Published == Published.Value);

            var languages = _staticCacheManager.Get(key, () =>
            {
                var l = recordsFiltered.ToList();
                return l;
            }).ToList();

            return languages;
        }
        public int GetAllCount()
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocalizationDefaults.AllCountCachKey);
            var count = _staticCacheManager.Get(key, () =>
            {
                return context.Set<Language>().Count();
            });
            return count;
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        public virtual IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0)
        {
            var query = context.Languages.AsQueryable();
            if (!showHidden) query = query.Where(l => l.Published);
            query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

            //cacheable copy
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocalizationDefaults.LanguagesAllCacheKey, storeId, showHidden);

            var languages = _staticCacheManager.Get(key, () =>
            {
                var allLanguages = query.ToList();

                //store mapping
                if (storeId > 0)
                {
                    allLanguages = allLanguages
                        .Where(l => _storeMappingService.Authorize(l, storeId))
                        .ToList();
                }

                return allLanguages;
            });

            return languages;
        }


        /// <summary>
        /// Get 2 letter ISO language code
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>ISO language code</returns>
        public virtual string GetTwoLetterIsoLanguageName(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (string.IsNullOrEmpty(language.LanguageCulture))
                return "en";

            var culture = new CultureInfo(language.LanguageCulture);
            var code = culture.TwoLetterISOLanguageName;

            return string.IsNullOrEmpty(code) ? "en" : code;
        }

        public virtual Language GetAdminDefaultLanguage()
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocalizationDefaults.AdminDefaultLanguageCacheKey);

            var language = _staticCacheManager.Get(key, () =>
            {
                var l = context.Languages.FirstOrDefault(o => o.IsAdminDefault);
                return l;
            });
            return language;
        }

        public virtual Language GetDefaultLanguage()
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocalizationDefaults.DefaultLanguageCacheKey);

            var language = _staticCacheManager.Get(key, () =>
            {
                var l = context.Languages.FirstOrDefault(o => o.Id == _storeInformationSettings.DefaultLanguageId);
                return l;
            });
            return language;
        }
    }
}
