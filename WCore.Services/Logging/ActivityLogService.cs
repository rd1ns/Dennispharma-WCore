using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain;
using WCore.Core.Domain.Logging;
using WCore.Core.Domain.Users;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;

namespace WCore.Services.Logging
{
    /// <summary>
    /// User activity service
    /// </summary>
    public class ActivityLogService : Repository<ActivityLog>, IActivityLogService
    {
        #region Fields

        private readonly IActivityLogTypeService _activityLogTypeService;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ActivityLogService(WCoreContext context,
            IActivityLogTypeService activityLogTypeService,
            ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            IWebHelper webHelper,
            IWorkContext workContext) : base(context)
        {
            _activityLogTypeService = activityLogTypeService;
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _webHelper = webHelper;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">System keyword</param>
        /// <param name="comment">Comment</param>
        /// <param name="entity">Entity</param>
        /// <returns>Activity log item</returns>
        public virtual ActivityLog InsertActivity(string systemKeyword, string comment, BaseEntity entity = null)
        {
            return InsertActivity(_workContext.CurrentUser, systemKeyword, comment, entity);
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="systemKeyword">System keyword</param>
        /// <param name="comment">Comment</param>
        /// <param name="entity">Entity</param>
        /// <returns>Activity log item</returns>
        public virtual ActivityLog InsertActivity(User user, string systemKeyword, string comment, BaseEntity entity = null)
        {
            if (user == null)
                return null;

            //try to get activity log type by passed system keyword
            var activityLogType = _activityLogTypeService.GetAllActivityTypes().FirstOrDefault(type => type.SystemKeyword.Equals(systemKeyword));
            if (!activityLogType?.Enabled ?? true)
                return null;

            //insert log item
            var logItem = new ActivityLog
            {
                ActivityLogTypeId = activityLogType.Id,
                EntityId = entity?.Id,
                EntityName = entity?.GetType().Name,
                UserId = user.Id,
                Comment = CommonHelper.EnsureMaximumLength(comment ?? string.Empty, 4000),
                CreatedOn = DateTime.Now,
                IpAddress = _webHelper.GetCurrentIpAddress()
            };
            Insert(logItem);

            //event notification
            _eventPublisher.EntityInserted(logItem);

            return logItem;
        }

        /// <summary>
        /// Gets all activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; pass null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; pass null to load all records</param>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="activityLogTypeId">Activity log type identifier; pass null to load all records</param>
        /// <param name="ipAddress">IP address; pass null or empty to load all records</param>
        /// <param name="entityName">Entity name; pass null to load all records</param>
        /// <param name="entityId">Entity identifier; pass null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        public virtual IPagedList<ActivityLog> GetAllActivities(DateTime? createdOnFrom = null, DateTime? createdOnTo = null,
            int? userId = null, int? activityLogTypeId = null, string ipAddress = null, string entityName = null, int? entityId = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            IQueryable<ActivityLog> query = context.Set<ActivityLog>();

            //filter by IP
            if (!string.IsNullOrEmpty(ipAddress))
                query = query.Where(logItem => logItem.IpAddress.Contains(ipAddress));

            //filter by creation date
            if (createdOnFrom.HasValue)
                query = query.Where(logItem => createdOnFrom.Value <= logItem.CreatedOn);
            if (createdOnTo.HasValue)
                query = query.Where(logItem => createdOnTo.Value >= logItem.CreatedOn);

            //filter by log type
            if (activityLogTypeId.HasValue && activityLogTypeId.Value > 0)
                query = query.Where(logItem => activityLogTypeId == logItem.ActivityLogTypeId);

            //filter by user
            if (userId.HasValue && userId.Value > 0)
                query = query.Where(logItem => userId.Value == logItem.UserId);

            //filter by entity
            if (!string.IsNullOrEmpty(entityName))
                query = query.Where(logItem => logItem.EntityName.Equals(entityName));
            if (entityId.HasValue && entityId.Value > 0)
                query = query.Where(logItem => entityId.Value == logItem.EntityId);

            query = query.OrderByDescending(logItem => logItem.CreatedOn).ThenBy(logItem => logItem.Id);

            return new PagedList<ActivityLog>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Clears activity log
        /// </summary>
        public virtual void ClearAllActivities()
        {
            
        }

        #endregion
    }
}
namespace WCore.Services.Logging
{
    /// <summary>
    /// User activity service
    /// </summary>
    public class ActivityLogTypeService : Repository<ActivityLogType>, IActivityLogTypeService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ActivityLogTypeService(WCoreContext context,
            ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            IWebHelper webHelper,
            IWorkContext workContext) : base(context)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _webHelper = webHelper;
            _workContext = workContext;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type items</returns>
        public virtual IList<ActivityLogType> GetAllActivityTypes()
        {
            var query = from alt in context.Set<ActivityLogType>()
                        orderby alt.Name
                        select alt;
            var activityLogTypes = query.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(WCoreLoggingDefaults.ActivityTypeAllCacheKey));

            return activityLogTypes;
        }

        #endregion
    }
}