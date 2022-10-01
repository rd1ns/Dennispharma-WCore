﻿using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Configuration;
using WCore.Core.Domain;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Security;
using WCore.Core.Domain.Settings;
using WCore.Services.Logging;
using WCore.Services.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using WCore.Services.Events;
using WCore.Services.Caching;
using WCore.Services.Plugins;

namespace WCore.Services.Localization
{
    public class LocalizationService : Repository<LocaleStringResource>, ILocalizationService
    {

        #region Fields
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;
        private readonly LocalizationSettings _localizationSettings;
        #endregion


        #region Ctor

        public LocalizationService(WCoreContext context, 
            ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            ILocalizedEntityService localizedEntityService,
            ILogger logger,
            ISettingService settingService,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext,
            LocalizationSettings localizationSettings) : base(context)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _localizedEntityService = localizedEntityService;
            _logger = logger;
            _settingService = settingService;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Utilities

        public IPagedList<LocaleStringResource> GetAllByFilters(int languageId, string searchValue = "", string searchName = "", int skip = 0, int take = 10)
        {
            IQueryable<LocaleStringResource> recordsFiltered = context.Set<LocaleStringResource>().Where(o => o.LanguageId == languageId);

            if (!string.IsNullOrEmpty(searchValue))
                recordsFiltered = recordsFiltered.Where(o => o.ResourceValue.Contains(searchValue));

            if (!string.IsNullOrEmpty(searchName))
                recordsFiltered = recordsFiltered.Where(o => o.ResourceName.Contains(searchName));


            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<LocaleStringResource>().Count();

            var data = recordsFiltered.Skip(skip).Take(take).ToList();

            return new PagedList<LocaleStringResource>(data, skip, take, recordsFilteredCount);
        }

        /// <summary>
        /// Insert resources
        /// </summary>
        /// <param name="resources">Resources</param>
        protected virtual void InsertLocaleStringResources(IList<LocaleStringResource> resources)
        {
            foreach (var resource in resources)
            {
                Insert(resource);
            }
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        protected virtual IList<LocaleStringResource> GetAllResources(int languageId)
        {
            var locales = context.LocaleStringResources.Where(o => o.LanguageId == languageId).ToList();
            return locales;
        }

        /// <summary>
        /// Update resources
        /// </summary>
        /// <param name="resources">Resources</param>
        protected virtual void UpdateLocaleStringResources(IList<LocaleStringResource> resources)
        {
            foreach (var resource in resources)
            {
                Update(resource);
            }
        }

        protected virtual HashSet<(string name, string value)> LoadLocaleResourcesFromStream(StreamReader xmlStreamReader, string language)
        {
            var result = new HashSet<(string name, string value)>();

            using (var xmlReader = XmlReader.Create(xmlStreamReader))
                while (xmlReader.ReadToFollowing("Language"))
                {
                    if (xmlReader.NodeType != XmlNodeType.Element)
                        continue;

                    using var languageReader = xmlReader.ReadSubtree();
                    while (languageReader.ReadToFollowing("LocaleResource"))
                        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.GetAttribute("Name") is string name)
                        {
                            using var lrReader = languageReader.ReadSubtree();
                            if (lrReader.ReadToFollowing("Value") && lrReader.NodeType == XmlNodeType.Element)
                                result.Add((name.ToLowerInvariant(), lrReader.ReadString()));
                        }

                    break;
                }

            return result;
        }

