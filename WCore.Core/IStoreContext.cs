﻿using WCore.Core.Domain.Stores;

namespace WCore.Core
{
    /// <summary>
    /// Store context
    /// </summary>
    public interface IStoreContext
    {
        /// <summary>
        /// Gets the current store
        /// </summary>
        Store CurrentStore { get; }

        /// <summary>
        /// Gets active store scope configuration
        /// </summary>
        int ActiveStoreScopeConfiguration { get; }
    }
}
