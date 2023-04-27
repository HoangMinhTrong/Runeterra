using System.Net.Http.Headers;
using System.Security.Claims;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product.API.Data;
using Product.API.Dtos;
using Product.API.Entity;
using Product.API.Services.Base;

namespace Product.API.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ProductService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;

    }
    public async Task<List<Entity.Product>> Get()
    {
        var products = await _context.Products
            .Include(x => x.ProductType)
            .Include(x => x.Store)
            .ToListAsync();
        return products;
    }

    public async Task<bool> Create(CreateProductRequest productDto)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var store = await _context.Stores.Where(x => x.UserId == userIdClaim).FirstOrDefaultAsync();
        if (productDto != null)
        {
            var product = new Entity.Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Status = productDto.Status,
                ImageUrl = productDto.ImageUrl,
                Quantity = productDto.Quantity,
                ProductTypeId = productDto.ProductTypeId,
                StoreId = store.Id
            };
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        return true;
    }

    public async Task<Entity.Product> GetById(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        return product;
    }
}