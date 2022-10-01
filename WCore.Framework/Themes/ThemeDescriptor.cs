using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WCore.Services.Plugins;

namespace WCore.Framework.Themes
{
    /// <summary>
    /// Represents a theme descriptor
    /// </summary>
    public class ThemeDescriptor : IDescriptor
    {
        public ThemeDescriptor()
        {
            LayoutTypes = new List<ThemeLayoutType>();
            ColorSchemes = new List<ThemeColorScheme>();
            HeaderTypes = new List<ThemeHeaderType>();
            PageTitles = new List<ThemePageTitle>();
            GalleryTypes = new List<ThemeGalleryType>();
            TemplateTypes = new List<TemplateType>();
        }
        /// <summary>
        /// Gets or sets the theme system name
        /// </summary>
        [JsonProperty(PropertyName = "SystemName")]
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the theme friendly name
        /// </summary>
        [JsonProperty(PropertyName = "FriendlyName")]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the theme supports RTL (right-to-left)
        /// </summary>
        [JsonProperty(PropertyName = "SupportRTL")]
        public bool SupportRtl { get; set; }

        /// <summary>
        /// Gets or sets the path to the preview image of the theme
        /// </summary>
        [JsonProperty(PropertyName = "PreviewImageUrl")]
        public string PreviewImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the preview text of the theme
        /// </summary>
        [JsonProperty(PropertyName = "PreviewText")]
        public string PreviewText { get; set; }

        [JsonProperty(PropertyName = "LayoutTypes")]
        public List<ThemeLayoutType> LayoutTypes { get; set; }

        [JsonProperty(PropertyName = "ColorSchemes")]
        public List<ThemeColorScheme> ColorSchemes { get; set; }
        [JsonProperty(PropertyName = "HeaderTypes")]
        public List<ThemeHeaderType> HeaderTypes { get; set; }
        [JsonProperty(PropertyName = "PageTitles")]
        public List<ThemePageTitle> PageTitles { get; set; }
        [JsonProperty(PropertyName = "GalleryTypes")]
        public List<ThemeGalleryType> GalleryTypes { get; set; }
        [JsonProperty(PropertyName = "TemplateTypes")]
        public List<TemplateType> TemplateTypes { get; set; }
    }

    public class ThemeColorScheme
    {
        [JsonProperty(PropertyName = "Color")]
        public string Color
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "CssPath")]
        public string CssPath
        {
            get;
            set;
        }

        public ThemeColorScheme()
        {
        }
    }
    public class ThemeGalleryType
    {
        [JsonProperty(PropertyName = "Type")]
        public string Type
        {
            get;
            set;
        }

        public ThemeGalleryType()
        {
        }
    }
    public class ThemeHeaderType
    {
        [JsonProperty(PropertyName = "Type")]
        public string Type
        {
            get;
            set;
        }

        public ThemeHeaderType()
        {
        }
    }
    public class ThemeLayoutType
    {
        [JsonProperty(PropertyName = "Type")]
        public string Type
        {
            get;
            set;
        }

        public ThemeLayoutType()
        {
        }
    }
    public class ThemePageTitle
    {
        [JsonProperty(PropertyName = "Type")]
        public string Type
        {
            get;
            set;
        }

        public ThemePageTitle()
        {
        }
    }
    public class TemplateTypeFeature
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name
        {
            get;
            set;
        }

        public TemplateTypeFeature()
        {
        }
    }
    public class TemplateType
    {
        [JsonProperty(PropertyName = "Features")]
        public List<TemplateTypeFeature> Features
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Type")]
        public string Type
        {
            get;
            set;
        }

        public TemplateType()
        {
            this.Features = new List<TemplateTypeFeature>();
        }
    }

}
