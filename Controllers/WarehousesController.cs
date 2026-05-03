using Microsoft.AspNetCore.Mvc;
using StockFlowERP.Contracts;
using StockFlowERP.Services;

namespace StockFlowERP.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly CatalogService _service;

    public WarehousesController(CatalogService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetWarehousesAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var warehouse = await _service.GetWarehouseAsync(id);
        return warehouse is null ? NotFound() : Ok(warehouse);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWarehouseRequest request)
    {
        var warehouse = await _service.CreateWarehouseAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = warehouse.Id }, warehouse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateWarehouseRequest request)
    {
        var warehouse = await _service.UpdateWarehouseAsync(id, request);
        return warehouse is null ? NotFound() : Ok(warehouse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteWarehouseAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
