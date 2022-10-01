using EFCore.BulkExtensions;
using LinqToDB.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain;
using WCore.Core.Infrastructure;
using WCore.Services.Events;

namespace WCore.Services
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll(Func<IQueryable<T>, IQueryable<T>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        T GetById(object id, Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        T Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);
        int Count();
        void BulkInsert(IList<T> list);
        void BulkDelete(IList<T> list);
        void BulkUpdate(IList<T> list);
        void BulkInsertOrUpdate(IList<T> list);
        void BulkInsertOrUpdateOrDelete(IList<T> list);
    }
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly WCoreContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(WCoreContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll(Func<IQueryable<T>, IQueryable<T>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            IList<T> getEntity()
            {
                var query = func != null ? func(entities.AsQueryable()) : entities.AsQueryable();
                return query.ToList();
            }

            return getEntity();
        }
        public T GetById(object id, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            T getEntity()
            {
                return entities.AsNoTracking().FirstOrDefault(entity => entity.Id == Convert.ToInt32(id));
            }

            return getEntity();
        }
        public T Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();

            var _eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
            _eventPublisher.EntityInserted(entity);

            return entity;
        }
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            var dbEntity = entities.SingleOrDefault(s => s.Id == entity.Id);
            context.Entry(dbEntity).CurrentValues.SetValues(entity);
            context.SaveChanges();

            var _eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
            _eventPublisher.EntityUpdated(entity);
        }
        public void Delete(int id)
        {
            if (id == 0) throw new ArgumentNullException("entity");

            var entity = entities.SingleOrDefault(s => s.Id == id);
            context.Entry(entity).State = EntityState.Deleted;
            context.SaveChanges();

            var _eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
            _eventPublisher.EntityUpdated(entity);
        }
        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            entities.SingleOrDefault(s => s.Id == entity.Id);
            context.Entry(entity).State = EntityState.Deleted;
            context.SaveChanges();

            var _eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
            _eventPublisher.EntityUpdated(entity);
        }
        public int Count()
        {
            return entities.Count();
        }

        public void BulkInsert(IList<T> list)
        {
            context.BulkInsert(list);
        }
        public void BulkDelete(IList<T> list)
        {
            context.BulkDelete(list);

            foreach (var item in list)
            {
                var _eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
                _eventPublisher.EntityDeleted(item);
            }
        }
        public void BulkUpdate(IList<T> list)
        {
            context.BulkUpdate(list);

            foreach (var item in list)
            {
                var _eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();
                _eventPublisher.EntityUpdated(item);
            }
        }
        public void BulkInsertOrUpdate(IList<T> list)
        {
            context.BulkInsertOrUpdate(list);
        }
        public void BulkInsertOrUpdateOrDelete(IList<T> list)
        {
            context.BulkInsertOrUpdateOrDelete(list);
        }


    }
}
