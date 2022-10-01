using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.DynamicForms;
using System.Data.Entity;
using System.Linq;
using WCore.Services.Caching;

namespace WCore.Services.DynamicForms
{
    public class DynamicFormService : Repository<DynamicForm>, IDynamicFormService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        public DynamicFormService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<DynamicForm> GetAllByFilters(DynamicFormType? DynamicFormType = null, bool? ShowOn = null, bool? IsActive = null, bool? Deleted = null, int skip = 0, int take = 10)
        {
            IQueryable<DynamicForm> recordsFiltered = context.Set<DynamicForm>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(CacheKeys.DynamicFormKeys.DynamicFormsAllCacheKey, DynamicFormType, ShowOn, IsActive, Deleted, skip, take);



            var result = _staticCacheManager.Get(key, () =>
            {
                int recordsFilteredCount = recordsFiltered.Count();

                if (DynamicFormType.HasValue)
                {
                    recordsFiltered = recordsFiltered.Where(a => a.DynamicFormType == DynamicFormType.Value);
                }
                if (ShowOn.HasValue)
                {
                    recordsFiltered = recordsFiltered.Where(a => a.ShowOn == ShowOn.Value);
                }
                if (IsActive.HasValue)
                {
                    recordsFiltered = recordsFiltered.Where(a => a.IsActive == IsActive.Value);
                }
                if (Deleted.HasValue)
                {
                    recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted.Value);
                }

                var data = recordsFiltered.Skip(skip)
                        .Take(take).ToList();

                return new PagedList<DynamicForm>(data, skip, take, recordsFilteredCount);
            });
            return result;

        }
        public DynamicForm GetByDynamicFormType(DynamicFormType DynamicFormType, bool? IsActive = null, bool? Deleted = null, bool? ShowOn = null)
        {
            IQueryable<DynamicForm> recordsFiltered = context.Set<DynamicForm>();

            recordsFiltered = recordsFiltered.Where(a => a.DynamicFormType == DynamicFormType);

            if (IsActive.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.ShowOn == ShowOn);

            var data = recordsFiltered.OrderByDescending(o => o.IsActive).FirstOrDefault(o => o.DynamicFormType == DynamicFormType);

            return data;
        }
    }
    public class DynamicFormElementService : Repository<DynamicFormElement>, IDynamicFormElementService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        public DynamicFormElementService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<DynamicFormElement> GetAllByFilters(int DynamicFormId = 0)
        {
            IQueryable<DynamicFormElement> recordsFiltered = context.Set<DynamicFormElement>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(CacheKeys.DynamicFormElementKeys.DynamicFormElementsAllCacheKey, DynamicFormId);


            recordsFiltered = recordsFiltered.Where(a => a.DynamicFormId == DynamicFormId);

            var result = _staticCacheManager.Get(key, () =>
            {
                int recordsFilteredCount = recordsFiltered.Count();

                int recordsTotalCount = context.Set<DynamicFormElement>().Count();

                var data = recordsFiltered.ToList();

                return new PagedList<DynamicFormElement>(data, 0, int.MaxValue, recordsFilteredCount);
            });
            return result;

        }
    }
    public class DynamicFormRecordService : Repository<DynamicFormRecord>, IDynamicFormRecordService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        public DynamicFormRecordService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<DynamicFormRecord> GetAllByFilters(int DynamicFormId = 0)
        {
            IQueryable<DynamicFormRecord> recordsFiltered = context.Set<DynamicFormRecord>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(CacheKeys.DynamicFormRecordKeys.DynamicFormRecordsAllCacheKey, DynamicFormId);

            recordsFiltered = recordsFiltered.Where(a => a.DynamicFormId == DynamicFormId);

            var result = _staticCacheManager.Get(key, () =>
            {
                int recordsFilteredCount = recordsFiltered.Count();

                var data = recordsFiltered.ToList();

                return new PagedList<DynamicFormRecord>(data, 0, int.MaxValue, recordsFilteredCount);
            });
            return result;

        }
    }
}
