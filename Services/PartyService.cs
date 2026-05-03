using StockFlowERP.Contracts;
using StockFlowERP.Models;
using StockFlowERP.Repositories;

namespace StockFlowERP.Services;

public class PartyService
{
    private readonly IJsonRepository<Supplier> _suppliers;
    private readonly IJsonRepository<Customer> _customers;

    public PartyService(IJsonRepository<Supplier> suppliers, IJsonRepository<Customer> customers)
    {
        _suppliers = suppliers;
        _customers = customers;
    }

    public Task<List<Supplier>> GetSuppliersAsync() => _suppliers.GetAllAsync();

    public Task<Supplier?> GetSupplierAsync(Guid id) => _suppliers.GetByIdAsync(id);

    public Task<Supplier> CreateSupplierAsync(CreateSupplierRequest request)
    {
        return _suppliers.AddAsync(new Supplier
        {
            Name = request.Name,
            ContactName = request.ContactName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        });
    }

    public Task<Supplier?> UpdateSupplierAsync(Guid id, UpdateSupplierRequest request)
    {
        return _suppliers.UpdateAsync(id, new Supplier
        {
            Name = request.Name,
            ContactName = request.ContactName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        });
    }

    public Task<bool> DeleteSupplierAsync(Guid id) => _suppliers.DeleteAsync(id);

    public Task<List<Customer>> GetCustomersAsync() => _customers.GetAllAsync();

    public Task<Customer?> GetCustomerAsync(Guid id) => _customers.GetByIdAsync(id);

    public Task<Customer> CreateCustomerAsync(CreateCustomerRequest request)
    {
        return _customers.AddAsync(new Customer
        {
            Name = request.Name,
            ContactName = request.ContactName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        });
    }

    public Task<Customer?> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request)
    {
        return _customers.UpdateAsync(id, new Customer
        {
            Name = request.Name,
            ContactName = request.ContactName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        });
    }

    public Task<bool> DeleteCustomerAsync(Guid id) => _customers.DeleteAsync(id);
}