        private static Dictionary<string, KeyValuePair<int, string>> ResourceValuesToDictionary(IEnumerable<LocaleStringResource> locales)
        {
            //format: <name, <id, value>>
            var dictionary = new Dictionary<string, KeyValuePair<int, string>>();
            foreach (var locale in locales)
            {
                var resourceName = locale.ResourceName.ToLowerInvariant();
                if (!dictionary.ContainsKey(resourceName))
                    dictionary.Add(resourceName, new KeyValuePair<int, string>(locale.Id, locale.ResourceValue));
            }

            return dictionary;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName)
        {
            if (_workContext.WorkingLanguage != null)
                return GetLocaleStringResourceByName(resourceName, _workContext.WorkingLanguage.Id);

            return null;
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId,
            bool logIfNotFound = true)
        {
            var query = from lsr in context.LocaleStringResources
                        orderby lsr.ResourceName
                        where lsr.LanguageId == languageId && lsr.ResourceName == resourceName
                        select lsr;

            var localeStringResource = query.FirstOrDefault();

            //if (localeStringResource == null && logIfNotFound)
                //_logger.Warning($"Resource string ({resourceName}) not found. Language ID = {languageId}");

            return localeStringResource;
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void InsertLocaleStringResource(LocaleStringResource localeStringResource)
        {
            Insert(localeStringResource);
            _staticCacheManager.RemoveByPrefix(WCoreEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void UpdateLocaleStringResource(LocaleStringResource localeStringResource)
        {
            Update(localeStringResource);
            _staticCacheManager.RemoveByPrefix(WCoreEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="loadPublicLocales">A value indicating whether to load data for the public store only (if "false", then for admin area only. If null, then load all locales. We use it for performance optimization of the site startup</param>
        /// <returns>Locale string resources</returns>
        public virtual Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId, bool? loadPublicLocales)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocaleStringResourceDefaults.ByLanguage, languageId);

            //get all locale string resources by language identifier
            if (!loadPublicLocales.HasValue || _staticCacheManager.IsSet(key))
            {
                var rez = _staticCacheManager.Get(key, () =>
                {
                    //we use no tracking here for performance optimization
                    //anyway records are loaded only for read-only operations
                    var query = from l in context.Set<LocaleStringResource>()
                                orderby l.ResourceName
                                where l.LanguageId == languageId
                                select l;

                    return ResourceValuesToDictionary(query);
                });

                //remove separated resource 
                _staticCacheManager.Remove(_cacheKeyService.PrepareKeyForDefaultCache(WCoreLocaleStringResourceDefaults.LocaleStringResourcesAllPublicCacheKey, languageId));
                _staticCacheManager.Remove(_cacheKeyService.PrepareKeyForDefaultCache(WCoreLocaleStringResourceDefaults.LocaleStringResourcesAllAdminCacheKey, languageId));

                return rez;
            }

            //performance optimization of the site startup
            key = _cacheKeyService.PrepareKeyForDefaultCache(
                    loadPublicLocales.Value ? WCoreLocaleStringResourceDefaults.LocaleStringResourcesAllPublicCacheKey : WCoreLocaleStringResourceDefaults.LocaleStringResourcesAllAdminCacheKey,
                    languageId);

            return _staticCacheManager.Get(key, () =>
            {
                //we use no tracking here for performance optimization
                //anyway records are loaded only for read-only operations
                var query = from l in context.Set<LocaleStringResource>()
                            orderby l.ResourceName
                            where l.LanguageId == languageId
                            select l;
                query = loadPublicLocales.Value ? query.Where(r => !r.ResourceName.StartsWith(WCoreLocalizationDefaults.AdminLocaleStringResourcesPrefix)) : query.Where(r => r.ResourceName.StartsWith(WCoreLocalizationDefaults.AdminLocaleStringResourcesPrefix));
                return ResourceValuesToDictionary(query);
            });
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey)
        {
            if (_workContext.WorkingLanguage != null)
                return GetResource(resourceKey, _workContext.WorkingLanguage.Id);

            return string.Empty;
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="returnEmptyIfNotFound">A value indicating whether an empty string will be returned if a resource is not found and default value is set to empty string</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey, int languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            var result = string.Empty;
            if (resourceKey == null)
                resourceKey = string.Empty;
            resourceKey = resourceKey.Trim().ToLowerInvariant();
            if (_localizationSettings.LoadAllLocaleRecordsOnStartup)
            {
                //load all records (we know they are cached)
                var resources = GetAllResourceValues(languageId, !resourceKey.StartsWith(WCoreLocalizationDefaults.AdminLocaleStringResourcesPrefix, StringComparison.InvariantCultureIgnoreCase));
                if (resources.ContainsKey(resourceKey))
                {
                    result = resources[resourceKey].Value;
                }
            }
            else
            {
                //gradual loading
                var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreLocaleStringResourceDefaults.LocaleStringResourcesByNameCacheKey
                    , languageId, resourceKey);

                var query = from l in context.LocaleStringResources
                            where l.ResourceName == resourceKey
                                  && l.LanguageId == languageId
                            select l.ResourceValue;

                var lsr = _staticCacheManager.Get(key, query.FirstOrDefault);

                if (lsr != null)
                    result = lsr;
            }

            if (!string.IsNullOrEmpty(result))
                return result;

            //if (logIfNotFound)
            //    _logger.Warning($"Resource string ({resourceKey}) is not found. Language ID = {languageId}");

            if (!string.IsNullOrEmpty(defaultValue))
            {
                result = defaultValue;
            }
            else
            {
                if (!returnEmptyIfNotFound)
                    result = resourceKey;
            }

            return result;
        }

        /// <summary>
        /// Export language resources to XML
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportResourcesToXml(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));
            using var stream = new MemoryStream();
            using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Language");
                xmlWriter.WriteAttributeString("Name", language.Name);
                xmlWriter.WriteAttributeString("SupportedVersion", WCoreVersion.CurrentVersion);

                var resources = GetAllResources(language.Id);
                foreach (var resource in resources)
                {
                    xmlWriter.WriteStartElement("LocaleResource");
                    xmlWriter.WriteAttributeString("Name", resource.ResourceName);
                    xmlWriter.WriteElementString("Value", null, resource.ResourceValue);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// Import language resources from XML file
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="xmlStreamReader">Stream reader of XML file</param>
        /// <param name="updateExistingResources">A value indicating whether to update existing resources</param>
        public virtual void ImportResourcesFromXml(Language language, StreamReader xmlStreamReader, bool updateExistingResources = true)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (xmlStreamReader.EndOfStream)
                return;

            var lsNamesList = new Dictionary<string, LocaleStringResource>();

            foreach (var localeStringResource in context.LocaleStringResources.Where(lsr => lsr.LanguageId == language.Id)
                .OrderBy(lsr => lsr.Id))
                lsNamesList[localeStringResource.ResourceName.ToLowerInvariant()] = localeStringResource;

            var lrsToUpdateList = new List<LocaleStringResource>();
            var lrsToInsertList = new Dictionary<string, LocaleStringResource>();

            foreach (var (name, value) in LoadLocaleResourcesFromStream(xmlStreamReader, language.Name))
            {
                if (lsNamesList.ContainsKey(name))
                {
                    if (!updateExistingResources)
                        continue;

                    var lsr = lsNamesList[name];
                    lsr.ResourceValue = value;
                    lrsToUpdateList.Add(lsr);
                }
                else
                {
                    var lsr = new LocaleStringResource { LanguageId = language.Id, ResourceName = name, ResourceValue = value };
                    lrsToInsertList[name] = lsr;
                }
            }

            UpdateLocaleStringResources(lrsToUpdateList);
            InsertLocaleStringResources(lrsToInsertList.Values.ToList());

            //clear cache
            _staticCacheManager.RemoveByPrefix(WCoreEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        /// <summary>
        /// Get localized property of an entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier; pass null to use the current working language; pass 0 to get standard language value</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        /// <returns>Localized property</returns>
        public virtual TPropType GetLocalized<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector,
            int? languageId = null, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where TEntity : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (!(keySelector.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

            var result = default(TPropType);
            var resultStr = string.Empty;

            var localeKeyGroup = entity.GetType().Name.Replace("Proxy", "");
            var localeKey = propInfo.Name;

            if (!languageId.HasValue)
                languageId = _workContext.WorkingLanguage.Id;

            if (languageId > 0)
            {
                //ensure that we have at least two published languages
                var loadLocalizedValue = true;
                if (ensureTwoPublishedLanguages)
                {
                    var totalPublishedLanguages = _languageService.GetAllCount();
                    loadLocalizedValue = totalPublishedLanguages >= 2;
                }

                //localized value
                if (loadLocalizedValue)
                {
                    resultStr = _localizedEntityService
                        .GetLocalizedValue(languageId.Value, entity.Id, localeKeyGroup, localeKey);
                    if (!string.IsNullOrEmpty(resultStr))
                        result = CommonHelper.To<TPropType>(resultStr);
                }
            }

            //set default value if required
            if (!string.IsNullOrEmpty(resultStr) || !returnDefaultValue)
                return result;
            var localizer = keySelector.Compile();
            result = localizer(entity);

            return result;
        }

        /// <summary>
        /// Get localized property of setting
        /// </summary>
        /// <typeparam name="TSettings">Settings type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        /// <returns>Localized property</returns>
        public virtual string GetLocalizedSetting<TSettings>(TSettings settings, Expression<Func<TSettings, string>> keySelector,
            int languageId, int storeId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where TSettings : ISettings, new()
        {
            var key = _settingService.GetSettingKey(settings, keySelector);

            //we do not support localized settings per store (overridden store settings)
            var setting = _settingService.GetSetting(key, loadSharedValueIfNotFound: true);
            if (setting == null)
                return null;

            return GetLocalized(setting, x => x.Value, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        /// <summary>
        /// Save localized property of setting
        /// </summary>
        /// <typeparam name="TSettings">Settings type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="value">Localized value</param>
        /// <returns>Localized property</returns>
        public virtual void SaveLocalizedSetting<TSettings>(TSettings settings, Expression<Func<TSettings, string>> keySelector,
            int languageId, string value) where TSettings : ISettings, new()
        {
            var key = _settingService.GetSettingKey(settings, keySelector);

            //we do not support localized settings per store (overridden store settings)
            var setting = _settingService.GetSetting(key);
            if (setting == null)
                return;

            _localizedEntityService.SaveLocalizedValue(setting, x => x.Value, value, languageId);
        }

        /// <summary>
        /// Get localized value of enum
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <param name="languageId">Language identifier; pass null to use the current working language</param>
        /// <returns>Localized value</returns>
        public virtual string GetLocalizedEnum<TEnum>(TEnum enumValue, int? languageId = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            //localized value
            var resourceName = $"{WCoreLocalizationDefaults.EnumLocaleStringResourcesPrefix}{typeof(TEnum)}.{enumValue}";
            var result = GetResource(resourceName, languageId ?? _workContext.WorkingLanguage.Id, false, string.Empty, true);

            //set default value if required
            if (string.IsNullOrEmpty(result))
                result = CommonHelper.ConvertEnum(enumValue.ToString());

            return result;
        }

        /// <summary>
        /// Get localized value of enum
        /// We don't have UI to manage permission localizable name. That's why we're using this method
        /// </summary>
        /// <param name="permissionRecord">Permission record</param>
        /// <param name="languageId">Language identifier; pass null to use the current working language</param>
        /// <returns>Localized value</returns>
        public virtual string GetLocalizedPermissionName(PermissionRecord permissionRecord, int? languageId = null)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));

            //localized value
            var resourceName = $"{WCoreLocalizationDefaults.PermissionLocaleStringResourcesPrefix}{permissionRecord.SystemName}";
            var result = GetResource(resourceName, languageId ?? _workContext.WorkingLanguage.Id, false, string.Empty, true);

            //set default value if required
            if (string.IsNullOrEmpty(result))
                result = permissionRecord.Name;

            return result;
        }

        /// <summary>
        /// Save localized name of a permission
        /// </summary>
        /// <param name="permissionRecord">Permission record</param>
        public virtual void SaveLocalizedPermissionName(PermissionRecord permissionRecord)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));

            var resourceName = $"{WCoreLocalizationDefaults.PermissionLocaleStringResourcesPrefix}{permissionRecord.SystemName}";
            var resourceValue = permissionRecord.Name;

            foreach (var lang in _languageService.GetAll())
            {
                var lsr = GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr == null)
                {
                    lsr = new LocaleStringResource
                    {
                        LanguageId = lang.Id,
                        ResourceName = resourceName,
                        ResourceValue = resourceValue
                    };
                    InsertLocaleStringResource(lsr);
                }
                else
                {
                    lsr.ResourceValue = resourceValue;
                    UpdateLocaleStringResource(lsr);
                }
            }
        }

