using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Tax;
using WCore.Core.Domain.Users;
using WCore.Services.Caching;
using WCore.Services.Common;
using WCore.Services.Events;
using WCore.Services.Roles;

namespace WCore.Services.Users
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly IRoleGroupService _roleGroupService;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IRepository<UserPassword> _userPasswordRepository;
        private readonly IRepository<UserAddress> _userAddressRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly UserSettings _userSettings;
        public UserService(WCoreContext context,
            IRoleGroupService roleGroupService,
            ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            IStaticCacheManager staticCacheManager,
            IGenericAttributeService genericAttributeService,
            IRepository<UserPassword> userPasswordRepository,
            IRepository<UserAddress> userAddressRepository,
            IRepository<Address> addressRepository,
            UserSettings userSettings) : base(context)
        {
            this._roleGroupService = roleGroupService;
            this._genericAttributeService = genericAttributeService;
            this._cacheKeyService = cacheKeyService;
            this._eventPublisher = eventPublisher;
            this._staticCacheManager = staticCacheManager;

            this._userPasswordRepository = userPasswordRepository;
            this._userAddressRepository = userAddressRepository;
            this._addressRepository = addressRepository;

            this._userSettings = userSettings;
        }

        #region Users
        public IPagedList<User> GetAllByFilters(string searchValue = "", UserType? userType = null, int? userAgencyId = null, bool? IsAgencyManager = null, bool? IsAgencyStaff = null, bool? IsActive = null, bool? Deleted = null, int skip = 0, int take = int.MaxValue)
        {
            IQueryable<User> recordsFiltered = context.Set<User>();

            if (IsActive.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Active == IsActive);

            if (Deleted.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted);

            if (Deleted.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted);

            if (userType.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.UserType == userType);

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<User>().Count();

            var data = recordsFiltered.OrderByDescending(o => o.Active).OrderByDescending(o => o.UserType).ThenByDescending(o => o.CreatedOn).Skip(skip).Take(take).ToList();

            return new PagedList<User>(data, skip, take, recordsFilteredCount);
        }


        public virtual User GetUserByEmail(string Email)
        {
            var user = context.Set<User>().AsNoTracking().FirstOrDefault(o => o.Email == Email);
            if (user != null)
            {
                if (user.RoleGroupId != 0)
                    user.RoleGroup = _roleGroupService.GetById(user.RoleGroupId, cache => default);
            }

            return user;
        }
        public virtual User GetUserByGuid(Guid guid)
        {
            var user = context.Set<User>().AsNoTracking().FirstOrDefault(o => o.UserGuid == guid);
            if (user != null)
            {
                if (user.RoleGroupId != 0)
                    user.RoleGroup = _roleGroupService.GetById(user.RoleGroupId, cache => default);
            }

            return user;
        }
        public virtual User GetUserByUsername(string Username)
        {
            var user = context.Set<User>().AsNoTracking().FirstOrDefault(o => o.Username == Username);
            if (user != null)
            {
                if (user.RoleGroupId != 0)
                    user.RoleGroup = _roleGroupService.GetById(user.RoleGroupId, cache => default);
            }

            return user;
        }
        public virtual User GetUserByType(UserType UserType)
        {
            var user = context.Set<User>().AsNoTracking().FirstOrDefault(o => o.UserType == UserType);

            if (user != null)
            {
                if (user.RoleGroupId != 0)
                    user.RoleGroup = _roleGroupService.GetById(user.RoleGroupId, cache => default);
            }

            return user;
        }
        public virtual User GetUserById(int id)
        {
            var user = context.Set<User>().AsNoTracking().FirstOrDefault(o => o.Id == id);

            if (user != null)
            {
                if (user.RoleGroupId != 0)
                    user.RoleGroup = _roleGroupService.GetById(user.RoleGroupId, cache => default);
            }

            return user;

        }
        public virtual User CheckUser(string Email, string Password)
        {
            var user = context.Set<User>().AsNoTracking().FirstOrDefault(o => o.Email == Email);

            return user;
        }
        public virtual User GetOrCreateSearchEngineUser()
        {
            var searchEngineUser = GetUserByType(UserType.SearchEngine);

            if (searchEngineUser is null)
            {
                //If for any reason the system user isn't in the database, then we add it
                searchEngineUser = new User
                {
                    Email = "builtin@search_engine_record.com",
                    Active = true,
                    SystemName = WCoreUserDefaults.SearchEngineUserName,
                    UserType = UserType.SearchEngine,
                    CreatedOn = DateTime.Now
                };

                Insert(searchEngineUser);
            }

            return searchEngineUser;
        }
        public virtual User GetOrCreateBackgroundTaskUser()
        {
            var backgroundTaskUser = GetUserByType(UserType.Task);

            if (backgroundTaskUser is null)
            {
                //If for any reason the system user isn't in the database, then we add it
                backgroundTaskUser = new User
                {
                    Email = "builtin@background-task-record.com",
                    Active = true,
                    SystemName = WCoreUserDefaults.BackgroundTaskUserName,
                    UserType = UserType.SearchEngine,
                    CreatedOn = DateTime.Now
                };
            }

            return backgroundTaskUser;
        }
        public virtual User InsertGuestUser()
        {
            var user = new User
            {
                SystemName = WCoreUserDefaults.GuestsRoleName,
                UserType = UserType.Guest,
                Active = true,
                Deleted = false,
                CreatedOn = DateTime.Now,
                CannotLoginUntilDate = DateTime.Now,
                LastActivityDate = DateTime.Now,
                LastLoginDate = DateTime.Now
            };
            Insert(user);

            //if (user != null)
            //    user.LastUserSearch = _userSearchService.GetLastSearch(user.Id);

            return user;
        }
        public virtual User InsertNewsletterUser(string Email)
        {
            var user = new User
            {
                SystemName = WCoreUserDefaults.NewslettersRoleName,
                UserType = UserType.Newsletter,
                Email = Email,
                Active = true,
                Deleted = false,
                CreatedOn = DateTime.Now,
                CannotLoginUntilDate = DateTime.Now,
                LastActivityDate = DateTime.Now,
                LastLoginDate = DateTime.Now
            };
            Insert(user);

            //if (user != null)
            //    user.LastUserSearch = _userSearchService.GetLastSearch(user.Id);

            return user;
        }

        /// <summary>
        /// Gets a default tax display type (if configured)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        /// TODO
        public virtual TaxDisplayType? GetUserDefaultTaxDisplayType(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return TaxDisplayType.ExcludingTax;
        }

        /// <summary>
        /// Gets user for shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <returns>Result</returns>
        public virtual User GetShoppingCartUser(IList<ShoppingCartItem> shoppingCart)
        {
            var userId = shoppingCart.FirstOrDefault()?.UserId;

            return userId.HasValue && userId != 0 ? GetById(userId.Value) : null;

        }

        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User full name</returns>
        public virtual string GetUserFullName(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var firstName = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.FirstNameAttribute);
            var lastName = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.LastNameAttribute);

            var fullName = string.Empty;
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                fullName = $"{firstName} {lastName}";
            else
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;

                if (!string.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }

            return fullName;
        }
        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(User user, bool onlyActiveUserRoles = true)
        {
            return user.UserType == UserType.Registered;
        }
        /// <summary>
        /// Gets a value indicating whether user is guest
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsGuest(User user, bool onlyActiveUserRoles = true)
        {
            return user.UserType == UserType.Guest;
        }

        #endregion

        #region Gift Card


        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        public virtual string[] ParseAppliedGiftCardCouponCodes(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.GiftCardCouponCodesAttribute);

            var couponCodes = new List<string>();
            if (string.IsNullOrEmpty(existingCouponCodes))
                return couponCodes.ToArray();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(existingCouponCodes);

                var nodeList1 = xmlDoc.SelectNodes(@"//GiftCardCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null)
                        continue;

                    var code = node1.Attributes["Code"].InnerText.Trim();
                    couponCodes.Add(code);
                }
            }
            catch
            {
                // ignored
            }

            return couponCodes.ToArray();
        }

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public virtual void ApplyGiftCardCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = string.Empty;
            try
            {
                var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.GiftCardCouponCodesAttribute);

                couponCode = couponCode.Trim().ToLower();

                var xmlDoc = new XmlDocument();
                if (string.IsNullOrEmpty(existingCouponCodes))
                {
                    var element1 = xmlDoc.CreateElement("GiftCardCouponCodes");
                    xmlDoc.AppendChild(element1);
                }
                else
                {
                    xmlDoc.LoadXml(existingCouponCodes);
                }

                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//GiftCardCouponCodes");

                XmlElement gcElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//GiftCardCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null)
                        continue;

                    var couponCodeAttribute = node1.Attributes["Code"].InnerText.Trim();
                    if (couponCodeAttribute.ToLower() != couponCode.ToLower())
                        continue;

                    gcElement = (XmlElement)node1;
                    break;
                }

                //create new one if not found
                if (gcElement == null)
                {
                    gcElement = xmlDoc.CreateElement("CouponCode");
                    gcElement.SetAttribute("Code", couponCode);
                    rootElement.AppendChild(gcElement);
                }

                result = xmlDoc.OuterXml;
            }
            catch
            {
                // ignored
            }

            //apply new value
            _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.GiftCardCouponCodesAttribute, result);
        }

        /// <summary>
        /// Removes a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code to remove</param>
        /// <returns>New coupon codes document</returns>
        public virtual void RemoveGiftCardCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get applied coupon codes
            var existingCouponCodes = ParseAppliedGiftCardCouponCodes(user);

            //clear them
            _genericAttributeService.SaveAttribute<string>(user, WCoreUserDefaults.GiftCardCouponCodesAttribute, null);

            //save again except removed one
            foreach (var existingCouponCode in existingCouponCodes)
                if (!existingCouponCode.Equals(couponCode, StringComparison.InvariantCultureIgnoreCase))
                    ApplyGiftCardCouponCode(user, existingCouponCode);
        }
        #endregion

        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        public virtual string[] ParseAppliedDiscountCouponCodes(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.DiscountCouponCodeAttribute);

            var couponCodes = new List<string>();
            if (string.IsNullOrEmpty(existingCouponCodes))
                return couponCodes.ToArray();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(existingCouponCodes);

                var nodeList1 = xmlDoc.SelectNodes(@"//DiscountCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null)
                        continue;
                    var code = node1.Attributes["Code"].InnerText.Trim();
                    couponCodes.Add(code);
                }
            }
            catch
            {
                // ignored
            }

            return couponCodes.ToArray();
        }

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public virtual void ApplyDiscountCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = string.Empty;
            try
            {
                var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.DiscountCouponCodeAttribute);

                couponCode = couponCode.Trim().ToLower();

                var xmlDoc = new XmlDocument();
                if (string.IsNullOrEmpty(existingCouponCodes))
                {
                    var element1 = xmlDoc.CreateElement("DiscountCouponCodes");
                    xmlDoc.AppendChild(element1);
                }
                else
                {
                    xmlDoc.LoadXml(existingCouponCodes);
                }

                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//DiscountCouponCodes");

                XmlElement gcElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//DiscountCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null)
                        continue;

                    var couponCodeAttribute = node1.Attributes["Code"].InnerText.Trim();

                    if (couponCodeAttribute.ToLower() != couponCode.ToLower())
                        continue;

                    gcElement = (XmlElement)node1;
                    break;
                }

                //create new one if not found
                if (gcElement == null)
                {
                    gcElement = xmlDoc.CreateElement("CouponCode");
                    gcElement.SetAttribute("Code", couponCode);
                    rootElement.AppendChild(gcElement);
                }

                result = xmlDoc.OuterXml;
            }
            catch
            {
                // ignored
            }

            //apply new value
            _genericAttributeService.SaveAttribute(user, WCoreUserDefaults.DiscountCouponCodeAttribute, result);
        }

        /// <summary>
        /// Removes a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code to remove</param>
        /// <returns>New coupon codes document</returns>
        public virtual void RemoveDiscountCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get applied coupon codes
            var existingCouponCodes = ParseAppliedDiscountCouponCodes(user);

            //clear them
            _genericAttributeService.SaveAttribute<string>(user, WCoreUserDefaults.DiscountCouponCodeAttribute, null);

            //save again except removed one
            foreach (var existingCouponCode in existingCouponCodes)
                if (!existingCouponCode.Equals(couponCode, StringComparison.InvariantCultureIgnoreCase))
                    ApplyDiscountCouponCode(user, existingCouponCode);
        }

        #region User address mapping

        /// <summary>
        /// Remove a user-address mapping record
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="address">Address</param>
        public virtual void RemoveUserAddress(User user, Address address)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (_userAddressRepository.GetAll().FirstOrDefault(m => m.AddressId == address.Id && m.UserId == user.Id) is UserAddress mapping)
            {
                if (user.BillingAddressId == address.Id)
                    user.BillingAddressId = null;
                if (user.ShippingAddressId == address.Id)
                    user.ShippingAddressId = null;

                _userAddressRepository.Delete(mapping);
            }
        }

        /// <summary>
        /// Inserts a user-address mapping record
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="address">Address</param>
        public virtual void InsertUserAddress(User user, Address address)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (address is null)
                throw new ArgumentNullException(nameof(address));

            if (_userAddressRepository.GetAll().FirstOrDefault(m => m.AddressId == address.Id && m.UserId == user.Id) is null)
            {
                var mapping = new UserAddress
                {
                    AddressId = address.Id,
                    UserId = user.Id
                };

                _userAddressRepository.Insert(mapping);
            }
        }


        /// <summary>
        /// Gets a list of addresses mapped to user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result</returns>
        public virtual IList<Address> GetAddressesByUserId(int userId)
        {
            var query = from address in _addressRepository.GetAll()
                        join cam in _userAddressRepository.GetAll() on address.Id equals cam.AddressId
                        where cam.UserId == userId
                        select address;

            var key = _cacheKeyService.PrepareKeyForShortTermCache(WCoreUserServicesDefaults.UserAddressesByUserIdCacheKey, userId);

            return _staticCacheManager.Get(key, () => query.ToList());
        }

        /// <summary>
        /// Gets a address mapped to user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="addressId">Address identifier</param>
        /// <returns>Result</returns>
        public virtual Address GetUserAddress(int userId, int addressId)
        {
            if (userId == 0 || addressId == 0)
                return null;

            var query = from address in _addressRepository.GetAll()
                        join cam in _userAddressRepository.GetAll() on address.Id equals cam.AddressId
                        where cam.UserId == userId && address.Id == addressId
                        select address;

            var key = _cacheKeyService.PrepareKeyForShortTermCache(WCoreUserServicesDefaults.UserAddressCacheKeyCacheKey, userId, addressId);

            return _staticCacheManager.Get(key, () => query.Single());
        }

        /// <summary>
        /// Gets a user billing address
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <returns>Result</returns>
        public virtual Address GetUserBillingAddress(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            return GetUserAddress(user.Id, user.BillingAddressId ?? 0);
        }

        /// <summary>
        /// Gets a user shipping address
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public virtual Address GetUserShippingAddress(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            return GetUserAddress(user.Id, user.ShippingAddressId ?? 0);
        }

        #endregion

        #region User passwords

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordFormat">Password format; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        public virtual IList<UserPassword> GetUserPasswords(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _userPasswordRepository.GetAll().AsQueryable();

            //filter by user
            if (userId.HasValue)
                query = query.Where(password => password.UserId == userId.Value);

            //filter by password format
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)passwordFormat.Value);

            //get the latest passwords
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedOn).Take(passwordsToReturn.Value);

            return query.ToList();
        }

        /// <summary>
        /// Get current user password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User password</returns>
        public virtual UserPassword GetCurrentPassword(int userId)
        {
            if (userId == 0)
                return null;

            //return the latest password
            return GetUserPasswords(userId, passwordsToReturn: 1).FirstOrDefault();
        }

        /// <summary>
        /// Insert a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void InsertUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Insert(userPassword);

            //event notification
            _eventPublisher.EntityInserted(userPassword);
        }

        /// <summary>
        /// Update a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void UpdateUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Update(userPassword);

            //event notification
            _eventPublisher.EntityUpdated(userPassword);
        }

        /// <summary>
        /// Check whether password recovery token is valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token to validate</param>
        /// <returns>Result</returns>
        public virtual bool IsPasswordRecoveryTokenValid(User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var cPrt = _genericAttributeService.GetAttribute<string>(user, WCoreUserDefaults.PasswordRecoveryTokenAttribute);
            if (string.IsNullOrEmpty(cPrt))
                return false;

            if (!cPrt.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Check whether password recovery link is expired
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public virtual bool IsPasswordRecoveryLinkExpired(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (_userSettings.PasswordRecoveryLinkDaysValid == 0)
                return false;

            var geneatedDate = _genericAttributeService.GetAttribute<DateTime?>(user, WCoreUserDefaults.PasswordRecoveryTokenDateGeneratedAttribute);
            if (!geneatedDate.HasValue)
                return false;

            var daysPassed = (DateTime.Now - geneatedDate.Value).TotalDays;
            if (daysPassed > _userSettings.PasswordRecoveryLinkDaysValid)
                return true;

            return false;
        }

        /// <summary>
        /// Check whether user password is expired 
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True if password is expired; otherwise false</returns>
        public virtual bool PasswordIsExpired(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //the guests don't have a password
            if (IsGuest(user))
                return false;

            //setting disabled for all
            if (_userSettings.PasswordLifetime == 0)
                return false;

            var cacheKey = _cacheKeyService.PrepareKeyForShortTermCache(WCoreUserServicesDefaults.UserPasswordLifetimeCacheKey, user);

            //get current password usage time
            var currentLifetime = _staticCacheManager.Get(cacheKey, () =>
            {
                var userPassword = GetCurrentPassword(user.Id);
                //password is not found, so return max value to force user to change password
                if (userPassword == null)
                    return int.MaxValue;

                return (DateTime.Now - userPassword.CreatedOn).Days;
            });

            return currentLifetime >= _userSettings.PasswordLifetime;
        }

        #endregion
    }
}
