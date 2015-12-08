﻿using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart
{
    public class ShoppingCart : Entity
    {
        public ShoppingCart(string storeId, string customerId, string customerName, string name, string currencyCode)
        {
            var currency = EnumUtility.SafeParse(currencyCode, CurrencyCodes.USD);

            Currency = new Currency(currency);
            CustomerId = customerId;
            CustomerName = customerName;
            Name = name;
            StoreId = storeId;

            Addresses = new List<Address>();
            Discounts = new List<Discount>();
            Items = new List<LineItem>();
            Payments = new List<Payment>();
            Shipments = new List<Shipment>();
            TaxDetails = new List<TaxDetail>();
            Errors = new List<string>();

            DiscountTotal = new Money(0, currency);
            HandlingTotal = new Money(0, currency);
            ShippingTotal = new Money(0, currency);
            SubTotal = new Money(0, currency);
            TaxTotal = new Money(0, currency);
            Total = new Money(0, currency);
        }

        /// <summary>
        /// Gets or sets the value of shopping cart name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the value of channel id
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets the sign that shopping cart contains line items which require shipping
        /// </summary>
        public bool HasPhysicalProducts
        {
            get
            {
                return Items.Any(i => i.ProductType.Equals("Physical", StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Gets or sets the flag of shopping cart is anonymous
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer name
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart organization id
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        public Currency Currency { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart coupon
        /// </summary>
        /// <value>
        /// Coupon object
        /// </value>
        public Coupon Coupon { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart language code
        /// </summary>
        /// <value>
        /// Culture name in ISO 3166-1 alpha-3 format
        /// </value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart has tax
        /// </summary>
        public bool TaxIncluded { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart is recurring
        /// </summary>
        public bool IsRecuring { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart text comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the value of volumetric weight
        /// </summary>
        public decimal VolumetricWeight { get; set; }

        /// <summary>
        /// Gets or sets the value of weight unit
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart weight
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of measurement unit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of height
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// Gets or sets the value of length
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Gets or sets the value of width
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart total cost
        /// </summary>
        public Money Total { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart subtotal
        /// </summary>
        public Money SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping total cost
        /// </summary>
        public Money ShippingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of handling total cost
        /// </summary>
        public Money HandlingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total discount amount
        /// </summary>
        public Money DiscountTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total tax cost
        /// </summary>
        public Money TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart addresses
        /// </summary>
        /// <value>
        /// Collection of Address objects
        /// </value>
        public ICollection<Address> Addresses { get; set; }

        /// <summary>
        /// Gets default shipping address
        /// </summary>
        public Address DefaultShippingAddress
        {
            get
            {
                Address shippingAddress = null;

                if (!HasPhysicalProducts)
                {
                    return shippingAddress;
                }

                shippingAddress = Addresses.FirstOrDefault(a => a.Type == AddressType.Shipping);
                if (shippingAddress == null)
                {
                    shippingAddress = Addresses.FirstOrDefault();
                    if (shippingAddress == null)
                    {
                        shippingAddress = new Address
                        {
                            Type = AddressType.Shipping
                        };
                    }
                }

                return shippingAddress;
            }
        }

        /// <summary>
        /// Gets default billing address
        /// </summary>
        public Address DefaultBillingAddress
        {
            get
            {
                var billingAddress = Addresses.FirstOrDefault(a => a.Type == AddressType.Billing);
                if (billingAddress == null)
                {
                    billingAddress = Addresses.FirstOrDefault();
                    if (billingAddress == null)
                    {
                        billingAddress = new Address
                        {
                            Type = AddressType.Billing
                        };
                    }
                }

                return billingAddress;
            }
        }

        /// <summary>
        /// Gets or sets the value of shopping cart line items
        /// </summary>
        /// <value>
        /// Collection of LineItem objects
        /// </value>
        public ICollection<LineItem> Items { get; set; }

        /// <summary>
        /// Gets shopping cart items quantity (sum of each line item quantity * items count)
        /// </summary>
        public int ItemsCount
        {
            get
            {
                return Items.Sum(i => i.Quantity);
            }
        }

        /// <summary>
        /// Gets or sets the collection of shopping cart payments
        /// </summary>
        /// <value>
        /// Collection of Payment objects
        /// </value>
        public ICollection<Payment> Payments { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart shipments
        /// </summary>
        /// <value>
        /// Collection of Shipment objects
        /// </value>
        public ICollection<Shipment> Shipments { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart discounts
        /// </summary>
        /// <value>
        /// Collection of Discount objects
        /// </value>
        public ICollection<Discount> Discounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item tax detalization lines
        /// </summary>
        /// <value>
        /// Collection of TaxDetail objects
        /// </value>
        public ICollection<TaxDetail> TaxDetails { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart errors
        /// </summary>
        public ICollection<string> Errors { get; set; }
    }
}