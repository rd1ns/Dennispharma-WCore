﻿using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCore.Core.Infrastructure;

namespace WCore.Framework.Mvc.Routing
{
    /// <summary>
    /// Represents implementation of route publisher
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {
        #region Fields

        /// <summary>
        /// Type finder
        /// </summary>
        protected readonly ITypeFinder _typeFinder;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="typeFinder">Type finder</param>
        public RoutePublisher(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public virtual void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //find route providers provided by other assemblies
            var routeProviders = _typeFinder.FindClassesOfType<IRouteProvider>();

            //create and sort instances of route providers
            var instances = routeProviders
                .Select(routeProvider => (IRouteProvider)Activator.CreateInstance(routeProvider))
                .OrderByDescending(routeProvider => routeProvider.Priority);

            //register all provided routes
            foreach (var routeProvider in instances)
                routeProvider.RegisterRoutes(endpointRouteBuilder);
        }

        #endregion
    }
}