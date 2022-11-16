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

    [HttpPost(Name = "CreateProduct")]
    public async Task<ProductDto> Create(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            await _productService.Create(productDto);
        }
        return productDto;
    }
}