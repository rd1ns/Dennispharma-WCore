using System;
using System.Collections.Generic;
using WCore.Core;
using WCore.Core.Domain.Users;
using WCore.Core.Domain.Orders;

namespace WCore.Services.Orders
{
    /// <summary>
    /// Gift card service interface
    /// </summary>
    public partial interface IGiftCardService : IRepository<GiftCard>
    {
        /// <summary>
        /// Deletes a gift card
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        void DeleteGiftCard(GiftCard giftCard);

        /// <summary>
        /// Gets a gift card
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <returns>Gift card entry</returns>
        GiftCard GetGiftCardById(int giftCardId);

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
        IPagedList<GiftCard> GetAllGiftCards(int? purchasedWithOrderId = null, int? usedWithOrderId = null,
            DateTime? createdFrom = null, DateTime? createdTo = null,
            bool? isGiftCardActivated = null, string giftCardCouponCode = null,
            string recipientName = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Inserts a gift card
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        void InsertGiftCard(GiftCard giftCard);

        /// <summary>
        /// Updates the gift card
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        void UpdateGiftCard(GiftCard giftCard);

        /// <summary>
        /// Gets gift cards by 'PurchasedWithOrderItemId'
        /// </summary>
        /// <param name="purchasedWithOrderItemId">Purchased with order item identifier</param>
        /// <returns>Gift card entries</returns>
        IList<GiftCard> GetGiftCardsByPurchasedWithOrderItemId(int purchasedWithOrderItemId);

        /// <summary>
        /// Get active gift cards that are applied by a user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Active gift cards</returns>
        IList<GiftCard> GetActiveGiftCardsAppliedByUser(User user);

        /// <summary>
        /// Generate new gift card code
        /// </summary>
        /// <returns>Result</returns>
        string GenerateGiftCardCode();

        /// <summary>
        /// Delete gift card usage history
        /// </summary>
        /// <param name="order">Order</param>
        void DeleteGiftCardUsageHistory(Order order);

        /// <summary>
        /// Gets a gift card remaining amount
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <returns>Gift card remaining amount</returns>
        decimal GetGiftCardRemainingAmount(GiftCard giftCard);

        /// <summary>
        /// Gets a gift card usage history entries
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <returns>Result</returns>
        IList<GiftCardUsageHistory> GetGiftCardUsageHistory(GiftCard giftCard);

        /// <summary>
        /// Gets a gift card usage history entries
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        IList<GiftCardUsageHistory> GetGiftCardUsageHistory(Order order);

        /// <summary>
        /// Inserts a gift card usage history entry
        /// </summary>
        /// <param name="giftCardUsageHistory">Gift card usage history entry</param>
        void InsertGiftCardUsageHistory(GiftCardUsageHistory giftCardUsageHistory);

        /// <summary>
        /// Is gift card valid
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <returns>Result</returns>
        bool IsGiftCardValid(GiftCard giftCard);
    }
}