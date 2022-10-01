using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Users;
using WCore.Services.Events;
using WCore.Services.Helpers;
using WCore.Services.Localization;

namespace WCore.Services.Orders
{
    /// <summary>
    /// Reward point service
    /// </summary>
    public partial class RewardPointService : IRewardPointService
    {
        #region Fields

        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IRepository<RewardPointsHistory> _rewardPointsHistoryRepository;
        private readonly RewardPointsSettings _rewardPointsSettings;

        #endregion

        #region Ctor

        public RewardPointService(IDateTimeHelper dateTimeHelper,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            IRepository<RewardPointsHistory> rewardPointsHistoryRepository,
            RewardPointsSettings rewardPointsSettings)
        {
            _dateTimeHelper = dateTimeHelper;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _rewardPointsHistoryRepository = rewardPointsHistoryRepository;
            _rewardPointsSettings = rewardPointsSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get query to load reward points history
        /// </summary>
        /// <param name="userId">User identifier; pass 0 to load all records</param>
        /// <param name="storeId">Store identifier; pass null to load all records</param>
        /// <param name="showNotActivated">Whether to load reward points that did not yet activated</param>
        /// <returns>Query to load reward points history</returns>
        protected virtual IQueryable<RewardPointsHistory> GetRewardPointsQuery(int userId, int? storeId, bool showNotActivated = false)
        {
            var query = _rewardPointsHistoryRepository.GetAll().AsQueryable();

            //filter by user
            if (userId > 0)
                query = query.Where(historyEntry => historyEntry.UserId == userId);

            //filter by store
            if (!_rewardPointsSettings.PointsAccumulatedForAllStores && storeId > 0)
                query = query.Where(historyEntry => historyEntry.StoreId == storeId);

            //whether to show only the points that already activated
            if (!showNotActivated)
            {
                query = query.Where(historyEntry => historyEntry.CreatedOn < DateTime.Now);
            }

            //update points balance
            UpdateRewardPointsBalance(query);

            return query;
        }

        /// <summary>
        /// Update reward points balance if necessary
        /// </summary>
        /// <param name="query">Input query</param>
        protected virtual void UpdateRewardPointsBalance(IQueryable<RewardPointsHistory> query)
        {
            //get expired points
            var now = DateTime.Now;
            var expiredPoints = query
                .Where(historyEntry => historyEntry.EndDate < now && historyEntry.ValidPoints > 0)
                .OrderBy(historyEntry => historyEntry.CreatedOn).ThenBy(historyEntry => historyEntry.Id).ToList();

            //reduce the balance for these points
            foreach (var historyEntry in expiredPoints)
            {
                InsertRewardPointsHistoryEntry(new RewardPointsHistory
                {
                    UserId = historyEntry.UserId,
                    StoreId = historyEntry.StoreId,
                    Points = -historyEntry.ValidPoints.Value,
                    Message = string.Format(_localizationService.GetResource("RewardPoints.Expired"),
                        _dateTimeHelper.ConvertToUserTime(historyEntry.CreatedOn, DateTimeKind.Local)),
                    CreatedOn = historyEntry.EndDate.Value
                });

                historyEntry.ValidPoints = 0;
                UpdateRewardPointsHistoryEntry(historyEntry);
            }

            //get has not yet activated points, but it's time to do it
            var notActivatedPoints = query
                .Where(historyEntry => !historyEntry.PointsBalance.HasValue && historyEntry.CreatedOn < now)
                .OrderBy(historyEntry => historyEntry.CreatedOn).ThenBy(historyEntry => historyEntry.Id).ToList();
            if (!notActivatedPoints.Any())
                return;

            //get current points balance
            //LINQ to entities does not support Last method, thus order by desc and use First one
            var currentPointsBalance = query
                .OrderByDescending(historyEntry => historyEntry.CreatedOn).ThenByDescending(historyEntry => historyEntry.Id)
                .FirstOrDefault(historyEntry => historyEntry.PointsBalance.HasValue)
                ?.PointsBalance ?? 0;

            //update appropriate records
            foreach (var historyEntry in notActivatedPoints)
            {
                currentPointsBalance += historyEntry.Points;
                historyEntry.PointsBalance = currentPointsBalance;
                UpdateRewardPointsHistoryEntry(historyEntry);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load reward point history records
        /// </summary>
        /// <param name="userId">User identifier; 0 to load all records</param>
        /// <param name="storeId">Store identifier; pass null to load all records</param>
        /// <param name="showNotActivated">A value indicating whether to show reward points that did not yet activated</param>
        /// <param name="orderGuid">Order Guid; pass null to load all record</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Reward point history records</returns>
        public virtual IPagedList<RewardPointsHistory> GetRewardPointsHistory(int userId = 0, int? storeId = null,
            bool showNotActivated = false, Guid? orderGuid = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = GetRewardPointsQuery(userId, storeId, showNotActivated);

            if (orderGuid.HasValue)
                query = query.Where(historyEntry => historyEntry.UsedWithOrder == orderGuid.Value);

            query = query.OrderByDescending(historyEntry => historyEntry.CreatedOn)
                .ThenByDescending(historyEntry => historyEntry.Id);

            //return paged reward points history
            return new PagedList<RewardPointsHistory>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets reward points balance
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Balance</returns>
        public virtual int GetRewardPointsBalance(int userId, int storeId)
        {
            var query = GetRewardPointsQuery(userId, storeId)
                .OrderByDescending(historyEntry => historyEntry.CreatedOn).ThenByDescending(historyEntry => historyEntry.Id);

            //return point balance of the first actual history entry
            return query.FirstOrDefault()?.PointsBalance ?? 0;
        }

        /// <summary>
        /// Gets reduced reward points balance per order
        /// </summary>
        /// <param name="rewardPointsBalance">Reward points balance</param>
        /// <returns>Reduced balance</returns>
        public int GetReducedPointsBalance(int rewardPointsBalance)
        {
            if (_rewardPointsSettings.MaximumRewardPointsToUsePerOrder > 0 &&
                rewardPointsBalance > _rewardPointsSettings.MaximumRewardPointsToUsePerOrder)
                return _rewardPointsSettings.MaximumRewardPointsToUsePerOrder;

            return rewardPointsBalance;
        }

        /// <summary>
        /// Add reward points history record
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="points">Number of points to add</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="message">Message</param>
        /// <param name="usedWithOrder">The order for which points were redeemed (spent) as a payment</param>
        /// <param name="usedAmount">Used amount</param>
        /// <param name="activatingDate">Date and time of activating reward points; pass null to immediately activating</param>
        /// <param name="endDate">Date and time when the reward points will no longer be valid; pass null to add date termless points</param>
        /// <returns>Reward points history entry identifier</returns>
        public virtual int AddRewardPointsHistoryEntry(User user, int points, int storeId, string message = "",
            Order usedWithOrder = null, decimal usedAmount = 0M, DateTime? activatingDate = null, DateTime? endDate = null)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (storeId == 0)
                throw new ArgumentException("Store ID should be valid");

            if (points < 0 && endDate.HasValue)
                throw new ArgumentException("End date is available only for positive points amount");

            //insert new history entry
            var newHistoryEntry = new RewardPointsHistory
            {
                UserId = user.Id,
                StoreId = storeId,
                Points = points,
                PointsBalance = activatingDate.HasValue ? null : (int?)(GetRewardPointsBalance(user.Id, storeId) + points),
                UsedAmount = usedAmount,
                Message = message,
                CreatedOn = activatingDate ?? DateTime.Now,
                EndDate = endDate,
                ValidPoints = points > 0 ? (int?)points : null,
                UsedWithOrder = usedWithOrder?.OrderGuid
            };
            InsertRewardPointsHistoryEntry(newHistoryEntry);

            //reduce valid points of previous entries
            if (points >= 0) 
                return newHistoryEntry.Id;

            var withValidPoints = GetRewardPointsQuery(user.Id, storeId)
                .Where(historyEntry => historyEntry.ValidPoints > 0)
                .OrderBy(historyEntry => historyEntry.CreatedOn).ThenBy(historyEntry => historyEntry.Id).ToList();
            foreach (var historyEntry in withValidPoints)
            {
                points += historyEntry.ValidPoints.Value;
                historyEntry.ValidPoints = Math.Max(points, 0);
                UpdateRewardPointsHistoryEntry(historyEntry);

                if (points >= 0)
                    break;
            }

            return newHistoryEntry.Id;
        }

        /// <summary>
        /// Gets a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        /// <returns>Reward point history entry</returns>
        public virtual RewardPointsHistory GetRewardPointsHistoryEntryById(int rewardPointsHistoryId)
        {
            if (rewardPointsHistoryId == 0)
                return null;

            return _rewardPointsHistoryRepository.GetById(rewardPointsHistoryId);
        }

        /// <summary>
        /// Insert the reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistory">Reward point history entry</param>
        public virtual void InsertRewardPointsHistoryEntry(RewardPointsHistory rewardPointsHistory)
        {
            if (rewardPointsHistory == null)
                throw new ArgumentNullException(nameof(rewardPointsHistory));

            _rewardPointsHistoryRepository.Insert(rewardPointsHistory);

            //event notification
            _eventPublisher.EntityInserted(rewardPointsHistory);
        }

        /// <summary>
        /// Update the reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistory">Reward point history entry</param>
        public virtual void UpdateRewardPointsHistoryEntry(RewardPointsHistory rewardPointsHistory)
        {
            if (rewardPointsHistory == null)
                throw new ArgumentNullException(nameof(rewardPointsHistory));

            _rewardPointsHistoryRepository.Update(rewardPointsHistory);

            //event notification
            _eventPublisher.EntityUpdated(rewardPointsHistory);
        }

        /// <summary>
        /// Delete the reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistory">Reward point history entry</param>
        public virtual void DeleteRewardPointsHistoryEntry(RewardPointsHistory rewardPointsHistory)
        {
            if (rewardPointsHistory == null)
                throw new ArgumentNullException(nameof(rewardPointsHistory));

            _rewardPointsHistoryRepository.Delete(rewardPointsHistory);

            //event notification
            _eventPublisher.EntityDeleted(rewardPointsHistory);
        }

        #endregion
    }
}