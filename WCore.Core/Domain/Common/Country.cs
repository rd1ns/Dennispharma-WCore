using System;
using System.Collections.Generic;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Shipping;
using WCore.Core.Domain.Users;

namespace WCore.Core.Domain.Common
{
    public class Country : BaseEntity, ILocalizedEntity
    {
        private ICollection<StateProvince> _stateProvinces;
        private ICollection<ShippingMethodCountry> _shippingMethodCountry;
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether billing is allowed to this country
        /// </summary>
        public bool AllowsBilling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shipping is allowed to this country
        /// </summary>
        public bool AllowsShipping { get; set; }

        /// <summary>
        /// Gets or sets the two letter ISO code
        /// </summary>
        public string TwoLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the three letter ISO code
        /// </summary>
        public string ThreeLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the numeric ISO code
        /// </summary>
        public int NumericIsoCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers in this country must be charged EU VAT
        /// </summary>
        public bool SubjectToVat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }

        public string ShortCode { get; set; }
        public string LanguageCode { get; set; }
        public string PhoneCode { get; set; }
        public string Flag { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the state/provinces
        /// </summary>
        public virtual ICollection<StateProvince> StateProvinces
        {
            get => _stateProvinces ?? (_stateProvinces = new List<StateProvince>());
            protected set => _stateProvinces = value;
        }

        /// <summary>
        /// Gets or sets the shipping method-country mappings
        /// </summary>
        public virtual ICollection<ShippingMethodCountry> ShippingMethodCountryMappings
        {
            get => _shippingMethodCountry ?? (_shippingMethodCountry = new List<ShippingMethodCountry>());
            protected set => _shippingMethodCountry = value;
        }
    }
    public class City : BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }
        public string PlaqueCode { get; set; }
        public string PhoneCode { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
    }
    public class StateProvince : BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation
        /// </summary>
        public string Abbreviation { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }

}