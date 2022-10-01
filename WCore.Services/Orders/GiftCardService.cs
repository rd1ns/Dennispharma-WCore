using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Users;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;
using WCore.Services.Users;

namespace WCore.Services.Orders
{
    /// <summary>
    /// Gift card service
    /// </summary>
    public partial class GiftCardService : Repository<GiftCard>, IGiftCardService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<GiftCardUsageHistory> _giftCardUsageHistoryRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        #endregion

        #region Ctor

        public GiftCardService(WCoreContext context, IUserService userService,
            IEventPublisher eventPublisher,
            IRepository<GiftCardUsageHistory> giftCardUsageHistoryRepository,
            IRepository<OrderItem> orderItemRepository) : base(context)
        {
            _userService = userService;
            _eventPublisher = eventPublisher;
            _giftCardUsageHistoryRepository = giftCardUsageHistoryRepository;
            _orderItemRepository = orderItemRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a gift card
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        public virtual void DeleteGiftCard(GiftCard giftCard)
        {
            if (giftCard == null)
                throw new ArgumentNullException(nameof(giftCard));

            Delete(giftCard);
        }

        /// <summary>
        /// Gets a gift card
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <returns>Gift card entry</returns>
        public virtual GiftCard GetGiftCardById(int giftCardId)
        {
            if (giftCardId == 0)
                return null;

            return GetById(giftCardId);
        }

        /// <summary>
        /// Gets all gift cards
        /// </summary>
        /// <param name="purchasedWithOrderId">Associated order ID; null to load all records</param>
        /// <param name="usedWithOrderId">The order ID in which the gift card was used; null to load all records</param>
        /// <param name="createdFrom">Created date from (UTC); null to load all records</param>
        /// <param name="createdTo">Created date to (UTC); null to load all records</param>
        /// <param name="isGiftCardActivated">Value indicating whether gift card is activated; null to load all records</param>
        /// <param name="giftCardCouponCode">Gift card coupon code; null to load all records</param>
        /// <param name="recipientName">Recipient name; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Gift cards</returns>
        public virtual IPagedList<GiftCard> GetAllGiftCards(int? purchasedWithOrderId = null, int? usedWithOrderId = null,
            DateTime? createdFrom = null, DateTime? createdTo = null,
            bool? isGiftCardActivated = null, string giftCardCouponCode = null,
            string recipientName = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = context.GiftCards.AsQueryable();

            if (purchasedWithOrderId.HasValue)
            {
                query = from gc in query
                        join oi in context.OrderItems on gc.PurchasedWithOrderItemId equals oi.Id
                        where oi.OrderId == purchasedWithOrderId.Value
                        select gc;
            }

            if (usedWithOrderId.HasValue)
                query = from gc in query
                        join gcuh in context.GiftCardUsageHistories on gc.Id equals gcuh.GiftCardId
                        where gcuh.UsedWithOrderId == usedWithOrderId
                        select gc;

            if (createdFrom.HasValue)
                query = query.Where(gc => createdFrom.Value <= gc.CreatedOn);
            if (createdTo.HasValue)
                query = query.Where(gc => createdTo.Value >= gc.CreatedOn);
            if (isGiftCardActivated.HasValue)
                query = query.Where(gc => gc.IsGiftCardActivated == isGiftCardActivated.Value);
            if (!string.IsNullOrEmpty(giftCardCouponCode))
                query = query.Where(gc => gc.GiftCardCouponCode == giftCardCouponCode);
            if (!string.IsNullOrWhiteSpace(recipientName))
                query = query.Where(c => c.RecipientName.Contains(recipientName));
            query = query.OrderByDescending(gc => gc.CreatedOn);

            var giftCards = new PagedList<GiftCard>(query, pageIndex, pageSize);

            return giftCards;
        }

        /// <summary>
        /// Inserts a gift card
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        public virtual void InsertGiftCard(GiftCard giftCard)
        {
            if (giftCard == null)
                throw new ArgumentNullException(nameof(giftCard));

            Insert(giftCard);
        }

        /// <summary>
        /// Updates the gift card
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        public virtual void UpdateGiftCard(GiftCard giftCard)
        {
            if (giftCard == null)
                throw new ArgumentNullException(nameof(giftCard));

            Update(giftCard);
        }

        /// <summary>
        /// Gets gift cards by 'PurchasedWithOrderItemId'
        /// </summary>
        /// <param name="purchasedWithOrderItemId">Purchased with order item identifier</param>
        /// <returns>Gift card entries</returns>
        public virtual IList<GiftCard> GetGiftCardsByPurchasedWithOrderItemId(int purchasedWithOrderItemId)
        {
            if (purchasedWithOrderItemId == 0)
                return new List<GiftCard>();

            var query = context.GiftCards.AsQueryable();
            query = query.Where(gc => gc.PurchasedWithOrderItemId.HasValue && gc.PurchasedWithOrderItemId.Value == purchasedWithOrderItemId);
            query = query.OrderBy(gc => gc.Id);

            var giftCards = query.ToList();
            return giftCards;
        }

        /// <summary>
        /// Get active gift cards that are applied by a user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Active gift cards</returns>
        public virtual IList<GiftCard> GetActiveGiftCardsAppliedByUser(User user)
        {
            var result = new List<GiftCard>();
            if (user == null)
                return result;

            var couponCodes = _userService.ParseAppliedGiftCardCouponCodes(user);
            foreach (var couponCode in couponCodes)
            {
                var giftCards = GetAllGiftCards(isGiftCardActivated: true, giftCardCouponCode: couponCode);
                foreach (var gc in giftCards)
                {
                    if (IsGiftCardValid(gc))
                        result.Add(gc);
                }
            }

            return result;
        }

        /// <summary>
        /// Generate new gift card code
        /// </summary>
        /// <returns>Result</returns>
        public virtual string GenerateGiftCardCode()
        {
            var length = 13;
            var result = Guid.NewGuid().ToString();
            if (result.Length > length)
                result = result.Substring(0, length);
            return result;
        }

        /// <summary>
        /// Delete gift card usage history
        /// </summary>
        /// <param name="order">Order</param>
        public virtual void DeleteGiftCardUsageHistory(Order order)
        {
            var giftCardUsageHistory = GetGiftCardUsageHistory(order);

            //BulkDelete(giftCardUsageHistory);

            var query = context.GiftCards;

            var giftCardIds = giftCardUsageHistory.Select(gcuh => gcuh.GiftCardId).ToArray();
            var giftCards = query.Where(bp => giftCardIds.Contains(bp.Id)).ToList();

            //event notification
            foreach (var giftCard in giftCards)
            {
                _eventPublisher.EntityUpdated(giftCard);
            }
        }

        /// <summary>
        /// Gets a gift card remaining amount
        /// </summary>
        /// <returns>Gift card remaining amount</returns>
        public virtual decimal GetGiftCardRemainingAmount(GiftCard giftCard)
        {
            if (giftCard == null)
                throw new ArgumentNullException(nameof(giftCard));

            var result = giftCard.Amount;

            foreach (var gcuh in GetGiftCardUsageHistory(giftCard))
                result -= gcuh.UsedValue;

            if (result < decimal.Zero)
                result = decimal.Zero;

            return result;
        }

        /// <summary>
        /// Gets a gift card usage history entries
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <returns>Result</returns>
        public virtual IList<GiftCardUsageHistory> GetGiftCardUsageHistory(GiftCard giftCard)
        {
            if (giftCard is null)
                throw new ArgumentNullException(nameof(giftCard));

            return context.GiftCardUsageHistories.Where(gcuh => gcuh.GiftCardId == giftCard.Id).ToList();
        }

        /// <summary>
        /// Gets a gift card usage history entries
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public virtual IList<GiftCardUsageHistory> GetGiftCardUsageHistory(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            return context.GiftCardUsageHistories.Where(gcuh => gcuh.UsedWithOrderId == order.Id).ToList();
        }

        /// <summary>
        /// Inserts a gift card usage history entry
        /// </summary>
        /// <param name="giftCardUsageHistory">Gift card usage history entry</param>
        public virtual void InsertGiftCardUsageHistory(GiftCardUsageHistory giftCardUsageHistory)
        {
            if (giftCardUsageHistory is null)
                throw new ArgumentNullException(nameof(giftCardUsageHistory));

            _giftCardUsageHistoryRepository.Insert(giftCardUsageHistory);

            //event notification
            _eventPublisher.EntityInserted(giftCardUsageHistory);
        }

        /// <summary>
        /// Is gift card valid
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <returns>Result</returns>
        public virtual bool IsGiftCardValid(GiftCard giftCard)
        {
            if (giftCard == null)
                throw new ArgumentNullException(nameof(giftCard));

            if (!giftCard.IsGiftCardActivated)
                return false;

            var remainingAmount = GetGiftCardRemainingAmount(giftCard);
            if (remainingAmount > decimal.Zero)
                return true;

            return false;
        }

        #endregion
    }
}