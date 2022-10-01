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
    /// WCore-tabs tag helper
    /// </summary>
    [HtmlTargetElement("WCore-tabs", Attributes = IdAttributeName)]
    public class WCoreTabsTagHelper : TagHelper
    {
        private const string IdAttributeName = "id";
        private const string TabNameToSelectAttributeName = "asp-tab-name-to-select";
        private const string RenderSelectedTabInputAttributeName = "asp-render-selected-tab-input";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Name of the tab which should be selected
        /// </summary>
        [HtmlAttributeName(TabNameToSelectAttributeName)]
        public string TabNameToSelect { set; get; }

        /// <summary>
        /// Indicates whether the tab is default
        /// </summary>
        [HtmlAttributeName(RenderSelectedTabInputAttributeName)]
        public string RenderSelectedTabInput { set; get; }

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
        public WCoreTabsTagHelper(IHtmlHelper htmlHelper)
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
            var tabContext = new List<WCoreTabContextItem>();
            context.Items.Add(typeof(WCoreTabsTagHelper), tabContext);

            //get tab name which should be selected
            //first try get tab name from query
            var tabNameToSelect = ViewContext.HttpContext.Request.Query["tabNameToSelect"];

            //then from attribute
            if (!string.IsNullOrEmpty(TabNameToSelect))
                tabNameToSelect = TabNameToSelect;

            //then save tab name in tab context to access it in tab item
            if (!string.IsNullOrEmpty(tabNameToSelect))
                context.Items.Add("tabNameToSelect", tabNameToSelect);

            //execute child tag helpers
            await output.GetChildContentAsync();

            //tabs title
            var tabsTitle = new TagBuilder("ul");
            tabsTitle.AddCssClass("nav");
            tabsTitle.AddCssClass("nav-tabs");
            tabsTitle.AddCssClass("nav-bold");
            tabsTitle.AddCssClass("nav-tabs-line");
            tabsTitle.AddCssClass("nav-tabs-line-3x");
            tabsTitle.AddCssClass("mb-5");

            //tabs content
            var tabsContent = new TagBuilder("div");
            tabsContent.AddCssClass("tab-content");

            foreach (var tabItem in tabContext)
            {
                tabsTitle.InnerHtml.AppendHtml(tabItem.Title);
                tabsContent.InnerHtml.AppendHtml(tabItem.Content);
            }

            //append data
            output.Content.AppendHtml(tabsTitle.RenderHtmlContent());
            output.Content.AppendHtml(tabsContent.RenderHtmlContent());

            bool.TryParse(RenderSelectedTabInput, out bool renderSelectedTabInput);
            if (string.IsNullOrEmpty(RenderSelectedTabInput) || renderSelectedTabInput)
            {
                //render input contains selected tab name
                var selectedTabInput = new TagBuilder("input");
                selectedTabInput.Attributes.Add("type", "hidden");
                selectedTabInput.Attributes.Add("id", "selected-tab-name");
                selectedTabInput.Attributes.Add("name", "selected-tab-name");
                selectedTabInput.Attributes.Add("value", _htmlHelper.GetSelectedTabName());
                output.PreContent.SetHtmlContent(selectedTabInput.RenderHtmlContent());

                //render tabs script
                if (output.Attributes.ContainsName("id"))
                {
                    var script = new TagBuilder("script");
                    script.InnerHtml.AppendHtml("$(document).ready(function () {bindBootstrapTabSelectEvent('" + output.Attributes["id"].Value + "', 'selected-tab-name');});");
                    output.PostContent.SetHtmlContent(script.RenderHtmlContent());
                }
            }

            output.TagName = "div";

            var itemClass = "nav-tabs-custom";
            //merge classes
            var classValue = output.Attributes.ContainsName("class")
                ? $"{output.Attributes["class"].Value} {itemClass}"
                : itemClass;
            output.Attributes.SetAttribute("class", classValue);
        }
    }

    /// <summary>
    /// "WCore-tab tag helper
    /// </summary>
    [HtmlTargetElement("WCore-tab", ParentTag = "WCore-tabs", Attributes = NameAttributeName)]
    public class WCoreTabTagHelper : TagHelper
    {
        private const string NameAttributeName = "asp-name";
        private const string TitleAttributeName = "asp-title";
        private const string DefaultAttributeName = "asp-default";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Title of the tab
        /// </summary>
        [HtmlAttributeName(TitleAttributeName)]
        public string Title { set; get; }

        /// <summary>
        /// Indicates whether the tab is default
        /// </summary>
        [HtmlAttributeName(DefaultAttributeName)]
        public string IsDefault { set; get; }

        /// <summary>
        /// Name of the tab
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
        public WCoreTabTagHelper(IHtmlHelper htmlHelper)
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

            bool.TryParse(IsDefault, out bool isDefaultTab);

            //get name of the tab should be selected
            var tabNameToSelect = context.Items.ContainsKey("tabNameToSelect")
                ? context.Items["tabNameToSelect"].ToString()
                : "";

            if (string.IsNullOrEmpty(tabNameToSelect))
                tabNameToSelect = _htmlHelper.GetSelectedTabName();

            if (string.IsNullOrEmpty(tabNameToSelect) && isDefaultTab)
                tabNameToSelect = Name;

            //tab title
            var tabTitle = new TagBuilder("li");
            var a = new TagBuilder("a")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("data-tab-name", Name),
                    new KeyValuePair<string, string>("href", $"#{Name}"),
                    new KeyValuePair<string, string>("data-toggle", "tab"),
                }
            };
            a.AddCssClass("nav-link");
            if (tabNameToSelect == Name)
            {
                a.AddCssClass("active");
            }
            a.InnerHtml.AppendHtml(Title);

            //merge classes
            if (context.AllAttributes.ContainsName("class"))
                tabTitle.Attributes.Add("class", context.AllAttributes["class"].Value.ToString());
            tabTitle.InnerHtml.AppendHtml(a.RenderHtmlContent());

            //tab content
            var tabContent = new TagBuilder("div");
            tabContent.AddCssClass("tab-pane");
            tabContent.AddCssClass("fade");
            tabContent.AddCssClass("px-7");
            tabContent.Attributes.Add("id", Name);
            tabContent.InnerHtml.AppendHtml(output.GetChildContentAsync().Result.GetContent());

            tabTitle.AddCssClass("nav-item mr-3");
            //active class
            var itemClass = string.Empty;
            if (tabNameToSelect == Name)
            {
                a.AddCssClass("active");
                tabTitle.AddCssClass("active");
                tabContent.AddCssClass("active");
                tabContent.AddCssClass("show");
            }

            //add to context
            var tabContext = (List<WCoreTabContextItem>)context.Items[typeof(WCoreTabsTagHelper)];
            tabContext.Add(new WCoreTabContextItem()
            {
                Title = tabTitle.RenderHtmlContent(),
                Content = tabContent.RenderHtmlContent(),
                IsDefault = isDefaultTab
            });

            //generate nothing
            output.SuppressOutput();
        }
    }

    /// <summary>
    /// Tab context item
    /// </summary>
    public class WCoreTabContextItem
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
        /// Is default tab
        /// </summary>
        public bool IsDefault { set; get; }
    }
}