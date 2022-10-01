using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Users;
using WCore.Services.Caching.Extensions;
using WCore.Services.Common;
using WCore.Services.Events;
using WCore.Services.Messages;

namespace WCore.Services.Catalog
{
    /// <summary>
    /// Back in stock subscription service
    /// </summary>
    public partial class BackInStockSubscriptionService : Repository<BackInStockSubscription>, IBackInStockSubscriptionService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IRepository<BackInStockSubscription> _backInStockSubscriptionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IWorkflowMessageService _workflowMessageService;

        #endregion

        #region Ctor

        public BackInStockSubscriptionService(WCoreContext context, IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            IRepository<BackInStockSubscription> backInStockSubscriptionRepository,
            IRepository<User> userRepository,
            IRepository<Product> productRepository,
            IWorkflowMessageService workflowMessageService) : base(context)
        {
            _eventPublisher = eventPublisher;
            _genericAttributeService = genericAttributeService;
            _backInStockSubscriptionRepository = backInStockSubscriptionRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a back in stock subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual void DeleteSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            _backInStockSubscriptionRepository.Delete(subscription);

            //event notification
            _eventPublisher.EntityDeleted(subscription);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Subscriptions</returns>
        public virtual IPagedList<BackInStockSubscription> GetAllSubscriptionsByUserId(int userId,
            int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            IQueryable<BackInStockSubscription> recordsFiltered = context.Set<BackInStockSubscription>();

            //user
            recordsFiltered = recordsFiltered.Where(biss => biss.UserId == userId);

            //store
            if (storeId > 0)
                recordsFiltered = recordsFiltered.Where(biss => biss.StoreId == storeId);

            //product
            recordsFiltered = from q in recordsFiltered
                              join p in context.Products on q.ProductId equals p.Id
                              where !p.Deleted
                              select q;

            recordsFiltered = recordsFiltered.OrderByDescending(biss => biss.CreatedOn);

            return new PagedList<BackInStockSubscription>(recordsFiltered, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Subscriptions</returns>
        public virtual IPagedList<BackInStockSubscription> GetAllSubscriptionsByProductId(int productId,
            int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            IQueryable<BackInStockSubscription> recordsFiltered = context.Set<BackInStockSubscription>();

            //product
            recordsFiltered = recordsFiltered.Where(biss => biss.ProductId == productId);
            //store
            if (storeId > 0)
                recordsFiltered = recordsFiltered.Where(biss => biss.StoreId == storeId);
            //user
            recordsFiltered = from biss in recordsFiltered
                              join c in context.Users on biss.UserId equals c.Id
                              where c.Active && !c.Deleted
                              select biss;

            recordsFiltered = recordsFiltered.OrderByDescending(biss => biss.CreatedOn);
            return new PagedList<BackInStockSubscription>(recordsFiltered, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Subscriptions</returns>
        public virtual BackInStockSubscription FindSubscription(int userId, int productId, int storeId)
        {
            var query = from biss in context.Set<BackInStockSubscription>()
                        orderby biss.CreatedOn descending
                        where biss.UserId == userId &&
                              biss.ProductId == productId &&
                              biss.StoreId == storeId
                        select biss;

            var subscription = query.FirstOrDefault();
            return subscription;
        }

        /// <summary>
        /// Gets a subscription
        /// </summary>
        /// <param name="subscriptionId">Subscription identifier</param>
        /// <returns>Subscription</returns>
        public virtual BackInStockSubscription GetSubscriptionById(int subscriptionId)
        {
            if (subscriptionId == 0)
                return null;

            var subscription = _backInStockSubscriptionRepository.ToCachedGetById(subscriptionId);
            return subscription;
        }

        /// <summary>
        /// Inserts subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual void InsertSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            _backInStockSubscriptionRepository.Insert(subscription);

            //event notification
            _eventPublisher.EntityInserted(subscription);
        }

        /// <summary>
        /// Updates subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual void UpdateSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            _backInStockSubscriptionRepository.Update(subscription);

            //event notification
            _eventPublisher.EntityUpdated(subscription);
        }

        /// <summary>
        /// Send notification to subscribers
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Number of sent email</returns>
        public virtual int SendNotificationsToSubscribers(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var result = 0;
            var subscriptions = GetAllSubscriptionsByProductId(product.Id);
            foreach (var subscription in subscriptions)
            {
                var userLanguageId = _genericAttributeService.GetAttribute<User, int>(subscription.UserId, WCoreUserDefaults.LanguageIdAttribute, subscription.StoreId);

                result += _workflowMessageService.SendBackInStockNotification(subscription, userLanguageId).Count;
            }

            for (var i = 0; i <= subscriptions.Count - 1; i++)
                DeleteSubscription(subscriptions[i]);

            return result;
        }

        #endregion
    }
}