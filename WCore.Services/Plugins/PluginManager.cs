using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core.Domain.Users;
using WCore.Services.Users;

namespace WCore.Services.Plugins
{
    /// <summary>
    /// Represents a plugin manager implementation
    /// </summary>
    /// <typeparam name="TPlugin">Type of plugin</typeparam>
    public partial class PluginManager<TPlugin> : IPluginManager<TPlugin> where TPlugin : class, IPlugin
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IPluginService _pluginService;

        private readonly Dictionary<string, IList<TPlugin>> _plugins = new Dictionary<string, IList<TPlugin>>();

        #endregion

        #region Ctor

        public PluginManager(IUserService userService,
            IPluginService pluginService)
        {
            _userService = userService;
            _pluginService = pluginService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare the dictionary key to store loaded plugins
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="systemName">Plugin system name</param>
        /// <returns>Key</returns>
        protected virtual string GetKey(User user, int storeId, string systemName = null)
        {
            //return $"{storeId}-{(user != null ? string.Join(',', _userService.GetUserRoleIds(user)) : null)}-{systemName}";
            return "";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load all plugins
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>List of plugins</returns>
        public virtual IList<TPlugin> LoadAllPlugins(User user = null, int storeId = 0)
        {
            //get plugins and put them into the dictionary to avoid further loading
            var key = GetKey(user, storeId);
            if (!_plugins.ContainsKey(key))
                _plugins.Add(key, _pluginService.GetPlugins<TPlugin>(user: user, storeId: storeId).ToList());

            return _plugins[key];
        }

        /// <summary>
        /// Load plugin by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>Plugin</returns>
        public virtual TPlugin LoadPluginBySystemName(string systemName, User user = null, int storeId = 0)
        {
            if (string.IsNullOrEmpty(systemName))
                return null;

            //try to get already loaded plugin
            var key = GetKey(user, storeId, systemName);
            if (_plugins.ContainsKey(key))
                return _plugins[key].FirstOrDefault();

            //or get it from list of all loaded plugins or load it for the first time
            var pluginBySystemName = _plugins.TryGetValue(GetKey(user, storeId), out var plugins)
                && plugins.FirstOrDefault(plugin =>
                    plugin.PluginDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) is TPlugin loadedPlugin
                ? loadedPlugin
                : _pluginService.GetPluginDescriptorBySystemName<TPlugin>(systemName, user: user, storeId: storeId)?.Instance<TPlugin>();

            _plugins.Add(key, new List<TPlugin> { pluginBySystemName });

            return pluginBySystemName;
        }

        /// <summary>
        /// Load primary active plugin
        /// </summary>
        /// <param name="systemName">System name of primary active plugin</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>Plugin</returns>
        public virtual TPlugin LoadPrimaryPlugin(string systemName, User user = null, int storeId = 0)
        {
            //try to get a plugin by system name or return the first loaded one (it's necessary to have a primary active plugin)
            var plugin = LoadPluginBySystemName(systemName, user, storeId)
                ?? LoadAllPlugins(user, storeId).FirstOrDefault();

            return plugin;
        }

        /// <summary>
        /// Load active plugins
        /// </summary>
        /// <param name="systemNames">System names of active plugins</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>List of active plugins</returns>
        public virtual IList<TPlugin> LoadActivePlugins(List<string> systemNames, User user = null, int storeId = 0)
        {
            if (systemNames == null)
                return new List<TPlugin>();

            //get loaded plugins according to passed system names
            return LoadAllPlugins(user, storeId)
                .Where(plugin => systemNames.Contains(plugin.PluginDescriptor.SystemName, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Check whether the passed plugin is active
        /// </summary>
        /// <param name="plugin">Plugin to check</param>
        /// <param name="systemNames">System names of active plugins</param>
        /// <returns>Result</returns>
        public virtual bool IsPluginActive(TPlugin plugin, List<string> systemNames)
        {
            if (plugin == null)
                return false;

            return systemNames
                ?.Any(systemName => plugin.PluginDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                ?? false;
        }

        /// <summary>
        /// Get plugin logo URL
        /// </summary>
        /// <param name="plugin">Plugin</param>
        /// <returns>Logo URL</returns>
        public virtual string GetPluginLogoUrl(TPlugin plugin)
        {
            return _pluginService.GetPluginLogoUrl(plugin.PluginDescriptor);
        }

        #endregion
    }
}