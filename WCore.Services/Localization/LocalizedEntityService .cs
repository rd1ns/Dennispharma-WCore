using Castle.DynamicProxy;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WCore.Services.Caching;

namespace WCore.Services.Localization
{
    public class LocalizedEntityService : Repository<LocalizedProperty>, ILocalizedEntityService
    {
        #region Fields
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly LocalizationSettings _localizationSettings;
        #endregion

        #region Ctor
        public LocalizedEntityService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService,
            LocalizationSettings localizationSettings) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService = cacheKeyService;
            this._localizationSettings = localizationSettings;
        }
        #endregion

        #region Utilities

        public IPagedList<LocalizedProperty> GetAllByFilters(string searchValue = "", string sortColumnName = "", string sortColumnDirection = "", int skip = 0, int take = 10)
        {
            sortColumnName = Helper.FirstLetterToUpper(sortColumnName);

            IQueryable<LocalizedProperty> recordsFiltered = context.Set<LocalizedProperty>();


            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<LocalizedProperty>().Count();

            var data = recordsFiltered.Skip(skip).Take(take).ToList();

            return new PagedList<LocalizedProperty>(data, 0, 10, recordsFilteredCount);
        }

        /// <summary>
        /// Gets localized properties
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="localeKeyGroup">Locale key group</param>
        /// <returns>Localized properties</returns>
        protected virtual IList<LocalizedProperty> GetLocalizedProperties(int entityId, string localeKeyGroup)
        {
            if (entityId == 0 || string.IsNullOrEmpty(localeKeyGroup))
                return new List<LocalizedProperty>();

            var query = from lp in context.LocalizedProperties
                        orderby lp.Id
                        where lp.EntityId == entityId &&
                              lp.LocaleKeyGroup == localeKeyGroup
                        select lp;

            var props = query.ToList();

            return props;
        }

        /// <summary>
        /// Gets all cached localized properties
        /// </summary>
        /// <returns>Cached localized properties</returns>
        protected virtual IList<LocalizedProperty> GetAllLocalizedProperties()
        {
            return GetAll().ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="localeKeyGroup">Locale key group</param>
        /// <param name="localeKey">Locale key</param>
        /// <returns>Found localized value</returns>
        public virtual string GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocalizationDefaults.LocalizedPropertyCacheKey
                , languageId, entityId, localeKeyGroup, localeKey);

            return _staticCacheManager.Get(key, () =>
            {
                var source = _localizationSettings.LoadAllLocalizedPropertiesOnStartup
                    //load all records (we know they are cached)
                    ? GetAllLocalizedProperties().AsQueryable()
                    //gradual loading
                    : context.LocalizedProperties;

                var query = from lp in source
                            where lp.LanguageId == languageId &&
                                  lp.EntityId == entityId &&
                                  lp.LocaleKeyGroup == localeKeyGroup &&
                                  lp.LocaleKey == localeKey
                            select lp.LocaleValue;

                //little hack here. nulls aren't cacheable so set it to ""
                var localeValue = query.FirstOrDefault() ?? string.Empty;

                return localeValue;
            });
        }

        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        public virtual void SaveLocalizedValue<T>(T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity
        {
            SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }

        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        public virtual void SaveLocalizedValue<T, TPropType>(T entity,
            Expression<Func<T, TPropType>> keySelector,
            TPropType localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            if (!(keySelector.Body is MemberExpression member))
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
            var localeKeyGroup = entity.GetType().Name;
            var localeKey = propInfo.Name;

            var props = GetLocalizedProperties(entity.Id, localeKeyGroup);
            var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
                lp.LocaleKey.Equals(localeKey, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var localeValueStr = CommonHelper.To<string>(localeValue);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //delete
                    Delete(prop.Id);
                }
                else
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    Update(prop);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                    return;

                //insert
                prop = new LocalizedProperty
                {
                    EntityId = entity.Id,
                    LanguageId = languageId,
                    LocaleKey = localeKey,
                    LocaleKeyGroup = localeKeyGroup,
                    LocaleValue = localeValueStr
                };
                Insert(prop);
            }
        }
        private static Type GetUnproxiedType(object source)
        {
            var proxy = (source as IProxyTargetAccessor);

            if (proxy == null)
                return source.GetType();

            return proxy.GetType().BaseType;
        }

        #endregion
    }
}
