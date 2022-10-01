using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WCore.Core;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Shipping;
using WCore.Core.Domain.Tax;
using WCore.Core.Domain.Users;
using WCore.Services.Common;
using WCore.Services.Directory;
using WCore.Services.Events;
using WCore.Services.Logging;
using WCore.Services.Tax.Events;
using WCore.Services.Users;

namespace WCore.Services.Tax
{
    /// <summary>
    /// Tax service
    /// </summary>
    public partial class TaxService : ITaxService
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly UserSettings _userSettings;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IGeoLookupService _geoLookupService;
        private readonly ILogger _logger;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreContext _storeContext;
        private readonly ITaxPluginManager _taxPluginManager;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly ShippingSettings _shippingSettings;
        private readonly TaxSettings _taxSettings;

        #endregion

        #region Ctor

        public TaxService(AddressSettings addressSettings,
            UserSettings userSettings,
            IAddressService addressService,
            ICountryService countryService,
            IUserService userService,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            IGeoLookupService geoLookupService,
            ILogger logger,
            IStateProvinceService stateProvinceService,
            IStoreContext storeContext,
            ITaxPluginManager taxPluginManager,
            IWebHelper webHelper,
            IWorkContext workContext,
            ShippingSettings shippingSettings,
            TaxSettings taxSettings)
        {
            _addressSettings = addressSettings;
            _userSettings = userSettings;
            _addressService = addressService;
            _countryService = countryService;
            _userService = userService;
            _eventPublisher = eventPublisher;
            _genericAttributeService = genericAttributeService;
            _geoLookupService = geoLookupService;
            _logger = logger;
            _stateProvinceService = stateProvinceService;
            _storeContext = storeContext;
            _taxPluginManager = taxPluginManager;
            _webHelper = webHelper;
            _workContext = workContext;
            _shippingSettings = shippingSettings;
            _taxSettings = taxSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get a value indicating whether a user is consumer (a person, not a company) located in Europe Union
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        protected virtual bool IsEuConsumer(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Country country = null;

            //get country from billing address
            if (_addressSettings.CountryEnabled && _userService.GetUserShippingAddress(user) is Address billingAddress)
                country = _countryService.GetCountryByAddress(billingAddress);

            //get country specified during registration?
            if (country == null && _userSettings.CountryEnabled)
            {
                var countryId = _genericAttributeService.GetAttribute<User, int>(user.Id, WCoreUserDefaults.CountryIdAttribute);
                country = _countryService.GetCountryById(countryId);
            }

            //get country by IP address
            if (country == null)
            {
                var ipAddress = _webHelper.GetCurrentIpAddress();
                var countryIsoCode = _geoLookupService.LookupCountryIsoCode(ipAddress);
                country = _countryService.GetCountryByTwoLetterIsoCode(countryIsoCode);
            }

            //we cannot detect country
            if (country == null)
                return false;

            //outside EU
            if (!country.SubjectToVat)
                return false;

            //company (business) or consumer?
            var userVatStatus = (VatNumberStatus)_genericAttributeService.GetAttribute<int>(user, WCoreUserDefaults.VatNumberStatusIdAttribute);
            if (userVatStatus == VatNumberStatus.Valid)
                return false;

            //consumer
            return true;
        }

        /// <summary>
        /// Gets a default tax address
        /// </summary>
        /// <returns>Address</returns>
        protected virtual Address LoadDefaultTaxAddress()
        {
            var addressId = _taxSettings.DefaultTaxAddressId;

            return _addressService.GetAddressById(addressId);
        }

        /// <summary>
        /// Gets or sets a pickup point address for tax calculation
        /// </summary>
        /// <param name="pickupPoint">Pickup point</param>
        /// <returns>Address</returns>
        protected virtual Address LoadPickupPointTaxAddress(PickupPoint pickupPoint)
        {
            if (pickupPoint == null)
                throw new ArgumentNullException(nameof(pickupPoint));

            var country = _countryService.GetCountryByTwoLetterIsoCode(pickupPoint.CountryCode);
            var state = _stateProvinceService.GetStateProvinceByAbbreviation(pickupPoint.StateAbbreviation, country?.Id);

            return new Address
            {
                CountryId = country?.Id ?? 0,
                StateProvinceId = state?.Id ?? 0,
                County = pickupPoint.County,
                City = pickupPoint.City,
                Address1 = pickupPoint.Address,
                ZipPostalCode = pickupPoint.ZipPostalCode
            };
        }

        /// <summary>
        /// Prepare request to get tax rate
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="user">User</param>
        /// <param name="price">Price</param>
        /// <returns>Package for tax calculation</returns>
        protected virtual TaxRateRequest PrepareTaxRateRequest(Product product, int taxCategoryId, User user, decimal price)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var taxRateRequest = new TaxRateRequest
            {
                User = user,
                Product = product,
                Price = price,
                TaxCategoryId = taxCategoryId > 0 ? taxCategoryId : product?.TaxCategoryId ?? 0,
                CurrentStoreId = _storeContext.CurrentStore.Id
            };

            var basedOn = _taxSettings.TaxBasedOn;

            //new EU VAT rules starting January 1st 2015
            //find more info at http://ec.europa.eu/taxation_customs/taxation/vat/how_vat_works/telecom/index_en.htm#new_rules
            var overriddenBasedOn =
                //EU VAT enabled?
                _taxSettings.EuVatEnabled &&
                //telecommunications, broadcasting and electronic services?
                product != null && product.IsTelecommunicationsOrBroadcastingOrElectronicServices &&
                //January 1st 2015 passed? Yes, not required anymore
                //DateTime.Now > new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.) &&
                //Europe Union consumer?
                IsEuConsumer(user);
            if (overriddenBasedOn)
            {
                //We must charge VAT in the EU country where the user belongs (not where the business is based)
                basedOn = TaxBasedOn.BillingAddress;
            }

            //tax is based on pickup point address
            if (!overriddenBasedOn && _taxSettings.TaxBasedOnPickupPointAddress && _shippingSettings.AllowPickupInStore)
            {
                var pickupPoint = _genericAttributeService.GetAttribute<PickupPoint>(user,
                    WCoreUserDefaults.SelectedPickupPointAttribute, _storeContext.CurrentStore.Id);
                if (pickupPoint != null)
                {
                    taxRateRequest.Address = LoadPickupPointTaxAddress(pickupPoint);
                    return taxRateRequest;
                }
            }

            if (basedOn == TaxBasedOn.BillingAddress && user.BillingAddressId == null ||
                basedOn == TaxBasedOn.ShippingAddress && user.ShippingAddressId == null)
                basedOn = TaxBasedOn.DefaultAddress;

            switch (basedOn)
            {
                case TaxBasedOn.BillingAddress:
                    var billingAddress = _userService.GetUserBillingAddress(user);
                    taxRateRequest.Address = billingAddress;
                    break;
                case TaxBasedOn.ShippingAddress:
                    var shippingAddress = _userService.GetUserShippingAddress(user);
                    taxRateRequest.Address = shippingAddress;
                    break;
                case TaxBasedOn.DefaultAddress:
                default:
                    taxRateRequest.Address = LoadDefaultTaxAddress();
                    break;
            }

            return taxRateRequest;
        }

