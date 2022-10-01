﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using WCore.Core.Domain;
using WCore.Core.Domain.Stores;
using WCore.Services.Stores;

namespace WCore.Framework.Models
{
    /// <summary>
    /// Represents the store mapping supported model factory
    /// </summary>
    public partial interface IStoreMappingSupportedModelFactory
    {
        /// <summary>
        /// Prepare selected and all available stores for the passed model
        /// </summary>
        /// <typeparam name="TModel">Store mapping supported model type</typeparam>
        /// <param name="model">Model</param>
        void PrepareModelStores<TModel>(TModel model) where TModel : IStoreMappingSupportedModel;

        /// <summary>
        /// Prepare selected and all available stores for the passed model by store mappings
        /// </summary>
        /// <typeparam name="TModel">Store mapping supported model type</typeparam>
        /// <typeparam name="TEntity">Store mapping supported entity type</typeparam>
        /// <param name="model">Model</param>
        /// <param name="entity">Entity</param>
        /// <param name="ignoreStoreMappings">Whether to ignore existing store mappings</param>
        void PrepareModelStores<TModel, TEntity>(TModel model, TEntity entity, bool ignoreStoreMappings)
            where TModel : IStoreMappingSupportedModel where TEntity : BaseEntity, IStoreMappingSupported;
    }

    /// <summary>
    /// Represents the base store mapping supported model factory implementation
    /// </summary>
    public partial class StoreMappingSupportedModelFactory : IStoreMappingSupportedModelFactory
    {
        #region Fields

        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public StoreMappingSupportedModelFactory(IStoreMappingService storeMappingService,
            IStoreService storeService)
        {
            _storeMappingService = storeMappingService;
            _storeService = storeService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare selected and all available stores for the passed model
        /// </summary>
        /// <typeparam name="TModel">Store mapping supported model type</typeparam>
        /// <param name="model">Model</param>
        public virtual void PrepareModelStores<TModel>(TModel model) where TModel : IStoreMappingSupportedModel
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare available stores
            var availableStores = _storeService.GetAllByFilters();
            model.AvailableStores = availableStores.Select(store => new SelectListItem
            {
                Text = store.Name,
                Value = store.Id.ToString(),
                Selected = model.SelectedStoreIds.Contains(store.Id)
            }).ToList();
        }

        /// <summary>
        /// Prepare selected and all available stores for the passed model by store mappings
        /// </summary>
        /// <typeparam name="TModel">Store mapping supported model type</typeparam>
        /// <typeparam name="TEntity">Store mapping supported entity type</typeparam>
        /// <param name="model">Model</param>
        /// <param name="entity">Entity</param>
        /// <param name="ignoreStoreMappings">Whether to ignore existing store mappings</param>
        public virtual void PrepareModelStores<TModel, TEntity>(TModel model, TEntity entity, bool ignoreStoreMappings)
            where TModel : IStoreMappingSupportedModel where TEntity : BaseEntity, IStoreMappingSupported
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare stores with granted access
            if (!ignoreStoreMappings && entity != null)
                model.SelectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(entity).ToList();

            PrepareModelStores(model);
        }

        #endregion
    }
}
