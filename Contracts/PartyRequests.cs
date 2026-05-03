namespace StockFlowERP.Contracts;

public record CreateSupplierRequest(string Name, string ContactName, string Email, string Phone, string Address);
public record UpdateSupplierRequest(string Name, string ContactName, string Email, string Phone, string Address);

public record CreateCustomerRequest(string Name, string ContactName, string Email, string Phone, string Address);
public record UpdateCustomerRequest(string Name, string ContactName, string Email, string Phone, string Address);
