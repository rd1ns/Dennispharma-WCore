using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WCore.Framework.TagHelpers.Admin
{
    /// <summary>
    /// WCore-editor tag helper
    /// </summary>
    [HtmlTargetElement("WCore-editor", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class WCoreEditorTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string FormAttributeName = "asp-form-name";
        private const string AutoComplateAttributeName = "asp-autocomplate";
        private const string DisabledAttributeName = "asp-disabled";
        private const string RequiredAttributeName = "asp-required";
        private const string RenderFormControlClassAttributeName = "asp-render-form-control-class";
        private const string TemplateAttributeName = "asp-template";
        private const string PostfixAttributeName = "asp-postfix";
        private const string ValueAttributeName = "asp-value";
        private const string PlaceholderAttributeName = "placeholder";
        private const string InputTypeName = "asp-type";
        private const string IconName = "asp-input-icon";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Indicates whether the field is disabled
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public string IsDisabled { set; get; }

        /// <summary>
        /// Indicates whether the field is disabled
        /// </summary>
        [HtmlAttributeName(FormAttributeName)]
        public string FormName { set; get; }

        /// <summary>
        /// Indicates whether the field is disabled
        /// </summary>
        [HtmlAttributeName(AutoComplateAttributeName)]
        public string IsAutoComplate { set; get; }

        /// <summary>
        /// Indicates whether the field is required
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public string IsRequired { set; get; }

        /// <summary>
        /// Placeholder for the field
        /// </summary>
        [HtmlAttributeName(PlaceholderAttributeName)]
        public string Placeholder { set; get; }

        /// <summary>
        /// form-control-lg,form-control-sm,form-control-xl
        /// </summary>
        [HtmlAttributeName(InputTypeName)]
        public string InputType { set; get; }

        /// <summary>
        /// fa-user,fa-clock
        /// </summary>
        [HtmlAttributeName(IconName)]
        public string Icon { set; get; }

        /// <summary>
        /// Indicates whether the "form-control" class shold be added to the input
        /// </summary>
        [HtmlAttributeName(RenderFormControlClassAttributeName)]
        public string RenderFormControlClass { set; get; }

        /// <summary>
        /// Editor template for the field
        /// </summary>
        [HtmlAttributeName(TemplateAttributeName)]
        public string Template { set; get; }

        /// <summary>
        /// Postfix
        /// </summary>
        [HtmlAttributeName(PostfixAttributeName)]
        public string Postfix { set; get; }

        /// <summary>
        /// The value of the element
        /// </summary>
        [HtmlAttributeName(ValueAttributeName)]
        public string Value { set; get; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        public WCoreEditorTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

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

            //container for additional attributes
            var htmlAttributes = new Dictionary<string, object>();

            //set placeholder if exists
            if (!string.IsNullOrEmpty(Placeholder))
                htmlAttributes.Add("placeholder", Placeholder);

            //set placeholder if exists
            bool.TryParse(IsAutoComplate, out bool autocomplate);
            if (!autocomplate)
                htmlAttributes.Add("autocomplate", "off");

            //set value if exists
            if (!string.IsNullOrEmpty(Value))
                htmlAttributes.Add("value", Value);

            if (!string.IsNullOrEmpty(FormName))
                htmlAttributes.Add("form-name", FormName);

            //disabled attribute
            bool.TryParse(IsDisabled, out bool disabled);
            if (disabled)
                htmlAttributes.Add("disabled", "disabled");

            //required asterisk
            bool.TryParse(IsRequired, out bool required);
            if (required)
            {
                output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                output.PostElement.SetHtmlContent("    <div class=\"input-group-append\">"+
												  "      <span class=\"input-group-text border\">"+
												  "         <i class=\"fa fa-info icon-lg\"></i>"+
												  "      </span>"+
												  "    </div>"+
                                                  "</div>");
                htmlAttributes.Add("required", "");

            }

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //add form-control class
            bool.TryParse(RenderFormControlClass, out var renderFormControlClass);
            if (string.IsNullOrEmpty(RenderFormControlClass)
                || For.Metadata.ModelType.Name.Equals("String")
                || For.Metadata.ModelType.Name.Equals("Decimal")
                || For.Metadata.ModelType.Name.Equals("Int") || renderFormControlClass)
                htmlAttributes.Add("class", "form-control " + (string.IsNullOrEmpty(InputType) ? "" : InputType) + "");



            if (For.Metadata.ModelType.Name.Equals("Int"))
                htmlAttributes.Add("type", "number");
            //generate editor
            var pattern = @"(?=\[\w+\]\.)";
            if (!_htmlHelper.ViewData.ContainsKey(For.Name) && Regex.IsMatch(For.Name, pattern))
                _htmlHelper.ViewData.Add(For.Name, For.Model);

            var htmlOutput = _htmlHelper.Editor(For.Name, Template, new { htmlAttributes, postfix = Postfix });
            output.Content.SetHtmlContent(htmlOutput);
        }
    }
}