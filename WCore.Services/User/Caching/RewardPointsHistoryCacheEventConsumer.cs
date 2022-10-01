using WCore.Core.Domain.Users;
using WCore.Services.Caching;

namespace WCore.Services.Users.Caching
{
    /// <summary>
    /// Represents a reward point history cache event consumer
    /// </summary>
    public partial class RewardPointsHistoryCacheEventConsumer : CacheEventConsumer<RewardPointsHistory>
    {
    }
}
