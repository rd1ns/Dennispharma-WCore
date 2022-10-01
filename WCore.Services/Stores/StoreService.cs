using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Stores;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;

namespace WCore.Services.Stores
{
    /// <summary>
    /// Store service
    /// </summary>
    public partial class StoreService : Repository<Store>, IStoreService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Store> _storeRepository;

        #endregion

        #region Ctor

        public StoreService(WCoreContext context, IEventPublisher eventPublisher,
            IRepository<Store> storeRepository) : base(context)
        {
            _eventPublisher = eventPublisher;
            _storeRepository = storeRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void DeleteStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            var allStores = GetAllByFilters();
            if (allStores.Count == 1)
                throw new Exception("You cannot delete the only configured store");

            _storeRepository.Delete(store);
        }

        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <returns>Stores</returns>
        public virtual IPagedList<Store> GetAllByFilters(int skip = 0, int take = int.MaxValue) 
        {

            IQueryable<Store> recordsFiltered = context.Set<Store>();

            int recordsFilteredCount = recordsFiltered.Count();

            var data = recordsFiltered.OrderByDescending(o => o.DisplayOrder).Skip(skip).Take(take).ToCachedList(WCoreStoreDefaults.StoresAllCacheKey);

            return new PagedList<Store>(data, skip, take, recordsFilteredCount);
        }
        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Comma-separated hosts</returns>
        public virtual string[] ParseHostValues(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            var parsedValues = new List<string>();
            if (string.IsNullOrEmpty(store.Hosts))
                return parsedValues.ToArray();

            var hosts = store.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var host in hosts)
            {
                var tmp = host.Trim();
                if (!string.IsNullOrEmpty(tmp))
                    parsedValues.Add(tmp);
            }

            return parsedValues.ToArray();
        }

        /// <summary>
        /// Indicates whether a store contains a specified host
        /// </summary>
        /// <param name="store">Store</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        public virtual bool ContainsHostValue(Store store, string host)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            if (string.IsNullOrEmpty(host))
                return false;

            var contains = ParseHostValues(store).Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));

            return contains;
        }

        /// <summary>
        /// Returns a list of names of not existing stores
        /// </summary>
        /// <param name="storeIdsNames">The names and/or IDs of the store to check</param>
        /// <returns>List of names and/or IDs not existing stores</returns>
        public string[] GetNotExistingStores(string[] storeIdsNames)
        {
            if (storeIdsNames == null)
                throw new ArgumentNullException(nameof(storeIdsNames));

            var query = context.Stores;
            var queryFilter = storeIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(store => store.Name).Where(store => queryFilter.Contains(store)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (!queryFilter.Any())
                return queryFilter.ToArray();

            //filtering by IDs
            filter = query.Select(store => store.Id.ToString()).Where(store => queryFilter.Contains(store)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        #endregion
    }
}