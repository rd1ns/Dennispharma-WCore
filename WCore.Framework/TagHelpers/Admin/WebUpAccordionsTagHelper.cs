using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WCore.Framework.Extensions;

namespace WCore.Framework.TagHelpers.Admin
{
    /// <summary>
    /// WCore-accordions tag helper
    /// </summary>
    [HtmlTargetElement("WCore-accordions", Attributes = IdAttributeName)]
    public class WCoreAccordionsTagHelper : TagHelper
    {
        private const string IdAttributeName = "id";
        private const string AccordionNameToSelectAttributeName = "asp-accordion-name-to-select";
        private const string RenderSelectedAccordionInputAttributeName = "asp-render-selected-accordion-input";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Name of the accordion which should be selected
        /// </summary>
        [HtmlAttributeName(AccordionNameToSelectAttributeName)]
        public string AccordionNameToSelect { set; get; }

        /// <summary>
        /// Indicates whether the accordion is default
        /// </summary>
        [HtmlAttributeName(RenderSelectedAccordionInputAttributeName)]
        public string RenderSelectedAccordionInput { set; get; }

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
        public WCoreAccordionsTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        /// <returns>Result</returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //create context item
            var accordionContext = new List<WCoreAccordionContextItem>();
            context.Items.Add(typeof(WCoreAccordionsTagHelper), accordionContext);

            //get accordion name which should be selected
            //first try get accordion name from query
            var accordionNameToSelect = ViewContext.HttpContext.Request.Query["accordionNameToSelect"];

            //then from attribute
            if (!string.IsNullOrEmpty(AccordionNameToSelect))
                accordionNameToSelect = AccordionNameToSelect;

            //then save accordion name in accordion context to access it in accordion item
            if (!string.IsNullOrEmpty(accordionNameToSelect))
                context.Items.Add("accordionNameToSelect", accordionNameToSelect);

            //execute child tag helpers
            await output.GetChildContentAsync();

            //accordions title
            var accordionsTitle = new TagBuilder("div");
            accordionsTitle.AddCssClass("card");

