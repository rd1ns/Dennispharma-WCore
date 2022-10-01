using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core.Domain.Users;
using WCore.Core.Domain.Shipping;
using WCore.Services.Plugins;
using WCore.Services.Users;

namespace WCore.Services.Shipping
{
    /// <summary>
    /// Represents a shipping plugin manager implementation
    /// </summary>
    public partial class ShippingPluginManager : PluginManager<IShippingRateComputationMethod>, IShippingPluginManager
    {
        #region Fields

        private readonly ShippingSettings _shippingSettings;
        private readonly IUserService _userService;

        #endregion

        #region Ctor

        public ShippingPluginManager(IPluginService pluginService,
            ShippingSettings shippingSettings,
            IUserService userService) : base(userService,pluginService)
        {
            _shippingSettings = shippingSettings;
            _userService= userService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load active shipping providers
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <param name="systemName">Filter by shipping provider system name; pass null to load all plugins</param>
        /// <returns>List of active shipping providers</returns>
        public virtual IList<IShippingRateComputationMethod> LoadActivePlugins(User user = null, int storeId = 0, string systemName = null)
        {
            var shippingProviders = LoadActivePlugins(_shippingSettings.ActiveShippingRateComputationMethodSystemNames, user, storeId);

            //filter by passed system name
            if (!string.IsNullOrEmpty(systemName))
            {
                shippingProviders = shippingProviders
                    .Where(provider => provider.PluginDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }

            return shippingProviders;
        }

        /// <summary>
        /// Check whether the passed shipping provider is active
        /// </summary>
        /// <param name="shippingProvider">Shipping provider to check</param>
        /// <returns>Result</returns>
        public virtual bool IsPluginActive(IShippingRateComputationMethod shippingProvider)
        {
            return IsPluginActive(shippingProvider, _shippingSettings.ActiveShippingRateComputationMethodSystemNames);
        }

        /// <summary>
        /// Check whether the shipping provider with the passed system name is active
        /// </summary>
        /// <param name="systemName">System name of shipping provider to check</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>Result</returns>
        public virtual bool IsPluginActive(string systemName, User user = null, int storeId = 0)
        {
            var shippingProvider = LoadPluginBySystemName(systemName, user, storeId);
            return IsPluginActive(shippingProvider);
        }

        #endregion
    }
}