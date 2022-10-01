using System.Collections.Generic;
using WCore.Core.Domain.Messages;

namespace WCore.Services.Messages
{
    /// <summary>
    /// Campaign service
    /// </summary>
    public partial interface ICampaignService
    {
        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <param name="storeId">Store identifier; 0 to load all records</param>
        /// <returns>Campaigns</returns>
        IList<Campaign> GetAllCampaigns(int storeId = 0);
        
        /// <summary>
        /// Sends a campaign to specified emails
        /// </summary>
        /// <param name="campaign">Campaign</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="subscriptions">Subscriptions</param>
        /// <returns>Total emails sent</returns>
        int SendCampaign(Campaign campaign, EmailAccount emailAccount,
            IEnumerable<NewsLetterSubscription> subscriptions);

        /// <summary>
        /// Sends a campaign to specified email
        /// </summary>
        /// <param name="campaign">Campaign</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="email">Email</param>
        void SendCampaign(Campaign campaign, EmailAccount emailAccount, string email);
    }
}
