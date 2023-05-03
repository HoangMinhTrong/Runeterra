using Microsoft.AspNetCore.Mvc;
using Product.API.Dtos;
using Product.API.Services.Base;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet(Name = "GetAllProduct")]
    public async Task<List<Entity.Product>> Get()
    {
        var products = await _productService.Get();
        return products;
    }
    [HttpGet("get-by-store")]
    public async Task<List<Entity.Product>> GetByStore()
    {
        var products = await _productService.GetByStore();
        return products;
    }
    [HttpGet("{id}")]
    public async Task<Entity.Product> GetById(int id)
    {
        var product = await _productService.GetById(id);
        return product;
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        return Ok(id);
    }
    
    [HttpPost(Name = "CreateProduct")]
    public async Task<IActionResult> Create(CreateProductRequest productDto)
    {
        if (ModelState.IsValid)
        {
            await _productService.Create(productDto);
        }
        return Ok();
    }
}