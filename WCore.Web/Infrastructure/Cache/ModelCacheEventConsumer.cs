using WCore.Core.Caching;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Logging;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Roles;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Tasks;
using WCore.Core.Domain.Templates;
using WCore.Core.Domain.Users;
using WCore.Core.Events;
using WCore.Services.Common;
using WCore.Services.Events;
using WCore.Services.Localization;
using WCore.Services.Logging;
using WCore.Services.Menus;
using WCore.Services.Roles;
using WCore.Services.Tasks;
using WCore.Services.Users;

namespace WCore.Web.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
    // City 
    IConsumer<EntityInsertedEvent<City>>, IConsumer<EntityUpdatedEvent<City>>, IConsumer<EntityDeletedEvent<City>>,
    // Country 
    IConsumer<EntityInsertedEvent<Country>>, IConsumer<EntityUpdatedEvent<Country>>, IConsumer<EntityDeletedEvent<Country>>,
    // Log 
    IConsumer<EntityInsertedEvent<Log>>, IConsumer<EntityUpdatedEvent<Log>>, IConsumer<EntityDeletedEvent<Log>>,
    // Menu
    IConsumer<EntityInsertedEvent<Menu>>, IConsumer<EntityUpdatedEvent<Menu>>, IConsumer<EntityDeletedEvent<Menu>>,
    // Page 
    IConsumer<EntityInsertedEvent<Page>>, IConsumer<EntityUpdatedEvent<Page>>, IConsumer<EntityDeletedEvent<Page>>,
    // LocaleStringResource 
    IConsumer<EntityInsertedEvent<LocaleStringResource>>, IConsumer<EntityUpdatedEvent<LocaleStringResource>>, IConsumer<EntityDeletedEvent<LocaleStringResource>>,
    // Role
    IConsumer<EntityInsertedEvent<Role>>, IConsumer<EntityUpdatedEvent<Role>>, IConsumer<EntityDeletedEvent<Role>>,
    // RoleGroup
    IConsumer<EntityInsertedEvent<RoleGroup>>, IConsumer<EntityUpdatedEvent<RoleGroup>>, IConsumer<EntityDeletedEvent<RoleGroup>>,
    // ScheduleTask 
    IConsumer<EntityInsertedEvent<ScheduleTask>>, IConsumer<EntityUpdatedEvent<ScheduleTask>>, IConsumer<EntityDeletedEvent<ScheduleTask>>,
    IConsumer<EntityInsertedEvent<Template>>, IConsumer<EntityUpdatedEvent<Template>>, IConsumer<EntityDeletedEvent<Template>>,
    // TempRole
    IConsumer<EntityInsertedEvent<TempRole>>, IConsumer<EntityUpdatedEvent<TempRole>>, IConsumer<EntityDeletedEvent<TempRole>>,
    // TempRoleGroup
    IConsumer<EntityInsertedEvent<TempRoleGroup>>, IConsumer<EntityUpdatedEvent<TempRoleGroup>>, IConsumer<EntityDeletedEvent<TempRoleGroup>>,
    // User 
    IConsumer<EntityInsertedEvent<User>>, IConsumer<EntityUpdatedEvent<User>>, IConsumer<EntityDeletedEvent<User>>,
    // UserRole
    IConsumer<EntityInsertedEvent<UserRole>>, IConsumer<EntityUpdatedEvent<UserRole>>, IConsumer<EntityDeletedEvent<UserRole>>
    {
        #region Fields
        private readonly IStaticCacheManager _staticCacheManager;
        #endregion

        #region Ctor
        public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }
        #endregion

        #region Users 
        public void HandleEvent(EntityInsertedEvent<User> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreUsersDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<User> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreUsersDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<User> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreUsersDefaults.AllByFiltersPrefix); }
        #endregion

        #region UserRoles 
        public void HandleEvent(EntityInsertedEvent<UserRole> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreUserRolesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<UserRole> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreUserRolesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<UserRole> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreUserRolesDefaults.AllByFiltersPrefix); }
        #endregion

        #region Roles 
        public void HandleEvent(EntityInsertedEvent<Role> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreRolesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<Role> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreRolesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<Role> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreRolesDefaults.AllByFiltersPrefix); }
        #endregion

        #region Menus 
        public void HandleEvent(EntityInsertedEvent<Menu> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreMenusDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<Menu> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreMenusDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<Menu> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreMenusDefaults.AllByFiltersPrefix); }
        #endregion

        #region TempRoles 
        public void HandleEvent(EntityInsertedEvent<TempRole> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTempRolesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<TempRole> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTempRolesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<TempRole> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTempRolesDefaults.AllByFiltersPrefix); }
        #endregion

        #region RoleGroups 
        public void HandleEvent(EntityInsertedEvent<RoleGroup> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreRoleGroupsDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<RoleGroup> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreRoleGroupsDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<RoleGroup> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreRoleGroupsDefaults.AllByFiltersPrefix); }
        #endregion

        #region TempRoleGroups 
        public void HandleEvent(EntityInsertedEvent<TempRoleGroup> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTempRoleGroupsDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<TempRoleGroup> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTempRoleGroupsDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<TempRoleGroup> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTempRoleGroupsDefaults.AllByFiltersPrefix); }
        #endregion

        #region Countries 
        public void HandleEvent(EntityInsertedEvent<Country> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreCountriesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<Country> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreCountriesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<Country> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreCountriesDefaults.AllByFiltersPrefix); }
        #endregion

        #region Cities 
        public void HandleEvent(EntityInsertedEvent<City> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreCitiesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<City> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreCitiesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<City> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreCitiesDefaults.AllByFiltersPrefix); }
        #endregion

        #region Pages 
        public void HandleEvent(EntityInsertedEvent<Page> eventMessage) {
            _staticCacheManager.RemoveByPrefix(WCorePagesDefaults.AllByFiltersPrefix);
        }
        public void HandleEvent(EntityUpdatedEvent<Page> eventMessage) {
            _staticCacheManager.RemoveByPrefix(WCorePagesDefaults.AllByFiltersPrefix);
        }
        public void HandleEvent(EntityDeletedEvent<Page> eventMessage) {
            _staticCacheManager.RemoveByPrefix(WCorePagesDefaults.AllByFiltersPrefix);
        }
        #endregion

        #region LocaleStringResources 
        public void HandleEvent(EntityInsertedEvent<LocaleStringResource> eventMessage) {
            _staticCacheManager.RemoveByPrefix(WCoreLocaleStringResourceDefaults.AllByFiltersPrefix);
        }
        public void HandleEvent(EntityUpdatedEvent<LocaleStringResource> eventMessage) {
            _staticCacheManager.RemoveByPrefix(WCoreLocaleStringResourceDefaults.AllByFiltersPrefix);
        }
        public void HandleEvent(EntityDeletedEvent<LocaleStringResource> eventMessage) {
            _staticCacheManager.RemoveByPrefix(WCoreLocaleStringResourceDefaults.AllByFiltersPrefix);
        }
        #endregion

        #region Settings 
        public void HandleEvent(EntityInsertedEvent<Setting> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreSettingsDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<Setting> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreSettingsDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<Setting> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreSettingsDefaults.AllByFiltersPrefix); }
        #endregion

        #region Logs 
        public void HandleEvent(EntityInsertedEvent<Log> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreLoggingDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<Log> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreLoggingDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<Log> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreLoggingDefaults.AllByFiltersPrefix); }
        #endregion

        #region ScheduleTasks 
        public void HandleEvent(EntityInsertedEvent<ScheduleTask> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTaskDefaults.ScheduleTaskPath); }
        public void HandleEvent(EntityUpdatedEvent<ScheduleTask> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTaskDefaults.ScheduleTaskPath); }
        public void HandleEvent(EntityDeletedEvent<ScheduleTask> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTaskDefaults.ScheduleTaskPath); }
        #endregion

        #region Templates 
        public void HandleEvent(EntityInsertedEvent<Template> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTemplatesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityUpdatedEvent<Template> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTemplatesDefaults.AllByFiltersPrefix); }
        public void HandleEvent(EntityDeletedEvent<Template> eventMessage) { _staticCacheManager.RemoveByPrefix(WCoreTemplatesDefaults.AllByFiltersPrefix); }
        #endregion
    }
}
