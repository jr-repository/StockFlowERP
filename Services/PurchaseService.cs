using StockFlowERP.Contracts;
using StockFlowERP.Models;
using StockFlowERP.Repositories;

namespace StockFlowERP.Services;

public class PurchaseService
{
    private readonly IJsonRepository<PurchaseOrder> _purchaseOrders;
    private readonly IJsonRepository<Supplier> _suppliers;
    private readonly IJsonRepository<Warehouse> _warehouses;
    private readonly IJsonRepository<Product> _products;
    private readonly IJsonRepository<StockMovement> _stockMovements;

    public PurchaseService(
        IJsonRepository<PurchaseOrder> purchaseOrders,
        IJsonRepository<Supplier> suppliers,
        IJsonRepository<Warehouse> warehouses,
        IJsonRepository<Product> products,
        IJsonRepository<StockMovement> stockMovements)
    {
        _purchaseOrders = purchaseOrders;
        _suppliers = suppliers;
        _warehouses = warehouses;
        _products = products;
        _stockMovements = stockMovements;
    }

    public Task<List<PurchaseOrder>> GetAllAsync() => _purchaseOrders.GetAllAsync();

    public Task<PurchaseOrder?> GetByIdAsync(Guid id) => _purchaseOrders.GetByIdAsync(id);

    public async Task<PurchaseOrder> CreateAsync(CreatePurchaseOrderRequest request)
    {
        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("Purchase order must contain at least one item.");
        }

        var supplier = await _suppliers.GetByIdAsync(request.SupplierId);

        if (supplier is null)
        {
            throw new InvalidOperationException("Supplier not found.");
        }

        var warehouse = await _warehouses.GetByIdAsync(request.WarehouseId);

        if (warehouse is null)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var order = new PurchaseOrder
        {
            OrderNumber = $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}",
            SupplierId = request.SupplierId,
            WarehouseId = request.WarehouseId,
            Items = new List<PurchaseOrderItem>()
        };

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException("Item quantity must be greater than zero.");
            }

            var product = await _products.GetByIdAsync(item.ProductId);

            if (product is null)
            {
                throw new InvalidOperationException($"Product not found: {item.ProductId}");
            }

            var subtotal = item.Quantity * item.UnitCost;

            order.Items.Add(new PurchaseOrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                Subtotal = subtotal
            });

            order.TotalAmount += subtotal;

            product.QuantityOnHand += item.Quantity;
            await _products.UpdateAsync(product.Id, product);
        }

        var savedOrder = await _purchaseOrders.AddAsync(order);

        foreach (var item in savedOrder.Items)
        {
            await _stockMovements.AddAsync(new StockMovement
            {
                ProductId = item.ProductId,
                WarehouseId = savedOrder.WarehouseId,
                MovementType = "IN",
                Quantity = item.Quantity,
                ReferenceType = "PurchaseOrder",
                ReferenceId = savedOrder.Id
            });
        }

        return savedOrder;
    }
}
