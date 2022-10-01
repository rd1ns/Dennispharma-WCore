﻿using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Shipping;
using WCore.Services.Events;
using WCore.Services.Shipping.Pickup;
using WCore.Services.Shipping.Tracking;

namespace WCore.Services.Shipping
{
    /// <summary>
    /// Shipment service
    /// </summary>
    public partial class ShipmentService : IShipmentService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IPickupPluginManager _pickupPluginManager;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Shipment> _shipmentRepository;
        private readonly IRepository<ShipmentItem> _siRepository;
        private readonly IShippingPluginManager _shippingPluginManager;

        #endregion

        #region Ctor

        public ShipmentService(IEventPublisher eventPublisher,
            IPickupPluginManager pickupPluginManager,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Shipment> shipmentRepository,
            IRepository<ShipmentItem> siRepository,
            IShippingPluginManager shippingPluginManager)
        {
            _eventPublisher = eventPublisher;
            _pickupPluginManager = pickupPluginManager;
            _orderItemRepository = orderItemRepository;
            _shipmentRepository = shipmentRepository;
            _siRepository = siRepository;
            _shippingPluginManager = shippingPluginManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a shipment
        /// </summary>
        /// <param name="shipment">Shipment</param>
        public virtual void DeleteShipment(Shipment shipment)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            _shipmentRepository.Delete(shipment);

            //event notification
            _eventPublisher.EntityDeleted(shipment);
        }

        /// <summary>
        /// Search shipments
        /// </summary>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <param name="warehouseId">Warehouse identifier, only shipments with products from a specified warehouse will be loaded; 0 to load all orders</param>
        /// <param name="shippingCountryId">Shipping country identifier; 0 to load all records</param>
        /// <param name="shippingStateId">Shipping state identifier; 0 to load all records</param>
        /// <param name="shippingCounty">Shipping county; null to load all records</param>
        /// <param name="shippingCity">Shipping city; null to load all records</param>
        /// <param name="trackingNumber">Search by tracking number</param>
        /// <param name="loadNotShipped">A value indicating whether we should load only not shipped shipments</param>
        /// <param name="createdFrom">Created date from (UTC); null to load all records</param>
        /// <param name="createdTo">Created date to (UTC); null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Shipments</returns>
        public virtual IPagedList<Shipment> GetAllShipments(int vendorId = 0, int warehouseId = 0,
            int shippingCountryId = 0,
            int shippingStateId = 0,
            string shippingCounty = null,
            string shippingCity = null,
            string trackingNumber = null,
            bool loadNotShipped = false,
            DateTime? createdFrom = null, DateTime? createdTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _shipmentRepository.GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(trackingNumber))
                query = query.Where(s => s.TrackingNumber.Contains(trackingNumber));

            if (shippingCountryId > 0)
                query = query.Where(s => s.Order.PickupInStore ? s.Order.PickupAddress.CountryId == shippingCountryId
                                                               : s.Order.ShippingAddress.CountryId == shippingCountryId);

            if (shippingStateId > 0)
                query = query.Where(s => s.Order.PickupInStore ? s.Order.PickupAddress.StateProvinceId == shippingStateId
                                                               : s.Order.ShippingAddress.StateProvinceId == shippingStateId);

            if (!string.IsNullOrWhiteSpace(shippingCounty))
                query = query.Where(s => s.Order.PickupInStore ? s.Order.PickupAddress.County.Contains(shippingCounty)
                                                               : s.Order.ShippingAddress.County.Contains(shippingCounty));

            if (!string.IsNullOrWhiteSpace(shippingCity))
                query = query.Where(s => s.Order.PickupInStore ? s.Order.PickupAddress.City.Contains(shippingCity)
                                                               : s.Order.ShippingAddress.City.Contains(shippingCity));

            if (loadNotShipped)
                query = query.Where(s => !s.ShippedDate.HasValue);
            if (createdFrom.HasValue)
                query = query.Where(s => createdFrom.Value <= s.CreatedOn);
            if (createdTo.HasValue)
                query = query.Where(s => createdTo.Value >= s.CreatedOn);
            query = query.Where(s => s.Order != null && !s.Order.Deleted);
            if (vendorId > 0)
            {
                var queryVendorOrderItems = from orderItem in _orderItemRepository.GetAll().AsQueryable()
                                            where orderItem.Product.VendorId == vendorId
                                            select orderItem.Id;

                query = from s in query
                        where queryVendorOrderItems.Intersect(s.ShipmentItems.Select(si => si.OrderItemId)).Any()
                        select s;
            }

            if (warehouseId > 0)
            {
                query = from s in query
                        where s.ShipmentItems.Any(si => si.WarehouseId == warehouseId)
                        select s;
            }

            query = query.OrderByDescending(s => s.CreatedOn);

