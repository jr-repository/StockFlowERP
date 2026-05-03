namespace StockFlowERP.Contracts;

public record CreateCategoryRequest(string Name, string Description);
public record UpdateCategoryRequest(string Name, string Description);

public record CreateWarehouseRequest(string Name, string Location);
public record UpdateWarehouseRequest(string Name, string Location);

public record CreateProductRequest(
    string Name,
    string Sku,
    string Description,
    Guid CategoryId,
    decimal UnitPrice,
    decimal CostPrice,
    int QuantityOnHand,
    int LowStockThreshold,
    bool IsActive
);

public record UpdateProductRequest(
    string Name,
    string Sku,
    string Description,
    Guid CategoryId,
    decimal UnitPrice,
    decimal CostPrice,
    int QuantityOnHand,
    int LowStockThreshold,
    bool IsActive
);
