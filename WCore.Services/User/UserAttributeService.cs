using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core.Domain.Users;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;

namespace WCore.Services.Users
{
    /// <summary>
    /// User attribute service
    /// </summary>
    public partial class UserAttributeService : IUserAttributeService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<UserAttribute> _userAttributeRepository;
        private readonly IRepository<UserAttributeValue> _userAttributeValueRepository;

        #endregion

        #region Ctor

        public UserAttributeService(ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            IRepository<UserAttribute> userAttributeRepository,
            IRepository<UserAttributeValue> userAttributeValueRepository)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _userAttributeRepository = userAttributeRepository;
            _userAttributeValueRepository = userAttributeValueRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        public virtual void DeleteUserAttribute(UserAttribute userAttribute)
        {
            if (userAttribute == null)
                throw new ArgumentNullException(nameof(userAttribute));

            _userAttributeRepository.Delete(userAttribute);

            //event notification
            _eventPublisher.EntityDeleted(userAttribute);
        }

        /// <summary>
        /// Gets all user attributes
        /// </summary>
        /// <returns>User attributes</returns>
        public virtual IList<UserAttribute> GetAllUserAttributes()
        {
            var query = from ca in _userAttributeRepository.GetAll().AsQueryable()
                orderby ca.DisplayOrder, ca.Id
                select ca;

            return query.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(WCoreUserServicesDefaults.UserAttributesAllCacheKey));
        }

        /// <summary>
        /// Gets a user attribute 
        /// </summary>
        /// <param name="userAttributeId">User attribute identifier</param>
        /// <returns>User attribute</returns>
        public virtual UserAttribute GetUserAttributeById(int userAttributeId)
        {
            if (userAttributeId == 0)
                return null;

            return _userAttributeRepository.ToCachedGetById(userAttributeId);
        }

        /// <summary>
        /// Inserts a user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        public virtual void InsertUserAttribute(UserAttribute userAttribute)
        {
            if (userAttribute == null)
                throw new ArgumentNullException(nameof(userAttribute));

            _userAttributeRepository.Insert(userAttribute);
            
            //event notification
            _eventPublisher.EntityInserted(userAttribute);
        }

        /// <summary>
        /// Updates the user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        public virtual void UpdateUserAttribute(UserAttribute userAttribute)
        {
            if (userAttribute == null)
                throw new ArgumentNullException(nameof(userAttribute));

            _userAttributeRepository.Update(userAttribute);

            //event notification
            _eventPublisher.EntityUpdated(userAttribute);
        }

        /// <summary>
        /// Deletes a user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        public virtual void DeleteUserAttributeValue(UserAttributeValue userAttributeValue)
        {
            if (userAttributeValue == null)
                throw new ArgumentNullException(nameof(userAttributeValue));

            _userAttributeValueRepository.Delete(userAttributeValue);

            //event notification
            _eventPublisher.EntityDeleted(userAttributeValue);
        }

        /// <summary>
        /// Gets user attribute values by user attribute identifier
        /// </summary>
        /// <param name="userAttributeId">The user attribute identifier</param>
        /// <returns>User attribute values</returns>
        public virtual IList<UserAttributeValue> GetUserAttributeValues(int userAttributeId)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreUserServicesDefaults.UserAttributeValuesAllCacheKey, userAttributeId);

            var query = from cav in _userAttributeValueRepository.GetAll().AsQueryable()
                        orderby cav.DisplayOrder, cav.Id
                where cav.UserAttributeId == userAttributeId
                select cav;
            var userAttributeValues = query.ToCachedList(key);

            return userAttributeValues;
        }

        /// <summary>
        /// Gets a user attribute value
        /// </summary>
        /// <param name="userAttributeValueId">User attribute value identifier</param>
        /// <returns>User attribute value</returns>
        public virtual UserAttributeValue GetUserAttributeValueById(int userAttributeValueId)
        {
            if (userAttributeValueId == 0)
                return null;

            return _userAttributeValueRepository.ToCachedGetById(userAttributeValueId);
        }

        /// <summary>
        /// Inserts a user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        public virtual void InsertUserAttributeValue(UserAttributeValue userAttributeValue)
        {
            if (userAttributeValue == null)
                throw new ArgumentNullException(nameof(userAttributeValue));

            _userAttributeValueRepository.Insert(userAttributeValue);

            //event notification
            _eventPublisher.EntityInserted(userAttributeValue);
        }

        /// <summary>
        /// Updates the user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        public virtual void UpdateUserAttributeValue(UserAttributeValue userAttributeValue)
        {
            if (userAttributeValue == null)
                throw new ArgumentNullException(nameof(userAttributeValue));

            _userAttributeValueRepository.Update(userAttributeValue);

            //event notification
            _eventPublisher.EntityUpdated(userAttributeValue);
        }

        #endregion
    }
}