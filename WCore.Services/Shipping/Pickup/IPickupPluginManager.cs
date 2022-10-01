﻿using System.Collections.Generic;
using WCore.Core.Domain.Users;
using WCore.Services.Plugins;

namespace WCore.Services.Shipping.Pickup
{
    /// <summary>
    /// Represents a pickup point plugin manager
    /// </summary>
    public partial interface IPickupPluginManager : IPluginManager<IPickupPointProvider>
    {
        /// <summary>
        /// Load active pickup point providers
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <param name="systemName">Filter by pickup point provider system name; pass null to load all plugins</param>
        /// <returns>List of active pickup point providers</returns>
        IList<IPickupPointProvider> LoadActivePlugins(User user = null, int storeId = 0, string systemName = null);

        /// <summary>
        /// Check whether the passed pickup point provider is active
        /// </summary>
        /// <param name="pickupPointProvider">Pickup point provider to check</param>
        /// <returns>Result</returns>
        bool IsPluginActive(IPickupPointProvider pickupPointProvider);

        /// <summary>
        /// Check whether the pickup point provider with the passed system name is active
        /// </summary>
        /// <param name="systemName">System name of pickup point provider to check</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>Result</returns>
        bool IsPluginActive(string systemName, User user = null, int storeId = 0);
    }
}