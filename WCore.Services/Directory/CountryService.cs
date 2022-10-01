using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Stores;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;
using WCore.Services.Localization;

namespace WCore.Services.Directory
{
    /// <summary>
    /// Country service
    /// </summary>
    public partial class CountryService : Repository<Country>, ICountryService
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public CountryService(WCoreContext context,
            CatalogSettings catalogSettings,
            StoreInformationSettings storeInformationSettings,
            ICacheKeyService cacheKeyService,
            IStaticCacheManager staticCacheManager,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            IRepository<Country> countryRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IStoreContext storeContext) : base(context)
        {
            this._catalogSettings = catalogSettings;
            this._storeInformationSettings = storeInformationSettings;
            this._cacheKeyService = cacheKeyService;
            this._staticCacheManager = staticCacheManager;
            this._eventPublisher = eventPublisher;
            this._localizationService = localizationService;
            this._countryRepository = countryRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._storeContext = storeContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void DeleteCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            _countryRepository.Delete(country);

            //event notification
            _eventPublisher.EntityDeleted(country);
        }

        public virtual IPagedList<Country> GetAllByFilters(string Name = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? Published = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<Country> query = context.Set<Country>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreDirectoryDefaults.AllByFilters,
                Name,
                IsActive,
                Published,
                Deleted,
                Skip,
                Take);

            if (!string.IsNullOrEmpty(Name))
                query = query.Where(a => a.Name.Contains(Name));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Published.HasValue)
                query = query.Where(a => a.Published == Published);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<Country>(data, Skip, Take, queryCount);
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountries(int languageId = 0, bool showHidden = false)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreDirectoryDefaults.CountriesAllCacheKey, languageId, showHidden);

            return _staticCacheManager.Get(key, () =>
            {
                var query = context.Countries.AsQueryable();
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

                if (!showHidden && !_catalogSettings.IgnoreStoreLimitations)
                {
                    //Store mapping
                    var currentStoreId = _storeContext.CurrentStore.Id;
                    query = from c in query
                            join sc in context.StoreMappings
                            on new { c1 = c.Id, c2 = nameof(Country) } equals new { c1 = sc.EntityId, c2 = sc.EntityName } into c_sc
                            from sc in c_sc.DefaultIfEmpty()
                            where !c.LimitedToStores || currentStoreId == sc.StoreId
                            select c;

                    query = query.Distinct().OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);
                }

                var countries = query.ToList();

                if (languageId > 0)
                {
                    //we should sort countries by localized names when they have the same display order
                    countries = countries
                        .OrderBy(c => c.DisplayOrder)
                        .ThenBy(c => _localizationService.GetLocalized(c, x => x.Name, languageId))
                        .ToList();
                }

                return countries;
            });
        }

        /// <summary>
        /// Gets all countries that allow billing
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountriesForBilling(int languageId = 0, bool showHidden = false)
        {
            return GetAllCountries(languageId, showHidden).Where(c => c.AllowsBilling).ToList();
        }

        /// <summary>
        /// Gets all countries that allow shipping
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountriesForShipping(int languageId = 0, bool showHidden = false)
        {
            return GetAllCountries(languageId, showHidden).Where(c => c.AllowsShipping).ToList();
        }

        /// <summary>
        /// Gets a country by address 
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryByAddress(Address address)
        {
            return GetCountryById(address?.CountryId ?? 0);
        }

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryById(int countryId)
        {
            if (countryId == 0)
                return null;

            return _countryRepository.ToCachedGetById(countryId);
        }

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public virtual Country GetDefaultCountry()
        {
            return GetCountryById(_storeInformationSettings.DefaultCountryId);
        }

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="countryIds">Country identifiers</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetCountriesByIds(int[] countryIds)
        {
            if (countryIds == null || countryIds.Length == 0)
                return new List<Country>();

            var query = from c in context.Countries
                        where countryIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Country>();
            foreach (var id in countryIds)
            {
                var country = countries.Find(x => x.Id == id);
                if (country != null)
                    sortedCountries.Add(country);
            }

            return sortedCountries;
        }

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="twoLetterIsoCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            if (string.IsNullOrEmpty(twoLetterIsoCode))
                return null;

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreDirectoryDefaults.CountriesByTwoLetterCodeCacheKey, twoLetterIsoCode);

            var query = from c in context.Countries
                        where c.TwoLetterIsoCode == twoLetterIsoCode
                        select c;

            return query.ToCachedFirstOrDefault(key);
        }

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="threeLetterIsoCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryByThreeLetterIsoCode(string threeLetterIsoCode)
        {
            if (string.IsNullOrEmpty(threeLetterIsoCode))
                return null;

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreDirectoryDefaults.CountriesByThreeLetterCodeCacheKey, threeLetterIsoCode);

            var query = from c in context.Countries
                        where c.ThreeLetterIsoCode == threeLetterIsoCode
                        select c;

            return query.ToCachedFirstOrDefault(key);
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void InsertCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            _countryRepository.Insert(country);

            //event notification
            _eventPublisher.EntityInserted(country);
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void UpdateCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            _countryRepository.Update(country);

            //event notification
            _eventPublisher.EntityUpdated(country);
        }

        #endregion
    }
}