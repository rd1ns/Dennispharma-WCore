using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core.Domain.Discounts;
using WCore.Services.Discounts;
using WCore.Services.Stores;

namespace WCore.Framework.Models
{
    /// <summary>
    /// Represents the discount supported model factory
    /// </summary>
    public partial interface IDiscountSupportedModelFactory
    {
        /// <summary>
        /// Prepare selected and all available discounts for the passed model
        /// </summary>
        /// <typeparam name="TModel">Discount supported model type</typeparam>
        /// <param name="model">Model</param>
        /// <param name="availableDiscounts">List of all available discounts</param>
        void PrepareModelDiscounts<TModel>(TModel model, IList<Discount> availableDiscounts) where TModel : IDiscountSupportedModel;

        /// <summary>
        /// Prepare selected and all available discounts for the passed model by entity applied discounts
        /// </summary>
        /// <typeparam name="TModel">Discount supported model type</typeparam>
        /// <typeparam name="TMapping">Discount supported entity type</typeparam>
        /// <param name="model">Model</param>
        /// <param name="entity">Entity</param>
        /// <param name="availableDiscounts">List of all available discounts</param>
        /// <param name="ignoreAppliedDiscounts">Whether to ignore existing applied discounts</param>
        void PrepareModelDiscounts<TModel, TMapping>(TModel model, IDiscountSupported<TMapping> entity,
            IList<Discount> availableDiscounts, bool ignoreAppliedDiscounts)
            where TModel : IDiscountSupportedModel where TMapping : DiscountMapping;
    }

    /// <summary>
    /// Represents the base discount supported model factory implementation
    /// </summary>
    public partial class DiscountSupportedModelFactory : IDiscountSupportedModelFactory
    {

        #region Fields

        private readonly IDiscountService _discountService;

        #endregion

        #region Ctor

        public DiscountSupportedModelFactory(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare selected and all available discounts for the passed model
        /// </summary>
        /// <typeparam name="TModel">Discount supported model type</typeparam>
        /// <param name="model">Model</param>
        /// <param name="availableDiscounts">List of all available discounts</param>
        public virtual void PrepareModelDiscounts<TModel>(TModel model, IList<Discount> availableDiscounts) where TModel : IDiscountSupportedModel
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare available discounts
            model.AvailableDiscounts = availableDiscounts.Select(discount => new SelectListItem
            {
                Text = discount.Name,
                Value = discount.Id.ToString(),
                Selected = model.SelectedDiscountIds.Contains(discount.Id)
            }).ToList();
        }

        /// <summary>
        /// Prepare selected and all available discounts for the passed model by entity applied discounts
        /// </summary>
        /// <typeparam name="TModel">Discount supported model type</typeparam>
        /// <typeparam name="TMapping">Discount supported entity type</typeparam>
        /// <param name="model">Model</param>
        /// <param name="entity">Entity</param>
        /// <param name="availableDiscounts">List of all available discounts</param>
        /// <param name="ignoreAppliedDiscounts">Whether to ignore existing applied discounts</param>
        public virtual void PrepareModelDiscounts<TModel, TMapping>(TModel model, IDiscountSupported<TMapping> entity,
            IList<Discount> availableDiscounts, bool ignoreAppliedDiscounts)
            where TModel : IDiscountSupportedModel where TMapping : DiscountMapping
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare already applied discounts
            if (!ignoreAppliedDiscounts && entity != null)
                model.SelectedDiscountIds = _discountService.GetAppliedDiscounts(entity).Select(discount => discount.Id).ToList();

            PrepareModelDiscounts(model, availableDiscounts);
        }

        #endregion
    }
}
