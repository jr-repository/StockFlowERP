using StockFlowERP.Contracts;
using StockFlowERP.Models;
using StockFlowERP.Repositories;

namespace StockFlowERP.Services;

public record SalesOrderResult(SalesOrder SalesOrder, Invoice Invoice);

public class SalesService
{
    private readonly IJsonRepository<SalesOrder> _salesOrders;
    private readonly IJsonRepository<Invoice> _invoices;
    private readonly IJsonRepository<Customer> _customers;
    private readonly IJsonRepository<Warehouse> _warehouses;
    private readonly IJsonRepository<Product> _products;
    private readonly IJsonRepository<StockMovement> _stockMovements;

    public SalesService(
        IJsonRepository<SalesOrder> salesOrders,
        IJsonRepository<Invoice> invoices,
        IJsonRepository<Customer> customers,
        IJsonRepository<Warehouse> warehouses,
        IJsonRepository<Product> products,
        IJsonRepository<StockMovement> stockMovements)
    {
        _salesOrders = salesOrders;
        _invoices = invoices;
        _customers = customers;
        _warehouses = warehouses;
        _products = products;
        _stockMovements = stockMovements;
    }

    public Task<List<SalesOrder>> GetSalesOrdersAsync() => _salesOrders.GetAllAsync();

    public Task<SalesOrder?> GetSalesOrderAsync(Guid id) => _salesOrders.GetByIdAsync(id);

    public Task<List<Invoice>> GetInvoicesAsync() => _invoices.GetAllAsync();

    public Task<Invoice?> GetInvoiceAsync(Guid id) => _invoices.GetByIdAsync(id);

    public async Task<SalesOrderResult> CreateAsync(CreateSalesOrderRequest request)
    {
        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("Sales order must contain at least one item.");
        }

        var customer = await _customers.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        var warehouse = await _warehouses.GetByIdAsync(request.WarehouseId);

        if (warehouse is null)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var productsToUpdate = new List<Product>();
        var order = new SalesOrder
        {
            OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMddHHmmss}",
            CustomerId = request.CustomerId,
            WarehouseId = request.WarehouseId,
            Items = new List<SalesOrderItem>()
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

            if (!product.IsActive)
            {
                throw new InvalidOperationException($"Product is inactive: {product.Name}");
            }

            if (product.QuantityOnHand < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product: {product.Name}");
            }

            var subtotal = item.Quantity * product.UnitPrice;

            order.Items.Add(new SalesOrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.UnitPrice,
                Subtotal = subtotal
            });

            order.TotalAmount += subtotal;

            product.QuantityOnHand -= item.Quantity;
            productsToUpdate.Add(product);
        }

        foreach (var product in productsToUpdate)
        {
            await _products.UpdateAsync(product.Id, product);
        }

        var savedOrder = await _salesOrders.AddAsync(order);

        foreach (var item in savedOrder.Items)
        {
            await _stockMovements.AddAsync(new StockMovement
            {
                ProductId = item.ProductId,
                WarehouseId = savedOrder.WarehouseId,
                MovementType = "OUT",
                Quantity = item.Quantity,
                ReferenceType = "SalesOrder",
                ReferenceId = savedOrder.Id
            });
        }

        var invoice = await _invoices.AddAsync(new Invoice
        {
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}",
            SalesOrderId = savedOrder.Id,
            CustomerId = savedOrder.CustomerId,
            Amount = savedOrder.TotalAmount,
            Status = "Issued",
            IssuedAt = DateTime.UtcNow
        });

        return new SalesOrderResult(savedOrder, invoice);
    }
}