        /// <summary>
        /// Calculated price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="percent">Percent</param>
        /// <param name="increase">Increase</param>
        /// <returns>New price</returns>
        protected virtual decimal CalculatePrice(decimal price, decimal percent, bool increase)
        {
            if (percent == decimal.Zero)
                return price;

            decimal result;
            if (increase)
                result = price * (1 + percent / 100);
            else
                result = price - price / (100 + percent) * percent;

            return result;
        }

        /// <summary>
        /// Gets tax rate
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="user">User</param>
        /// <param name="price">Price (taxable value)</param>
        /// <param name="taxRate">Calculated tax rate</param>
        /// <param name="isTaxable">A value indicating whether a request is taxable</param>
        protected virtual void GetTaxRate(Product product, int taxCategoryId,
            User user, decimal price, out decimal taxRate, out bool isTaxable)
        {
            taxRate = decimal.Zero;
            isTaxable = true;

            //active tax provider
            var activeTaxProvider = _taxPluginManager.LoadPrimaryPlugin(user, _storeContext.CurrentStore.Id);
            if (activeTaxProvider == null)
                return;

            //tax request
            var taxRateRequest = PrepareTaxRateRequest(product, taxCategoryId, user, price);

            //tax exempt
            if (IsTaxExempt(product, taxRateRequest.User))
            {
                isTaxable = false;
            }

            //make EU VAT exempt validation (the European Union Value Added Tax)
            if (isTaxable &&
                _taxSettings.EuVatEnabled &&
                IsVatExempt(taxRateRequest.Address, taxRateRequest.User))
            {
                //VAT is not chargeable
                isTaxable = false;
            }

            //get tax rate
            var taxRateResult = activeTaxProvider.GetTaxRate(taxRateRequest);

            //tax rate is calculated, now consumers can adjust it
            _eventPublisher.Publish(new TaxRateCalculatedEvent(taxRateResult));

            if (taxRateResult.Success)
            {
                //ensure that tax is equal or greater than zero
                if (taxRateResult.TaxRate < decimal.Zero)
                    taxRateResult.TaxRate = decimal.Zero;

                taxRate = taxRateResult.TaxRate;
            }
            else if (_taxSettings.LogErrors)
            {
                foreach (var error in taxRateResult.Errors)
                {
                    _logger.Error($"{activeTaxProvider.PluginDescriptor.FriendlyName} - {error}", null, user);
                }
            }
        }

