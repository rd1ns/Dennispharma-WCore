using WCore.Core;
using WCore.Core.Domain.DynamicForms;

namespace WCore.Services.DynamicForms
{
    public interface IDynamicFormService : IRepository<DynamicForm>
    {
        IPagedList<DynamicForm> GetAllByFilters(DynamicFormType? DynamicFormType = null, bool? ShowOn = null, bool? IsActive = null, bool? Deleted = null, int skip = 0, int take = 10);
        DynamicForm GetByDynamicFormType(DynamicFormType DynamicFormType, bool? IsActive = null, bool? Deleted = null, bool? ShowOn = null);
    }
    public interface IDynamicFormElementService : IRepository<DynamicFormElement>
    {
        IPagedList<DynamicFormElement> GetAllByFilters(int DynamicFormId = 0);
    }
    public interface IDynamicFormRecordService : IRepository<DynamicFormRecord>
    {
        IPagedList<DynamicFormRecord> GetAllByFilters(int DynamicFormId = 0);
    }
}
