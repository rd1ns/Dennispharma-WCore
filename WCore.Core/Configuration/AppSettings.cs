﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Core.Configuration
{
    /// <summary>
    /// Represents the app settings
    /// </summary>
    public partial class AppSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets cache configuration parameters
        /// </summary>
        public CacheConfig CacheConfig { get; set; } = new CacheConfig();

        /// <summary>
        /// Gets or sets database configuration parameters
        /// </summary>
        public DbConfig DbConfig { get; set; } = new DbConfig();

        /// <summary>
        /// Gets or sets hosting configuration parameters
        /// </summary>
        public HostingConfig HostingConfig { get; set; } = new HostingConfig();

        /// <summary>
        /// Gets or sets common configuration parameters
        /// </summary>
        public CommonConfig CommonConfig { get; set; } = new CommonConfig();

        /// <summary>
        /// Gets or sets additional configuration parameters
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        #endregion
    }
}