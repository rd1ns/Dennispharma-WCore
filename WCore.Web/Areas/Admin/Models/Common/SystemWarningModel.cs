using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Common
{
    public partial class SystemWarningModel : BaseWCoreModel
    {
        public SystemWarningLevel Level { get; set; }

        public string Text { get; set; }

        public bool DontEncode { get; set; }
    }

    public enum SystemWarningLevel
    {
        Pass,
        Recommendation,
        CopyrightRemovalKey,
        Warning,
        Fail
    }
}