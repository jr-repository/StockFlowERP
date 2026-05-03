namespace StockFlowERP.Models;

public class PurchaseOrder : EntityBase
{
    public string OrderNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public Guid WarehouseId { get; set; }
    public List<PurchaseOrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Received";
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}

public class PurchaseOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal Subtotal { get; set; }
}
