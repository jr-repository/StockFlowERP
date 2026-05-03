using StockFlowERP.Contracts;
using StockFlowERP.Models;
using StockFlowERP.Repositories;

namespace StockFlowERP.Services;

public class CatalogService
{
    private readonly IJsonRepository<Category> _categories;
    private readonly IJsonRepository<Warehouse> _warehouses;
    private readonly IJsonRepository<Product> _products;

    public CatalogService(
        IJsonRepository<Category> categories,
        IJsonRepository<Warehouse> warehouses,
        IJsonRepository<Product> products)
    {
        _categories = categories;
        _warehouses = warehouses;
        _products = products;
    }

    public Task<List<Category>> GetCategoriesAsync() => _categories.GetAllAsync();

    public Task<Category?> GetCategoryAsync(Guid id) => _categories.GetByIdAsync(id);

    public Task<Category> CreateCategoryAsync(CreateCategoryRequest request)
    {
        return _categories.AddAsync(new Category
        {
            Name = request.Name,
            Description = request.Description
        });
    }

    public Task<Category?> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
    {
        return _categories.UpdateAsync(id, new Category
        {
            Name = request.Name,
            Description = request.Description
        });
    }

    public Task<bool> DeleteCategoryAsync(Guid id) => _categories.DeleteAsync(id);

    public Task<List<Warehouse>> GetWarehousesAsync() => _warehouses.GetAllAsync();

    public Task<Warehouse?> GetWarehouseAsync(Guid id) => _warehouses.GetByIdAsync(id);

    public Task<Warehouse> CreateWarehouseAsync(CreateWarehouseRequest request)
    {
        return _warehouses.AddAsync(new Warehouse
        {
            Name = request.Name,
            Location = request.Location
        });
    }

    public Task<Warehouse?> UpdateWarehouseAsync(Guid id, UpdateWarehouseRequest request)
    {
        return _warehouses.UpdateAsync(id, new Warehouse
        {
            Name = request.Name,
            Location = request.Location
        });
    }

    public Task<bool> DeleteWarehouseAsync(Guid id) => _warehouses.DeleteAsync(id);

    public Task<List<Product>> GetProductsAsync() => _products.GetAllAsync();

    public Task<Product?> GetProductAsync(Guid id) => _products.GetByIdAsync(id);

    public async Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        var category = await _categories.GetByIdAsync(request.CategoryId);

        if (category is null)
        {
            throw new InvalidOperationException("Category not found.");
        }

        return await _products.AddAsync(new Product
        {
            Name = request.Name,
            Sku = request.Sku,
            Description = request.Description,
            CategoryId = request.CategoryId,
            UnitPrice = request.UnitPrice,
            CostPrice = request.CostPrice,
            QuantityOnHand = request.QuantityOnHand,
            LowStockThreshold = request.LowStockThreshold,
            IsActive = request.IsActive
        });
    }

    public async Task<Product?> UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        var category = await _categories.GetByIdAsync(request.CategoryId);

        if (category is null)
        {
            throw new InvalidOperationException("Category not found.");
        }

        return await _products.UpdateAsync(id, new Product
        {
            Name = request.Name,
            Sku = request.Sku,
            Description = request.Description,
            CategoryId = request.CategoryId,
            UnitPrice = request.UnitPrice,
            CostPrice = request.CostPrice,
            QuantityOnHand = request.QuantityOnHand,
            LowStockThreshold = request.LowStockThreshold,
            IsActive = request.IsActive
        });
    }

    public Task<bool> DeleteProductAsync(Guid id) => _products.DeleteAsync(id);
}
