using System.Collections.Generic;
using WCore.Core;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Popup;

namespace WCore.Services.Popups
{
    public interface IPopupService : IRepository<Popup>
    {
        IPagedList<Popup> GetAllByFilters(string ShowUrl = "",
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
}
