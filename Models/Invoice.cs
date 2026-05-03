namespace StockFlowERP.Models;

public class Invoice : EntityBase
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid SalesOrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Issued";
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
}
