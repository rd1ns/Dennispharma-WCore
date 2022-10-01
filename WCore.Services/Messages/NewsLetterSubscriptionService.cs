using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Messages;
using WCore.Core.Domain.Users;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;
using WCore.Services.Users;

namespace WCore.Services.Messages
{
    /// <summary>
    /// Newsletter subscription service
    /// </summary>
    public class NewsLetterSubscriptionService : Repository<NewsLetterSubscription>, INewsLetterSubscriptionService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<NewsLetterSubscription> _subscriptionRepository;

        #endregion

        #region Ctor

        public NewsLetterSubscriptionService(WCoreContext context, IUserService userService,
            IEventPublisher eventPublisher,
            IRepository<User> userRepository,
            IRepository<NewsLetterSubscription> subscriptionRepository) : base(context)
        {
            _userService = userService;
            _eventPublisher = eventPublisher;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Publishes the subscription event.
        /// </summary>
        /// <param name="subscription">The newsletter subscription.</param>
        /// <param name="isSubscribe">if set to <c>true</c> [is subscribe].</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        private void PublishSubscriptionEvent(NewsLetterSubscription subscription, bool isSubscribe, bool publishSubscriptionEvents)
        {
            if (!publishSubscriptionEvents)
                return;

            if (isSubscribe)
            {
                _eventPublisher.PublishNewsletterSubscribe(subscription);
            }
            else
            {
                _eventPublisher.PublishNewsletterUnsubscribe(subscription);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts a newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscription">NewsLetter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void InsertNewsLetterSubscription(NewsLetterSubscription newsLetterSubscription, bool publishSubscriptionEvents = true)
        {
            if (newsLetterSubscription == null)
            {
                throw new ArgumentNullException(nameof(newsLetterSubscription));
            }

            //Handle e-mail
            newsLetterSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(newsLetterSubscription.Email);

            //Persist
            _subscriptionRepository.Insert(newsLetterSubscription);

            //Publish the subscription event 
            if (newsLetterSubscription.Active)
            {
                PublishSubscriptionEvent(newsLetterSubscription, true, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityInserted(newsLetterSubscription);
        }

        /// <summary>
        /// Updates a newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscription">NewsLetter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void UpdateNewsLetterSubscription(NewsLetterSubscription newsLetterSubscription, bool publishSubscriptionEvents = true)
        {
            if (newsLetterSubscription == null)
            {
                throw new ArgumentNullException(nameof(newsLetterSubscription));
            }

            //Handle e-mail
            newsLetterSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(newsLetterSubscription.Email);

            //Get original subscription record
            var originalSubscription = _subscriptionRepository.GetById(newsLetterSubscription);

            //Persist
            _subscriptionRepository.Update(newsLetterSubscription);

            //Publish the subscription event 
            if ((originalSubscription.Active == false && newsLetterSubscription.Active) ||
                (newsLetterSubscription.Active && originalSubscription.Email != newsLetterSubscription.Email))
            {
                //If the previous entry was false, but this one is true, publish a subscribe.
                PublishSubscriptionEvent(newsLetterSubscription, true, publishSubscriptionEvents);
            }

            if (originalSubscription.Active && newsLetterSubscription.Active &&
                originalSubscription.Email != newsLetterSubscription.Email)
            {
                //If the two emails are different publish an unsubscribe.
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            if (originalSubscription.Active && !newsLetterSubscription.Active)
            {
                //If the previous entry was true, but this one is false
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityUpdated(newsLetterSubscription);
        }

        /// <summary>
        /// Deletes a newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscription">NewsLetter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void DeleteNewsLetterSubscription(NewsLetterSubscription newsLetterSubscription, bool publishSubscriptionEvents = true)
        {
            if (newsLetterSubscription == null) throw new ArgumentNullException(nameof(newsLetterSubscription));

            _subscriptionRepository.Delete(newsLetterSubscription);

            //Publish the unsubscribe event 
            PublishSubscriptionEvent(newsLetterSubscription, false, publishSubscriptionEvents);

            //event notification
            _eventPublisher.EntityDeleted(newsLetterSubscription);
        }

        /// <summary>
        /// Gets a newsletter subscription by newsletter subscription identifier
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        /// <returns>NewsLetter subscription</returns>
        public virtual NewsLetterSubscription GetNewsLetterSubscriptionById(int newsLetterSubscriptionId)
        {
            if (newsLetterSubscriptionId == 0) return null;

            return _subscriptionRepository.ToCachedGetById(newsLetterSubscriptionId);
        }

        /// <summary>
        /// Gets a newsletter subscription by newsletter subscription GUID
        /// </summary>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <returns>NewsLetter subscription</returns>
        public virtual NewsLetterSubscription GetNewsLetterSubscriptionByGuid(Guid newsLetterSubscriptionGuid)
        {
            if (newsLetterSubscriptionGuid == Guid.Empty) return null;

            var newsLetterSubscriptions = from nls in context.NewsLetterSubscriptions
                                          where nls.NewsLetterSubscriptionGuid == newsLetterSubscriptionGuid
                                          orderby nls.Id
                                          select nls;

            return newsLetterSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets a newsletter subscription by email and store ID
        /// </summary>
        /// <param name="email">The newsletter subscription email</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>NewsLetter subscription</returns>
        public virtual NewsLetterSubscription GetNewsLetterSubscriptionByEmailAndStoreId(string email, int storeId)
        {
            if (!CommonHelper.IsValidEmail(email))
                return null;

            email = email.Trim();

            var newsLetterSubscriptions = from nls in context.NewsLetterSubscriptions
                                          where nls.Email == email && nls.StoreId == storeId
                                          orderby nls.Id
                                          select nls;

            return newsLetterSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets the newsletter subscription list
        /// </summary>
        /// <param name="email">Email to search or string. Empty to load all records.</param>
        /// <param name="createdFrom">Created date from (); null to load all records</param>
        /// <param name="createdTo">Created date to (); null to load all records</param>
        /// <param name="storeId">Store identifier. 0 to load all records.</param>
        /// <param name="userRoleId">User role identifier. Used to filter subscribers by user role. 0 to load all records.</param>
        /// <param name="isActive">Value indicating whether subscriber record should be active or not; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>NewsLetterSubscription entities</returns>
        public virtual IPagedList<NewsLetterSubscription> GetAllNewsLetterSubscriptions(string email = null,
            DateTime? createdFrom = null, DateTime? createdTo = null,
            int storeId = 0, bool? isActive = null, int userRoleId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //if (userRoleId == 0)
            //{
                //do not filter by user role
                var query = context.NewsLetterSubscriptions.AsQueryable();
                if (!string.IsNullOrEmpty(email))
                    query = query.Where(nls => nls.Email.Contains(email));
                if (createdFrom.HasValue)
                    query = query.Where(nls => nls.CreatedOn >= createdFrom.Value);
                if (createdTo.HasValue)
                    query = query.Where(nls => nls.CreatedOn <= createdTo.Value);
                if (storeId > 0)
                    query = query.Where(nls => nls.StoreId == storeId);
                if (isActive.HasValue)
                    query = query.Where(nls => nls.Active == isActive.Value);
                query = query.OrderBy(nls => nls.Email);

                var subscriptions = new PagedList<NewsLetterSubscription>(query, pageIndex, pageSize);
                return subscriptions;
            //}

            //filter by user role
            //var guestRole = _userService.GetUserRoleBySystemName(WCoreUserDefaults.GuestsRoleName);
            //if (guestRole == null)
            //    throw new WCoreException("'Guests' role could not be loaded");

            //if (guestRole.Id == userRoleId)
            //{
            //    //guests
            //    var query = context.NewsLetterSubscriptions.AsQueryable();
            //    if (!string.IsNullOrEmpty(email))
            //        query = query.Where(nls => nls.Email.Contains(email));
            //    if (createdFrom.HasValue)
            //        query = query.Where(nls => nls.CreatedOn >= createdFrom.Value);
            //    if (createdTo.HasValue)
            //        query = query.Where(nls => nls.CreatedOn <= createdTo.Value);
            //    if (storeId > 0)
            //        query = query.Where(nls => nls.StoreId == storeId);
            //    if (isActive.HasValue)
            //        query = query.Where(nls => nls.Active == isActive.Value);
            //    //query = query.Where(nls => !_userRepository.GetAll().Any(c => c.Email == nls.Email));
            //    query = query.OrderBy(nls => nls.Email);

            //    var subscriptions = new PagedList<NewsLetterSubscription>(query, pageIndex, pageSize);

            //    return subscriptions;
            //}
            //else
            //{
                //other user roles (not guests)
                //var query = context.NewsLetterSubscriptions.Join(_userRepository.GetAll(),
                //    nls => nls.Email,
                //    c => c.Email,
                //    (nls, c) => new
                //    {
                //        NewsletterSubscribers = nls,
                //        User = c
                //    });

                ////query = query.Where(x => _userUserRoleMappingRepository.Table.Any(ccrm => ccrm.UserId == x.User.Id && ccrm.UserRoleId == userRoleId));

                //if (!string.IsNullOrEmpty(email))
                //    query = query.Where(x => x.NewsletterSubscribers.Email.Contains(email));
                //if (createdFrom.HasValue)
                //    query = query.Where(x => x.NewsletterSubscribers.CreatedOn >= createdFrom.Value);
                //if (createdTo.HasValue)
                //    query = query.Where(x => x.NewsletterSubscribers.CreatedOn <= createdTo.Value);
                //if (storeId > 0)
                //    query = query.Where(x => x.NewsletterSubscribers.StoreId == storeId);
                //if (isActive.HasValue)
                //    query = query.Where(x => x.NewsletterSubscribers.Active == isActive.Value);

                //query = query.OrderBy(x => x.NewsletterSubscribers.Email);

                //var subscriptions = new PagedList<NewsLetterSubscription>(query.Select(x => x.NewsletterSubscribers), pageIndex, pageSize);

                //return subscriptions;
            //}
        }

        #endregion
    }
}