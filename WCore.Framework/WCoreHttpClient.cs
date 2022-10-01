using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WCore.Core;
using WCore.Services.Common;
using WCore.Services.Localization;

namespace WCore.Framework
{
    /// <summary>
    /// Represents the HTTP client to request wCoreCommerce official site
    /// </summary>
    public partial class WCoreHttpClient
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHelper _webHelper;
        private readonly ILanguageService _languageService;

        #endregion

        #region Ctor

        public WCoreHttpClient(HttpClient client,
            IHttpContextAccessor httpContextAccessor,
            IWebHelper webHelper,
            ILanguageService languageService)
        {
            //configure client
            client.BaseAddress = new Uri("https://www.WCore.com/");
            client.Timeout = TimeSpan.FromMilliseconds(5000);
            client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, $"WCore-{WCoreVersion.CurrentVersion}");

            this._httpClient = client;
            this._httpContextAccessor = httpContextAccessor;
            this._webHelper = webHelper;
            this._languageService = languageService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check whether the site is available
        /// </summary>
        /// <returns>The asynchronous task whose result determines that request is completed</returns>
        public virtual async Task PingAsync()
        {
            await _httpClient.GetStringAsync("/");
        }

        /// <summary>
        /// Get official news RSS
        /// </summary>
        /// <returns>The asynchronous task whose result contains news RSS feed</returns>
        //public virtual async Task<RssFeed> GetNewsRssAsync()
        //{
        //    //prepare URL to request
        //    var url = string.Format(WCoreCommonDefaults.WCoreNewsRssPath,
        //        WCoreVersion.CurrentVersion,
        //        _webHelper.IsLocalRequest(_httpContextAccessor.HttpContext.Request),
        //        _adminAreaSettings.HideAdvertisementsOnAdminArea,
        //        _webHelper.GetStoreLocation())
        //        .ToLowerInvariant();

        //    //get response
        //    var stream = await _httpClient.GetStreamAsync(url);
        //    return await RssFeed.LoadAsync(stream);
        //}

        /// <summary>
        /// Get a response regarding available categories of marketplace extensions
        /// </summary>
        /// <returns>The asynchronous task whose result contains the result string</returns>
        //public virtual async Task<string> GetExtensionsCategoriesAsync()
        //{
        //    //prepare URL to request
        //    var url = WCoreCommonDefaults.WCoreExtensionsCategoriesPath.ToLowerInvariant();

        //    //get response
        //    return await _httpClient.GetStringAsync(url);
        //}

        /// <summary>
        /// Get a response regarding available versions of marketplace extensions
        /// </summary>
        /// <returns>The asynchronous task whose result contains the result string</returns>
        //public virtual async Task<string> GetExtensionsVersionsAsync()
        //{
        //    //prepare URL to request
        //    var url = WCoreCommonDefaults.WCoreExtensionsVersionsPath.ToLowerInvariant();

        //    //get response
        //    return await _httpClient.GetStringAsync(url);
        //}

        /// <summary>
        /// Get a response regarding marketplace extensions
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="versionId">Version identifier</param>
        /// <param name="price">Price; 0 - all, 10 - free, 20 - paid</param>
        /// <param name="searchTerm">Search term</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The asynchronous task whose result contains the result string</returns>
        public virtual async Task<string> GetExtensionsAsync(int categoryId = 0,
            int versionId = 0, int price = 0, string searchTerm = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //prepare URL to request
            var url = string.Format(WCoreCommonDefaults.WCoreExtensionsPath,
                categoryId, versionId, price, WebUtility.UrlEncode(searchTerm), pageIndex, pageSize)
                .ToLowerInvariant();

            //get response
            return await _httpClient.GetStringAsync(url);
        }

        #endregion
    }
}
