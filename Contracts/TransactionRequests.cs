namespace StockFlowERP.Contracts;

public record PurchaseOrderItemRequest(Guid ProductId, int Quantity, decimal UnitCost);
public record CreatePurchaseOrderRequest(Guid SupplierId, Guid WarehouseId, List<PurchaseOrderItemRequest> Items);

public record SalesOrderItemRequest(Guid ProductId, int Quantity);
public record CreateSalesOrderRequest(Guid CustomerId, Guid WarehouseId, List<SalesOrderItemRequest> Items);
