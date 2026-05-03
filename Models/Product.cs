namespace StockFlowERP.Models;

public class Product : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal CostPrice { get; set; }
    public int QuantityOnHand { get; set; }
    public int LowStockThreshold { get; set; }
    public bool IsActive { get; set; } = true;
}
