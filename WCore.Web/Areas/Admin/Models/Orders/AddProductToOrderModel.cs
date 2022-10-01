using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WCore.Core.Domain.Catalog;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a product model to add to the order
    /// </summary>
    public partial class AddProductToOrderModel : BaseWCoreModel
    {
        #region Ctor

        public AddProductToOrderModel()
        {
            ProductAttributes = new List<ProductAttributeModel>();
            GiftCard = new GiftCardModel();
            Warnings = new List<string>();
        }

        #endregion

        #region Properties

        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public ProductType ProductType { get; set; }

        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Orders.Products.AddNew.UnitPriceInclTax")]
        public decimal UnitPriceInclTax { get; set; }
        [WCoreResourceDisplayName("Admin.Orders.Products.AddNew.UnitPriceExclTax")]
        public decimal UnitPriceExclTax { get; set; }

        [WCoreResourceDisplayName("Admin.Orders.Products.AddNew.Quantity")]
        public int Quantity { get; set; }

        [WCoreResourceDisplayName("Admin.Orders.Products.AddNew.SubTotalInclTax")]
        public decimal SubTotalInclTax { get; set; }
        [WCoreResourceDisplayName("Admin.Orders.Products.AddNew.SubTotalExclTax")]
        public decimal SubTotalExclTax { get; set; }

        //product attributes
        public IList<ProductAttributeModel> ProductAttributes { get; set; }
        //gift card info
        public GiftCardModel GiftCard { get; set; }
        //rental
        public bool IsRental { get; set; }

        public List<string> Warnings { get; set; }

        /// <summary>
        /// A value indicating whether this attribute depends on some other attribute
        /// </summary>
        public bool HasCondition { get; set; }

        public bool AutoUpdateOrderTotals { get; set; }

        #endregion

        #region Nested classes
        
        public partial class ProductAttributeModel : BaseWCoreEntityModel
        {
            public ProductAttributeModel()
            {
                Values = new List<ProductAttributeValueModel>();
            }

            public int ProductAttributeId { get; set; }

            public string Name { get; set; }

            public string TextPrompt { get; set; }

            public bool IsRequired { get; set; }

            public bool HasCondition { get; set; }

            /// <summary>
            /// Allowed file extensions for user uploaded files
            /// </summary>
            public IList<string> AllowedFileExtensions { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<ProductAttributeValueModel> Values { get; set; }
        }

        public partial class ProductAttributeValueModel : BaseWCoreEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }

            public string PriceAdjustment { get; set; }

            public decimal PriceAdjustmentValue { get; set; }

            public bool UserEntersQty { get; set; }

            public int Quantity { get; set; }
        }

        public partial class GiftCardModel : BaseWCoreModel
        {
            public bool IsGiftCard { get; set; }

            [WCoreResourceDisplayName("Admin.GiftCards.Fields.RecipientName")]
            public string RecipientName { get; set; }
            [DataType(DataType.EmailAddress)]
            [WCoreResourceDisplayName("Admin.GiftCards.Fields.RecipientEmail")]
            public string RecipientEmail { get; set; }
            [WCoreResourceDisplayName("Admin.GiftCards.Fields.SenderName")]
            public string SenderName { get; set; }
            [DataType(DataType.EmailAddress)]
            [WCoreResourceDisplayName("Admin.GiftCards.Fields.SenderEmail")]
            public string SenderEmail { get; set; }
            [WCoreResourceDisplayName("Admin.GiftCards.Fields.Message")]
            public string Message { get; set; }

            public GiftCardType GiftCardType { get; set; }
        }

        #endregion
    }
}