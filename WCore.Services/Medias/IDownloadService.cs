using Microsoft.AspNetCore.Http;
using System;
using WCore.Core.Domain.Medias;
using WCore.Core.Domain.Orders;

namespace WCore.Services.Medias
{
    /// <summary>
    /// Download service interface
    /// </summary>
    public partial interface IDownloadService : IRepository<Download>
    {
        /// <summary>
        /// Gets a download by GUID
        /// </summary>
        /// <param name="downloadGuid">Download GUID</param>
        /// <returns>Download</returns>
        Download GetDownloadByGuid(Guid downloadGuid);

        /// <summary>
        /// Gets a value indicating whether download is allowed
        /// </summary>
        /// <param name="orderItem">Order item to check</param>
        /// <returns>True if download is allowed; otherwise, false.</returns>
        bool IsDownloadAllowed(OrderItem orderItem);

        /// <summary>
        /// Gets a value indicating whether license download is allowed
        /// </summary>
        /// <param name="orderItem">Order item to check</param>
        /// <returns>True if license download is allowed; otherwise, false.</returns>
        bool IsLicenseDownloadAllowed(OrderItem orderItem);

        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Download binary array</returns>
        byte[] GetDownloadBits(IFormFile file);
    }
}