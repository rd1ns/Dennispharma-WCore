using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Messages;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;

namespace WCore.Services.Messages
{
    /// <summary>
    /// Queued email service
    /// </summary>
    public partial class QueuedEmailService : Repository<QueuedEmail>, IQueuedEmailService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<QueuedEmail> _queuedEmailRepository;

        #endregion

        #region Ctor

        public QueuedEmailService(WCoreContext context, IEventPublisher eventPublisher,
            IRepository<QueuedEmail> queuedEmailRepository) : base(context)
        {
            _eventPublisher = eventPublisher;
            _queuedEmailRepository = queuedEmailRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>        
        public virtual void InsertQueuedEmail(QueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException(nameof(queuedEmail));

            _queuedEmailRepository.Insert(queuedEmail);

            //event notification
            _eventPublisher.EntityInserted(queuedEmail);
        }

        /// <summary>
        /// Updates a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>
        public virtual void UpdateQueuedEmail(QueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException(nameof(queuedEmail));

            _queuedEmailRepository.Update(queuedEmail);

            //event notification
            _eventPublisher.EntityUpdated(queuedEmail);
        }

        /// <summary>
        /// Deleted a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>
        public virtual void DeleteQueuedEmail(QueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException(nameof(queuedEmail));

            _queuedEmailRepository.Delete(queuedEmail);

            //event notification
            _eventPublisher.EntityDeleted(queuedEmail);
        }

        /// <summary>
        /// Deleted a queued emails
        /// </summary>
        /// <param name="queuedEmails">Queued emails</param>
        public virtual void DeleteQueuedEmails(IList<QueuedEmail> queuedEmails)
        {
            if (queuedEmails == null)
                throw new ArgumentNullException(nameof(queuedEmails));

            _queuedEmailRepository.BulkDelete(queuedEmails);
        }

        /// <summary>
        /// Gets a queued email by identifier
        /// </summary>
        /// <param name="queuedEmailId">Queued email identifier</param>
        /// <returns>Queued email</returns>
        public virtual QueuedEmail GetQueuedEmailById(int queuedEmailId)
        {
            if (queuedEmailId == 0)
                return null;

            return _queuedEmailRepository.ToCachedGetById(queuedEmailId);
        }

        /// <summary>
        /// Get queued emails by identifiers
        /// </summary>
        /// <param name="queuedEmailIds">queued email identifiers</param>
        /// <returns>Queued emails</returns>
        public virtual IList<QueuedEmail> GetQueuedEmailsByIds(int[] queuedEmailIds)
        {
            if (queuedEmailIds == null || queuedEmailIds.Length == 0)
                return new List<QueuedEmail>();

            var query = from qe in _queuedEmailRepository.GetAll()
                        where queuedEmailIds.Contains(qe.Id)
                        select qe;
            var queuedEmails = query.ToList();
            //sort by passed identifiers
            var sortedQueuedEmails = new List<QueuedEmail>();
            foreach (var id in queuedEmailIds)
            {
                var queuedEmail = queuedEmails.Find(x => x.Id == id);
                if (queuedEmail != null)
                    sortedQueuedEmails.Add(queuedEmail);
            }

            return sortedQueuedEmails;
        }

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="fromEmail">From Email</param>
        /// <param name="toEmail">To Email</param>
        /// <param name="createdFrom">Created date from (); null to load all records</param>
        /// <param name="createdTo">Created date to (); null to load all records</param>
        /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="loadOnlyItemsToBeSent">A value indicating whether to load only emails for ready to be sent</param>
        /// <param name="maxSendTries">Maximum send tries</param>
        /// <param name="loadNewest">A value indicating whether we should sort queued email descending; otherwise, ascending.</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Email item list</returns>
        public virtual IPagedList<QueuedEmail> SearchEmails(string fromEmail,
            string toEmail, DateTime? createdFrom, DateTime? createdTo,
            bool loadNotSentItemsOnly, bool loadOnlyItemsToBeSent, int maxSendTries,
            bool loadNewest, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            fromEmail = (fromEmail ?? string.Empty).Trim();
            toEmail = (toEmail ?? string.Empty).Trim();

            var query = _queuedEmailRepository.GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(fromEmail))
                query = query.Where(qe => qe.From.Contains(fromEmail));
            if (!string.IsNullOrEmpty(toEmail))
                query = query.Where(qe => qe.To.Contains(toEmail));
            if (createdFrom.HasValue)
                query = query.Where(qe => qe.CreatedOn >= createdFrom);
            if (createdTo.HasValue)
                query = query.Where(qe => qe.CreatedOn <= createdTo);
            if (loadNotSentItemsOnly)
                query = query.Where(qe => !qe.SentOn.HasValue);
            if (loadOnlyItemsToBeSent)
            {
                var now = DateTime.Now;
                query = query.Where(qe => !qe.DontSendBeforeDate.HasValue || qe.DontSendBeforeDate.Value <= now);
            }

            query = query.Where(qe => qe.SentTries < maxSendTries);
            query = loadNewest ?
                //load the newest records
                query.OrderByDescending(qe => qe.CreatedOn) :
                //load by priority
                query.OrderByDescending(qe => qe.PriorityId).ThenBy(qe => qe.CreatedOn);

            var queuedEmails = new PagedList<QueuedEmail>(query, pageIndex, pageSize);
            return queuedEmails;
        }

        /// <summary>
        /// Deletes already sent emails
        /// </summary>
        /// <param name="createdFrom">Created date from (); null to load all records</param>
        /// <param name="createdTo">Created date to (); null to load all records</param>
        /// <returns>Number of deleted emails</returns>
        public virtual int DeleteAlreadySentEmails(DateTime? createdFrom, DateTime? createdTo)
        {
            var query = _queuedEmailRepository.GetAll();

            // only sent emails
            query = query.Where(qe => qe.SentOn.HasValue);

            if (createdFrom.HasValue)
                query = query.Where(qe => qe.CreatedOn >= createdFrom);
            if (createdTo.HasValue)
                query = query.Where(qe => qe.CreatedOn <= createdTo);

            var emails = query.ToArray();

            DeleteQueuedEmails(emails);

            return emails.Length;
        }

        /// <summary>
        /// Delete all queued emails
        /// </summary>
        public virtual void DeleteAllEmails()
        {
            //var bb = context.QueuedEmails.FromSql("truncate table QueuedEmails", pTotalRecords);
        }

        #endregion
    }
}