        /// <summary>
        /// Delete a localized name of a permission
        /// </summary>
        /// <param name="permissionRecord">Permission record</param>
        public virtual void DeleteLocalizedPermissionName(PermissionRecord permissionRecord)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));

            var resourceName = $"{WCoreLocalizationDefaults.PermissionLocaleStringResourcesPrefix}{permissionRecord.SystemName}";
            foreach (var lang in _languageService.GetAll())
            {
                var lsr = GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr != null)
                    Delete(lsr);
            }
        }

        /// <summary>
        /// Add a locale resource (if new) or update an existing one
        /// </summary>
        /// <param name="resourceName">Resource name</param>
        /// <param name="resourceValue">Resource value</param>
        /// <param name="languageCulture">Language culture code. If null or empty, then a resource will be added for all languages</param>
        public virtual void AddOrUpdateLocaleResource(string resourceName, string resourceValue, string languageCulture = null)
        {
            foreach (var lang in _languageService.GetAll())
            {
                if (!string.IsNullOrEmpty(languageCulture) && !languageCulture.Equals(lang.LanguageCulture))
                    continue;

                var lsr = GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr == null)
                {
                    lsr = new LocaleStringResource
                    {
                        LanguageId = lang.Id,
                        ResourceName = resourceName,
                        ResourceValue = resourceValue
                    };
                    InsertLocaleStringResource(lsr);
                }
                else
                {
                    lsr.ResourceValue = resourceValue;
                    UpdateLocaleStringResource(lsr);
                }
            }
        }

        /// <summary>
        /// Add locale resources
        /// </summary>
        /// <param name="resources">Resource name-value pairs</param>
        /// <param name="languageId">Language identifier; pass null to add the passed resources for all languages</param>
        public virtual void AddLocaleResource(IDictionary<string, string> resources, int? languageId = null)
        {
            //first delete all previous locales with the passed names if they exist
            DeleteLocaleResources(resources.Keys.ToList(), languageId);

            //insert new locale resources
            var locales = _languageService.GetAll()
                .Where(language => !languageId.HasValue || language.Id == languageId.Value)
                .SelectMany(language => resources.Select(resource => new LocaleStringResource
                {
                    LanguageId = language.Id,
                    ResourceName = resource.Key,
                    ResourceValue = resource.Value
                }))
                .ToList();
            InsertLocaleStringResources(locales);

            //clear cache
            _staticCacheManager.RemoveByPrefix(WCoreEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        /// <summary>
        /// Delete a locale resource
        /// </summary>
        /// <param name="resourceName">Resource name</param>
        public virtual void DeleteLocaleResource(string resourceName)
        {
            foreach (var lang in _languageService.GetAll())
            {
                var lsr = GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr != null)
                    Delete(lsr);
            }
        }

        /// <summary>
        /// Delete locale resources
        /// </summary>
        /// <param name="resourceNames">Resource names</param>
        /// <param name="languageId">Language identifier; pass null to delete the passed resources from all languages</param>
        public virtual void DeleteLocaleResources(IList<string> resourceNames, int? languageId = null)
        {
            //    Delete(locale => (!languageId.HasValue || locale.LanguageId == languageId.Value) &&
            //        resourceNames.Contains(locale.ResourceName, StringComparer.InvariantCultureIgnoreCase));

            //clear cache
            _staticCacheManager.RemoveByPrefix(WCoreEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        /// <summary>
        /// Delete locale resources by the passed name prefix
        /// </summary>
        /// <param name="resourceNamePrefix">Resource name prefix</param>
        /// <param name="languageId">Language identifier; pass null to delete resources by prefix from all languages</param>
        public virtual void DeleteLocaleResources(string resourceNamePrefix, int? languageId = null)
        {
            //_lsrRepository.Delete(locale => (!languageId.HasValue || locale.LanguageId == languageId.Value) &&
            //    !string.IsNullOrEmpty(locale.ResourceName) &&
            //    locale.ResourceName.StartsWith(resourceNamePrefix, StringComparison.InvariantCultureIgnoreCase));

            //clear cache
            _staticCacheManager.RemoveByPrefix(WCoreEntityCacheDefaults<LocaleStringResource>.Prefix);
        }

        /// <summary>
        /// Get localized friendly name of a plugin
        /// </summary>
        /// <typeparam name="TPlugin">Plugin type</typeparam>
        /// <param name="plugin">Plugin</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <returns>Localized value</returns>
        public virtual string GetLocalizedFriendlyName<TPlugin>(TPlugin plugin, int languageId, bool returnDefaultValue = true)
            where TPlugin : IPlugin
        {
            if (plugin == null)
                throw new ArgumentNullException(nameof(plugin));

            if (plugin.PluginDescriptor == null)
                throw new ArgumentException("Plugin descriptor cannot be loaded");

            var systemName = plugin.PluginDescriptor.SystemName;
            //localized value
            var resourceName = $"{WCoreLocalizationDefaults.PluginNameLocaleStringResourcesPrefix}{systemName}";
            var result = GetResource(resourceName, languageId, false, string.Empty, true);

            //set default value if required
            if (string.IsNullOrEmpty(result) && returnDefaultValue)
                result = plugin.PluginDescriptor.FriendlyName;

            return result;
        }

        /// <summary>
        /// Save localized friendly name of a plugin
        /// </summary>
        /// <typeparam name="TPlugin">Plugin</typeparam>
        /// <param name="plugin">Plugin</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="localizedFriendlyName">Localized friendly name</param>
        public virtual void SaveLocalizedFriendlyName<TPlugin>(TPlugin plugin, int languageId, string localizedFriendlyName)
            where TPlugin : IPlugin
        {
            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            if (plugin == null)
                throw new ArgumentNullException(nameof(plugin));

            if (plugin.PluginDescriptor == null)
                throw new ArgumentException("Plugin descriptor cannot be loaded");

            var systemName = plugin.PluginDescriptor.SystemName;
            //localized value
            var resourceName = $"{WCoreLocalizationDefaults.PluginNameLocaleStringResourcesPrefix}{systemName}";
            var resource = GetLocaleStringResourceByName(resourceName, languageId, false);

            if (resource != null)
            {
                if (string.IsNullOrWhiteSpace(localizedFriendlyName))
                {
                    //delete
                    //DeleteLocaleStringResource(resource);
                }
                else
                {
                    //update
                    resource.ResourceValue = localizedFriendlyName;
                    UpdateLocaleStringResource(resource);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(localizedFriendlyName))
                    return;

                //insert
                resource = new LocaleStringResource
                {
                    LanguageId = languageId,
                    ResourceName = resourceName,
                    ResourceValue = localizedFriendlyName
                };
                InsertLocaleStringResource(resource);
            }
        }
        #endregion
    }
}