        #endregion

        #region Methods

        #region Product price

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="price">Price</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetProductPrice(Product product, decimal price,
            out decimal taxRate)
        {
            var user = _workContext.CurrentUser;
            return GetProductPrice(product, price, user, out taxRate);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="price">Price</param>
        /// <param name="user">User</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetProductPrice(Product product, decimal price,
            User user, out decimal taxRate)
        {
            var includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return GetProductPrice(product, price, includingTax, user, out taxRate);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetProductPrice(Product product, decimal price,
            bool includingTax, User user, out decimal taxRate)
        {
            var priceIncludesTax = _taxSettings.PricesIncludeTax;
            var taxCategoryId = 0;
            return GetProductPrice(product, taxCategoryId, price, includingTax,
                user, priceIncludesTax, out taxRate);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <param name="priceIncludesTax">A value indicating whether price already includes tax</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetProductPrice(Product product, int taxCategoryId,
            decimal price, bool includingTax, User user,
            bool priceIncludesTax, out decimal taxRate)
        {
            //no need to calculate tax rate if passed "price" is 0
            if (price == decimal.Zero)
            {
                taxRate = decimal.Zero;
                return taxRate;
            }

            GetTaxRate(product, taxCategoryId, user, price, out taxRate, out var isTaxable);

            if (priceIncludesTax)
            {
                //"price" already includes tax
                if (includingTax)
                {
                    //we should calculate price WITH tax
                    if (!isTaxable)
                    {
                        //but our request is not taxable
                        //hence we should calculate price WITHOUT tax
                        price = CalculatePrice(price, taxRate, false);
                    }
                }
                else
                {
                    //we should calculate price WITHOUT tax
                    price = CalculatePrice(price, taxRate, false);
                }
            }
            else
            {
                //"price" doesn't include tax
                if (includingTax)
                {
                    //we should calculate price WITH tax
                    //do it only when price is taxable
                    if (isTaxable)
                    {
                        price = CalculatePrice(price, taxRate, true);
                    }
                }
            }

            if (!isTaxable)
            {
                //we return 0% tax rate in case a request is not taxable
                taxRate = decimal.Zero;
            }

            //allowed to support negative price adjustments
            //if (price < decimal.Zero)
            //    price = decimal.Zero;

            return price;
        }

        #endregion

        #region Shipping price

        /// <summary>
        /// Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="user">User</param>
        /// <returns>Price</returns>
        public virtual decimal GetShippingPrice(decimal price, User user)
        {
            var includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return GetShippingPrice(price, includingTax, user);
        }

        /// <summary>
        /// Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <returns>Price</returns>
        public virtual decimal GetShippingPrice(decimal price, bool includingTax, User user)
        {
            return GetShippingPrice(price, includingTax, user, out var _);
        }

        /// <summary>
        /// Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetShippingPrice(decimal price, bool includingTax, User user, out decimal taxRate)
        {
            taxRate = decimal.Zero;

            if (!_taxSettings.ShippingIsTaxable)
            {
                return price;
            }

            var taxClassId = _taxSettings.ShippingTaxClassId;
            var priceIncludesTax = _taxSettings.ShippingPriceIncludesTax;
            return GetProductPrice(null, taxClassId, price, includingTax, user,
                priceIncludesTax, out taxRate);
        }

        #endregion

        #region Payment additional fee

        /// <summary>
        /// Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="user">User</param>
        /// <returns>Price</returns>
        public virtual decimal GetPaymentMethodAdditionalFee(decimal price, User user)
        {
            var includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return GetPaymentMethodAdditionalFee(price, includingTax, user);
        }

        /// <summary>
        /// Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <returns>Price</returns>
        public virtual decimal GetPaymentMethodAdditionalFee(decimal price, bool includingTax, User user)
        {
            return GetPaymentMethodAdditionalFee(price, includingTax, user, out var _);
        }

        /// <summary>
        /// Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetPaymentMethodAdditionalFee(decimal price, bool includingTax, User user, out decimal taxRate)
        {
            taxRate = decimal.Zero;

            if (!_taxSettings.PaymentMethodAdditionalFeeIsTaxable)
            {
                return price;
            }

            var taxClassId = _taxSettings.PaymentMethodAdditionalFeeTaxClassId;
            var priceIncludesTax = _taxSettings.PaymentMethodAdditionalFeeIncludesTax;
            return GetProductPrice(null, taxClassId, price, includingTax, user,
                priceIncludesTax, out taxRate);
        }

        #endregion

        #region Checkout attribute price

        /// <summary>
        /// Gets checkout attribute value price
        /// </summary>
        /// <param name="ca">Checkout attribute</param>
        /// <param name="cav">Checkout attribute value</param>
        /// <returns>Price</returns>
        public virtual decimal GetCheckoutAttributePrice(CheckoutAttribute ca, CheckoutAttributeValue cav)
        {
            var user = _workContext.CurrentUser;
            return GetCheckoutAttributePrice(ca, cav, user);
        }

        /// <summary>
        /// Gets checkout attribute value price
        /// </summary>
        /// <param name="ca">Checkout attribute</param>
        /// <param name="cav">Checkout attribute value</param>
        /// <param name="user">User</param>
        /// <returns>Price</returns>
        public virtual decimal GetCheckoutAttributePrice(CheckoutAttribute ca, CheckoutAttributeValue cav, User user)
        {
            var includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return GetCheckoutAttributePrice(ca, cav, includingTax, user);
        }

        /// <summary>
        /// Gets checkout attribute value price
        /// </summary>
        /// <param name="ca">Checkout attribute</param>
        /// <param name="cav">Checkout attribute value</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <returns>Price</returns>
        public virtual decimal GetCheckoutAttributePrice(CheckoutAttribute ca, CheckoutAttributeValue cav,
            bool includingTax, User user)
        {
            return GetCheckoutAttributePrice(ca, cav, includingTax, user, out var _);
        }

        /// <summary>
        /// Gets checkout attribute value price
        /// </summary>
        /// <param name="ca">Checkout attribute</param>
        /// <param name="cav">Checkout attribute value</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="user">User</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetCheckoutAttributePrice(CheckoutAttribute ca, CheckoutAttributeValue cav,
            bool includingTax, User user, out decimal taxRate)
        {
            if (cav == null)
                throw new ArgumentNullException(nameof(cav));

            taxRate = decimal.Zero;

            var price = cav.PriceAdjustment;
            if (ca.IsTaxExempt)
            {
                return price;
            }

            var priceIncludesTax = _taxSettings.PricesIncludeTax;
            var taxClassId = ca.TaxCategoryId;
            return GetProductPrice(null, taxClassId, price, includingTax, user,
                priceIncludesTax, out taxRate);
        }

        #endregion

        #region VAT

        /// <summary>
        /// Gets VAT Number status
        /// </summary>
        /// <param name="fullVatNumber">Two letter ISO code of a country and VAT number (e.g. GB 111 1111 111)</param>
        /// <returns>VAT Number status</returns>
        public virtual VatNumberStatus GetVatNumberStatus(string fullVatNumber)
        {
            return GetVatNumberStatus(fullVatNumber, out var _, out var _);
        }

        /// <summary>
        /// Gets VAT Number status
        /// </summary>
        /// <param name="fullVatNumber">Two letter ISO code of a country and VAT number (e.g. GB 111 1111 111)</param>
        /// <param name="name">Name (if received)</param>
        /// <param name="address">Address (if received)</param>
        /// <returns>VAT Number status</returns>
        public virtual VatNumberStatus GetVatNumberStatus(string fullVatNumber,
            out string name, out string address)
        {
            name = string.Empty;
            address = string.Empty;

            if (string.IsNullOrWhiteSpace(fullVatNumber))
                return VatNumberStatus.Empty;
            fullVatNumber = fullVatNumber.Trim();

            //GB 111 1111 111 or GB 1111111111
            //more advanced regex - http://codeigniter.com/wiki/European_Vat_Checker
            var r = new Regex(@"^(\w{2})(.*)");
            var match = r.Match(fullVatNumber);
            if (!match.Success)
                return VatNumberStatus.Invalid;
            var twoLetterIsoCode = match.Groups[1].Value;
            var vatNumber = match.Groups[2].Value;

            return GetVatNumberStatus(twoLetterIsoCode, vatNumber, out name, out address);
        }

        /// <summary>
        /// Gets VAT Number status
        /// </summary>
        /// <param name="twoLetterIsoCode">Two letter ISO code of a country</param>
        /// <param name="vatNumber">VAT number</param>
        /// <returns>VAT Number status</returns>
        public virtual VatNumberStatus GetVatNumberStatus(string twoLetterIsoCode, string vatNumber)
        {
            return GetVatNumberStatus(twoLetterIsoCode, vatNumber, out var _, out var _);
        }

        /// <summary>
        /// Gets VAT Number status
        /// </summary>
        /// <param name="twoLetterIsoCode">Two letter ISO code of a country</param>
        /// <param name="vatNumber">VAT number</param>
        /// <param name="name">Name (if received)</param>
        /// <param name="address">Address (if received)</param>
        /// <returns>VAT Number status</returns>
        public virtual VatNumberStatus GetVatNumberStatus(string twoLetterIsoCode, string vatNumber,
            out string name, out string address)
        {
            name = string.Empty;
            address = string.Empty;

            if (string.IsNullOrEmpty(twoLetterIsoCode))
                return VatNumberStatus.Empty;

            if (string.IsNullOrEmpty(vatNumber))
                return VatNumberStatus.Empty;

            if (_taxSettings.EuVatAssumeValid)
                return VatNumberStatus.Valid;

            if (!_taxSettings.EuVatUseWebService)
                return VatNumberStatus.Unknown;

            return DoVatCheck(twoLetterIsoCode, vatNumber, out name, out address, out var _);
        }

        /// <summary>
        /// Performs a basic check of a VAT number for validity
        /// </summary>
        /// <param name="twoLetterIsoCode">Two letter ISO code of a country</param>
        /// <param name="vatNumber">VAT number</param>
        /// <param name="name">Company name</param>
        /// <param name="address">Address</param>
        /// <param name="exception">Exception</param>
        /// <returns>VAT number status</returns>
        public virtual VatNumberStatus DoVatCheck(string twoLetterIsoCode, string vatNumber,
            out string name, out string address, out Exception exception)
        {
            name = string.Empty;
            address = string.Empty;

            if (vatNumber == null)
                vatNumber = string.Empty;
            vatNumber = vatNumber.Trim().Replace(" ", string.Empty);

            if (twoLetterIsoCode == null)
                twoLetterIsoCode = string.Empty;
            if (!string.IsNullOrEmpty(twoLetterIsoCode))
                //The service returns INVALID_INPUT for country codes that are not uppercase.
                twoLetterIsoCode = twoLetterIsoCode.ToUpper();

            name = address = string.Empty;
            exception = null;
            return VatNumberStatus.Unknown;
        }

        #endregion

        #region Exempts

        /// <summary>
        /// Gets a value indicating whether a product is tax exempt
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="user">User</param>
        /// <returns>A value indicating whether a product is tax exempt</returns>
        public virtual bool IsTaxExempt(Product product, User user)
        {
            if (user != null)
            {
                if (user.IsTaxExempt)
                    return true;
            }

            if (product == null)
            {
                return false;
            }

            if (product.IsTaxExempt)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether EU VAT exempt (the European Union Value Added Tax)
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public virtual bool IsVatExempt(Address address, User user)
        {
            if (!_taxSettings.EuVatEnabled)
                return false;

            if (user == null || address == null)
                return false;

            var country = _countryService.GetCountryById(address.CountryId ?? 0);
            if (country == null)
                return false;

            if (!country.SubjectToVat)
                // VAT not chargeable if shipping outside VAT zone
                return true;

            // VAT not chargeable if address, user and config meet our VAT exemption requirements:
            // returns true if this user is VAT exempt because they are shipping within the EU but outside our shop country, they have supplied a validated VAT number, and the shop is configured to allow VAT exemption
            var userVatStatus = (VatNumberStatus)_genericAttributeService.GetAttribute<int>(user, WCoreUserDefaults.VatNumberStatusIdAttribute);
            return country.Id != _taxSettings.EuVatShopCountryId &&
                   userVatStatus == VatNumberStatus.Valid &&
                   _taxSettings.EuVatAllowVatExemption;
        }

        #endregion

        #region Tax total

        /// <summary>
        /// Get tax total for the passed shopping cart
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <param name="usePaymentMethodAdditionalFee">A value indicating whether we should use payment method additional fee when calculating tax</param>
        /// <returns>Result</returns>
        public virtual TaxTotalResult GetTaxTotal(IList<ShoppingCartItem> cart, bool usePaymentMethodAdditionalFee = true)
        {
            var user = _userService.GetShoppingCartUser(cart);
            var activeTaxProvider = _taxPluginManager.LoadPrimaryPlugin(user, _storeContext.CurrentStore.Id);
            if (activeTaxProvider == null)
                return null;

            //get result by using primary tax provider
            var taxTotalResult = activeTaxProvider.GetTaxTotal(new TaxTotalRequest
            {
                ShoppingCart = cart,
                User = user,
                StoreId = _storeContext.CurrentStore.Id,
                UsePaymentMethodAdditionalFee = usePaymentMethodAdditionalFee
            });

            //tax total is calculated, now consumers can adjust it
            _eventPublisher.Publish(new TaxTotalCalculatedEvent(taxTotalResult));

            //error logging
            if (taxTotalResult != null && !taxTotalResult.Success && _taxSettings.LogErrors)
            {
                foreach (var error in taxTotalResult.Errors)
                {
                    _logger.Error($"{activeTaxProvider.PluginDescriptor.FriendlyName} - {error}", null, user);
                }
            }

            return taxTotalResult;
        }

        #endregion

        #endregion
    }
}