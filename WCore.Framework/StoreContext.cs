using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Stores;
using WCore.Core.Infrastructure;
using WCore.Services.Common;
using WCore.Services.Stores;
using WCore.Services.Users;

namespace WCore.Framework
{
    /// <summary>
    /// Store context for web application
    /// </summary>
    public partial class StoreContext : IStoreContext
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStoreService _storeService;

        private Store _cachedStore;
        private int? _cachedActiveStoreScopeConfiguration;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="storeService">Store service</param>
        public StoreContext(IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            IStoreService storeService)
        {
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
            _storeService = storeService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current store
        /// </summary>
        public virtual Store CurrentStore
        {
            get
            {
                if (_cachedStore != null)
                    return _cachedStore;

                //try to determine the current store by HOST header
                string host = _httpContextAccessor.HttpContext?.Request?.Headers[HeaderNames.Host];

                var allStores = _storeService.GetAllByFilters();
                var store = allStores.FirstOrDefault(s => _storeService.ContainsHostValue(s, host));

                if (store == null)
                {
                    //load the first found store
                    store = allStores.FirstOrDefault();
                }

                _cachedStore = store ?? throw new Exception("No store could be loaded");

                return _cachedStore;
            }
        }

        /// <summary>
        /// Gets active store scope configuration
        /// </summary>
        public virtual int ActiveStoreScopeConfiguration
        {
            get
            {
                if (_cachedActiveStoreScopeConfiguration.HasValue)
                    return _cachedActiveStoreScopeConfiguration.Value;

                //ensure that we have 2 (or more) stores
                if (_storeService.GetAllByFilters().Count > 1)
                {
                    //do not inject IWorkContext via constructor because it'll cause circular references
                    var currentCustomer = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;

                    //try to get store identifier from attributes
                    var storeId = _genericAttributeService
                        .GetAttribute<int>(currentCustomer, WCoreUsersDefaults.AdminAreaStoreScopeConfigurationAttribute);

                    _cachedActiveStoreScopeConfiguration = _storeService.GetById(storeId)?.Id ?? 0;
                }
                else
                    _cachedActiveStoreScopeConfiguration = 0;

                return _cachedActiveStoreScopeConfiguration ?? 0;
            }
        }

        #endregion
    }
}
