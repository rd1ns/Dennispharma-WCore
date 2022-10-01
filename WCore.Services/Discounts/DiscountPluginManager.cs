using WCore.Services.Users;
using WCore.Services.Plugins;

namespace WCore.Services.Discounts
{
    /// <summary>
    /// Represents a discount requirement plugin manager implementation
    /// </summary>
    public partial class DiscountPluginManager : PluginManager<IDiscountRequirementRule>, IDiscountPluginManager
    {
        #region Ctor

        public DiscountPluginManager(IUserService userService,
            IPluginService pluginService) : base(userService, pluginService)
        {
        }

        #endregion
    }
}