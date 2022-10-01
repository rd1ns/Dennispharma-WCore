﻿using System;
using System.Net;
using System.Text;
using WCore.Core;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Users;
using WCore.Core.Html;
using WCore.Services.Common;
using WCore.Services.Localization;

namespace WCore.Services.Catalog
{
    /// <summary>
    /// Product attribute formatter
    /// </summary>
    public partial class ProductAttributeFormatter : IProductAttributeFormatter
    {
        #region Fields

        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeService _productAttributeService;
        //private readonly ITaxService _taxService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly ShoppingCartSettings _shoppingCartSettings;

        #endregion

        #region Ctor

        public ProductAttributeFormatter(ICurrencyService currencyService,
            ILocalizationService localizationService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeParser productAttributeParser,
            IProductAttributeService productAttributeService,
            //ITaxService taxService,
            IWebHelper webHelper,
            IWorkContext workContext,
            ShoppingCartSettings shoppingCartSettings)
        {
            _currencyService = currencyService;
            _localizationService = localizationService;
            _priceCalculationService = priceCalculationService;
            _priceFormatter = priceFormatter;
            _productAttributeParser = productAttributeParser;
            _productAttributeService = productAttributeService;
            //_taxService = taxService;
            _webHelper = webHelper;
            _workContext = workContext;
            _shoppingCartSettings = shoppingCartSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Attributes</returns>
        public virtual string FormatAttributes(Product product, string attributesXml)
        {
            var user = _workContext.CurrentUser;
            return FormatAttributes(product, attributesXml, user);
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="user">User</param>
        /// <param name="separator">Separator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <param name="renderPrices">A value indicating whether to render prices</param>
        /// <param name="renderProductAttributes">A value indicating whether to render product attributes</param>
        /// <param name="renderGiftCardAttributes">A value indicating whether to render gift card attributes</param>
        /// <param name="allowHyperlinks">A value indicating whether to HTML hyperink tags could be rendered (if required)</param>
        /// <returns>Attributes</returns>
        public virtual string FormatAttributes(Product product, string attributesXml,
            User user, string separator = "<br />", bool htmlEncode = true, bool renderPrices = true,
            bool renderProductAttributes = true, bool renderGiftCardAttributes = true,
            bool allowHyperlinks = true)
        {
            var result = new StringBuilder();

            //attributes
            if (renderProductAttributes)
            {
                foreach (var attribute in _productAttributeParser.ParseProductProductAttributes(attributesXml))
                {
                    var productAttrubute = _productAttributeService.GetProductAttributeById(attribute.ProductAttributeId);
                    var attributeName = _localizationService.GetLocalized(productAttrubute, a => a.Name, _workContext.WorkingLanguage.Id);

                    //attributes without values
                    if (!attribute.ShouldHaveValues())
                    {
                        foreach (var value in _productAttributeParser.ParseValues(attributesXml, attribute.Id))
                        {
                            var formattedAttribute = string.Empty;
                            if (attribute.AttributeControlType == AttributeControlType.MultilineTextbox)
                            {
                                //encode (if required)
                                if (htmlEncode)
                                    attributeName = WebUtility.HtmlEncode(attributeName);

                                //we never encode multiline textbox input
                                formattedAttribute = $"{attributeName}: {HtmlHelper.FormatText(value, false, true, false, false, false, false)}";
                            }
                            else if (attribute.AttributeControlType == AttributeControlType.FileUpload)
                            {
                                //file upload
                            }
                            else
                            {
                                //other attributes (textbox, datepicker)
                                formattedAttribute = $"{attributeName}: {value}";

                                //encode (if required)
                                if (htmlEncode)
                                    formattedAttribute = WebUtility.HtmlEncode(formattedAttribute);
                            }

                            if (string.IsNullOrEmpty(formattedAttribute))
                                continue;

                            if (result.Length > 0)
                                result.Append(separator);
                            result.Append(formattedAttribute);
                        }
                    }
                    //product attribute values
                    else
                    {
                        foreach (var attributeValue in _productAttributeParser.ParseProductAttributeValues(attributesXml, attribute.Id))
                        {
                            var formattedAttribute = $"{attributeName}: {_localizationService.GetLocalized(attributeValue, a => a.Name, _workContext.WorkingLanguage.Id)}";

                            if (renderPrices)
                            {
                                if (attributeValue.PriceAdjustmentUsePercentage)
                                {
                                    if (attributeValue.PriceAdjustment > decimal.Zero)
                                    {
                                        formattedAttribute += string.Format(
                                                _localizationService.GetResource("FormattedAttributes.PriceAdjustment"),
                                                "+", attributeValue.PriceAdjustment.ToString("G29"), "%");
                                    }
                                    else if (attributeValue.PriceAdjustment < decimal.Zero)
                                    {
                                        formattedAttribute += string.Format(
                                                _localizationService.GetResource("FormattedAttributes.PriceAdjustment"),
                                                string.Empty, attributeValue.PriceAdjustment.ToString("G29"), "%");
                                    }
                                }
                                else
                                {
                                    var attributeValuePriceAdjustment = _priceCalculationService.GetProductAttributeValuePriceAdjustment(product, attributeValue, user);
                                    var priceAdjustmentBase = 100;
                                    var priceAdjustment = _currencyService.ConvertFromPrimaryStoreCurrency(priceAdjustmentBase, _workContext.WorkingCurrency);

                                    if (priceAdjustmentBase > decimal.Zero)
                                    {
                                        formattedAttribute += string.Format(
                                                _localizationService.GetResource("FormattedAttributes.PriceAdjustment"),
                                                "+", _priceFormatter.FormatPrice(priceAdjustment, false, false), string.Empty);
                                    }
                                    else if (priceAdjustmentBase < decimal.Zero)
                                    {
                                        formattedAttribute += string.Format(
                                                _localizationService.GetResource("FormattedAttributes.PriceAdjustment"),
                                                "-", _priceFormatter.FormatPrice(-priceAdjustment, false, false), string.Empty);
                                    }
                                }
                            }

                            //display quantity
                            if (_shoppingCartSettings.RenderAssociatedAttributeValueQuantity && attributeValue.AttributeValueType == AttributeValueType.AssociatedToProduct)
                            {
                                //render only when more than 1
                                if (attributeValue.Quantity > 1)
                                    formattedAttribute += string.Format(_localizationService.GetResource("ProductAttributes.Quantity"), attributeValue.Quantity);
                            }

                            //encode (if required)
                            if (htmlEncode)
                                formattedAttribute = WebUtility.HtmlEncode(formattedAttribute);

                            if (string.IsNullOrEmpty(formattedAttribute))
                                continue;

                            if (result.Length > 0)
                                result.Append(separator);
                            result.Append(formattedAttribute);
                        }
                    }
                }
            }

            //gift cards
            if (!renderGiftCardAttributes)
                return result.ToString();

            if (!product.IsGiftCard)
                return result.ToString();

            _productAttributeParser.GetGiftCardAttribute(attributesXml, out var giftCardRecipientName, out var giftCardRecipientEmail, out var giftCardSenderName, out var giftCardSenderEmail, out var _);

            //sender
            var giftCardFrom = product.GiftCardType == GiftCardType.Virtual ?
                string.Format(_localizationService.GetResource("GiftCardAttribute.From.Virtual"), giftCardSenderName, giftCardSenderEmail) :
                string.Format(_localizationService.GetResource("GiftCardAttribute.From.Physical"), giftCardSenderName);
            //recipient
            var giftCardFor = product.GiftCardType == GiftCardType.Virtual ?
                string.Format(_localizationService.GetResource("GiftCardAttribute.For.Virtual"), giftCardRecipientName, giftCardRecipientEmail) :
                string.Format(_localizationService.GetResource("GiftCardAttribute.For.Physical"), giftCardRecipientName);

            //encode (if required)
            if (htmlEncode)
            {
                giftCardFrom = WebUtility.HtmlEncode(giftCardFrom);
                giftCardFor = WebUtility.HtmlEncode(giftCardFor);
            }

            if (!string.IsNullOrEmpty(result.ToString()))
            {
                result.Append(separator);
            }

            result.Append(giftCardFrom);
            result.Append(separator);
            result.Append(giftCardFor);

            return result.ToString();
        }

        #endregion
    }
}