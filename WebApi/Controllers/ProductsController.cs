using Microsoft.AspNetCore.Mvc;
using WebApi.CachService;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTop(CancellationToken ct)
        => Ok(await _service.GetTopProductsAsync(ct));

    [HttpPost("top/invalidate")]
    public IActionResult Invalidate()
    {
        _service.InvalidateTopProducts();
        return NoContent();
    }
}