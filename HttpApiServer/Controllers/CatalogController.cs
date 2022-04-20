
using HttpModels;
using Microsoft.AspNetCore.Mvc;

namespace HttpApiServer;

[Route("catalog")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly CatalogService _service;

    public CatalogController(CatalogService service)
    {
        _service = service;
    }
    [LogTimeExecutionRequestFilter]
    [HttpGet("get_products")]
    public async Task<IReadOnlyList<Product>> GetProducts()
    {
        return await _service.GetProducts(HttpContext.Request.Headers.UserAgent.ToString());
    }

    [HttpGet("get_product_by_id")]
    public async Task<ActionResult<Product>> GetProductById(Guid id)
    {
        return await _service.GetProductById(HttpContext.Request.Headers.UserAgent.ToString(), id);
    }

    [HttpGet("get_categories")]
    public async Task<IReadOnlyList<Category>> GetCategories()
    {
        return await _service.GetCategories();
    }

    [HttpPost("add_product")]
    public async Task AddProduct(Product product)
    {
        await _service.AddProduct(product);
    }
}