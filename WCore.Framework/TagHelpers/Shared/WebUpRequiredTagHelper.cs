using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WCore.Framework.TagHelpers.Shared
{
    /// <summary>
    /// WCore-required tag helper
    /// </summary>
    [HtmlTargetElement("WCore-required", TagStructure = TagStructure.WithoutEndTag)]
    public class WCoreRequiredTagHelper : TagHelper
    {
        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //clear the output
            output.SuppressOutput();

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "required");
            output.Content.SetContent("*");
        }
    }
}