            var shipments = new PagedList<Shipment>(query, pageIndex, pageSize);
            return shipments;
        }

        /// <summary>
        /// Get shipment by identifiers
        /// </summary>
        /// <param name="shipmentIds">Shipment identifiers</param>
        /// <returns>Shipments</returns>
        public virtual IList<Shipment> GetShipmentsByIds(int[] shipmentIds)
        {
            if (shipmentIds == null || shipmentIds.Length == 0)
                return new List<Shipment>();

            var query = from o in _shipmentRepository.GetAll().AsQueryable()
                        where shipmentIds.Contains(o.Id)
                        select o;
            var shipments = query.ToList();
            //sort by passed identifiers
            var sortedOrders = new List<Shipment>();
            foreach (var id in shipmentIds)
            {
                var shipment = shipments.Find(x => x.Id == id);
                if (shipment != null)
                    sortedOrders.Add(shipment);
            }

            return sortedOrders;
        }

        /// <summary>
        /// Gets a shipment
        /// </summary>
        /// <param name="shipmentId">Shipment identifier</param>
        /// <returns>Shipment</returns>
        public virtual Shipment GetShipmentById(int shipmentId)
        {
            if (shipmentId == 0)
                return null;

            return _shipmentRepository.GetById(shipmentId);
        }

        /// <summary>
        /// Inserts a shipment
        /// </summary>
        /// <param name="shipment">Shipment</param>
        public virtual void InsertShipment(Shipment shipment)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            _shipmentRepository.Insert(shipment);

            //event notification
            _eventPublisher.EntityInserted(shipment);
        }

        /// <summary>
        /// Updates the shipment
        /// </summary>
        /// <param name="shipment">Shipment</param>
        public virtual void UpdateShipment(Shipment shipment)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            _shipmentRepository.Update(shipment);

            //event notification
            _eventPublisher.EntityUpdated(shipment);
        }

        /// <summary>
        /// Deletes a shipment item
        /// </summary>
        /// <param name="shipmentItem">Shipment item</param>
        public virtual void DeleteShipmentItem(ShipmentItem shipmentItem)
        {
            if (shipmentItem == null)
                throw new ArgumentNullException(nameof(shipmentItem));

            _siRepository.Delete(shipmentItem);

            //event notification
            _eventPublisher.EntityDeleted(shipmentItem);
        }

        /// <summary>
        /// Gets a shipment item
        /// </summary>
        /// <param name="shipmentItemId">Shipment item identifier</param>
        /// <returns>Shipment item</returns>
        public virtual ShipmentItem GetShipmentItemById(int shipmentItemId)
        {
            if (shipmentItemId == 0)
                return null;

            return _siRepository.GetById(shipmentItemId);
        }

        /// <summary>
        /// Inserts a shipment item
        /// </summary>
        /// <param name="shipmentItem">Shipment item</param>
        public virtual void InsertShipmentItem(ShipmentItem shipmentItem)
        {
            if (shipmentItem == null)
                throw new ArgumentNullException(nameof(shipmentItem));

            _siRepository.Insert(shipmentItem);

            //event notification
            _eventPublisher.EntityInserted(shipmentItem);
        }

        /// <summary>
        /// Updates the shipment item
        /// </summary>
        /// <param name="shipmentItem">Shipment item</param>
        public virtual void UpdateShipmentItem(ShipmentItem shipmentItem)
        {
            if (shipmentItem == null)
                throw new ArgumentNullException(nameof(shipmentItem));

            _siRepository.Update(shipmentItem);

            //event notification
            _eventPublisher.EntityUpdated(shipmentItem);
        }

        /// <summary>
        /// Get quantity in shipments. For example, get planned quantity to be shipped
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="ignoreShipped">Ignore already shipped shipments</param>
        /// <param name="ignoreDelivered">Ignore already delivered shipments</param>
        /// <returns>Quantity</returns>
        public virtual int GetQuantityInShipments(Product product, int warehouseId,
            bool ignoreShipped, bool ignoreDelivered)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //only products with "use multiple warehouses" are handled this way
            if (product.ManageInventoryMethod != ManageInventoryMethod.ManageStock)
                return 0;
            if (!product.UseMultipleWarehouses)
                return 0;

            const int cancelledOrderStatusId = (int)OrderStatus.Cancelled;

            var query = _siRepository.GetAll().AsQueryable();
            query = query.Where(si => !si.Shipment.Order.Deleted);
            query = query.Where(si => si.Shipment.Order.OrderStatusId != cancelledOrderStatusId);
            if (warehouseId > 0)
                query = query.Where(si => si.WarehouseId == warehouseId);
            if (ignoreShipped)
                query = query.Where(si => !si.Shipment.ShippedDate.HasValue);
            if (ignoreDelivered)
                query = query.Where(si => !si.Shipment.DeliveryDate.HasValue);

            var queryProductOrderItems = from orderItem in _orderItemRepository.GetAll().AsQueryable()
                                         where orderItem.ProductId == product.Id
                                         select orderItem.Id;
            query = from si in query
                    where queryProductOrderItems.Any(orderItemId => orderItemId == si.OrderItemId)
                    select si;

            //some null validation
            var result = Convert.ToInt32(query.Sum(si => (int?)si.Quantity));
            return result;
        }

        /// <summary>
        /// Get the tracker of the shipment
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <returns>Shipment tracker</returns>
        public virtual IShipmentTracker GetShipmentTracker(Shipment shipment)
        {
            if (!shipment.Order.PickupInStore)
            {
                var shippingRateComputationMethod = _shippingPluginManager
                    .LoadPluginBySystemName(shipment.Order.ShippingRateComputationMethodSystemName);
                return shippingRateComputationMethod?.ShipmentTracker;
            }
            else
            {
                var pickupPointProvider = _pickupPluginManager
                    .LoadPluginBySystemName(shipment.Order.ShippingRateComputationMethodSystemName);
                return pickupPointProvider?.ShipmentTracker;
            }
        }

        #endregion
    }
}