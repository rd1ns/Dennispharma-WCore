using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using WCore.Core.Infrastructure;
using WCore.Framework.Models;
using System;
using System.Linq;

namespace WCore.Framework.Mvc.ModelBinding
{
    /// <summary>
    /// Represents model binder provider for the creating WCoreModelBinder
    /// </summary>
    public class WCoreModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Creates a WCore model binder based on passed context
        /// </summary>
        /// <param name="context">Model binder provider context</param>
        /// <returns>Model binder</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            var modelType = context.Metadata.ModelType;
            if (!typeof(BaseWCoreModel).IsAssignableFrom(modelType))
                return null;

            //use WCoreModelBinder as a ComplexTypeModelBinder for BaseWCoreModel
            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                //create binders for all model properties
                var propertyBinders = context.Metadata.Properties
                    .ToDictionary(modelProperty => modelProperty, modelProperty => context.CreateBinder(modelProperty));

                return new WCoreModelBinder(propertyBinders, EngineContext.Current.Resolve<ILoggerFactory>());
            }

            //or return null to further search for a suitable binder
            return null;
        }
    }
}
