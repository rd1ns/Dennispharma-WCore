using WCore.Core.Domain.Orders;

namespace WCore.Services.Orders
{
    /// <summary>
    /// Custom number formatter
    /// </summary>
    public partial class CustomNumberFormatter : ICustomNumberFormatter
    {
        #region Fields

        private readonly OrderSettings _orderSettings;

        #endregion

        #region Ctor

        public CustomNumberFormatter(OrderSettings orderSettings)
        {
            _orderSettings = orderSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate return request custom number
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        /// <returns>Custom number</returns>
        public virtual string GenerateReturnRequestCustomNumber(ReturnRequest returnRequest)
        {
            string customNumber;

            if (string.IsNullOrEmpty(_orderSettings.ReturnRequestNumberMask))
            {
                customNumber = returnRequest.Id.ToString();
            }
            else
            {
                customNumber = _orderSettings.ReturnRequestNumberMask
                    .Replace("{ID}", returnRequest.Id.ToString())
                    .Replace("{YYYY}", returnRequest.CreatedOn.ToString("yyyy"))
                    .Replace("{YY}", returnRequest.CreatedOn.ToString("yy"))
                    .Replace("{MM}", returnRequest.CreatedOn.ToString("MM"))
                    .Replace("{DD}", returnRequest.CreatedOn.ToString("dd"));

                ////if you need to use the format for the ID with leading zeros, use the following code instead of the previous one.
                ////mask for Id example {#:00000000}
                //var rgx = new Regex(@"{#:\d+}");
                //var match = rgx.Match(customNumber);
                //var maskForReplase = match.Value;
                //
                //rgx = new Regex(@"\d+");
                //match = rgx.Match(maskForReplase);
                //
                //var formatValue = match.Value;
                //if(!string.IsNullOrEmpty(formatValue) && !string.IsNullOrEmpty(maskForReplase))
                //    customNumber = customNumber.Replace(maskForReplase, returnRequest.Id.ToString(formatValue));
                //else
                //    customNumber = customNumber.Insert(0, $"{returnRequest.Id}-");
            }

            return customNumber;
        }

        /// <summary>
        /// Generate order custom number
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Custom number</returns>
        public virtual string GenerateOrderCustomNumber(Order order)
        {
            if (string.IsNullOrEmpty(_orderSettings.CustomOrderNumberMask))
                return order.Id.ToString();

            var customNumber = _orderSettings.CustomOrderNumberMask
                .Replace("{ID}", order.Id.ToString())
                .Replace("{YYYY}", order.CreatedOn.ToString("yyyy"))
                .Replace("{YY}", order.CreatedOn.ToString("yy"))
                .Replace("{MM}", order.CreatedOn.ToString("MM"))
                .Replace("{DD}", order.CreatedOn.ToString("dd")).Trim();

            ////if you need to use the format for the ID with leading zeros, use the following code instead of the previous one.
            ////mask for Id example {#:00000000}
            //var rgx = new Regex(@"{#:\d+}");
            //var match = rgx.Match(customNumber);
            //var maskForReplase = match.Value;

            //rgx = new Regex(@"\d+");
            //match = rgx.Match(maskForReplase);

            //var formatValue = match.Value;
            //if (!string.IsNullOrEmpty(formatValue) && !string.IsNullOrEmpty(maskForReplase))
            //    customNumber = customNumber.Replace(maskForReplase, order.Id.ToString(formatValue));
            //else
            //    customNumber = customNumber.Insert(0, $"{order.Id}-");

            return customNumber;
        }

        #endregion
    }
}