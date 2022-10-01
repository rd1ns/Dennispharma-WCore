using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WCore.Framework.TagHelpers.Admin
{
    /// <summary>
    /// WCore-panel tag helper
    /// </summary>
    [HtmlTargetElement("WCore-panels", Attributes = ID_ATTRIBUTE_NAME)]
    public class WCorePanelsTagHelper : TagHelper
    {
        private const string ID_ATTRIBUTE_NAME = "id";

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
    }
}