            //accordions content
            var accordionsContent = new TagBuilder("div")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("id", $"{accordionNameToSelect}"),
                    new KeyValuePair<string, string>("data-parent", $"#{output.Attributes["id"].Value}")
                }
            };
            accordionsContent.AddCssClass("collapse show");

            foreach (var accordionItem in accordionContext)
            {
                accordionsTitle.InnerHtml.AppendHtml(accordionItem.Title);
                accordionsTitle.InnerHtml.AppendHtml(accordionsContent.RenderHtmlContent());
            }

            var aa = output.GetChildContentAsync().Result.GetContent();

            //append data
            output.Content.AppendHtml(accordionsTitle.RenderHtmlContent());

            bool.TryParse(RenderSelectedAccordionInput, out bool renderSelectedAccordionInput);
            if (string.IsNullOrEmpty(RenderSelectedAccordionInput) || renderSelectedAccordionInput)
            {
                //render input contains selected accordion name
                var selectedAccordionInput = new TagBuilder("input");
                selectedAccordionInput.Attributes.Add("type", "hidden");
                selectedAccordionInput.Attributes.Add("id", "selected-accordion-name");
                selectedAccordionInput.Attributes.Add("name", "selected-accordion-name");
                selectedAccordionInput.Attributes.Add("value", _htmlHelper.GetSelectedAccordionName());
                output.PreContent.SetHtmlContent(selectedAccordionInput.RenderHtmlContent());

                //render accordions script
                if (output.Attributes.ContainsName("id"))
                {
                    var script = new TagBuilder("script");
                    script.InnerHtml.AppendHtml("$(document).ready(function () {bindBootstrapAccordionSelectEvent('" + output.Attributes["id"].Value + "', 'selected-accordion-name');});");
                    output.PostContent.SetHtmlContent(script.RenderHtmlContent());
                }
            }

            output.TagName = "div";

            var itemClass = "accordion  accordion-toggle-arrow";
            //merge classes
            var classValue = output.Attributes.ContainsName("class")
                ? $"{output.Attributes["class"].Value} {itemClass}"
                : itemClass;
            output.Attributes.SetAttribute("class", classValue);
        }
    }

    /// <summary>
    /// "WCore-accordion tag helper
    /// </summary>
    [HtmlTargetElement("WCore-accordion", ParentTag = "WCore-accordions", Attributes = NameAttributeName)]
    public class WCoreAccordionTagHelper : TagHelper
    {
        private const string NameAttributeName = "asp-name";
        private const string TitleAttributeName = "asp-title";
        private const string DefaultAttributeName = "asp-default";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Title of the accordion
        /// </summary>
        [HtmlAttributeName(TitleAttributeName)]
        public string Title { set; get; }

        /// <summary>
        /// Indicates whether the accordion is default
        /// </summary>
        [HtmlAttributeName(DefaultAttributeName)]
        public string IsDefault { set; get; }

        /// <summary>
        /// Name of the accordion
        /// </summary>
        [HtmlAttributeName(NameAttributeName)]
        public string Name { set; get; }

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
        public WCoreAccordionTagHelper(IHtmlHelper htmlHelper)
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

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            bool.TryParse(IsDefault, out bool isDefaultAccordion);

            //get name of the accordion should be selected
            var accordionNameToSelect = context.Items.ContainsKey("accordionNameToSelect")
                ? context.Items["accordionNameToSelect"].ToString()
                : "";

            if (string.IsNullOrEmpty(accordionNameToSelect))
                accordionNameToSelect = _htmlHelper.GetSelectedAccordionName();

            if (string.IsNullOrEmpty(accordionNameToSelect) && isDefaultAccordion)
                accordionNameToSelect = Name;

            //accordion title
            var accordionTitle = new TagBuilder("div");
            var a = new TagBuilder("div")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("data-toggle", "collapse"),
                    new KeyValuePair<string, string>("data-target", $"#{accordionNameToSelect}")
                }
            };
            a.AddCssClass("card-title");
            if (accordionNameToSelect == Name)
            {
                a.AddCssClass("");
            }
            a.InnerHtml.AppendHtml("<i class=\"flaticon2-layers-1\"></i>"+Title);

            //merge classes
            if (context.AllAttributes.ContainsName("class"))
                accordionTitle.Attributes.Add("class", context.AllAttributes["class"].Value.ToString());
            accordionTitle.InnerHtml.AppendHtml(a.RenderHtmlContent());

            //accordion content
            var accordionContent = new TagBuilder("div");
            accordionContent.AddCssClass("accordion-pane");
            accordionContent.AddCssClass("fade");
            accordionContent.AddCssClass("px-7");
            accordionContent.Attributes.Add("id", Name);
            accordionContent.InnerHtml.AppendHtml(output.GetChildContentAsync().Result.GetContent());

            accordionTitle.AddCssClass("card-header");
            //active class
            var itemClass = string.Empty;
            if (accordionNameToSelect == Name)
            {
                a.AddCssClass("");
                accordionTitle.AddCssClass("");
                accordionContent.AddCssClass("");
                accordionContent.AddCssClass("show");
            }

            //add to context
            var accordionContext = (List<WCoreAccordionContextItem>)context.Items[typeof(WCoreAccordionsTagHelper)];
            accordionContext.Add(new WCoreAccordionContextItem()
            {
                Title = accordionTitle.RenderHtmlContent(),
                Content = accordionContent.RenderHtmlContent(),
                IsDefault = isDefaultAccordion
            });

            //generate nothing
            output.SuppressOutput();
        }
    }

    /// <summary>
    /// Accordion context item
    /// </summary>
    public class WCoreAccordionContextItem
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// Is default accordion
        /// </summary>
        public bool IsDefault { set; get; }
    }
}