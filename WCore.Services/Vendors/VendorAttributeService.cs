using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Vendors;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;

namespace WCore.Services.Vendors
{
    /// <summary>
    /// Represents a vendor attribute service implementation
    /// </summary>
    public partial class VendorAttributeService : Repository<VendorAttribute>, IVendorAttributeService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<VendorAttribute> _vendorAttributeRepository;
        private readonly IRepository<VendorAttributeValue> _vendorAttributeValueRepository;

        #endregion

        #region Ctor

        public VendorAttributeService(WCoreContext context, ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            IRepository<VendorAttribute> vendorAttributeRepository,
            IRepository<VendorAttributeValue> vendorAttributeValueRepository) : base(context)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _vendorAttributeRepository = vendorAttributeRepository;
            _vendorAttributeValueRepository = vendorAttributeValueRepository;
        }

        #endregion

        #region Methods

        #region Vendor attributes

        /// <summary>
        /// Gets all vendor attributes
        /// </summary>
        /// <returns>Vendor attributes</returns>
        public virtual IList<VendorAttribute> GetAllVendorAttributes()
        {
            var query = _vendorAttributeRepository.GetAll()
                .OrderBy(vendorAttribute => vendorAttribute.DisplayOrder).ThenBy(vendorAttribute => vendorAttribute.Id).AsQueryable();
            return query
                .ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(WCoreVendorDefaults.VendorAttributesAllCacheKey));
        }

        /// <summary>
        /// Gets a vendor attribute 
        /// </summary>
        /// <param name="vendorAttributeId">Vendor attribute identifier</param>
        /// <returns>Vendor attribute</returns>
        public virtual VendorAttribute GetVendorAttributeById(int vendorAttributeId)
        {
            if (vendorAttributeId == 0)
                return null;

            return _vendorAttributeRepository.ToCachedGetById(vendorAttributeId);
        }

        /// <summary>
        /// Inserts a vendor attribute
        /// </summary>
        /// <param name="vendorAttribute">Vendor attribute</param>
        public virtual void InsertVendorAttribute(VendorAttribute vendorAttribute)
        {
            if (vendorAttribute == null)
                throw new ArgumentNullException(nameof(vendorAttribute));

            _vendorAttributeRepository.Insert(vendorAttribute);

            //event notification
            _eventPublisher.EntityInserted(vendorAttribute);
        }

        /// <summary>
        /// Updates a vendor attribute
        /// </summary>
        /// <param name="vendorAttribute">Vendor attribute</param>
        public virtual void UpdateVendorAttribute(VendorAttribute vendorAttribute)
        {
            if (vendorAttribute == null)
                throw new ArgumentNullException(nameof(vendorAttribute));

            _vendorAttributeRepository.Update(vendorAttribute);

            //event notification
            _eventPublisher.EntityUpdated(vendorAttribute);
        }

        /// <summary>
        /// Deletes a vendor attribute
        /// </summary>
        /// <param name="vendorAttribute">Vendor attribute</param>
        public virtual void DeleteVendorAttribute(VendorAttribute vendorAttribute)
        {
            if (vendorAttribute == null)
                throw new ArgumentNullException(nameof(vendorAttribute));

            _vendorAttributeRepository.Delete(vendorAttribute);

            //event notification
            _eventPublisher.EntityDeleted(vendorAttribute);
        }

        #endregion

        #region Vendor attribute values

        /// <summary>
        /// Gets vendor attribute values by vendor attribute identifier
        /// </summary>
        /// <param name="vendorAttributeId">The vendor attribute identifier</param>
        /// <returns>Vendor attribute values</returns>
        public virtual IList<VendorAttributeValue> GetVendorAttributeValues(int vendorAttributeId)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreVendorDefaults.VendorAttributeValuesAllCacheKey, vendorAttributeId);

            return _vendorAttributeValueRepository.GetAll()
                .Where(vendorAttributeValue => vendorAttributeValue.VendorAttributeId == vendorAttributeId)
                .OrderBy(vendorAttributeValue => vendorAttributeValue.DisplayOrder)
                .ThenBy(vendorAttributeValue => vendorAttributeValue.Id).AsQueryable()
                .ToCachedList(key);
        }

        /// <summary>
        /// Gets a vendor attribute value
        /// </summary>
        /// <param name="vendorAttributeValueId">Vendor attribute value identifier</param>
        /// <returns>Vendor attribute value</returns>
        public virtual VendorAttributeValue GetVendorAttributeValueById(int vendorAttributeValueId)
        {
            if (vendorAttributeValueId == 0)
                return null;

            return _vendorAttributeValueRepository.ToCachedGetById(vendorAttributeValueId);
        }

        /// <summary>
        /// Inserts a vendor attribute value
        /// </summary>
        /// <param name="vendorAttributeValue">Vendor attribute value</param>
        public virtual void InsertVendorAttributeValue(VendorAttributeValue vendorAttributeValue)
        {
            if (vendorAttributeValue == null)
                throw new ArgumentNullException(nameof(vendorAttributeValue));

            _vendorAttributeValueRepository.Insert(vendorAttributeValue);

            //event notification
            _eventPublisher.EntityInserted(vendorAttributeValue);
        }

        /// <summary>
        /// Updates the vendor attribute value
        /// </summary>
        /// <param name="vendorAttributeValue">Vendor attribute value</param>
        public virtual void UpdateVendorAttributeValue(VendorAttributeValue vendorAttributeValue)
        {
            if (vendorAttributeValue == null)
                throw new ArgumentNullException(nameof(vendorAttributeValue));

            _vendorAttributeValueRepository.Update(vendorAttributeValue);

            //event notification
            _eventPublisher.EntityUpdated(vendorAttributeValue);
        }

        /// <summary>
        /// Deletes a vendor attribute value
        /// </summary>
        /// <param name="vendorAttributeValue">Vendor attribute value</param>
        public virtual void DeleteVendorAttributeValue(VendorAttributeValue vendorAttributeValue)
        {
            if (vendorAttributeValue == null)
                throw new ArgumentNullException(nameof(vendorAttributeValue));

            _vendorAttributeValueRepository.Delete(vendorAttributeValue);

            //event notification
            _eventPublisher.EntityDeleted(vendorAttributeValue);
        }

        #endregion

        #endregion
    }
}