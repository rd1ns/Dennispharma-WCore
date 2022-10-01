using System;
using System.Collections.Generic;
using WCore.Core;
using WCore.Core.Domain.Users;
using WCore.Core.Domain.Logging;
using WCore.Core.Domain;

namespace WCore.Services.Logging
{
    /// <summary>
    /// User activity service interface
    /// </summary>
    public partial interface IActivityLogService : IRepository<ActivityLog>
    {
        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">System keyword</param>
        /// <param name="comment">Comment</param>
        /// <param name="entity">Entity</param>
        /// <returns>Activity log item</returns>
        ActivityLog InsertActivity(string systemKeyword, string comment, BaseEntity entity = null);

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="systemKeyword">System keyword</param>
        /// <param name="comment">Comment</param>
        /// <param name="entity">Entity</param>
        /// <returns>Activity log item</returns>
        ActivityLog InsertActivity(User user, string systemKeyword, string comment, BaseEntity entity = null);

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
        IPagedList<ActivityLog> GetAllActivities(DateTime? createdOnFrom = null, DateTime? createdOnTo = null,
            int? userId = null, int? activityLogTypeId = null, string ipAddress = null, string entityName = null, int? entityId = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Clears activity log
        /// </summary>
        void ClearAllActivities();
    }
    /// <summary>
    /// User activity service interface
    /// </summary>
    public partial interface IActivityLogTypeService : IRepository<ActivityLogType>
    {
        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type items</returns>
        IList<ActivityLogType> GetAllActivityTypes();
    }
}
