using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;

namespace WCore.Services.Catalog
{
    /// <summary>
    /// Review type service implementation
    /// </summary>
    public partial class ReviewTypeService : Repository<ReviewType>, IReviewTypeService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ProductReviewReviewType> _productReviewReviewTypeRepository;
        private readonly IRepository<ReviewType> _reviewTypeRepository;

        #endregion

        #region Ctor

        public ReviewTypeService(WCoreContext context, ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            IRepository<ProductReviewReviewType> productReviewReviewTypeRepository,
            IRepository<ReviewType> reviewTypeRepository) : base(context)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _productReviewReviewTypeRepository = productReviewReviewTypeRepository;
            _reviewTypeRepository = reviewTypeRepository;
        }

        #endregion

        #region Methods

        #region Review type

        /// <summary>
        /// Gets all review types
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<ReviewType> GetAllReviewTypes()
        {
            return context.ReviewTypes
                .OrderBy(reviewType => reviewType.DisplayOrder).ThenBy(reviewType => reviewType.Id)
                .ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ReviewTypeAllCacheKey));
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="reviewTypeId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ReviewType GetReviewTypeById(int reviewTypeId)
        {
            if (reviewTypeId == 0)
                return null;

            return _reviewTypeRepository.ToCachedGetById(reviewTypeId);
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void InsertReviewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Insert(reviewType);

            //event notification
            _eventPublisher.EntityInserted(reviewType);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void UpdateReviewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Update(reviewType);

            //event notification
            _eventPublisher.EntityUpdated(reviewType);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void DeleteReiewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Delete(reviewType);

            //event notification
            _eventPublisher.EntityDeleted(reviewType);
        }

        #endregion

        #region Product review type mapping

        /// <summary>
        /// Gets product review and review type mappings by product review identifier
        /// </summary>
        /// <param name="productReviewId">The product review identifier</param>
        /// <returns>Product review and review type mapping collection</returns>
        public IList<ProductReviewReviewType> GetProductReviewReviewTypesByProductReviewId(
            int productReviewId)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductReviewReviewTypeMappingAllCacheKey, productReviewId);

            var query = from pam in context.ProductReviewReviewTypes.AsQueryable()
                        orderby pam.Id
                        where pam.ProductReviewId == productReviewId
                        select pam;
            var productReviewReviewTypes = query.ToCachedList(key);

            return productReviewReviewTypes;
        }

        /// <summary>
        /// Inserts a product review and review type mapping
        /// </summary>
        /// <param name="productReviewReviewType">Product review and review type mapping</param>
        public virtual void InsertProductReviewReviewTypes(ProductReviewReviewType productReviewReviewType)
        {
            if (productReviewReviewType == null)
                throw new ArgumentNullException(nameof(productReviewReviewType));

            _productReviewReviewTypeRepository.Insert(productReviewReviewType);

            //event notification
            _eventPublisher.EntityInserted(productReviewReviewType);
        }

        #endregion

        #endregion
    }
}