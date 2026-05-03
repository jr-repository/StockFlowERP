using Microsoft.AspNetCore.Mvc;
using StockFlowERP.Services;

namespace StockFlowERP.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly SalesService _service;

    public InvoicesController(SalesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetInvoicesAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var invoice = await _service.GetInvoiceAsync(id);
        return invoice is null ? NotFound() : Ok(invoice);
    }
}
