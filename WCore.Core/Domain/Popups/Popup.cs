using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Popup
{
    public partial class Popup : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ShowUrl { get; set; }
        public int PopupTime { get; set; }
        public PopupShowType PopupShowType { get; set; }
        public PopupTimeType PopupTimeType { get; set; }

        public bool ShowOn { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }

    }

    public enum PopupShowType
    {
        AllPage = 0,
        Url = 1
    }
    public enum PopupTimeType
    {
        Hourly = 0,
        Dialy = 1,
        Weekly = 2
    }
}
