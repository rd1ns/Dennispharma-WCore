using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using WCore.Core;
using WCore.Services.Logging;

namespace WCore.Services.Plugins.Marketplace
{
    /// <summary>
    /// Represents the official feed manager (plugins from wCorecommerce marketplace)
    /// </summary>
    public partial class OfficialFeedManager
    {
        #region Fields

        private readonly ILogger _logger;
        //private readonly WCoreHttpClient _wCoreHttpClient;

        #endregion

        #region Ctor

        public OfficialFeedManager(ILogger logger
            /*WCoreHttpClient wCoreHttpClient*/)
        {
            _logger = logger;
            //_wCoreHttpClient = wCoreHttpClient;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get element value
        /// </summary>
        /// <param name="node">XML node</param>
        /// <param name="elementName">Element name</param>
        /// <returns>Value (text)</returns>
        protected virtual string GetElementValue(XmlNode node, string elementName)
        {
            return node?.SelectSingleNode(elementName)?.InnerText;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get available categories of marketplace extensions
        /// </summary>
        /// <returns>Result</returns>
        public virtual IList<OfficialFeedCategory> GetCategories()
        {
            return null;
        }

        /// <summary>
        /// Get available versions of marketplace extensions
        /// </summary>
        /// <returns>Result</returns>
        public virtual IList<OfficialFeedVersion> GetVersions()
        {
            return null;
        }

        /// <summary>
        /// Get all plugins
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="versionId">Version identifier</param>
        /// <param name="price">Price; 0 - all, 10 - free, 20 - paid</param>
        /// <param name="searchTerm">Search term</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Plugins</returns>
        public virtual IPagedList<OfficialFeedPlugin> GetAllPlugins(int categoryId = 0,
            int versionId = 0, int price = 0, string searchTerm = "",
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return null;
        }

        #endregion
    }
}