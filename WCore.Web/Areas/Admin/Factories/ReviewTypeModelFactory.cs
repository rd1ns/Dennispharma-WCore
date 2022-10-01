using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCore.Core.Domain.Catalog;
using WCore.Framework.Factories;
using WCore.Framework.Models;
using WCore.Services.Catalog;
using WCore.Services.Localization;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Catalog;

namespace WCore.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents a review type model factory
    /// </summary>
    public partial interface IReviewTypeModelFactory
    {
        /// <summary>
        /// Prepare review type search model
        /// </summary>
        /// <param name="searchModel">Review type search model</param>
        /// <returns>Review type search model</returns>
        ReviewTypeSearchModel PrepareReviewTypeSearchModel(ReviewTypeSearchModel searchModel);

        /// <summary>
        /// Prepare paged review type list model
        /// </summary>
        /// <param name="searchModel">Review type search model</param>
        /// <returns>Review type list model</returns>
        ReviewTypeListModel PrepareReviewTypeListModel(ReviewTypeSearchModel searchModel);

        /// <summary>
        /// Prepare review type model
        /// </summary>
        /// <param name="model">Review type model</param>
        /// <param name="reviewType">Review type</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Review type model</returns>
        ReviewTypeModel PrepareReviewTypeModel(ReviewTypeModel model,
            ReviewType reviewType, bool excludeProperties = false);
    }
}
namespace WCore.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents a review type model factory implementation
    /// </summary>
    public partial class ReviewTypeModelFactory : IReviewTypeModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IReviewTypeService _reviewTypeService;

        #endregion

        #region Ctor

        public ReviewTypeModelFactory(ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IReviewTypeService reviewTypeService)
        {
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _reviewTypeService = reviewTypeService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare review type search model
        /// </summary>
        /// <param name="searchModel">Review type search model</param>
        /// <returns>Review type search model</returns>
        public virtual ReviewTypeSearchModel PrepareReviewTypeSearchModel(ReviewTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged review type list model
        /// </summary>
        /// <param name="searchModel">Review type search model</param>
        /// <returns>Review type list model</returns>
        public virtual ReviewTypeListModel PrepareReviewTypeListModel(ReviewTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get review types
            var reviewTypes = _reviewTypeService.GetAllReviewTypes().ToPagedList(searchModel);

            //prepare list model
            var model = new ReviewTypeListModel().PrepareToGrid(searchModel, reviewTypes, () =>
            {
                //fill in model values from the entity
                return reviewTypes.Select(reviewType => reviewType.ToModel<ReviewTypeModel>());
            });

            return model;
        }

        /// <summary>
        /// Prepare review type model
        /// </summary>
        /// <param name="model">Review type model</param>
        /// <param name="reviewType">Review type</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Review type model</returns>
        public virtual ReviewTypeModel PrepareReviewTypeModel(ReviewTypeModel model,
            ReviewType reviewType, bool excludeProperties = false)
        {
            Action<ReviewTypeLocalizedModel, int> localizedModelConfiguration = null;

            if (reviewType != null)
            {
                //fill in model values from the entity
                model ??= reviewType.ToModel<ReviewTypeModel>();

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(reviewType, entity => entity.Name, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(reviewType, entity => entity.Description, languageId, false, false);
                };
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        #endregion
    }
}