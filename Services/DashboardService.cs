using StockFlowERP.Models;
using StockFlowERP.Repositories;

namespace StockFlowERP.Services;

public class DashboardService
{
    private readonly IJsonRepository<Product> _products;
    private readonly IJsonRepository<Category> _categories;
    private readonly IJsonRepository<Warehouse> _warehouses;
    private readonly IJsonRepository<Supplier> _suppliers;
    private readonly IJsonRepository<Customer> _customers;
    private readonly IJsonRepository<PurchaseOrder> _purchaseOrders;
    private readonly IJsonRepository<SalesOrder> _salesOrders;
    private readonly IJsonRepository<Invoice> _invoices;
    private readonly IJsonRepository<StockMovement> _stockMovements;

    public DashboardService(
        IJsonRepository<Product> products,
        IJsonRepository<Category> categories,
        IJsonRepository<Warehouse> warehouses,
        IJsonRepository<Supplier> suppliers,
        IJsonRepository<Customer> customers,
        IJsonRepository<PurchaseOrder> purchaseOrders,
        IJsonRepository<SalesOrder> salesOrders,
        IJsonRepository<Invoice> invoices,
        IJsonRepository<StockMovement> stockMovements)
    {
        _products = products;
        _categories = categories;
        _warehouses = warehouses;
        _suppliers = suppliers;
        _customers = customers;
        _purchaseOrders = purchaseOrders;
        _salesOrders = salesOrders;
        _invoices = invoices;
        _stockMovements = stockMovements;
    }

    public async Task<object> GetSummaryAsync()
    {
        var products = await _products.GetAllAsync();
        var categories = await _categories.GetAllAsync();
        var warehouses = await _warehouses.GetAllAsync();
        var suppliers = await _suppliers.GetAllAsync();
        var customers = await _customers.GetAllAsync();
        var purchaseOrders = await _purchaseOrders.GetAllAsync();
        var salesOrders = await _salesOrders.GetAllAsync();
        var invoices = await _invoices.GetAllAsync();
        var stockMovements = await _stockMovements.GetAllAsync();

        var lowStockProducts = products
            .Where(product => product.QuantityOnHand <= product.LowStockThreshold)
            .Select(product => new
            {
                product.Id,
                product.Name,
                product.Sku,
                product.QuantityOnHand,
                product.LowStockThreshold
            })
            .ToList();

        return new
        {
            totals = new
            {
                products = products.Count,
                categories = categories.Count,
                warehouses = warehouses.Count,
                suppliers = suppliers.Count,
                customers = customers.Count,
                purchaseOrders = purchaseOrders.Count,
                salesOrders = salesOrders.Count,
                invoices = invoices.Count,
                stockMovements = stockMovements.Count
            },
            inventory = new
            {
                totalStock = products.Sum(product => product.QuantityOnHand),
                inventoryValue = products.Sum(product => product.QuantityOnHand * product.CostPrice),
                lowStockCount = lowStockProducts.Count,
                lowStockProducts
            },
            finance = new
            {
                purchaseTotal = purchaseOrders.Sum(order => order.TotalAmount),
                salesTotal = salesOrders.Sum(order => order.TotalAmount),
                invoiceTotal = invoices.Sum(invoice => invoice.Amount)
            }
        };
    }
}
