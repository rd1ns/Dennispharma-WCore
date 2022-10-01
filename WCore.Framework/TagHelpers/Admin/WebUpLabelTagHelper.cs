using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WCore.Core;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Services.Localization;
using System;
using System.Net;

namespace WCore.Framework.TagHelpers.Admin
{
    /// <summary>
    /// WCore-label tag helper
    /// </summary>
    [HtmlTargetElement("WCore-label", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class WCoreLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string ColLGAttributeName = "asp-col-lg";
        private const string ColMDAttributeName = "asp-col-md";
        private const string ColSMAttributeName = "asp-col-sm";
        private const string DisplayHintAttributeName = "asp-display-hint";

        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// HtmlGenerator
        /// </summary>
        protected IHtmlGenerator Generator { get; set; }

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Indicates whether the hint should be displayed
        /// </summary>
        [HtmlAttributeName(DisplayHintAttributeName)]
        public bool DisplayHint { get; set; } = true;

        /// <summary>
        /// Indicates whether the hint should be displayed
        /// </summary>
        [HtmlAttributeName(ColLGAttributeName)]
        public int ColLG { get; set; } = 2;

        /// <summary>
        /// Indicates whether the hint should be displayed
        /// </summary>
        [HtmlAttributeName(ColMDAttributeName)]
        public int ColMD { get; set; } = 6;

        /// <summary>
        /// Indicates whether the hint should be displayed
        /// </summary>
        [HtmlAttributeName(ColSMAttributeName)]
        public int ColSM { get; set; } = 12;

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="generator">HTML generator</param>
        /// <param name="workContext">Work context</param>
        /// <param name="localizationService">Localization service</param>
        public WCoreLabelTagHelper(IHtmlGenerator generator, IWorkContext workContext, ILocalizationService localizationService)
        {
            Generator = generator;
            _workContext = workContext;
            _localizationService = localizationService;
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

            //generate label
            var tagBuilder = Generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, null, new { @class = "control-label" });
            if (tagBuilder != null)
            {
                //create a label wrapper
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                //merge classes
                var classValue = output.Attributes.ContainsName("class")
                                    ? $"{output.Attributes["class"].Value} label-wrapper"
                                    : "col-form-label col-lg-" + ColLG + " col-md-" + ColMD + " col-sm-" + ColSM + " text-lg-right text-left";
                output.Attributes.SetAttribute("class", classValue);

                //add label
                output.Content.SetHtmlContent(tagBuilder);

                var ctName = For.Metadata.ContainerType.FullName;

                var fullName = For.Metadata.ContainerType.FullName + "." + For.Name;

                //add hint
                if (For.Metadata.AdditionalValues.TryGetValue("WCoreResourceDisplayNameAttribute", out object value))
                {
                    var resourceDisplayName = value as WCoreResourceDisplayNameAttribute;
                    if (resourceDisplayName != null && DisplayHint)
                    {
                        var langId = _workContext.WorkingLanguage.Id;
                        var hintResource = _localizationService.GetResource(
                            resourceDisplayName.ResourceKey + ".Hint", langId, returnEmptyIfNotFound: true,
                            logIfNotFound: false);

                        if (!string.IsNullOrEmpty(hintResource))
                        {
                            var hintContent = $"<div class='float-right pl-3' title='{WebUtility.HtmlEncode(hintResource)}' data-toggle='tooltip' class='ico-help'><i class='fa fa-question-circle'></i></div>";
                            output.Content.AppendHtml(hintContent);
                        }
                    }
                }
            }
        }
    }
}